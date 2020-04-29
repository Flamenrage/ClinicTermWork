using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ClinicBusinessLogic.ViewModels
{
    public class MedicationViewModel
    {
        public int Id { get; set; }
        [DisplayName("Название")]
        public string Name { get; set; }
        [DisplayName("Цена")]
        public int Price { get; set; }
        [DisplayName("Дозировка")]
        public int Count { get; set; }
    }
}
