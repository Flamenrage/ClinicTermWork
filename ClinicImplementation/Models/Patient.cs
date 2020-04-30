using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClinicImplementation.Models
{
    public class Patient
    {
        public int Id { get; set; }
        [Required]
        public string FIO { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [ForeignKey("PatientId")]
        public List<Treatment> Treatments { get; set; }
    }
}
