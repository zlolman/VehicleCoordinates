using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using VehicleCoordinates.Repositories;
using VehicleCoordinates.Interfaces;
using VehicleCoordinates.Models;
namespace VehicleCoordinates
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IRepository repository = new SqlRepository();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(repository));
        }
    }
}
