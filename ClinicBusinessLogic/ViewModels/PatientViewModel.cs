using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ClinicBusinessLogic.ViewModels
{
    public class PatientViewModel
    {
        public int Id { get; set; }

        [DisplayName("ФИО")]

        public string FIO { get; set; }

        [DisplayName("Email")]

        public string Email { get; set; }

        [DisplayName("Пароль")]

        public string Password { get; set; }
    }
}
