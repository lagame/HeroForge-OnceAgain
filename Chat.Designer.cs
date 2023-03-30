namespace HeroForge_OnceAgain
{
    partial class Chat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label38 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label37 = new System.Windows.Forms.Label();
            this.textBoxNick = new System.Windows.Forms.TextBox();
            this.textBox91 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label38.Location = new System.Drawing.Point(12, 6);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(32, 13);
            this.label38.TabIndex = 34;
            this.label38.Text = "Nick:";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 52);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(163, 88);
            this.richTextBox1.TabIndex = 33;
            this.richTextBox1.Text = "";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label37.Location = new System.Drawing.Point(12, 143);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(29, 13);
            this.label37.TabIndex = 32;
            this.label37.Text = "label";
            // 
            // textBoxNick
            // 
            this.textBoxNick.Location = new System.Drawing.Point(12, 25);
            this.textBoxNick.Name = "textBoxNick";
            this.textBoxNick.Size = new System.Drawing.Size(163, 20);
            this.textBoxNick.TabIndex = 31;
            // 
            // textBox91
            // 
            this.textBox91.Location = new System.Drawing.Point(12, 163);
            this.textBox91.Name = "textBox91";
            this.textBox91.Size = new System.Drawing.Size(163, 20);
            this.textBox91.TabIndex = 30;
            // 
            // button2
            // 
            this.button2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button2.Location = new System.Drawing.Point(100, 188);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 29;
            this.button2.Text = "Conectar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button1.Location = new System.Drawing.Point(12, 188);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 28;
            this.button1.Text = "Enviar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label38);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label37);
            this.Controls.Add(this.textBoxNick);
            this.Controls.Add(this.textBox91);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Chat";
            this.Text = "Chat";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.TextBox textBoxNick;
        private System.Windows.Forms.TextBox textBox91;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}