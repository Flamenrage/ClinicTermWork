using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClinicImplementation.Models
{
    public class Request
    {
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Name { get; set; }
        [ForeignKey("RequestId")]
        public virtual List<RequestMedication> RequestMedications { get; set; }
    }
}
