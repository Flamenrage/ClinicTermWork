using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBusinessLogic.BindingModels
{
    public class PrescriptionMedicationBindingModel
    {
        public int Id { get; set; }
        public int PrescriptionId { get; set; }
        public int MedicationId { get; set; }
        public string MedicationName { get; set; }        
        public int Count { get; set; }
    }
}
