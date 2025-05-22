using DocumentFormat.OpenXml.ExtendedProperties;
using HeroForge_OnceAgain.Infrastructure.Database;
using MaterialSkin;
using MaterialSkin.Controls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeroForge_OnceAgain
{
    public partial class FrmLogin : MaterialForm
    {
        //private MaterialLabel lblAppTitle;
        private MaterialTextBox txtEmail;
        private MaterialTextBox txtPassword;
        private MaterialButton btnLogin;
        private LinkLabel lblForgotPassword;
        private MaterialCheckbox chkRememberMe;
        private PictureBox picUser;
        private PictureBox picEmailIcon;
        private PictureBox picPasswordIcon;
        private LinkLabel lblRegister;
        private PictureBox picLoading;

        public bool LoginSucceeded { get; private set; } = false;


        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );

        public FrmLogin()
        {
            
            InitializeComponent();            
            this.TransparencyKey = Color.Empty;

            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = false;

            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue800, Primary.Blue900, Primary.Blue500, Accent.Blue200, TextShade.WHITE);


            //materialSkinManager.ColorScheme = new ColorScheme(
            //    Primary.Indigo500, Primary.Indigo700,
            //    Primary.Indigo100, Accent.Pink200,
            //    TextShade.WHITE
            //);
            
            this.Load += FrmLogin_Load;

            btnLogin.Click += BtnLogin_Click;
            txtEmail.TextChanged += InputFields_TextChanged;
            txtPassword.TextChanged += InputFields_TextChanged;

        }

        private void FrmLogin_Load(object sender, System.EventArgs e)
        {
            // Criar um painel para segurar a imagem de fundo
            Panel backgroundPanel = new Panel();
            backgroundPanel.Dock = DockStyle.Fill;
            backgroundPanel.BackgroundImage = Properties.Resources.fundo;
            backgroundPanel.BackgroundImageLayout = ImageLayout.Stretch;            

            // Adicionar o painel ao formulário
            this.Controls.Add(backgroundPanel);
            backgroundPanel.SendToBack(); // Garante que fique no fundo
            btnLogin.Enabled = false;

            if (Properties.Settings.Default.RememberMe)
            {
                txtEmail.Text = Properties.Settings.Default.SavedEmail;
                chkRememberMe.Checked = true;
            }

        }
        private void InputFields_TextChanged(object sender, EventArgs e)
        {
            btnLogin.Enabled = !string.IsNullOrWhiteSpace(txtEmail.Text.Trim()) &&
                               !string.IsNullOrWhiteSpace(txtPassword.Text);
        }

        private void InitializeComponent()
        {
            this.Text = "HeroForge - Login";
            this.Size = new Size(600, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(10, 132, 185);

            // Loading GIF
            picLoading = new PictureBox()
            {
                Size = new Size(95, 95),
                Location = new Point((this.Width - 95) / 2, 360), // centralizado horizontalmente
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Visible = false
            };
            // Se estiver usando Resources:
            picLoading.Image = Properties.Resources.loading;

            // Adiciona ao formulário
            this.Controls.Add(picLoading);

            // Ícone do usuário
            picUser = new PictureBox()
            {
                Size = new Size(100, 100),
                Location = new Point(250, 50),
                //Image = Properties.Resources.icon_user,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };

            // Ícone do email
            picEmailIcon = new PictureBox()
            {
                Size = new Size(24, 24),
                Location = new Point(150, 160),
                //Image = Properties.Resources.icon_email,
                SizeMode = PictureBoxSizeMode.Zoom,




                //BackColor = Color.Transparent
            };

            // Campo de email
            txtEmail = new MaterialTextBox()
            {
                Hint = "Email ID",
                Location = new Point(180, 150),
                Size = new Size(250, 10),
                ForeColor = Color.Black,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Ícone de senha
            picPasswordIcon = new PictureBox()
            {
                Size = new Size(24, 24),
                Location = new Point(150, 220),
                //Image = Properties.Resources.icon_password,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };

            // Campo de senha
            txtPassword = new MaterialTextBox()
            {
                Hint = "Password",
                Location = new Point(180, 210),
                Size = new Size(250, 10),
                Password = true,
                ForeColor = Color.Black,
                BackColor = SystemColors.Control,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Checkbox "Lembrar-me"
            chkRememberMe = new MaterialCheckbox()
            {
                Text = "Remember me",
                AutoSize = true,
                Location = new Point(180, 270),
                Size = new Size(120, 30)
            };


            // Botão de login
            btnLogin = new MaterialButton()
            {
                Text = "LOGIN",
                Location = new Point(180, 320),
                Size = new Size(250, 40)
            };

            // Link para cadastro
            lblRegister = new LinkLabel()
            {
                Text = "Already registered?",
                LinkVisited = false,
                Location = new Point(250, 370),
                ForeColor = Color.White,
                //LinkClicked += new LinkLabelLinkClickedEventHandler(this.lblRegister_LinkClicked),
                //LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblRegister_LinkClicked);
                LinkColor = Color.White,
                AutoSize = true,
                LinkBehavior = LinkBehavior.HoverUnderline,
                Cursor = Cursors.Hand,
            };
            //            lblRegister.Click += (sender, e) => MessageBox.Show("Redirecting to registration page...");

            // Link "Esqueci minha senha"




            lblForgotPassword = new LinkLabel()
            {
                Text = "Forgot Password?",
                Location = new Point(lblRegister.Location.X, lblRegister.Location.Y + 30),
                ForeColor = Color.White,
                LinkColor = Color.White,
                LinkBehavior = LinkBehavior.HoverUnderline,
                LinkVisited = false,
                AutoSize = true,
                Cursor = Cursors.Hand
            };
            //            lblForgotPassword.Click += (sender, e) => MessageBox.Show("Feature in development");

            //txtEmail.Multiline = false;
            //txtPassword.Multiline = false;




            // Adicionar componentes ao formulário
            //this.Controls.Add(lblAppTitle);
            //this.Controls.Add(picUser);
            //this.Controls.Add(picEmailIcon);
            this.Controls.Add(txtEmail);
            //this.Controls.Add(picPasswordIcon);
            this.Controls.Add(txtPassword);
            this.Controls.Add(chkRememberMe);
            this.Controls.Add(btnLogin);
            this.Controls.Add(lblRegister);
            this.Controls.Add(lblForgotPassword);
        }
        private void lblRegister_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            // Determine which link was clicked within the LinkLabel.
            this.lblRegister.Links[lblRegister.Links.IndexOf(e.Link)].Visited = true;

            // Display the appropriate link based on the value of the 
            // LinkData property of the Link object.
            string target = e.Link.LinkData as string;

            // If the value looks like a URL, navigate to it.
            // Otherwise, display it in a message box.
            if (null != target && target.StartsWith("www"))
            {
                System.Diagnostics.Process.Start(target);
            }
            else
            {
                MessageBox.Show("Item clicked: " + target);
            }
        }
        

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            //var hasher = new PasswordHasher<IdentityUser>();
            //string hash = hasher.HashPassword(null, "Password123!");

            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both email and password.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var db = new ApplicationDbContext())
            {
                var userStore = new UserStore<IdentityUser>(db);
                var userManager = new Microsoft.AspNet.Identity.UserManager<IdentityUser>(userStore);

                var user = userManager.FindByEmail(email);
                if (user != null && userManager.CheckPassword(user, password))
                {
                    LoginSucceeded = true;
                    if (chkRememberMe.Checked)
                    {
                        Properties.Settings.Default.RememberMe = true;
                        Properties.Settings.Default.SavedEmail = txtEmail.Text.Trim();
                    }
                    else
                    {
                        Properties.Settings.Default.RememberMe = false;
                        Properties.Settings.Default.SavedEmail = string.Empty;
                    }

                    Properties.Settings.Default.Save();

                    //MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    picLoading.Visible = true;
                    picLoading.Image = null;
                    picLoading.Image = Properties.Resources.loading;


                    var mainForm = new Form1();
                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid email or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
