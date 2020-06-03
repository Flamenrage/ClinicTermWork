using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace ClinicImplementation.Models
{
    [DataContract]
    public class MedicationPrescription
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int MedicationId { get; set; }
        [DataMember]
        public int PrescriptionId { get; set; }
        [DataMember]
        public string MedicationName { get; set; }
        [DataMember]
        [Required]
        public int Count { get; set; }
        public virtual Medication Medication { get; set; }
        public virtual Prescription Prescription { get; set; }
    }
}
