using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBusinessLogic.BindingModels
{
    public class MedicationBindingModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
    }
}
