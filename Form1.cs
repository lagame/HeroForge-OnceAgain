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
    }
}
