using ClinicBusinessLogic.BindingModels;
using ClinicBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBusinessLogic.Interfaces
{
    public interface IMainLogic
    {
        List<TreatmentViewModel> GetList();

        List<TreatmentViewModel> GetPatientList(int PatientId);

        TreatmentViewModel GetTreatment(int id);

        void CreateTreatment(TreatmentBindingModel model);

        void UpdTreatment(TreatmentBindingModel model);

        void DelTreatment(int id);

        DateTime TreatmentReservation(int id);

        void MedicationRefill(RequestMedicationBindingModel model);
    }
}
