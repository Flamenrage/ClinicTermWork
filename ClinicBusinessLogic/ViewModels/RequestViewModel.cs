using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ClinicBusinessLogic.ViewModels
{
    public class RequestViewModel
    {
        public int Id { get; set; }
        [DisplayName("Название")]
        public string Name { get; set; }
        [DisplayName("Дата создания")]
        public DateTime Date { get; set; }
        public Dictionary<int, (string, int)> RequestMedications { get; set; }
    }
}
