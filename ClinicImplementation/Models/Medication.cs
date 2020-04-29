using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClinicImplementation.Models
{
    public class Medication
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Count { get; set; }
        [ForeignKey("MedicationId")]
        public virtual List<RequestMedication> RequestMedications { get; set; }
        [ForeignKey("MedicationId")]
        public virtual List<MedicationPrescription> MedicationPrescriptions { get; set; }
    }
}
