using DocumentFormat.OpenXml.VariantTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeroForge_OnceAgain
{
    public partial class Preferences : Form
    {
        public Preferences()
        {
            InitializeComponent();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btSaveSettings_Click(object sender, EventArgs e)
        {
            SaveChooses();

            //Properties.Settings.Default.SystemofUnit = cBBaseUnit.SelectedIndex;
            //Properties.Settings.Default.Save();
            var mainForm = Application.OpenForms.OfType<Form1>().Single();
            mainForm.Reload();            
            this.Close();
        }

        private void Preferences_Load(object sender, EventArgs e)
        {
            cBBaseUnit.SelectedIndex = Properties.Settings.Default.SystemofUnit;
            cBLanguage.SelectedIndex = Properties.Settings.Default.LanguageIndex;
        }

        private void cBLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            

            //SaveChooseLanguage(cBLanguage.Text, cBLanguage.SelectedIndex);

            
        }

        private void SaveChooses()
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

            Properties.Settings.Default.Language = cBLanguage.Text;
            Properties.Settings.Default.LanguageIndex = cBLanguage.SelectedIndex;
            Properties.Settings.Default.SystemofUnit = cBBaseUnit.SelectedIndex;
            Properties.Settings.Default.Save();
            this.Controls.Clear();
            ChangeLanguage();
            InitializeComponent();
        }

        private void ChangeLanguage()
        {
            foreach (Control c in this.Controls)
            {
                ComponentResourceManager resources = new ComponentResourceManager(typeof(Form1));
                resources.ApplyResources(c, c.Name, new CultureInfo(cBLanguage.Text));
            }
        }
    }
}
