using System;
using System.Collections.Generic;
using ClinicBusinessLogic.ViewModels;
using ClinicBusinessLogic.BindingModels;
using System.Text;

namespace ClinicBusinessLogic.Interfaces
{
    public interface IPatientLogic
    {
        List<PatientViewModel> GetList();

        PatientViewModel GetElement(string Email, string Password);

        void AddElement(PatientBindingModel model);
    }
}
