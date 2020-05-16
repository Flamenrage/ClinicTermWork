using ClinicImplementation;
using ClinicImplementation.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DatabaseContext context = new DatabaseContext();
            context.Patients.Add(new Patient
            {
                FIO = "sakljasf",
                Email = "oipoip",
                Password = "12345"
            });
            context.SaveChanges();
            var query = context.Patients.First();
            Debug.WriteLine(query.Password.ToString());
            Application.Run(new Form1());
        }
    }
}
