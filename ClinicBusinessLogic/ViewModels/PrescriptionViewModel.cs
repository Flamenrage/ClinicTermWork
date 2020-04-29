using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ClinicBusinessLogic.ViewModels
{
    public class PrescriptionViewModel
    {
        public int Id { get; set; }
        [DisplayName("Название")]
        public string Name { get; set; }
        [DisplayName("Стоимость")]
        public int? TotalPrice { get; set; }
        public Dictionary<int, (string, int)> PrescriptionMedications { get; set; }
    }
}
