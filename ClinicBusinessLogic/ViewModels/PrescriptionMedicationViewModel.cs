using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ClinicBusinessLogic.ViewModels
{
    public class PrescriptionMedicationViewModel
    {
        public int Id { get; set; }
        public int PrescriptionId { get; set; }
        public int MedicationId { get; set; }
        [DisplayName("Название лекарства")]
        public string MedicationName { get; set; }        
        [DisplayName("Количество")]
        public int Count { get; set; }
    }
}
