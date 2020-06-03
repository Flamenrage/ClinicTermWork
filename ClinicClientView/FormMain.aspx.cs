using ClinicBusinessLogic.BindingModels;
using ClinicBusinessLogic.BusinessLogic;
using ClinicBusinessLogic.HelperModels;
using ClinicBusinessLogic.Interfaces;
using ClinicBusinessLogic.ViewModels;
using ClinicImplementation.Implementations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClinicClientView
{
    public partial class FormMain : System.Web.UI.Page
    {

        private readonly IMainLogic logic = new MainLogic();

        private readonly IReportLogic logicR = new ReportLogic();

        List<TreatmentViewModel> list;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                list = logic.GetPatientList(Convert.ToInt32(Session["PatientId"]));
                dataGridView.DataSource = list;
                dataGridView.DataBind();
                dataGridView.ShowHeaderWhenEmpty = true;
                dataGridView.SelectedRowStyle.BackColor = Color.Silver;
                dataGridView.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonCreateTreatment_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormCreateTreatment.aspx");
        }

        protected void ButtonReviewTreatment_Click(object sender, EventArgs e)
        {
            try
            {
                string index = list[dataGridView.SelectedIndex].Id.ToString();
                Session["id"] = index;
                Response.Redirect("FormReviewTreatment.aspx");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonDeleteTreatment_Click(object sender, EventArgs e)
        {
            try
            {
                logic.DelTreatment(list[dataGridView.SelectedIndex].Id);
                LoadData();
                Response.Redirect("FormMain.aspx");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }
        protected void ButtonReserve_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime date = logic.TreatmentReservation(list[dataGridView.SelectedIndex].Id);
                string name;
                string path = null;
                if (!string.IsNullOrEmpty(textBoxReport.Text)) 
                {
                    name = textBoxReport.Text;
                    if (name == "xls")
                    {
                        path = @"C:\temp\PatientTreatment.xls";
                        logicR.SaveToExcel(new ReportBindingModel
                        {
                            FileName = path,
                            DateFrom = date,
                            DateTo = date.AddMilliseconds(100)
                        }, Convert.ToInt32(Session["PatientId"]));
                    }
                    else if (name == "doc")
                    {
                        path = @"C:\temp\PatientTreatment.doc";
                        logicR.SaveToWord(new ReportBindingModel
                        {
                            FileName = path,
                            DateFrom = date,
                            DateTo = date.AddMilliseconds(100)
                        }, Convert.ToInt32(Session["PatientId"]));
                    }
                }
                _ = MailLogic.SendMail(new MailSendInfo
                {
                    Email = Session["PatientEmail"].ToString(),
                    Subject = "Лечение зарезервировано",
                    Body = " ",
                    AttachmentPath = path
                });
                LoadData();
                Response.Redirect("FormMain.aspx");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }
        protected void ButtonXML_Click(object sender, EventArgs e)
        {
            try
            {
                BackUpLogic.PatientBackUpXML(Convert.ToInt32(Session["PatientId"]));
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Успешно сохранено');</script>");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }

        }
        protected void ButtonJSON_Click(object sender, EventArgs e) 
        {
            try
            {
                BackUpLogic.PatientBackUpJSON(Convert.ToInt32(Session["PatientId"]));
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Успешно сохранено');</script>");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }

        }
        protected void ButtonRef_Click(object sender, EventArgs e)
        {
            LoadData();
            Response.Redirect("FormMain.aspx");
        }
    }
}