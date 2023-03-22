using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Status;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using ClosedXML.Excel;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections;
using System.Reflection;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using DocumentFormat.OpenXml.Vml.Office;
using HeroForge_OnceAgain.Properties;
using DocumentFormat.OpenXml.InkML;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Office2010.ExcelAc;


namespace HeroForge_OnceAgain
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cBLanguage.SelectedIndex)
            {
                case 0:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
                    break;
                case 1:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt-BR");
                    break;
            }

            SaveChooseLanguage(cBLanguage.Text, cBLanguage.SelectedIndex);

            this.Controls.Clear();
            InitializeComponent();

        }
        private void SaveChooseLanguage(string language, Int32 integer)
        {
            Properties.Settings.Default.Language = language;
            Properties.Settings.Default.LanguageIndex = integer;
            Properties.Settings.Default.Save();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cBLanguage.SelectedIndex = Properties.Settings.Default.LanguageIndex;

            workSheet = LoadFile();

            this.Controls.Clear();
            InitializeComponent();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {


            tabControl1.Size = new Size(this.Size.Width - 40, this.Size.Height - 80);


            //tabControl1.ItemSize = this.Width - 10;
            //tabControl1.Size.Height = this.Height - 10;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.UnableOpenLink);
            }
        }

        private void VisitLink()
        {
            // Change the color of the link text by setting LinkVisited
            // to true.
            linkLabel1.LinkVisited = true;
            //Call the Process.Start method to open the default browser
            //with a URL:
            if (Properties.Settings.Default.LanguageIndex != null && Properties.Settings.Default.LanguageIndex != -1)
            {
                switch (Properties.Settings.Default.LanguageIndex)
                {
                    case 0:
                        System.Diagnostics.Process.Start("https://creativecommons.org/licenses/by-nc-sa/4.0/");
                        break;
                    case 1:
                        System.Diagnostics.Process.Start("https://creativecommons.org/licenses/by-nc-sa/4.0/deed.pt_BR");
                        break;
                }
            }
            else
            {
                MessageBox.Show("Unable to open link that was clicked.");
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!lblRandomName.Text.Equals("__________________"))
            {
                txtName.Text = lblRandomName.Text;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (!lblRandomHair.Text.Equals("__________________"))
            {
                txtHair.Text = lblRandomHair.Text;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (!lblRandomEyes.Text.Equals("__________________"))
            {
                txtEyes.Text = lblRandomEyes.Text;
            }
        }

        private void btRandomAge_Click(object sender, EventArgs e)
        {
            RandomAge();
        }


        private void RandomAge()
        {
            int ageMod = 0, hgtMod = 0, wgtMod = 0;

            string race = lblRace.Text;
            var BaseAgeRace = LookupRace("AR", race); //await LookupRaceAsync("Base Age", raca);
            if (BaseAgeRace != null && !string.IsNullOrEmpty(BaseAgeRace))
            {
                var AgeRace = BaseAgeRace.Split('/').ToList();

                if (!string.IsNullOrEmpty(BaseAgeRace.Trim()))
                {

                    var classe = lblClasses.Text;

                    if (!string.IsNullOrEmpty(classe))
                    {
                        var vetorClasse = classe.Split('/');
                        foreach (var item in vetorClasse)
                        {
                            var item2 = item.ToUpper();
                            if (item2.Contains("BARBARIAN") || item2.Contains("ROGUE") || item2.Contains("SORCERER"))
                            {
                                if (!string.IsNullOrEmpty(AgeRace[0]))
                                {
                                    string AgeString = AgeRace[0];
                                    var Age = AgeString.Split('+').ToList();
                                    int agebasic = Convert.ToInt32(Age[0]);
                                    string Dice = Age[1];
                                    ageMod = RollDice(Dice);
                                    lblRandomAge.Text = (agebasic + ageMod).ToString();
                                }

                            }
                            else if (item2.Contains("BARD") || item2.Contains("FIGHTER") || item2.Contains("PALADIN") || item2.Contains("RANGER"))
                            {
                                string AgeString = AgeRace[0];
                                var Age = AgeString.Split('+').ToList();
                                int agebasic = Convert.ToInt32(Age[0]);

                                string AgeString2 = AgeRace[1];
                                ageMod = RollDice(AgeString2);
                                lblRandomAge.Text = (agebasic + ageMod).ToString();
                            }
                            else if (item2.Contains("CLERIC") || item2.Contains("DRUID") || item2.Contains("MONK") || item2.Contains("WIZARD"))
                            {
                                string AgeString = AgeRace[0];
                                var Age = AgeString.Split('+').ToList();
                                int agebasic = Convert.ToInt32(Age[0]);

                                string AgeString2 = AgeRace[2];
                                ageMod = RollDice(AgeString2);
                                lblRandomAge.Text = (agebasic + ageMod).ToString();
                            }
                        }

                    }
                    else
                    {
                        lblRandomAge.Text = "0";
                    }



                } else
                {
                    lblRandomAge.Text = "0";
                }
            }
        }

        //private int RollDice(int numDice, int dieType)
        //{
        //    Random random = new Random();
        //    int total = 0;
        //    for (int i = 0; i < numDice; i++)
        //    {
        //        total += random.Next(1, dieType + 1);
        //    }
        //    return total;
        //}

        private int RollDice(string diceString)
        {
            if (!string.IsNullOrEmpty(diceString))
            {
                Random random = new Random();
                if (diceString.Contains("d"))
                {
                    var dice = diceString.Split('d').ToList();

                    int numDice = Convert.ToInt32(dice[0]);
                    int dieType = Convert.ToInt32(dice[1]);

                    int total = 0;
                    for (int i = 0; i < numDice; i++)
                    {
                        total += random.Next(1, dieType + 1);
                    }
                    return total;
                }

            }
            else
            {
                return 0;
            }
            return 0;
        }

        //private const int ageNumDice = 2;
        //private const int ageDieType = 4;
        //private const string baseInfoCreatures = "baseInfo.xlsm";

        //static async Task<string> ReadAllTextAsync(string path)
        //{
        //    switch (path)
        //    {
        //        case "": throw new ArgumentException("Empty path name is not legal.", nameof(path));
        //        case null: throw new ArgumentNullException(nameof(path));
        //    }

        //    var sourceStream = new FileStream(path, FileMode.Open,
        //        FileAccess.Read, FileShare.Read,
        //        bufferSize: 4096,
        //        useAsync: true);
        //    var streamReader = new StreamReader(sourceStream, Encoding.UTF8,
        //        detectEncodingFromByteOrderMarks: true);
        //    // detectEncodingFromByteOrderMarks allows you to handle files with BOM correctly. 
        //    // Otherwise you may get chinese characters even when your text does not contain any

        //    return await streamReader.ReadToEndAsync();
        //}

        private XLWorkbook LoadFile()
        {
            string path = System.IO.Path.GetTempFileName();

            System.IO.File.WriteAllBytes(path, Properties.Resources.baseInfo);

            var fileName = Path.ChangeExtension(path, "xlsm");
            File.Copy(path, fileName, true);

            pathfile = path;
            tmpfile = Path.ChangeExtension(path, "xlsm");
            var workbook = new XLWorkbook(tmpfile);
            return workbook;
        }

        private IXLWorksheet getTable(string tabela, string campo) {
            try
            {

                var workbook = workSheet;
                IXLWorksheet sheet;
                try
                {
                    sheet = workbook.Worksheets.First(w => w.Name == tabela);
                }
                catch (Exception)
                {
                    throw new Exception("Planilha não encontrada");
                }

                if (!validaCabecalho(sheet, campo))
                    throw new Exception("Cabeçalho incorreto");

                return sheet;

            }
            catch (Exception)
            {
                throw new Exception("Planilha não encontrada");
            }

        }

        private string LookupRace(string colunm, string race)
        {
            IXLWorksheet sheet;
            sheet = getTable("Race Info", "RACE*");

            var totalLines = sheet.Rows().Count();


            //var lookupRange = sheet.Range("$A$1:$BQ$523");
            for (int l = 6; l <= totalLines; l++)
            {
                var _lineSheet = sheet.Row(l);
                var _colunmSheet = sheet.Column(colunm);

                var strNameRace = _lineSheet.Cell($"A").Value.ToString();
                var strBase = "";
                if (strNameRace == race)
                {
                    strBase = _lineSheet.Cell(colunm).Value.ToString();
                    //4'10"/4'5"+2d10
                    //workbook.Dispose();
                    return strBase;
                }
            }
            workSheet.Dispose();
            return "";

        }

        private bool validaCabecalho(IXLWorksheet plan, string campo)
        {
            if (plan.Cell($"A{3}").Value.ToString().Trim().ToUpper() != campo
                )
                return false;
            else
                return true;

        }

        //public static string LookupRace(string key)
        //{
        //    //string path = @"C:\Users\cristiano.lagame\source\repos\HeroForge-OnceAgain2\HeroForge-OnceAgain\CreatureInfo.xlsx";
        //    string path = @"C:\Users\cristiano.lagame\source\repos\HeroForge-OnceAgain2\HeroForge-OnceAgain\"+ baseInfoCreatures;

        //    using (var reader = new StreamReader(path))
        //    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        //    {
        //        var records = csv.GetRecords<CsvRecord>();
        //        foreach (var record in records)
        //        {
        //            if (record.Race == key)
        //            {
        //                return record.BaseAge;
        //            }
        //        }
        //    }
        //    return "";
        //}

        public class CsvRecord
        {
            public string Race { get; set; }
            public string Category { get; set; }
            public string ShortDescription { get; set; }
            public string Size { get; set; }
            public string Type { get; set; }
            public string Subtype { get; set; }
            public string HD { get; set; }
            public string Land { get; set; }
            public string Burrow { get; set; }
            public string Climb { get; set; }
            public string Fly { get; set; }
            public string Maneuver { get; set; }
            public string Swim { get; set; }
            public string NaturalArmor { get; set; }
            public string NaturalAttacks { get; set; }
            public string SpecialAttacks { get; set; }
            public string Spell_likeabilities { get; set; }
            public string Psionicabilities { get; set; }
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
            public string Str { get; set; }
            public string Dex { get; set; }
            public string Con { get; set; }
            public string Int { get; set; }
            public string Wis { get; set; }
            public string Cha { get; set; }
            public string StrAdj { get; set; }
            public string DexAdj { get; set; }
            public string ConAdj { get; set; }
            public string IntAdj { get; set; }
            public string WisAdj { get; set; }
            public string ChaAdj { get; set; }
            public string RacialSkills { get; set; }
            public string RacialWeaponFamiliarity { get; set; }
            public string RacialWeaponProficiency { get; set; }
            public string BonusFeats { get; set; }
            public string AutomaticLanguages { get; set; }
            public string BonusLanguages { get; set; }
            public string DragonlanceLanguages { get; set; }
            public string CRAdj { get; set; }
            public string Alignment { get; set; }
            public string LevelAdj { get; set; }
            public string FavoredClass { get; set; }
            public string BaseAge { get; set; }
            public string Height { get; set; }
            public string Weight { get; set; }
            public string SpecialQualities { get; set; }
            public string Filler1 { get; set; }
            public string FamiliarLevel { get; set; }
            public string CompanionLevel { get; set; }
            public string FamiliarType { get; set; }
            public string CompanionType { get; set; }
            public string WildshapeSpecial { get; set; }
            public string Special { get; set; }
            public string Index { get; set; }
            public string Species { get; set; }
            public string Src { get; set; }
            public string Pg { get; set; }
            public string AltSrc { get; set; }
        }

        public class ModelClassMap : ClassMap<CsvRecord>
        {
            public ModelClassMap()
            {
                Map(m => m.Race).Name("Race");
                Map(m => m.Category).Name("Category");
                Map(m => m.ShortDescription).Name("Short Description");
                Map(m => m.Size).Name("Size");
                Map(m => m.Type).Name("Type");
                Map(m => m.Subtype).Name("Subtype");
                Map(m => m.HD).Name("HD");
                Map(m => m.Land).Name("Land");
                Map(m => m.Burrow).Name("Burrow");
                Map(m => m.Climb).Name("Climb");
                Map(m => m.Fly).Name("Fly");
                Map(m => m.Maneuver).Name("Maneuver");
                Map(m => m.Swim).Name("Swim");
                Map(m => m.NaturalArmor).Name("Natural Armor");
                Map(m => m.NaturalAttacks).Name("Natural Attacks");
                Map(m => m.SpecialAttacks).Name("Special Attacks");
                Map(m => m.Spell_likeabilities).Name("Spell-like abilities");
                Map(m => m.Psionicabilities).Name("Psionic abilities");
                Map(m => m.OtherSpecialAbilities).Name("Other Special Abilities");
                Map(m => m.LowLightVision).Name("LowLight Vision");
                Map(m => m.Darkvision).Name("Darkvision");
                Map(m => m.OtherSenses).Name("Other Senses");
                Map(m => m.Immunities).Name("Immunities");
                Map(m => m.Vulnerabilities).Name("Vulnerabilities");
                Map(m => m.EnergyResistance).Name("Energy Resistance");
                Map(m => m.SpellResistance).Name("Spell Resistance");
                Map(m => m.DamageReduction).Name("Damage Reduction");
                Map(m => m.FastHealing).Name("Fast Healing");
                Map(m => m.BonusEssentia).Name("Bonus Essentia");
                Map(m => m.OtherSpecialQualities).Name("Other Special Qualities");
                Map(m => m.Str).Name("Str");
                Map(m => m.Dex).Name("Dex");
                Map(m => m.Con).Name("Con");
                Map(m => m.Int).Name("Int");
                Map(m => m.Wis).Name("Wis");
                Map(m => m.Cha).Name("Cha");
                Map(m => m.StrAdj).Name("StrAdj");
                Map(m => m.DexAdj).Name("DexAdj");
                Map(m => m.ConAdj).Name("ConAdj");
                Map(m => m.IntAdj).Name("IntAdj");
                Map(m => m.WisAdj).Name("WisAdj");
                Map(m => m.ChaAdj).Name("ChaAdj");
                Map(m => m.RacialSkills).Name("Racial Skills");
                Map(m => m.RacialWeaponFamiliarity).Name("Racial Weapon Familiarity");
                Map(m => m.RacialWeaponProficiency).Name("Racial Weapon Proficiency");
                Map(m => m.BonusFeats).Name("Bonus Feat(s)");
                Map(m => m.AutomaticLanguages).Name("Automatic Languages");
                Map(m => m.BonusLanguages).Name("Bonus Languages");
                Map(m => m.DragonlanceLanguages).Name("Dragonlance Languages");
                Map(m => m.CRAdj).Name("CR Adj.");
                Map(m => m.Alignment).Name("Alignment");
                Map(m => m.LevelAdj).Name("Level Adj.");
                Map(m => m.FavoredClass).Name("Favored Class");
                Map(m => m.BaseAge).Name("Base Age");
                Map(m => m.Height).Name("Height");
                Map(m => m.Weight).Name("Weight");
                Map(m => m.SpecialQualities).Name("Special Qualities");
                Map(m => m.Filler1).Name("Filler1");
                Map(m => m.FamiliarLevel).Name("Familiar Level");
                Map(m => m.CompanionLevel).Name("Companion Level");
                Map(m => m.FamiliarType).Name("Familiar Type");
                Map(m => m.CompanionType).Name("Companion  Type");
                Map(m => m.WildshapeSpecial).Name("Wildshape Special");
                Map(m => m.Special).Name("Special");
                Map(m => m.Index).Name("Index");
                Map(m => m.Species).Name("Species");
                Map(m => m.Src).Name("Src");
                Map(m => m.Pg).Name("Pg");
                Map(m => m.AltSrc).Name("AltSrc");
            }
        }

        static string tmpfile = "";
        static XLWorkbook workSheet = null;
        static string pathfile = "";
        static string pathphisicalfile = "";

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            string path = System.IO.Path.GetTempFileName();
            //File.Delete(path);

            var dir = System.IO.Path.GetDirectoryName(path);
            var d = new DirectoryInfo(dir);
            foreach (var file in Directory.GetFiles(d.ToString()))
            {
                //File.Delete(file);
                FileInfo archive = new FileInfo(file);
                //for (int tries = 0; IsFileLocked(archive) && tries < 5; tries++)
                //    Thread.Sleep(100);

                try
                {
                    if (path.Equals(archive.FullName))
                        archive.Delete();

                    var fileName = Path.ChangeExtension(path, "xlsm");
                    if (fileName.Equals(archive.FullName))
                        archive.Delete();

                    if (tmpfile.Equals(archive.FullName) || pathfile.Equals(archive.FullName))
                        archive.Delete();

                }
                catch (IOException)
                {

                }
            }
            workSheet.Dispose();
        }

        private void btRandomHeight_Click(object sender, EventArgs e)
        {
            int height = RandomHeight();
            RandomWeight(height);
        }

        private int RandomHeight()
        {
            int ageMod = 0, hgtMod = 0, wgtMod = 0;

            string race = lblRace.Text;
            var BaseHeightRace = LookupRace("AS", race); //LookupRaceAsync("Height", race);
            if (BaseHeightRace != null && !string.IsNullOrEmpty(BaseHeightRace))
            {
                var HeightRace = BaseHeightRace.Split('/').ToList();
                var gender = lblGender.Text;

                if (!string.IsNullOrEmpty(BaseHeightRace.Trim()))
                {
                    
                    if (!string.IsNullOrEmpty(gender))
                    {
                        var vetorGender = HeightRace[1].Split('+');
                        string Height = "";
                        if (gender.Equals("Male"))
                        {
                            Height = HeightRace[0];
                        }
                        else
                        {
                            Height = vetorGender[0];
                        }

                        if (!string.IsNullOrEmpty(Height))
                        {
                            Height = Height.Replace("\\", "");
                            string Dice = vetorGender[1];
                            var Mod = RollDice(Dice);
                            hgtMod = CalcHeight(Dice, Height);
                        }
                    }
                    

                }
                else
                {
                    lblRandomHeight.Text = "0";
                }
            }
            return hgtMod;
        }


        private int CalcHeight(string Dice, string heightDice)
        {
            int hgtMod = RollDice(Dice);
            // int hgtBase = var BaseHeightRace = await LookupRaceAsync("AS", race); 
            //int hgtBase = 0;// GetHeight();
            int hgtBase = GetHeight(heightDice);
            int hgtFinal = 0;
            hgtFinal = hgtBase + hgtMod;

            string height = (hgtFinal / 12).ToString() + "'" + (hgtFinal % 12).ToString()+"\"";
            if (Properties.Settings.Default.SystemofUnit == 1) 
            { 
                height = ConvertHeightToMetersCm(height);
            }
            lblRandomHeight.Text = height.ToString();
            return hgtMod;
        }

        public static string ConvertHeightToMetersCm(string heightString)
        {
            // Extrai os valores de pés e polegadas da string
            string[] parts = heightString.Split('\'');
            int feet = int.Parse(parts[0]);
            int inches = int.Parse(parts[1].TrimEnd('"'));

            // Faz a conversão para centímetros e depois para metros e centímetros
            double heightCm = (feet * 30.48) + (inches * 2.54);
            int heightMeters = (int)(heightCm / 100);
            int heightCmRemaining = (int)(heightCm % 100);

            return heightMeters.ToString() + "," + heightCmRemaining.ToString() + "m";
        }


        private int GetHeight(string heightString)
        {
            //string heightString = excelWorksheet.Range["CK18"].Value;            
            int feetPosition = heightString.IndexOf("'");
            int inchesPosition = heightString.IndexOf("\"");
            int feet = int.Parse(heightString.Substring(0, feetPosition));
            int inches = int.Parse(heightString.Substring(feetPosition + 1, inchesPosition - feetPosition - 1));
            int totalInches = feet * 12 + inches;
            //excelWorksheet.Range["CK19"].Value = totalInches;
            return totalInches;
        }

        public class Root
        {
            public string Name { get; set; }
        }
        private static Random rng = new Random();

        private void btRandomName_Click(object sender, EventArgs e)
        {
            Root root = new Root();
            string path = "";
            if (lblGender.Text == "Male")
            {
                path = System.Windows.Forms.Application.StartupPath.Replace("\\bin\\Debug", "") + "\\names.json";
            }
            else
            {
                path = System.Windows.Forms.Application.StartupPath.Replace("\\bin\\Debug", "") + "\\femalenames.json";
            }
            

            if (File.Exists(path))
            {
                string jsonString = File.ReadAllText(path);
                var dict = JsonConvert.DeserializeObject<IList<Root>>(jsonString);                
                var name = dict[rng.Next(dict.Count)].Name;
                lblRandomName.Text = name;
            }
                

        }

        private void btRandomWeight_Click(object sender, EventArgs e)
        {
            int height = RandomHeight();
            RandomWeight(height);

        }

        private void RandomWeight(int hgtBase)
        {
            int ageMod = 0, wgtMod = 0;

            string race = lblRace.Text;
            var BaseWeightRace = LookupRace("AT", race); //LookupRaceAsync("Weight", race);
            if (BaseWeightRace != null && !string.IsNullOrEmpty(BaseWeightRace))
            {
                var WeightRace = BaseWeightRace.Split('/').ToList();
                var gender = lblGender.Text;

                if (!string.IsNullOrEmpty(BaseWeightRace.Trim()))
                {                    
                    if (!string.IsNullOrEmpty(gender))
                    {
                        var vetorGender = WeightRace[1].Split('+');
                        string Weight = "";
                        if (gender.Equals("Male"))
                        {
                            Weight = WeightRace[0];
                        }
                        else
                        {
                            Weight = vetorGender[0];
                        }

                        if (!string.IsNullOrEmpty(Weight))
                        {
                            Weight = Weight.Replace("\\", "");
                            string Dice = vetorGender[1];
                            var Mod = RollDice(Dice);
                            CalcWeight(Dice, Weight, hgtBase);
                        }
                    }
                    

                }
                else
                {
                    lblRandomAge.Text = "0";
                }
            }
        }


        private void CalcWeight(string Dice, string weightDice, int hgtBase)
        {
            int wgtMod = RollDice(Dice);
            // int wgtBase = var BaseWeightRace = await LookupRaceAsync("AS", race); 
            //int wgtBase = 0;// GetWeight();
            //int wgtBase = GetWeight(weightDice);
            int wgtFinal = 0;
            wgtFinal = int.Parse(weightDice) + (hgtBase * wgtMod);

            //string weight = (wgtFinal / 12).ToString() + "'" + (wgtFinal % 12).ToString() + "\"";
            if (Properties.Settings.Default.SystemofUnit == 1)
            {
                lblRandomWeight.Text = Math.Round(ConvertPoundsToKilos(wgtFinal), 2).ToString() + " kg";
            }
            else
            {
                lblRandomWeight.Text = wgtFinal.ToString() + " lbs";
            }
            
        }

        public static double ConvertPoundsToKilos(int pounds)
        {
            double kilos = pounds * 0.45359237;
            return kilos;
        }




        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Preferences nForm = new Preferences();
            nForm.TopLevel = true;
            nForm.Show();
        }

        private void lblLanguage_Click(object sender, EventArgs e)
        {

        }
    }
}
