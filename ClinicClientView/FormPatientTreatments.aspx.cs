using ClinicBusinessLogic.BindingModels;
using ClinicBusinessLogic.Interfaces;
using ClinicImplementation.Implementations;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            reportObject.Visible = false;
        }

        protected void ButtonMake_Click(object sender, EventArgs e)
        {
            if (Calendar1.SelectedDate >= Calendar2.SelectedDate)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "ScriptAlertDate", "<script>alert('Дата начала должна быть меньше даты окончания');</script>");
                return;
            }
            try
            {                
                string path = "Treatments.pdf";
                logicR.SaveTreatments(new ReportBindingModel
                {
                    FileName = path,
                    DateFrom = Calendar1.SelectedDate,
                    DateTo = Calendar2.SelectedDate
                }, Convert.ToInt32(Session["PatientId"]));
                Page.ClientScript.RegisterStartupScript(GetType(), "ScriptUpdate",  @"<script language = ""javascript"" type = ""text/javascript"" > getElementById('reportObject').contentDocument.location.reload() </ script >");
                reportObject.Visible = true;
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