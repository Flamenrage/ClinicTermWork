using ClinicBusinessLogic.BindingModels;
using ClinicBusinessLogic.Interfaces;
using ClinicBusinessLogic.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;


namespace ClinicImplementation.Implementations
{
    public class ReportLogic : IReportLogic
    {
        public List<ReportViewModel> GetReport(ReportBindingModel model)
        {
            using (var context = new DatabaseContext())
            {
                List<ReportViewModel> list = new List<ReportViewModel>();

                list.AddRange(GetRequests(model));

                list.AddRange(GetTreatments(model, -1));

                return list;
            }
        }

        public List<ReportViewModel> GetRequests(ReportBindingModel model)
        {
            using (var context = new DatabaseContext())
            {

            }
        }

        public List<ReportViewModel> GetTreatments(ReportBindingModel model, int PatientId)
        {
            using (var context = new DatabaseContext())
            {
                List<ReportViewModel> list = new List<ReportViewModel>();
                //собираем информацию о лечении для конкретного пациента
                if (PatientId != -1)
                {
                    foreach (var tr in context.Treatments.Where(rec => rec.Date >= model.DateFrom && rec.Date <= model.DateTo && rec.PatientId == PatientId))
                    {
                        foreach (var trPr in context.TreatmentPrescriptions.Where(rec => rec.TreatmentId == tr.Id))
                        {
                            int i = 0;
                            foreach (var med in context.MedicationPrescriptions.Where(rec => rec.PrescriptionId == trPr.PrescriptionId))
                            {
                                ReportViewModel report = new ReportViewModel();
                                if (i < 1)
                                {
                                    report.FIO = context.Patients.FirstOrDefault(rec => rec.Id == tr.PatientId).FIO;
                                    report.Name = tr.Name;
                                    report.Date = tr.Date;
                                }
                                else
                                {
                                    report.FIO = " ";
                                    report.Name = " ";
                                    report.Date = default;
                                }
                                report.MedicationName = med.MedicationName;
                                report.Count = med.Count * trPr.Count;
                                list.Add(report);
                                i++;
                            }
                        }
                    }
                }
                else  //собираем информацию о лечении для всех пациентов
                {
                    foreach (var tr in context.Treatments.Where(rec => rec.Date >= model.DateFrom && rec.Date <= model.DateTo))
                    {
                        foreach (var trPr in context.TreatmentPrescriptions.Where(rec => rec.TreatmentId == tr.Id))
                        {
                            int i = 0;
                            foreach (var med in context.MedicationPrescriptions.Where(rec => rec.PrescriptionId == trPr.PrescriptionId))
                            {
                                ReportViewModel report = new ReportViewModel();
                                if (i < 1)
                                {
                                    report.FIO = context.Patients.FirstOrDefault(rec => rec.Id == tr.PatientId).FIO;
                                    report.Name = tr.Name;
                                    report.Date = tr.Date;
                                }
                                else
                                {
                                    report.FIO = " ";
                                    report.Name = " ";
                                    report.Date = default;
                                }
                                report.MedicationName = med.MedicationName;
                                report.Count = med.Count * trPr.Count;
                                list.Add(report);
                                i++;
                            }
                        }
                    }
                }
                return list;
            }
        }

        public void SaveReport(ReportBindingModel model, int PatientId)
        {
            using (var context = new DatabaseContext())
            {

            }
        }

        public void SaveToExcel(ReportBindingModel model, int PatientId)
        {
            using (var context = new DatabaseContext())
            {

            }
        }

        public void SaveTreatments(ReportBindingModel model, int PatientId)
        {
            using (var context = new DatabaseContext())
            {
                FileStream fs = new FileStream(model.FileName, FileMode.OpenOrCreate, FileAccess.Write);

                //создаем документ, задаем границы, связываем документ и поток
                iTextSharp.text.Document doc = new iTextSharp.text.Document();

                try
                {
                    //открываем файл для работы                
                    doc.SetMargins(0.5f, 0.5f, 0.5f, 0.5f);
                    PdfWriter writer = PdfWriter.GetInstance(doc, fs);

                    doc.Open();
                    BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\times.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                    //вставляем заголовок
                    var phraseName = new Phrase("Отчет",
                    new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD));
                    iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph(phraseName)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 12
                    };
                    doc.Add(paragraph);

                    var timePeriod = new Phrase("c " + model.DateFrom.Value.ToShortDateString() +
                                                  " по " + model.DateTo.Value.ToShortDateString(),
                                                  new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD));

                    paragraph = new iTextSharp.text.Paragraph(timePeriod)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 12
                    };
                    doc.Add(paragraph);

                    //вставляем таблицу, задаем количество столбцов, и ширину колонок
                    PdfPTable table = new PdfPTable(5)
                    {
                        TotalWidth = 800F
                    };
                    table.SetTotalWidth(new float[] { 160, 100, 120, 180, 120 });

                    //вставляем шапку
                    PdfPCell cell = new PdfPCell();
                    var fontForCellBold = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD);
                    table.AddCell(new PdfPCell(new Phrase("Название", fontForCellBold))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });
                    table.AddCell(new PdfPCell(new Phrase("Дата", fontForCellBold))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });
                    table.AddCell(new PdfPCell(new Phrase("Имя", fontForCellBold))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });
                    table.AddCell(new PdfPCell(new Phrase("Лекарство", fontForCellBold))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });
                    table.AddCell(new PdfPCell(new Phrase("Количество", fontForCellBold))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });

                    //заполняем таблицу
                    var list = GetTreatments(model, PatientId);
                    var fontForCells = new iTextSharp.text.Font(baseFont, 10);

                    foreach (var el in list)
                    {
                        cell = new PdfPCell(new Phrase(el.Name, fontForCells));
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(el.Date.ToShortDateString(), fontForCells));
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(el.FIO, fontForCells));
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(el.MedicationName, fontForCells));
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(el.Count.ToString(), fontForCells));
                        table.AddCell(cell);
                    }

                    doc.Add(table);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
                finally
                {
                    doc.Close();
                    Thread.Sleep(5);
                }
            }
        }
    }
}
