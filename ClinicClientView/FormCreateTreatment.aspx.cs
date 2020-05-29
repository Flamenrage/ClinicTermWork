using ClinicBusinessLogic.BindingModels;
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
    public partial class FormCreateTreatment : System.Web.UI.Page
    {
        private readonly IMainLogic logic = new MainLogic();

        private readonly IPrescriptionLogic logicP = new PrescriptionLogic();

        private int id;

        private List<TreatmentPrescriptionViewModel> TreatmentPrescriptions;

        private TreatmentPrescriptionViewModel model;

        private int price;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (int.TryParse((string)Session["id"], out id))
            {
                try
                {
                    TreatmentViewModel view = logic.GetTreatment(id);
                    if (view != null)
                    {
                        if (!Page.IsPostBack)
                        {
                            textBoxName.Text = view.Name;
                            textBoxPrice.Text = view.TotalPrice.ToString();
                        }
                        this.TreatmentPrescriptions = view.TreatmentPrescriptions;
                        LoadData();
                    }

                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                }
            }
            else
            {
                this.TreatmentPrescriptions = new List<TreatmentPrescriptionViewModel>();

            }
            if (Session["SEId"] != null)
            {
                if ((Session["SEIs"] != null) && (Session["Change"].ToString() != "0"))
                {
                    model = new TreatmentPrescriptionViewModel
                    {
                        Id = (int)Session["SEId"],
                        TreatmentId = (int)Session["SETreatmentId"],
                        PrescriptionId = (int)Session["SEPrescriptionId"],
                        PrescriptionName = (string)Session["SEPrescriptionName"],
                        Count = (int)Session["SECount"]
                    };
                    this.TreatmentPrescriptions[(int)Session["SEIs"]] = model;
                    Session["Change"] = "0";
                }
                else
                {
                    model = new TreatmentPrescriptionViewModel
                    {
                        TreatmentId = (int)Session["SETreatmentId"],
                        PrescriptionId = (int)Session["SEPrescriptionId"],
                        PrescriptionName = (string)Session["SEPrescriptionName"],
                        Count = (int)Session["SECount"]
                    };
                    this.TreatmentPrescriptions.Add(model);
                }
                Session["SEId"] = null;
                Session["SETreatmentId"] = null;
                Session["SEPrescriptionId"] = null;
                Session["SEPrescriptionName"] = null;
                Session["SEIsReserved"] = null;
                Session["SECount"] = null;
                Session["SEIs"] = null;
            }
            List<TreatmentPrescriptionBindingModel> TreatmentPrescriptionBM = new List<TreatmentPrescriptionBindingModel>();
            for (int i = 0; i < TreatmentPrescriptions.Count; ++i)
            {
                TreatmentPrescriptionBM.Add(new TreatmentPrescriptionBindingModel
                {
                    Id = this.TreatmentPrescriptions[i].Id,
                    TreatmentId = this.TreatmentPrescriptions[i].TreatmentId,
                    PrescriptionId = this.TreatmentPrescriptions[i].PrescriptionId,
                    PrescriptionName = this.TreatmentPrescriptions[i].PrescriptionName,
                    Count = this.TreatmentPrescriptions[i].Count
                });
            }
            if (TreatmentPrescriptionBM.Count != 0)
            {
                CalcSum();
                string name = "Введите название";
                if (textBoxName.Text.Length != 0)
                {
                    name = textBoxName.Text;
                }
                if (int.TryParse((string)Session["id"], out id))
                {
                    logic.UpdTreatment(new TreatmentBindingModel
                    {
                        Id = id,
                        PatientId = Int32.Parse(Session["PatientId"].ToString()),
                        Name = name,
                        TotalPrice = price,
                        IsReserved = false,
                        TreatmentPrescriptions = TreatmentPrescriptionBM
                    });
                }
                else
                {
                    logic.CreateTreatment(new TreatmentBindingModel
                    {
                        PatientId = Int32.Parse(Session["PatientId"].ToString()),
                        Name = name,
                        TotalPrice = price,
                        IsReserved = false,
                        TreatmentPrescriptions = TreatmentPrescriptionBM
                    });
                    Session["id"] = logic.GetList().Last().Id.ToString();
                    Session["Change"] = "0";
                }
            }
            LoadData();
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
                    dataGridView.Columns[1].Visible = false;
                    dataGridView.Columns[2].Visible = false;
                    dataGridView.Columns[3].Visible = false;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormTreatmentPrescription.aspx");
        }

        protected void ButtonChange_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedIndex >= 0)
            {
                model = logic.GetTreatment(id).TreatmentPrescriptions[dataGridView.SelectedIndex];
                Session["SEId"] = model.Id;
                Session["SETreatmentId"] = model.TreatmentId;
                Session["SEPrescriptionId"] = model.PrescriptionId;
                Session["SEPrescriptionName"] = model.PrescriptionName;
                Session["SEIsReserved"] = logic.GetTreatment(id).IsReserved;
                Session["SECount"] = model.Count;
                Session["SEIs"] = dataGridView.SelectedIndex;
                Session["Change"] = "0";
                Response.Redirect("FormTreatmentPrescription.aspx");
            }
        }

        protected void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedIndex >= 0)
            {
                try
                {
                    TreatmentPrescriptions.RemoveAt(dataGridView.SelectedIndex);
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                }
                LoadData();
            }
        }

        protected void ButtonUpd_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните название');</script>");
                return;
            }
            if (TreatmentPrescriptions == null || TreatmentPrescriptions.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Добавьте рецепты');</script>");
                return;
            }
            try
            {
                List<TreatmentPrescriptionBindingModel> TreatmentPrescriptionBM = new List<TreatmentPrescriptionBindingModel>();
                for (int i = 0; i < TreatmentPrescriptions.Count; ++i)
                {
                    TreatmentPrescriptionBM.Add(new TreatmentPrescriptionBindingModel
                    {
                        Id = TreatmentPrescriptions[i].Id,
                        TreatmentId = TreatmentPrescriptions[i].TreatmentId,
                        PrescriptionId = TreatmentPrescriptions[i].PrescriptionId,
                        PrescriptionName = TreatmentPrescriptions[i].PrescriptionName,
                        Count = TreatmentPrescriptions[i].Count
                    });
                }
                if (Int32.TryParse((string)Session["id"], out id))
                {
                    logic.UpdTreatment(new TreatmentBindingModel
                    {
                        Id = id,
                        PatientId = Int32.Parse(Session["PatientId"].ToString()),
                        Name = textBoxName.Text,
                        TotalPrice = Convert.ToInt32(textBoxPrice.Text),
                        IsReserved = false,
                        TreatmentPrescriptions = TreatmentPrescriptionBM
                    });
                }
                else
                {
                    logic.CreateTreatment(new TreatmentBindingModel
                    {
                        PatientId = Int32.Parse(Session["PatientId"].ToString()),
                        Name = textBoxName.Text,
                        TotalPrice = Convert.ToInt32(textBoxPrice.Text),
                        IsReserved = false,
                        TreatmentPrescriptions = TreatmentPrescriptionBM
                    });
                }
                Session["id"] = null;
                Session["Change"] = null;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Сохранение прошло успешно');</script>");
                Response.Redirect("FormMain.aspx");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {

            if (logic.GetList().Count != 0 && logic.GetList().Last().Name == null)
            {
                logic.DelTreatment(logic.GetList().Last().Id);
            }
            if (!String.Equals(Session["Change"], null))
            {
                logic.DelTreatment(id);
            }
            Session["id"] = null;
            Session["Change"] = null;
            Response.Redirect("FormMain.aspx");
        }

        protected void dataGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[3].Visible = false;
        }

        private void CalcSum()
        {
            if (TreatmentPrescriptions.Count != 0)
            {
                try
                {
                    price = 0;
                    for (int i = 0; i < TreatmentPrescriptions.Count; i++)
                    {
                        PrescriptionViewModel prescription = logicP.GetElement(TreatmentPrescriptions[i].PrescriptionId);
                        price += (int)prescription.TotalPrice * TreatmentPrescriptions[i].Count;
                    }
                    textBoxPrice.Text = price.ToString();
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                }
            }
        }
    }
}