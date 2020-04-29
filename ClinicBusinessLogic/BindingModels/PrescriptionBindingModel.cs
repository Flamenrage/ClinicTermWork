using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBusinessLogic.BindingModels
{
    public class PrescriptionBindingModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? TotalPrice { get; set; }
        public Dictionary<int, (string, int)> PrescriptionMedications { get; set; }
    }
}
