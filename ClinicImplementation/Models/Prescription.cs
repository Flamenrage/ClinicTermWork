using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace ClinicImplementation.Models
{
    [DataContract]
    public class Prescription
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        [Required]
        public string Name { get; set; }
        [DataMember]
        public int? TotalPrice { get; set; }
        [ForeignKey("PrescriptionId")]
        public virtual List<MedicationPrescription> MedicationPrescriptions { get; set; }
        [ForeignKey("PrescriptionId")]
        public virtual List<TreatmentPrescription> TreatmentPrescriptions { get; set; }
    }
}
