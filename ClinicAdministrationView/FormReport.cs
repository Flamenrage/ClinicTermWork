using ClinicBusinessLogic.BindingModels;
using ClinicBusinessLogic.Interfaces;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClinicAdministrationView
{
    public partial class FormReport : Form
    {
        private readonly IReportLogic logicRep;

        private readonly IMainLogic logic;
        public FormReport(IReportLogic logicRep, IMainLogic logic)
        {
            InitializeComponent();
            this.logicRep = logicRep;
            this.logic = logic;
        }

        private void buttonForm_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.Value.Date >= dateTimePickerTo.Value.Date)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var list = logicRep.GetReport(new ReportBindingModel
            {
                FileName = "",
                DateFrom = dateTimePickerFrom.Value,
                DateTo = dateTimePickerTo.Value
            });

            ReportParameter parameter = new ReportParameter("ReportParameterPeriod",
                "c " + dateTimePickerFrom.Value.ToShortDateString() +
                " по " + dateTimePickerTo.Value.ToShortDateString());
            reportViewer.LocalReport.SetParameters(parameter);
            reportViewer.LocalReport.DataSources.Clear();
            ReportDataSource source = new ReportDataSource("DataSet", list);
            reportViewer.LocalReport.DataSources.Add(source);
            reportViewer.RefreshReport();
        }

        private void buttonPdf_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.Value.Date >= dateTimePickerTo.Value.Date)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                string path = @"C:\temp\adminReport.pdf";
                logicRep.SaveReport(new ReportBindingModel
                {
                    FileName = path,
                    DateFrom = dateTimePickerFrom.Value,
                    DateTo = dateTimePickerTo.Value
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }

        private void FormReport_Load(object sender, EventArgs e)
        {

        }
    }
}
