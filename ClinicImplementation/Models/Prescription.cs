using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClinicImplementation.Models
{
    public class Prescription
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int? TotalPrice { get; set; }
        [ForeignKey("PrescriptionId")]
        public virtual List<MedicationPrescription> MedicationPrescriptions { get; set; }
    }
}
