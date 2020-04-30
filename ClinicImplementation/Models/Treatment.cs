using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClinicImplementation.Models
{
    public class Treatment
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int PatientId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int TotalPrice { get; set; }

        [Required]
        public bool isReserved { get; set; }

        public virtual Patient Patient { get; set; }

        [ForeignKey("TreatmentId")]
        public virtual List<TreatmentPrescription> TreatmentPrescriptions { get; set; }
    }
}
