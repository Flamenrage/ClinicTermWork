using ClinicBusinessLogic.BindingModels;
using ClinicBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBusinessLogic.Interfaces
{
    public interface IMedicationLogic
    {
        List<MedicationViewModel> GetList();

        List<MedicationViewModel> GetMostList(bool isForDiagram);

        MedicationViewModel GetElement(int id);

        void AddElement(MedicationBindingModel model);

        void UpdElement(MedicationBindingModel model);

        void DelElement(int id);
    }
}
