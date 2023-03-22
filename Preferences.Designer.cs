namespace HeroForge_OnceAgain
{
    partial class Preferences
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Preferences));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cBBaseUnit = new System.Windows.Forms.ComboBox();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.btCancel = new System.Windows.Forms.Button();
            this.btSaveSettings = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cBBaseUnit);
            this.groupBox1.Controls.Add(this.lblLanguage);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cBBaseUnit
            // 
            this.cBBaseUnit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cBBaseUnit.FormattingEnabled = true;
            this.cBBaseUnit.Items.AddRange(new object[] {
            resources.GetString("cBBaseUnit.Items"),
            resources.GetString("cBBaseUnit.Items1")});
            resources.ApplyResources(this.cBBaseUnit, "cBBaseUnit");
            this.cBBaseUnit.Name = "cBBaseUnit";
            // 
            // lblLanguage
            // 
            resources.ApplyResources(this.lblLanguage, "lblLanguage");
            this.lblLanguage.Name = "lblLanguage";
            // 
            // btCancel
            // 
            resources.ApplyResources(this.btCancel, "btCancel");
            this.btCancel.Name = "btCancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btSaveSettings
            // 
            resources.ApplyResources(this.btSaveSettings, "btSaveSettings");
            this.btSaveSettings.Name = "btSaveSettings";
            this.btSaveSettings.UseVisualStyleBackColor = true;
            this.btSaveSettings.Click += new System.EventHandler(this.btSaveSettings_Click);
            // 
            // Preferences
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btSaveSettings);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.groupBox1);
            this.Name = "Preferences";
            this.Load += new System.EventHandler(this.Preferences_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cBBaseUnit;
        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btSaveSettings;
    }
}