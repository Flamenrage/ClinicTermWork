using ClinicBusinessLogic.BindingModels;
using ClinicBusinessLogic.Interfaces;
using ClinicBusinessLogic.ViewModels;
using ClinicImplementation.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClinicImplementation.Implementations
{
    public class PrescriptionLogic : IPrescriptionLogic
    {
        public List<PrescriptionViewModel> GetList()
        {
            using (var context = new DatabaseContext())
            {
                List<PrescriptionViewModel> result = context.Prescriptions.Select(rec => new PrescriptionViewModel
                {
                    Id = rec.Id,
                    Name = rec.Name,
                    TotalPrice = rec.TotalPrice,
                    PrescriptionMedications = context.MedicationPrescriptions
                    .Where(recPM => recPM.PrescriptionId == rec.Id)
                    .Select(recPM => new PrescriptionMedicationViewModel
                    {
                        Id = recPM.Id,
                        PrescriptionId = recPM.PrescriptionId,
                        MedicationId = recPM.MedicationId,
                        MedicationName = recPM.MedicationName,
                        Count = recPM.Count
                    }).ToList()
                }).ToList();
                return result;
            }
        }

        public List<PrescriptionViewModel> GetPatientList(int PatientId)
        {
            using (var context = new DatabaseContext())
            {
                var unitedPrescriptons = context.TreatmentPrescriptions // рецепты по лечениям пациента
                                    .Include(rec => rec.Prescription)
                                    .Include(rec => rec.Treatment)
                                    .Where(rec => rec.Treatment.PatientId == PatientId) // выбираем все рецепты,
                                                                                        //  относящиеся к лечению пациента по Id
                                    .Select(rec => new PrescriptionViewModel
                                    {
                                        Id = rec.PrescriptionId,
                                        Name = rec.PrescriptionName,
                                        TotalPrice = rec.Count
                                    })
                                    // группировать по Id рецепта
                                    .GroupBy(rec => rec.Id)
                                    .Select(rec => new
                                    {
                                        PrescriptionId = rec.Key,
                                        Count = rec.Sum(r => r.TotalPrice) //считываем Id и цену группы
                                    })
                                    .OrderByDescending(rec => rec.Count)
                                    .ToList();

                List<PrescriptionViewModel> result = new List<PrescriptionViewModel>();
                foreach (var element in unitedPrescriptons)
                {
                    // выбираем из таблицы рецептов первый попавшийся, который содержится в таблице связей между рецептами и лечениями
                    var pr = context.Prescriptions.FirstOrDefault(rec => rec.Id == element.PrescriptionId);
                    result.Add(new PrescriptionViewModel
                    {
                        Id = pr.Id,
                        Name = pr.Name,
                        TotalPrice = pr.TotalPrice,
                        // отбираем все лекарства, относящиеся к рецепту, используя таблицу связей между ними для поиска
                        PrescriptionMedications = context.MedicationPrescriptions
                                                  .Where(recMP => recMP.PrescriptionId == pr.Id)
                                                  .Select(MP => new PrescriptionMedicationViewModel
                                                  {
                                                      Id = MP.Id,
                                                      PrescriptionId = MP.PrescriptionId,
                                                      MedicationId = MP.MedicationId,
                                                      MedicationName = MP.MedicationName,
                                                      Count = MP.Count
                                                  }).ToList()
                    });
                }
                foreach (var elem in context.Prescriptions)
                {
                    bool check = false;
                    foreach (var pre in result)
                    {
                        if (elem.Id == pre.Id)
                        {
                            check = true;
                        }
                    }
                    if (!check)
                    {
                        result.Add(new PrescriptionViewModel
                        {
                            Id = elem.Id,
                            Name = elem.Name,
                            TotalPrice = elem.TotalPrice,
                            PrescriptionMedications = context.MedicationPrescriptions
                                .Where(recMP => recMP.PrescriptionId == elem.Id)
                                .Select(recMP => new PrescriptionMedicationViewModel
                                {
                                    Id = recMP.Id,
                                    PrescriptionId = recMP.PrescriptionId,
                                    MedicationId = recMP.MedicationId,
                                    MedicationName = recMP.MedicationName,
                                    Count = recMP.Count
                                }).ToList()
                        });
                    }
                }
                return result;
            }
        }

        public List<PrescriptionViewModel> GetAvailableList()
        {
            using (var context = new DatabaseContext())
            {
                // создаем для каждого рецепта из результата свою модель и группируем в лист
                List<PrescriptionViewModel> result = context.Prescriptions.Select(rec => new PrescriptionViewModel
                {
                    Id = rec.Id,
                    Name = rec.Name,
                    TotalPrice = rec.TotalPrice,
                    PrescriptionMedications = context.MedicationPrescriptions
                    .Where(recPM => recPM.PrescriptionId == rec.Id)
                    .Select(recPM => new PrescriptionMedicationViewModel
                    {
                        Id = recPM.Id,
                        PrescriptionId = recPM.PrescriptionId,
                        MedicationId = recPM.MedicationId,
                        MedicationName = recPM.MedicationName,
                        Count = recPM.Count
                    }).ToList()
                }).ToList();

                List<PrescriptionViewModel> res = new List<PrescriptionViewModel>();
                foreach (var element in result)
                {
                    bool check = false;
                    //Ищем по таблице связей между лекарствами и рецептами
                    foreach (var medication in element.PrescriptionMedications)
                    {
                        //если количество лекарств в рецепте не превышает кол-во доступных лекарств
                        if (medication.Count <= context.Medications.FirstOrDefault(rec => rec.Id == medication.MedicationId).Count)
                        {
                            check = true;
                        }
                    }
                    if (check) //если достаточно, то добавляем в список рецептов
                    {
                        res.Add(element);
                    }
                }
                return res;
            }
        }

        public PrescriptionViewModel GetElement(int id)
        {
            using (var context = new DatabaseContext())
            {
                Prescription element = context.Prescriptions.FirstOrDefault(rec => rec.Id == id);
                if (element != null)
                {
                    return new PrescriptionViewModel
                    {
                        Id = element.Id,
                        Name = element.Name,
                        TotalPrice = element.TotalPrice,
                        PrescriptionMedications = context.MedicationPrescriptions
                            .Where(recMP => recMP.PrescriptionId == element.Id)
                            .Select(MP => new PrescriptionMedicationViewModel
                            {
                                Id = MP.Id,
                                PrescriptionId = MP.PrescriptionId,
                                MedicationId = MP.MedicationId,
                                MedicationName = MP.MedicationName,
                                Count = MP.Count
                            }).ToList()
                    };
                }
                throw new Exception("Элемент не найден");
            }
        }

        public void AddElement(PrescriptionBindingModel model)
        {
            using (var context = new DatabaseContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Prescription element = context.Prescriptions.FirstOrDefault(rec =>
                        rec.Name == model.Name);
                        if (element != null)
                        {
                            throw new Exception("Уже есть рецепт с таким названием");
                        }
                        element = new Prescription
                        {
                            Name = model.Name,
                            TotalPrice = model.TotalPrice
                        };
                        context.Prescriptions.Add(element);
                        context.SaveChanges();
                        // избавляемся от дублей по лекарствам
                        var groupMedications = model.PrescriptionMedications
                                                    .GroupBy(rec => rec.MedicationId)
                                                    .Select(rec => new
                                                    {
                                                        MedicationId = rec.Key,
                                                        Count = rec.Sum(r => r.Count)
                                                    });
                        var medicationName = model.PrescriptionMedications.Select(rec => new
                        {
                            MedicationId = rec.MedicationId,
                            MedicationName = rec.MedicationName
                        });
                        // добавляем лекарства
                        foreach (var groupMedication in groupMedications)
                        {
                            string Name = null;
                            foreach (var medication in medicationName)
                            {
                                if (groupMedication.MedicationId == medication.MedicationId)
                                {
                                    Name = medication.MedicationName;
                                }
                            }
                            context.MedicationPrescriptions.Add(new MedicationPrescription
                            {
                                PrescriptionId = element.Id,
                                MedicationId = groupMedication.MedicationId,
                                MedicationName = Name,
                                Count = groupMedication.Count
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

        public void UpdElement(PrescriptionBindingModel model)
        {
            using (var context = new DatabaseContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Prescription element = context.Prescriptions.FirstOrDefault(rec =>
                        rec.Name == model.Name && rec.Id != model.Id);
                        if (element != null)
                        {
                            throw new Exception("Уже есть рецепт с таким названием");
                        }
                        element = context.Prescriptions.FirstOrDefault(rec => rec.Id == model.Id);
                        if (element == null)
                        {
                            throw new Exception("Элемент не найден");
                        }
                        element.Name = model.Name;
                        element.TotalPrice = model.TotalPrice;
                        context.SaveChanges();
                        // обновляем существуюущие лекарства
                        var presIds = model.PrescriptionMedications.Select(rec =>
                        rec.MedicationId).Distinct();
                        var updateMedications = context.MedicationPrescriptions.Where(rec =>
                        rec.PrescriptionId == model.Id && presIds.Contains(rec.MedicationId));
                        foreach (var updateMedication in updateMedications)
                        {
                            updateMedication.Count =
                            model.PrescriptionMedications.FirstOrDefault(rec => rec.Id == updateMedication.Id).Count;
                        }
                        context.SaveChanges();
                        context.MedicationPrescriptions.RemoveRange(context.MedicationPrescriptions.Where(rec =>
                        rec.PrescriptionId == model.Id && !presIds.Contains(rec.MedicationId)));
                        context.SaveChanges();
                        // новые записи
                        var groupMedications = model.PrescriptionMedications
                                                    .Where(rec => rec.Id == 0)
                                                    .GroupBy(rec => rec.MedicationId)
                                                    .Select(rec => new
                                                    {
                                                        MedicationId = rec.Key,
                                                        Count = rec.Sum(r => r.Count)
                                                    });
                        foreach (var groupMedication in groupMedications)
                        {
                            MedicationPrescription elementPC =
                            context.MedicationPrescriptions.FirstOrDefault(rec => rec.PrescriptionId == model.Id &&
                            rec.MedicationId == groupMedication.MedicationId);
                            if (elementPC != null)
                            {
                                elementPC.Count += groupMedication.Count;
                                context.SaveChanges();
                            }
                            else
                            {
                                context.MedicationPrescriptions.Add(new MedicationPrescription
                                {
                                    PrescriptionId = (int)model.Id,
                                    MedicationId = groupMedication.MedicationId,
                                    Count = groupMedication.Count
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

        public void DelElement(int id)
        {
            using (var context = new DatabaseContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Prescription element = context.Prescriptions.FirstOrDefault(rec => rec.Id ==
                        id);
                        if (element != null)
                        {
                            // удаяем записи по лекарствам при удалении рецепта
                            context.MedicationPrescriptions.RemoveRange(context.MedicationPrescriptions.Where(rec =>
                            rec.PrescriptionId == id));
                            context.Prescriptions.Remove(element);
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
    }
}
