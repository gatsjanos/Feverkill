namespace OpenHardwareMonitor.GUI
{
    partial class RiasztKezelo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RiasztKezelo));
            this.listView1 = new System.Windows.Forms.ListView();
            this.hoszenzor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.muvelet = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.riasztpont = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.uzenet = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hangjelzes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.specmuvelet = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EbresztIdo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.törlésToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.újRiasztásToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hoszenzor,
            this.muvelet,
            this.riasztpont,
            this.uzenet,
            this.hangjelzes,
            this.specmuvelet,
            this.EbresztIdo});
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(-1, -1);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(902, 320);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyUp);
            this.listView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseUp);
            // 
            // hoszenzor
            // 
            this.hoszenzor.Text = "Hőszenzor";
            this.hoszenzor.Width = 292;
            // 
            // muvelet
            // 
            this.muvelet.Text = "Reláció";
            this.muvelet.Width = 51;
            // 
            // riasztpont
            // 
            this.riasztpont.Text = "Riasztási pont";
            this.riasztpont.Width = 80;
            // 
            // uzenet
            // 
            this.uzenet.Text = "Üzenet";
            this.uzenet.Width = 248;
            // 
            // hangjelzes
            // 
            this.hangjelzes.Text = "Hangjelzés";
            this.hangjelzes.Width = 65;
            // 
            // specmuvelet
            // 
            this.specmuvelet.Text = "Speciális művelet";
            this.specmuvelet.Width = 96;
            // 
            // EbresztIdo
            // 
            this.EbresztIdo.Text = "Ébresztés";
            this.EbresztIdo.Width = 61;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.törlésToolStripMenuItem,
            this.újRiasztásToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(130, 48);
            // 
            // törlésToolStripMenuItem
            // 
            this.törlésToolStripMenuItem.Name = "törlésToolStripMenuItem";
            this.törlésToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.törlésToolStripMenuItem.Text = "Törlés";
            this.törlésToolStripMenuItem.Click += new System.EventHandler(this.törlésToolStripMenuItem_Click);
            // 
            // újRiasztásToolStripMenuItem
            // 
            this.újRiasztásToolStripMenuItem.Name = "újRiasztásToolStripMenuItem";
            this.újRiasztásToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.újRiasztásToolStripMenuItem.Text = "Új Riasztás";
            this.újRiasztásToolStripMenuItem.Click += new System.EventHandler(this.újRiasztásToolStripMenuItem_Click);
            // 
            // RiasztKezelo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(913, 332);
            this.Controls.Add(this.listView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RiasztKezelo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Riasztások Kezelése";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RiasztKezelo_FormClosing);
            this.Resize += new System.EventHandler(this.RiasztTorlo_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader hoszenzor;
        private System.Windows.Forms.ColumnHeader riasztpont;
        private System.Windows.Forms.ColumnHeader muvelet;
        private System.Windows.Forms.ColumnHeader uzenet;
        private System.Windows.Forms.ColumnHeader hangjelzes;
        private System.Windows.Forms.ColumnHeader specmuvelet;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem törlésToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem újRiasztásToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader EbresztIdo;
    }
}