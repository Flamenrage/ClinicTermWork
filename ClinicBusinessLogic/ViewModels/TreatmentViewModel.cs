using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ClinicBusinessLogic.ViewModels
{
    public class TreatmentViewModel
    {
        public int Id { get; set; }

        public int PatientId { get; set; }
        [DisplayName("Название")]
        public string Name { get; set; }

        [DisplayName("Дата")]
        public DateTime Date { get; set; }

        [DisplayName("Стоимость")]
        public int? TotalPrice { get; set; }

        [DisplayName("Забронировано")]
        public bool isReserved { get; set; }

        public Dictionary<int, (string, int)> TreatmentPrescriptions { get; set; }
    }
}
