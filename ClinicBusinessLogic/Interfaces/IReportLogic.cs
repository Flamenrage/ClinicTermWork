using ClinicBusinessLogic.BindingModels;
using ClinicBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBusinessLogic.Interfaces
{
    public interface IReportLogic
    {
        List<ReportViewModel> GetReport(ReportBindingModel model);

        List<ReportViewModel> GetRequests(ReportBindingModel model);

        List<ReportViewModel> GetTreatments(ReportBindingModel model, int PatientId);

        void SaveReport(ReportBindingModel model, int PatientId);

        void SaveTreatments(ReportBindingModel model, int PatientId);

        void SaveToExcel(ReportBindingModel model, int PatientId);
    }
}
