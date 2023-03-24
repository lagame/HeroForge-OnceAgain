using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Resources;

namespace HeroForge_OnceAgain.Utils
{   
    public static class LocalizationUtils
    {
        public static string L(string key)
        {
            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            ResourceManager resourceManager = new ResourceManager("HeroForge_OnceAgain.Properties.Resources", typeof(LocalizationUtils).Assembly);

            string text = resourceManager.GetString(key, cultureInfo);
            return text;
        }
    }
}
