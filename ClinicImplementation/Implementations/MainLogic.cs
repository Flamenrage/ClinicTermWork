using ClinicBusinessLogic.BindingModels;
using ClinicBusinessLogic.Interfaces;
using ClinicBusinessLogic.ViewModels;
using ClinicImplementation.Models;
using System;
using System.Collections.Generic;
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
                Treatment element = context.Treatments.FirstOrDefault(rec => rec.Id == id);
                if (element != null)
                {
                    return new TreatmentViewModel
                    {
                        Id = element.Id,
                        Date = element.Date,
                        PatientId = element.PatientId,
                        Name = element.Name,
                        TotalPrice = element.TotalPrice,
                        IsReserved = element.IsReserved,
                        TreatmentPrescriptions = context.TreatmentPrescriptions
                        .Where(recPC => recPC.TreatmentId == element.Id)
                        .Select(recPC => new TreatmentPrescriptionViewModel
                        {
                            Id = recPC.Id,
                            TreatmentId = recPC.TreatmentId,
                            PrescriptionId = recPC.PrescriptionId,
                            PrescriptionName = recPC.Prescription.Name,
                            Count = recPC.Count,
                        }).ToList()
                    };
                }
                throw new Exception("Элемент не найден");
            }
        }

        public void MedicationRefill(RequestMedicationBindingModel model)
        {
            using (var context = new DatabaseContext())
            {
                RequestMedication element = context.RequestMedications.FirstOrDefault(rec => rec.RequestId == model.RequestId && rec.MedicationId == model.MedicationId);

                if (element != null)
                {
                    element.Count += model.Count;
                }
                else
                {
                    context.RequestMedications.Add(new RequestMedication
                    {
                        MedicationId = model.MedicationId,
                        MedicationName = model.MedicationName,
                        RequestId = model.RequestId,
                        Count = model.Count
                    });
                }
                context.Medications.FirstOrDefault(res => res.Id == model.MedicationId).Count += model.Count;
                context.SaveChanges();
            }
        }

        public DateTime TreatmentReservation(int id)
        {
            using (var context = new DatabaseContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // ищем первое лечение, Id которого равен искомому
                        Treatment element = context.Treatments.FirstOrDefault(rec => rec.Id == id);
                        if (element == null)
                        {
                            throw new Exception("Элемент не найден");
                        }
                        if (element.IsReserved)
                        {
                            throw new Exception("Лекарства по этому лечению уже зарезервированы");
                        }
                        else
                        {
                            element.IsReserved = true;
                        }
                        List<PrescriptionMedicationViewModel> presMed = new List<PrescriptionMedicationViewModel>();
                        // из таблицы связи лечения с рецептом выбираем те, у которых лечение совпадает с искомым
                        var treatmentPrescriptions = context.TreatmentPrescriptions
                            .Where(rec => rec.TreatmentId == element.Id)
                            .Select(rec => new TreatmentPrescriptionViewModel
                            {
                                PrescriptionId = rec.PrescriptionId,
                                Count = rec.Count
                            });
                        foreach (var trPres in treatmentPrescriptions)
                        {
                            // из таблицы связей лекарства с рецептом выбираем те, 
                            //где id лекарства совпадает с искомым
                            var medicationPrescriptions = context.MedicationPrescriptions
                                .Where(rec => rec.PrescriptionId == trPres.PrescriptionId)
                                .Select(rec => new PrescriptionMedicationViewModel
                                {
                                    MedicationId = rec.MedicationId,
                                    Count = rec.Count
                                });
                            // подсчитываем количество лекарств по рецептам
                            foreach (var medPres in medicationPrescriptions)
                            {
                                bool flag = false;
                                for (int i = 0; i < presMed.Count(); i++)
                                {
                                    if (presMed[i].MedicationId == medPres.MedicationId)
                                    {
                                        presMed[i].Count += medPres.Count;
                                        flag = true;
                                    }
                                }
                                if (!flag)
                                {
                                    presMed.Add(medPres);
                                    presMed.Last().Count = medPres.Count * trPres.Count;
                                }
                            }
                        }
                        var medications = context.Medications.Select(rec => new MedicationViewModel
                        {
                            Id = rec.Id,
                            Count = rec.Count
                        }).ToList();
                        // бронируем лекарства
                        for (int i = 0; i < presMed.Count(); i++)
                        {
                            var index = presMed[i].MedicationId;
                            var Med = context.Medications.Where(rec => rec.Id == index);
                            foreach (var med in Med)
                            {
                                if (presMed[i].Count <= med.Count)
                                {
                                    med.Count -= presMed[i].Count;
                                    context.SaveChanges();
                                }
                                else
                                {
                                    throw new Exception("Это лечение пока не доступно для бронирования");
                                }
                            }
                        }
                        context.SaveChanges();
                        transaction.Commit();
                        return element.Date;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void UpdTreatment(TreatmentBindingModel model)
        {
            using (var context = new DatabaseContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // проверяем уникальность
                        Treatment element = context.Treatments.FirstOrDefault(rec =>
                            (rec.Name == model.Name) && (rec.Id != model.Id));
                        if (element != null)
                        {
                            throw new Exception("Уже есть лечение с таким названием");
                        }
                        element = context.Treatments.FirstOrDefault(rec => rec.Id == model.Id);
                        if (element == null)
                        {
                            throw new Exception("Элемент не найден");
                        }
                        element.Name = model.Name;
                        element.TotalPrice = (int)model.TotalPrice;
                        context.SaveChanges();

                        // обновляем существующие рецепты
                        // получаем неповторяющиеся Id рецептов в таблице связей
                        var presIds = model.TreatmentPrescriptions.Select(rec => rec.PrescriptionId).Distinct();
                        var updatePrescriptions = context.TreatmentPrescriptions.Where(rec =>
                            (rec.TreatmentId == model.Id) && (presIds.Contains(rec.PrescriptionId)));
                        foreach (var updatePrescription in updatePrescriptions)
                        {
                            updatePrescription.Count = model.TreatmentPrescriptions.FirstOrDefault(rec =>
                                rec.Id == updatePrescription.Id).Count;
                        }
                        context.SaveChanges();
                        // обновляем связующую таблицу с рецептами
                        context.TreatmentPrescriptions.RemoveRange(context.TreatmentPrescriptions.Where(rec =>
                            (rec.TreatmentId == model.Id) && !(presIds.Contains(rec.PrescriptionId))));
                        context.SaveChanges();
                        // новые записи  
                        var groupPrescriptions = model.TreatmentPrescriptions
                            .Where(rec => rec.Id == 0)
                            .GroupBy(rec => rec.PrescriptionId)
                            .Select(rec => new
                            {
                                PrescriptionId = rec.Key,
                                Count = rec.Sum(r => r.Count)
                            });
                        foreach (var groupPrescription in groupPrescriptions)
                        {
                            TreatmentPrescription elementTP = context.TreatmentPrescriptions.FirstOrDefault(rec =>
                                rec.TreatmentId == model.Id && 
                                rec.PrescriptionId == groupPrescription.PrescriptionId);
                            if (elementTP != null)
                            {
                                elementTP.Count += groupPrescription.Count;
                                context.SaveChanges();
                            }
                            else
                            {
                                context.TreatmentPrescriptions.Add(new TreatmentPrescription
                                {
                                    TreatmentId = (int)model.Id,
                                    PrescriptionId = groupPrescription.PrescriptionId,
                                    Count = groupPrescription.Count
                                });
                                context.SaveChanges();
                            }
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
    }
}
