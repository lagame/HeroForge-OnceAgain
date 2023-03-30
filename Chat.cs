using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeroForge_OnceAgain
{
    public partial class Chat : Form
    {
        public Chat()
        {
            InitializeComponent();
        }

        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();

        NetworkStream serverStream = default(NetworkStream);

        string readData = null;

        private void button1_Click(object sender, EventArgs e)
        {
            

            try
            {
                NetworkStream serverStream = clientSocket.GetStream();
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(textBox91.Text + "$");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
                byte[] bytesFrom = new byte[10000];
                int read = serverStream.Read(bytesFrom, 0, bytesFrom.Length);                
                string returndata = System.Text.Encoding.ASCII.GetString(bytesFrom, 0, read);
                if (read > 0)
                {
                    msg(returndata);
                    textBox91.Text = "";
                    textBox91.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                richTextBox1.Text = richTextBox1.Text + Environment.NewLine + " >> " + ex.Message;
            }

            
        }

        private void button2_Click(object sender, EventArgs e)
        {   
            try
            {
                clientSocket.Connect("127.0.0.1", 8888);
                label37.Text = "RPGChat - Servidor Conectado ...";
                msg(textBoxNick.Text+" logado");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro ao tentar conectar com o servidor.");
            }
        }

        public void msg(string mesg)
        {
            richTextBox1.Text = richTextBox1.Text + " >> " + mesg + Environment.NewLine;

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            // scroll it automatically
            richTextBox1.ScrollToCaret();
        }

        public void openSocket()
        {
            if (clientSocket.Connected)
            {
                clientSocket.Close();
            }
            clientSocket = new System.Net.Sockets.TcpClient();
            clientSocket.Connect("127.0.0.1", 8888);
            label37.Text = "Client Socket Program - Server Connected ...";

        }
    }
}
