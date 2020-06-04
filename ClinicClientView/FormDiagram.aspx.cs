using ClinicBusinessLogic.BusinessLogic;
using ClinicBusinessLogic.HelperModels;
using ClinicBusinessLogic.Interfaces;
using ClinicImplementation.Implementations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace ClinicClientView
{
    public partial class FormDiagram : System.Web.UI.Page
    {
        private readonly IMedicationLogic logic;
        private const string path = @"C:\temp\Diagram.jpg";

        public FormDiagram()
        {
            logic = new MedicationLogic();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadDiagram();
        }
        private void LoadDiagram()
        {
            var list = logic.GetMostList(true);
            ChartDiagram.BackColor = Color.Gray;
            ChartDiagram.BackSecondaryColor = Color.WhiteSmoke;
            ChartDiagram.BackGradientStyle = GradientStyle.DiagonalRight;

            ChartDiagram.BorderlineDashStyle = ChartDashStyle.Solid;
            ChartDiagram.BorderlineColor = Color.Gray;
            ChartDiagram.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;

            // Форматировать область диаграммы
            ChartDiagram.ChartAreas[0].BackColor = Color.Wheat;

            // Добавить и форматировать заголовок
            ChartDiagram.Titles.Add("Актуальность лекарств"); 
            ChartDiagram.Titles[0].Font = new Font("Utopia", 16);
            ChartDiagram.Series.Add(new Series("SplineSeries")
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

            ChartDiagram.DataSource = dt;
            ChartDiagram.Series["Series1"].XValueMember = "X";
            ChartDiagram.Series["Series1"].YValueMembers = "Y";
            ChartDiagram.DataBind();
            if (File.Exists(path)) 
            {
                File.Delete(path);
            }
            ChartDiagram.SaveImage(path);
        }
        protected void ButtonSendMail_Click(object sender, EventArgs e)
        {
            try
            {
                _ = MailLogic.SendMail(new MailSendInfo
                {
                    Email = Session["PatientEmail"].ToString(),
                    Subject = "Отчет об актуальности",
                    Body = " ",
                    AttachmentPath = path
                });
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Успешно отправлено');</script>");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }
        protected void ButtonReturn_Click (object sender, EventArgs e)
        {
            Response.Redirect("FormMain.aspx");
        }
    }
}