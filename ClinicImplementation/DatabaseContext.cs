using ClinicImplementation.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicImplementation
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() { } // ВРЕМЕННО
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Data Source=MSI\SQLEXPRESS;Initial Catalog=ClinicDatabase;Integrated Security=True;MultipleActiveResultSets=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }
        public virtual DbSet<Patient> Patients { get; set; }

        public virtual DbSet<Request> Requests { get; set; }

        public virtual DbSet<Treatment> Treatments { get; set; }

        public virtual DbSet<Medication> Medications { get; set; }

        public virtual DbSet<Prescription> Prescriptions { get; set; }

        public virtual DbSet<MedicationPrescription> MedicationPrescriptions { get; set; }

        public virtual DbSet<TreatmentPrescription> TreatmentPrescriptions { get; set; }

        public virtual DbSet<RequestMedication> RequestMedications { get; set; }
    }
}
