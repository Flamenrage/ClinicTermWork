using ClinicBusinessLogic.BindingModels;
using ClinicBusinessLogic.Interfaces;
using ClinicImplementation;
using ClinicImplementation.Implementations;
using ClinicImplementation.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;

namespace ClinicAdministrationView
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);            
            Application.Run(container.Resolve<FormAuthorization>());
        }
        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<DbContext, DatabaseContext>(
                new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IPatientLogic, PatientLogic>(
                new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IRequestLogic, RequestLogic>(
                new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMedicationLogic, MedicationLogic>(
                new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IPrescriptionLogic, PrescriptionLogic>(
                new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMainLogic, MainLogic>(
                new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IReportLogic, ReportLogic>(
                new HierarchicalLifetimeManager());
            return currentContainer;
        }
    }
}
