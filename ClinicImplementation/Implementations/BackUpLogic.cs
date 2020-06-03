using ClinicBusinessLogic.BusinessLogic;
using ClinicBusinessLogic.HelperModels;
using ClinicImplementation.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;

namespace ClinicImplementation.Implementations
{
    public static class BackUpLogic
    {
        public static void PatientBackUpXML(int PatientId)
        {
            using (var context = new DatabaseContext())
            {
                var treatments = context.Treatments.Where(rec => rec.PatientId == PatientId).ToList();
                var trprs = new List<TreatmentPrescription>();
                foreach (var t in treatments)
                {
                    foreach (var m in context.TreatmentPrescriptions.Where(rec => rec.TreatmentId == t.Id).ToList())
                    {
                        trprs.Add(m);
                    }
                }
                var prescriptions = context.Prescriptions.ToList();

                string treatPath = @"C:\temp\Treatments.xml";
                string treatPrescPath = @"C:\temp\TreatmentPrescriptions.xml";
                string prescPath = @"C:\temp\Prescriptions.xml";

                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                    NewLineOnAttributes = true
                };
                using (FileStream fileStream = new FileStream(treatPath, FileMode.Create))
                {
                    using (XmlWriter writer = XmlWriter.Create(fileStream, settings))
                    {
                        writer.WriteStartElement("Treatments");
                        foreach (var treat in treatments)
                        {
                            writer.WriteStartElement("Treatment");
                            writer.WriteElementString("Id", treat.Id.ToString());
                            writer.WriteElementString("Date", treat.Date.ToShortDateString());
                            writer.WriteElementString("Name", treat.Name);
                            writer.WriteElementString("TotalPrice", treat.TotalPrice.ToString());
                            writer.WriteElementString("IsReserved", treat.IsReserved.ToString());
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        writer.Flush();
                    }
                }

                using (FileStream fileStream = new FileStream(treatPrescPath, FileMode.Create))
                {
                    using (XmlWriter writer = XmlWriter.Create(fileStream, settings))
                    {
                        writer.WriteStartElement("TreatmentPrescriptions");
                        foreach (var tp in trprs)
                        {
                            writer.WriteStartElement("TreatmentPrescription");
                            writer.WriteElementString("Id", tp.Id.ToString());
                            writer.WriteElementString("TreatmentId", tp.TreatmentId.ToString());
                            writer.WriteElementString("PrescriptionId", tp.PrescriptionId.ToString());
                            writer.WriteElementString("PrescriptionName", tp.PrescriptionName);
                            writer.WriteElementString("Count", tp.Count.ToString());
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        writer.Flush();
                    }
                }

                using (FileStream fileStream = new FileStream(prescPath, FileMode.Create))
                {
                    using (XmlWriter writer = XmlWriter.Create(fileStream, settings))
                    {
                        writer.WriteStartElement("Prescriptions");
                        foreach (var pres in prescriptions)
                        {
                            writer.WriteStartElement("Prescription");
                            writer.WriteElementString("Id", pres.Id.ToString());
                            writer.WriteElementString("Name", pres.Name);
                            writer.WriteElementString("TotalPrice", pres.TotalPrice.ToString());
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        writer.Flush();
                    }
                }

                Patient patient = context.Patients.FirstOrDefault(rec => rec.Id == PatientId);
                _ = MailLogic.SendMailBackUp(new MailSendInfo { Email = patient.Email, Subject = "Бекап БД в формате XML", Body = "" },
                    new string[] { treatPath, treatPrescPath, prescPath }); 
            }
        }

        public static void PatientBackUpJSON(int PatientId)
        {
            using (var context = new DatabaseContext())
            {
                string treatPath = @"C:\temp\Treatments.json";
                string treatPrescPath = @"C:\temp\TreatmentPrescriptions.json";
                string prescPath = @"C:\temp\Prescriptions.json";

                var treatments = context.Treatments.Where(rec => rec.PatientId == PatientId).ToList();
                var trprs = new List<TreatmentPrescription>();
                foreach (var t in treatments)
                {
                    foreach (var m in context.TreatmentPrescriptions.Where(rec => rec.TreatmentId == t.Id).ToList())
                    {
                        trprs.Add(m);
                    }
                }
                var prescriptions = context.Prescriptions.ToList();

                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<Treatment>));
                using (FileStream fs = new FileStream(treatPath, FileMode.Create))
                {
                    jsonFormatter.WriteObject(fs, treatments);
                }

                jsonFormatter = new DataContractJsonSerializer(typeof(List<TreatmentPrescription>));
                using (FileStream fs = new FileStream(treatPrescPath, FileMode.Create))
                {
                    jsonFormatter.WriteObject(fs, trprs);
                }

                jsonFormatter = new DataContractJsonSerializer(typeof(List<Prescription>));
                using (FileStream fs = new FileStream(prescPath, FileMode.Create))
                {
                    jsonFormatter.WriteObject(fs, prescriptions);
                }

                Patient patient = context.Patients.FirstOrDefault(rec => rec.Id == PatientId);
                _ = MailLogic.SendMailBackUp(new MailSendInfo { Email = patient.Email, Subject = "Бекап БД в формате JSON", Body = "" },
                   new string[] { treatPath, treatPrescPath, prescPath });
            }
        }
    }
}