using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;
using ClinicBusinessLogic.BindingModels;
using ClinicBusinessLogic.Interfaces;
using ClinicImplementation;
using ClinicImplementation.Implementations;
using Unity.Lifetime;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace ClinicClientView.App_Start
{
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });
        public static IUnityContainer Container => container.Value;
        #endregion
        public static void RegisterTypes(IUnityContainer container)
        {
            
            container.RegisterType<DbContext, DatabaseContext>(
                new HierarchicalLifetimeManager());
            container.RegisterType<IPatientLogic, PatientLogic>(
                new HierarchicalLifetimeManager());
            container.RegisterType<IRequestLogic, RequestLogic>(
                new HierarchicalLifetimeManager());
            container.RegisterType<IMedicationLogic, MedicationLogic>(
                new HierarchicalLifetimeManager());
            container.RegisterType<IPrescriptionLogic, PrescriptionLogic>(
                new HierarchicalLifetimeManager());
            container.RegisterType<IMainLogic, MainLogic>(
                new HierarchicalLifetimeManager());
            container.RegisterType<IReportLogic, ReportLogic>(
                new HierarchicalLifetimeManager());
        }
    }
}