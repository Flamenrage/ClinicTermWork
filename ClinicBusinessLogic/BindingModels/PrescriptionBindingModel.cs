using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBusinessLogic.BindingModels
{
    public class PrescriptionBindingModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? TotalPrice { get; set; }
        public List<PrescriptionMedicationBindingModel> PrescriptionMedications { get; set; }
    }
}
