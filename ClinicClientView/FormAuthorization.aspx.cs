using ClinicBusinessLogic.BindingModels;
using ClinicBusinessLogic.Interfaces;
using ClinicBusinessLogic.ViewModels;
using ClinicClientView.App_Start;
using ClinicImplementation.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClinicClientView
{
    public partial class FormAuthorization : System.Web.UI.Page
    {
        private readonly IPatientLogic logic = new PatientLogic();

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void RegistrationButton_Click(object sender, EventArgs e)
        {
            try
            {
                String fio = textBoxFIO.Text;
                String email = textBoxEmail.Text;
                String password = textBoxPassword.Text;

                if (!string.IsNullOrEmpty(fio) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    logic.AddElement(new PatientBindingModel
                    {
                        FIO = fio,
                        Email = email,
                        Password = password
                    });
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Регистрация прошла успешно');</script>");

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните все поля');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void SignInButton_Click(object sender, EventArgs e)
        {
            String email = textBoxEmail.Text;
            String password = textBoxPassword.Text;

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                List<PatientViewModel> patients = logic.GetList();
                foreach (PatientViewModel patient in patients)
                {
                    if (patient.Email.Equals(email) && patient.Password.Equals(password))
                    {
                        Session["PatientId"] = patient.Id.ToString();
                        Session["PatientEmail"] = patient.Email;
                        Response.Redirect("FormMain.aspx");
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Нет такого пользователя');</script>");
                    }
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните все поля');</script>");
            }
        }
    }
}