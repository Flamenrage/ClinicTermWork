using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBusinessLogic.ViewModels
{
    public class TreatmentPrescriptionViewModel
    {
        public int Id { get; set; }

        public int TreatmentId { get; set; }

        public int PrescriptionId { get; set; }

        public string PrescriptionName { get; set; }

        public int Count { get; set; }
    }
}
