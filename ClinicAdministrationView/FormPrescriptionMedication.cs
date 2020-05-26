using ClinicBusinessLogic.Interfaces;
using ClinicBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;

namespace ClinicAdministrationView
{
    public partial class FormPrescriptionMedication : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public PrescriptionMedicationViewModel Model { get; set; }
        
        public int TotalPrice { get; set; }
        

        private readonly IMedicationLogic logic;


        private List<MedicationViewModel> list;

        public FormPrescriptionMedication(IMedicationLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void FormPrescriptionMedication_Load(object sender, EventArgs e)
        {
            try
            {
                list = logic.GetList();
                if (list != null)
                {
                    comboBoxMedication.DisplayMember = "Name";
                    comboBoxMedication.ValueMember = "Id";
                    comboBoxMedication.DataSource = list;
                    comboBoxMedication.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
            if (Model != null)
            {
                comboBoxMedication.Enabled = false;
                comboBoxMedication.SelectedValue = Model.MedicationId;
                textBoxCount.Text = Model.Count.ToString();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxMedication.SelectedValue == null)
            {
                MessageBox.Show("Выберите лекарство", "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (Model == null)
                {
                    Model = new PrescriptionMedicationViewModel
                    {
                        MedicationId = Convert.ToInt32(comboBoxMedication.SelectedValue),
                        MedicationName = comboBoxMedication.Text,
                        Count = Convert.ToInt32(textBoxCount.Text)
                    };
                    foreach (var Medication in list)
                    {
                        if (Medication.Id == Model.MedicationId)
                        {
                            TotalPrice = Medication.Price * Model.Count;
                        }
                    }
                }
                else
                {
                    Model.Count = Convert.ToInt32(textBoxCount.Text);
                }
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
