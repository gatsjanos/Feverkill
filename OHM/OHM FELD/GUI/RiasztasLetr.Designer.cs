namespace OpenHardwareMonitor.GUI
{
    partial class RiasztasLetr
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RiasztasLetr));
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.labelSzenz = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.labelUzen = new System.Windows.Forms.Label();
            this.labelRPont = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.labelMertEgys = new System.Windows.Forms.Label();
            this.labelRelacio = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.checkBoxHangj = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonAlvo = new System.Windows.Forms.RadioButton();
            this.radioButtonUjraind = new System.Windows.Forms.RadioButton();
            this.radioButtonLeall = new System.Windows.Forms.RadioButton();
            this.radioButtonHibern = new System.Windows.Forms.RadioButton();
            this.radioButtonNincsMuv = new System.Windows.Forms.RadioButton();
            this.checkBoxEbreszt = new System.Windows.Forms.CheckBox();
            this.numericUpDownEbresztIdo = new System.Windows.Forms.NumericUpDown();
            this.buttonKerdojel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEbresztIdo)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 25);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(532, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // labelSzenz
            // 
            this.labelSzenz.AutoSize = true;
            this.labelSzenz.Location = new System.Drawing.Point(12, 9);
            this.labelSzenz.Name = "labelSzenz";
            this.labelSzenz.Size = new System.Drawing.Size(177, 13);
            this.labelSzenz.TabIndex = 1;
            this.labelSzenz.Text = "Válassza ki a figyelendő hőszenzort:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 65);
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(532, 20);
            this.textBox1.TabIndex = 2;
            // 
            // labelUzen
            // 
            this.labelUzen.AutoSize = true;
            this.labelUzen.Location = new System.Drawing.Point(12, 49);
            this.labelUzen.Name = "labelUzen";
            this.labelUzen.Size = new System.Drawing.Size(145, 13);
            this.labelUzen.TabIndex = 3;
            this.labelUzen.Text = "Írja ide a megjelenő üzenetet:";
            // 
            // labelRPont
            // 
            this.labelRPont.AutoSize = true;
            this.labelRPont.Location = new System.Drawing.Point(12, 94);
            this.labelRPont.Name = "labelRPont";
            this.labelRPont.Size = new System.Drawing.Size(76, 13);
            this.labelRPont.TabIndex = 4;
            this.labelRPont.Text = "Riasztási pont:";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(86, 92);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            -1530494977,
            232830,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(38, 20);
            this.numericUpDown1.TabIndex = 5;
            this.numericUpDown1.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // labelMertEgys
            // 
            this.labelMertEgys.AutoSize = true;
            this.labelMertEgys.Location = new System.Drawing.Point(124, 95);
            this.labelMertEgys.Name = "labelMertEgys";
            this.labelMertEgys.Size = new System.Drawing.Size(18, 13);
            this.labelMertEgys.TabIndex = 6;
            this.labelMertEgys.Text = "°C";
            // 
            // labelRelacio
            // 
            this.labelRelacio.AutoSize = true;
            this.labelRelacio.Location = new System.Drawing.Point(157, 94);
            this.labelRelacio.Name = "labelRelacio";
            this.labelRelacio.Size = new System.Drawing.Size(277, 13);
            this.labelRelacio.TabIndex = 7;
            this.labelRelacio.Text = "    Riasztás, ha a hőmérséklet             a Riasztási pontnál.";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "<",
            "=",
            ">"});
            this.comboBox2.Location = new System.Drawing.Point(302, 91);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(32, 21);
            this.comboBox2.TabIndex = 8;
            this.comboBox2.Text = ">";
            // 
            // checkBoxHangj
            // 
            this.checkBoxHangj.AutoSize = true;
            this.checkBoxHangj.Location = new System.Drawing.Point(466, 93);
            this.checkBoxHangj.Name = "checkBoxHangj";
            this.checkBoxHangj.Size = new System.Drawing.Size(78, 17);
            this.checkBoxHangj.TabIndex = 9;
            this.checkBoxHangj.Text = "Hangjelzés";
            this.checkBoxHangj.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(441, 120);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(103, 50);
            this.button1.TabIndex = 10;
            this.button1.Text = "Mentés";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonAlvo);
            this.groupBox1.Controls.Add(this.radioButtonUjraind);
            this.groupBox1.Controls.Add(this.radioButtonLeall);
            this.groupBox1.Controls.Add(this.radioButtonHibern);
            this.groupBox1.Controls.Add(this.radioButtonNincsMuv);
            this.groupBox1.Location = new System.Drawing.Point(12, 118);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(423, 50);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Speciális művelet";
            // 
            // radioButtonAlvo
            // 
            this.radioButtonAlvo.AutoSize = true;
            this.radioButtonAlvo.Location = new System.Drawing.Point(104, 19);
            this.radioButtonAlvo.Name = "radioButtonAlvo";
            this.radioButtonAlvo.Size = new System.Drawing.Size(80, 17);
            this.radioButtonAlvo.TabIndex = 4;
            this.radioButtonAlvo.Text = "Alvó állapot";
            this.radioButtonAlvo.UseVisualStyleBackColor = true;
            this.radioButtonAlvo.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButtonUjraind
            // 
            this.radioButtonUjraind.AutoSize = true;
            this.radioButtonUjraind.Location = new System.Drawing.Point(342, 19);
            this.radioButtonUjraind.Name = "radioButtonUjraind";
            this.radioButtonUjraind.Size = new System.Drawing.Size(76, 17);
            this.radioButtonUjraind.TabIndex = 3;
            this.radioButtonUjraind.Text = "Újraindítás";
            this.radioButtonUjraind.UseVisualStyleBackColor = true;
            this.radioButtonUjraind.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButtonLeall
            // 
            this.radioButtonLeall.AutoSize = true;
            this.radioButtonLeall.Location = new System.Drawing.Point(271, 19);
            this.radioButtonLeall.Name = "radioButtonLeall";
            this.radioButtonLeall.Size = new System.Drawing.Size(65, 17);
            this.radioButtonLeall.TabIndex = 2;
            this.radioButtonLeall.Text = "Leállítás";
            this.radioButtonLeall.UseVisualStyleBackColor = true;
            this.radioButtonLeall.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButtonHibern
            // 
            this.radioButtonHibern.AutoSize = true;
            this.radioButtonHibern.Location = new System.Drawing.Point(190, 19);
            this.radioButtonHibern.Name = "radioButtonHibern";
            this.radioButtonHibern.Size = new System.Drawing.Size(75, 17);
            this.radioButtonHibern.TabIndex = 1;
            this.radioButtonHibern.Text = "Hibernálás";
            this.radioButtonHibern.UseVisualStyleBackColor = true;
            this.radioButtonHibern.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButtonNincsMuv
            // 
            this.radioButtonNincsMuv.AutoSize = true;
            this.radioButtonNincsMuv.Checked = true;
            this.radioButtonNincsMuv.Location = new System.Drawing.Point(6, 19);
            this.radioButtonNincsMuv.Name = "radioButtonNincsMuv";
            this.radioButtonNincsMuv.Size = new System.Drawing.Size(92, 17);
            this.radioButtonNincsMuv.TabIndex = 0;
            this.radioButtonNincsMuv.TabStop = true;
            this.radioButtonNincsMuv.Text = "Nincs művelet";
            this.radioButtonNincsMuv.UseVisualStyleBackColor = true;
            this.radioButtonNincsMuv.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // checkBoxEbreszt
            // 
            this.checkBoxEbreszt.AutoSize = true;
            this.checkBoxEbreszt.Enabled = false;
            this.checkBoxEbreszt.Location = new System.Drawing.Point(15, 174);
            this.checkBoxEbreszt.Name = "checkBoxEbreszt";
            this.checkBoxEbreszt.Size = new System.Drawing.Size(198, 17);
            this.checkBoxEbreszt.TabIndex = 12;
            this.checkBoxEbreszt.Text = "PC felébresztése               perc után.";
            this.checkBoxEbreszt.UseVisualStyleBackColor = true;
            // 
            // numericUpDownEbresztIdo
            // 
            this.numericUpDownEbresztIdo.Location = new System.Drawing.Point(115, 173);
            this.numericUpDownEbresztIdo.Maximum = new decimal(new int[] {
            -1530494977,
            232830,
            0,
            0});
            this.numericUpDownEbresztIdo.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownEbresztIdo.Name = "numericUpDownEbresztIdo";
            this.numericUpDownEbresztIdo.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownEbresztIdo.TabIndex = 13;
            this.numericUpDownEbresztIdo.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // buttonKerdojel
            // 
            this.buttonKerdojel.BackColor = System.Drawing.SystemColors.Highlight;
            this.buttonKerdojel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonKerdojel.ForeColor = System.Drawing.Color.White;
            this.buttonKerdojel.Location = new System.Drawing.Point(217, 169);
            this.buttonKerdojel.Name = "buttonKerdojel";
            this.buttonKerdojel.Size = new System.Drawing.Size(26, 31);
            this.buttonKerdojel.TabIndex = 14;
            this.buttonKerdojel.Text = "?";
            this.buttonKerdojel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonKerdojel.UseVisualStyleBackColor = false;
            this.buttonKerdojel.Click += new System.EventHandler(this.buttonKerdojel_Click);
            // 
            // RiasztasLetr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(556, 201);
            this.Controls.Add(this.buttonKerdojel);
            this.Controls.Add(this.numericUpDownEbresztIdo);
            this.Controls.Add(this.checkBoxEbreszt);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBoxHangj);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.labelRelacio);
            this.Controls.Add(this.labelMertEgys);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.labelRPont);
            this.Controls.Add(this.labelUzen);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.labelSzenz);
            this.Controls.Add(this.comboBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "RiasztasLetr";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Riasztás Létrehozása";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEbresztIdo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label labelSzenz;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label labelUzen;
        private System.Windows.Forms.Label labelRPont;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label labelMertEgys;
        private System.Windows.Forms.Label labelRelacio;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.CheckBox checkBoxHangj;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonUjraind;
        private System.Windows.Forms.RadioButton radioButtonLeall;
        private System.Windows.Forms.RadioButton radioButtonHibern;
        private System.Windows.Forms.RadioButton radioButtonNincsMuv;
        private System.Windows.Forms.RadioButton radioButtonAlvo;
        private System.Windows.Forms.CheckBox checkBoxEbreszt;
        private System.Windows.Forms.NumericUpDown numericUpDownEbresztIdo;
        private System.Windows.Forms.Button buttonKerdojel;
    }
}