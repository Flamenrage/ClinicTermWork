using ClinicBusinessLogic.BindingModels;
using ClinicBusinessLogic.Interfaces;
using ClinicBusinessLogic.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.qrcode;
using Microsoft.EntityFrameworkCore;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
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
                return context.RequestMedications
                .Include(rec => rec.Request)
                .Include(rec => rec.Medication)
                .Where(rec => rec.Request.Date >= model.DateFrom && rec.Request.Date <= model.DateTo)
                .Select(rec => new ReportViewModel
                {
                    Name = rec.Request.Name,
                    Date = rec.Request.Date,
                    FIO = "Администратор",
                    MedicationName = rec.MedicationName,
                    Count = rec.Count
                })
                .ToList();
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
                                    report.Date = DateTime.MinValue;
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
                                    report.Date = DateTime.MinValue;
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

        public void SaveReport(ReportBindingModel model)
        {
            using (var context = new DatabaseContext())
            {
                using (FileStream fs = new FileStream(model.FileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    //создаем документ, задаем границы, связываем документ и поток
                    iTextSharp.text.Document doc = new iTextSharp.text.Document();

                    try
                    {
                        //открываем файл для работы                
                        doc.SetMargins(0.5f, 0.5f, 0.5f, 0.5f);
                        PdfWriter writer = PdfWriter.GetInstance(doc, fs);

                        doc.Open();
                        BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\times.ttf",
                            BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                        //вставляем заголовок
                        var phraseTitle = new Phrase("Отчет",
                        new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD));
                        iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph(phraseTitle)
                        {
                            Alignment = Element.ALIGN_CENTER,
                            SpacingAfter = 12
                        };
                        doc.Add(paragraph);

                        var phrasePeriod = new Phrase("c " + model.DateFrom.Value.ToShortDateString() +
                                                      " по " + model.DateTo.Value.ToShortDateString(),
                                                      new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD));

                        paragraph = new iTextSharp.text.Paragraph(phrasePeriod)
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
                        var list = GetReport(model);
                        var fontForCells = new iTextSharp.text.Font(baseFont, 10);

                        foreach (var el in list)
                        {
                            cell = new PdfPCell(new Phrase(el.Name, fontForCells));
                            table.AddCell(cell);

                            if (el.Date == DateTime.MinValue)
                            {
                                cell = new PdfPCell(new Phrase(" ", fontForCells));
                            }
                            else
                            {
                                cell = new PdfPCell(new Phrase(el.Date.ToShortDateString(), fontForCells));
                            }
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

        public void SaveToExcel(ReportBindingModel model, int PatientId)
        {
            using (var context = new DatabaseContext())
            {
                var excel = new Microsoft.Office.Interop.Excel.Application();
                try
                {
                    if (File.Exists(model.FileName))
                    {
                        File.Delete(model.FileName);                        
                    }
                    File.Create(model.FileName);
                    excel.Workbooks.Open(model.FileName, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing);                    
                    excel.SheetsInNewWorkbook = 1;
                    excel.Workbooks.Add(Type.Missing);
                    excel.Workbooks[1].SaveAs(model.FileName, XlFileFormat.xlExcel8, Type.Missing,
                        Type.Missing, false, false, XlSaveAsAccessMode.xlNoChange, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing);                    
                    Sheets excelsheets = excel.Workbooks[1].Worksheets;
                    var excelworksheet = (Worksheet)excelsheets.get_Item(1);
                    excelworksheet.Cells.Clear();
                    excelworksheet.PageSetup.Orientation = XlPageOrientation.xlLandscape;
                    excelworksheet.PageSetup.CenterHorizontally = true;
                    excelworksheet.PageSetup.CenterVertically = true;
                    Microsoft.Office.Interop.Excel.Range excelcells = excelworksheet.get_Range("A1", "E1");
                    excelcells.Merge(Type.Missing);
                    excelcells.Font.Bold = true;
                    excelcells.Value2 = "Отчет";
                    excelcells.RowHeight = 25;
                    excelcells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    excelcells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                    excelcells.Font.Name = "Times New Roman";
                    excelcells.Font.Size = 14;

                    excelcells = excelworksheet.get_Range("A2", "E2");
                    excelcells.Merge(Type.Missing);
                    excelcells.Value2 = DateTime.Now.ToShortDateString();
                    excelcells.RowHeight = 20;
                    excelcells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    excelcells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                    excelcells.Font.Name = "Times New Roman";
                    excelcells.Font.Size = 12;

                    excelcells = excelworksheet.get_Range("A3", "A3");
                    excelcells.Font.Bold = true;
                    excelcells.ColumnWidth = 15;
                    excelcells.Value2 = "Название";
                    excelcells = excelcells.get_Offset(0, 1);
                    excelcells.Font.Bold = true;
                    excelcells.ColumnWidth = 15;
                    excelcells.Value2 = "Дата";
                    excelcells = excelcells.get_Offset(0, 1);
                    excelcells.Font.Bold = true;
                    excelcells.ColumnWidth = 15;
                    excelcells.Value2 = "Имя";
                    excelcells = excelcells.get_Offset(0, 1);
                    excelcells.Font.Bold = true;
                    excelcells.ColumnWidth = 15;
                    excelcells.Value2 = "Лекарство";
                    excelcells = excelcells.get_Offset(0, 1);
                    excelcells.Font.Bold = true;
                    excelcells.ColumnWidth = 15;
                    excelcells.Value2 = "Количество";
                    excelcells = excelworksheet.get_Range("A4", "A4");

                    List<ReportViewModel> list = new List<ReportViewModel>();
                    //если название файла содержит "Пациент", то получаем информацию по лечениям для пациента по Id
                    if (model.FileName.Contains("Patient"))
                    {
                        list = GetTreatments(model, PatientId);
                    }
                    else //если нет, то получаем информацию о заявках 
                    {
                        list = GetRequests(model);
                    }
                    //если список не пустой
                    if (list != null)
                    {   //заполняем таблицу
                        foreach (var el in list)
                        {
                            excelcells.Value2 = el.Name;
                            excelcells = excelcells.get_Offset(0, 1);

                            if (el.Date == DateTime.MinValue)
                            {
                                excelcells.Value2 = " ";
                            }
                            else
                            {
                                excelcells.Value2 = el.Date.ToShortDateString();
                            }                            
                            excelcells = excelcells.get_Offset(0, 1);

                            excelcells.Value2 = el.FIO;
                            excelcells = excelcells.get_Offset(0, 1);

                            excelcells.Value2 = el.MedicationName;
                            excelcells = excelcells.get_Offset(0, 1);

                            excelcells.Value2 = el.Count;
                            excelcells = excelcells.get_Offset(1, -4);
                        }
                    }
                    excel.Workbooks[1].Save();
                    excel.Workbooks.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
                finally
                {
                    excel.Quit();
                    Thread.Sleep(5);
                }
            }
        }
        public void SaveToWord(ReportBindingModel model, int patientId)
        {
            using(var context = new DatabaseContext())
            {
                var word = new Microsoft.Office.Interop.Word.Application();
                try
                {                    
                    if (File.Exists(model.FileName))
                    {
                        File.Delete(model.FileName);
                    }
                    Microsoft.Office.Interop.Word.Document doc = word.Documents.Add(Type.Missing);                                       
                    // общие настройки страницы
                    doc.Content.Font.Name = "Times New Roman";
                    doc.PageSetup.LeftMargin = 50;
                    doc.PageSetup.RightMargin = 50;
                    // заголовок
                    var paragraph = doc.Content.Paragraphs.Add(Type.Missing);
                    paragraph.Range.Text = "Отчёт";
                    paragraph.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    paragraph.Range.Font.Size = 20;
                    paragraph.Range.Font.Bold = 2;
                    paragraph.Format.SpaceAfter = 18;
                    paragraph.Range.InsertParagraphAfter();
                    // дата
                    paragraph = doc.Content.Paragraphs.Add(Type.Missing);
                    paragraph.Range.Text = $"{DateTime.Now.ToShortDateString()}";
                    paragraph.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    paragraph.Range.Font.Size = 18;
                    paragraph.Range.Font.Bold = 2;
                    paragraph.Format.SpaceAfter = 18;
                    paragraph.Range.InsertParagraphAfter();
                    // таблица 

                    // нам нужно сразу знать, сколько строк потребуется
                    List<ReportViewModel> list = null;
                    if (patientId == -1)
                    {
                        list = GetRequests(model);
                    }
                    else
                    {
                        list = GetTreatments(model, patientId);
                    }
                    var table = doc.Tables.Add(doc.Bookmarks.get_Item("\\endofdoc").Range,
                        list.Count + 1, 5, Type.Missing, WdAutoFitBehavior.wdAutoFitContent);
                    table.Range.Font.Size = 14;
                    table.Range.Font.Bold = 0;
                    table.AllowAutoFit = true;
                    table.Borders.Enable = 1;
                    table.Cell(1, 1).Range.Text = "Название";
                    table.Cell(1, 2).Range.Text = "Дата";
                    table.Cell(1, 3).Range.Text = "Имя";
                    table.Cell(1, 4).Range.Text = "Лекарство";
                    table.Cell(1, 5).Range.Text = "Количество";
                    for (int i = 0; i < list.Count; i++)
                    {
                        table.Cell(i + 2, 1).Range.Text = list[i].Name;
                        if (list[i].Date == DateTime.MinValue)
                        {
                            table.Cell(i + 2, 2).Range.Text = " ";
                        }
                        else
                        {
                            table.Cell(i + 2, 2).Range.Text = list[i].Date.ToShortDateString();
                        }                        
                        table.Cell(i + 2, 3).Range.Text = list[i].FIO;
                        table.Cell(i + 2, 4).Range.Text = list[i].MedicationName;
                        table.Cell(i + 2, 5).Range.Text = list[i].Count.ToString();
                    }
                    doc.SaveAs(FileName: model.FileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
                finally
                {
                    word.Quit();
                    Thread.Sleep(5);
                }
            }
        }

        public void SaveTreatments(ReportBindingModel model, int PatientId)
        {
            using (var context = new DatabaseContext())
            {
                using (FileStream fs = new FileStream(model.FileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
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

                            if (el.Date == DateTime.MinValue)
                            {
                                cell = new PdfPCell(new Phrase(" ", fontForCells));
                            }
                            else
                            {
                                cell = new PdfPCell(new Phrase(el.Date.ToShortDateString(), fontForCells));
                            }                            
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
}
