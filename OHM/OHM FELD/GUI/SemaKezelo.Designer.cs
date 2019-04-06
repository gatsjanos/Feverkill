using System.Windows.Forms;

namespace OpenHardwareMonitor.GUI
{
    partial class SemaKezelo
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SemaKezelo));
            this.listView1 = new System.Windows.Forms.ListView();
            this.Nev = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Homero = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Csatornak = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.szerkesztésToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.klónozásToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.törlésToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.listView2 = new System.Windows.Forms.ListView();
            this.Nev2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Homero2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Csatornak2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelH6 = new System.Windows.Forms.Label();
            this.labelH11 = new System.Windows.Forms.Label();
            this.labelH7 = new System.Windows.Forms.Label();
            this.labelH8 = new System.Windows.Forms.Label();
            this.labelH9 = new System.Windows.Forms.Label();
            this.labelH10 = new System.Windows.Forms.Label();
            this.labelH12 = new System.Windows.Forms.Label();
            this.labelH13 = new System.Windows.Forms.Label();
            this.labelH14 = new System.Windows.Forms.Label();
            this.labelH16 = new System.Windows.Forms.Label();
            this.labelH15 = new System.Windows.Forms.Label();
            this.labelH17 = new System.Windows.Forms.Label();
            this.labelH3 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.labelH18 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Nev,
            this.Homero,
            this.Csatornak});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(-1, 31);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(624, 384);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            this.listView1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyUp);
            this.listView1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseMove);
            this.listView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseUp);
            // 
            // Nev
            // 
            this.Nev.Text = "Név";
            this.Nev.Width = 144;
            // 
            // Homero
            // 
            this.Homero.Text = "Hőmérő";
            this.Homero.Width = 266;
            // 
            // Csatornak
            // 
            this.Csatornak.Text = "Csatornák";
            this.Csatornak.Width = 211;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem1,
            this.szerkesztésToolStripMenuItem,
            this.klónozásToolStripMenuItem,
            this.toolStripSeparator1,
            this.törlésToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(133, 104);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(132, 22);
            this.toolStripMenuItem2.Text = ">>>";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(129, 6);
            // 
            // szerkesztésToolStripMenuItem
            // 
            this.szerkesztésToolStripMenuItem.Name = "szerkesztésToolStripMenuItem";
            this.szerkesztésToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.szerkesztésToolStripMenuItem.Text = "Szerkesztés";
            this.szerkesztésToolStripMenuItem.Click += new System.EventHandler(this.szerkesztésToolStripMenuItem_Click);
            // 
            // klónozásToolStripMenuItem
            // 
            this.klónozásToolStripMenuItem.Name = "klónozásToolStripMenuItem";
            this.klónozásToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.klónozásToolStripMenuItem.Text = "Klónozás";
            this.klónozásToolStripMenuItem.Click += new System.EventHandler(this.klónozásToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(129, 6);
            // 
            // törlésToolStripMenuItem
            // 
            this.törlésToolStripMenuItem.Name = "törlésToolStripMenuItem";
            this.törlésToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.törlésToolStripMenuItem.Text = "Törlés";
            this.törlésToolStripMenuItem.Click += new System.EventHandler(this.törlésToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(243, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Inaktív sémák";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listView2
            // 
            this.listView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Nev2,
            this.Homero2,
            this.Csatornak2});
            this.listView2.ContextMenuStrip = this.contextMenuStrip1;
            this.listView2.FullRowSelect = true;
            this.listView2.Location = new System.Drawing.Point(661, 31);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(624, 384);
            this.listView2.TabIndex = 3;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            this.listView2.SelectedIndexChanged += new System.EventHandler(this.listView2_SelectedIndexChanged);
            this.listView2.DoubleClick += new System.EventHandler(this.listView2_DoubleClick);
            this.listView2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listView2_MouseUp);
            // 
            // Nev2
            // 
            this.Nev2.Text = "Név";
            this.Nev2.Width = 148;
            // 
            // Homero2
            // 
            this.Homero2.Text = "Hőmérő";
            this.Homero2.Width = 262;
            // 
            // Csatornak2
            // 
            this.Csatornak2.Text = "Csatornák";
            this.Csatornak2.Width = 211;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(908, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Aktív sémák";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LawnGreen;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(629, 106);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(26, 26);
            this.button1.TabIndex = 5;
            this.button1.Text = ">";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.LawnGreen;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(629, 138);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(26, 70);
            this.button2.TabIndex = 6;
            this.button2.Text = ">>>>";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.MediumTurquoise;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(629, 305);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(26, 26);
            this.button3.TabIndex = 7;
            this.button3.Text = "<";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.MediumTurquoise;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(629, 337);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(26, 70);
            this.button4.TabIndex = 8;
            this.button4.Text = "<<<<";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.SystemColors.Highlight;
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Location = new System.Drawing.Point(629, 23);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(26, 31);
            this.button5.TabIndex = 10;
            this.button5.Text = "?";
            this.button5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.Red;
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button6.ForeColor = System.Drawing.Color.White;
            this.button6.Location = new System.Drawing.Point(629, 214);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(26, 85);
            this.button6.TabIndex = 11;
            this.button6.Text = "˄˄˄˄˄";
            this.button6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(563, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "0/0db";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(689, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "0/0db";
            // 
            // labelH6
            // 
            this.labelH6.Location = new System.Drawing.Point(0, 0);
            this.labelH6.Name = "labelH6";
            this.labelH6.Size = new System.Drawing.Size(100, 23);
            this.labelH6.TabIndex = 26;
            // 
            // labelH11
            // 
            this.labelH11.Location = new System.Drawing.Point(0, 0);
            this.labelH11.Name = "labelH11";
            this.labelH11.Size = new System.Drawing.Size(100, 23);
            this.labelH11.TabIndex = 25;
            // 
            // labelH7
            // 
            this.labelH7.Location = new System.Drawing.Point(0, 0);
            this.labelH7.Name = "labelH7";
            this.labelH7.Size = new System.Drawing.Size(100, 23);
            this.labelH7.TabIndex = 24;
            // 
            // labelH8
            // 
            this.labelH8.Location = new System.Drawing.Point(0, 0);
            this.labelH8.Name = "labelH8";
            this.labelH8.Size = new System.Drawing.Size(100, 23);
            this.labelH8.TabIndex = 2;
            // 
            // labelH9
            // 
            this.labelH9.Location = new System.Drawing.Point(0, 0);
            this.labelH9.Name = "labelH9";
            this.labelH9.Size = new System.Drawing.Size(100, 23);
            this.labelH9.TabIndex = 23;
            // 
            // labelH10
            // 
            this.labelH10.Location = new System.Drawing.Point(0, 0);
            this.labelH10.Name = "labelH10";
            this.labelH10.Size = new System.Drawing.Size(100, 23);
            this.labelH10.TabIndex = 22;
            // 
            // labelH12
            // 
            this.labelH12.Location = new System.Drawing.Point(0, 0);
            this.labelH12.Name = "labelH12";
            this.labelH12.Size = new System.Drawing.Size(100, 23);
            this.labelH12.TabIndex = 21;
            // 
            // labelH13
            // 
            this.labelH13.Location = new System.Drawing.Point(0, 0);
            this.labelH13.Name = "labelH13";
            this.labelH13.Size = new System.Drawing.Size(100, 23);
            this.labelH13.TabIndex = 1;
            // 
            // labelH14
            // 
            this.labelH14.Location = new System.Drawing.Point(0, 0);
            this.labelH14.Name = "labelH14";
            this.labelH14.Size = new System.Drawing.Size(100, 23);
            this.labelH14.TabIndex = 20;
            // 
            // labelH16
            // 
            this.labelH16.Location = new System.Drawing.Point(0, 0);
            this.labelH16.Name = "labelH16";
            this.labelH16.Size = new System.Drawing.Size(100, 23);
            this.labelH16.TabIndex = 19;
            // 
            // labelH15
            // 
            this.labelH15.Location = new System.Drawing.Point(0, 0);
            this.labelH15.Name = "labelH15";
            this.labelH15.Size = new System.Drawing.Size(100, 23);
            this.labelH15.TabIndex = 18;
            // 
            // labelH17
            // 
            this.labelH17.Location = new System.Drawing.Point(0, 0);
            this.labelH17.Name = "labelH17";
            this.labelH17.Size = new System.Drawing.Size(100, 23);
            this.labelH17.TabIndex = 17;
            // 
            // labelH3
            // 
            this.labelH3.Location = new System.Drawing.Point(0, 0);
            this.labelH3.Name = "labelH3";
            this.labelH3.Size = new System.Drawing.Size(100, 23);
            this.labelH3.TabIndex = 16;
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.Black;
            this.button7.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Pixel, ((byte)(238)));
            this.button7.ForeColor = System.Drawing.Color.White;
            this.button7.Location = new System.Drawing.Point(623, 60);
            this.button7.Name = "button7";
            this.button7.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.button7.Size = new System.Drawing.Size(38, 40);
            this.button7.TabIndex = 15;
            this.button7.Text = "+";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // labelH18
            // 
            this.labelH18.Location = new System.Drawing.Point(0, 0);
            this.labelH18.Name = "labelH18";
            this.labelH18.Size = new System.Drawing.Size(100, 23);
            this.labelH18.TabIndex = 3;
            // 
            // SemaKezelo
            // 
            this.AcceptButton = this.button6;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1285, 415);
            this.Controls.Add(this.labelH13);
            this.Controls.Add(this.labelH8);
            this.Controls.Add(this.labelH18);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.labelH3);
            this.Controls.Add(this.labelH17);
            this.Controls.Add(this.labelH15);
            this.Controls.Add(this.labelH16);
            this.Controls.Add(this.labelH14);
            this.Controls.Add(this.labelH12);
            this.Controls.Add(this.labelH10);
            this.Controls.Add(this.labelH9);
            this.Controls.Add(this.labelH7);
            this.Controls.Add(this.labelH11);
            this.Controls.Add(this.labelH6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listView2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listView1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SemaKezelo";
            this.Text = "Szabályzósémák Kezelése";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ListaKivalaszto_FormClosing);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader Nev;
        private System.Windows.Forms.ColumnHeader Homero;
        private System.Windows.Forms.ColumnHeader Csatornak;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader Nev2;
        private System.Windows.Forms.ColumnHeader Homero2;
        private System.Windows.Forms.ColumnHeader Csatornak2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem szerkesztésToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem törlésToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem klónozásToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Label labelH6;
        private System.Windows.Forms.Label labelH11;
        private System.Windows.Forms.Label labelH7;
        private System.Windows.Forms.Label labelH8;
        private System.Windows.Forms.Label labelH9;
        private System.Windows.Forms.Label labelH10;
        private System.Windows.Forms.Label labelH12;
        private System.Windows.Forms.Label labelH13;
        private System.Windows.Forms.Label labelH14;
        private System.Windows.Forms.Label labelH16;
        private System.Windows.Forms.Label labelH15;
        private System.Windows.Forms.Label labelH17;
        private System.Windows.Forms.Label labelH3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Label labelH18;
    }
}