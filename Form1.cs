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

            SalvaEscolhaLinguagem(cBLanguage.Text, cBLanguage.SelectedIndex);
            
            this.Controls.Clear();
            InitializeComponent();

        }
        private void SalvaEscolhaLinguagem(string linguagem, Int32 inteiro)
        {   
            Properties.Settings.Default.Linguagem = linguagem;            
            Properties.Settings.Default.LinguagemIndice = inteiro;
            Properties.Settings.Default.Save();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
            cBLanguage.SelectedIndex = Properties.Settings.Default.LinguagemIndice;
            
            
            
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
                if (Properties.Settings.Default.LinguagemIndice != null && Properties.Settings.Default.LinguagemIndice != -1)
                {
                    switch (Properties.Settings.Default.LinguagemIndice)
                    {
                        case 0:
                            MessageBox.Show("Unable to open link that was clicked.");
                            break;
                        case 1:
                            MessageBox.Show("Não é possível abrir o link que foi clicado.");
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Unable to open link that was clicked.");
                }
                
            }
        }

        private void VisitLink()
        {
            // Change the color of the link text by setting LinkVisited
            // to true.
            linkLabel1.LinkVisited = true;
            //Call the Process.Start method to open the default browser
            //with a URL:
            if (Properties.Settings.Default.LinguagemIndice != null && Properties.Settings.Default.LinguagemIndice != -1)
            {
                switch (Properties.Settings.Default.LinguagemIndice)
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


        private async void RandomAge()
        {
            int ageMod = 0, hgtMod = 0, wgtMod = 0;

            //if (HasAgeInfo.Value)
            //{
            ageMod = RollDice(ageNumDice, ageDieType);
            //string raca = LookupRace("Human");
            string BaseAgeRace = await LookupRaceAsync("Base Age", "Human");

            lblRandomAge.Text = BaseAgeRace + ageMod;

            //}
            //else
            //{
            //    lblRandomAge.Text = 0;
            //}

            //if (HasHeightWeightInfo.Value)
            //{
            //    hgtMod = RollDice(hgtNumDice.Value, hgtDieType.Value);
            //    RandomHeight.Value = baseHgt.Value + hgtMod;
            //    wgtMod = hgtMod * RollDice(wgtNumDice.Value, wgtDieType.Value);
            //    RandomWeight.Value = baseWgt.Value + wgtMod;
            //}
            //else
            //{
            //    RandomHeight.Value = 0;
            //    RandomWeight.Value = 0;
            //}
        }

        private int RollDice(int numDice, int dieType)
        {
            Random random = new Random();
            int total = 0;
            for (int i = 0; i < numDice; i++)
            {
                total += random.Next(1, dieType + 1);
            }
            return total;
        }

        private const int ageNumDice = 2;
        private const int ageDieType = 4;

        //string LookupRace(string key)
        //{
        //    Dictionary<string, string> TblCreatureInfoExt = new Dictionary<string, string>();
        //    TblCreatureInfoExt.Add("key1", "value1");
        //    TblCreatureInfoExt.Add("key2", "value2");
        //    // Adicione mais pares de valores chave-valor conforme necessário

        //    string value;
        //    if (TblCreatureInfoExt.TryGetValue(key, out value))
        //    {
        //        return value;
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}

        static async Task<string> ReadAllTextAsync(string path)
        {
            switch (path)
            {
                case "": throw new ArgumentException("Empty path name is not legal.", nameof(path));
                case null: throw new ArgumentNullException(nameof(path));
            }

            var sourceStream = new FileStream(path, FileMode.Open,
                FileAccess.Read, FileShare.Read,
                bufferSize: 4096,
                useAsync: true);
            var streamReader = new StreamReader(sourceStream, Encoding.UTF8,
                detectEncodingFromByteOrderMarks: true);
            // detectEncodingFromByteOrderMarks allows you to handle files with BOM correctly. 
            // Otherwise you may get chinese characters even when your text does not contain any

            return await streamReader.ReadToEndAsync();
        }

        async Task<string> LookupRaceAsync(string key, string race)
        {
            string path = @"C:\Users\cristiano.lagame\source\repos\HeroForge-OnceAgain2\HeroForge-OnceAgain\CreatureInfo.xlsx";
            string conteudo = "";
            var indexLinha = 0;

            //string path = @"C:\Users\cristiano.lagame\source\repos\HeroForge-OnceAgain2\HeroForge-OnceAgain\CreatureInfo.xlsx";
            var workbook = new XLWorkbook(path);
            var sheet = workbook.Worksheets.First();
            //var worksheet = workbook.Worksheet(1);

            if (!validaCabecalho(sheet))
                throw new Exception("Cabeçalho incorreto");

            var totalLines = sheet.Rows().Count();


            //var lookupRange = planilha.Range("$A$1:$BQ$523");
            for (int l = 2; l <= totalLines; l++)
            {
                indexLinha = l;
                var _lineSheet = sheet.Row(indexLinha);
                var strNameRace = _lineSheet.Cell($"A").Value.ToString();
                var strBaseAge = "";
                if (strNameRace == race)
                {
                    strBaseAge = _lineSheet.Cell($"BB").Value.ToString();
                    workbook.Dispose();
                    return strBaseAge;
                }
            }

            //foreach (var cell in lookupRange.CellsUsed())
            //{
            //    if (cell.GetString() == key)
            //    {
            //        string value = cell.CellRight().GetString();
            //        workbook.Dispose();
            //        return value;
            //    }
            //}
            workbook.Dispose();
            return "";


            //var arquivo = ReadAllTextAsync(path);

            //using (var excelWorkbook = new XLWorkbook(path))
            //{
            //    var nonEmptyDataRows = excelWorkbook.Worksheet(1).RowsUsed();

            //    foreach (var dataRow in nonEmptyDataRows)
            //    {
            //        //for row number check
            //        if (dataRow.RowNumber() >= 3 && dataRow.RowNumber() <= 20)
            //        {
            //            //to get column # 3's data
            //            var cell = dataRow.Cell(3).Value;
            //            conteudo = cell.ToString();
            //        }
            //    }
            //}
            //return conteudo;

            //    using (var workbook = new XLWorkbook(arquivo))
            //    {

            //    }







            //    Workbook workbook = app.Workbooks.Open(path);
            //    Worksheet worksheet = workbook.Worksheets[1];
            //    Range lookupRange = worksheet.Range["$A$1:$BQ$523"];
            //    Range lookupCell = lookupRange.Find(key, Type.Missing, XlFindLookIn.xlValues, XlLookAt.xlWhole, XlSearchOrder.xlByRows, XlSearchDirection.xlNext, false, false, Type.Missing);
            //    if (lookupCell != null)
            //    {
            //        string value = lookupCell.Offset[0, 1].Value.ToString();
            //        workbook.Close(false, Type.Missing, Type.Missing);
            //        app.Quit();
            //        return value;
            //    }
            //    else
            //    {
            //        workbook.Close(false, Type.Missing, Type.Missing);
            //        app.Quit();
            //        return "";
            //    }
        }

        private bool validaCabecalho(IXLWorksheet plan)
        {
            if (plan.Cell($"A{1}").Value.ToString().Trim().ToUpper() != "RACE"
                )
                return false;
            else
                return true;

        }

        public static string LookupRace(string key)
        {
            string path = @"C:\Users\cristiano.lagame\source\repos\HeroForge-OnceAgain2\HeroForge-OnceAgain\CreatureInfo.xlsx";
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<CsvRecord>();
                foreach (var record in records)
                {
                    if (record.Race == key)
                    {
                        return record.BaseAge;
                    }
                }
            }
            return "";
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


    }
}
