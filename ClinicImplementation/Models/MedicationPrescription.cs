using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClinicImplementation.Models
{
    public class MedicationPrescription
    {
        public int Id { get; set; }
        public int MedicationId { get; set; }
        public int PrescriptionId { get; set; }
        [Required]
        public int Count { get; set; }
        public virtual Medication Medication { get; set; }
        public virtual Prescription Prescription { get; set; }
    }
}
