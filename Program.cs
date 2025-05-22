using HeroForge_OnceAgain.Infrastructure.Database;
using HeroForge_OnceAgain.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static HeroForge_OnceAgain.Migrations.Configuration;

namespace HeroForge_OnceAgain
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            HttpListenerService.Instance.StartListener();

            using (var context = new ApplicationDbContext())
            {
                SeedHelper.SeedData(context); // Agora é acessível
                context.SaveChanges();
            }

            Application.Run(new FrmLogin());
        }
    }
}
