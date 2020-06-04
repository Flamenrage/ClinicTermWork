using ClinicBusinessLogic.BusinessLogic;
using ClinicBusinessLogic.HelperModels;
using ClinicBusinessLogic.Interfaces;
using ClinicImplementation.Implementations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ClinicAdministrationView
{
    public partial class FormDiagram : Form
    {
        private readonly IMedicationLogic logic;
        private const string path = @"C:\temp\Diagram.jpeg";        
        public FormDiagram()
        {
            InitializeComponent();
            logic = new MedicationLogic();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            try
            {
                _ = MailLogic.SendMail(new MailSendInfo
                {
                    Email = ConfigurationManager.AppSettings["AdminEmail"],
                    Subject = "Отчет об актуальности",
                    Body = " ",
                    AttachmentPath = path
                });
                MessageBox.Show("Успешно отправлено", "Готово", MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormDiagram_Load(object sender, EventArgs e)
        {
            var list = logic.GetMostList(true);
            chart.BackColor = Color.Gray;
            chart.BackSecondaryColor = Color.WhiteSmoke;
            chart.BackGradientStyle = GradientStyle.DiagonalRight;
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BorderlineColor = Color.Gray;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;

            // Форматировать область диаграммы
            chart.ChartAreas[0].BackColor = Color.Wheat;
            // Добавить и форматировать заголовок
            chart.Titles.Add("Актуальность лекарств"); 
            chart.Titles[0].Font = new Font("Utopia", 16);
            chart.Series.Add(new Series("SplineSeries")
            {
                ChartType = SeriesChartType.Column,
                BorderWidth = 3,
                ShadowOffset = 2,
                Color = Color.PaleVioletRed
            });
            DataTable dt = new DataTable();
            dt.Columns.Add("X");
            dt.Columns.Add("Y");
            foreach (var el in list)
            {
                dt.Rows.Add(el.Name, el.Count);
            }
            chart.DataSource = dt;
            chart.Series["Series1"].XValueMember = "X";
            chart.Series["Series1"].YValueMembers = "Y";
            chart.DataBind();
            if (File.Exists(path)) 
            {
                File.Delete(path);
            }
            chart.SaveImage(path, ChartImageFormat.Jpeg);
        }
    }
}
