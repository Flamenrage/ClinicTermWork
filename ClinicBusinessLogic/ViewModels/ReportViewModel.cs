using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBusinessLogic.ViewModels
{
    public class ReportViewModel
    {
        public string FIO { get; set; }

        public DateTime Date { get; set; }

        public string Name { get; set; }

        public string MedicationName { get; set; }

        public int Count { get; set; }
    }
}
