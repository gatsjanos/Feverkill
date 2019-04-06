/*
 
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
 
  Copyright (C) 2009-2013 Michael Möller <mmoeller@openhardwaremonitor.org>
	
*/
using OpenHardwareMonitor.Eszkozok;
using System;
using System.Diagnostics;

namespace OpenHardwareMonitor.GUI
{
    partial class FoAblak
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
            if (WindowState == System.Windows.Forms.FormWindowState.Minimized)
                WindowState = System.Windows.Forms.FormWindowState.Normal;
            Visible = false;
            //Program.SorosPort.Close();
            //Program.HomersKuldTH.Abort();
            //SysTrayicon.Visible = false;
            //SysTrayicon.Dispose();

            //Fajlkezelo.FoKonfMento();

            //Process[] Exek = Process.GetProcessesByName("Feverkill");
            //foreach (Process item in Exek)
            //{
            //    item.Kill();
            //}

            ////if (disposing && (components != null))
            ////{
            ////    components.Dispose();
            ////}S
            ////base.Dispose(disposing);
        }
        public void Bezar(bool Ujraindit, string ParancssoriParameter)
        {

            try { SysTrayicon.ShowBalloonTip(1000, ((Program.KONFNyelv == "hun") ? "Vezérlőszoftver leállítva" : Eszk.GetNyelvSzo("TRUZLeallCIM")), ((Program.KONFNyelv == "hun") ? "Nincs riasztás és vezérlés!\nBelső ventilátorok Eredeti vezérlésen!" : Eszk.GetNyelvSzo("TRUZLeallSZOVEG")), System.Windows.Forms.ToolTipIcon.Warning); } catch { }
            for (int i = 0; i <= 5 && !MindenAlaplapiAlapertelmezettre(i, false); i++)
            {

            }

            try { Program.SorosPort.Close(); } catch { }
            SysTrayicon.Visible = false;
            try { Program.HomersKuldTH.Abort(); } catch { }
            SysTrayicon.Dispose();

            Fajlkezelo.FoKonfMento();

            if (Ujraindit)
                Process.Start("FeverkillSupervisor.exe", ParancssoriParameter);

            System.Environment.Exit(19981001);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FoAblak));
            this.startupMenuItem = new System.Windows.Forms.MenuItem();
            this.sensor = new Aga.Controls.Tree.TreeColumn();
            this.value = new Aga.Controls.Tree.TreeColumn();
            this.min = new Aga.Controls.Tree.TreeColumn();
            this.max = new Aga.Controls.Tree.TreeColumn();
            this.nodeImage = new Aga.Controls.Tree.NodeControls.NodeIcon();
            this.nodeCheckBox = new Aga.Controls.Tree.NodeControls.NodeCheckBox();
            this.nodeTextBoxText = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeTextBoxValue = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeTextBoxMin = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeTextBoxMax = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.fileMenuItem = new System.Windows.Forms.MenuItem();
            this.saveReportMenuItem = new System.Windows.Forms.MenuItem();
            this.MenuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem32 = new System.Windows.Forms.MenuItem();
            this.menuItem55 = new System.Windows.Forms.MenuItem();
            this.menuItem31 = new System.Windows.Forms.MenuItem();
            this.menuItem33 = new System.Windows.Forms.MenuItem();
            this.menuItemKilepes = new System.Windows.Forms.MenuItem();
            this.vezerlesMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItem38 = new System.Windows.Forms.MenuItem();
            this.menuItem34 = new System.Windows.Forms.MenuItem();
            this.menuItem35 = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.menuItem91 = new System.Windows.Forms.MenuItem();
            this.menuItem56 = new System.Windows.Forms.MenuItem();
            this.menuItem90 = new System.Windows.Forms.MenuItem();
            this.menuItem25 = new System.Windows.Forms.MenuItem();
            this.menuItem26 = new System.Windows.Forms.MenuItem();
            this.menuItem97 = new System.Windows.Forms.MenuItem();
            this.menuItem103 = new System.Windows.Forms.MenuItem();
            this.menuItem104 = new System.Windows.Forms.MenuItem();
            this.optionsMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem15 = new System.Windows.Forms.MenuItem();
            this.menuItem106 = new System.Windows.Forms.MenuItem();
            this.separatorMenuItem = new System.Windows.Forms.MenuItem();
            this.logSensorsMenuItem = new System.Windows.Forms.MenuItem();
            this.loggingIntervalMenuItem = new System.Windows.Forms.MenuItem();
            this.log1sMenuItem = new System.Windows.Forms.MenuItem();
            this.log2sMenuItem = new System.Windows.Forms.MenuItem();
            this.log5sMenuItem = new System.Windows.Forms.MenuItem();
            this.log10sMenuItem = new System.Windows.Forms.MenuItem();
            this.log30sMenuItem = new System.Windows.Forms.MenuItem();
            this.log1minMenuItem = new System.Windows.Forms.MenuItem();
            this.log2minMenuItem = new System.Windows.Forms.MenuItem();
            this.log5minMenuItem = new System.Windows.Forms.MenuItem();
            this.log10minMenuItem = new System.Windows.Forms.MenuItem();
            this.log30minMenuItem = new System.Windows.Forms.MenuItem();
            this.log1hMenuItem = new System.Windows.Forms.MenuItem();
            this.log2hMenuItem = new System.Windows.Forms.MenuItem();
            this.log6hMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem78 = new System.Windows.Forms.MenuItem();
            this.menuItem60 = new System.Windows.Forms.MenuItem();
            this.menuItem61 = new System.Windows.Forms.MenuItem();
            this.menuItem80 = new System.Windows.Forms.MenuItem();
            this.menuItem62 = new System.Windows.Forms.MenuItem();
            this.menuItem77 = new System.Windows.Forms.MenuItem();
            this.menuItem63 = new System.Windows.Forms.MenuItem();
            this.menuItem64 = new System.Windows.Forms.MenuItem();
            this.menuItem65 = new System.Windows.Forms.MenuItem();
            this.menuItem66 = new System.Windows.Forms.MenuItem();
            this.menuItem67 = new System.Windows.Forms.MenuItem();
            this.menuItem68 = new System.Windows.Forms.MenuItem();
            this.menuItem69 = new System.Windows.Forms.MenuItem();
            this.menuItem70 = new System.Windows.Forms.MenuItem();
            this.menuItem71 = new System.Windows.Forms.MenuItem();
            this.menuItem72 = new System.Windows.Forms.MenuItem();
            this.menuItem73 = new System.Windows.Forms.MenuItem();
            this.menuItem74 = new System.Windows.Forms.MenuItem();
            this.menuItem75 = new System.Windows.Forms.MenuItem();
            this.menuItem76 = new System.Windows.Forms.MenuItem();
            this.menuItem93 = new System.Windows.Forms.MenuItem();
            this.menuIthisz0 = new System.Windows.Forms.MenuItem();
            this.menuIthisz05 = new System.Windows.Forms.MenuItem();
            this.menuIthisz1 = new System.Windows.Forms.MenuItem();
            this.menuIthisz15 = new System.Windows.Forms.MenuItem();
            this.menuIthisz2 = new System.Windows.Forms.MenuItem();
            this.menuIthisz25 = new System.Windows.Forms.MenuItem();
            this.menuIthisz3 = new System.Windows.Forms.MenuItem();
            this.menuIthisz35 = new System.Windows.Forms.MenuItem();
            this.menuIthisz4 = new System.Windows.Forms.MenuItem();
            this.menuIthisz45 = new System.Windows.Forms.MenuItem();
            this.menuIthisz5 = new System.Windows.Forms.MenuItem();
            this.menuItem94 = new System.Windows.Forms.MenuItem();
            this.menuItem57 = new System.Windows.Forms.MenuItem();
            this.logSeparatorMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem96 = new System.Windows.Forms.MenuItem();
            this.temperatureUnitsMenuItem = new System.Windows.Forms.MenuItem();
            this.celsiusMenuItem = new System.Windows.Forms.MenuItem();
            this.fahrenheitMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem100 = new System.Windows.Forms.MenuItem();
            this.menuItem105 = new System.Windows.Forms.MenuItem();
            this.viewMenuItem = new System.Windows.Forms.MenuItem();
            this.startMinMenuItem = new System.Windows.Forms.MenuItem();
            this.columnsMenuItem = new System.Windows.Forms.MenuItem();
            this.valueMenuItem = new System.Windows.Forms.MenuItem();
            this.minMenuItem = new System.Windows.Forms.MenuItem();
            this.maxMenuItem = new System.Windows.Forms.MenuItem();
            this.resetMinMaxMenuItem = new System.Windows.Forms.MenuItem();
            this.hiddenMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem92 = new System.Windows.Forms.MenuItem();
            this.menuItem102 = new System.Windows.Forms.MenuItem();
            this.menuItem39 = new System.Windows.Forms.MenuItem();
            this.menuItem37 = new System.Windows.Forms.MenuItem();
            this.menuItem58 = new System.Windows.Forms.MenuItem();
            this.plotMenuItem = new System.Windows.Forms.MenuItem();
            this.plotLocationMenuItem = new System.Windows.Forms.MenuItem();
            this.plotWindowMenuItem = new System.Windows.Forms.MenuItem();
            this.plotBottomMenuItem = new System.Windows.Forms.MenuItem();
            this.plotRightMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem36 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem44 = new System.Windows.Forms.MenuItem();
            this.menuItem79 = new System.Windows.Forms.MenuItem();
            this.menuItem45 = new System.Windows.Forms.MenuItem();
            this.menuItem46 = new System.Windows.Forms.MenuItem();
            this.menuItem47 = new System.Windows.Forms.MenuItem();
            this.menuItem48 = new System.Windows.Forms.MenuItem();
            this.menuItem49 = new System.Windows.Forms.MenuItem();
            this.menuItem50 = new System.Windows.Forms.MenuItem();
            this.menuItem51 = new System.Windows.Forms.MenuItem();
            this.menuItem52 = new System.Windows.Forms.MenuItem();
            this.menuItem53 = new System.Windows.Forms.MenuItem();
            this.menuItem54 = new System.Windows.Forms.MenuItem();
            this.menuItem89 = new System.Windows.Forms.MenuItem();
            this.menuItem59 = new System.Windows.Forms.MenuItem();
            this.menuItem95 = new System.Windows.Forms.MenuItem();
            this.menuItemSegitseg = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem16 = new System.Windows.Forms.MenuItem();
            this.menuItem17 = new System.Windows.Forms.MenuItem();
            this.menuItem13 = new System.Windows.Forms.MenuItem();
            this.menuItem40 = new System.Windows.Forms.MenuItem();
            this.menuItem18 = new System.Windows.Forms.MenuItem();
            this.menuItem29 = new System.Windows.Forms.MenuItem();
            this.menuItem19 = new System.Windows.Forms.MenuItem();
            this.menuItem27 = new System.Windows.Forms.MenuItem();
            this.menuItem20 = new System.Windows.Forms.MenuItem();
            this.menuItem21 = new System.Windows.Forms.MenuItem();
            this.menuItem43 = new System.Windows.Forms.MenuItem();
            this.menuItem42 = new System.Windows.Forms.MenuItem();
            this.menuItem41 = new System.Windows.Forms.MenuItem();
            this.menuItem28 = new System.Windows.Forms.MenuItem();
            this.menuItem30 = new System.Windows.Forms.MenuItem();
            this.menuItem81 = new System.Windows.Forms.MenuItem();
            this.menuItem82 = new System.Windows.Forms.MenuItem();
            this.menuItem83 = new System.Windows.Forms.MenuItem();
            this.menuItem84 = new System.Windows.Forms.MenuItem();
            this.menuItem85 = new System.Windows.Forms.MenuItem();
            this.menuItem22 = new System.Windows.Forms.MenuItem();
            this.menuItem23 = new System.Windows.Forms.MenuItem();
            this.menuItem24 = new System.Windows.Forms.MenuItem();
            this.menuItem101 = new System.Windows.Forms.MenuItem();
            this.menuItem88 = new System.Windows.Forms.MenuItem();
            this.menuItem86 = new System.Windows.Forms.MenuItem();
            this.menuItem87 = new System.Windows.Forms.MenuItem();
            this.menuItem98 = new System.Windows.Forms.MenuItem();
            this.menuItem99 = new System.Windows.Forms.MenuItem();
            this.menuItemSupervisor = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem14 = new System.Windows.Forms.MenuItem();
            this.webMenuItem = new System.Windows.Forms.MenuItem();
            this.runWebServerMenuItem = new System.Windows.Forms.MenuItem();
            this.serverPortMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.mainboardMenuItem = new System.Windows.Forms.MenuItem();
            this.cpuMenuItem = new System.Windows.Forms.MenuItem();
            this.ramMenuItem = new System.Windows.Forms.MenuItem();
            this.gpuMenuItem = new System.Windows.Forms.MenuItem();
            this.fanControllerMenuItem = new System.Windows.Forms.MenuItem();
            this.hddMenuItem = new System.Windows.Forms.MenuItem();
            this.treeContextMenu = new System.Windows.Forms.ContextMenu();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.SysTrayicon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStripTrayicon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.betekintesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3Elvalaszto = new System.Windows.Forms.ToolStripSeparator();
            this.fordulatszámokToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hőmérsékletekToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mutatásRejtésToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.áttekintőToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.felülMaradóToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.előtérbeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kilépésToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timerVedelem = new System.Windows.Forms.Timer(this.components);
            this.splitContainer = new OpenHardwareMonitor.GUI.SplitContainerAdv();
            this.treeView = new Aga.Controls.Tree.TreeViewAdv();
            this.contextMenuStripTrayicon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // startupMenuItem
            // 
            this.startupMenuItem.Index = 0;
            this.startupMenuItem.Shortcut = System.Windows.Forms.Shortcut.ShiftF1;
            this.startupMenuItem.Text = "Indítás a Windowszal";
            this.startupMenuItem.Click += new System.EventHandler(this.startupMenuItem_Click);
            // 
            // sensor
            // 
            this.sensor.Header = "Szenzor";
            this.sensor.SortOrder = System.Windows.Forms.SortOrder.None;
            this.sensor.TooltipText = null;
            this.sensor.Width = 250;
            // 
            // value
            // 
            this.value.Header = "Érték";
            this.value.SortOrder = System.Windows.Forms.SortOrder.None;
            this.value.TooltipText = null;
            this.value.Width = 100;
            // 
            // min
            // 
            this.min.Header = "Minimum";
            this.min.SortOrder = System.Windows.Forms.SortOrder.None;
            this.min.TooltipText = null;
            this.min.Width = 100;
            // 
            // max
            // 
            this.max.Header = "Maximum";
            this.max.SortOrder = System.Windows.Forms.SortOrder.None;
            this.max.TooltipText = null;
            this.max.Width = 100;
            // 
            // nodeImage
            // 
            this.nodeImage.DataPropertyName = "Image";
            this.nodeImage.LeftMargin = 1;
            this.nodeImage.ParentColumn = this.sensor;
            this.nodeImage.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Fit;
            // 
            // nodeCheckBox
            // 
            this.nodeCheckBox.DataPropertyName = "Plot";
            this.nodeCheckBox.EditEnabled = true;
            this.nodeCheckBox.LeftMargin = 3;
            this.nodeCheckBox.ParentColumn = this.sensor;
            // 
            // nodeTextBoxText
            // 
            this.nodeTextBoxText.DataPropertyName = "Text";
            this.nodeTextBoxText.IncrementalSearchEnabled = true;
            this.nodeTextBoxText.LeftMargin = 3;
            this.nodeTextBoxText.ParentColumn = this.sensor;
            this.nodeTextBoxText.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            this.nodeTextBoxText.UseCompatibleTextRendering = true;
            // 
            // nodeTextBoxValue
            // 
            this.nodeTextBoxValue.DataPropertyName = "Value";
            this.nodeTextBoxValue.IncrementalSearchEnabled = true;
            this.nodeTextBoxValue.LeftMargin = 3;
            this.nodeTextBoxValue.ParentColumn = this.value;
            this.nodeTextBoxValue.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            this.nodeTextBoxValue.UseCompatibleTextRendering = true;
            // 
            // nodeTextBoxMin
            // 
            this.nodeTextBoxMin.DataPropertyName = "Min";
            this.nodeTextBoxMin.IncrementalSearchEnabled = true;
            this.nodeTextBoxMin.LeftMargin = 3;
            this.nodeTextBoxMin.ParentColumn = this.min;
            this.nodeTextBoxMin.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            this.nodeTextBoxMin.UseCompatibleTextRendering = true;
            // 
            // nodeTextBoxMax
            // 
            this.nodeTextBoxMax.DataPropertyName = "Max";
            this.nodeTextBoxMax.IncrementalSearchEnabled = true;
            this.nodeTextBoxMax.LeftMargin = 3;
            this.nodeTextBoxMax.ParentColumn = this.max;
            this.nodeTextBoxMax.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            this.nodeTextBoxMax.UseCompatibleTextRendering = true;
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.fileMenuItem,
            this.vezerlesMenuItem,
            this.optionsMenuItem,
            this.viewMenuItem,
            this.menuItemSegitseg});
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.Index = 0;
            this.fileMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.saveReportMenuItem,
            this.MenuItem2,
            this.menuItem32,
            this.menuItem55,
            this.menuItem31,
            this.menuItem33,
            this.menuItemKilepes});
            this.fileMenuItem.Text = "Fájl";
            // 
            // saveReportMenuItem
            // 
            this.saveReportMenuItem.Index = 0;
            this.saveReportMenuItem.Shortcut = System.Windows.Forms.Shortcut.Ctrl0;
            this.saveReportMenuItem.Text = "Hardverkonfiguráció Mentése";
            this.saveReportMenuItem.Click += new System.EventHandler(this.saveReportMenuItem_Click);
            // 
            // MenuItem2
            // 
            this.MenuItem2.Index = 1;
            this.MenuItem2.Text = "-";
            // 
            // menuItem32
            // 
            this.menuItem32.Index = 2;
            this.menuItem32.Shortcut = System.Windows.Forms.Shortcut.Ctrl2;
            this.menuItem32.Text = "Frissítések Keresése";
            this.menuItem32.Click += new System.EventHandler(this.menuItem32_Click);
            // 
            // menuItem55
            // 
            this.menuItem55.Index = 3;
            this.menuItem55.Shortcut = System.Windows.Forms.Shortcut.Ctrl3;
            this.menuItem55.Text = "Keresés Minden Indításkor";
            this.menuItem55.Click += new System.EventHandler(this.menuItem55_Click);
            // 
            // menuItem31
            // 
            this.menuItem31.Index = 4;
            this.menuItem31.Text = "-";
            // 
            // menuItem33
            // 
            this.menuItem33.Index = 5;
            this.menuItem33.Shortcut = System.Windows.Forms.Shortcut.CtrlF4;
            this.menuItem33.Text = "Haladó Ablak Mutatása/Rejtése";
            this.menuItem33.Click += new System.EventHandler(this.menuItem33_Click);
            // 
            // menuItemKilepes
            // 
            this.menuItemKilepes.Index = 6;
            this.menuItemKilepes.Shortcut = System.Windows.Forms.Shortcut.AltF5;
            this.menuItemKilepes.Text = "Kilépés";
            this.menuItemKilepes.Click += new System.EventHandler(this.menuItem14_Click);
            // 
            // vezerlesMenuItem
            // 
            this.vezerlesMenuItem.Index = 1;
            this.vezerlesMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem8,
            this.menuItem9,
            this.menuItem38,
            this.menuItem34,
            this.menuItem35,
            this.menuItem10,
            this.menuItem11,
            this.menuItem12,
            this.menuItem91,
            this.menuItem56,
            this.menuItem90,
            this.menuItem25,
            this.menuItem26,
            this.menuItem97,
            this.menuItem103,
            this.menuItem104});
            this.vezerlesMenuItem.Text = "VEZÉRLÉS";
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 0;
            this.menuItem8.Shortcut = System.Windows.Forms.Shortcut.F1;
            this.menuItem8.Text = "Szabályzólista Létrehozása";
            this.menuItem8.Click += new System.EventHandler(this.menuItem8_Click);
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 1;
            this.menuItem9.Shortcut = System.Windows.Forms.Shortcut.F2;
            this.menuItem9.Text = "Szabályzólisták Kezelése";
            this.menuItem9.Click += new System.EventHandler(this.menuItem9_Click);
            // 
            // menuItem38
            // 
            this.menuItem38.Index = 2;
            this.menuItem38.Text = "-";
            // 
            // menuItem34
            // 
            this.menuItem34.Index = 3;
            this.menuItem34.Shortcut = System.Windows.Forms.Shortcut.F3;
            this.menuItem34.Text = "Riasztás Létrehozása";
            this.menuItem34.Click += new System.EventHandler(this.menuItem34_Click);
            // 
            // menuItem35
            // 
            this.menuItem35.Index = 4;
            this.menuItem35.Shortcut = System.Windows.Forms.Shortcut.F4;
            this.menuItem35.Text = "Riasztások Kezelése";
            this.menuItem35.Click += new System.EventHandler(this.menuItem35_Click);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 5;
            this.menuItem10.Text = "-";
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 6;
            this.menuItem11.Shortcut = System.Windows.Forms.Shortcut.F5;
            this.menuItem11.Text = "Manuális Vezérlés (Célh)";
            this.menuItem11.Click += new System.EventHandler(this.menuItem11_Click);
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 7;
            this.menuItem12.Shortcut = System.Windows.Forms.Shortcut.F6;
            this.menuItem12.Text = "Minden Manuális Vezérlés Feloldása (Célh)";
            this.menuItem12.Click += new System.EventHandler(this.menuItem12_Click);
            // 
            // menuItem91
            // 
            this.menuItem91.Index = 8;
            this.menuItem91.Text = "-";
            // 
            // menuItem56
            // 
            this.menuItem56.Index = 9;
            this.menuItem56.Shortcut = System.Windows.Forms.Shortcut.F7;
            this.menuItem56.Text = "Minden Alaplapi Ventilátor Listaalapú Vezérlésre";
            this.menuItem56.Click += new System.EventHandler(this.menuItem56_Click);
            // 
            // menuItem90
            // 
            this.menuItem90.Index = 10;
            this.menuItem90.Shortcut = System.Windows.Forms.Shortcut.F8;
            this.menuItem90.Text = "Minden Alaplapi Ventilátor Eredeti Vezérlésre";
            this.menuItem90.Click += new System.EventHandler(this.menuItem90_Click);
            // 
            // menuItem25
            // 
            this.menuItem25.Index = 11;
            this.menuItem25.Text = "-";
            // 
            // menuItem26
            // 
            this.menuItem26.Index = 12;
            this.menuItem26.Shortcut = System.Windows.Forms.Shortcut.F9;
            this.menuItem26.Text = "Indítási Fordulatszámok Beállítása (Célh)";
            this.menuItem26.Click += new System.EventHandler(this.menuItem26_Click);
            // 
            // menuItem97
            // 
            this.menuItem97.Index = 13;
            this.menuItem97.Shortcut = System.Windows.Forms.Shortcut.F10;
            this.menuItem97.Text = "Biztonsági Hőszenzorok Beállítása (Célh)";
            this.menuItem97.Click += new System.EventHandler(this.menuItem97_Click);
            // 
            // menuItem103
            // 
            this.menuItem103.Index = 14;
            this.menuItem103.Text = "-";
            // 
            // menuItem104
            // 
            this.menuItem104.Index = 15;
            this.menuItem104.Shortcut = System.Windows.Forms.Shortcut.F11;
            this.menuItem104.Text = "Refresh Motherboard (and Inner) Controls";
            this.menuItem104.Click += new System.EventHandler(this.menuItem104_Click);
            // 
            // optionsMenuItem
            // 
            this.optionsMenuItem.Index = 2;
            this.optionsMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.startupMenuItem,
            this.menuItem15,
            this.menuItem106,
            this.separatorMenuItem,
            this.logSensorsMenuItem,
            this.loggingIntervalMenuItem,
            this.menuItem78,
            this.menuItem60,
            this.menuItem93,
            this.menuItem94,
            this.menuItem57,
            this.logSeparatorMenuItem,
            this.menuItem96,
            this.temperatureUnitsMenuItem,
            this.menuItem100,
            this.menuItem105});
            this.optionsMenuItem.Text = "Beállítások";
            // 
            // menuItem15
            // 
            this.menuItem15.Index = 1;
            this.menuItem15.Text = "Auto Indítás Késleltetése";
            this.menuItem15.Click += new System.EventHandler(this.menuItem15_Click);
            // 
            // menuItem106
            // 
            this.menuItem106.Index = 2;
            this.menuItem106.Text = "Set Start Up Iterations";
            this.menuItem106.Click += new System.EventHandler(this.menuItem106_Click);
            // 
            // separatorMenuItem
            // 
            this.separatorMenuItem.Index = 3;
            this.separatorMenuItem.Text = "-";
            // 
            // logSensorsMenuItem
            // 
            this.logSensorsMenuItem.Index = 4;
            this.logSensorsMenuItem.Shortcut = System.Windows.Forms.Shortcut.ShiftF2;
            this.logSensorsMenuItem.Text = "Szenzoradatok Naplózása";
            // 
            // loggingIntervalMenuItem
            // 
            this.loggingIntervalMenuItem.Index = 5;
            this.loggingIntervalMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.log1sMenuItem,
            this.log2sMenuItem,
            this.log5sMenuItem,
            this.log10sMenuItem,
            this.log30sMenuItem,
            this.log1minMenuItem,
            this.log2minMenuItem,
            this.log5minMenuItem,
            this.log10minMenuItem,
            this.log30minMenuItem,
            this.log1hMenuItem,
            this.log2hMenuItem,
            this.log6hMenuItem});
            this.loggingIntervalMenuItem.Text = "Naplózási Sűrűség";
            // 
            // log1sMenuItem
            // 
            this.log1sMenuItem.Index = 0;
            this.log1sMenuItem.RadioCheck = true;
            this.log1sMenuItem.Text = "1s";
            // 
            // log2sMenuItem
            // 
            this.log2sMenuItem.Index = 1;
            this.log2sMenuItem.RadioCheck = true;
            this.log2sMenuItem.Text = "2s";
            // 
            // log5sMenuItem
            // 
            this.log5sMenuItem.Index = 2;
            this.log5sMenuItem.RadioCheck = true;
            this.log5sMenuItem.Text = "5s";
            // 
            // log10sMenuItem
            // 
            this.log10sMenuItem.Index = 3;
            this.log10sMenuItem.RadioCheck = true;
            this.log10sMenuItem.Text = "10s";
            // 
            // log30sMenuItem
            // 
            this.log30sMenuItem.Index = 4;
            this.log30sMenuItem.RadioCheck = true;
            this.log30sMenuItem.Text = "30s";
            // 
            // log1minMenuItem
            // 
            this.log1minMenuItem.Index = 5;
            this.log1minMenuItem.RadioCheck = true;
            this.log1minMenuItem.Text = "1min";
            // 
            // log2minMenuItem
            // 
            this.log2minMenuItem.Index = 6;
            this.log2minMenuItem.RadioCheck = true;
            this.log2minMenuItem.Text = "2min";
            // 
            // log5minMenuItem
            // 
            this.log5minMenuItem.Index = 7;
            this.log5minMenuItem.RadioCheck = true;
            this.log5minMenuItem.Text = "5min";
            // 
            // log10minMenuItem
            // 
            this.log10minMenuItem.Index = 8;
            this.log10minMenuItem.RadioCheck = true;
            this.log10minMenuItem.Text = "10min";
            // 
            // log30minMenuItem
            // 
            this.log30minMenuItem.Index = 9;
            this.log30minMenuItem.RadioCheck = true;
            this.log30minMenuItem.Text = "30min";
            // 
            // log1hMenuItem
            // 
            this.log1hMenuItem.Index = 10;
            this.log1hMenuItem.RadioCheck = true;
            this.log1hMenuItem.Text = "1h";
            // 
            // log2hMenuItem
            // 
            this.log2hMenuItem.Index = 11;
            this.log2hMenuItem.RadioCheck = true;
            this.log2hMenuItem.Text = "2h";
            // 
            // log6hMenuItem
            // 
            this.log6hMenuItem.Index = 12;
            this.log6hMenuItem.RadioCheck = true;
            this.log6hMenuItem.Text = "6h";
            // 
            // menuItem78
            // 
            this.menuItem78.Index = 6;
            this.menuItem78.Text = "-";
            // 
            // menuItem60
            // 
            this.menuItem60.Index = 7;
            this.menuItem60.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem61,
            this.menuItem80,
            this.menuItem62,
            this.menuItem77,
            this.menuItem63,
            this.menuItem64,
            this.menuItem65,
            this.menuItem66,
            this.menuItem67,
            this.menuItem68,
            this.menuItem69,
            this.menuItem70,
            this.menuItem71,
            this.menuItem72,
            this.menuItem73,
            this.menuItem74,
            this.menuItem75,
            this.menuItem76});
            this.menuItem60.Text = "Frissítési Időköz";
            // 
            // menuItem61
            // 
            this.menuItem61.Index = 0;
            this.menuItem61.Text = "Mi ez?";
            this.menuItem61.Click += new System.EventHandler(this.menuItem61_Click);
            // 
            // menuItem80
            // 
            this.menuItem80.Index = 1;
            this.menuItem80.Text = "-";
            // 
            // menuItem62
            // 
            this.menuItem62.Index = 2;
            this.menuItem62.Text = "1 sec";
            this.menuItem62.Click += new System.EventHandler(this.menuItem62_Click);
            // 
            // menuItem77
            // 
            this.menuItem77.Index = 3;
            this.menuItem77.Text = "1,5 sec";
            this.menuItem77.Click += new System.EventHandler(this.menuItem77_Click);
            // 
            // menuItem63
            // 
            this.menuItem63.Index = 4;
            this.menuItem63.Text = "2 sec";
            this.menuItem63.Click += new System.EventHandler(this.menuItem63_Click);
            // 
            // menuItem64
            // 
            this.menuItem64.Index = 5;
            this.menuItem64.Text = "3 sec";
            this.menuItem64.Click += new System.EventHandler(this.menuItem64_Click);
            // 
            // menuItem65
            // 
            this.menuItem65.Index = 6;
            this.menuItem65.Text = "4 sec (ajánlott)";
            this.menuItem65.Click += new System.EventHandler(this.menuItem65_Click);
            // 
            // menuItem66
            // 
            this.menuItem66.Index = 7;
            this.menuItem66.Text = "5 sec";
            this.menuItem66.Click += new System.EventHandler(this.menuItem66_Click);
            // 
            // menuItem67
            // 
            this.menuItem67.Index = 8;
            this.menuItem67.Text = "6 sec";
            this.menuItem67.Click += new System.EventHandler(this.menuItem67_Click);
            // 
            // menuItem68
            // 
            this.menuItem68.Index = 9;
            this.menuItem68.Text = "7 sec";
            this.menuItem68.Click += new System.EventHandler(this.menuItem68_Click);
            // 
            // menuItem69
            // 
            this.menuItem69.Index = 10;
            this.menuItem69.Text = "8 sec (nagyobb NEM ajánlott)";
            this.menuItem69.Click += new System.EventHandler(this.menuItem69_Click);
            // 
            // menuItem70
            // 
            this.menuItem70.Index = 11;
            this.menuItem70.Text = "9 sec";
            this.menuItem70.Click += new System.EventHandler(this.menuItem70_Click);
            // 
            // menuItem71
            // 
            this.menuItem71.Index = 12;
            this.menuItem71.Text = "10 sec";
            this.menuItem71.Click += new System.EventHandler(this.menuItem71_Click);
            // 
            // menuItem72
            // 
            this.menuItem72.Index = 13;
            this.menuItem72.Text = "12 sec";
            this.menuItem72.Click += new System.EventHandler(this.menuItem72_Click);
            // 
            // menuItem73
            // 
            this.menuItem73.Index = 14;
            this.menuItem73.Text = "14 sec";
            this.menuItem73.Click += new System.EventHandler(this.menuItem73_Click);
            // 
            // menuItem74
            // 
            this.menuItem74.Index = 15;
            this.menuItem74.Text = "16 sec";
            this.menuItem74.Click += new System.EventHandler(this.menuItem74_Click);
            // 
            // menuItem75
            // 
            this.menuItem75.Index = 16;
            this.menuItem75.Text = "18 sec";
            this.menuItem75.Click += new System.EventHandler(this.menuItem75_Click);
            // 
            // menuItem76
            // 
            this.menuItem76.Index = 17;
            this.menuItem76.Text = "20 sec";
            this.menuItem76.Click += new System.EventHandler(this.menuItem76_Click);
            // 
            // menuItem93
            // 
            this.menuItem93.Index = 8;
            this.menuItem93.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuIthisz0,
            this.menuIthisz05,
            this.menuIthisz1,
            this.menuIthisz15,
            this.menuIthisz2,
            this.menuIthisz25,
            this.menuIthisz3,
            this.menuIthisz35,
            this.menuIthisz4,
            this.menuIthisz45,
            this.menuIthisz5});
            this.menuItem93.Text = "Hiszterézis";
            // 
            // menuIthisz0
            // 
            this.menuIthisz0.Index = 0;
            this.menuIthisz0.Text = "0°C (kikapcsolva)";
            this.menuIthisz0.Click += new System.EventHandler(this.menuIthisz0_Click);
            // 
            // menuIthisz05
            // 
            this.menuIthisz05.Index = 1;
            this.menuIthisz05.Text = "0,5°C";
            this.menuIthisz05.Click += new System.EventHandler(this.menuIthisz05_Click);
            // 
            // menuIthisz1
            // 
            this.menuIthisz1.Index = 2;
            this.menuIthisz1.Text = "1°C";
            this.menuIthisz1.Click += new System.EventHandler(this.menuIthisz1_Click);
            // 
            // menuIthisz15
            // 
            this.menuIthisz15.Index = 3;
            this.menuIthisz15.Text = "1,5°C";
            this.menuIthisz15.Click += new System.EventHandler(this.menuIthisz15_Click);
            // 
            // menuIthisz2
            // 
            this.menuIthisz2.Index = 4;
            this.menuIthisz2.Text = "2°C (ajánlott)";
            this.menuIthisz2.Click += new System.EventHandler(this.menuIthisz2_Click);
            // 
            // menuIthisz25
            // 
            this.menuIthisz25.Index = 5;
            this.menuIthisz25.Text = "2,5°C";
            this.menuIthisz25.Click += new System.EventHandler(this.menuIthisz25_Click);
            // 
            // menuIthisz3
            // 
            this.menuIthisz3.Index = 6;
            this.menuIthisz3.Text = "3°C";
            this.menuIthisz3.Click += new System.EventHandler(this.menuIthisz3_Click);
            // 
            // menuIthisz35
            // 
            this.menuIthisz35.Index = 7;
            this.menuIthisz35.Text = "3,5°C";
            this.menuIthisz35.Click += new System.EventHandler(this.menuIthisz35_Click);
            // 
            // menuIthisz4
            // 
            this.menuIthisz4.Index = 8;
            this.menuIthisz4.Text = "4°C";
            this.menuIthisz4.Click += new System.EventHandler(this.menuIthisz4_Click);
            // 
            // menuIthisz45
            // 
            this.menuIthisz45.Index = 9;
            this.menuIthisz45.Text = "4,5°C";
            this.menuIthisz45.Click += new System.EventHandler(this.menuIthisz45_Click);
            // 
            // menuIthisz5
            // 
            this.menuIthisz5.Index = 10;
            this.menuIthisz5.Text = "5°C";
            this.menuIthisz5.Click += new System.EventHandler(this.menuIthisz5_Click);
            // 
            // menuItem94
            // 
            this.menuItem94.Index = 9;
            this.menuItem94.Text = "-";
            // 
            // menuItem57
            // 
            this.menuItem57.Index = 10;
            this.menuItem57.Shortcut = System.Windows.Forms.Shortcut.ShiftF3;
            this.menuItem57.Text = "Célhardver Csatlakoztatva";
            // 
            // logSeparatorMenuItem
            // 
            this.logSeparatorMenuItem.Index = 11;
            this.logSeparatorMenuItem.Text = "-";
            // 
            // menuItem96
            // 
            this.menuItem96.Index = 12;
            this.menuItem96.Text = "Nyelv";
            this.menuItem96.Click += new System.EventHandler(this.menuItem96_Click);
            // 
            // temperatureUnitsMenuItem
            // 
            this.temperatureUnitsMenuItem.Index = 13;
            this.temperatureUnitsMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.celsiusMenuItem,
            this.fahrenheitMenuItem});
            this.temperatureUnitsMenuItem.Text = "Mértékegység";
            // 
            // celsiusMenuItem
            // 
            this.celsiusMenuItem.Index = 0;
            this.celsiusMenuItem.RadioCheck = true;
            this.celsiusMenuItem.Text = "Celsius";
            this.celsiusMenuItem.Click += new System.EventHandler(this.celsiusMenuItem_Click);
            // 
            // fahrenheitMenuItem
            // 
            this.fahrenheitMenuItem.Index = 1;
            this.fahrenheitMenuItem.RadioCheck = true;
            this.fahrenheitMenuItem.Text = "Fahrenheit";
            this.fahrenheitMenuItem.Click += new System.EventHandler(this.fahrenheitMenuItem_Click);
            // 
            // menuItem100
            // 
            this.menuItem100.Index = 14;
            this.menuItem100.Text = "Kernel32 HDDinfo Letiltása";
            this.menuItem100.Click += new System.EventHandler(this.menuItem100_Click);
            // 
            // menuItem105
            // 
            this.menuItem105.Index = 15;
            this.menuItem105.Text = "Load All PCI Devices";
            this.menuItem105.Click += new System.EventHandler(this.menuItem105_Click);
            // 
            // viewMenuItem
            // 
            this.viewMenuItem.Index = 3;
            this.viewMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.startMinMenuItem,
            this.columnsMenuItem,
            this.resetMinMaxMenuItem,
            this.hiddenMenuItem,
            this.menuItem4,
            this.menuItem92,
            this.menuItem102,
            this.menuItem39,
            this.menuItem37,
            this.menuItem58,
            this.plotMenuItem,
            this.plotLocationMenuItem,
            this.menuItem36,
            this.menuItem6,
            this.menuItem44,
            this.menuItem89,
            this.menuItem59,
            this.menuItem95});
            this.viewMenuItem.Text = "Nézet";
            // 
            // startMinMenuItem
            // 
            this.startMinMenuItem.Index = 0;
            this.startMinMenuItem.Shortcut = System.Windows.Forms.Shortcut.Alt1;
            this.startMinMenuItem.Text = "Haladó Ablak Indítása Kis Méretben";
            this.startMinMenuItem.Click += new System.EventHandler(this.startMinMenuItem_Click);
            // 
            // columnsMenuItem
            // 
            this.columnsMenuItem.Index = 1;
            this.columnsMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.valueMenuItem,
            this.minMenuItem,
            this.maxMenuItem});
            this.columnsMenuItem.Text = "Megjelenő Oszlopok (Haladó Ablak)";
            // 
            // valueMenuItem
            // 
            this.valueMenuItem.Index = 0;
            this.valueMenuItem.Text = "Érték";
            // 
            // minMenuItem
            // 
            this.minMenuItem.Index = 1;
            this.minMenuItem.Text = "Minimum";
            // 
            // maxMenuItem
            // 
            this.maxMenuItem.Index = 2;
            this.maxMenuItem.Text = "Maximum";
            // 
            // resetMinMaxMenuItem
            // 
            this.resetMinMaxMenuItem.Index = 2;
            this.resetMinMaxMenuItem.Shortcut = System.Windows.Forms.Shortcut.Alt2;
            this.resetMinMaxMenuItem.Text = "Minimum és Maximum Értékek Nullázása";
            this.resetMinMaxMenuItem.Click += new System.EventHandler(this.resetMinMaxMenuItem_Click);
            // 
            // hiddenMenuItem
            // 
            this.hiddenMenuItem.Index = 3;
            this.hiddenMenuItem.Shortcut = System.Windows.Forms.Shortcut.Alt3;
            this.hiddenMenuItem.Text = "Mutasd a Rejtett Szenzorokat Is (Haladó Ablak)";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 4;
            this.menuItem4.Text = "-";
            // 
            // menuItem92
            // 
            this.menuItem92.Index = 5;
            this.menuItem92.Shortcut = System.Windows.Forms.Shortcut.Alt7;
            this.menuItem92.Text = "Áttekintő Mutatása";
            this.menuItem92.Click += new System.EventHandler(this.menuItem92_Click);
            // 
            // menuItem102
            // 
            this.menuItem102.Index = 6;
            this.menuItem102.Text = "Betekintő Megjelenítése";
            this.menuItem102.Click += new System.EventHandler(this.menuItem102_Click);
            // 
            // menuItem39
            // 
            this.menuItem39.Index = 7;
            this.menuItem39.Shortcut = System.Windows.Forms.Shortcut.Alt6;
            this.menuItem39.Text = "Hőmérsékletek Mutatása";
            this.menuItem39.Click += new System.EventHandler(this.menuItem39_Click);
            // 
            // menuItem37
            // 
            this.menuItem37.Index = 8;
            this.menuItem37.Shortcut = System.Windows.Forms.Shortcut.Alt5;
            this.menuItem37.Text = "Aktuális Fordulatszámok Mutatása";
            this.menuItem37.Click += new System.EventHandler(this.menuItem37_Click);
            // 
            // menuItem58
            // 
            this.menuItem58.Index = 9;
            this.menuItem58.Text = "-";
            // 
            // plotMenuItem
            // 
            this.plotMenuItem.Index = 10;
            this.plotMenuItem.Shortcut = System.Windows.Forms.Shortcut.Alt8;
            this.plotMenuItem.Text = "Szenzorgrafikon Mutatása";
            // 
            // plotLocationMenuItem
            // 
            this.plotLocationMenuItem.Index = 11;
            this.plotLocationMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.plotWindowMenuItem,
            this.plotBottomMenuItem,
            this.plotRightMenuItem});
            this.plotLocationMenuItem.Text = "Szenzorgrafikon Elhelyezkedése";
            // 
            // plotWindowMenuItem
            // 
            this.plotWindowMenuItem.Index = 0;
            this.plotWindowMenuItem.RadioCheck = true;
            this.plotWindowMenuItem.Text = "Saját Ablak";
            // 
            // plotBottomMenuItem
            // 
            this.plotBottomMenuItem.Index = 1;
            this.plotBottomMenuItem.RadioCheck = true;
            this.plotBottomMenuItem.Text = "Lent Dokkol";
            // 
            // plotRightMenuItem
            // 
            this.plotRightMenuItem.Index = 2;
            this.plotRightMenuItem.RadioCheck = true;
            this.plotRightMenuItem.Text = "Jobbra Dokkol";
            // 
            // menuItem36
            // 
            this.menuItem36.Index = 12;
            this.menuItem36.Text = "-";
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 13;
            this.menuItem6.Shortcut = System.Windows.Forms.Shortcut.Alt4;
            this.menuItem6.Text = "Felül Maradó";
            this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
            // 
            // menuItem44
            // 
            this.menuItem44.Index = 14;
            this.menuItem44.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem79,
            this.menuItem45,
            this.menuItem46,
            this.menuItem47,
            this.menuItem48,
            this.menuItem49,
            this.menuItem50,
            this.menuItem51,
            this.menuItem52,
            this.menuItem53,
            this.menuItem54});
            this.menuItem44.Text = "Opacitás Beállítása";
            // 
            // menuItem79
            // 
            this.menuItem79.Index = 0;
            this.menuItem79.Text = "0%";
            this.menuItem79.Visible = false;
            this.menuItem79.Click += new System.EventHandler(this.menuItem79_Click);
            // 
            // menuItem45
            // 
            this.menuItem45.Index = 1;
            this.menuItem45.Text = "10%";
            this.menuItem45.Click += new System.EventHandler(this.menuItem45_Click);
            // 
            // menuItem46
            // 
            this.menuItem46.Index = 2;
            this.menuItem46.Text = "20%";
            this.menuItem46.Click += new System.EventHandler(this.menuItem46_Click);
            // 
            // menuItem47
            // 
            this.menuItem47.Index = 3;
            this.menuItem47.Text = "30%";
            this.menuItem47.Click += new System.EventHandler(this.menuItem47_Click);
            // 
            // menuItem48
            // 
            this.menuItem48.Index = 4;
            this.menuItem48.Text = "40%";
            this.menuItem48.Click += new System.EventHandler(this.menuItem48_Click);
            // 
            // menuItem49
            // 
            this.menuItem49.Index = 5;
            this.menuItem49.Text = "50%";
            this.menuItem49.Click += new System.EventHandler(this.menuItem49_Click);
            // 
            // menuItem50
            // 
            this.menuItem50.Index = 6;
            this.menuItem50.Text = "60%";
            this.menuItem50.Click += new System.EventHandler(this.menuItem50_Click);
            // 
            // menuItem51
            // 
            this.menuItem51.Index = 7;
            this.menuItem51.Text = "70%";
            this.menuItem51.Click += new System.EventHandler(this.menuItem51_Click);
            // 
            // menuItem52
            // 
            this.menuItem52.Index = 8;
            this.menuItem52.Text = "80%";
            this.menuItem52.Click += new System.EventHandler(this.menuItem52_Click);
            // 
            // menuItem53
            // 
            this.menuItem53.Index = 9;
            this.menuItem53.Text = "90%";
            this.menuItem53.Click += new System.EventHandler(this.menuItem53_Click);
            // 
            // menuItem54
            // 
            this.menuItem54.Index = 10;
            this.menuItem54.Shortcut = System.Windows.Forms.Shortcut.Ins;
            this.menuItem54.Text = "100%";
            this.menuItem54.Click += new System.EventHandler(this.menuItem54_Click);
            // 
            // menuItem89
            // 
            this.menuItem89.Index = 15;
            this.menuItem89.Text = "-";
            // 
            // menuItem59
            // 
            this.menuItem59.Index = 16;
            this.menuItem59.Shortcut = System.Windows.Forms.Shortcut.Alt9;
            this.menuItem59.Text = "Nyitottakat Előtérbe!";
            this.menuItem59.Click += new System.EventHandler(this.előtérbeToolStripMenuItem_Click);
            // 
            // menuItem95
            // 
            this.menuItem95.Index = 17;
            this.menuItem95.Text = "Üdvözlőképernyő Megjelenítése Indításkor";
            this.menuItem95.Click += new System.EventHandler(this.menuItem95_Click);
            // 
            // menuItemSegitseg
            // 
            this.menuItemSegitseg.Index = 4;
            this.menuItemSegitseg.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem3,
            this.menuItem16,
            this.menuItem22,
            this.menuItem23,
            this.menuItem24,
            this.menuItem101,
            this.menuItem88,
            this.menuItem86,
            this.menuItem87,
            this.menuItem98,
            this.menuItem99,
            this.menuItemSupervisor,
            this.menuItem1,
            this.menuItem14});
            this.menuItemSegitseg.Text = "Segítség";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 0;
            this.menuItem3.Shortcut = System.Windows.Forms.Shortcut.CtrlF1;
            this.menuItem3.Text = "Súgó";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem16
            // 
            this.menuItem16.Index = 1;
            this.menuItem16.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem17,
            this.menuItem13,
            this.menuItem40,
            this.menuItem18,
            this.menuItem29,
            this.menuItem19,
            this.menuItem27,
            this.menuItem20,
            this.menuItem21,
            this.menuItem43,
            this.menuItem42,
            this.menuItem41,
            this.menuItem28,
            this.menuItem30,
            this.menuItem81,
            this.menuItem82,
            this.menuItem83,
            this.menuItem84,
            this.menuItem85});
            this.menuItem16.Shortcut = System.Windows.Forms.Shortcut.ShiftF1;
            this.menuItem16.Text = "Használati Útmutató";
            this.menuItem16.Visible = false;
            // 
            // menuItem17
            // 
            this.menuItem17.Index = 0;
            this.menuItem17.Text = "Feverkill";
            this.menuItem17.Click += new System.EventHandler(this.menuItem17_Click);
            // 
            // menuItem13
            // 
            this.menuItem13.Index = 1;
            this.menuItem13.Text = "-";
            // 
            // menuItem40
            // 
            this.menuItem40.Index = 2;
            this.menuItem40.Text = "-";
            // 
            // menuItem18
            // 
            this.menuItem18.Index = 3;
            this.menuItem18.Text = "Szabályzólisták";
            this.menuItem18.Click += new System.EventHandler(this.menuItem18_Click);
            // 
            // menuItem29
            // 
            this.menuItem29.Index = 4;
            this.menuItem29.Text = "Szabályzólisták Összevetése";
            this.menuItem29.Click += new System.EventHandler(this.menuItem29_Click);
            // 
            // menuItem19
            // 
            this.menuItem19.Index = 5;
            this.menuItem19.Text = "Manuális Vezérlés";
            this.menuItem19.Click += new System.EventHandler(this.menuItem19_Click);
            // 
            // menuItem27
            // 
            this.menuItem27.Index = 6;
            this.menuItem27.Text = "-";
            // 
            // menuItem20
            // 
            this.menuItem20.Index = 7;
            this.menuItem20.Text = "Indítási Fordulatszám";
            this.menuItem20.Click += new System.EventHandler(this.menuItem20_Click);
            // 
            // menuItem21
            // 
            this.menuItem21.Index = 8;
            this.menuItem21.Text = "-";
            // 
            // menuItem43
            // 
            this.menuItem43.Index = 9;
            this.menuItem43.Text = "Riasztások";
            this.menuItem43.Click += new System.EventHandler(this.menuItem43_Click);
            // 
            // menuItem42
            // 
            this.menuItem42.Index = 10;
            this.menuItem42.Text = "-";
            // 
            // menuItem41
            // 
            this.menuItem41.Index = 11;
            this.menuItem41.Text = "-";
            // 
            // menuItem28
            // 
            this.menuItem28.Index = 12;
            this.menuItem28.Text = "Alaplapi Vezérlés";
            this.menuItem28.Click += new System.EventHandler(this.menuItem28_Click);
            // 
            // menuItem30
            // 
            this.menuItem30.Index = 13;
            this.menuItem30.Text = "Felül Maradás";
            this.menuItem30.Click += new System.EventHandler(this.menuItem30_Click);
            // 
            // menuItem81
            // 
            this.menuItem81.Index = 14;
            this.menuItem81.Text = "Gyorsbillenytűk";
            this.menuItem81.Click += new System.EventHandler(this.menuItem81_Click);
            // 
            // menuItem82
            // 
            this.menuItem82.Index = 15;
            this.menuItem82.Text = "-";
            // 
            // menuItem83
            // 
            this.menuItem83.Index = 16;
            this.menuItem83.Text = "-";
            // 
            // menuItem84
            // 
            this.menuItem84.Index = 17;
            this.menuItem84.Text = "Célhardver Első Használata";
            this.menuItem84.Click += new System.EventHandler(this.menuItem84_Click);
            // 
            // menuItem85
            // 
            this.menuItem85.Index = 18;
            this.menuItem85.Text = "Célhardver";
            this.menuItem85.Click += new System.EventHandler(this.menuItem85_Click);
            // 
            // menuItem22
            // 
            this.menuItem22.Index = 2;
            this.menuItem22.Text = "-";
            // 
            // menuItem23
            // 
            this.menuItem23.Index = 3;
            this.menuItem23.Shortcut = System.Windows.Forms.Shortcut.CtrlF2;
            this.menuItem23.Text = "Küldj Visszajelzést!";
            this.menuItem23.Click += new System.EventHandler(this.menuItem23_Click);
            // 
            // menuItem24
            // 
            this.menuItem24.Index = 4;
            this.menuItem24.Shortcut = System.Windows.Forms.Shortcut.CtrlF3;
            this.menuItem24.Text = "Kapcsolat";
            this.menuItem24.Click += new System.EventHandler(this.menuItem24_Click);
            // 
            // menuItem101
            // 
            this.menuItem101.Index = 5;
            this.menuItem101.Text = "Licensz Információk";
            this.menuItem101.Click += new System.EventHandler(this.menuItem101_Click);
            // 
            // menuItem88
            // 
            this.menuItem88.Index = 6;
            this.menuItem88.Text = "-";
            // 
            // menuItem86
            // 
            this.menuItem86.Index = 7;
            this.menuItem86.Shortcut = System.Windows.Forms.Shortcut.CtrlF5;
            this.menuItem86.Text = "Tutorial";
            this.menuItem86.Click += new System.EventHandler(this.menuItem86_Click);
            // 
            // menuItem87
            // 
            this.menuItem87.Index = 8;
            this.menuItem87.Shortcut = System.Windows.Forms.Shortcut.CtrlF6;
            this.menuItem87.Text = "Tutorial Léptetése";
            this.menuItem87.Visible = false;
            this.menuItem87.Click += new System.EventHandler(this.menuItem87_Click);
            // 
            // menuItem98
            // 
            this.menuItem98.Index = 9;
            this.menuItem98.Text = "-";
            // 
            // menuItem99
            // 
            this.menuItem99.Index = 10;
            this.menuItem99.Text = "Játszótér";
            this.menuItem99.Click += new System.EventHandler(this.menuItem99_Click);
            // 
            // menuItemSupervisor
            // 
            this.menuItemSupervisor.Index = 11;
            this.menuItemSupervisor.Text = "Feverkill Supervisor";
            this.menuItemSupervisor.Click += new System.EventHandler(this.menuItemSupervisor_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 12;
            this.menuItem1.Text = "DEVOPT";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuItem14
            // 
            this.menuItem14.Index = 13;
            this.menuItem14.Text = "-";
            this.menuItem14.Visible = false;
            // 
            // webMenuItem
            // 
            this.webMenuItem.Index = -1;
            this.webMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.runWebServerMenuItem,
            this.serverPortMenuItem});
            this.webMenuItem.Text = "Távvezérlő  Webszerver";
            // 
            // runWebServerMenuItem
            // 
            this.runWebServerMenuItem.Index = 0;
            this.runWebServerMenuItem.Text = "Futás";
            // 
            // serverPortMenuItem
            // 
            this.serverPortMenuItem.Index = 1;
            this.serverPortMenuItem.Text = "Port";
            this.serverPortMenuItem.Click += new System.EventHandler(this.serverPortMenuItem_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = -1;
            this.menuItem5.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mainboardMenuItem,
            this.cpuMenuItem,
            this.ramMenuItem,
            this.gpuMenuItem,
            this.fanControllerMenuItem,
            this.hddMenuItem});
            this.menuItem5.Text = "Hardverek";
            // 
            // mainboardMenuItem
            // 
            this.mainboardMenuItem.Index = 0;
            this.mainboardMenuItem.Text = "Alaplap";
            // 
            // cpuMenuItem
            // 
            this.cpuMenuItem.Index = 1;
            this.cpuMenuItem.Text = "CPU";
            // 
            // ramMenuItem
            // 
            this.ramMenuItem.Index = 2;
            this.ramMenuItem.Text = "RAM";
            // 
            // gpuMenuItem
            // 
            this.gpuMenuItem.Index = 3;
            this.gpuMenuItem.Text = "GPU";
            // 
            // fanControllerMenuItem
            // 
            this.fanControllerMenuItem.Index = 4;
            this.fanControllerMenuItem.Text = "Ventillátor vezérlők";
            // 
            // hddMenuItem
            // 
            this.hddMenuItem.Index = 5;
            this.hddMenuItem.Text = "Merevlemezek";
            // 
            // treeContextMenu
            // 
            this.treeContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem7});
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 0;
            this.menuItem7.Text = "";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "txt";
            this.saveFileDialog.FileName = "Hardverkonfiguráció-jelentés.txt";
            this.saveFileDialog.Filter = "Szöveges Dokumentum|*.txt|All Files|*.*";
            this.saveFileDialog.Title = "Hardverkonfiguráció-jelentés Mentése";
            // 
            // SysTrayicon
            // 
            this.SysTrayicon.ContextMenuStrip = this.contextMenuStripTrayicon;
            this.SysTrayicon.Icon = ((System.Drawing.Icon)(resources.GetObject("SysTrayicon.Icon")));
            this.SysTrayicon.Tag = "Feverkill Vezérlőszoftver V";
            this.SysTrayicon.Visible = true;
            this.SysTrayicon.BalloonTipClicked += new System.EventHandler(this.SysTrayicon_BalloonTipClicked);
            this.SysTrayicon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SysTrayicon_MouseClick);
            // 
            // contextMenuStripTrayicon
            // 
            this.contextMenuStripTrayicon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.betekintesToolStripMenuItem,
            this.toolStripMenuItem3Elvalaszto,
            this.fordulatszámokToolStripMenuItem,
            this.hőmérsékletekToolStripMenuItem,
            this.toolStripMenuItem2,
            this.mutatásRejtésToolStripMenuItem,
            this.áttekintőToolStripMenuItem,
            this.toolStripMenuItem1,
            this.felülMaradóToolStripMenuItem,
            this.előtérbeToolStripMenuItem,
            this.kilépésToolStripMenuItem});
            this.contextMenuStripTrayicon.Name = "contextMenuStripTrayicon";
            this.contextMenuStripTrayicon.Size = new System.Drawing.Size(183, 198);
            // 
            // betekintesToolStripMenuItem
            // 
            this.betekintesToolStripMenuItem.Name = "betekintesToolStripMenuItem";
            this.betekintesToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.betekintesToolStripMenuItem.Text = "Betekintés";
            // 
            // toolStripMenuItem3Elvalaszto
            // 
            this.toolStripMenuItem3Elvalaszto.Name = "toolStripMenuItem3Elvalaszto";
            this.toolStripMenuItem3Elvalaszto.Size = new System.Drawing.Size(179, 6);
            // 
            // fordulatszámokToolStripMenuItem
            // 
            this.fordulatszámokToolStripMenuItem.Name = "fordulatszámokToolStripMenuItem";
            this.fordulatszámokToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.fordulatszámokToolStripMenuItem.Text = "Fordulatszámok";
            this.fordulatszámokToolStripMenuItem.Click += new System.EventHandler(this.fordulatszámokToolStripMenuItem_Click);
            // 
            // hőmérsékletekToolStripMenuItem
            // 
            this.hőmérsékletekToolStripMenuItem.Name = "hőmérsékletekToolStripMenuItem";
            this.hőmérsékletekToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.hőmérsékletekToolStripMenuItem.Text = "Hőmérsékletek";
            this.hőmérsékletekToolStripMenuItem.Click += new System.EventHandler(this.hőmérsékletekToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(179, 6);
            // 
            // mutatásRejtésToolStripMenuItem
            // 
            this.mutatásRejtésToolStripMenuItem.Name = "mutatásRejtésToolStripMenuItem";
            this.mutatásRejtésToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.mutatásRejtésToolStripMenuItem.Text = "Haladó";
            this.mutatásRejtésToolStripMenuItem.Click += new System.EventHandler(this.mutatásRejtésToolStripMenuItem_Click);
            // 
            // áttekintőToolStripMenuItem
            // 
            this.áttekintőToolStripMenuItem.Name = "áttekintőToolStripMenuItem";
            this.áttekintőToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.áttekintőToolStripMenuItem.Text = "Áttekintő";
            this.áttekintőToolStripMenuItem.Click += new System.EventHandler(this.áttekintőToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(179, 6);
            // 
            // felülMaradóToolStripMenuItem
            // 
            this.felülMaradóToolStripMenuItem.Name = "felülMaradóToolStripMenuItem";
            this.felülMaradóToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.felülMaradóToolStripMenuItem.Text = "Felül Maradó";
            this.felülMaradóToolStripMenuItem.Click += new System.EventHandler(this.felülMaradóToolStripMenuItem_Click);
            // 
            // előtérbeToolStripMenuItem
            // 
            this.előtérbeToolStripMenuItem.Name = "előtérbeToolStripMenuItem";
            this.előtérbeToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.előtérbeToolStripMenuItem.Text = "Nyitottakat Előtérbe!";
            this.előtérbeToolStripMenuItem.Click += new System.EventHandler(this.előtérbeToolStripMenuItem_Click);
            // 
            // kilépésToolStripMenuItem
            // 
            this.kilépésToolStripMenuItem.Name = "kilépésToolStripMenuItem";
            this.kilépésToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.kilépésToolStripMenuItem.Text = "Kilépés";
            this.kilépésToolStripMenuItem.Click += new System.EventHandler(this.kilépésToolStripMenuItem_Click);
            // 
            // timerVedelem
            // 
            this.timerVedelem.Enabled = true;
            this.timerVedelem.Interval = 1200000;
            this.timerVedelem.Tick += new System.EventHandler(this.timerVedelem_Tick);
            // 
            // splitContainer
            // 
            this.splitContainer.Border3DStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.splitContainer.Color = System.Drawing.SystemColors.Control;
            this.splitContainer.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitContainer.Location = new System.Drawing.Point(19, 12);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.treeView);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitContainer.Size = new System.Drawing.Size(386, 483);
            this.splitContainer.SplitterDistance = 354;
            this.splitContainer.SplitterWidth = 5;
            this.splitContainer.TabIndex = 3;
            // 
            // treeView
            // 
            this.treeView.AllowColumnReorder = true;
            this.treeView.AsyncExpanding = true;
            this.treeView.BackColor = System.Drawing.SystemColors.Window;
            this.treeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView.Columns.Add(this.sensor);
            this.treeView.Columns.Add(this.value);
            this.treeView.Columns.Add(this.min);
            this.treeView.Columns.Add(this.max);
            this.treeView.DefaultToolTipProvider = null;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeView.FullRowSelect = true;
            this.treeView.GridLineStyle = Aga.Controls.Tree.GridLineStyle.Horizontal;
            this.treeView.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Model = null;
            this.treeView.Name = "treeView";
            this.treeView.NodeControls.Add(this.nodeImage);
            this.treeView.NodeControls.Add(this.nodeCheckBox);
            this.treeView.NodeControls.Add(this.nodeTextBoxText);
            this.treeView.NodeControls.Add(this.nodeTextBoxValue);
            this.treeView.NodeControls.Add(this.nodeTextBoxMin);
            this.treeView.NodeControls.Add(this.nodeTextBoxMax);
            this.treeView.SelectedNode = null;
            this.treeView.Size = new System.Drawing.Size(386, 354);
            this.treeView.TabIndex = 0;
            this.treeView.Text = "treeView";
            this.treeView.UseColumns = true;
            this.treeView.NodeMouseDoubleClick += new System.EventHandler<Aga.Controls.Tree.TreeNodeAdvMouseEventArgs>(this.treeView_NodeMouseDoubleClick);
            this.treeView.Click += new System.EventHandler(this.treeView_Click);
            this.treeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseDown);
            this.treeView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseMove);
            this.treeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseUp);
            // 
            // FoAblak
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(402, 379);
            this.Controls.Add(this.splitContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu;
            this.Name = "FoAblak";
            this.Tag = "";
            this.Text = "Feverkill";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResizeEnd += new System.EventHandler(this.MainForm_MoveOrResize);
            this.Move += new System.EventHandler(this.MainForm_MoveOrResize);
            this.contextMenuStripTrayicon.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.MenuItem fileMenuItem;
        private Aga.Controls.Tree.TreeColumn sensor;
        private Aga.Controls.Tree.TreeColumn value;
        private Aga.Controls.Tree.TreeColumn min;
        private Aga.Controls.Tree.TreeColumn max;
        private Aga.Controls.Tree.NodeControls.NodeIcon nodeImage;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBoxText;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBoxValue;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBoxMin;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBoxMax;
        private SplitContainerAdv splitContainer;
        private System.Windows.Forms.MenuItem viewMenuItem;
        public System.Windows.Forms.MenuItem plotMenuItem;
        private Aga.Controls.Tree.NodeControls.NodeCheckBox nodeCheckBox;
        private System.Windows.Forms.MenuItem saveReportMenuItem;
        private System.Windows.Forms.MenuItem optionsMenuItem;
        private System.Windows.Forms.MenuItem hddMenuItem;
        private System.Windows.Forms.MenuItem separatorMenuItem;
        public System.Windows.Forms.ContextMenu treeContextMenu;
        private System.Windows.Forms.MenuItem startMinMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.MenuItem hiddenMenuItem;
        private System.Windows.Forms.MenuItem columnsMenuItem;
        private System.Windows.Forms.MenuItem valueMenuItem;
        private System.Windows.Forms.MenuItem minMenuItem;
        private System.Windows.Forms.MenuItem maxMenuItem;
        private System.Windows.Forms.MenuItem temperatureUnitsMenuItem;
        private System.Windows.Forms.MenuItem celsiusMenuItem;
        private System.Windows.Forms.MenuItem fahrenheitMenuItem;
        private System.Windows.Forms.MenuItem MenuItem2;
        private System.Windows.Forms.MenuItem resetMinMaxMenuItem;
        private System.Windows.Forms.MenuItem plotLocationMenuItem;
        private System.Windows.Forms.MenuItem plotWindowMenuItem;
        private System.Windows.Forms.MenuItem plotBottomMenuItem;
        private System.Windows.Forms.MenuItem plotRightMenuItem;
        private System.Windows.Forms.MenuItem webMenuItem;
        private System.Windows.Forms.MenuItem runWebServerMenuItem;
        private System.Windows.Forms.MenuItem serverPortMenuItem;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem mainboardMenuItem;
        private System.Windows.Forms.MenuItem cpuMenuItem;
        private System.Windows.Forms.MenuItem gpuMenuItem;
        private System.Windows.Forms.MenuItem fanControllerMenuItem;
        private System.Windows.Forms.MenuItem ramMenuItem;
        private System.Windows.Forms.MenuItem logSensorsMenuItem;
        private System.Windows.Forms.MenuItem loggingIntervalMenuItem;
        private System.Windows.Forms.MenuItem log1sMenuItem;
        private System.Windows.Forms.MenuItem log2sMenuItem;
        private System.Windows.Forms.MenuItem log5sMenuItem;
        private System.Windows.Forms.MenuItem log10sMenuItem;
        private System.Windows.Forms.MenuItem log30sMenuItem;
        private System.Windows.Forms.MenuItem log1minMenuItem;
        private System.Windows.Forms.MenuItem log2minMenuItem;
        private System.Windows.Forms.MenuItem log5minMenuItem;
        private System.Windows.Forms.MenuItem log10minMenuItem;
        private System.Windows.Forms.MenuItem log30minMenuItem;
        private System.Windows.Forms.MenuItem log1hMenuItem;
        private System.Windows.Forms.MenuItem log2hMenuItem;
        private System.Windows.Forms.MenuItem log6hMenuItem;
        private System.Windows.Forms.MenuItem menuItem4;
        public System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem startupMenuItem;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.MenuItem vezerlesMenuItem;
        private System.Windows.Forms.MenuItem menuItem8;
        public System.Windows.Forms.MenuItem menuItem9;
        private System.Windows.Forms.MenuItem menuItem10;
        public System.Windows.Forms.MenuItem menuItem11;
        public System.Windows.Forms.MenuItem menuItem12;
        private System.Windows.Forms.MenuItem menuItemKilepes;
        public System.Windows.Forms.MenuItem menuItemSegitseg;
        private System.Windows.Forms.MenuItem menuItem16;
        private System.Windows.Forms.MenuItem menuItem17;
        private System.Windows.Forms.MenuItem menuItem18;
        private System.Windows.Forms.MenuItem menuItem19;
        private System.Windows.Forms.MenuItem menuItem20;
        private System.Windows.Forms.MenuItem menuItem21;
        private System.Windows.Forms.MenuItem menuItem22;
        public System.Windows.Forms.MenuItem menuItem23;
        private System.Windows.Forms.MenuItem menuItem24;
        private System.Windows.Forms.MenuItem menuItem25;
        public System.Windows.Forms.MenuItem menuItem26;
        private System.Windows.Forms.MenuItem menuItem13;
        private System.Windows.Forms.MenuItem menuItem27;
        private System.Windows.Forms.MenuItem menuItem28;
        private System.Windows.Forms.MenuItem menuItem29;
        private System.Windows.Forms.MenuItem menuItem30;
        private System.Windows.Forms.MenuItem menuItem32;
        private System.Windows.Forms.MenuItem menuItem31;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTrayicon;
        private System.Windows.Forms.ToolStripMenuItem kilépésToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mutatásRejtésToolStripMenuItem;
        private System.Windows.Forms.MenuItem menuItem33;
        private System.Windows.Forms.MenuItem menuItem36;
        private System.Windows.Forms.MenuItem menuItem37;
        private System.Windows.Forms.MenuItem menuItem34;
        private System.Windows.Forms.MenuItem menuItem38;
        public System.Windows.Forms.MenuItem menuItem35;
        public System.Windows.Forms.MenuItem menuItem39;
        private System.Windows.Forms.MenuItem menuItem40;
        private System.Windows.Forms.MenuItem menuItem43;
        private System.Windows.Forms.MenuItem menuItem42;
        private System.Windows.Forms.MenuItem menuItem41;
        private System.Windows.Forms.MenuItem menuItem44;
        private System.Windows.Forms.MenuItem menuItem45;
        private System.Windows.Forms.MenuItem menuItem46;
        private System.Windows.Forms.MenuItem menuItem47;
        private System.Windows.Forms.MenuItem menuItem48;
        private System.Windows.Forms.MenuItem menuItem49;
        private System.Windows.Forms.MenuItem menuItem50;
        private System.Windows.Forms.MenuItem menuItem51;
        private System.Windows.Forms.MenuItem menuItem52;
        private System.Windows.Forms.MenuItem menuItem53;
        private System.Windows.Forms.MenuItem menuItem54;
        private System.Windows.Forms.MenuItem menuItem55;
        private System.Windows.Forms.ToolStripMenuItem fordulatszámokToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hőmérsékletekToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem felülMaradóToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem előtérbeToolStripMenuItem;
        private System.Windows.Forms.MenuItem menuItem3;
        public System.Windows.Forms.MenuItem menuItem57;
        public System.Windows.Forms.NotifyIcon SysTrayicon;
        private System.Windows.Forms.MenuItem menuItem58;
        private System.Windows.Forms.MenuItem menuItem59;
        private System.Windows.Forms.MenuItem menuItem61;
        private System.Windows.Forms.MenuItem menuItem62;
        private System.Windows.Forms.MenuItem menuItem63;
        private System.Windows.Forms.MenuItem menuItem64;
        private System.Windows.Forms.MenuItem menuItem65;
        private System.Windows.Forms.MenuItem menuItem66;
        private System.Windows.Forms.MenuItem menuItem67;
        private System.Windows.Forms.MenuItem menuItem68;
        private System.Windows.Forms.MenuItem menuItem69;
        private System.Windows.Forms.MenuItem menuItem70;
        private System.Windows.Forms.MenuItem menuItem71;
        private System.Windows.Forms.MenuItem menuItem72;
        private System.Windows.Forms.MenuItem menuItem73;
        private System.Windows.Forms.MenuItem menuItem74;
        private System.Windows.Forms.MenuItem menuItem75;
        private System.Windows.Forms.MenuItem menuItem76;
        private System.Windows.Forms.MenuItem menuItem77;
        private System.Windows.Forms.MenuItem menuItem78;
        private System.Windows.Forms.MenuItem menuItem79;
        public System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem menuItem80;
        private System.Windows.Forms.MenuItem menuItem81;
        private System.Windows.Forms.MenuItem menuItem82;
        private System.Windows.Forms.MenuItem menuItem83;
        private System.Windows.Forms.MenuItem menuItem84;
        private System.Windows.Forms.MenuItem menuItem85;
        public System.Windows.Forms.MenuItem menuItem87;
        private System.Windows.Forms.MenuItem menuItem88;
        public System.Windows.Forms.MenuItem menuItem86;
        private System.Windows.Forms.MenuItem menuItem89;
        public Aga.Controls.Tree.TreeViewAdv treeView;
        private System.Windows.Forms.MenuItem menuItem56;
        private System.Windows.Forms.MenuItem menuItem90;
        private System.Windows.Forms.MenuItem menuItem91;
        public System.Windows.Forms.MenuItem menuItem92;
        private System.Windows.Forms.ToolStripMenuItem áttekintőToolStripMenuItem;
        public System.Windows.Forms.MenuItem menuItem60;
        private System.Windows.Forms.MenuItem menuItem93;
        private System.Windows.Forms.MenuItem menuItem94;
        public System.Windows.Forms.MenuItem menuIthisz1;
        public System.Windows.Forms.MenuItem menuIthisz2;
        public System.Windows.Forms.MenuItem menuIthisz3;
        public System.Windows.Forms.MenuItem menuIthisz4;
        public System.Windows.Forms.MenuItem menuIthisz5;
        private System.Windows.Forms.MenuItem menuIthisz0;
        private System.Windows.Forms.MenuItem menuItem95;
        private System.Windows.Forms.MenuItem menuItem96;
        private System.Windows.Forms.MenuItem menuItem97;
        private System.Windows.Forms.MenuItem menuItem98;
        private System.Windows.Forms.MenuItem menuItem99;
        private System.Windows.Forms.MenuItem logSeparatorMenuItem;
        private System.Windows.Forms.MenuItem menuItem100;
        private System.Windows.Forms.Timer timerVedelem;
        private System.Windows.Forms.MenuItem menuItem101;
        private System.Windows.Forms.ToolStripMenuItem betekintesToolStripMenuItem;
        private System.Windows.Forms.MenuItem menuItem102;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3Elvalaszto;
        private System.Windows.Forms.MenuItem menuItem1;
        public System.Windows.Forms.MenuItem menuItem15;
        private System.Windows.Forms.MenuItem menuItemSupervisor;
        public System.Windows.Forms.MenuItem menuItem14;
        private System.Windows.Forms.MenuItem menuIthisz05;
        private System.Windows.Forms.MenuItem menuIthisz15;
        private System.Windows.Forms.MenuItem menuIthisz25;
        private System.Windows.Forms.MenuItem menuIthisz35;
        private System.Windows.Forms.MenuItem menuIthisz45;
        private System.Windows.Forms.MenuItem menuItem103;
        private System.Windows.Forms.MenuItem menuItem104;
        private System.Windows.Forms.MenuItem menuItem105;
        private System.Windows.Forms.MenuItem menuItem106;
        //public System.Windows.Forms.Timer timer;
    }
}

