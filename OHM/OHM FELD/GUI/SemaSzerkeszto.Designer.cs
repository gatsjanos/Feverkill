namespace OpenHardwareMonitor.GUI
{
    partial class SemaSzerkeszto
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
            if(!Program.Dolgozott)
            Program.SZLIST_SZERK_MENT = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SemaSzerkeszto));
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBoxIntOssz = new System.Windows.Forms.CheckBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.Ventilator = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBoxPID = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.numericUpDownDerivaltBeleszamitas = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.numericUpDownIntegralBeleszamitas = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numericUpDownCel = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownD = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownI = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownP = new System.Windows.Forms.NumericUpDown();
            this.buttonPIDValt = new System.Windows.Forms.Button();
            this.labelPIDTutor = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.button4 = new System.Windows.Forms.Button();
            this.pictureBoxHelpSegitoCsuszkakhoz = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBoxPID.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDerivaltBeleszamitas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIntegralBeleszamitas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHelpSegitoCsuszkakhoz)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox8);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.checkBox7);
            this.groupBox1.Controls.Add(this.checkBox6);
            this.groupBox1.Controls.Add(this.checkBox5);
            this.groupBox1.Controls.Add(this.checkBox4);
            this.groupBox1.Controls.Add(this.checkBox3);
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Controls.Add(this.checkBox1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // checkBox8
            // 
            resources.ApplyResources(this.checkBox8, "checkBox8");
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // checkBox7
            // 
            resources.ApplyResources(this.checkBox7, "checkBox7");
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            resources.ApplyResources(this.checkBox6, "checkBox6");
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            resources.ApplyResources(this.checkBox5, "checkBox5");
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            resources.ApplyResources(this.checkBox4, "checkBox4");
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            resources.ApplyResources(this.checkBox3, "checkBox3");
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            resources.ApplyResources(this.checkBox2, "checkBox2");
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.White;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Name = "comboBox1";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // textBox1
            // 
            this.textBox1.ForeColor = System.Drawing.Color.Blue;
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            this.textBox1.Tag = "";
            this.textBox1.Click += new System.EventHandler(this.textBox1_Click);
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            this.textBox1.MouseLeave += new System.EventHandler(this.textBox1_MouseLeave);
            // 
            // numericUpDown1
            // 
            resources.ApplyResources(this.numericUpDown1, "numericUpDown1");
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // checkBoxIntOssz
            // 
            resources.ApplyResources(this.checkBoxIntOssz, "checkBoxIntOssz");
            this.checkBoxIntOssz.Name = "checkBoxIntOssz";
            this.checkBoxIntOssz.UseVisualStyleBackColor = true;
            this.checkBoxIntOssz.CheckedChanged += new System.EventHandler(this.checkBoxIntOssz_CheckedChanged);
            // 
            // listView1
            // 
            this.listView1.AllowColumnReorder = true;
            this.listView1.BackColor = System.Drawing.SystemColors.Window;
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Ventilator});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            resources.ApplyResources(this.listView1, "listView1");
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Scrollable = false;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            this.listView1.MouseEnter += new System.EventHandler(this.listView1_MouseEnter);
            // 
            // Ventilator
            // 
            resources.ApplyResources(this.Ventilator, "Ventilator");
            // 
            // groupBoxPID
            // 
            this.groupBoxPID.Controls.Add(this.label11);
            this.groupBoxPID.Controls.Add(this.numericUpDownDerivaltBeleszamitas);
            this.groupBoxPID.Controls.Add(this.label12);
            this.groupBoxPID.Controls.Add(this.label10);
            this.groupBoxPID.Controls.Add(this.numericUpDownIntegralBeleszamitas);
            this.groupBoxPID.Controls.Add(this.label9);
            this.groupBoxPID.Controls.Add(this.label8);
            this.groupBoxPID.Controls.Add(this.label7);
            this.groupBoxPID.Controls.Add(this.numericUpDownCel);
            this.groupBoxPID.Controls.Add(this.numericUpDownD);
            this.groupBoxPID.Controls.Add(this.numericUpDownI);
            this.groupBoxPID.Controls.Add(this.label6);
            this.groupBoxPID.Controls.Add(this.label5);
            this.groupBoxPID.Controls.Add(this.label4);
            this.groupBoxPID.Controls.Add(this.numericUpDownP);
            resources.ApplyResources(this.groupBoxPID, "groupBoxPID");
            this.groupBoxPID.Name = "groupBoxPID";
            this.groupBoxPID.TabStop = false;
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // numericUpDownDerivaltBeleszamitas
            // 
            this.numericUpDownDerivaltBeleszamitas.DecimalPlaces = 1;
            resources.ApplyResources(this.numericUpDownDerivaltBeleszamitas, "numericUpDownDerivaltBeleszamitas");
            this.numericUpDownDerivaltBeleszamitas.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.numericUpDownDerivaltBeleszamitas.Name = "numericUpDownDerivaltBeleszamitas";
            this.numericUpDownDerivaltBeleszamitas.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // numericUpDownIntegralBeleszamitas
            // 
            this.numericUpDownIntegralBeleszamitas.DecimalPlaces = 1;
            resources.ApplyResources(this.numericUpDownIntegralBeleszamitas, "numericUpDownIntegralBeleszamitas");
            this.numericUpDownIntegralBeleszamitas.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.numericUpDownIntegralBeleszamitas.Name = "numericUpDownIntegralBeleszamitas";
            this.numericUpDownIntegralBeleszamitas.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // numericUpDownCel
            // 
            resources.ApplyResources(this.numericUpDownCel, "numericUpDownCel");
            this.numericUpDownCel.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.numericUpDownCel.Name = "numericUpDownCel";
            this.numericUpDownCel.Value = new decimal(new int[] {
            53,
            0,
            0,
            0});
            // 
            // numericUpDownD
            // 
            this.numericUpDownD.DecimalPlaces = 10;
            this.numericUpDownD.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            resources.ApplyResources(this.numericUpDownD, "numericUpDownD");
            this.numericUpDownD.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.numericUpDownD.Name = "numericUpDownD";
            this.numericUpDownD.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericUpDownI
            // 
            this.numericUpDownI.DecimalPlaces = 10;
            this.numericUpDownI.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            resources.ApplyResources(this.numericUpDownI, "numericUpDownI");
            this.numericUpDownI.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.numericUpDownI.Name = "numericUpDownI";
            this.numericUpDownI.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // numericUpDownP
            // 
            this.numericUpDownP.DecimalPlaces = 10;
            this.numericUpDownP.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            resources.ApplyResources(this.numericUpDownP, "numericUpDownP");
            this.numericUpDownP.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.numericUpDownP.Name = "numericUpDownP";
            this.numericUpDownP.Value = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            // 
            // buttonPIDValt
            // 
            resources.ApplyResources(this.buttonPIDValt, "buttonPIDValt");
            this.buttonPIDValt.Name = "buttonPIDValt";
            this.buttonPIDValt.UseVisualStyleBackColor = true;
            this.buttonPIDValt.Click += new System.EventHandler(this.buttonPIDValt_Click);
            // 
            // labelPIDTutor
            // 
            resources.ApplyResources(this.labelPIDTutor, "labelPIDTutor");
            this.labelPIDTutor.Name = "labelPIDTutor";
            this.labelPIDTutor.MouseEnter += new System.EventHandler(this.ListaSzerkeszto_MouseEnter);
            // 
            // linkLabel1
            // 
            resources.ApplyResources(this.linkLabel1, "linkLabel1");
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.TabStop = true;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // button4
            // 
            resources.ApplyResources(this.button4, "button4");
            this.button4.Name = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // pictureBoxHelpSegitoCsuszkakhoz
            // 
            resources.ApplyResources(this.pictureBoxHelpSegitoCsuszkakhoz, "pictureBoxHelpSegitoCsuszkakhoz");
            this.pictureBoxHelpSegitoCsuszkakhoz.Name = "pictureBoxHelpSegitoCsuszkakhoz";
            this.pictureBoxHelpSegitoCsuszkakhoz.TabStop = false;
            // 
            // SemaSzerkeszto
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.button4);
            this.Controls.Add(this.buttonPIDValt);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelPIDTutor);
            this.Controls.Add(this.groupBoxPID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.checkBoxIntOssz);
            this.Controls.Add(this.pictureBoxHelpSegitoCsuszkakhoz);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SemaSzerkeszto";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SemaSzerkeszto_FormClosing);
            this.MouseEnter += new System.EventHandler(this.ListaSzerkeszto_MouseEnter);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBoxPID.ResumeLayout(false);
            this.groupBoxPID.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDerivaltBeleszamitas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIntegralBeleszamitas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHelpSegitoCsuszkakhoz)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox7;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox checkBox8;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBoxIntOssz;
        public System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader Ventilator;
        private System.Windows.Forms.GroupBox groupBoxPID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownP;
        private System.Windows.Forms.Button buttonPIDValt;
        private System.Windows.Forms.NumericUpDown numericUpDownD;
        private System.Windows.Forms.NumericUpDown numericUpDownI;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numericUpDownCel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numericUpDownIntegralBeleszamitas;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelPIDTutor;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numericUpDownDerivaltBeleszamitas;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.PictureBox pictureBoxHelpSegitoCsuszkakhoz;
    }
}