using HeroForge_OnceAgain.Infrastructure.Database;
using MaterialSkin;
using MaterialSkin.Controls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace HeroForge_OnceAgain
{
    public partial class FrmRegister : MaterialForm
    {
        private MaterialTextBox txtEmail;
        private MaterialTextBox txtPassword;
        private MaterialTextBox txtConfirmPassword;
        private MaterialButton btnRegister;
        private LinkLabel lblBackToLogin;

        public FrmRegister()
        {
            // Garante que as mensagens estejam carregadas
            LocalizationHelper.LoadMessages(Properties.Settings.Default.LanguageCode);

            InitializeComponent();

            btnRegister.Click += BtnRegister_Click;
            lblBackToLogin.Click += (s, e) => this.Close();
        }

        private void InitializeComponent()
        {
            this.Text = LocalizationHelper.T("Form.Register.Title");
            this.Size = new Size(600, 450);
            this.StartPosition = FormStartPosition.CenterScreen;

            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.DARK;
            skinManager.ColorScheme = new ColorScheme(Primary.Blue800, Primary.Blue900, Primary.Blue500, Accent.Blue200, TextShade.WHITE);

            int left = 180;
            int top = 100;
            int spacing = 60;
            Size fieldSize = new Size(250, 40);

            txtEmail = new MaterialTextBox()
            {
                Hint = LocalizationHelper.T("Form.Register.Email"),
                Location = new Point(left, top),
                Size = fieldSize
            };

            txtPassword = new MaterialTextBox()
            {
                Hint = LocalizationHelper.T("Form.Register.Password"),
                Location = new Point(left, top + spacing),
                Size = fieldSize,
                Password = true
            };

            txtConfirmPassword = new MaterialTextBox()
            {
                Hint = LocalizationHelper.T("Form.Register.ConfirmPassword"),
                Location = new Point(left, top + spacing * 2),
                Size = fieldSize,
                Password = true
            };

            btnRegister = new MaterialButton()
            {
                Text = LocalizationHelper.T("Form.Register.Submit").ToUpper(),
                Location = new Point(left, top + spacing * 3),
                Size = new Size(fieldSize.Width, 40)
            };

            lblBackToLogin = new LinkLabel()
            {
                Text = LocalizationHelper.T("Form.Register.BackToLogin"),
                Location = new Point(left + 30, top + spacing * 4),
                AutoSize = true,
                LinkColor = Color.White,
                Cursor = Cursors.Hand
            };

            this.Controls.Add(txtEmail);
            this.Controls.Add(txtPassword);
            this.Controls.Add(txtConfirmPassword);
            this.Controls.Add(btnRegister);
            this.Controls.Add(lblBackToLogin);
        }


        private void BtnRegister_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show(LocalizationHelper.T("Error.FillAllFields"), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show(LocalizationHelper.T("Error.PasswordMismatch"), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var db = new ApplicationDbContext())
            {
                var userStore = new UserStore<IdentityUser>(db);
                var userManager = new Microsoft.AspNet.Identity.UserManager<IdentityUser>(userStore);

                var existing = userManager.FindByEmail(email);
                if (existing != null)
                {
                    MessageBox.Show(LocalizationHelper.T("Error.EmailExists"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var user = new IdentityUser { UserName = email, Email = email };
                var result = userManager.Create(user, password);

                if (result.Succeeded)
                {
                    MessageBox.Show(LocalizationHelper.T("Success.Registration"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show(string.Join("\n", result.Errors), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
