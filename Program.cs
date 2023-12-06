using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            HttpListenerService.Instance.StartListener(); // Iniciando o listener aqui           

            //Application.Run(new Form1());
            //Application.Run(new Form1());
            Application.Run(FrmLogin.Default);
        }
    }
}
