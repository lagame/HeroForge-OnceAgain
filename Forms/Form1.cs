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
using static System.Net.Mime.MediaTypeNames;
using System.IO;
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
using DocumentFormat.OpenXml.Bibliography;
using ClosedXML.Excel;
using MaterialSkin.Controls;

namespace HeroForge_OnceAgain
{
    public partial class Form1 : MaterialForm
    {
        private Character character;
        //private System.Windows.Forms.Button printButton = new System.Windows.Forms.Button();
        private PrintDocument printDocument1 = new PrintDocument();
        Bitmap memoryImage;
        private Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            
            // Inicialização do botão

            btnRolar.Text = "Rolar dados";
            btnRolar.Location = new System.Drawing.Point(500, 25); // Posição inicial
            btnRolar.Size = new System.Drawing.Size(110, 50);   // Tamanho inicial
            this.Controls.Add(btnRolar);  // Nome alterado

            CharacterCreationInfo creationInfo = new CharacterCreationInfo();
            character = new Character(creationInfo);

            
            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            
        }
        public void Form1_Load(object sender, EventArgs e)
        {
            

            if (workSheet == null)                
                workSheet = LoadFileFromResource();
                //workSheet = LoadFile();

            Reload();
            RaceUtils.PopulateRaceComboBox(cbRaces, Properties.Settings.Default.LanguageIndex);
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

            LoadSupplementSources();
        }

        private void LoadSupplementSources()
        {
            // Caminho para a pasta de suplementos
            string supplementsFolderPath = System.Windows.Forms.Application.StartupPath.Replace("\\bin\\Debug", "") + "\\Resources\\Supplements";

            // Verifica se existem arquivos JSON na pasta
            if (Directory.Exists(supplementsFolderPath))
            {
                string[] jsonFiles = Directory.GetFiles(supplementsFolderPath, "*.json");

                if (jsonFiles.Length > 0)
            {
                    // Carrega as fontes de suplementos da pasta
                    List<SupplementSource> sources = LoadSourcesFromFolder(supplementsFolderPath);
                    if (sources != null)
                    {
                        //if (listViewDragonLanceSources != null)
                        {
                            dataGridViewDragonLanceSources.AutoGenerateColumns = false;
                            
                            dataGridViewDragonLanceSources.BackgroundColor = System.Drawing.Color.White; // Define o fundo branco
                            dataGridViewDragonLanceSources.DefaultCellStyle.BackColor = System.Drawing.Color.White; // Define a cor de fundo das células
                            dataGridViewDragonLanceSources.RowHeadersVisible = false; // Torna as linhas de grade invisíveis

                            // Adicione a coluna de checkboxes
                            DataGridViewCheckBoxColumn checkboxColumn = new DataGridViewCheckBoxColumn();
                            checkboxColumn.HeaderText = "";
                            checkboxColumn.Name = "CheckboxColumn";
                            checkboxColumn.Width = 20;

                            dataGridViewDragonLanceSources.RowsDefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8);                            
                            dataGridViewDragonLanceSources.Columns.Add(checkboxColumn);
                            dataGridViewDragonLanceSources.GridColor = dataGridViewDragonLanceSources.BackgroundColor;

                            // Adiciona uma coluna para exibir o campo "SupplementName"
                            DataGridViewTextBoxColumn supplementNameColumn = new DataGridViewTextBoxColumn();
                            supplementNameColumn.DataPropertyName = "SupplementName"; // O nome da propriedade no seu objeto de dados
                            supplementNameColumn.HeaderText = "Supplement Name"; // O cabeçalho da coluna
                            supplementNameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            supplementNameColumn.ReadOnly = true;
                            dataGridViewDragonLanceSources.Columns.Add(supplementNameColumn);

                            // Remove os cabeçalhos de coluna
                            dataGridViewDragonLanceSources.ColumnHeadersVisible = false;

                            // Definindo a fonte de dados
                            dataGridViewDragonLanceSources.DataSource = sources;

                            //foreach (var source in sources)
                            //{
                            //    ListViewItem item = new ListViewItem();
                            //    item.Text = source.SupplementName;
                            //    item.SubItems.Add(source.SupplementSourceAbbreviation);
                            //    listViewDragonLanceSources.Items.Add(item);
                            //}
                            //listViewDragonLanceSources.Invalidate();
                            //DataTable dt = new DataTable();
                            //dt.Columns.Add(new DataColumn("column1"));
                            //DataRow row = dt.NewRow();
                            //dataGridView1.AutoGenerateColumns = false;
                            //dataGridView1.Columns.Add("Name", "Name");
                            //foreach (var item in sources)
                            //{
                            //    //row[0] = item.SupplementName;
                            //    dataGridView1.Rows.Add(item.SupplementName);
                            //}
                            //dt.Rows.Add(row);
                            //dataGridView1.DataSource = dt;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Nenhum arquivo JSON encontrado na pasta de suplementos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }


        }

        // Método para carregar as fontes de um arquivo JSON        
        public static List<SupplementSource> LoadFromJson(string filePath)
        {
            try
            {
                string jsonContent = File.ReadAllText(filePath);

                // Deserializa o JSON para uma lista de SupplementSource
                List<SupplementSource> sources = JsonConvert.DeserializeObject<List<SupplementSource>>(jsonContent);

                return sources;
            }
            catch (Exception ex)
            {
                // Lida com qualquer erro ao ler os arquivos JSON
                MessageBox.Show($"Erro ao ler {filePath}: {ex.Message}");
                return new List<SupplementSource>();
            }
        }
        public static List<SupplementSource> LoadSourcesFromFolders(string rootFolder)
        {
            List<SupplementSource> allSources = new List<SupplementSource>();

            try
            {
                // Obtém todas as pastas dentro do diretório rootFolder
                string[] supplementFolders = Directory.GetDirectories(rootFolder);

                // Para cada pasta, carrega os arquivos JSON e adiciona as fontes
                foreach (string supplementFolder in supplementFolders)
                {
                    List<SupplementSource> sourcesInFolder = LoadSourcesFromFolder(supplementFolder);
                    allSources.AddRange(sourcesInFolder);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar fontes: {ex.Message}");
            }

            return allSources;
        }

        public static List<SupplementSource> LoadSourcesFromFolder(string folderPath)
        {
            List<SupplementSource> sources = new List<SupplementSource>();

            try
            {
                // Obtém todos os arquivos JSON dentro da pasta
                string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");

                // Para cada arquivo, carrega as fontes e adiciona à lista
                foreach (string jsonFile in jsonFiles)
                {
                    List<SupplementSource> sourcesFromFile = LoadFromJson(jsonFile);
                    sources.AddRange(sourcesFromFile);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar fontes: {ex.Message}");
            }

            return sources;
        }


        public void CalculateAbility(Label ability)
        {
            int mod = Convert.ToInt32(ability.Text);


            int strMod = Convert.ToInt32(labelModStr.Text);
            int dexMod = Convert.ToInt32(labelModDex.Text);
            int conMod = Convert.ToInt32(labelModCon.Text);
            int intMod = Convert.ToInt32(labelModInt.Text);
            int wisMod = Convert.ToInt32(labelModWis.Text);
            int chaMod = Convert.ToInt32(labelModCha.Text);

            int str = Convert.ToInt32(lblTotalStr.Text) ;
            int dex = Convert.ToInt32(lblTotalDex.Text) ;
            int con = Convert.ToInt32(lblTotalCon.Text) ;
            int inte = Convert.ToInt32(lblTotalInt.Text);
            int wis = Convert.ToInt32(lblTotalWis.Text) ;
            int cha = Convert.ToInt32(lblTotalCha.Text); 

            lblTotalStr.Text = (Convert.ToInt32(InitialStr.Value) + strMod).ToString();
            lblTotalDex.Text = (Convert.ToInt32(InitialDex.Value) + dexMod).ToString();
            lblTotalCon.Text = (Convert.ToInt32(InitialCon.Value) + conMod).ToString();
            lblTotalInt.Text = (Convert.ToInt32(InitialInt.Value) + intMod).ToString();
            lblTotalWis.Text = (Convert.ToInt32(InitialWis.Value) + wisMod).ToString();
            lblTotalCha.Text = (Convert.ToInt32(InitialCha.Value) + chaMod).ToString();

            lblModStrTotal.Text = CalcAttributeTotal(str).ToString();
            lblModDexTotal.Text = CalcAttributeTotal(dex).ToString();
            lblModConTotal.Text = CalcAttributeTotal(con).ToString();
            lblModIntTotal.Text = CalcAttributeTotal(inte).ToString();
            lblModWisTotal.Text = CalcAttributeTotal(wis).ToString();
            lblModChaTotal.Text = CalcAttributeTotal(cha).ToString();
        }

        private int CalcAttributeTotal(int valAttrib)
        {
            if (valAttrib < 1 || valAttrib > 99)
            {
                throw new ArgumentException("Valor da habilidade inválido.");
            }

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
            //Localization();
            
            UpdateLabels();

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
        }

        public void ClearStatsDescriptionSelections()
        {

            var confirmResult = MessageBox.Show(LocalizationUtils.L("AreYouSureClearForm1Stats"),
                                     LocalizationUtils.L("ConfirmClear"),
                                     MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                InitialStr.Value = 8;
                InitialDex.Value = 8;
                InitialCon.Value = 8;
                InitialInt.Value = 8;
                InitialWis.Value = 8;
                InitialCha.Value = 8;

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
            //RaceUtils.PopulateRaceComboBox(cbRaces, 3);
            InitializeComponent();

            //LoadMenuTranslations();

            RaceUtils.PopulateRaceComboBox(cbRaces, Properties.Settings.Default.LanguageIndex);
            cbAlignment.SelectedIndex = 0;
        }

        private void menuPreferences_Click(object sender, EventArgs e)
        {
            using (var prefsForm = new Preferences())
            {
                prefsForm.ShowDialog(this);
                Reload(); // Se quiser recarregar configurações após salvar
            }
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
            if (!(cbRaces.SelectedItem is Race selectedRace)) return;

            Race raceInfo = GetRaceInfoById(selectedRace.Id);
            if (raceInfo == null || string.IsNullOrWhiteSpace(raceInfo.BaseAge))
            {
                lblRandomAge.Text = "0";
                return;
            }

            int ageBasic = 0, ageMod = 0;
            var ageRace = raceInfo.BaseAge.Split('/').ToList();
            var classe = lblClasses.Text;

            if (string.IsNullOrWhiteSpace(classe))
            {
                lblRandomAge.Text = "0";
                return;
            }

            var vetorClasse = classe.Split('/');
            foreach (var item in vetorClasse)
            {
                var ageString = ageRace[0];
                var age = ageString.Split('+');
                ageBasic = Convert.ToInt32(age[0]);

                var itemUpper = item.ToUpperInvariant();

                if (itemUpper.Contains("BARBARIAN") || itemUpper.Contains("ROGUE") || itemUpper.Contains("SORCERER"))
                {
                    ageMod = RollDice(age[1]);
                }
                else if (itemUpper.Contains("BARD") || itemUpper.Contains("FIGHTER") || itemUpper.Contains("PALADIN") || itemUpper.Contains("RANGER"))
                {
                    ageMod = RollDice(ageRace[1]);
                }
                else if (itemUpper.Contains("CLERIC") || itemUpper.Contains("DRUID") || itemUpper.Contains("MONK") || itemUpper.Contains("WIZARD"))
                {
                    ageMod = RollDice(ageRace[2]);
                }
            }

            lblRandomAge.Text = (ageBasic + ageMod).ToString();
        }


        //private void RandomAge()
        //{
        //    int agebasic = 0;
        //    int ageMod = 0;
        //    string textLookup = lblRace.Text;
        //    List<string> AgeRace = new List<string>();

        //    Race race = RaceUtils.GetOriginalRace(textLookup);

        //    string BaseAgeRace = "";
        //    if (race != null)
        //    {
        //        BaseAgeRace = LookupInfo("AR", race.DisplayName, "Race Info");
        //    }


        //    if (string.IsNullOrEmpty(BaseAgeRace))
        //    {
        //        lblRandomAge.Text = "0";
        //        return;
        //    }

        //    AgeRace = BaseAgeRace.Split('/').ToList();
        //    var classe = lblClasses.Text;

        //    if (string.IsNullOrEmpty(classe))
        //    {
        //        lblRandomAge.Text = "0";
        //        return;
        //    }

        //    var vetorClasse = classe.Split('/');
        //    foreach (var item in vetorClasse)
        //    {
        //        var AgeString = AgeRace[0];
        //        var Age = AgeString.Split('+').ToList();
        //        agebasic = Convert.ToInt32(Age[0]);
        //        var item2 = item.ToUpper();

        //        if (item2.Contains("BARBARIAN") || item2.Contains("ROGUE") || item2.Contains("SORCERER"))
        //        {
        //            if (!string.IsNullOrEmpty(AgeRace[0]))
        //            {
        //                string Dice = Age[1];
        //                ageMod = RollDice(Dice);
        //            }
        //        }
        //        else if (item2.Contains("BARD") || item2.Contains("FIGHTER") || item2.Contains("PALADIN") || item2.Contains("RANGER"))
        //        {
        //            string AgeString2 = AgeRace[1];
        //            ageMod = RollDice(AgeString2);
        //        }
        //        else if (item2.Contains("CLERIC") || item2.Contains("DRUID") || item2.Contains("MONK") || item2.Contains("WIZARD"))
        //        {
        //            string AgeString2 = AgeRace[2];
        //            ageMod = RollDice(AgeString2);
        //        }
        //    }

        //    lblRandomAge.Text = (agebasic + ageMod).ToString();
        //}


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
                //Random random = new Random();
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

        private XLWorkbook LoadFileFromResource()
        {
            // Caminho para o arquivo no sistema de arquivos
            string filePath = System.Windows.Forms.Application.StartupPath.Replace("\\bin\\Debug", "") + "\\Resources\\baseInfo.xlsm";

            if (File.Exists(filePath))
            {
                using (FileStream fileStream = File.OpenRead(filePath))
                {
                    // Crie o XLWorkbook diretamente do FileStream
                    var workbook = new XLWorkbook(fileStream);
                    return workbook;
                }
            }
            else
            {
                throw new Exception("Arquivo não encontrado.");
            }
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
            HttpListenerService.Instance.StopListener();
            //this.Hide();
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
            if (!(cbRaces.SelectedItem is Race selectedRace)) return 0;

            Race raceInfo = GetRaceInfoById(selectedRace.Id);
            if (raceInfo == null || string.IsNullOrWhiteSpace(raceInfo.Height))
            {
                lblRandomHeight.Text = "0";
                return 0;
            }

            var heightParts = raceInfo.Height.Split('/');
            var gender = lblGender.Text;

            string baseHeight = gender.Equals("Male", StringComparison.OrdinalIgnoreCase) ? heightParts[0] : heightParts[1].Split('+')[0];
            string dice = heightParts[1].Split('+')[1];

            int hgtMod = CalcHeight(dice, baseHeight);
            return hgtMod;
        }


        //private int RandomHeight()
        //{
        //    int ageMod = 0, hgtMod = 0, wgtMod = 0;

        //    string textLookup = lblRace.Text;

        //    Race race = RaceUtils.GetOriginalRace(textLookup);

        //    string BaseHeightRace = "";
        //    if (race != null)
        //    {
        //        BaseHeightRace = LookupInfo("AS", race.DisplayName, "Race Info");
        //    }

        //    if (BaseHeightRace != null && !string.IsNullOrEmpty(BaseHeightRace))
        //    {
        //        var HeightRace = BaseHeightRace.Split('/').ToList();
        //        var gender = lblGender.Text;

        //        if (!string.IsNullOrEmpty(BaseHeightRace.Trim()))
        //        {

        //            if (!string.IsNullOrEmpty(gender))
        //            {
        //                var vetorGender = HeightRace[1].Split('+');
        //                string Height = "";
        //                if (gender.Equals("Male"))
        //                {
        //                    Height = HeightRace[0];
        //                }
        //                else
        //                {
        //                    Height = vetorGender[0];
        //                }

        //                if (!string.IsNullOrEmpty(Height))
        //                {
        //                    Height = Height.Replace("\\", "");
        //                    string Dice = vetorGender[1];
        //                    var Mod = RollDice(Dice);
        //                    hgtMod = CalcHeight(Dice, Height);
        //                }
        //            }


        //        }
        //        else
        //        {
        //            lblRandomHeight.Text = "0";
        //        }
        //    }
        //    return hgtMod;
        //}


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

        public class SupplementSource
        {
            public string SupplementSourceAbbreviation { get; set; }
            public string SupplementName { get; set; }
            public string DisplayLanguage { get; set; }

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

        public Race GetRaceInfoById(int id)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "RaceInfo.json");

            if (!File.Exists(path))
            {
                MessageBox.Show($"Arquivo não encontrado: {path}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            string json = File.ReadAllText(path);
            var raceInfos = JsonConvert.DeserializeObject<List<Race>>(json);

            return raceInfos.FirstOrDefault(r => r.Id == id);
        }

        private void RandomWeight(int hgtBase)
        {
            if (!(cbRaces.SelectedItem is Race selectedRace)) return;

            Race raceInfo = GetRaceInfoById(selectedRace.Id);
            if (raceInfo == null || string.IsNullOrWhiteSpace(raceInfo.Weight))
            {
                lblRandomAge.Text = "0";
                return;
            }

            var weightParts = raceInfo.Weight.Split('/');
            var gender = lblGender.Text;

            string baseWeight = gender.Equals("Male", StringComparison.OrdinalIgnoreCase)
                ? weightParts[0]
                : weightParts[1].Split('+')[0];

            string dice = weightParts[1].Split('+')[1];

            CalcWeight(dice, baseWeight, hgtBase);
        }


        //private void RandomWeight(int hgtBase)
        //{
        //    int ageMod = 0, wgtMod = 0;

        //    string textLookup = lblRace.Text;

        //    Race race = RaceUtils.GetOriginalRace(textLookup);

        //    string BaseWeightRace = "";
        //    if (race != null)
        //    {
        //        BaseWeightRace = LookupInfo("AT", race.DisplayName, "Race Info");
        //    }

        //    if (BaseWeightRace != null && !string.IsNullOrEmpty(BaseWeightRace))
        //    {
        //        var WeightRace = BaseWeightRace.Split('/').ToList();
        //        var gender = lblGender.Text;

        //        if (!string.IsNullOrEmpty(BaseWeightRace.Trim()))
        //        {
        //            if (!string.IsNullOrEmpty(gender))
        //            {
        //                var vetorGender = WeightRace[1].Split('+');
        //                string Weight = "";
        //                if (gender.Equals("Male"))
        //                {
        //                    Weight = WeightRace[0];
        //                }
        //                else
        //                {
        //                    Weight = vetorGender[0];
        //                }

        //                if (!string.IsNullOrEmpty(Weight))
        //                {
        //                    Weight = Weight.Replace("\\", "");
        //                    string Dice = vetorGender[1];
        //                    var Mod = RollDice(Dice);
        //                    CalcWeight(Dice, Weight, hgtBase);
        //                }
        //            }


        //        }
        //        else
        //        {
        //            lblRandomAge.Text = "0";
        //        }
        //    }
        //}


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

            if (cbRaces.SelectedItem is Race selectedRace)
            {
                lblRace.Text = selectedRace.DisplayName;

                // Carrega os dados completos da raça a partir do RaceInfo.json
                Race info = GetRaceInfoById(selectedRace.Id);

                if (info != null)
                {
                    lblRace.Text = info.DisplayName;
                    //lblRaceType.Text = info.Type ?? "-";
                    //lblRaceSubtype.Text = info.Subtype ?? "-";
                    //lblRaceSize.Text = info.Size ?? "-";

                    //lblRaceHD.Text = info.HD?.ToString() ?? "-";
                    //lblRaceStr.Text = info.Str?.ToString() ?? "-";
                    //lblRaceDex.Text = info.Dex?.ToString() ?? "-";
                    //lblRaceCon.Text = info.Con?.ToString() ?? "-";
                    //lblRaceInt.Text = info.Int?.ToString() ?? "-";
                    //lblRaceWis.Text = info.Wis?.ToString() ?? "-";
                    //lblRaceCha.Text = info.Cha?.ToString() ?? "-";

                    //lblRaceNaturalArmor.Text = info.NaturalArmor?.ToString() ?? "-";
                    //lblRaceNaturalAttacks.Text = info.NaturalAttacks ?? "-";
                    //lblRaceSpecialAttacks.Text = info.SpecialAttacks ?? "-";

                    //lblRaceSpellLikeAbilities.Text = info.SpellLikeAbilities ?? "-";
                    //lblRacePsionicAbilities.Text = info.PsionicAbilities ?? "-";
                    //lblRaceOtherSpecialAbilities.Text = info.OtherSpecialAbilities ?? "-";

                    //lblRaceDarkvision.Text = info.Darkvision ?? "-";
                    //lblRaceLowLightVision.Text = info.LowLightVision ?? "-";
                    //lblRaceOtherSenses.Text = info.OtherSenses ?? "-";

                    //lblRaceImmunities.Text = info.Immunities ?? "-";
                    //lblRaceVulnerabilities.Text = info.Vulnerabilities ?? "-";
                    //lblRaceEnergyResistance.Text = info.EnergyResistance ?? "-";
                    //lblRaceSpellResistance.Text = info.SpellResistance ?? "-";
                    //lblRaceDamageReduction.Text = info.DamageReduction ?? "-";
                    //lblRaceFastHealing.Text = info.FastHealing ?? "-";
                    //lblRaceBonusEssentia.Text = info.BonusEssentia ?? "-";
                    //lblRaceOtherSpecialQualities.Text = info.OtherSpecialQualities ?? "-";

                    //lblRaceLand.Text = info.Land?.ToString() ?? "-";
                    //lblRaceBurrow.Text = info.Burrow?.ToString() ?? "-";
                    //lblRaceClimb.Text = info.Climb?.ToString() ?? "-";
                    //lblRaceFly.Text = info.Fly?.ToString() ?? "-";
                    //lblRaceManeuver.Text = info.Maneuver ?? "-";
                    //lblRaceSwim.Text = info.Swim?.ToString() ?? "-";

                    //lblRaceRacialSkills.Text = info.RacialSkills ?? "-";
                    //lblRaceBonusFeats.Text = info.BonusFeats ?? "-";

                    //lblRaceAutoLang.Text = info.AutomaticLanguages ?? "-";
                    //lblRaceBonusLang.Text = info.BonusLanguages ?? "-";

                    //lblRaceCRAdj.Text = info.CRAdj?.ToString() ?? "-";
                    //lblRaceLevelAdj.Text = info.LevelAdj?.ToString() ?? "-";
                    //lblRaceAlignment.Text = info.Alignment ?? "-";
                    //lblRaceFavoredClass.Text = info.FavoredClass ?? "-";

                    //lblRaceBaseAge.Text = info.BaseAge ?? "-";
                    //lblRaceHeight.Text = info.Height ?? "-";
                    //lblRaceWeight.Text = info.Weight ?? "-";

                    //lblRacePrereq.Text = info.PrerequisitesDescription ?? "-";

                    //lblRaceSrc.Text = info.Src ?? "-";
                    //lblRacePg.Text = info.Pg?.ToString() ?? "-";
                    //lblRaceAltSrc.Text = info.AltSrc ?? "-";

                }
            }
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
            int totalStr = Convert.ToInt32(InitialStr.Value) + CalculaCampo(labelModStr.Text) +
                 CalculaCampo(IncreaseModStr.Text) + CalculaCampo(MagicBonusStr.Text);
            lblTotalStr.Text = totalStr.ToString();

            int totalDex = Convert.ToInt32(InitialDex.Value) + CalculaCampo(labelModDex.Text) +
                CalculaCampo(IncreaseModDex.Text) + CalculaCampo(MagicBonusDex.Text);
            lblTotalDex.Text = totalDex.ToString();

            int totalCon = Convert.ToInt32(InitialCon.Text) + CalculaCampo(labelModCon.Text) +
                CalculaCampo(IncreaseModCon.Text) + CalculaCampo(MagicBonusCon.Text);
            lblTotalCon.Text = totalCon.ToString();

            int totalInt = Convert.ToInt32(InitialInt.Text) + CalculaCampo(labelModInt.Text) +
                CalculaCampo(IncreaseModInt.Text) + CalculaCampo(MagicBonusInt.Text);
            lblTotalInt.Text = totalInt.ToString();

            int totalWis = Convert.ToInt32(InitialWis.Text) + CalculaCampo(labelModWis.Text) +
                CalculaCampo(IncreaseModWis.Text) + CalculaCampo(MagicBonusWis.Text);
            lblTotalWis.Text = totalWis.ToString();

            int totalCha = Convert.ToInt32(InitialCha.Text) + CalculaCampo(labelModCha.Text) +
                CalculaCampo(IncreaseModCha.Text) + CalculaCampo(MagicBonusCha.Text);
            lblTotalCha.Text = totalCha.ToString();
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
            CalculatePointBuy();
            int totalAttrib = (Convert.ToInt32(lblModStr.Text) + Convert.ToInt32(lblModDex.Text) + Convert.ToInt32(lblModCon.Text) + Convert.ToInt32(lblModInt.Text) + Convert.ToInt32(lblModWis.Text) + Convert.ToInt32(lblModCha.Text));
            var selectedItem = cBAbilityScoreSystem.SelectedIndex;
            
            EventHandler newClickEvent;

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

                    //newClickEvent = (s, args) => { rolar3d6(s, args); };
                    newClickEvent = (s, args) => { rolar4d6DropLowest(s, args); };
                    UpdateButton(btnRolar, new Size(110, 50), new Point(500, 25), "Rolar Jogada Flutuante", newClickEvent);                   

                    break;
                case 4:
                    lblTotalPoints.Visible = false;
                    lblPointBuy.Visible = false;
                    labelTypeCampaign.Visible = false;
                    lblTypeCampaign.Visible = false;
                    lblTotalPoints.Text = "";
                    lblAlternativeRoll.Text = "";

                    lblAlternativeRoll.Text = LocalizationUtils.L("EliteArrayText");
                    newClickEvent = (s, args) => { rolarOrganic(s, args); };
                    
                    UpdateButton(btnRolar, new Size(110, 50), new Point(500, 25), "Rolar Personagens Orgânicos", newClickEvent);

                    break;
                case 5:
                    lblTotalPoints.Visible = false;
                    lblPointBuy.Visible = false;
                    labelTypeCampaign.Visible = false;
                    lblTypeCampaign.Visible = false;
                    lblTotalPoints.Text = "";
                    lblAlternativeRoll.Text = "";
                    newClickEvent = (s, args) => { rolar3d6(s, args); };

                    UpdateButton(btnRolar, new Size(110, 50), new Point(500, 25), "Rolar Personagens Padrão Personalizados", newClickEvent);
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
                    lblTotalStr.Text = (Convert.ToInt32(InitialStr.Value) + Convert.ToInt32(labelModStr.Text)).ToString();                    
                    lblModStrTotal.Text = CalcAttributeTotal(Convert.ToInt32(lblTotalStr.Text)).ToString();                    
                    break;
                case 2:
                    labelModDex.Visible = true;
                    labelModDex.Text = (Convert.ToInt32(labelModDex.Text) + 1).ToString();                    
                    lblTotalDex.Text = (Convert.ToInt32(InitialDex.Value) + Convert.ToInt32(labelModDex.Text)).ToString();                    
                    lblModDexTotal.Text = CalcAttributeTotal(Convert.ToInt32(lblTotalDex.Text)).ToString();
                    break;
                case 3:
                    labelModCon.Visible = true;
                    labelModCon.Text = (Convert.ToInt32(labelModCon.Text) + 1).ToString();                    
                    lblTotalCon.Text = (Convert.ToInt32(InitialCon.Value) + Convert.ToInt32(labelModCon.Text)).ToString();                    
                    lblModConTotal.Text = CalcAttributeTotal(Convert.ToInt32(lblTotalCon.Text)).ToString();
                    break;
                case 4:
                    labelModInt.Visible = true;                    
                    labelModInt.Text = (Convert.ToInt32(labelModInt.Text) + 1).ToString();
                    lblTotalInt.Text = (Convert.ToInt32(InitialInt.Value) + Convert.ToInt32(labelModInt.Text)).ToString();
                    lblModIntTotal.Text = CalcAttributeTotal(Convert.ToInt32(lblTotalInt.Text)).ToString();
                    break;
                case 5:
                    labelModWis.Visible = true;                    
                    labelModWis.Text = (Convert.ToInt32(labelModWis.Text) + 1).ToString();
                    lblTotalWis.Text = (Convert.ToInt32(InitialWis.Value) + Convert.ToInt32(labelModWis.Text)).ToString();
                    lblModWisTotal.Text = CalcAttributeTotal(Convert.ToInt32(lblTotalWis.Text)).ToString();
                    break;
                case 6:
                    labelModCha.Visible = true;
                    labelModCha.Visible = true;
                    labelModCha.Text = (Convert.ToInt32(labelModCha.Text) + 1).ToString();
                    lblTotalCha.Text = (Convert.ToInt32(InitialCha.Value) + Convert.ToInt32(labelModCha.Text)).ToString();
                    lblModChaTotal.Text = CalcAttributeTotal(Convert.ToInt32(lblTotalCha.Text)).ToString();
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

        private void AtualizarAtributo(NumericUpDown num, System.Windows.Forms.ComboBox[] combos, System.Windows.Forms.TextBox[] ganhoInerente, System.Windows.Forms.TextBox[] ganhoMagico, Label label)
        {
            int valor = (int)num.Value;
            string atributo = label.Name.Substring(5); // obtém o nome do atributo a partir do nome da label

            // soma o valor dos combos, ganho inerente e ganho mágico para o atributo correspondente
            for (int i = 0; i < combos.Length; i++)
            {
                if (combos[i].SelectedIndex == 0) // se o combo selecionado for referente ao atributo correspondente
                {
                    valor += 1;
                }
            }
            for (int i = 0; i < ganhoInerente.Length; i++)
            {
                if (ganhoInerente[i].Name.EndsWith(atributo)) // se o campo de ganho inerente for referente ao atributo correspondente
                {
                    int ganho = 0;
                    int.TryParse(ganhoInerente[i].Text, out ganho);
                    if (ganho > 0 && ganho <= 5) // limita o ganho inerente em +5
                    {
                        valor += ganho;
                    }
                }
            }
            for (int i = 0; i < ganhoMagico.Length; i++)
            {
                if (ganhoMagico[i].Name.EndsWith(atributo)) // se o campo de ganho mágico for referente ao atributo correspondente
                {
                    int ganho = 0;
                    int.TryParse(ganhoMagico[i].Text, out ganho);
                    if (ganho > 0) // só soma o ganho mágico se ele for maior que 0
                    {
                        valor += ganho;
                    }
                }
            }

            // atualiza a label correspondente
            label.Text = valor.ToString();
        }
        public void UpdateLabels(params Label[] labels)
        {
            // Inicializar a soma das habilidades com seus valores iniciais
            int totalStr = (int)InitialStr.Value;
            int totalDex = (int)InitialDex.Value;
            int totalCon = (int)InitialCon.Value;
            int totalInt = (int)InitialInt.Value;
            int totalWis = (int)InitialWis.Value;
            int totalCha = (int)InitialCha.Value;

            // Adicionar o bônus das combos de aumento de habilidade
            Dictionary<int, string> bumpOptions = new Dictionary<int, string>()
            {
                { 1, "1" },
                { 2, "2" },
                { 3, "3" },
                { 4, "4" },
                { 5, "5" },
                { 6, "6" }
            };

            for (int i = 1; i <= 15; i++)
            {
                System.Windows.Forms.ComboBox cbBump = Controls.Find($"cbBump{i}", true).FirstOrDefault() as System.Windows.Forms.ComboBox;
                if (cbBump == null) continue;
                if (cbBump.SelectedItem != null)
                {
                    string selectedOption = cbBump.SelectedIndex.ToString();
                    if (bumpOptions.ContainsValue(selectedOption))
                    {
                        int bumpValue = bumpOptions.FirstOrDefault(x => x.Value == selectedOption).Key;
                        switch (bumpValue)
                        {
                            case 1:
                                totalStr += 1;
                                break;
                            case 2:
                                totalDex += 1;
                                break;
                            case 3:
                                totalCon += 1;
                                break;
                            case 4:
                                totalInt += 1;
                                break;
                            case 5:
                                totalWis += 1;
                                break;
                            case 6:
                                totalCha += 1;
                                break;
                            default:
                                break;
                        }
                    }
                }

            }

            // Adicionar o bônus dos campos de aumento inerente e mágico
            List<TextBox> increaseModTextBoxes = new List<TextBox>() { IncreaseModStr, IncreaseModDex, IncreaseModCon, IncreaseModInt, IncreaseModWis, IncreaseModCha };
            int[] increaseMods = new int[increaseModTextBoxes.Count];
            for (int i = 0; i < increaseModTextBoxes.Count; i++)
            {
                increaseMods[i] = CalculaCampo(increaseModTextBoxes[i].Text);
            }

            List<TextBox> magicModTextBoxes = new List<TextBox>() { MagicBonusStr, MagicBonusDex, MagicBonusCon, MagicBonusInt, MagicBonusWis, MagicBonusCha };
            int[] magicBonuses = new int[magicModTextBoxes.Count];
            for (int i = 0; i < magicModTextBoxes.Count; i++)
            {
                magicBonuses[i] = CalculaCampo(magicModTextBoxes[i].Text);
            }

            totalStr += increaseMods[0] + magicBonuses[0];
            totalDex += increaseMods[1] + magicBonuses[1];
            totalCon += increaseMods[2] + magicBonuses[2];
            totalInt += increaseMods[3] + magicBonuses[3];
            totalWis += increaseMods[4] + magicBonuses[4];
            totalCha += increaseMods[5] + magicBonuses[5];


            lblTotalStr.Text = totalStr.ToString();
            lblTotalDex.Text = totalDex.ToString();
            lblTotalCon.Text = totalCon.ToString();
            lblTotalInt.Text = totalInt.ToString();
            lblTotalWis.Text = totalWis.ToString();
            lblTotalCha.Text = totalCha.ToString();

            lblModStrTotal.Text = CalcAttributeTotal(totalStr).ToString();
            lblModDexTotal.Text = CalcAttributeTotal(totalDex).ToString();
            lblModConTotal.Text = CalcAttributeTotal(totalCon).ToString();
            lblModIntTotal.Text = CalcAttributeTotal(totalInt).ToString();
            lblModWisTotal.Text = CalcAttributeTotal(totalWis).ToString();
            lblModChaTotal.Text = CalcAttributeTotal(totalCha).ToString();

            lblModStr.Text = CalcAttribute(totalStr).ToString();
            lblModDex.Text = CalcAttribute(totalDex).ToString();
            lblModCon.Text = CalcAttribute(totalCon).ToString();
            lblModInt.Text = CalcAttribute(totalInt).ToString();
            lblModWis.Text = CalcAttribute(totalWis).ToString();
            lblModCha.Text = CalcAttribute(totalCha).ToString();
        }
        private void cbBump_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateLabels();
        }

        private void increaseModStr_TextChanged(object sender, EventArgs e)
        {
            UpdateLabels(); //CalcAbilityScore();
        }

        private void increaseModDex_TextChanged(object sender, EventArgs e)
        {
            UpdateLabels(); //CalcAbilityScore();
        }

        private void increaseModCon_TextChanged(object sender, EventArgs e)
        {
            UpdateLabels(); //CalcAbilityScore();
        }

        private void increaseModInt_TextChanged(object sender, EventArgs e)
        {
            UpdateLabels(); //CalcAbilityScore();
        }

        private void increaseModWis_TextChanged(object sender, EventArgs e)
        {
            UpdateLabels(); //CalcAbilityScore();
        }

        private void increaseModCha_TextChanged(object sender, EventArgs e)
        {
            UpdateLabels(); //CalcAbilityScore();
        }

        private void txtMagicBonusStr_TextChanged(object sender, EventArgs e)
        {
            UpdateLabels(); //CalcAbilityScore();
        }

        private void txtMagicBonusDex_TextChanged(object sender, EventArgs e)
        {
            UpdateLabels(); //CalcAbilityScore();
        }

        private void txtMagicBonusCon_TextChanged(object sender, EventArgs e)
        {
            UpdateLabels(); //CalcAbilityScore();
        }

        private void txtMagicBonusInt_TextChanged(object sender, EventArgs e)
        {
            UpdateLabels(); //CalcAbilityScore();
        }

        private void txtMagicBonusWis_TextChanged(object sender, EventArgs e)
        {
            UpdateLabels(); //CalcAbilityScore();
        }

        private void txtMagicBonusCha_TextChanged(object sender, EventArgs e)
        {
            UpdateLabels(); //CalcAbilityScore();
        }

        private void UpdateButton(Button button, Size size, Point location, string text, EventHandler newClickEvent)
        {
            button.Size = size;
            button.Location = location;
            button.Text = text;
            //button.Click -= SomeOtherEventHandler;  // Remover evento anterior
            button.Click += newClickEvent;  // Adicionar novo evento
        }

        private void rolarXd6(object sender, EventArgs e, int numDados)
        {
            string resultado = "Atributos: ";
            for (int i = 0; i < 6; i++)  // Uma vez para cada atributo
            {
                int roll = RollDice($"{numDados}d6");
                resultado += roll.ToString() + ", ";
            }
            lblResultado.Text = resultado;
        }
        private (int, string) Roll3d6()
        {
            int total = 0;
            string details = "(";

            for (int i = 0; i < 3; i++) // Rolar 3 dados
            {
                int roll = RollDice("1d6");
                total += roll;
                details += roll.ToString() + ", ";
            }

            details = details.TrimEnd(' ', ',') + ")";
            return (total, details);
        }


        private void rolar3d6(object sender, EventArgs e)
        {
            StringBuilder resultado = new StringBuilder("Atributos:\n");
            List<(int total, string detalhes)> listaResultados = new List<(int, string)>();

            for (int i = 0; i < 6; i++)  // Uma vez para cada atributo
            {
                var (total, detalhes) = Roll3d6();
                listaResultados.Add((total, $"{total} {detalhes}"));
            }

            // Ordenar a lista de forma ascendente com base no total
            listaResultados.Sort((x, y) => y.total.CompareTo(x.total));

            foreach (var (total, res) in listaResultados)
            {
                resultado.AppendLine(res);
            }

            lblResultado.Text = resultado.ToString();
        }


        private (int total, string detalhes) Roll4d6DropLowest()
        {
            List<int> rolagens = new List<int>();
            int total = 0;
            for (int i = 0; i < 4; i++)  // Rolar 4 dados
            {
                int roll = RollDice("1d6");
                rolagens.Add(roll);
            }

            rolagens.Sort();  // ordena a lista de rolagens
            rolagens.RemoveAt(0);  // remove o menor valor

            total = rolagens.Sum();  // atualiza o total
            string detalhes = $"{total} ({string.Join(", ", rolagens)})";  // cria a string de detalhes

            return (total, detalhes);
        }

        private void rolarOrganic(object sender, EventArgs e)
        {
            StringBuilder resultado = new StringBuilder("Atributos Orgânicos:\n");
            List<KeyValuePair<int, string>> pairs = new List<KeyValuePair<int, string>>();

            for (int i = 0; i < 6; i++) // Uma vez para cada atributo
            {
                List<int> rolls = new List<int>();

                for (int j = 0; j < 4; j++) // Rolar 4d6
                {
                    int roll = random.Next(1, 7);
                    rolls.Add(roll);
                }

                rolls.Sort();  // Ordena de forma ascendente
                int total = rolls.Skip(1).Sum();  // Descarta o menor

                string rollDetails = $"{total} ({string.Join(", ", rolls.Skip(1))}, [{rolls[0]}])";
                pairs.Add(new KeyValuePair<int, string>(total, rollDetails));
            }

            // Ordenar de forma descendente os totais e atualizar a label.
            var orderedPairs = pairs.OrderByDescending(x => x.Key).ToList();

            foreach (var pair in orderedPairs)
            {
                resultado.AppendLine(pair.Value);
            }

            lblResultado.Text = resultado.ToString();
        }


        private void rolar4d6DropLowest(object sender, EventArgs e)
        {
            StringBuilder resultado = new StringBuilder("Atributos:\n");
            List<int> totals = new List<int>();

            for (int i = 0; i < 6; i++)  // Uma vez para cada atributo
            {
                List<int> rolls = new List<int>();

                for (int j = 0; j < 4; j++) // 4d6
                {
                    int roll = random.Next(1, 7);
                    rolls.Add(roll);
                }

                rolls.Sort();
                int total = rolls.Skip(1).Sum(); // Descarta o menor

                totals.Add(total);
                resultado.AppendLine($"{total} ({string.Join(", ", rolls)})");
            }

            totals.Sort();
            totals.Reverse(); // Para ordenar de forma descendente

            lblResultado.Text = resultado.ToString();
        }

        private (int, string, int) Roll4d6DropLowestWithMin()
        {
            List<int> rolls = new List<int>();
            for (int i = 0; i < 4; i++)  // Rolar 4 dados
            {
                int roll = random.Next(1, 7);  // Rolar um d6
                rolls.Add(roll);
            }

            int minRoll = rolls.Min(); // Encontrar o menor resultado
            rolls.Remove(minRoll);  // Remover o menor resultado

            int total = rolls.Sum(); // Soma dos 3 maiores resultados
            string detalhes = $"{total} ({String.Join(", ", rolls)})";
            return (total, detalhes, minRoll);
        }

        private void dataGridViewDragonLanceSources_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica se a célula clicada pertence à coluna de CheckBox (suponhamos que seja a primeira coluna)
            if (e.ColumnIndex == 1 && e.RowIndex != -1)
            {
                DataGridViewCheckBoxCell cell = dataGridViewDragonLanceSources.Rows[e.RowIndex].Cells[e.ColumnIndex - 1] as DataGridViewCheckBoxCell;

                if (cell != null)
                {
                    if (cell.Value != null)
                    {
                        cell.Value = !(bool)cell.Value;
                    }
                    else
                    {
                        cell.Value = true;
                    }
                        
                }
            }
        }

        private void LoadMenuTranslations()
        {
            string langCode = Properties.Settings.Default.LanguageCode ?? "en";
            string translationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Translations", "menu", langCode + ".json");

            if (!File.Exists(translationPath))
            {
                MessageBox.Show($"Translation file not found: {translationPath}");
                return;
            }

            var json = File.ReadAllText(translationPath);
            var translations = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            // Aplicação nos menus
            if (menuStrip1.Items[0] is ToolStripMenuItem fileMenu)
            {
                fileMenu.Text = translations["Menu.File"];
                fileMenu.DropDownItems[0].Text = translations["Menu.File.Open"];
                fileMenu.DropDownItems[1].Text = translations["Menu.File.Exit"];
            }

            if (menuStrip1.Items[1] is ToolStripMenuItem editMenu)
            {
                editMenu.Text = translations["Menu.Edit"];

                // Limpa itens anteriores (se houver)
                editMenu.DropDownItems.Clear();

                // Cria novo item de menu "Preferences"
                var preferencesItem = new ToolStripMenuItem(translations["Menu.Edit.Preferences"]);
                preferencesItem.Click += (s, e) =>
                {
                    var preferencesForm = new Preferences();
                    preferencesForm.ShowDialog();
                };

                // Adiciona ao menu "Edit"
                editMenu.DropDownItems.Add(preferencesItem);
            }


            if (menuStrip1.Items[2] is ToolStripMenuItem helpMenu)
            {
                helpMenu.Text = translations["Menu.Help"];
                helpMenu.DropDownItems[0].Text = translations["Menu.Help.About"];
            }
        }

        
    }
}
