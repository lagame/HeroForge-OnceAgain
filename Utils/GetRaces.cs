using HeroForge_OnceAgain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using static HeroForge_OnceAgain.Form1;

namespace HeroForge_OnceAgain.Utils
{
    public static class GetListRaces
    {
        public static List<Race> Races { get; } = new List<Race>();
    }

    public class Race
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string OriginalName { get; set; }
        public string Lang { get; set; }
    }

    public static class RaceUtils
    {
        private static List<Race> _races;

        public static List<Race> GetRaces()
        {
            //if (_races == null)
            //{
                string path = Path.Combine(System.Windows.Forms.Application.StartupPath.Replace("\\bin\\Debug", ""), "Resources\\races.json");
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);

                    var races = JsonConvert.DeserializeObject<List<Race>>(json);
                    var languageCode = Properties.Settings.Default.LanguageIndex == 0 ? "en" : "pt-BR";
                    var filteredRaces = races.Where(c => c.Lang == languageCode).ToList();
                    _races = filteredRaces.OrderBy(r => r.Id).ToList();
                }
                return _races;
            //}else { 
            //    return _races; 
            //}
            
        }

        public static Race GetOriginalRace(string raceText)
        {
            Race race = null;
                string path = Path.Combine(System.Windows.Forms.Application.StartupPath.Replace("\\bin\\Debug", ""), "Resources\\races.json");
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);

                    var races = JsonConvert.DeserializeObject<List<Race>>(json);
                    var languageCode = Properties.Settings.Default.LanguageIndex == 0 ? "en" : "pt-BR";

                    race = races
                        .Where(c => c.DisplayName == raceText)
                        .Where(c => c.Lang == languageCode).FirstOrDefault();
                    
                }
                return race;          

        }

        public static void PopulateRaceComboBox(ComboBox cbRace, int selectedValue = 3)
        {
            var races = GetRaces();
            cbRace.DataSource = races;
            cbRace.DisplayMember = "DisplayName";
            cbRace.ValueMember = "Id";
            cbRace.SelectedValue = selectedValue;
        }
    }
}

