﻿using ClinicBusinessLogic.BindingModels;
using ClinicBusinessLogic.Interfaces;
using ClinicBusinessLogic.ViewModels;
using ClinicImplementation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClinicImplementation.Implementations
{
    public class MedicationLogic : IMedicationLogic
    {
        public void AddElement(MedicationBindingModel model)
        {
            using(var context = new DatabaseContext())
            {
                Medication element = context.Medications.FirstOrDefault(rec => rec.Name ==
                    model.Name);
                if (element != null)
                {
                    throw new Exception("Уже есть компонент с таким названием");
                }
                context.Medications.Add(new Medication
                {
                    Name = model.Name,
                    Price = model.Price,
                    Count = model.Count
                });
                context.SaveChanges();
            }
        }

        public void DelElement(int id)
        {
            using (var context = new DatabaseContext())
            {
                Medication element = context.Medications.FirstOrDefault(rec => rec.Id == id);
                if (element != null)
                {
                    context.Medications.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        public MedicationViewModel GetElement(int id)
        {
            using (var context = new DatabaseContext())
            {
                Medication element = context.Medications.FirstOrDefault(rec => rec.Id == id);
                if (element != null)
                {
                    return new MedicationViewModel
                    {
                        Id = element.Id,
                        Name = element.Name,
                        Price = element.Price,
                        Count = element.Count
                    };
                }
                throw new Exception("Элемент не найден");
            }
        }

        public List<MedicationViewModel> GetList()
        {
            using (var context = new DatabaseContext())
            {
                List<MedicationViewModel> result = context.Medications.Select(rec => 
                    new MedicationViewModel
                    {
                        Id = rec.Id,
                        Name = rec.Name,
                        Price = rec.Price,
                        Count = rec.Count
                    })
                    .ToList();
                return result;
            }
        }

        public List<MedicationViewModel> GetMostList()
        {
            using (var context = new DatabaseContext())
            {
                List<ReportViewModel> list = new List<ReportViewModel>();
                // проходим по всем лечениям, для которых зарезервированы лекарства
                foreach (var tr in context.Treatments.Where(rec => rec.IsReserved))
                {
                    // ищем для них соответствия в таблице связей с рецептами 
                    foreach (var tPres in context.TreatmentPrescriptions.Where(rec => rec.TreatmentId == tr.Id))
                    {
                        // ищем соответствия в таблице связей с лекарствами
                        foreach (var med in context.MedicationPrescriptions.Where(rec =>
                                rec.PrescriptionId == tPres.PrescriptionId))
                        {
                            list.Add(new ReportViewModel
                            {
                                FIO = context.Patients.FirstOrDefault(rec => rec.Id == tr.PatientId).FIO,
                                Name = tr.Name,
                                Date = tr.Date.ToShortDateString(),
                                MedicationName = med.MedicationName,
                                // рассчитываем нужное кол-во лекарств
                                MedicationCount = med.Count * tPres.Count
                            });
                        }
                    }
                }
                // группируем по названию лекарства
                var groupMed = list.GroupBy(rec => rec.MedicationName)
                                        // выбираем название лекарства и кол-во
                                        .Select(rec => new
                                        {
                                            MedicationName = rec.Key,
                                            Count = rec.Sum(r => r.MedicationCount)
                                        })
                                        .OrderByDescending(rec => rec.Count)
                                        .ToList();

                List<MedicationViewModel> result = new List<MedicationViewModel>();
                foreach (var med in groupMed)
                {
                    // из лекарств выбираем те, которые есть в группировке лекарств
                    var medication = context.Medications.FirstOrDefault(rec => rec.Name == med.MedicationName);
                    result.Add(new MedicationViewModel
                    {
                        Id = medication.Id,
                        Name = medication.Name,
                        Price = medication.Price,
                        Count = medication.Count
                    });
                }
                // ищем в таблице с лекарствами лекарства из результата
                foreach (var element in context.Medications)
                {
                    bool check = false;
                    foreach (var med in result)
                    {
                        if (element.Id == med.Id)
                        {
                            check = true;
                        }
                    }
                    // если не нашли, добавляем к результату текущее лекарство
                    if (!check)
                    {
                        result.Add(new MedicationViewModel
                        {
                            Id = element.Id,
                            Name = element.Name,
                            Price = element.Price,
                            Count = element.Count
                        });
                    }
                }

                return result;
            }
        }

        public void UpdElement(MedicationBindingModel model)
        {
            using (var context = new DatabaseContext())
            {
                Medication element = context.Medications.FirstOrDefault(rec => rec.Name ==
                        model.Name && rec.Id != model.Id);
                if (element != null)
                {
                    throw new Exception("Уже есть компонент с таким названием");
                }
                element = context.Medications.FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                element.Name = model.Name;
                element.Price = model.Price;
                element.Count = model.Count;
                context.SaveChanges();
            }
        }
    }
}
