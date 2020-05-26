using ClinicBusinessLogic.BindingModels;
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
    public partial class FormPrescription : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IPrescriptionLogic logic;

        private int? id;

        private List<PrescriptionMedicationViewModel> PrescriptionMedications;

        public FormPrescription(IPrescriptionLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void FormPrescription_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    PrescriptionViewModel view = logic.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.Name;
                        textBoxPrice.Text = view.TotalPrice.ToString();
                        PrescriptionMedications = view.PrescriptionMedications;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }
            }
            else
            {
                textBoxPrice.Text = "0";
                PrescriptionMedications = new List<PrescriptionMedicationViewModel>();
            }
        }

        private void LoadData()
        {
            try
            {
                if (PrescriptionMedications != null)
                {
                    dataGridView.DataSource = null;
                    dataGridView.DataSource = PrescriptionMedications;
                    dataGridView.Columns[0].Visible = false;
                    dataGridView.Columns[1].Visible = false;
                    dataGridView.Columns[2].Visible = false;
                    dataGridView.Columns[3].AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormPrescriptionMedication>();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                    {
                        form.Model.PrescriptionId = id.Value;
                    }
                    PrescriptionMedications.Add(form.Model);
                    textBoxPrice.Text = (Int32.Parse(textBoxPrice.Text) + form.TotalPrice).ToString();
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                var form = Container.Resolve<FormPrescriptionMedication>();
                form.Model =
                PrescriptionMedications[dataGridView.SelectedRows[0].Cells[0].RowIndex];
                if (form.ShowDialog() == DialogResult.OK)
                {
                    PrescriptionMedications[dataGridView.SelectedRows[0].Cells[0].RowIndex] =
                    form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        PrescriptionMedications.RemoveAt(dataGridView.SelectedRows[0].Cells[0].RowIndex);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }
            if (PrescriptionMedications == null || PrescriptionMedications.Count == 0)
            {
                MessageBox.Show("Заполните лекарства", "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }
            try
            {
                List<PrescriptionMedicationBindingModel> PrescriptionMedicationBM = new
                List<PrescriptionMedicationBindingModel>();
                for (int i = 0; i < PrescriptionMedications.Count; ++i)
                {
                    PrescriptionMedicationBM.Add(new PrescriptionMedicationBindingModel
                    {
                        Id = PrescriptionMedications[i].Id,
                        PrescriptionId = PrescriptionMedications[i].PrescriptionId,
                        MedicationId = PrescriptionMedications[i].MedicationId,
                        MedicationName = PrescriptionMedications[i].MedicationName,
                        Count = PrescriptionMedications[i].Count
                    });
                }
                if (id.HasValue)
                {
                    logic.UpdElement(new PrescriptionBindingModel
                    {
                        Id = id.Value,
                        Name = textBoxName.Text,
                        TotalPrice = Int32.Parse(textBoxPrice.Text),
                        PrescriptionMedications = PrescriptionMedicationBM
                    });
                }
                else
                {
                    logic.AddElement(new PrescriptionBindingModel
                    {
                        Name = textBoxName.Text,
                        TotalPrice = Int32.Parse(textBoxPrice.Text),
                        PrescriptionMedications = PrescriptionMedicationBM
                    });
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
