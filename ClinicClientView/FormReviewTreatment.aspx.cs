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
    public partial class FormReviewTreatment : Page
    {
        private readonly IMainLogic service = new MainLogic();

        private List<TreatmentPrescriptionViewModel> TreatmentPrescriptions;

        private int id;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Int32.TryParse((string)Session["id"], out id))
            {
                try
                {
                    TreatmentViewModel view = service.GetTreatment(id);
                    if (view != null)
                    {
                        if (!Page.IsPostBack)
                        {
                            textBoxName.Text = view.Name;
                            textBoxPrice.Text = view.TotalPrice.ToString();
                        }
                        TreatmentPrescriptions = view.TreatmentPrescriptions;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        private void LoadData()
        {
            try
            {
                if (TreatmentPrescriptions != null)
                {
                    dataGridView.DataBind();
                    dataGridView.DataSource = TreatmentPrescriptions;
                    dataGridView.DataBind();
                    dataGridView.ShowHeaderWhenEmpty = true;
                    dataGridView.SelectedRowStyle.BackColor = Color.Silver;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormMain.aspx");
        }
    }
}