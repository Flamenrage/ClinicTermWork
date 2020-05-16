using ClinicBusinessLogic.BindingModels;
using ClinicBusinessLogic.Interfaces;
using ClinicBusinessLogic.ViewModels;
using ClinicImplementation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClinicImplementation.Implementations
{
    public class RequestLogic : IRequestLogic
    {
        public int AddElement(RequestBindingModel model)
        {
            using (var context = new DatabaseContext())
            {
                int id = -1;
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Request element = context.Requests.FirstOrDefault(rec =>
                            rec.Name == model.Name);
                        if (element != null)
                        {
                            throw new Exception("Уже есть запрос с таким названием");
                        }
                        element = new Request
                        {
                            Name = model.Name,
                            Date = model.Date,
                        };
                        context.Requests.Add(element);
                        context.SaveChanges();
                        var record = context.Requests.OrderByDescending(p => p.Id).Take(1);
                        foreach (var req in record)
                        {
                            id = req.Id;
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                return id;
            }
        }

        public RequestViewModel GetElement(int id)
        {
            using (var context = new DatabaseContext())
            {
                Request element = context.Requests.FirstOrDefault(rec => rec.Id == id);
                if (element != null)
                {
                    return new RequestViewModel
                    {
                        Id = element.Id,
                        Name = element.Name,
                        Date = element.Date,
                        RequestMedications = context.RequestMedications
                            .Where(recPC => recPC.RequestId == element.Id)
                            .Select(recPC => new RequestMedicationViewModel
                            {
                                Id = recPC.Id,
                                MedicationId = recPC.MedicationId,
                                RequestId = recPC.RequestId,
                                MedicationName = recPC.Medication.Name,
                                Count = recPC.Count
                            }).ToList()
                    };
                }
                throw new Exception("Элемент не найден");
            }
        }

        public List<RequestViewModel> GetList()
        {
            using (var context = new DatabaseContext())
            {
                List<RequestViewModel> result = context.Requests.Select(rec =>
                    new RequestViewModel
                    {
                        Id = rec.Id,
                        Name = rec.Name,
                        Date = rec.Date,
                        RequestMedications = context.RequestMedications
                            .Where(recPC => recPC.RequestId == rec.Id)
                            .Select(recPC => new RequestMedicationViewModel
                            {
                                Id = recPC.Id,
                                MedicationId = recPC.MedicationId,
                                RequestId = recPC.RequestId,
                                MedicationName = recPC.Medication.Name,
                                Count = recPC.Count
                            }).ToList()
                    }).ToList();
                return result;
            }
        }
    }
}
