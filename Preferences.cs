using DocumentFormat.OpenXml.VariantTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

            Properties.Settings.Default.SystemofUnit = cBBaseUnit.SelectedIndex;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void Preferences_Load(object sender, EventArgs e)
        {
            cBBaseUnit.SelectedIndex = Properties.Settings.Default.SystemofUnit;
        }
    }
}
