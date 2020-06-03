using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace ClinicImplementation.Models
{
    [DataContract]
    public class Medication
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        [Required]
        public string Name { get; set; }
        [DataMember]
        [Required]
        public int Price { get; set; }
        [DataMember]
        [Required]
        public int Count { get; set; }
        [ForeignKey("MedicationId")]
        public virtual List<RequestMedication> RequestMedications { get; set; }
        [ForeignKey("MedicationId")]
        public virtual List<MedicationPrescription> MedicationPrescriptions { get; set; }
    }
}
