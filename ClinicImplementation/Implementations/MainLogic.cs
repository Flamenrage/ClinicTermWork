using ClinicBusinessLogic.BindingModels;
using ClinicBusinessLogic.Interfaces;
using ClinicBusinessLogic.ViewModels;
using ClinicImplementation.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;

namespace ClinicImplementation.Implementations
{
    public class MainLogic : IMainLogic
    {
        public void CreateTreatment(TreatmentBindingModel model)
        {
            using (var context = new DatabaseContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Treatment element = context.Treatments.FirstOrDefault(rec =>
                         rec.Name == model.Name);
                        if (element != null)
                        {
                            throw new Exception("Уже есть лечение с таким названием");
                        }
                        element = new Treatment
                        {
                            Date = DateTime.Now,
                            PatientId = model.PatientId,
                            Name = model.Name,
                            TotalPrice = (int)model.TotalPrice,
                        };
                        context.Treatments.Add(element);
                        context.SaveChanges();
                        // избавляемся от дублей по рецептам 
                        var unitedPrescriptions = model.TreatmentPrescriptions
                            .GroupBy(rec => rec.PrescriptionId)
                            .Select(rec => new
                            {
                                PrescriptionId = rec.Key,
                                Count = rec.Sum(r => r.Count)
                            });
                        // запоминаем id и названия рецептов
                        var prescriptionName = model.TreatmentPrescriptions.Select(rec => new
                        {
                            PrescriptionId = rec.PrescriptionId,
                            PrescriptionName = rec.PrescriptionName
                        });
                        // добавляем рецепты  
                        foreach (var unitedPrescription in unitedPrescriptions)
                        {
                            string Name = null;
                            foreach (var prescription in prescriptionName)
                            {
                                if (unitedPrescription.PrescriptionId == prescription.PrescriptionId)
                                {
                                    Name = prescription.PrescriptionName;
                                }
                            }
                            context.TreatmentPrescriptions.Add(new TreatmentPrescription
                            {
                                TreatmentId = element.Id,
                                PrescriptionId = unitedPrescription.PrescriptionId,
                                PrescriptionName = Name,
                                Count = unitedPrescription.Count
                            });
                            context.SaveChanges();
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void DelTreatment(int id)
        {
            using (var context = new DatabaseContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Treatment element = context.Treatments.FirstOrDefault(rec => rec.Id == id);
                        if (element != null)
                        {
                            // удаяем записи по рецептам при удалении лечения
                            context.TreatmentPrescriptions.RemoveRange(context.TreatmentPrescriptions.Where(rec => rec.TreatmentId == id));
                            context.Treatments.Remove(element);
                            context.SaveChanges();
                        }
                        else
                        {
                            throw new Exception("Элемент не найден");
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public List<TreatmentViewModel> GetList()
        {
            using (var context = new DatabaseContext())
            {
                List<TreatmentViewModel> result = context.Treatments.Select(rec =>
           new TreatmentViewModel
           {
               Id = rec.Id,
               Date = rec.Date,
               PatientId = rec.PatientId,
               Name = rec.Name,
               TotalPrice = rec.TotalPrice,
               IsReserved = rec.IsReserved,
               TreatmentPrescriptions = context.TreatmentPrescriptions
                   .Where(recPC => recPC.TreatmentId == rec.Id)
                   .Select(recPC => new TreatmentPrescriptionViewModel
                   {
                       Id = recPC.Id,
                       TreatmentId = recPC.TreatmentId,
                       PrescriptionId = recPC.PrescriptionId,
                       PrescriptionName = recPC.Prescription.Name,
                       Count = recPC.Count,
                   }).ToList()
           }).ToList();
                return result;
            }
        }

        public List<TreatmentViewModel> GetPatientList(int PatientId)
        {
            using (var context = new DatabaseContext())
            {
                List<TreatmentViewModel> result = context.Treatments.
               Where(rec => rec.PatientId == PatientId).
               Select(rec => new TreatmentViewModel
               {
                   Id = rec.Id,
                   Date = rec.Date,
                   PatientId = rec.PatientId,
                   Name = rec.Name,
                   TotalPrice = rec.TotalPrice,
                   IsReserved = rec.IsReserved,
                   TreatmentPrescriptions = context.TreatmentPrescriptions
                   .Where(recPC => recPC.TreatmentId == rec.Id)
                   .Select(recPC => new TreatmentPrescriptionViewModel
                   {
                       Id = recPC.Id,
                       TreatmentId = recPC.TreatmentId,
                       PrescriptionId = recPC.PrescriptionId,
                       PrescriptionName = recPC.Prescription.Name,
                       Count = recPC.Count,
                   }).ToList()
               }).ToList();
                return result;
            }
        }

        public TreatmentViewModel GetTreatment(int id)
        {
            using (var context = new DatabaseContext())
            {

            }
        }

        public void MedicationRefill(RequestMedicationBindingModel model)
        {
            using (var context = new DatabaseContext())
            {

            }
        }

        public DateTime TreatmentReservation(int id)
        {
            using (var context = new DatabaseContext())
            {

            }
        }

        public void UpdTreatment(TreatmentBindingModel model)
        {
            using (var context = new DatabaseContext())
            {

            }
        }
    }
}
