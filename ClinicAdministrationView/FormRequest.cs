using ClinicBusinessLogic.BindingModels;
using ClinicBusinessLogic.BusinessLogic;
using ClinicBusinessLogic.Interfaces;
using ClinicBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;

namespace ClinicAdministrationView
{
    public partial class FormRequest : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IMainLogic logic;

        private readonly IRequestLogic logicR;

        private readonly IMedicationLogic logicM;

        private readonly IReportLogic logicRep;

        public FormRequest(IMainLogic logic, IRequestLogic logicR, IMedicationLogic logicM, IReportLogic logicRep)
        {
            InitializeComponent();
            this.logic = logic;
            this.logicR = logicR;
            this.logicM = logicM;
            this.logicRep = logicRep;
        }

        private void FormRequest_Load(object sender, EventArgs e)
        {
            try
            {
                List<MedicationViewModel> listM = logicM.GetMostList();
                if (listM != null)
                {
                    comboBoxMedication.DisplayMember = "Name";
                    comboBoxMedication.ValueMember = "Id";
                    comboBoxMedication.DataSource = listM;
                    comboBoxMedication.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(textBoxRequest.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (comboBoxMedication.SelectedValue == null)
            {
                MessageBox.Show("Выберите лекарство", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                DateTime date = DateTime.Now;
                string path = @"C:\temp\request.xls";
                int Requestid = logicR.AddElement(new RequestBindingModel
                {
                    Name = textBoxRequest.Text,
                    Date = DateTime.Now
                });
                logic.MedicationRefill(new RequestMedicationBindingModel
                {
                    MedicationId = Convert.ToInt32(comboBoxMedication.SelectedValue),
                    MedicationName = comboBoxMedication.Text,
                    RequestId = Requestid,
                    Count = Convert.ToInt32(textBoxCount.Text)
                });
                logicRep.SaveToExcel(new ReportBindingModel
                {
                    FileName = path,
                    DateFrom = date,
                    DateTo = date.AddMilliseconds(100)
                }, -1);
                MailLogic.SendMail(new ClinicBusinessLogic.HelperModels.MailSendInfo
                {
                    Email = ConfigurationManager.AppSettings["AdminEmail"],
                    Subject = "Оповещение по заявке",
                    Body = "Заявка выполнена",
                    AttachmentPath = path
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
