namespace OpenHardwareMonitor.GUI
{
    partial class Jatszoter
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
            MF.DevFormNyitva = false;

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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.checkBoxEmuFans = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxRandomSpeeds = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownFanNumber = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBar5 = new System.Windows.Forms.TrackBar();
            this.trackBar4 = new System.Windows.Forms.TrackBar();
            this.trackBar3 = new System.Windows.Forms.TrackBar();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.checkBoxTopMost = new System.Windows.Forms.CheckBox();
            this.checkBoxKotottCOM = new System.Windows.Forms.CheckBox();
            this.textBoxCOM = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFanNumber)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(125, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Temperature Input";
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(7, 66);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(110, 17);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "Emulated Sensors";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(7, 43);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(104, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "External Sensors";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(7, 20);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(101, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "Internal Sensors";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // checkBoxEmuFans
            // 
            this.checkBoxEmuFans.AutoSize = true;
            this.checkBoxEmuFans.Location = new System.Drawing.Point(6, 19);
            this.checkBoxEmuFans.Name = "checkBoxEmuFans";
            this.checkBoxEmuFans.Size = new System.Drawing.Size(128, 17);
            this.checkBoxEmuFans.TabIndex = 1;
            this.checkBoxEmuFans.Text = "Emulate Internal Fans";
            this.checkBoxEmuFans.UseVisualStyleBackColor = true;
            this.checkBoxEmuFans.CheckedChanged += new System.EventHandler(this.checkBoxEmuFans_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxRandomSpeeds);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.numericUpDownFanNumber);
            this.groupBox2.Controls.Add(this.checkBoxEmuFans);
            this.groupBox2.Location = new System.Drawing.Point(143, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(137, 100);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Emulated Internal Fans";
            // 
            // checkBoxRandomSpeeds
            // 
            this.checkBoxRandomSpeeds.AutoSize = true;
            this.checkBoxRandomSpeeds.Location = new System.Drawing.Point(6, 65);
            this.checkBoxRandomSpeeds.Name = "checkBoxRandomSpeeds";
            this.checkBoxRandomSpeeds.Size = new System.Drawing.Size(113, 30);
            this.checkBoxRandomSpeeds.TabIndex = 4;
            this.checkBoxRandomSpeeds.Text = "Generate Random\r\nDefault Speeds";
            this.checkBoxRandomSpeeds.UseVisualStyleBackColor = true;
            this.checkBoxRandomSpeeds.CheckedChanged += new System.EventHandler(this.checkBoxRandomSpeeds_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Number of fans:";
            // 
            // numericUpDownFanNumber
            // 
            this.numericUpDownFanNumber.Location = new System.Drawing.Point(94, 39);
            this.numericUpDownFanNumber.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownFanNumber.Name = "numericUpDownFanNumber";
            this.numericUpDownFanNumber.Size = new System.Drawing.Size(32, 20);
            this.numericUpDownFanNumber.TabIndex = 3;
            this.numericUpDownFanNumber.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownFanNumber.ValueChanged += new System.EventHandler(this.numericUpDownFanNumber_ValueChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.trackBar5);
            this.groupBox3.Controls.Add(this.trackBar4);
            this.groupBox3.Controls.Add(this.trackBar3);
            this.groupBox3.Controls.Add(this.trackBar2);
            this.groupBox3.Controls.Add(this.trackBar1);
            this.groupBox3.Location = new System.Drawing.Point(12, 118);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(268, 299);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Emulated Temperature Sensors (20°C - 100°C)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label6.Location = new System.Drawing.Point(208, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 31);
            this.label6.TabIndex = 8;
            this.label6.Text = "#5";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.Location = new System.Drawing.Point(157, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 31);
            this.label5.TabIndex = 7;
            this.label5.Text = "#4";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(106, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 31);
            this.label4.TabIndex = 6;
            this.label4.Text = "#3";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(55, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 31);
            this.label3.TabIndex = 5;
            this.label3.Text = "#2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(4, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 31);
            this.label2.TabIndex = 4;
            this.label2.Text = "#1";
            // 
            // trackBar5
            // 
            this.trackBar5.LargeChange = 33;
            this.trackBar5.Location = new System.Drawing.Point(214, 55);
            this.trackBar5.Maximum = 10000;
            this.trackBar5.Minimum = 2000;
            this.trackBar5.Name = "trackBar5";
            this.trackBar5.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar5.Size = new System.Drawing.Size(45, 238);
            this.trackBar5.SmallChange = 7;
            this.trackBar5.TabIndex = 4;
            this.trackBar5.Value = 2000;
            // 
            // trackBar4
            // 
            this.trackBar4.LargeChange = 33;
            this.trackBar4.Location = new System.Drawing.Point(163, 55);
            this.trackBar4.Maximum = 10000;
            this.trackBar4.Minimum = 2000;
            this.trackBar4.Name = "trackBar4";
            this.trackBar4.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar4.Size = new System.Drawing.Size(45, 238);
            this.trackBar4.SmallChange = 7;
            this.trackBar4.TabIndex = 3;
            this.trackBar4.Value = 2000;
            // 
            // trackBar3
            // 
            this.trackBar3.LargeChange = 33;
            this.trackBar3.Location = new System.Drawing.Point(112, 55);
            this.trackBar3.Maximum = 10000;
            this.trackBar3.Minimum = 2000;
            this.trackBar3.Name = "trackBar3";
            this.trackBar3.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar3.Size = new System.Drawing.Size(45, 238);
            this.trackBar3.SmallChange = 7;
            this.trackBar3.TabIndex = 2;
            this.trackBar3.Value = 2000;
            // 
            // trackBar2
            // 
            this.trackBar2.LargeChange = 33;
            this.trackBar2.Location = new System.Drawing.Point(61, 55);
            this.trackBar2.Maximum = 10000;
            this.trackBar2.Minimum = 2000;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar2.Size = new System.Drawing.Size(45, 238);
            this.trackBar2.SmallChange = 7;
            this.trackBar2.TabIndex = 1;
            this.trackBar2.Value = 2000;
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 33;
            this.trackBar1.Location = new System.Drawing.Point(10, 55);
            this.trackBar1.Maximum = 10000;
            this.trackBar1.Minimum = 2000;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar1.Size = new System.Drawing.Size(45, 238);
            this.trackBar1.SmallChange = 7;
            this.trackBar1.TabIndex = 0;
            this.trackBar1.Value = 2000;
            // 
            // checkBoxTopMost
            // 
            this.checkBoxTopMost.AutoSize = true;
            this.checkBoxTopMost.Checked = true;
            this.checkBoxTopMost.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTopMost.Location = new System.Drawing.Point(166, 424);
            this.checkBoxTopMost.Name = "checkBoxTopMost";
            this.checkBoxTopMost.Size = new System.Drawing.Size(114, 17);
            this.checkBoxTopMost.TabIndex = 5;
            this.checkBoxTopMost.Text = "TopMost DevForm";
            this.checkBoxTopMost.UseVisualStyleBackColor = true;
            this.checkBoxTopMost.CheckedChanged += new System.EventHandler(this.checkBoxTopMost_CheckedChanged);
            // 
            // checkBoxKotottCOM
            // 
            this.checkBoxKotottCOM.AutoSize = true;
            this.checkBoxKotottCOM.Checked = true;
            this.checkBoxKotottCOM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxKotottCOM.Location = new System.Drawing.Point(12, 423);
            this.checkBoxKotottCOM.Name = "checkBoxKotottCOM";
            this.checkBoxKotottCOM.Size = new System.Drawing.Size(104, 17);
            this.checkBoxKotottCOM.TabIndex = 6;
            this.checkBoxKotottCOM.Text = "Force COM port:";
            this.checkBoxKotottCOM.UseVisualStyleBackColor = true;
            this.checkBoxKotottCOM.CheckedChanged += new System.EventHandler(this.checkBoxKotottCOM_CheckedChanged);
            // 
            // textBoxCOM
            // 
            this.textBoxCOM.Location = new System.Drawing.Point(113, 421);
            this.textBoxCOM.Name = "textBoxCOM";
            this.textBoxCOM.Size = new System.Drawing.Size(47, 20);
            this.textBoxCOM.TabIndex = 7;
            this.textBoxCOM.TextChanged += new System.EventHandler(this.textBoxCOM_TextChanged);
            // 
            // Jatszoter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 448);
            this.Controls.Add(this.textBoxCOM);
            this.Controls.Add(this.checkBoxKotottCOM);
            this.Controls.Add(this.checkBoxTopMost);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Jatszoter";
            this.Text = "Playground";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFanNumber)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TrackBar trackBar5;
        public System.Windows.Forms.TrackBar trackBar4;
        public System.Windows.Forms.TrackBar trackBar3;
        public System.Windows.Forms.TrackBar trackBar2;
        public System.Windows.Forms.TrackBar trackBar1;
        public System.Windows.Forms.CheckBox checkBoxEmuFans;
        public System.Windows.Forms.NumericUpDown numericUpDownFanNumber;
        public System.Windows.Forms.CheckBox checkBoxRandomSpeeds;
        public System.Windows.Forms.CheckBox checkBoxTopMost;
        public System.Windows.Forms.CheckBox checkBoxKotottCOM;
        private System.Windows.Forms.TextBox textBoxCOM;
    }
}