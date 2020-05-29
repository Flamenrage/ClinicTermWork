using ClinicBusinessLogic.Interfaces;
using ClinicBusinessLogic.ViewModels;
using ClinicImplementation.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClinicClientView
{
    public partial class FormTreatmentPrescription : System.Web.UI.Page
    {
        private readonly IPrescriptionLogic logicS = new PrescriptionLogic();

        private readonly IMainLogic serviceM = new MainLogic();

        private TreatmentPrescriptionViewModel model;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    List<PrescriptionViewModel> listP = logicS.GetPatientList(Convert.ToInt32(Session["PatientId"]));
                    if (listP != null)
                    {
                        DropDownListPrescription.DataSource = listP;
                        DropDownListPrescription.DataBind();
                        DropDownListPrescription.DataTextField = "Name";
                        DropDownListPrescription.DataValueField = "Id";
                    }
                    Page.DataBind();

                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxCount.Text))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните поле Количество');</script>");
                return;
            }
            if (DropDownListPrescription.SelectedValue == null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Выберите лекарство');</script>");
                return;
            }
            try
            {
                if (Session["SEId"] == null)
                {
                    model = new TreatmentPrescriptionViewModel
                    {
                        PrescriptionId = Convert.ToInt32(DropDownListPrescription.SelectedValue),
                        PrescriptionName = DropDownListPrescription.SelectedItem.Text,
                        Count = Convert.ToInt32(TextBoxCount.Text)
                    };
                    Session["SEId"] = model.Id;
                    Session["SETreatmentId"] = model.TreatmentId;
                    Session["SEPrescriptionId"] = model.PrescriptionId;
                    Session["SEPrescriptionName"] = model.PrescriptionName;
                    Session["SECount"] = model.Count;
                }
                else
                {
                    model.Count = Convert.ToInt32(TextBoxCount.Text);
                    Session["SEId"] = model.Id;
                    Session["SETreatmentId"] = model.TreatmentId;
                    Session["SEPrescriptionId"] = model.PrescriptionId;
                    Session["SEPrescriptionName"] = model.PrescriptionName;
                    Session["SECount"] = model.Count;
                    Session["Change"] = "1";
                }
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Сохранение прошло успешно');</script>");
                Response.Redirect("FormCreateTreatment.aspx");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormCreateTreatment.aspx");
        }

        protected void TextBoxCount_TextChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void CalcSum()
        {
            if (DropDownListPrescription.SelectedValue != null && !string.IsNullOrEmpty(TextBoxCount.Text))
            {
                try
                {
                    int id = Convert.ToInt32(DropDownListPrescription.SelectedValue);
                    PrescriptionViewModel prescription = logicS.GetElement(id);
                    int count = Convert.ToInt32(TextBoxCount.Text);
                    TextBoxSum.Text = (count * prescription.TotalPrice).ToString();
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                }
            }
        }
    }
}