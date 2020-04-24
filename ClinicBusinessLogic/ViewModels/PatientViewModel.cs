using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBusinessLogic.ViewModels
{
    public class PatientViewModel
    {
        public int Id { get; set; }

        public string FIO { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
