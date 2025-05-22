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
        //public string OriginalName { get; set; }
        public string ShortDescription { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Subtype { get; set; }

        public double? HD { get; set; }
        public double? Land { get; set; }
        public double? Burrow { get; set; }
        public double? Climb { get; set; }
        public double? Fly { get; set; }
        public string Maneuver { get; set; }
        public double? Swim { get; set; }

        public double? NaturalArmor { get; set; }
        public string NaturalAttacks { get; set; }
        public string SpecialAttacks { get; set; }
        public string SpellLikeAbilities { get; set; }
        public string PsionicAbilities { get; set; }
        public string OtherSpecialAbilities { get; set; }

        public string LowLightVision { get; set; }
        public string Darkvision { get; set; }
        public string OtherSenses { get; set; }
        public string Immunities { get; set; }
        public string Vulnerabilities { get; set; }
        public string EnergyResistance { get; set; }
        public string SpellResistance { get; set; }
        public string DamageReduction { get; set; }
        public string FastHealing { get; set; }
        public string BonusEssentia { get; set; }
        public string OtherSpecialQualities { get; set; }

        public double? Str { get; set; }
        public double? Dex { get; set; }
        public double? Con { get; set; }
        public double? Int { get; set; }
        public double? Wis { get; set; }
        public double? Cha { get; set; }

        public string RacialSkills { get; set; }
        public string BonusFeats { get; set; }

        public string AutomaticLanguages { get; set; }
        public string BonusLanguages { get; set; }

        public double? CRAdj { get; set; }
        public string Alignment { get; set; }
        public double? LevelAdj { get; set; }

        public string FavoredClass { get; set; }
        public string BaseAge { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }

        public string PrerequisitesDescription { get; set; }
        public double? Prereq { get; set; }
        public int? Index { get; set; }

        public double? LGOpen { get; set; }
        public string Src { get; set; }
        public double? Pg { get; set; }
        public string AltSrc { get; set; }
        public double? SrcSel { get; set; }
        public double? Selected { get; set; }
        public double? Qualified { get; set; }

        public string Lang { get; set; }
    }


    public static class RaceUtils
    {
        private static List<Race> _races;

        //public static List<Race> GetRaces()
        //{
        //    if (_races == null)
        //    {
        //        string fileName = "RaceInfo.json"; // novo arquivo
        //        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "RaceInfo.json");

        //        if (File.Exists(path))
        //        {
        //            string json = File.ReadAllText(path);

        //            var races = JsonConvert.DeserializeObject<List<Race>>(json);
        //            string languageCode = Properties.Settings.Default.LanguageCode ?? "en";

        //            _races = races
        //                .Where(r => r.Lang == languageCode)
        //                .OrderBy(r => r.DisplayName) // ou `r.Id`, se houver
        //                .ToList();
        //        }
        //        else
        //        {
        //            MessageBox.Show($"Arquivo não encontrado: {path}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            _races = new List<Race>();
        //        }
        //    }

        //    return _races;
        //}
        public static List<Race> GetRaces()
        {
            if (_races == null)
            {
                string fileName = "Races.json";
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Translations", "races", fileName);

                if (!File.Exists(path))
                {
                    MessageBox.Show($"Arquivo não encontrado: {path}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _races = new List<Race>();
                    return _races;
                }

                string json = File.ReadAllText(path);
                var raceDict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);

                string languageCode = Properties.Settings.Default.LanguageCode ?? "en";

                _races = raceDict
                    .Select(kvp =>
                    {
                        int id = int.Parse(kvp.Key);
                        var values = kvp.Value;
                        string name = values.ContainsKey(languageCode) && !string.IsNullOrWhiteSpace(values[languageCode])
                            ? values[languageCode]
                            : values["en"];

                        return new Race
                        {
                            Id = id,
                            DisplayName = name
                        };
                    })
                    .OrderBy(r => r.DisplayName)
                    .ToList();
            }

            return _races;
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

        public static void PopulateRaceComboBox(ComboBox comboBox, int languageIndex)
        {
            string languageCode = GetLanguageCode(languageIndex); // exemplo: "pt-BR"
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Translations", "races", "Races.json");

            if (!File.Exists(path))
            {
                MessageBox.Show("Arquivo de raças não encontrado.");
                return;
            }

            string json = File.ReadAllText(path);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);

            var raceList = dict.Select(kvp =>
                new RaceDisplayItem
                {
                    Id = int.Parse(kvp.Key),
                    DisplayName = kvp.Value.ContainsKey(languageCode) && !string.IsNullOrWhiteSpace(kvp.Value[languageCode])
                                    ? kvp.Value[languageCode]
                                    : kvp.Value["en"]
                }
            ).OrderBy(r => r.DisplayName).ToList();

            comboBox.DisplayMember = "DisplayName";
            comboBox.ValueMember = "Id";
            comboBox.DataSource = raceList;
        }
        private static string GetLanguageCode(int index)
        {
            switch (index)
            {
                case 0: return "en";
                case 1: return "pt-BR";
                case 2: return "es";
                case 3: return "fr";
                case 4: return "it";
                default: return "en";
            }
        }
        public class RaceDisplayItem
        {
            public int Id { get; set; }
            public string DisplayName { get; set; }
        }

    }
}

