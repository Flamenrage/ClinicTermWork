using ClinicBusinessLogic.Interfaces;
using ClinicBusinessLogic.ViewModels;
using ClinicImplementation.Implementations;
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
    public partial class FormMain : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IMainLogic logicT;

        private readonly IMedicationLogic logicM;

        public FormMain(IMainLogic logicT, IMedicationLogic logicM)
        {
            InitializeComponent();
            this.logicT = logicT;
            this.logicM = logicM;
        }

        private void LoadData()
        {
            try
            {
                List<TreatmentViewModel> listT = logicT.GetList();
                if (listT != null)
                {
                    dataGridViewT.DataSource = listT;
                    dataGridViewT.Columns[0].Visible = false;
                    dataGridViewT.Columns[1].Visible = false;
                    dataGridViewT.Columns[2].Visible = false;
                    dataGridViewT.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridViewT.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                List<MedicationViewModel> listM = logicM.GetList();
                if (listM != null)
                {
                    dataGridViewM.DataSource = listM;
                    dataGridViewM.Columns[0].Visible = false;
                    dataGridViewM.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridViewT.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridViewT.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void рецептыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormPrescriptions>();
            form.ShowDialog();
        }

        private void лекарстваToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormMedications>();
            form.ShowDialog();
        }

        private void отчетToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormReport>();
            form.ShowDialog();
        }

        private void buttonCreateRequest_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormRequest>();
            form.ShowDialog();
        }

        private void FormMainAdmin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void SaveToXmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                BackUpLogic.AdminBackUpXML();
                MessageBox.Show("Успешно сохранено в XML", "Готово", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при сохранении в XML", "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void SaveToJsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                BackUpLogic.AdminBackUpJSON();
                MessageBox.Show("Успешно сохранено в JSON", "Готово", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при сохранении в JSON", "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void диаграммаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormDiagram>();
            form.ShowDialog();
        }
    }
}
