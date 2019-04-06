namespace OpenHardwareMonitor.GUI
{
    partial class BootIteracioBeallit
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
            this.numericUpDownIteracioszam = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownFrissido = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIteracioszam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFrissido)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDownIteracioszam
            // 
            this.numericUpDownIteracioszam.Location = new System.Drawing.Point(317, 65);
            this.numericUpDownIteracioszam.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.numericUpDownIteracioszam.Name = "numericUpDownIteracioszam";
            this.numericUpDownIteracioszam.Size = new System.Drawing.Size(104, 20);
            this.numericUpDownIteracioszam.TabIndex = 0;
            this.numericUpDownIteracioszam.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // numericUpDownFrissido
            // 
            this.numericUpDownFrissido.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownFrissido.Location = new System.Drawing.Point(317, 91);
            this.numericUpDownFrissido.Maximum = new decimal(new int[] {
            26000,
            0,
            0,
            0});
            this.numericUpDownFrissido.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownFrissido.Name = "numericUpDownFrissido";
            this.numericUpDownFrissido.Size = new System.Drawing.Size(104, 20);
            this.numericUpDownFrissido.TabIndex = 1;
            this.numericUpDownFrissido.Value = new decimal(new int[] {
            1001,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(429, 46);
            this.label1.TabIndex = 2;
            this.label1.Text = "You can apply different refreshing interval for specified\r\nnumber of iterations a" +
    "fter starting the software.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(224, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Numer of iterations you apply the new interval:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(243, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "The new interval you apply for the first n iterations:";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Calibri", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button1.Location = new System.Drawing.Point(12, 125);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(425, 43);
            this.button1.TabIndex = 5;
            this.button1.Text = "Apply";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(420, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "pc";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(420, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "ms";
            // 
            // BootIteracioBeallit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(449, 180);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownFrissido);
            this.Controls.Add(this.numericUpDownIteracioszam);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BootIteracioBeallit";
            this.ShowIcon = false;
            this.Text = "Set Start Up Iterations";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIteracioszam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFrissido)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericUpDownIteracioszam;
        private System.Windows.Forms.NumericUpDown numericUpDownFrissido;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}