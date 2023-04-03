using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace HeroForge_OnceAgain.Utils
{   
    public static class LocalizationUtils
    {
        public static string L(string key)
        {
            CultureInfo cultureInfo = Localization();//CultureInfo.CurrentCulture;
            ResourceManager resourceManager = new ResourceManager("HeroForge_OnceAgain.Properties.Resources", typeof(LocalizationUtils).Assembly);

            string text = resourceManager.GetString(key, cultureInfo);
            return text;
        }

        public static CultureInfo Localization()
        {
            switch (Properties.Settings.Default.LanguageIndex)
            {
                case 0:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
                    break;
                case 1:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt-BR");
                    break;
            }
            return Thread.CurrentThread.CurrentUICulture;
        }
    }
}
