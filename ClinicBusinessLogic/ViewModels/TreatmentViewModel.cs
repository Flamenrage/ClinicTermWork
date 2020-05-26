using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ClinicBusinessLogic.ViewModels
{
    public class TreatmentViewModel
    {
        public int Id { get; set; }
        [DisplayName("Дата")]
        public DateTime Date { get; set; }
        public int PatientId { get; set; }
        [DisplayName("Название")]
        public string Name { get; set; }
        [DisplayName("Стоимость")]
        public int? TotalPrice { get; set; }
        [DisplayName("Забронировано")]
        public bool IsReserved { get; set; }
        public List<TreatmentPrescriptionViewModel> TreatmentPrescriptions { get; set; }
    }
}
