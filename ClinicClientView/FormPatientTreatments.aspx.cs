using ClinicBusinessLogic.BindingModels;
using ClinicBusinessLogic.BusinessLogic;
using ClinicBusinessLogic.HelperModels;
using ClinicBusinessLogic.Interfaces;
using ClinicBusinessLogic.ViewModels;
using ClinicImplementation.Implementations;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ClinicClientView
{
    public partial class FormPatientTreatments : System.Web.UI.Page
    {
        private readonly IReportLogic logicR = new ReportLogic();

        private readonly IMainLogic logic = new MainLogic();

        protected void Page_Load(object sender, EventArgs e) { }

        protected void ButtonMake_Click(object sender, EventArgs e)
        {
            if (Calendar1.SelectedDate >= Calendar2.SelectedDate)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "ScriptAlertDate", "<script>alert('Дата начала должна быть меньше даты окончания');</script>");
                return;
            }
            try
            {                
                string path = @"C:\Program Files (x86)\IIS Express\Treatments.pdf";
                logicR.SaveTreatments(new ReportBindingModel
                {
                    FileName = path,
                    DateFrom = Calendar1.SelectedDate,
                    DateTo = Calendar2.SelectedDate.AddDays(1)
                }, Convert.ToInt32(Session["PatientId"]));
                MailLogic.SendMail(new MailSendInfo
                {
                    Email = Session["PatientEmail"].ToString(),
                    Subject = "Лечения пациента",
                    Body = " ",
                    AttachmentPath = path
                });
                var list = logicR.GetTreatments(new ReportBindingModel
                {
                    FileName = path,
                    DateFrom = Calendar1.SelectedDate,
                    DateTo = Calendar2.SelectedDate.AddDays(1)

                }, Convert.ToInt32(Session["PatientId"]))
                .Select(rec => new
                {
                    FIO = rec.FIO,
                    MedicationName = rec.MedicationName,
                    Count = rec.Count,
                    Name = rec.Name,
                    Date = (rec.Date == DateTime.MinValue) ? " " : rec.Date.ToString("dd/MM/yyyy HH:mm")
                });
                if (list != null)
                {
                    dataGridView.DataBind();
                    dataGridView.DataSource = list;
                    dataGridView.DataBind();
                    dataGridView.ShowHeaderWhenEmpty = true;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "ScriptAlert", "<script>alert('" + ex.Message + "');</script>");
            }
        }
        
        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormMain.aspx");
        }
    }
}