using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClinicImplementation.Models
{
    public class RequestMedication
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int MedicationId { get; set; }
        public string MedicationName { get; set; }
        [Required]
        public int Count { get; set; }
        public virtual Medication Medication { get; set; }
        public virtual Request Request { get; set; }        
    }
}
