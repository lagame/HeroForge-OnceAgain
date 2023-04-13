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
using static HeroForge_OnceAgain.Form1;
using HeroForge_OnceAgain.Models;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Resources;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using HeroForge_OnceAgain.Utils;
using DocumentFormat.OpenXml.Math;
using System.Security.Cryptography;
using System.Net.Sockets;
using System.Windows.Interop;
using static System.Windows.Forms.AxHost;
using DocumentFormat.OpenXml.Presentation;
using System.Drawing.Printing;
using System.Runtime.Remoting.Messaging;

namespace HeroForge_OnceAgain
{
    public partial class Form1 : Form
    {
        private Character character;
        //private System.Windows.Forms.Button printButton = new System.Windows.Forms.Button();
        private PrintDocument printDocument1 = new PrintDocument();
        Bitmap memoryImage;

        public Form1()
        {
            InitializeComponent();
            CharacterCreationInfo creationInfo = new CharacterCreationInfo();
            character = new Character(creationInfo);

            
            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            
        }
        public void Form1_Load(object sender, EventArgs e)
        {
            if (workSheet == null)
                workSheet = LoadFile();

            Reload();
            RaceUtils.PopulateRaceComboBox(cbRaces, 3);
            cbAlignment.SelectedIndex = 0;
            cBAbilityScoreSystem.SelectedIndex = 0;
            CheckedListBox checkede = ckListCharacterSheetDisplayHitPointOptions;
            for (int i = 0; i < checkede.Items.Count; i++)
            {
                if (i.Equals(0))
                {
                    checkede.SetItemCheckState(i, (true ? CheckState.Checked : CheckState.Unchecked));
                }                   
            }
        }

        public void CalculateAbility(Label ability)
        {
            int mod = Convert.ToInt32(ability.Text);
            //switch (ability)
            //{
            //    case 0:
            //        break;
            //    case 1:
            //        break;
            //    case 2:
            //        break;
            //    case 3:
            //        break;
            //    case 4:
            //        break;
            //    case 5:
            //        break;
            //}


                    int strMod = Convert.ToInt32(labelModStr.Text);
            int dexMod = Convert.ToInt32(labelModDex.Text);
            int conMod = Convert.ToInt32(labelModCon.Text);
            int intMod = Convert.ToInt32(labelModInt.Text);
            int wisMod = Convert.ToInt32(labelModWis.Text);
            int chaMod = Convert.ToInt32(labelModCha.Text);

            int str = Convert.ToInt32(labelStr.Text) ;//+ strMod;
            int dex = Convert.ToInt32(labelDex.Text) ;//+ dexMod;
            int con = Convert.ToInt32(labelCon.Text) ;//+ conMod;
            int inte = Convert.ToInt32(labelInt.Text);// + intMod;
            int wis = Convert.ToInt32(labelWis.Text) ;//+ wisMod;
            int cha = Convert.ToInt32(labelCha.Text); //+ chaMod;

            labelStr.Text = (Convert.ToInt32(initialStrength.Value) + strMod).ToString();
            labelDex.Text = (Convert.ToInt32(initialDexterity.Value) + dexMod).ToString();
            labelCon.Text = (Convert.ToInt32(initialConstitution.Value) + conMod).ToString();
            labelInt.Text = (Convert.ToInt32(initialIntelligence.Value) + intMod).ToString();
            labelWis.Text = (Convert.ToInt32(initialWisdom.Value) + wisMod).ToString();
            labelCha.Text = (Convert.ToInt32(initialCharisma.Value) + chaMod).ToString();

            lblModStrTotal.Text = CalcAttributeTotal(str).ToString();
            lblModDexTotal.Text = CalcAttributeTotal(dex).ToString();
            lblModConTotal.Text = CalcAttributeTotal(con).ToString();
            lblModIntTotal.Text = CalcAttributeTotal(inte).ToString();
            lblModWisTotal.Text = CalcAttributeTotal(wis).ToString();
            lblModChaTotal.Text = CalcAttributeTotal(cha).ToString();
        }

        private int CalcAttributeTotal(int valAttrib)
        {
            double val = valAttrib;
            val = val - 10;

            double valpos = val % 2;
            

            val = val / 2;

            if (valpos == 1 || valpos == -1)
            {   
                val = val - 0.5;                
            }

            return Convert.ToInt32(val);
        }

        public void CalculatePointBuy()
        {
            Localization();
            
            int strMod = Convert.ToInt32(labelModStr.Text);
            int dexMod = Convert.ToInt32(labelModDex.Text);
            int conMod = Convert.ToInt32(labelModCon.Text);
            int intMod = Convert.ToInt32(labelModInt.Text);
            int wisMod = Convert.ToInt32(labelModWis.Text);
            int chaMod = Convert.ToInt32(labelModCha.Text);

            int str = Convert.ToInt32(initialStrength.Value)       ;//+ strMod;
            int dex = Convert.ToInt32(initialDexterity.Value)      ;//+ dexMod;
            int con = Convert.ToInt32(initialConstitution.Value)   ;//+ conMod;
            int inte = Convert.ToInt32(initialIntelligence.Value)  ;//+ intMod;
            int wis = Convert.ToInt32(initialWisdom.Value)         ;//+ wisMod;
            int cha = Convert.ToInt32(initialCharisma.Value)       ;//+ chaMod;

            labelStr.Text = str.ToString();
            labelDex.Text = dex.ToString();
            labelCon.Text = con.ToString();
            labelInt.Text = inte.ToString();
            labelWis.Text = wis.ToString();
            labelCha.Text = cha.ToString();

            lblModStr.Text = CalcAttribute(str).ToString();
            lblModDex.Text = CalcAttribute(dex).ToString();
            lblModCon.Text = CalcAttribute(con).ToString();
            lblModInt.Text = CalcAttribute(inte).ToString();
            lblModWis.Text = CalcAttribute(wis).ToString();
            lblModCha.Text = CalcAttribute(cha).ToString();
            
            lblModStrTotal.Text = CalcAttributeTotal(str).ToString();
            lblModDexTotal.Text = CalcAttributeTotal(dex).ToString();
            lblModConTotal.Text = CalcAttributeTotal(con).ToString();
            lblModIntTotal.Text = CalcAttributeTotal(inte).ToString();
            lblModWisTotal.Text = CalcAttributeTotal(wis).ToString();
            lblModChaTotal.Text = CalcAttributeTotal(cha).ToString();

            CalcAbilityScore();

            int totalAttrib = (Convert.ToInt32(lblModStr.Text) + Convert.ToInt32(lblModDex.Text) + Convert.ToInt32(lblModCon.Text) + Convert.ToInt32(lblModInt.Text) + Convert.ToInt32(lblModWis.Text) + Convert.ToInt32(lblModCha.Text));
            
            if (totalAttrib > 0)
            {
                lblTypeCampaign.Text = "";
                switch (totalAttrib)
                {
                    case 15:
                        lblTypeCampaign.Text = LocalizationUtils.L("LowPoweredCampaign");
                        break;
                    case 22:
                        lblTypeCampaign.Text = LocalizationUtils.L("ChallengingCampaign");
                        break;
                    case 25:
                        lblTypeCampaign.Text = LocalizationUtils.L("StandardCampaign");
                        break;
                    case 28:
                        lblTypeCampaign.Text = LocalizationUtils.L("TougherCampaign");
                        break;
                    case 32:
                        lblTypeCampaign.Text = LocalizationUtils.L("HighPoweredCampaign");
                        break;
                }
            }

            //CalculateAbility();
        }

        public void ClearStatsDescriptionSelections()
        {

            var confirmResult = MessageBox.Show(LocalizationUtils.L("AreYouSureClearForm1Stats"),
                                     LocalizationUtils.L("ConfirmClear"),
                                     MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                initialStrength.Value = 8;
                initialDexterity.Value = 8;
                initialConstitution.Value = 8;
                initialIntelligence.Value = 8;
                initialWisdom.Value = 8;
                initialCharisma.Value = 8;

                cbAlignment.SelectedIndex = 0;
            }

        }

        private int CalcAttribute(int valAttrib)
        {
            int attrib = 0;
            int valueBase = valAttrib - 8;

            if (valAttrib >= 8 && valAttrib <= 14)
            {
                attrib = valueBase;
            }
            else
            {
                if (valAttrib == 15)
                {
                    attrib = (valueBase + 1);
                }
                else
                {
                    if (valAttrib == 16)
                    {
                        attrib = (valueBase + 2);
                    }
                    else
                    {
                        if (valAttrib == 17)
                        {
                            attrib = (valueBase + 4);
                        }
                        else
                        {
                            if (valAttrib == 18)
                            {
                                attrib = (valueBase + 6);
                            }
                        }

                    }
                }
            }

            return attrib;
        }

        public void Reload()
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
            this.Controls.Clear();
            RaceUtils.PopulateRaceComboBox(cbRaces, 3);
            InitializeComponent();
            RaceUtils.PopulateRaceComboBox(cbRaces, 3);
            cbAlignment.SelectedIndex = 0;
        }

        public void Localization()
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
            if (!lblRandomAge.Text.Equals("0"))
            {
                txtAge.Text = lblRandomAge.Text;
            }
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
            if (!lblRandomHeight.Text.Equals("0 m") && !lblRandomHeight.Text.Equals("0 ft"))
            {
                txtHeight.Text = lblRandomHeight.Text;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!lblRandomWeight.Text.Equals("0 kg") && !lblRandomWeight.Text.Equals("0 lbs"))
            {
                txtWeight.Text = lblRandomWeight.Text;
            }
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
            int agebasic = 0;
            int ageMod = 0;
            string textLookup = lblRace.Text;
            List<string> AgeRace = new List<string>();

            Race race = RaceUtils.GetOriginalRace(textLookup);

            string BaseAgeRace = "";
            if (race != null)
            {
                BaseAgeRace = LookupInfo("AR", race.OriginalName, "Race Info");
            }


            if (string.IsNullOrEmpty(BaseAgeRace))
            {
                lblRandomAge.Text = "0";
                return;
            }

            AgeRace = BaseAgeRace.Split('/').ToList();
            var classe = lblClasses.Text;

            if (string.IsNullOrEmpty(classe))
            {
                lblRandomAge.Text = "0";
                return;
            }

            var vetorClasse = classe.Split('/');
            foreach (var item in vetorClasse)
            {
                var AgeString = AgeRace[0];
                var Age = AgeString.Split('+').ToList();
                agebasic = Convert.ToInt32(Age[0]);
                var item2 = item.ToUpper();

                if (item2.Contains("BARBARIAN") || item2.Contains("ROGUE") || item2.Contains("SORCERER"))
                {
                    if (!string.IsNullOrEmpty(AgeRace[0]))
                    {
                        string Dice = Age[1];
                        ageMod = RollDice(Dice);
                    }
                }
                else if (item2.Contains("BARD") || item2.Contains("FIGHTER") || item2.Contains("PALADIN") || item2.Contains("RANGER"))
                {
                    string AgeString2 = AgeRace[1];
                    ageMod = RollDice(AgeString2);
                }
                else if (item2.Contains("CLERIC") || item2.Contains("DRUID") || item2.Contains("MONK") || item2.Contains("WIZARD"))
                {
                    string AgeString2 = AgeRace[2];
                    ageMod = RollDice(AgeString2);
                }
            }

            lblRandomAge.Text = (agebasic + ageMod).ToString();
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

        private IXLWorksheet getTable(string tabela, string campo)
        {
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

                //if (!validaCabecalho(sheet, campo))
                //    throw new Exception("Cabeçalho incorreto");

                return sheet;

            }
            catch (Exception)
            {
                throw new Exception("Planilha não encontrada");
            }

        }

        private string LookupInfo(string colunm, string textLookup, string table)
        {
            IXLWorksheet sheet;
            sheet = getTable(table, "RACE*");

            var totalLines = sheet.Rows().Count();


            //var lookupRange = sheet.Range("$A$1:$BQ$523");
            for (int l = 6; l <= totalLines; l++)
            {
                var _lineSheet = sheet.Row(l);
                var _colunmSheet = sheet.Column(colunm);

                var strName = _lineSheet.Cell($"A").Value.ToString();
                var strBase = "";
                if (strName == textLookup)
                {
                    strBase = _lineSheet.Cell(colunm).Value.ToString();
                    //4'10"/4'5"+2d10
                    //workbook.Dispose();
                    return strBase;
                }
            }
            //workSheet.Dispose();
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
            CloseProgram();
        }

        private void CloseProgram()
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

            string textLookup = lblRace.Text;

            Race race = RaceUtils.GetOriginalRace(textLookup);

            string BaseHeightRace = "";
            if (race != null)
            {
                BaseHeightRace = LookupInfo("AS", race.OriginalName, "Race Info");
            }

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

            string height = (hgtFinal / 12).ToString() + "'" + (hgtFinal % 12).ToString() + "\"";
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

        public class HairColors
        {
            public string id { get; set; }
            public string name { get; set; }
            public string lang { get; set; }
        }

        public class HairTypes
        {
            public string id { get; set; }
            public string name { get; set; }
            public string lang { get; set; }
        }

        public class EyesColors
        {
            public string id { get; set; }
            public string name { get; set; }
            public string lang { get; set; }
        }

        private static Random rng = new Random();
        private bool state;

        private void btRandomName_Click(object sender, EventArgs e)
        {
            Root root = new Root();
            string path = "";
            if (lblGender.Text == "Male")
            {
                path = System.Windows.Forms.Application.StartupPath.Replace("\\bin\\Debug", "") + "\\Resources\\names.json";
            }
            else
            {
                path = System.Windows.Forms.Application.StartupPath.Replace("\\bin\\Debug", "") + "\\Resources\\femalenames.json";
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

            string textLookup = lblRace.Text;

            Race race = RaceUtils.GetOriginalRace(textLookup);

            string BaseWeightRace = "";
            if (race != null)
            {
                BaseWeightRace = LookupInfo("AT", race.OriginalName, "Race Info");
            }

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
            int wgtFinal = 0;
            wgtFinal = int.Parse(weightDice) + (hgtBase * wgtMod);

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

        private void btRandomHair_Click(object sender, EventArgs e)
        {

            // Load data from JSON files
            var hairColorsJson = File.ReadAllText(System.Windows.Forms.Application.StartupPath.Replace("\\bin\\Debug", "") + "\\Resources\\haircolors.json");
            var hairTypesJson = File.ReadAllText(System.Windows.Forms.Application.StartupPath.Replace("\\bin\\Debug", "") + "\\Resources\\hairtypes.json");
            var hairColors = JsonConvert.DeserializeObject<List<HairColors>>(hairColorsJson);
            var hairTypes = JsonConvert.DeserializeObject<List<HairTypes>>(hairTypesJson);

            // Filter records in selected language
            var languageCode = Properties.Settings.Default.LanguageIndex == 0 ? "en" : "pt-BR";
            var filteredHairColors = hairColors.Where(c => c.lang == languageCode).ToList();
            var filteredHairTypes = hairTypes.Where(t => t.lang == languageCode).ToList();

            // Select a random record from each list
            var randomHairColor = filteredHairColors[rng.Next(filteredHairColors.Count)];
            var randomHairType = filteredHairTypes[rng.Next(filteredHairTypes.Count)];

            // Display concatenated hair
            lblRandomHair.Text = randomHairColor.name + " " + randomHairType.name;
        }
        private async void btRandomEyes_Click(object sender, EventArgs e)
        {
            string eyes = "";
            string path = Path.Combine(System.Windows.Forms.Application.StartupPath.Replace("\\bin\\Debug", ""), "Resources\\eyescolors.json");

            if (File.Exists(path))
            {
                var eyesColors = await LoadJsonAsync<EyesColors>(path);

                // Filter records in selected language
                var languageCode = Properties.Settings.Default.LanguageIndex == 0 ? "en" : "pt-BR";
                var filteredEyesColors = eyesColors.Where(c => c.lang == languageCode).ToList();

                // Select a random record from each list
                var randomEyesColors = filteredEyesColors[rng.Next(filteredEyesColors.Count)];
                eyes = randomEyesColors.name;
            }
            lblRandomEyes.Text = eyes;
        }

        private async Task<IList<T>> LoadJsonAsync<T>(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
            {
                var serializer = new JsonSerializer();
                using (var streamReader = new StreamReader(stream))
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    return await Task.Run(() => serializer.Deserialize<IList<T>>(jsonTextReader));
                }
            }
        }
        private void saveCharacterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Preenche os campos da instância da classe Character com as informações do personagem
            character.Name = txtName.Text;
            CharacterClass Class = new CharacterClass();
            Class.Name = "Guerreiro";
            Class.Level = 5; //Convert.ToInt32(txtLevel.Text);
            if (!string.IsNullOrEmpty(txtAge.Text))
            {
                character.Age = int.Parse(txtAge.Text);
            }

            character.Gender = cBGender.SelectedItem != null ? Convert.ToInt32(cBGender.SelectedIndex) : 0;

            var selectedRace = (Race)cbRaces.SelectedItem;
            character.Race = selectedRace.DisplayName;

            character.Alignment = cbAlignment.SelectedIndex;
            character.Deity = cbDeity.SelectedIndex;
            character.Height = txtHeight.Text.Replace("\\", "");
            character.Weight = txtWeight.Text;
            character.Eyes = txtEyes.Text;
            character.Hair = txtHair.Text;


            character.CharacterClasses.Add(Class);

            // Abre uma janela de diálogo para permitir que o usuário escolha onde salvar o arquivo

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Arquivos JSON (*.json)|*.json";
                saveFileDialog.Title = LocalizationUtils.L("OpenSaveCharacter");
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string json = JsonConvert.SerializeObject(character);

                    string fileName = saveFileDialog.FileName;
                    // Salva a última pasta usada nas configurações de usuário
                    string folderPath = Path.GetDirectoryName(fileName);
                    Properties.Settings.Default.LastUsedFolder = folderPath;
                    Properties.Settings.Default.Save();

                    File.WriteAllText(fileName, json);
                    string msg = LocalizationUtils.L("MsgSaveCharacter");
                    MessageBox.Show(msg);
                }
            }
        }
        private void loadCharacterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Localization();
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Recuperar a última pasta usada
                if (!string.IsNullOrEmpty(Properties.Settings.Default.LastUsedFolder))
                {
                    openFileDialog.InitialDirectory = Properties.Settings.Default.LastUsedFolder;
                }
                openFileDialog.Title = LocalizationUtils.L("SelectFileToOpen");
                openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string json = File.ReadAllText(openFileDialog.FileName);
                    character = JsonConvert.DeserializeObject<Character>(json);

                    txtName.Text = character.Name;
                    txtAge.Text = character.Age.ToString();
                    cBGender.SelectedIndex = character.Gender;
                    lblRace.Text = character.Race;
                    foreach (var item in cbRaces.Items)
                    {
                        var race = item as Race;
                        if (race.DisplayName.Equals(character.Race))
                        {
                            cbRaces.SelectedIndex = race.Id;
                        }
                    }
                    //cbRaces.SelectedValue = character.Race;
                    cbAlignment.SelectedIndex = character.Alignment;
                    cbDeity.SelectedIndex = character.Deity;
                    txtHeight.Text = character.Height;
                    txtWeight.Text = character.Weight;
                    txtEyes.Text = character.Eyes;
                    txtHair.Text = character.Hair;
                    //cbClass.SelectedItem = character.Class;
                    string msg = LocalizationUtils.L("LoadSavedCharacter");
                    MessageBox.Show(msg);
                }
            }
        }

        private void cbRaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            Localization();
            var selectedRace = (Race)cbRaces.SelectedItem;
            lblRace.Text = selectedRace.DisplayName;
        }

        private void cBGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblGender.Text = (string)cBGender.SelectedItem;
        }

        private void initialStrength_ValueChanged(object sender, EventArgs e)
        {
            AbilityScoreSystem();
        }

        private void initialDexterity_ValueChanged(object sender, EventArgs e)
        {
            AbilityScoreSystem();
        }

        private void initialConstitution_ValueChanged(object sender, EventArgs e)
        {
            AbilityScoreSystem();
        }

        private void initialIntelligence_ValueChanged(object sender, EventArgs e)
        {
            AbilityScoreSystem();
        }

        private void initialWisdom_ValueChanged(object sender, EventArgs e)
        {
            AbilityScoreSystem();
        }

        private void initialCharisma_ValueChanged(object sender, EventArgs e)
        {
            AbilityScoreSystem();
        }

        private void btClearStatsDescriptionSelections_Click(object sender, EventArgs e)
        {
            ClearStatsDescriptionSelections();
        }
        private void cBCampaignSelect_SelectedIndexChanged(object sender, EventArgs e)
        {

            setCampaign(cBCampaignSelect.SelectedIndex);
            
        }

        private void inactiveCampaign(CheckedListBox campaign)
        {
            for (int i = 0; i < campaign.Items.Count; i++)
            {
                campaign.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));
            }
        }

        private void activeCampaign(CheckedListBox campaign)
        {
            for (int i = 0; i < campaign.Items.Count; i++)
            {
                if (campaign.Name.Equals("ckOtherSources"))
                {
                    if (i.Equals(0))
                    {
                        campaign.SetItemCheckState(i, (true ? CheckState.Checked : CheckState.Unchecked));
                    }
                }
                else
                {
                    campaign.SetItemCheckState(i, (true ? CheckState.Checked : CheckState.Unchecked));
                    
                    if (i.Equals(0))
                    {
                        CheckedListBox checkede = ckListCharacterSheetDisplayHitPointOptions;                        
                        checkede.SetItemCheckState(1, (true ? CheckState.Checked : CheckState.Unchecked));                        
                    }
                }
            }
        }

        private void setCampaign(int selectedCampaign)
        {
            Localization();
            var forgotten =   ckForgottenRealmsSources;
            var eberron =     ckEberronSettingSources;
            var dragonlance = ckDragonLanceSources;
            var greyhawk =    ckLGSources;
            var ravenloft = cBRavenloftSources;
            var rokugan = ckOtherSources;
            var deities = ckDeityOptions;

            switch (cBCampaignSelect.SelectedIndex)
            {
                case 0: //cBCampaignSelect.SelectedItem = Generic 3.5 D&D                    
                    inactiveCampaign(forgotten);
                    inactiveCampaign(eberron);
                    inactiveCampaign(dragonlance);
                    inactiveCampaign(greyhawk);
                    inactiveCampaign(ravenloft);
                    inactiveCampaign(rokugan);                    
                    inactiveCampaign(deities);

                    break;
                case 1: // Dragonlance
                    inactiveCampaign(forgotten);
                    inactiveCampaign(eberron);                    
                    inactiveCampaign(greyhawk);
                    inactiveCampaign(ravenloft);
                    inactiveCampaign(rokugan);
                    inactiveCampaign(deities);
                    activeCampaign(dragonlance);
                    ckDeityOptions.SetItemCheckState(4, (true ? CheckState.Checked : CheckState.Unchecked));
                    break;
                case 2: // Eberron
                    inactiveCampaign(forgotten);                    
                    inactiveCampaign(dragonlance);
                    inactiveCampaign(greyhawk);
                    inactiveCampaign(ravenloft);
                    inactiveCampaign(rokugan);
                    inactiveCampaign(deities);
                    activeCampaign(eberron);
                    ckDeityOptions.SetItemCheckState(2, (true ? CheckState.Checked : CheckState.Unchecked));
                    break;
                case 3: // Forgotten Realms                    
                    inactiveCampaign(eberron);
                    inactiveCampaign(dragonlance);
                    inactiveCampaign(greyhawk);
                    inactiveCampaign(ravenloft);
                    inactiveCampaign(rokugan);
                    inactiveCampaign(deities);
                    activeCampaign(forgotten);
                    ckDeityOptions.SetItemCheckState(1, (true ? CheckState.Checked : CheckState.Unchecked));
                    break;
                case 4: // Living Greyhawk
                    inactiveCampaign(eberron);
                    inactiveCampaign(forgotten);
                    inactiveCampaign(dragonlance);
                    inactiveCampaign(ravenloft);
                    inactiveCampaign(rokugan);
                    inactiveCampaign(deities);
                    activeCampaign(greyhawk);
                    ckDeityOptions.SetItemCheckState(0, (true ? CheckState.Checked : CheckState.Unchecked));
                    break;
                case 5: // Ravenloft
                    inactiveCampaign(eberron);
                    inactiveCampaign(forgotten);
                    inactiveCampaign(dragonlance);
                    inactiveCampaign(greyhawk);
                    inactiveCampaign(rokugan);
                    inactiveCampaign(deities);
                    activeCampaign(ravenloft);
                    ckDeityOptions.SetItemCheckState(3, (true ? CheckState.Checked : CheckState.Unchecked));
                    break;
                case 6: // Rokugan                    
                    inactiveCampaign(eberron);
                    inactiveCampaign(forgotten);
                    inactiveCampaign(dragonlance);
                    inactiveCampaign(greyhawk);
                    inactiveCampaign(ravenloft);
                    inactiveCampaign(deities);
                    activeCampaign(rokugan);
                    break;
            }
        }

        private void btChat_Click(object sender, EventArgs e)
        {
            Chat cForm = new Chat();
            cForm.TopLevel = true;
            cForm.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseProgram();
        } 

        private void CaptureScreen()
        {
            Graphics myGraphics = this.CreateGraphics();
            Size s = this.Size;
            memoryImage = new Bitmap(s.Width, s.Height, myGraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, s);
        }

        private void printDocument1_PrintPage(System.Object sender,
               System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CaptureScreen();
            printDocument1.Print();
        }

        private void btAllNonAvaliable_Click(object sender, EventArgs e)
        {
            Localization();
            activeCampaign(ckNonSettingSources);
            activeCampaign(ckMonstrousNonSettingSources);
        }

        private void btResetOptionSelections_Click(object sender, EventArgs e)
        {
            Localization();
            inactiveCampaign(ckNonSettingSources);
            inactiveCampaign(ckMonstrousNonSettingSources);
        }

        private void btResetAllTabsSelections_Click(object sender, EventArgs e)
        {
            inactiveCampaign(ckForgottenRealmsSources);
            inactiveCampaign(ckEberronSettingSources);
            inactiveCampaign(ckDragonLanceSources);
            inactiveCampaign(ckLGSources);
            inactiveCampaign(cBRavenloftSources);
            inactiveCampaign(ckOtherSources);
            inactiveCampaign(ckDeityOptions);
            inactiveCampaign(ckNonSettingSources);
            inactiveCampaign(ckMonstrousNonSettingSources);
        }

        public void CalcAbilityScore()
        {
            int totalStr = Convert.ToInt32(initialStrength.Value) + CalculaCampo(labelModStr.Text) +
                 CalculaCampo(increaseModStr.Text) + CalculaCampo(txtMagicBonusStr.Text);
            labelStr.Text = totalStr.ToString();

            int totalDex = Convert.ToInt32(initialDexterity.Value) + CalculaCampo(labelModDex.Text) +
                CalculaCampo(increaseModDex.Text) + CalculaCampo(txtMagicBonusDex.Text);
            labelDex.Text = totalDex.ToString();

            int totalCon = Convert.ToInt32(initialConstitution.Text) + CalculaCampo(labelModCon.Text) +
                CalculaCampo(increaseModCon.Text) + CalculaCampo(txtMagicBonusCon.Text);
            labelCon.Text = totalCon.ToString();

            int totalInt = Convert.ToInt32(initialIntelligence.Text) + CalculaCampo(labelModInt.Text) +
                CalculaCampo(increaseModInt.Text) + CalculaCampo(txtMagicBonusInt.Text);
            labelInt.Text = totalInt.ToString();

            int totalWis = Convert.ToInt32(initialWisdom.Text) + CalculaCampo(labelModWis.Text) +
                CalculaCampo(increaseModWis.Text) + CalculaCampo(txtMagicBonusWis.Text);
            labelWis.Text = totalWis.ToString();

            int totalCha = Convert.ToInt32(initialCharisma.Text) + CalculaCampo(labelModCha.Text) +
                CalculaCampo(increaseModCha.Text) + CalculaCampo(txtMagicBonusCha.Text);
            labelCha.Text = totalCha.ToString();
        }

        private int CalculaCampo(string campo)
        {
            int increasemod = 0;
            if (campo.Trim() != "0" && campo.Trim() != "")
            {                
                increasemod = Convert.ToInt32(campo);
            }
            return increasemod;
        }

        public void AbilityScoreSystem()
        {
            Localization();
            int totalAttrib = (Convert.ToInt32(lblModStr.Text) + Convert.ToInt32(lblModDex.Text) + Convert.ToInt32(lblModCon.Text) + Convert.ToInt32(lblModInt.Text) + Convert.ToInt32(lblModWis.Text) + Convert.ToInt32(lblModCha.Text));
            var selectedItem = cBAbilityScoreSystem.SelectedIndex;
            
            lblAlternativeRoll.Location = new Point(185, 25);            
            lblAlternativeRoll.Size = new Size(0, 13);
            lblAlternativeRoll.AutoSize = true;
            switch (selectedItem)
            {
                case 0:
                    lblTotalPoints.Visible = true;
                    lblPointBuy.Visible = true;
                    labelTypeCampaign.Visible = true;
                    lblTypeCampaign.Visible = true;

                    lblTotalPoints.Text = "25 " + LocalizationUtils.L("Points");
                    lblTypeCampaign.Text = "";
                    lblTotalPoints.Text = (totalAttrib).ToString() + " " + LocalizationUtils.L("Points");
                    lblAlternativeRoll.Text = "";                    
                    CalculatePointBuy();                    
                    break;
                case 1:
                    lblTotalPoints.Visible = true;
                    lblPointBuy.Visible = true;
                    labelTypeCampaign.Visible = true;
                    lblTypeCampaign.Visible = true;
                    lblTotalPoints.Text = "15 " + LocalizationUtils.L("Points");
                    lblTypeCampaign.Text = LocalizationUtils.L("LowPoweredCampaign");
                    lblTotalPoints.Text = (totalAttrib).ToString() + " " + LocalizationUtils.L("Points");
                    lblAlternativeRoll.Text = "";
                    break;
                case 2:
                    lblTotalPoints.Visible = false;
                    lblPointBuy.Visible = false;
                    labelTypeCampaign.Visible = false;
                    lblTypeCampaign.Visible = false;
                    lblTotalPoints.Text = "";

                    lblAlternativeRoll.Size = new Size(0, 13);
                    lblAlternativeRoll.Location = new Point(185, 25);
                    lblAlternativeRoll.Text = LocalizationUtils.L("ArrangeAsDesired");
                    break;
                case 3:
                    lblTotalPoints.Visible = false;
                    lblPointBuy.Visible = false;
                    labelTypeCampaign.Visible = false;
                    lblTypeCampaign.Visible = false;
                    lblTotalPoints.Text = "";
                    lblAlternativeRoll.AutoSize = false;
                    lblAlternativeRoll.Size = new Size(320, 39);
                    lblAlternativeRoll.Location = new Point(185, 13);
                    lblAlternativeRoll.Text = LocalizationUtils.L("EliteArrayText");
                    break;
                case 4:
                    lblTotalPoints.Visible = false;
                    lblPointBuy.Visible = false;
                    labelTypeCampaign.Visible = false;
                    lblTypeCampaign.Visible = false;
                    lblTotalPoints.Text = "";
                    lblAlternativeRoll.Text = "";
                    break;
                case 5:
                    lblTotalPoints.Visible = false;
                    lblPointBuy.Visible = false;
                    labelTypeCampaign.Visible = false;
                    lblTypeCampaign.Visible = false;
                    lblTotalPoints.Text = "";
                    lblAlternativeRoll.Text = "";
                    break;
                case 6:
                    lblTotalPoints.Visible = false;
                    lblPointBuy.Visible = false;
                    labelTypeCampaign.Visible = false;
                    lblTypeCampaign.Visible = false;
                    lblTotalPoints.Text = "";
                    lblAlternativeRoll.Text = "";
                    break;
                case 7:
                    lblTotalPoints.Visible = false;
                    lblPointBuy.Visible = false;
                    labelTypeCampaign.Visible = false;
                    lblTypeCampaign.Visible = false;
                    lblTotalPoints.Text = "";
                    lblAlternativeRoll.Text = "";
                    break;
            }
        }

        private void cBAbilityScoreSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            AbilityScoreSystem();
        }

        public void AbilityBump(System.Windows.Forms.ComboBox comboBox)
        {
            int value = 0;
            var selectedItem = comboBox.SelectedIndex;
            switch (selectedItem)
            {
                case 0:
                    break;
                case 1:
                    labelModStr.Visible = true;
                    labelModStr.Text = (Convert.ToInt32(labelModStr.Text) + 1).ToString();                                        
                    labelStr.Text = (Convert.ToInt32(initialStrength.Value) + Convert.ToInt32(labelModStr.Text)).ToString();                    
                    lblModStrTotal.Text = CalcAttributeTotal(Convert.ToInt32(labelStr.Text)).ToString();                    
                    break;
                case 2:
                    labelModDex.Visible = true;
                    labelModDex.Text = (Convert.ToInt32(labelModDex.Text) + 1).ToString();                    
                    labelDex.Text = (Convert.ToInt32(initialDexterity.Value) + Convert.ToInt32(labelModDex.Text)).ToString();                    
                    lblModDexTotal.Text = CalcAttributeTotal(Convert.ToInt32(labelDex.Text)).ToString();
                    break;
                case 3:
                    labelModCon.Visible = true;
                    labelModCon.Text = (Convert.ToInt32(labelModCon.Text) + 1).ToString();                    
                    labelCon.Text = (Convert.ToInt32(initialConstitution.Value) + Convert.ToInt32(labelModCon.Text)).ToString();                    
                    lblModConTotal.Text = CalcAttributeTotal(Convert.ToInt32(labelCon.Text)).ToString();
                    break;
                case 4:
                    labelModInt.Visible = true;                    
                    labelModInt.Text = (Convert.ToInt32(labelModInt.Text) + 1).ToString();
                    labelInt.Text = (Convert.ToInt32(initialIntelligence.Value) + Convert.ToInt32(labelModInt.Text)).ToString();
                    lblModIntTotal.Text = CalcAttributeTotal(Convert.ToInt32(labelInt.Text)).ToString();
                    break;
                case 5:
                    labelModWis.Visible = true;                    
                    labelModWis.Text = (Convert.ToInt32(labelModWis.Text) + 1).ToString();
                    labelWis.Text = (Convert.ToInt32(initialWisdom.Value) + Convert.ToInt32(labelModWis.Text)).ToString();
                    lblModWisTotal.Text = CalcAttributeTotal(Convert.ToInt32(labelWis.Text)).ToString();
                    break;
                case 6:
                    labelModCha.Visible = true;
                    labelModCha.Visible = true;
                    labelModCha.Text = (Convert.ToInt32(labelModCha.Text) + 1).ToString();
                    labelCha.Text = (Convert.ToInt32(initialCharisma.Value) + Convert.ToInt32(labelModCha.Text)).ToString();
                    lblModChaTotal.Text = CalcAttributeTotal(Convert.ToInt32(labelCha.Text)).ToString();
                    break;
            }
        }

        private int BumpAttribute(string attributeValue)
        {
            int numero;
            bool resultado = Int32.TryParse(attributeValue, out numero);
            if (resultado)
            {
                Console.WriteLine("Conversão de '{0}' para {1}.", attributeValue, numero);
            }
            else
            {
                Console.WriteLine("A conversão de '{0}' Falhou.", attributeValue == null ? "<null>" : attributeValue);
            }
            return numero;
        }

        private void cbBump1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AbilityBump((System.Windows.Forms.ComboBox)sender);
        }        

        private void cbBump2_SelectedIndexChanged(object sender, EventArgs e)
        {
            AbilityBump((System.Windows.Forms.ComboBox)sender);
        }
        private void cbBump3_SelectedIndexChanged(object sender, EventArgs e)
        {
            AbilityBump((System.Windows.Forms.ComboBox)sender);
        }

        private void cbBump4_SelectedIndexChanged(object sender, EventArgs e)
        {
            AbilityBump((System.Windows.Forms.ComboBox)sender);
        }
        private void cbBump5_SelectedIndexChanged(object sender, EventArgs e)
        {
            AbilityBump((System.Windows.Forms.ComboBox)sender);
        }
        private void cbBump6_SelectedIndexChanged(object sender, EventArgs e)
        {
            AbilityBump((System.Windows.Forms.ComboBox)sender);
        }

        private void cbBump7_SelectedIndexChanged(object sender, EventArgs e)
        {
            AbilityBump((System.Windows.Forms.ComboBox)sender);
        }
        private void cbBump8_SelectedIndexChanged(object sender, EventArgs e)
        {
            AbilityBump((System.Windows.Forms.ComboBox)sender);
        }

        private void cbBump9_SelectedIndexChanged(object sender, EventArgs e)
        {
            AbilityBump((System.Windows.Forms.ComboBox)sender);
        }
        private void cbBump10_SelectedIndexChanged(object sender, EventArgs e)
        {
            AbilityBump((System.Windows.Forms.ComboBox)sender);
        }
        private void cbBump11_SelectedIndexChanged(object sender, EventArgs e)
        {
            AbilityBump((System.Windows.Forms.ComboBox)sender);
        }

        private void cbBump12_SelectedIndexChanged(object sender, EventArgs e)
        {
            AbilityBump((System.Windows.Forms.ComboBox)sender);
        }
        private void cbBump13_SelectedIndexChanged(object sender, EventArgs e)
        {
            AbilityBump((System.Windows.Forms.ComboBox)sender);
        }

        private void cbBump14_SelectedIndexChanged(object sender, EventArgs e)
        {
            AbilityBump((System.Windows.Forms.ComboBox)sender);
        }
        private void cbBump15_SelectedIndexChanged(object sender, EventArgs e)
        {
            AbilityBump((System.Windows.Forms.ComboBox)sender);
        }

        private void labelModStr_TextChanged(object sender, EventArgs e)
        {
            //CalculateAbility();
        }

        private void increaseModStr_TextChanged(object sender, EventArgs e)
        {
            CalcAbilityScore();
        }

        private void increaseModDex_TextChanged(object sender, EventArgs e)
        {
            CalcAbilityScore();
        }

        private void increaseModCon_TextChanged(object sender, EventArgs e)
        {
            CalcAbilityScore();
        }

        private void increaseModInt_TextChanged(object sender, EventArgs e)
        {
            CalcAbilityScore();
        }

        private void increaseModWis_TextChanged(object sender, EventArgs e)
        {
            CalcAbilityScore();
        }

        private void increaseModCha_TextChanged(object sender, EventArgs e)
        {
            CalcAbilityScore();
        }

        private void txtMagicBonusStr_TextChanged(object sender, EventArgs e)
        {
            CalcAbilityScore();
        }

        private void txtMagicBonusDex_TextChanged(object sender, EventArgs e)
        {
            CalcAbilityScore();
        }

        private void txtMagicBonusCon_TextChanged(object sender, EventArgs e)
        {
            CalcAbilityScore();
        }

        private void txtMagicBonusInt_TextChanged(object sender, EventArgs e)
        {
            CalcAbilityScore();
        }

        private void txtMagicBonusWis_TextChanged(object sender, EventArgs e)
        {
            CalcAbilityScore();
        }

        private void txtMagicBonusCha_TextChanged(object sender, EventArgs e)
        {
            CalcAbilityScore();
        }
    }
}
