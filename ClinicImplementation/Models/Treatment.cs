using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace ClinicImplementation.Models
{
    [DataContract]
    public class Treatment
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public int PatientId { get; set; }
        [DataMember]
        [Required]
        public string Name { get; set; }
        [DataMember]
        [Required]
        public int TotalPrice { get; set; }
        [DataMember]
        [Required]
        public bool IsReserved { get; set; }
        public virtual Patient Patient { get; set; }
        [ForeignKey("TreatmentId")]
        public virtual List<TreatmentPrescription> TreatmentPrescriptions { get; set; }
    }
}
