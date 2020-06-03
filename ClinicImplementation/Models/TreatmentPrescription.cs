using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace ClinicImplementation.Models
{
    [DataContract]
    public class TreatmentPrescription
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int TreatmentId { get; set; }
        [DataMember]
        public int PrescriptionId { get; set; }
        [DataMember]
        public string PrescriptionName { get; set; }
        [DataMember]
        public int Count { get; set; }
        public virtual Treatment Treatment { get; set; }
        public virtual Prescription Prescription { get; set; }
    }
}
