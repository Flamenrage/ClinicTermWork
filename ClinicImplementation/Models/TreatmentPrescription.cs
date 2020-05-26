using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClinicImplementation.Models
{
    public class TreatmentPrescription
    {
        public int Id { get; set; }
        public int TreatmentId { get; set; }
        public int PrescriptionId { get; set; }
        public string PrescriptionName { get; set; }
        public int Count { get; set; }
        public virtual Treatment Treatment { get; set; }
        public virtual Prescription Prescription { get; set; }
    }
}
