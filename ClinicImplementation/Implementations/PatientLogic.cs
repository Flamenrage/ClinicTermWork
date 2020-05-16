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
    public class PatientLogic : IPatientLogic
    {
        public List<PatientViewModel> GetList()
        {
            using (var context = new DatabaseContext())
            {
                List<PatientViewModel> result = context.Patients.Select(rec =>
                new PatientViewModel
                {
                    Id = rec.Id,
                    FIO = rec.FIO,
                    Email = rec.Email,
                    Password = rec.Password
                })
                .ToList();
                return result;
            }
        }

        public PatientViewModel GetElement(string email, string password)
        {
            using (var context = new DatabaseContext())
            {
                Patient element = context.Patients.FirstOrDefault(rec => rec.Email == email && rec.Password == password);

                if (element != null)
                {
                    return new PatientViewModel
                    {
                        Id = element.Id,
                        FIO = element.FIO,
                        Email = element.Email,
                        Password = element.Password
                    };
                }
                throw new Exception("Элемент не найден");
            }
        }
        public void AddElement(PatientBindingModel model)
        {
            using (var context = new DatabaseContext())
            {
                Patient element = context.Patients.FirstOrDefault(rec => rec.Email == model.Email);

                if (element != null)
                {
                    throw new Exception("Уже есть пациент с такой почтой");
                }
                context.Patients.Add(new Patient
                {
                    FIO = model.FIO,
                    Email = model.Email,
                    Password = model.Password
                });
                context.SaveChanges();
            }
        }
    }
}
