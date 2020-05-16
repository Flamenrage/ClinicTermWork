using ClinicBusinessLogic.BindingModels;
using ClinicBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBusinessLogic.Interfaces
{
    public interface IPrescriptionLogic
    {
        List<PrescriptionViewModel> GetList();

        List<PrescriptionViewModel> GetAvailableList();

        List<PrescriptionViewModel> GetPatientList(int PatientId);

        PrescriptionViewModel GetElement(int id);

        void AddElement(PrescriptionBindingModel model);

        void UpdElement(PrescriptionBindingModel model);

        void DelElement(int id);
    }
}
