using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBusinessLogic.BindingModels
{
    public class TreatmentBindingModel
    {
        public int? Id { get; set; }
        public int PatientId { get; set; }
        public string Name { get; set; }
        public int? TotalPrice { get; set; }
        public bool IsReserved { get; set; }
        public List<TreatmentPrescriptionBindingModel> TreatmentPrescriptions { get; set; }
    }
}
