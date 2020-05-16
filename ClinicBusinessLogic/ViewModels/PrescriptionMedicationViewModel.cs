using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBusinessLogic.ViewModels
{
    public class PrescriptionMedicationViewModel
    {
        public int Id { get; set; }

        public int PrescriptionId { get; set; }

        public string MedicationName { get; set; }

        public int MedicationId { get; set; }

        public int Count { get; set; }
    }
}
