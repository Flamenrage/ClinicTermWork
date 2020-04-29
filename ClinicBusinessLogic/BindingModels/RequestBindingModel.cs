using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBusinessLogic.BindingModels
{
    public class RequestBindingModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public Dictionary<int, (string, int)> RequestMedications { get; set; }
    }
}
