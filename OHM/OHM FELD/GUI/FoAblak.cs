/*
 
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
 
  Copyright (C) 2009-2013 Michael Möller <mmoeller@openhardwaremonitor.org>
	Copyright (C) 2010 Paul Werelds <paul@werelds.net>
	Copyright (C) 2012 Prince Samuel <prince.samuel@gmail.com>
    Atdolgozva: Gats Janos Istvan: 2015

*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;
using OpenHardwareMonitor.Hardware;
using OpenHardwareMonitor.WMI;
using OpenHardwareMonitor.Utilities;
using System.Net;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.IO.Ports;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms.Integration;
using System.Windows.Data;
using OpenHardwareMonitor.Eszkozok;

namespace OpenHardwareMonitor.GUI
{
    public partial class FoAblak : Form
    {
        private PersistentSettings settings;
        private UnitManager unitManager;
        public Computer computer;
        private Node root;
        private TreeModel treeModel;
        private IDictionary<ISensor, Color> sensorPlotColors =
          new Dictionary<ISensor, Color>();
        private Color[] plotColorPalette;
        // private SystemTray systemTray;
        private StartupManager startupManager = new StartupManager();
        private UpdateVisitor updateVisitor = new UpdateVisitor();
        private SensorGadget gadget;
        private Form plotForm;
        private PlotPanel plotPanel;

        private UserOption showHiddenSensors;
        private UserOption showPlot;
        private UserOption showValue;
        private UserOption showMin;
        private UserOption showMax;
        private UserOption startMinimized;
        //private UserOption minimizeToTray;
        //private UserOption minimizeOnClose;
        private UserOption autoStart;

        private UserOption readMainboardSensors;
        private UserOption readCpuSensors;
        private UserOption readRamSensors;
        private UserOption readGpuSensors;
        private UserOption readFanControllersSensors;
        private UserOption readHddSensors;

        private UserOption showGadget;
        private UserRadioGroup plotLocation;
        private WmiProvider wmiProvider;

        private UserOption runWebServer;
        private HttpServer server;

        private UserOption logSensors;
        private UserRadioGroup loggingInterval;
        private Logger logger;

        private bool selectionDragging = false;

        public FoAblak()
        {
            Program.FoAblak = this;

            InitializeComponent();

            if (!Program.DEBUG_gyorsinditas)
            {
                // check if the OpenHardwareMonitorLib assembly has the correct version
                if (Assembly.GetAssembly(typeof(Computer)).GetName().Version !=
                  Assembly.GetExecutingAssembly().GetName().Version)
                {
                    MessageBox.Show(
                      "The version of the file OpenHardwareMonitorLib.dll is incompatible.",
                      "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                }

                this.settings = new PersistentSettings();
                this.settings.Load(Path.ChangeExtension(
                  Application.ExecutablePath, ".config"));

                this.unitManager = new UnitManager(settings);

                // make sure the buffers used for double buffering are not disposed 
                // after each draw call
                BufferedGraphicsManager.Current.MaximumBuffer =
                  Screen.PrimaryScreen.Bounds.Size;

                // set the DockStyle here, to avoid conflicts with the MainMenu
                this.splitContainer.Dock = DockStyle.Fill;

                this.Font = SystemFonts.MessageBoxFont;
                treeView.Font = SystemFonts.MessageBoxFont;

                plotPanel = new PlotPanel(settings, unitManager);
                plotPanel.Font = SystemFonts.MessageBoxFont;
                plotPanel.Dock = DockStyle.Fill;

                nodeCheckBox.IsVisibleValueNeeded += nodeCheckBox_IsVisibleValueNeeded;
                nodeTextBoxText.DrawText += nodeTextBoxText_DrawText;
                nodeTextBoxValue.DrawText += nodeTextBoxText_DrawText;
                nodeTextBoxMin.DrawText += nodeTextBoxText_DrawText;
                nodeTextBoxMax.DrawText += nodeTextBoxText_DrawText;
                nodeTextBoxText.EditorShowing += nodeTextBoxText_EditorShowing;

                foreach (TreeColumn column in treeView.Columns)
                    column.Width = Math.Max(20, Math.Min(400,
                      settings.GetValue("treeView.Columns." + column.Header + ".Width",
                      column.Width)));

                treeModel = new TreeModel();
                root = new Node(System.Environment.MachineName);
                root.Image = Utilities.EmbeddedResources.GetImage("computer.png");

                treeModel.Nodes.Add(root);
                treeView.Model = treeModel;

                this.computer = new Computer(settings);

                //systemTray = new SystemTray(computer, settings, unitManager);
                //systemTray.HideShowCommand += hideShowClick;
                //systemTray.ExitCommand += exitClick;

                int p = (int)Environment.OSVersion.Platform;
                if ((p == 4) || (p == 128))
                { // Unix
                    treeView.RowHeight = Math.Max(treeView.RowHeight, 18);
                    splitContainer.BorderStyle = BorderStyle.None;
                    splitContainer.Border3DStyle = Border3DStyle.Adjust;
                    splitContainer.SplitterWidth = 4;
                    treeView.BorderStyle = BorderStyle.Fixed3D;
                    plotPanel.BorderStyle = BorderStyle.Fixed3D;
                    //gadgetMenuItem.Visible = false;
                    //minCloseMenuItem.Visible = false;
                    //minTrayMenuItem.Visible = false;
                    startMinMenuItem.Visible = false;
                }
                else
                { // Windows
                    treeView.RowHeight = Math.Max(treeView.Font.Height + 1, 18);

                    gadget = new SensorGadget(computer, settings, unitManager);
                    gadget.HideShowCommand += hideShowClick;

                    wmiProvider = new WmiProvider(computer);
                }

                logger = new Logger(computer);

                plotColorPalette = new Color[13];
                plotColorPalette[0] = Color.OrangeRed;
                plotColorPalette[1] = Color.Blue;
                plotColorPalette[2] = Color.Green;
                plotColorPalette[3] = Color.LightSeaGreen;
                plotColorPalette[4] = Color.Goldenrod;
                plotColorPalette[5] = Color.DarkViolet;
                plotColorPalette[6] = Color.YellowGreen;
                plotColorPalette[7] = Color.SaddleBrown;
                plotColorPalette[8] = Color.RoyalBlue;
                plotColorPalette[9] = Color.DeepPink;
                plotColorPalette[10] = Color.MediumSeaGreen;
                plotColorPalette[11] = Color.Olive;
                plotColorPalette[12] = Color.Firebrick;

                computer.HardwareAdded += new HardwareEventHandler(HardwareAdded);
                computer.HardwareRemoved += new HardwareEventHandler(HardwareRemoved);

                computer.Open();

                //timer.Enabled = true;

                showHiddenSensors = new UserOption("hiddenMenuItem", false,
                  hiddenMenuItem, settings);
                showHiddenSensors.Changed += delegate (object sender, EventArgs e)
                {
                    treeModel.ForceVisible = showHiddenSensors.Value;
                };

                showValue = new UserOption("valueMenuItem", true, valueMenuItem,
                  settings);
                showValue.Changed += delegate (object sender, EventArgs e)
                {
                    treeView.Columns[1].IsVisible = showValue.Value;
                };

                showMin = new UserOption("minMenuItem", false, minMenuItem, settings);
                showMin.Changed += delegate (object sender, EventArgs e)
                {
                    treeView.Columns[2].IsVisible = showMin.Value;
                };

                showMax = new UserOption("maxMenuItem", true, maxMenuItem, settings);
                showMax.Changed += delegate (object sender, EventArgs e)
                {
                    treeView.Columns[3].IsVisible = showMax.Value;
                };

                startMinimized = new UserOption("startMinMenuItem", false,
                  startMinMenuItem, settings);



                //minimizeOnClose = new UserOption("minCloseMenuItem", false,
                //  minCloseMenuItem, settings);

                //autoStart = new UserOption(null, startupManager.Startup,
                //  startupMenuItem, settings);
                //autoStart.Changed += delegate (object sender, EventArgs e)
                //{
                //    try
                //    {
                //        startupManager.Startup = autoStart.Value;
                //    }
                //    catch (InvalidOperationException)
                //    {
                //        MessageBox.Show("Updating the auto-startup option failed.", "Error",
                //          MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        autoStart.Value = startupManager.Startup;
                //    }
                //};

                readMainboardSensors = new UserOption("mainboardMenuItem", true,
                  mainboardMenuItem, settings);
                readMainboardSensors.Changed += delegate (object sender, EventArgs e)
                {
                    computer.MainboardEnabled = readMainboardSensors.Value;
                };

                readCpuSensors = new UserOption("cpuMenuItem", true,
                  cpuMenuItem, settings);
                readCpuSensors.Changed += delegate (object sender, EventArgs e)
                {
                    computer.CPUEnabled = readCpuSensors.Value;
                };

                readRamSensors = new UserOption("ramMenuItem", true,
                  ramMenuItem, settings);
                readRamSensors.Changed += delegate (object sender, EventArgs e)
                {
                    computer.RAMEnabled = readRamSensors.Value;
                };

                readGpuSensors = new UserOption("gpuMenuItem", true,
                  gpuMenuItem, settings);
                readGpuSensors.Changed += delegate (object sender, EventArgs e)
                {
                    computer.GPUEnabled = readGpuSensors.Value;
                };

                readFanControllersSensors = new UserOption("fanControllerMenuItem", true,
                  fanControllerMenuItem, settings);
                readFanControllersSensors.Changed += delegate (object sender, EventArgs e)
                {
                    computer.FanControllerEnabled = readFanControllersSensors.Value;
                };

                readHddSensors = new UserOption("hddMenuItem", true, hddMenuItem,
                  settings);
                readHddSensors.Changed += delegate (object sender, EventArgs e)
                {
                    computer.HDDEnabled = readHddSensors.Value;
                };

                //     showGadget = new UserOption("gadgetMenuItem", false, gadgetMenuItem,
                //settings);
                //     showGadget.Changed += delegate(object sender, EventArgs e)
                //     {
                //         if (gadget != null)
                //             gadget.Visible = showGadget.Value;
                //     };


                celsiusMenuItem.Checked =
                  unitManager.TemperatureUnit == TemperatureUnit.Celsius;
                fahrenheitMenuItem.Checked = !celsiusMenuItem.Checked;

                //server = new HttpServer(root, this.settings.GetValue("listenerPort", 8085));
                //if (server.PlatformNotSupported)
                //{
                //    webMenuItemSeparator.Visible = false;
                //    webMenuItem.Visible = false;
                //}

                //runWebServer = new UserOption("runWebServerMenuItem", false,
                //  runWebServerMenuItem, settings);
                //runWebServer.Changed += delegate(object sender, EventArgs e)
                //{
                //    if (runWebServer.Value)
                //        server.StartHTTPListener();
                //    else
                //        server.StopHTTPListener();
                //};

                logSensors = new UserOption("logSensorsMenuItem", false, logSensorsMenuItem,
                  settings);

                loggingInterval = new UserRadioGroup("loggingInterval", 0,
                  new[] { log1sMenuItem, log2sMenuItem, log5sMenuItem, log10sMenuItem,
        log30sMenuItem, log1minMenuItem, log2minMenuItem, log5minMenuItem,
        log10minMenuItem, log30minMenuItem, log1hMenuItem, log2hMenuItem,
        log6hMenuItem},
                  settings);
                loggingInterval.Changed += (sender, e) =>
                {
                    switch (loggingInterval.Value)
                    {
                        case 0: logger.LoggingInterval = new TimeSpan(0, 0, 1); break;
                        case 1: logger.LoggingInterval = new TimeSpan(0, 0, 2); break;
                        case 2: logger.LoggingInterval = new TimeSpan(0, 0, 5); break;
                        case 3: logger.LoggingInterval = new TimeSpan(0, 0, 10); break;
                        case 4: logger.LoggingInterval = new TimeSpan(0, 0, 30); break;
                        case 5: logger.LoggingInterval = new TimeSpan(0, 1, 0); break;
                        case 6: logger.LoggingInterval = new TimeSpan(0, 2, 0); break;
                        case 7: logger.LoggingInterval = new TimeSpan(0, 5, 0); break;
                        case 8: logger.LoggingInterval = new TimeSpan(0, 10, 0); break;
                        case 9: logger.LoggingInterval = new TimeSpan(0, 30, 0); break;
                        case 10: logger.LoggingInterval = new TimeSpan(1, 0, 0); break;
                        case 11: logger.LoggingInterval = new TimeSpan(2, 0, 0); break;
                        case 12: logger.LoggingInterval = new TimeSpan(6, 0, 0); break;
                    }
                };

                InitializePlotForm();
                //startupMenuItem.Visible = startupManager.IsAvailable;

                // Create a handle, otherwise calling Close() does not fire FormClosed     
                IntPtr handle = Handle;

                // Make sure the settings are saved when the user logs off
                Microsoft.Win32.SystemEvents.SessionEnded += delegate
                {
                    computer.Close();
                    SaveConfiguration();
                    if (runWebServer.Value)
                        server.Quit();
                };
            }
            ////////////////////Computer ÖSSZEHASONLÍTÁS//////////////////////////////////////////////////////////////////
            //{
            //    try
            //    {
            //        Computer computer2 = new Computer(settings);
            //        computer2.Open();

            //        try
            //        {
            //            computer2.MainboardEnabled = readMainboardSensors.Value;
            //            computer2.CPUEnabled = readCpuSensors.Value;
            //            computer2.RAMEnabled = readRamSensors.Value;
            //            computer2.GPUEnabled = readGpuSensors.Value;
            //            computer2.FanControllerEnabled = readFanControllersSensors.Value;
            //            computer2.HDDEnabled = readHddSensors.Value;
            //        }
            //        catch (Exception e1) { MessageBox.Show("Catch 1\n" + e1.Message); }

            //        if (Marshal.SizeOf(this.computer) == Marshal.SizeOf(computer2))
            //        {
            //            MessageBox.Show("Egyezik\n" + Marshal.SizeOf(this.computer) + "\n" + Marshal.SizeOf(computer2));
            //        }
            //        else
            //        {
            //            MessageBox.Show("Nem egyezik\n" + Marshal.SizeOf(this.computer) + "\n" + Marshal.SizeOf(computer2));
            //        }
            //    }
            //    catch (Exception e2) { MessageBox.Show("Catch 2\n" + e2.Message); }

            //}
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            UdvozloKepernyo.UdvKepernyokMegjelenito.Elhalvanyito();
            startupMenuItem.Enabled = startupManager.IsAvailable;
            startupManager.Startup = Program.KONFAutoIndul;

            TutorTH = new Thread(TutorThread);

            HoMeRok = new Homerok();
            KMutato = new KitTenyMutato(this);
            Program.Attekint = new Attekinto.AttekintoWPF(this);
            ElementHost.EnableModelessKeyboardInterop(Program.Attekint);

            DevForm = new Jatszoter(this);

            if (Program.KONFNyelv != "hun")
                Lokalizalj();

            if (Program.VanSupervisor)
                menuItemSupervisor.Checked = true;
            else
                menuItemSupervisor.Text += " (INACTIVE!)";

            #region Főkonfiguráció Beállítása
            menuItem105.Checked = Program.KONFMindenPCIEszkBetolt;
            toolStripMenuItem3Elvalaszto.Visible = betekintesToolStripMenuItem.Visible = menuItem102.Checked = Program.KONFBetekintoMutat;
            menuItem100.Checked = Program.KONFHDDKernel32Tiltas;
            KMutato.Opacity = HoMeRok.Opacity = this.Opacity = Program.KONFOpacitas;
            HoMeRok.trackBar1.Value = (int)(Program.KONFOpacitas * 100);
            HoMeRok.checkBox1.Checked = Program.KONFFelulMarado;
            menuItem55.Checked = Program.KONFFrissitesInditaskor;
            Program.Attekint.Topmost = HoMeRok.TopMost = KMutato.TopMost = this.TopMost = menuItem6.Checked = felülMaradóToolStripMenuItem.Checked = Program.KONFFelulMarado;
            előtérbeToolStripMenuItem.Enabled = !Program.KONFFelulMarado;
            if (Program.KONFHomersMutat) HoMeRok.ShowHelppel(); menuItem39.Checked = Program.KONFHomersMutat;
            if (Program.KONFKittenyMutat) KMutato.Show(); menuItem37.Checked = Program.KONFKittenyMutat;
            Program.HomerokNyitva = new UserOption("HomerokNyitva", true, menuItem39, new PersistentSettings());
            Program.KitTenyMutNyitva = new UserOption("KitTenyMutNyitva", true, menuItem37, new PersistentSettings());
            Program.HomerokNyitva.Value = Program.KONFHomersMutat;
            Program.KitTenyMutNyitva.Value = Program.KONFKittenyMutat;
            startMinMenuItem.Checked = Program.KONFKismeretIndit;
            try { Program.FoAblak.menuItem15.Text = ((Program.KONFNyelv == "hun") ? "Auto Indítás Késleltetése" : Eszk.GetNyelvSzo("MFmenuItem15")) + " (" + (Program.KONFAutoIndKesleltetes / (double)1000) + " sec)"; } catch { }
            if (!Program.KONFKismeretIndit) Show();
            Program.HomerokNyitva.Changed += delegate (object sender, EventArgs e)
            {
                try
                {
                    menuItem39.Checked = Program.KONFHomersMutat = hőmérsékletekToolStripMenuItem.Checked = Program.HomerokNyitva.Value;
                    if (Program.HomerokNyitva.Value)
                    {
                        HomersMutatFrissit();
                        HoMeRok.ShowHelppel();
                    }
                    else
                        HoMeRok.HideHelppel();
                }
                catch (NullReferenceException) { }
            };

            Program.KitTenyMutNyitva.Changed += delegate (object sender, EventArgs e)
            {
                try
                {
                    menuItem37.Checked = Program.KONFKittenyMutat = fordulatszámokToolStripMenuItem.Checked = Program.KitTenyMutNyitva.Value;
                    if (Program.KitTenyMutNyitva.Value)
                    {
                        KittenyMutatFrissit();
                        KMutato.Show();
                    }
                    else
                        KMutato.Hide();
                }
                catch (NullReferenceException) { }
            };


            AttekintoLathatosagAktival();

            Program.HomerokNyitva.Value = Program.KONFHomersMutat;
            Program.KitTenyMutNyitva.Value = Program.KONFKittenyMutat;
            menuItem57.Checked = Program.KONFVanVezerlo;

            Eszkozok.Eszk.Elnevezo();

            menuItem57.Click += delegate (object sender, EventArgs e)
            {
                menuItem57.Checked = !menuItem57.Checked;
                try
                {
                    if (menuItem57.Checked == false)
                    {
                        if (MessageBox.Show((Program.KONFNyelv == "hun") ? "Biztos benne, hogy leválasztja a célhardvert?\nHa megteszi, a ventilátorok fordulatszámai nem lesznek elküldve\na célhardvernek és Ön erről nem kap majd hibajelzést.\n\nA vezérlő újra csatlakoztatható, ha ismét erre a menüpontra kattint." : Eszk.GetNyelvSzo("CELHLEVTORZS"), (Program.KONFNyelv == "hun") ? "CÉLHARDVER LEVÁLASZTÁSA" : Eszk.GetNyelvSzo("CELHLEVCIM"), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            Program.KONFVanVezerlo = false;
                            Eszkozok.Eszk.Elnevezo();

                            try { Program.SorosPort.Close(); }
                            catch { }
                        }
                        else
                            menuItem57.Checked = true;
                    }
                    else
                    {
                        NTFYI_szamlalo = 0;
                        KezfogasSzalIndito(true);
                        Program.KONFVanVezerlo = true;
                        Eszkozok.Eszk.Elnevezo();
                    }
                }
                catch (NullReferenceException) { }
            };

            startupMenuItem.Checked = Program.KONFAutoIndul;
            HisztJelolo();

            if (Program.KONFUdvKeperny)
                menuItem95.Checked = true;
            #endregion

            #region Hitelesít/Aktivál
            //#if !DEBUG
            Program.FelallasOtaEltelIdo.Start();
            if (!File.Exists("lic\\hit.ved") || !File.Exists("lic\\hitcr.ved"))
            {
                try
                {
                    this.Opacity = 0;
                    SysTrayicon.Visible = false;
                    Program.Attekint.Dispatcher.Invoke(delegate () { Program.Attekint.Opacity = 0; Program.Attekint.ShowInTaskbar = false; });
                }
                catch { }
                try { HoMeRok.Opacity = 0; } catch { }

                Aktivalo akt = new Aktivalo();
                akt.ShowDialog(this);

                try
                {
                    this.Opacity = 1;
                    SysTrayicon.Visible = true;
                    Program.Attekint.Dispatcher.Invoke(delegate () { Program.Attekint.Opacity = 1; Program.Attekint.ShowInTaskbar = true; });
                }
                catch { }
                try { HoMeRok.Opacity = 1; } catch { }

                if (akt.BezarasiUzenet != "")
                {
                    Program.FoAblak.SysTrayicon.ShowBalloonTip(3000, "Feverkill", akt.BezarasiUzenet, ToolTipIcon.Info);
                }
            }
            else
            {
                new Thread(Vedelem.IndulasiHitelesites).Start();
            }
            //#endif
            #endregion

            timerFreemiumReklam.Elapsed += timerFreemiumReklam_Elapsed;

            Program.HomersKuldTH = new System.Threading.Thread(KitoltTenyKuld);
            Program.HomersKuldTH.Start();

            #region Felallasi Uzenet
            if (!Program.DEBUG_gyorsinditas)
            {
                if (Program.frissitve == false)
                {
                    if (Program.hibafrissiteskor != null)
                    {
                        SysTrayicon.ShowBalloonTip(800, "Feverkill", ((Program.KONFNyelv == "hun") ? "Hiba a frissítések keresésekor!" : Eszk.GetNyelvSzo("TRUZFrissHibaSZOVEG")) + "\n>>" + Program.hibafrissiteskor, ToolTipIcon.Info);
                    }
                    else
                    {
                        if (Program.naprakesz == null)
                            SysTrayicon.ShowBalloonTip(800, "Feverkill", ((Program.KONFNyelv == "hun") ? "Nem keresett frissítést" : Eszk.GetNyelvSzo("TRUZNemFrissSZOVEG")), ToolTipIcon.Info);
                        else if (Program.naprakesz == true)
                        {
                            SysTrayicon.ShowBalloonTip(800, "Feverkill", ((Program.KONFNyelv == "hun") ? "Naprakész" : Eszk.GetNyelvSzo("TRUZNaprakeszSZOVEG")), ToolTipIcon.Info);
                        }
                        else
                        {
                            SysTrayicon.ShowBalloonTip(1500, "Feverkill", ((Program.KONFNyelv == "hun") ? "Újabb verzió érhető el\nHaladó ablak: Gyorsbillentyű:" : Eszk.GetNyelvSzo("TRUZUjVerzVanSZOVEG")) + " Ctrl + 2", ToolTipIcon.Info);
                        }
                    }
                }
                else
                {
                    SysTrayicon.ShowBalloonTip(800, ((Program.KONFNyelv == "hun") ? "Feverkill Frissítve" : Eszk.GetNyelvSzo("TRUZFrissitveCIM")), "V" + Program.Verzioszam, ToolTipIcon.Info);
                }

            }
            else
                SysTrayicon.ShowBalloonTip(800, "Feverkill DEBUG", "Gyorsindítás", ToolTipIcon.Warning);

            if (!Program.VanSupervisor)
                SysTrayicon.ShowBalloonTip(3000, "Feverkill Supervisor is INACTIVE", "For maximal protection, start Feverkill by \"FeverkillSupervisor.exe\"!", ToolTipIcon.Error);

            if (Program.KONFKellMegNyelvvalasztas)
            {
                Nyelvvalaszto NYelvV = new Nyelvvalaszto();
                NYelvV.ShowDialog();
            }


            if (Program.KONFTutorialMegjelenit)
            {
                //if (MessageBox.Show(((Program.KONFNyelv == "hun") ? "A program jelen telepítésének ez az első indítása.\n\nEl akarja indítani a tutorialt?" : Eszk.GetNyelvSzo("MboxElsoind1SZOVEG"]), "Tutorial", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                //{
                menuItem86.PerformClick();
                //}
                //else
                //MessageBox.Show(((Program.KONFNyelv == "hun") ? "A tutorial bármikor elindítható, a jobbra fent látható \"?\" gombra, vagy a \"Haladó Ablak: Segítség>>Tutorial Mód Aktív\" menüpontra kattintva." : Eszk.GetNyelvSzo("MboxElsoind2SZOVEG"]), "");

                //Program.KONFKellTutorial = false;
                //Fajlkezelo.FoKonfMento();
            }
            #endregion

            try
            {
                Program.Ervenyesek = Fajlkezelo.ErvenyListBeolvas();
                Program.Attekint.listViewSzablistak.Dispatcher.Invoke(AttekintSzablistFrissit);
                Program.Attekint.listViewRiaszt.Dispatcher.Invoke(AttekintRiasztFrissit);
                Program.Attekint.labelFrissId.Dispatcher.Invoke(delegate () { Program.Attekint.labelFrissId.Content = ((Program.KONFNyelv == "hun") ? "Frissítési időköz: " : (Eszk.GetNyelvSzo("ATTEKUIFrissido") + ": ")) + ((double)Program.KONFFrisIdo / (double)1000).ToString() + " sec"; });
            }
            catch { }

            if (Program.LICENSZTipus == 10)//Ha próbaverziós (akár Freemium, akár teljes próbaverzió), egy db 'GetFullVersion' menuitemet mindenképpen csinál
            {
                MenuItem Menitx;

                Menitx = new MenuItem();
                Menitx.Text = "Get Full Version ...";
                Menitx.Click += new System.EventHandler(delegate (object sender, EventArgs e) { Eszkozok.Eszk.GetFullVersion(); });
                Program.FoAblak.menuItemSegitseg.MenuItems.Add(Menitx);

                Program.FoAblak.menuItem14.Visible = true;
            }
        }

        System.Timers.Timer timerFreemiumReklam = new System.Timers.Timer() { Interval = 5000, AutoReset = true };

        public Jatszoter DevForm;

        public int NTFYI_szamlalo = 0;

        public Homerok HoMeRok;

        public KitTenyMutato KMutato;
        //List<byte> HoMersek;
        Dictionary<string, float> HoMersek;
        public Dictionary<string, float> HiszterezisesHomok;
        Dictionary<string, bool> HiszMaxValtHom;//true: növekvő hőmérséklet, a hiszterézis felfelé engedékeny; (false: csökken, lefelé enged)

        public List<Program.HoMers> HommMersek = new List<Program.HoMers>();

        public bool[] DirektVez = new bool[8];
        public byte[] KitTenyezok = new byte[8];

        public Dictionary<IControl, byte> KitTenyekAlaplapi = new Dictionary<IControl, byte>();
        public Dictionary<Program.EmulaltBelsoVenti, byte> KitTenyekAlaplapiEMULALT = new Dictionary<Program.EmulaltBelsoVenti, byte>();
        static bool Elsokor = true;
        static uint AktIteracioSzama = 0;
        public List<SensorNode> SndLsita = new List<SensorNode>();
        bool HianyzoAlaplapiControl = true;
        string[] Ertekek;


        public bool GetFullVersionCreateKellMeg = true;

        Stopwatch sw = new Stopwatch();
        long idok = 0;
        Stopwatch sw1 = new Stopwatch();
        long idok1 = 0;
        Stopwatch sw2 = new Stopwatch();
        long idok2 = 0;
        Stopwatch sw3 = new Stopwatch();
        long idok3 = 0;
        Stopwatch sw4 = new Stopwatch();
        long idok4 = 0;
        Stopwatch sw5 = new Stopwatch();
        long idok5 = 0;
        Stopwatch sw6 = new Stopwatch();
        long idok6 = 0;
        Stopwatch sw7 = new Stopwatch();
        long idok7 = 0;
        Stopwatch sw8 = new Stopwatch();
        long idok8 = 0;
        Stopwatch sw9 = new Stopwatch();
        long idok9 = 0;
        Stopwatch sw10 = new Stopwatch();
        long idok10 = 0;
        Stopwatch sw11 = new Stopwatch();
        long idok11 = 0;
        Stopwatch sw12 = new Stopwatch();
        long idok12 = 0;
        void KitoltTenyKuld()
        {
            KezfogasSzalIndito(false);
            HoMersek = new Dictionary<string, float>();
            HiszterezisesHomok = new Dictionary<string, float>();
            HiszMaxValtHom = new Dictionary<string, bool>();
            //HoMersek = new List<byte>();
            while (true)
            {
                sw.Reset();
                sw.Start();

                sw12.Reset();
                sw12.Start();
                //Thread.Sleep(100);
                sw12.Stop();
                idok12 = sw12.ElapsedTicks;

                sw1.Reset();
                sw1.Start();
                try
                {
                    timer_Tick(0, null);//COMPUTER ADATOK LEKÉRÉSE, FRISSÍTÉSE (timer KIKAPCSOLVA)
                }
                catch
                { }
                sw1.Stop();
                idok1 = sw1.ElapsedTicks;

                sw2.Reset();
                sw2.Start();
                try
                {
                    if (!Eszkozok.Eszk.IsPremiumFuncEabled() && Program.FelallasOtaEltelIdo.ElapsedMilliseconds > 40000)//40 seccel az aktiválás megkezdése utántól
                    {
                        if (GetFullVersionCreateKellMeg)
                        {
                            Eszkozok.Eszk.CreateTOBBIGetFullVersionMenuitem();
                        }
                        if (!timerFreemiumReklam.Enabled)
                        {
                            timerFreemiumReklam.Enabled = true;
                        }
                        if (Program.Ervenyesek.Count > 2)
                        {
                            try
                            {
                                Program.Ervenyesek.RemoveRange(2, Program.Ervenyesek.Count - 2);
                            }
                            catch { }
                            try
                            {
                                Program.Attekint.listViewSzablistak.Dispatcher.Invoke(AttekintSzablistFrissit);
                            }
                            catch { }
                            SysTrayicon.ShowBalloonTip(Program.KONFFrisIdo * 4, "Freemium Feverkill", "Szabályzólisták korlátozva: max 2db\nControl Lists limited: max 2pc\nIf this is a Trial version, you need internet to use it as Full version!", ToolTipIcon.Warning);
                        }
                        if (Program.Riasztasok.Count > 2)
                        {
                            try
                            {
                                Program.Riasztasok.RemoveRange(2, Program.Riasztasok.Count - 2);
                            }
                            catch { }
                            try
                            {
                                Program.Attekint.listViewRiaszt.Dispatcher.Invoke(AttekintRiasztFrissit);
                            }
                            catch { }
                            SysTrayicon.ShowBalloonTip(Program.KONFFrisIdo * 4, "Freemium Feverkill", "Riasztások korlátozva: max 2db\nAlerts limited: max 2pc\nIf this is a Trial version, you need internet to use it as Full version!", ToolTipIcon.Warning);
                        }
                        bool cimkelimital = false;
                        for (int i = 1; i < Program.CsatCimkekCelh.Length; i++)
                        {
                            if (Program.CsatCimkekCelh[i] != "--")
                            {
                                cimkelimital = true;
                            }
                        }
                        if (cimkelimital)
                        {
                            try
                            {
                                for (int i = 1; i < Program.CsatCimkekCelh.Length; i++)
                                {
                                    Program.CsatCimkekCelh[i] = "--";
                                }
                            }
                            catch { }
                            try
                            {
                                Program.Attekint.listViewFordszamok.Dispatcher.Invoke(AttekintFordszamFrissit);
                            }
                            catch { }
                            SysTrayicon.ShowBalloonTip(Program.KONFFrisIdo, "Freemium Feverkill", "Címkék korlátozva: max 2db\nLabels limited: max 2pc", ToolTipIcon.Warning);
                        }
                    }
                    else if (Eszkozok.Eszk.IsPremiumFuncEabled())
                    {
                        timerFreemiumReklam.Enabled = false;
                    }
                }
                catch { }

                sw2.Stop();
                idok2 = sw2.ElapsedTicks;
                { //DESTRUKTOR ZAROJEL
                  //////////
                  //sw.Stop();
                  //sw.Restart();
                  //sw.Start();
                  //////////
                    #region FOKONFIGURACIOSFAJL IRASA
                    if (Fajlkezelo.KiirtKONFTESZT() == false)
                    {
                        Fajlkezelo.FoKonfMento();
                    }
                    #endregion
                    ///////////////
                    //sw.Stop();
                    //idok[0] += sw.ElapsedTicks;
                    //sw.Restart();
                    //////////////

                    sw3.Reset();
                    sw3.Start();

                    #region HOMEROK LISTVIEW KESZITES
                    try
                    {
                        if (Program.BEMUTATO_SzenzBemenet == Program.SzenzorBemenet.BelsoSzenzor)
                        {///////////////////NORMÁL MŰKÖDÉS
                            sw10.Reset();
                            sw10.Start();
                            if (!Program.DEBUG_gyorsinditas)
                                Ertekek = this.computer.Szenzorertekek();
                            sw10.Stop();
                            idok10 = sw10.ElapsedTicks;
                            // bool teszt1 = true;
                            HommMersek.Clear();
                            for (int i = 0; i < Ertekek.Length; i++)
                            {
                                if (Ertekek[i].ToLower().Contains("temperature") || Ertekek[i].ToLower().Contains("hőmérséklet") || Ertekek[i].ToLower().Contains("homérséklet") || Ertekek[i].ToLower().Contains("homerseklet") || Ertekek[i].ToLower().Contains("hőmerseklet"))
                                {
                                    // if(teszt1)
                                    //HoMeRok.Text = Ertekek[i];
                                    // teszt1 = false;

                                    Program.HoMers buff = new Program.HoMers();
                                    buff.Ertek = Ertekek[i].Split(':')[1].Split('{')[0].Replace(" ", "") + " °C";
                                    for (int x = i; x >= 0; --x)
                                    {
                                        if (Ertekek[x].Contains("\\"))
                                        {
                                            buff.Csop = Ertekek[x].Split('\\')[Ertekek[x].Split('\\').Length - 1];
                                            buff.Nev = Ertekek[i].Split('|')[Ertekek[i].Split('|').Length - 1].Split(':')[0];
                                            break;
                                        }
                                    }
                                    HommMersek.Add(buff);
                                }
                            }
                        }
                        else if (Program.BEMUTATO_SzenzBemenet == Program.SzenzorBemenet.KulsoSzenzor)
                        {///////////////////BEMUTATÓ KÜLSŐ SZENZOROKKAL
                            List<Program.HoMers> buff = KulsoHoszenzor.HomListAd();
                            if (buff.Count != 0)
                                HommMersek = buff;

                            //Program.HoMers buf; buf.Csop = "ArdKulso"; buf.Nev = "Szenzor #1"; buf.Ertek = "30.19";
                            //HommMersek.Add(buf);
                            //HommMersek.Add("ArdKulso =->> Szenzor #1", 28);

                        }
                        else if (Program.BEMUTATO_SzenzBemenet == Program.SzenzorBemenet.EmulaltSzenzor)
                        {
                            List<Program.HoMers> buff = new List<Program.HoMers>();

                            {
                                Program.HoMers Buffx;
                                Buffx.Csop = "Emulated Test Sensors Group 1";
                                Buffx.Nev = "Sensor #1";
                                Buffx.Ertek = ((double)DevForm.trackBar1.Value / 100).ToString() + " °C";
                                buff.Add(Buffx);
                            }
                            {
                                Program.HoMers Buffx;
                                Buffx.Csop = "Emulated Test Sensors Group 1";
                                Buffx.Nev = "Sensor #2";
                                Buffx.Ertek = ((double)DevForm.trackBar2.Value / 100).ToString() + " °C";
                                buff.Add(Buffx);
                            }
                            {
                                Program.HoMers Buffx;
                                Buffx.Csop = "Emulated Test Sensors Group 2";
                                Buffx.Nev = "Sensor #3";
                                Buffx.Ertek = ((double)DevForm.trackBar3.Value / 100).ToString() + " °C";
                                buff.Add(Buffx);
                            }
                            {
                                Program.HoMers Buffx;
                                Buffx.Csop = "Emulated Test Sensors Group 2";
                                Buffx.Nev = "Sensor #4";
                                Buffx.Ertek = ((double)DevForm.trackBar4.Value / 100).ToString() + " °C";
                                buff.Add(Buffx);
                            }
                            {
                                Program.HoMers Buffx;
                                Buffx.Csop = "Emulated Test Sensors Group 2";
                                Buffx.Nev = "Sensor #5";
                                Buffx.Ertek = ((double)DevForm.trackBar5.Value / 100).ToString() + " °C";
                                buff.Add(Buffx);
                            }

                            HommMersek = buff;
                        }

                        //HoMeRok.listView1.Items.Clear();
                        //HoMeRok.listView1.Groups.Clear();
                        if (Program.KONFHomersMutat)
                        {
                            HomersMutatFrissit();
                        }
                        /////////////////ATTEKINTO////////////////////////// 
                        if (Program.KONFAttekintMutat)
                        {
                            Program.Attekint.listViewHomers.Items.Dispatcher.Invoke(AttekintHomersFrissit);
                        }
                        /////////////////BETEKINTO////////////////////////// 
                        if (Program.KONFBetekintoMutat)
                        {
                            toolStripMenuItem3Elvalaszto.Visible = betekintesToolStripMenuItem.Visible = true;
                            BetekintoFrissit();
                        }
                        else
                        {
                            toolStripMenuItem3Elvalaszto.Visible = betekintesToolStripMenuItem.Visible = false;
                        }

                    }
                    catch { }
                    #endregion
                    sw3.Stop();
                    idok3 = sw3.ElapsedTicks;

                    sw4.Reset();
                    sw4.Start();
                    #region HOMERSEKLET-JELENTES FELDOLGOZASA

                    try
                    {
                        HoMersek.Clear();
                        foreach (Program.HoMers item in HommMersek)
                        {
                            try
                            {
                                string szenzornev = item.Csop + " =->> " + item.Nev;

                                HoMersek.Add(szenzornev, float.Parse(item.Ertek.Replace(" ", "").Split('°')[0]));

                                if (HiszterezisesHomok.ContainsKey(szenzornev) == false)
                                    HiszterezisesHomok.Add(szenzornev, -2000);
                                if (HiszMaxValtHom.ContainsKey(szenzornev) == false)
                                    HiszMaxValtHom.Add(szenzornev, true);
                            }
                            catch
                            { }
                        }
                    }
                    catch { }
                    #endregion

                    sw4.Stop();
                    idok4 = sw4.ElapsedTicks;
                    ///////////////
                    //sw.Stop();
                    //idok[1] += sw.ElapsedTicks;
                    //sw.Restart();
                    //////////////

                    ///////////////
                    //sw.Stop();
                    //idok[2] += sw.ElapsedTicks;
                    //sw.Restart();
                    //////////////

                    ///////////////
                    //sw.Stop();
                    //idok[3] += sw.ElapsedTicks;
                    //sw.Restart();
                    //////////////

                    //if (!Program.AzonnaliVez)
                    //{
                    //if (Program.Ervenyesek.Count != 0)
                    //{

                    try
                    {
                        while (Program.ErvenyVanIras)
                            System.Threading.Thread.Sleep(50);
                        Program.ErvenyVanOlvasas = true;


                        ///////////////
                        //sw.Stop();
                        //idok[4] += sw.ElapsedTicks;
                        //sw.Restart();
                        //////////////
                        sw5.Reset();
                        sw5.Start();

                        #region CSATORNATENYEZOK BEALLITASA
                        //for (int i = 0; i < KitTenyezok.Length; i++)
                        //{
                        //    MasoltKitTenyezok[i] = KitTenyezok[i];
                        //    //KitTenyezok[i] = -99;
                        //}
                        KitTenyekAlaplapi.Clear();
                        KitTenyekAlaplapiEMULALT.Clear();

                        sw11.Reset();
                        sw11.Start();
                        if (Program.BelsoVentiVezTip == Program.BelsoVentiTipus.Hardware)
                        {
                            if (HianyzoAlaplapiControl || Elsokor)
                            {
                                try
                                {
                                    foreach (TreeNodeAdv item in treeView.AllNodes)
                                    {
                                        foreach (NodeControlInfo NCInfo in treeView.GetNodeControls(item))
                                        {
                                            SensorNode snd = NCInfo.Node.Tag as SensorNode;
                                            if (snd != null)
                                                if (snd.Sensor != null)
                                                    if (snd.Sensor.Control != null)
                                                    {
                                                        if (SndLsita.Contains(snd) == false)
                                                            SndLsita.Add(snd);
                                                    }
                                        }
                                    }
                                }
                                catch { }
                                HianyzoAlaplapiControl = false;
                            }
                        }
                        else if (Program.BelsoVentiVezTip == Program.BelsoVentiTipus.Emulalt)
                        {
                            if (DevForm.numericUpDownFanNumber.Value != Program.EmulaltBelsoVentik.Count)
                            {
                                while (DevForm.numericUpDownFanNumber.Value > Program.EmulaltBelsoVentik.Count)
                                {
                                    Program.EmulaltBelsoVenti buffx = new Program.EmulaltBelsoVenti();
                                    buffx.csoport = "Emuladet Built-in Fans";
                                    buffx.nev = "Emulated Fan Control " + Convert.ToChar(65 + Program.EmulaltBelsoVentik.Count);
                                    buffx.ertek = 0;
                                    buffx.ControlMode = ControlMode.Listaalapu;
                                    Program.EmulaltBelsoVentik.Add(buffx);
                                }

                                while (DevForm.numericUpDownFanNumber.Value < Program.EmulaltBelsoVentik.Count)
                                {
                                    Program.EmulaltBelsoVentik.RemoveAt(Program.EmulaltBelsoVentik.Count - 1);
                                }
                            }
                        }

                        List<byte>[] HozzarendeltFszamok = new List<byte>[8];
                        for (int i = 0; i < HozzarendeltFszamok.Length; i++)
                        {
                            HozzarendeltFszamok[i] = new List<byte>();
                        }
                        bool HisztDeaktivTRUEVOLT = Program.HisztDeaktiv;

                        sw11.Stop();
                        idok11 = sw11.ElapsedTicks;

                        foreach (Program.SzabLista AKTUALISLista in Program.Ervenyesek)
                        {
                            try
                            {
                                if (!HiszterezisesHomok.ContainsKey(AKTUALISLista.Homero) || !HoMersek.ContainsKey(AKTUALISLista.Homero) || !HiszMaxValtHom.ContainsKey(AKTUALISLista.Homero) || Program.HisztDeaktiv)
                                {
                                    if (HoMersek.ContainsKey(AKTUALISLista.Homero))
                                    {
                                        if (!HiszterezisesHomok.ContainsKey(AKTUALISLista.Homero))
                                        {
                                            HiszterezisesHomok.Add(AKTUALISLista.Homero, HoMersek[AKTUALISLista.Homero]);
                                            HiszMaxValtHom[AKTUALISLista.Homero] = true;
                                        }
                                        else
                                        {
                                            if (HoMersek[AKTUALISLista.Homero] >= HiszterezisesHomok[AKTUALISLista.Homero])
                                                HiszMaxValtHom[AKTUALISLista.Homero] = true;
                                            else
                                                HiszMaxValtHom[AKTUALISLista.Homero] = false;

                                            HiszterezisesHomok[AKTUALISLista.Homero] = HoMersek[AKTUALISLista.Homero];
                                        }
                                    }
                                }
                                else if (Math.Abs(HoMersek[AKTUALISLista.Homero] - HiszterezisesHomok[AKTUALISLista.Homero]) > 0.02)//Ha az előző és a mostani eltér
                                {
                                    if (HiszMaxValtHom[AKTUALISLista.Homero])
                                    {//növekvő
                                        if (HoMersek[AKTUALISLista.Homero] > HiszterezisesHomok[AKTUALISLista.Homero])
                                        {
                                            HiszterezisesHomok[AKTUALISLista.Homero] = HoMersek[AKTUALISLista.Homero];
                                        }
                                        else if (HoMersek[AKTUALISLista.Homero] <= HiszterezisesHomok[AKTUALISLista.Homero])
                                        {
                                            if (HiszterezisesHomok[AKTUALISLista.Homero] - HoMersek[AKTUALISLista.Homero] >= Program.KONFHiszterezis)
                                            {
                                                HiszterezisesHomok[AKTUALISLista.Homero] = HoMersek[AKTUALISLista.Homero];
                                                HiszMaxValtHom[AKTUALISLista.Homero] = false;
                                            }
                                        }
                                    }
                                    else
                                    {//csökkenő
                                        if (HoMersek[AKTUALISLista.Homero] < HiszterezisesHomok[AKTUALISLista.Homero])
                                        {
                                            HiszterezisesHomok[AKTUALISLista.Homero] = HoMersek[AKTUALISLista.Homero];
                                        }
                                        else if (HoMersek[AKTUALISLista.Homero] >= HiszterezisesHomok[AKTUALISLista.Homero])
                                        {
                                            if (HoMersek[AKTUALISLista.Homero] - HiszterezisesHomok[AKTUALISLista.Homero] >= Program.KONFHiszterezis)
                                            {
                                                HiszterezisesHomok[AKTUALISLista.Homero] = HoMersek[AKTUALISLista.Homero];
                                                HiszMaxValtHom[AKTUALISLista.Homero] = true;
                                            }
                                        }
                                    }
                                }
                            }
                            catch { }

                            if (HoMersek.ContainsKey(AKTUALISLista.Homero))
                            {
                                if (!AKTUALISLista.VezTipListaalapu)
                                {//PID VEZÉRLÉS

                                    try
                                    {
                                        byte ertek = (byte)AKTUALISLista.PIDObjektum.GetKovetkFordszam(Program.KONFFrisIdo, HoMersek[AKTUALISLista.Homero]);

                                        for (int csat = 0; csat < 8; ++csat) //Osszes Celhardveres csatorna dinamikusan kezelve
                                        {
                                            if (AKTUALISLista.Csatornak.Contains("," + Convert.ToString(csat + 1) + ",") && !DirektVez[csat])
                                            {
                                                HozzarendeltFszamok[csat].Add(ertek);
                                            }
                                        }
                                        //this.Text = /*"ki: " + AKTUALISLista.PIDObjektum.Output + */" err: " + AKTUALISLista.PIDObjektum.error + " P: " + AKTUALISLista.PIDObjektum.error* AKTUALISLista.PIDObjektum.Kp + " I: "  + AKTUALISLista.PIDObjektum.integral * AKTUALISLista.PIDObjektum.Ki + " D: " + AKTUALISLista.PIDObjektum.derivative * AKTUALISLista.PIDObjektum.Kd;
                                        if (Program.BelsoVentiVezTip == Program.BelsoVentiTipus.Hardware)
                                        {
                                            int talatcsatornak = 0;
                                            foreach (SensorNode snd in SndLsita)
                                            {
                                                IControl control = snd.Sensor.Control;
                                                if (AKTUALISLista.Csatornak.Contains(control.Identifier + " => " + snd.Text))
                                                {
                                                    ++talatcsatornak;
                                                    if (KitTenyekAlaplapi.ContainsKey(control))
                                                    {
                                                        if (KitTenyekAlaplapi[control] < ertek)
                                                        {
                                                            KitTenyekAlaplapi[control] = ertek;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        KitTenyekAlaplapi.Add(control, ertek);
                                                    }
                                                }
                                            }
                                            if (talatcsatornak < AKTUALISLista.GetAlaplapiControlDarabszam())
                                            {
                                                HianyzoAlaplapiControl = true;
                                            }
                                        }
                                        else if (Program.BelsoVentiVezTip == Program.BelsoVentiTipus.Emulalt)
                                        {
                                            foreach (Program.EmulaltBelsoVenti control in Program.EmulaltBelsoVentik)
                                            {
                                                if (AKTUALISLista.Csatornak.Contains(control.csoport + " => " + control.nev))
                                                {
                                                    if (KitTenyekAlaplapiEMULALT.ContainsKey(control))
                                                    {
                                                        if (KitTenyekAlaplapiEMULALT[control] < ertek)
                                                        {
                                                            KitTenyekAlaplapiEMULALT[control] = ertek;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        KitTenyekAlaplapiEMULALT.Add(control, ertek);
                                                    }
                                                }

                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        try
                                        {
                                            SysTrayicon.ShowBalloonTip(1000, "PID.GetKovetkFordszam() Error", e.Message + " " + e.StackTrace, ToolTipIcon.Error);
                                        }
                                        catch { }
                                    }
                                }
                                else
                                {//LISTAALAPÚ VEZÉRLÉS
                                 //for (int x = 0; x < 46; ++x)
                                 //{
                                    try
                                    {
                                        byte AktualisPWM = 50;

                                        if (HiszterezisesHomok[AKTUALISLista.Homero] >= 110)
                                        {
                                            AktualisPWM = AKTUALISLista.PWM[45];
                                        }
                                        else if (HiszterezisesHomok[AKTUALISLista.Homero] < 20)
                                        {
                                            AktualisPWM = AKTUALISLista.PWM[0];
                                        }
                                        else
                                        {
                                            double pontosertek = (HiszterezisesHomok[AKTUALISLista.Homero] - (float)20) / (float)2;

                                            int egeszresz = (int)pontosertek;
                                            double arany = pontosertek - egeszresz;

                                            short hozzaadando = 0;//előjeles típus, mert lehet negatív is, ha a nagyobb indexű csúszka értéke kisebb

                                            if (egeszresz < 45)//van még egy egyel nagyobb elem a PWM listában
                                            {
                                                hozzaadando = (short)((AKTUALISLista.PWM[egeszresz + 1] - AKTUALISLista.PWM[egeszresz]) * arany);
                                            }

                                            AktualisPWM = (byte)(AKTUALISLista.PWM[egeszresz] + hozzaadando);
                                        }


                                        #region ElozoBugosMegoldasok
                                        //if (!ValtottHomok.Contains(AKTUALISLista.Homero)) { ValtottHomok.Add(AKTUALISLista.Homero);}

                                        ///////////////
                                        //sw5.Stop();
                                        ////idok5[0] += sw5.ElapsedTicks;
                                        //sw5.Restart();
                                        //////////////
                                        #region CsatornakEgyesevel
                                        ////1. Csatorna
                                        //if (AKTUALISLista.Csatornak.Contains(",1,") && !DirektVez[0])
                                        //{
                                        //    Valtoztatva[0] = true;
                                        //    if (HoMersek[AKTUALISLista.Homero] >= ((x * 2) + 20) && HoMersek[AKTUALISLista.Homero] < (((x + 1) * 2) + 20) && KitTenyezok[0] < AKTUALISLista.PWM[x])
                                        //        KitTenyezok[0] = AKTUALISLista.PWM[x];
                                        //    else if (HoMersek[AKTUALISLista.Homero] >= 110)
                                        //        KitTenyezok[0] = AKTUALISLista.PWM[AKTUALISLista.PWM.Length - 1];
                                        //    else if (HoMersek[AKTUALISLista.Homero] < 20)
                                        //        KitTenyezok[0] = AKTUALISLista.PWM[0];
                                        //}
                                        ////2. Csatorna
                                        //if (AKTUALISLista.Csatornak.Contains(",2,") && !DirektVez[1])
                                        //{
                                        //    Valtoztatva[1] = true;
                                        //    if (HoMersek[AKTUALISLista.Homero] >= ((x * 2) + 20) && HoMersek[AKTUALISLista.Homero] < (((x + 1) * 2) + 20) && KitTenyezok[1] < AKTUALISLista.PWM[x])
                                        //        KitTenyezok[1] = AKTUALISLista.PWM[x];
                                        //    else if (HoMersek[AKTUALISLista.Homero] >= 110)
                                        //        KitTenyezok[1] = AKTUALISLista.PWM[AKTUALISLista.PWM.Length - 1];
                                        //    else if (HoMersek[AKTUALISLista.Homero] < 20)
                                        //        KitTenyezok[1] = AKTUALISLista.PWM[0];
                                        //}
                                        ////3. Csatorna
                                        //if (AKTUALISLista.Csatornak.Contains(",3,") && !DirektVez[2])
                                        //{
                                        //    Valtoztatva[2] = true;
                                        //    if (HoMersek[AKTUALISLista.Homero] >= ((x * 2) + 20) && HoMersek[AKTUALISLista.Homero] < (((x + 1) * 2) + 20) && KitTenyezok[2] < AKTUALISLista.PWM[x])
                                        //        KitTenyezok[2] = AKTUALISLista.PWM[x];
                                        //    else if (HoMersek[AKTUALISLista.Homero] >= 110)
                                        //        KitTenyezok[2] = AKTUALISLista.PWM[AKTUALISLista.PWM.Length - 1];
                                        //    else if (HoMersek[AKTUALISLista.Homero] < 20)
                                        //        KitTenyezok[2] = AKTUALISLista.PWM[0];
                                        //}
                                        ////4. Csatorna
                                        //if (AKTUALISLista.Csatornak.Contains(",4,") && !DirektVez[3])
                                        //{
                                        //    Valtoztatva[3] = true;
                                        //    if (HoMersek[AKTUALISLista.Homero] >= ((x * 2) + 20) && HoMersek[AKTUALISLista.Homero] < (((x + 1) * 2) + 20) && KitTenyezok[3] < AKTUALISLista.PWM[x])
                                        //        KitTenyezok[3] = AKTUALISLista.PWM[x];
                                        //    else if (HoMersek[AKTUALISLista.Homero] >= 110)
                                        //        KitTenyezok[3] = AKTUALISLista.PWM[AKTUALISLista.PWM.Length - 1];
                                        //    else if (HoMersek[AKTUALISLista.Homero] < 20)
                                        //        KitTenyezok[3] = AKTUALISLista.PWM[0];
                                        //}
                                        ////5. Csatorna
                                        //if (AKTUALISLista.Csatornak.Contains(",5,") && !DirektVez[4])
                                        //{
                                        //    Valtoztatva[4] = true;
                                        //    if (HoMersek[AKTUALISLista.Homero] >= ((x * 2) + 20) && HoMersek[AKTUALISLista.Homero] < (((x + 1) * 2) + 20) && KitTenyezok[4] < AKTUALISLista.PWM[x])
                                        //        KitTenyezok[4] = AKTUALISLista.PWM[x];
                                        //    else if (HoMersek[AKTUALISLista.Homero] >= 110)
                                        //        KitTenyezok[4] = AKTUALISLista.PWM[AKTUALISLista.PWM.Length - 1];
                                        //    else if (HoMersek[AKTUALISLista.Homero] < 20)
                                        //        KitTenyezok[4] = AKTUALISLista.PWM[0];
                                        //}
                                        ////6. Csatorna
                                        //if (AKTUALISLista.Csatornak.Contains(",6,") && !DirektVez[5])
                                        //{
                                        //    Valtoztatva[5] = true;
                                        //    if (HoMersek[AKTUALISLista.Homero] >= ((x * 2) + 20) && HoMersek[AKTUALISLista.Homero] < (((x + 1) * 2) + 20) && KitTenyezok[5] < AKTUALISLista.PWM[x])
                                        //        KitTenyezok[5] = AKTUALISLista.PWM[x];
                                        //    else if (HoMersek[AKTUALISLista.Homero] >= 110)
                                        //        KitTenyezok[5] = AKTUALISLista.PWM[AKTUALISLista.PWM.Length - 1];
                                        //    else if (HoMersek[AKTUALISLista.Homero] < 20)
                                        //        KitTenyezok[5] = AKTUALISLista.PWM[0];
                                        //}
                                        ////7. Csatorna
                                        //if (AKTUALISLista.Csatornak.Contains(",7,") && !DirektVez[6])
                                        //{
                                        //    Valtoztatva[6] = true;
                                        //    if (HoMersek[AKTUALISLista.Homero] >= ((x * 2) + 20) && HoMersek[AKTUALISLista.Homero] < (((x + 1) * 2) + 20) && KitTenyezok[6] < AKTUALISLista.PWM[x])
                                        //        KitTenyezok[6] = AKTUALISLista.PWM[x];
                                        //    else if (HoMersek[AKTUALISLista.Homero] >= 110)
                                        //        KitTenyezok[6] = AKTUALISLista.PWM[AKTUALISLista.PWM.Length - 1];
                                        //    else if (HoMersek[AKTUALISLista.Homero] < 20)
                                        //        KitTenyezok[6] = AKTUALISLista.PWM[0];
                                        //}

                                        ////8. Csatorna
                                        //if (AKTUALISLista.Csatornak.Contains(",8,") && !DirektVez[7])
                                        //{
                                        //    Valtoztatva[7] = true;
                                        //    if (HoMersek[AKTUALISLista.Homero] >= ((x * 2) + 20) && HoMersek[AKTUALISLista.Homero] < (((x + 1) * 2) + 20) && KitTenyezok[7] < AKTUALISLista.PWM[x])
                                        //        KitTenyezok[7] = AKTUALISLista.PWM[x];
                                        //    else if (HoMersek[AKTUALISLista.Homero] >= 110)
                                        //        KitTenyezok[7] = AKTUALISLista.PWM[AKTUALISLista.PWM.Length - 1];
                                        //    else if (HoMersek[AKTUALISLista.Homero] < 20)
                                        //        KitTenyezok[7] = AKTUALISLista.PWM[0];
                                        //}
                                        #endregion

                                        //for (int csat = 0; csat < 8; ++csat) //Osszes Celhardveres csatorna dinamikusan kezelve
                                        //{
                                        //    if (AKTUALISLista.Csatornak.Contains("," + Convert.ToString(csat + 1) + ",") && !DirektVez[csat] && KitTenyezok[csat] < AKTUALISLista.PWM[x])
                                        //    {
                                        //        if (HoMersek[AKTUALISLista.Homero] >= ((x * 2) + 20) && HoMersek[AKTUALISLista.Homero] < (((x + 1) * 2) + 20))
                                        //        { KitTenyezok[csat] = AKTUALISLista.PWM[x]; Valtoztatva[csat] = true; if (!ValtottHomok.Contains(AKTUALISLista.Homero)) { ValtottHomok.Add(AKTUALISLista.Homero); } }
                                        //        else if (HoMersek[AKTUALISLista.Homero] >= 110)
                                        //        { KitTenyezok[csat] = AKTUALISLista.PWM[AKTUALISLista.PWM.Length - 1]; Valtoztatva[csat] = true; if (!ValtottHomok.Contains(AKTUALISLista.Homero)) { ValtottHomok.Add(AKTUALISLista.Homero); } }
                                        //        else if (HoMersek[AKTUALISLista.Homero] < 20)
                                        //        { KitTenyezok[csat] = AKTUALISLista.PWM[0]; Valtoztatva[csat] = true; if (!ValtottHomok.Contains(AKTUALISLista.Homero)) { ValtottHomok.Add(AKTUALISLista.Homero); } }

                                        //    }
                                        //}
                                        #endregion

                                        for (int csat = 0; csat < 8; ++csat) //Osszes Celhardveres csatorna dinamikusan kezelve
                                        {
                                            if (!DirektVez[csat] && AKTUALISLista.Csatornak.Contains("," + Convert.ToString(csat + 1) + ","))
                                            {
                                                HozzarendeltFszamok[csat].Add(AktualisPWM);


                                                //if (HoMersek[AKTUALISLista.Homero] >= ((x * 2) + 20) && HoMersek[AKTUALISLista.Homero] < (((x + 1) * 2) + 20))
                                                //{
                                                //    HozzarendeltFszamok[csat].Add(AKTUALISLista.PWM[x]);
                                                //    if (!ValtottHomok.Contains(AKTUALISLista.Homero))
                                                //    {
                                                //        ValtottHomok.Add(AKTUALISLista.Homero);
                                                //    }
                                                //}
                                                //else if (HoMersek[AKTUALISLista.Homero] >= 110)
                                                //{
                                                //    HozzarendeltFszamok[csat].Add(AKTUALISLista.PWM[AKTUALISLista.PWM.Length - 1]);
                                                //    if (!ValtottHomok.Contains(AKTUALISLista.Homero))
                                                //    {
                                                //        ValtottHomok.Add(AKTUALISLista.Homero);
                                                //    }
                                                //}
                                                //else if (HoMersek[AKTUALISLista.Homero] < 20)
                                                //{
                                                //    HozzarendeltFszamok[csat].Add(AKTUALISLista.PWM[0]);
                                                //    if (!ValtottHomok.Contains(AKTUALISLista.Homero))
                                                //    {
                                                //        ValtottHomok.Add(AKTUALISLista.Homero);
                                                //    }
                                                //}

                                                //else
                                                //{
                                                //if (ElozoHomok[AKTUALISLista.Homero] >= ((x * 2) + 20) && ElozoHomok[AKTUALISLista.Homero] < (((x + 1) * 2) + 20))
                                                //{
                                                //    HozzarendeltFszamok[csat].Add(AKTUALISLista.PWM[x]);
                                                //}
                                                //else if (ElozoHomok[AKTUALISLista.Homero] >= 110)
                                                //{
                                                //    HozzarendeltFszamok[csat].Add(AKTUALISLista.PWM[AKTUALISLista.PWM.Length - 1]);
                                                //}
                                                //else if (ElozoHomok[AKTUALISLista.Homero] < 20)
                                                //{
                                                //    HozzarendeltFszamok[csat].Add(AKTUALISLista.PWM[0]);
                                                //}
                                                //}
                                            }
                                        }

                                        ///////////////
                                        //sw5.Stop();
                                        //idok5[0] += sw5.ElapsedTicks;
                                        //sw5.Restart();
                                        //////////////
                                        if (Program.BelsoVentiVezTip == Program.BelsoVentiTipus.Hardware)
                                        {
                                            int talatcsatornak = 0;
                                            foreach (SensorNode snd in SndLsita)
                                            {
                                                IControl control = snd.Sensor.Control;
                                                if (AKTUALISLista.Csatornak.Contains(control.Identifier + " => " + snd.Text))
                                                {
                                                    ++talatcsatornak;
                                                    if (KitTenyekAlaplapi.ContainsKey(control))
                                                    {
                                                        if (KitTenyekAlaplapi[control] < AktualisPWM)
                                                        {
                                                            KitTenyekAlaplapi[control] = AktualisPWM;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        KitTenyekAlaplapi.Add(control, AktualisPWM);
                                                    }


                                                    //if (HoMersek[AKTUALISLista.Homero] >= ((x * 2) + 20) && HoMersek[AKTUALISLista.Homero] < (((x + 1) * 2) + 20))
                                                    //{
                                                    //    if (KitTenyekAlaplapi.ContainsKey(control))
                                                    //    {
                                                    //        if (KitTenyekAlaplapi[control] < AKTUALISLista.PWM[x])
                                                    //        {
                                                    //            KitTenyekAlaplapi[control] = AKTUALISLista.PWM[x];
                                                    //        }
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        KitTenyekAlaplapi.Add(control, AKTUALISLista.PWM[x]);
                                                    //    }
                                                    //    if (!ValtottHomok.Contains(AKTUALISLista.Homero))
                                                    //    {
                                                    //        ValtottHomok.Add(AKTUALISLista.Homero);
                                                    //    }
                                                    //}
                                                    //else if (HoMersek[AKTUALISLista.Homero] >= 110)
                                                    //{
                                                    //    if (KitTenyekAlaplapi.ContainsKey(control))
                                                    //    {
                                                    //        if (KitTenyekAlaplapi[control] < AKTUALISLista.PWM[AKTUALISLista.PWM.Length - 1])
                                                    //        {
                                                    //            KitTenyekAlaplapi[control] = AKTUALISLista.PWM[AKTUALISLista.PWM.Length - 1];
                                                    //        }
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        KitTenyekAlaplapi.Add(control, AKTUALISLista.PWM[AKTUALISLista.PWM.Length - 1]);
                                                    //    }
                                                    //    if (!ValtottHomok.Contains(AKTUALISLista.Homero))
                                                    //    {
                                                    //        ValtottHomok.Add(AKTUALISLista.Homero);
                                                    //    }
                                                    //}
                                                    //else if (HoMersek[AKTUALISLista.Homero] < 20)
                                                    //{
                                                    //    if (KitTenyekAlaplapi.ContainsKey(control))
                                                    //    {
                                                    //        if (KitTenyekAlaplapi[control] < AKTUALISLista.PWM[0])
                                                    //        {
                                                    //            KitTenyekAlaplapi[control] = AKTUALISLista.PWM[0];
                                                    //        }
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        KitTenyekAlaplapi.Add(control, AKTUALISLista.PWM[0]);
                                                    //    }
                                                    //    if (!ValtottHomok.Contains(AKTUALISLista.Homero))
                                                    //    {
                                                    //        ValtottHomok.Add(AKTUALISLista.Homero);
                                                    //    }
                                                    //}

                                                    //else
                                                    //{
                                                    //    if (ElozoHomok[AKTUALISLista.Homero] >= ((x * 2) + 20) && ElozoHomok[AKTUALISLista.Homero] < (((x + 1) * 2) + 20))
                                                    //    {
                                                    //        if (KitTenyekAlaplapi.ContainsKey(control))
                                                    //        {
                                                    //            if (KitTenyekAlaplapi[control] < AKTUALISLista.PWM[x])
                                                    //            {
                                                    //                KitTenyekAlaplapi[control] = AKTUALISLista.PWM[x];
                                                    //            }
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            KitTenyekAlaplapi.Add(control, AKTUALISLista.PWM[x]);
                                                    //        }
                                                    //    }
                                                    //    else if (ElozoHomok[AKTUALISLista.Homero] >= 110)
                                                    //    {
                                                    //        if (KitTenyekAlaplapi.ContainsKey(control))
                                                    //        {
                                                    //            if (KitTenyekAlaplapi[control] < AKTUALISLista.PWM[AKTUALISLista.PWM.Length - 1])
                                                    //            {
                                                    //                KitTenyekAlaplapi[control] = AKTUALISLista.PWM[AKTUALISLista.PWM.Length - 1];
                                                    //            }
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            KitTenyekAlaplapi.Add(control, AKTUALISLista.PWM[AKTUALISLista.PWM.Length - 1]);
                                                    //        }
                                                    //    }
                                                    //    else if (ElozoHomok[AKTUALISLista.Homero] < 20)
                                                    //    {
                                                    //        if (KitTenyekAlaplapi.ContainsKey(control))
                                                    //        {
                                                    //            if (KitTenyekAlaplapi[control] < AKTUALISLista.PWM[0])
                                                    //            {
                                                    //                KitTenyekAlaplapi[control] = AKTUALISLista.PWM[0];
                                                    //            }
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            KitTenyekAlaplapi.Add(control, AKTUALISLista.PWM[0]);
                                                    //        }
                                                    //    }
                                                    //}
                                                }
                                            }
                                            if (talatcsatornak < AKTUALISLista.GetAlaplapiControlDarabszam())
                                            {
                                                HianyzoAlaplapiControl = true;
                                            }
                                        }
                                        else if (Program.BelsoVentiVezTip == Program.BelsoVentiTipus.Emulalt)
                                        {
                                            foreach (Program.EmulaltBelsoVenti control in Program.EmulaltBelsoVentik)
                                            {
                                                if (AKTUALISLista.Csatornak.Contains(control.csoport + " => " + control.nev))
                                                {
                                                    if (KitTenyekAlaplapiEMULALT.ContainsKey(control))
                                                    {
                                                        if (KitTenyekAlaplapiEMULALT[control] < AktualisPWM)
                                                        {
                                                            KitTenyekAlaplapiEMULALT[control] = AktualisPWM;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        KitTenyekAlaplapiEMULALT.Add(control, AktualisPWM);
                                                    }



                                                    //if (hsztki)
                                                    //{
                                                    //    if (HoMersek[AKTUALISLista.Homero] >= ((x * 2) + 20) && HoMersek[AKTUALISLista.Homero] < (((x + 1) * 2) + 20))
                                                    //    {
                                                    //        if (KitTenyekAlaplapiEMULALT.ContainsKey(control))
                                                    //        {
                                                    //            if (KitTenyekAlaplapiEMULALT[control] < AKTUALISLista.PWM[x])
                                                    //            {
                                                    //                KitTenyekAlaplapiEMULALT[control] = AKTUALISLista.PWM[x];

                                                    //            }
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            KitTenyekAlaplapiEMULALT.Add(control, AKTUALISLista.PWM[x]);
                                                    //        }
                                                    //        if (!ValtottHomok.Contains(AKTUALISLista.Homero))
                                                    //        {
                                                    //            ValtottHomok.Add(AKTUALISLista.Homero);
                                                    //        }

                                                    //    }
                                                    //    else if (HoMersek[AKTUALISLista.Homero] >= 110)
                                                    //    {
                                                    //        if (KitTenyekAlaplapiEMULALT.ContainsKey(control))
                                                    //        {
                                                    //            if (KitTenyekAlaplapiEMULALT[control] < AKTUALISLista.PWM[AKTUALISLista.PWM.Length - 1])
                                                    //            {
                                                    //                KitTenyekAlaplapiEMULALT[control] = AKTUALISLista.PWM[AKTUALISLista.PWM.Length - 1];
                                                    //            }
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            KitTenyekAlaplapiEMULALT.Add(control, AKTUALISLista.PWM[AKTUALISLista.PWM.Length - 1]);
                                                    //        }
                                                    //        if (!ValtottHomok.Contains(AKTUALISLista.Homero))
                                                    //        {
                                                    //            ValtottHomok.Add(AKTUALISLista.Homero);
                                                    //        }
                                                    //    }
                                                    //    else if (HoMersek[AKTUALISLista.Homero] < 20)
                                                    //    {
                                                    //        if (KitTenyekAlaplapiEMULALT.ContainsKey(control))
                                                    //        {
                                                    //            if (KitTenyekAlaplapiEMULALT[control] < AKTUALISLista.PWM[0])
                                                    //            {
                                                    //                KitTenyekAlaplapiEMULALT[control] = AKTUALISLista.PWM[0];
                                                    //            }
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            KitTenyekAlaplapiEMULALT.Add(control, AKTUALISLista.PWM[0]);
                                                    //        }
                                                    //        if (!ValtottHomok.Contains(AKTUALISLista.Homero)) { ValtottHomok.Add(AKTUALISLista.Homero); }
                                                    //    }
                                                    //}
                                                    //else
                                                    //{
                                                    //    if (ElozoHomok[AKTUALISLista.Homero] >= ((x * 2) + 20) && ElozoHomok[AKTUALISLista.Homero] < (((x + 1) * 2) + 20))
                                                    //    {
                                                    //        if (KitTenyekAlaplapiEMULALT.ContainsKey(control)) { if (KitTenyekAlaplapiEMULALT[control] < AKTUALISLista.PWM[x]) { KitTenyekAlaplapiEMULALT[control] = AKTUALISLista.PWM[x]; } } else { KitTenyekAlaplapiEMULALT.Add(control, AKTUALISLista.PWM[x]); }
                                                    //    }
                                                    //    else if (ElozoHomok[AKTUALISLista.Homero] >= 110)
                                                    //    { if (KitTenyekAlaplapiEMULALT.ContainsKey(control)) { if (KitTenyekAlaplapiEMULALT[control] < AKTUALISLista.PWM[AKTUALISLista.PWM.Length - 1]) { KitTenyekAlaplapiEMULALT[control] = AKTUALISLista.PWM[AKTUALISLista.PWM.Length - 1]; } } else { KitTenyekAlaplapiEMULALT.Add(control, AKTUALISLista.PWM[AKTUALISLista.PWM.Length - 1]); } }
                                                    //    else if (ElozoHomok[AKTUALISLista.Homero] < 20)
                                                    //    { if (KitTenyekAlaplapiEMULALT.ContainsKey(control)) { if (KitTenyekAlaplapiEMULALT[control] < AKTUALISLista.PWM[0]) { KitTenyekAlaplapiEMULALT[control] = AKTUALISLista.PWM[0]; } } else { KitTenyekAlaplapiEMULALT.Add(control, AKTUALISLista.PWM[0]); } }
                                                    //}
                                                }
                                            }
                                        }
                                        /////////////
                                        //sw5.Stop();
                                        //idok5[1] += sw5.ElapsedTicks;
                                        //sw5.Restart();
                                        //////////////


                                    }
                                    catch { }
                                    //}
                                }
                            }
                        }

                        for (int i = 0; i < HozzarendeltFszamok.Length; i++)
                        {
                            if (DirektVez[i] == false)
                            {
                                if (HozzarendeltFszamok[i].Count != 0)
                                {
                                    byte legnagyobb = 0;
                                    foreach (var item in HozzarendeltFszamok[i])
                                    {
                                        if (item > legnagyobb)
                                            legnagyobb = item;
                                    }
                                    KitTenyezok[i] = legnagyobb;
                                }
                            }

                        }

                        //for (int i = 0; i < KitTenyezok.Length; i++)
                        //{
                        //    /*if (!Valtoztatva[i])*/ if(KitTenyezok[i] < 0)
                        //        KitTenyezok[i] = MasoltKitTenyezok[i];

                        //    //if (KitTenyezok[i] < 0)
                        //    //    ByteKitTenyezok[i] = 0;
                        //    //else
                        //        ByteKitTenyezok[i] = Convert.ToByte(KitTenyezok[i]);
                        //}

                        if (HisztDeaktivTRUEVOLT)
                            Program.HisztDeaktiv = false;

                        #endregion

                        ///////////////
                        //sw.Stop();
                        //idok[5] += sw.ElapsedTicks;
                        //sw.Restart();
                        //////////////
                        Program.ErvenyVanOlvasas = false;

                        sw5.Stop();
                        idok5 = sw5.ElapsedTicks;
                        ///////////////
                        //sw.Stop();
                        //idok[6] += sw.ElapsedTicks;
                        //sw.Restart();
                        //////////////

                        sw6.Reset();
                        sw6.Start();
                        if (Program.BelsoVentiVezTip == Program.BelsoVentiTipus.Hardware)
                        {
                            foreach (KeyValuePair<IControl, byte> item in KitTenyekAlaplapi)
                            {
                                if (item.Key.ControlMode == ControlMode.Listaalapu || item.Key.ControlMode == ControlMode.Undefined)
                                {
                                    item.Key.SetSoftware(item.Value);
                                    item.Key.SetCMListaAlapu();
                                }
                            }
                        }
                        else if (Program.BelsoVentiVezTip == Program.BelsoVentiTipus.Emulalt)
                        {
                            foreach (KeyValuePair<Program.EmulaltBelsoVenti, byte> item in KitTenyekAlaplapiEMULALT)
                            {
                                if (item.Key.ControlMode == ControlMode.Listaalapu || item.Key.ControlMode == ControlMode.Undefined)
                                {
                                    item.Key.ertek = item.Value;
                                    item.Key.ControlMode = ControlMode.Listaalapu;
                                }
                            }

                            foreach (Program.EmulaltBelsoVenti item in Program.EmulaltBelsoVentik)
                            {
                                if (item.ControlMode == ControlMode.Alapert && DevForm.checkBoxRandomSpeeds.Checked)
                                {
                                    item.ertek = (byte)Program.RandomObject.Next(0, 101);
                                }
                            }
                        }
                        sw6.Stop();
                        idok6 = sw6.ElapsedTicks;
                        ///////////////
                        //sw.Stop();
                        //idok[7] += sw.ElapsedTicks;
                        //sw.Restart();
                        //////////////

                        sw7.Reset();
                        sw7.Start();
                        if (Program.KONFVanVezerlo && !Elsokor)
                        {
                            bajtTombKuldo(KitTenyezok);
                        }

                        sw7.Stop();
                        idok7 = sw7.ElapsedTicks;

                        sw8.Reset();
                        sw8.Start();
                        #region RIASZTASOK LEJATSZASA
                        if (!Elsokor)
                        {
                            List<string> Uzenetek = new List<string>();
                            bool jatszle = false;
                            foreach (Program.Riasztas item in Program.Riasztasok)
                            {
                                try
                                {
                                    if (HoMersek.ContainsKey(item.Homero))
                                    {
                                        if (item.Muvelet == ">" && HoMersek[item.Homero] > item.RiasztPont)
                                        {
                                            Uzenetek.Add(item.Uzenet + " >> " + HoMersek[item.Homero] + "°C ");
                                            if (item.Hangjelzes)
                                                jatszle = true;
                                            SpecMuvVegrehajt(item);
                                        }
                                        else if (item.Muvelet == "=" && HoMersek[item.Homero] == item.RiasztPont)
                                        {
                                            Uzenetek.Add(item.Uzenet + " >> " + HoMersek[item.Homero] + "°C ");
                                            if (item.Hangjelzes)
                                                jatszle = true;
                                            SpecMuvVegrehajt(item);
                                        }
                                        else if (item.Muvelet == "<" && HoMersek[item.Homero] < item.RiasztPont)
                                        {
                                            Uzenetek.Add(item.Uzenet + " >> " + HoMersek[item.Homero] + "°C ");
                                            if (item.Hangjelzes)
                                                jatszle = true;
                                            SpecMuvVegrehajt(item);
                                        }
                                    }
                                }
                                catch (KeyNotFoundException) { }
                            }

                            if (Uzenetek.Count != 0)
                            {
                                string uz = "";
                                for (int i = 0; i < Uzenetek.Count - 1; ++i)
                                {
                                    uz += "- " + Uzenetek[i] + "\n";// + "||+||\n";
                                }
                                uz += Uzenetek[Uzenetek.Count - 1];

                                SysTrayicon.ShowBalloonTip(Program.KONFFrisIdo, ((Program.KONFNyelv == "hun") ? "Feverkill RIASZTÁS" : Eszk.GetNyelvSzo("TRUZSzelcsRiasztasCim")), uz + " ", ToolTipIcon.Warning);
                            }

                            if (!Program.RiasztHangMegy && jatszle)
                            {
                                Program.RiasztHang.PlayLooping();
                                Program.RiasztHangMegy = true;
                            }
                            else if (Program.RiasztHangMegy && !jatszle)
                            {
                                Program.RiasztHang.Stop();
                                Program.RiasztHangMegy = false;
                            }
                        }
                        #endregion

                        sw8.Stop();
                        idok8 = sw8.ElapsedTicks;

                        Elsokor = false;

                        ///////////////
                        //sw.Stop();
                        //idok[8] += sw.ElapsedTicks;
                        //sw.Restart();
                        //////////////
                    }
                    catch { }
                    Program.ErvenyVanOlvasas = false;
                    //}
                    //}
                    //else
                    //{
                    //    this.menuItem11.Checked = true;

                    //    if (Program.KONFVanVezerlo)
                    //    {
                    //        bajtTombKuldo(KitTenyezok);
                    //    }

                    //}

                    sw9.Reset();
                    sw9.Start();

                    if (Program.KONFKittenyMutat)
                    {
                        KittenyMutatFrissit();
                    }
                    if (Program.KONFAttekintMutat)
                    {
                        Program.Attekint.listViewFordszamok.Items.Dispatcher.Invoke(AttekintFordszamFrissit);
                    }



                    //label1.Text = "";
                    //for (int i = 0; i < idok.Length; i++)
                    //{
                    //    label1.Text += i + ": " + idok[i] + "\n";
                    //}

                    //label1.Text += "\n\n\n";
                    //for (int i = 0; i < idok5.Length; i++)
                    //{
                    //    label1.Text += i + ": " + idok5[i] + "\n";
                    //}

                    Program.Attekint.labelCelh.Dispatcher.Invoke(delegate () { Program.Attekint.labelCelh.Content = ((Program.KONFNyelv == "hun") ? ("Célhardver: ") : (Eszk.GetNyelvSzo("Celhardver") + ": ")) + ((Program.KONFVanVezerlo) ? ((NTFYI_szamlalo > NTFYMax) ? ((Program.KONFNyelv == "hun") ? ("Hiba!") : Eszk.GetNyelvSzo("Hiba!")) : ("OK")) : ((Program.KONFNyelv == "hun") ? ("Leválasztva") : Eszk.GetNyelvSzo("Levalasztva"))); });

                    sw9.Stop();
                    idok9 = sw9.ElapsedTicks;

                    sw.Stop();
                    idok = sw.ElapsedTicks;

                    if (AktIteracioSzama < Program.KONFBootIteracioSzam)
                    {
                        Thread.Sleep(Program.KONFBootFrisIdo);
                    }
                    else
                    {
                        for (int i = 0; i < Program.KONFFrisIdo / 500; i++)
                        {
                            Thread.Sleep(500);
                        }
                    }
                    ++AktIteracioSzama;
                }//DESTRUKTOR ZAROJEL
            }
            Environment.Exit(1001);
        }

        public int NTFYMax = 3;
        void bajtTombKuldo(byte[] Be)
        {
            if (!KezfogasFolyamatban)
            {
                if (!Program.SorosKuldes)
                {
                    Program.SorosKuldes = true;
                    try
                    {
                        int i;
                        for (i = 0; i < 3; ++i)
                        {
                            if (!Fajlkezelo.UARTbajtKuldo(107, Be))
                            { continue; }
                            break;
                        }
                        if (i >= 3 || Program.DEVCOMReconnect)
                        {
                            try
                            {
                                ++NTFYI_szamlalo;
                                if (NTFYI_szamlalo > 1 || Program.DEVCOMReconnect)
                                { KezfogasSzalIndito(false); }

                                Program.DEVCOMReconnect = false;
                            }
                            catch { }
                            if (Program.KONFFrisIdo >= 3000)
                            {
                                NTFYMax = 3;
                                if (NTFYI_szamlalo > NTFYMax && NTFYI_szamlalo % 2 == 0)
                                    SysTrayicon.ShowBalloonTip(3000, ((Program.KONFNyelv == "hun") ? "Hiba a kapcsolatban!" : Eszk.GetNyelvSzo("KapcsHIBACIM")), ((Program.KONFNyelv == "hun") ? "A ventilátor-fordulatszámok küldése\neddig " : Eszk.GetNyelvSzo("KapcsHIBASZOVEG1")) + NTFYI_szamlalo + ((Program.KONFNyelv == "hun") ? " alkalommal nem sikerült!\nAz értékek feltehetően\nnem megfelelőek a célhardveren." : Eszk.GetNyelvSzo("KapcsHIBASZOVEG2")), ToolTipIcon.Error);
                            }
                            else
                            {
                                NTFYMax = 5;
                                if (NTFYI_szamlalo > NTFYMax && NTFYI_szamlalo % 6 == 0)
                                    SysTrayicon.ShowBalloonTip(1000, ((Program.KONFNyelv == "hun") ? "Hiba a kapcsolatban!" : Eszk.GetNyelvSzo("KapcsHIBACIM")), ((Program.KONFNyelv == "hun") ? "A ventilátor-fordulatszámok küldése\neddig " : Eszk.GetNyelvSzo("KapcsHIBASZOVEG1")) + NTFYI_szamlalo + ((Program.KONFNyelv == "hun") ? " alkalommal nem sikerült!\nAz értékek feltehetően\nnem megfelelőek a célhardveren." : Eszk.GetNyelvSzo("KapcsHIBASZOVEG2")), ToolTipIcon.Error);
                            }
                        }
                        else
                            NTFYI_szamlalo = 0;
                    }
                    catch { }
                    Program.SorosKuldes = false;
                }
            }
        }

        static bool elsovissza = false, masodikvissza = false, rendben = true, sikeresport = false;
        public void KezfogasSzalIndito(bool menubol)
        {
            if (menubol)
                KezfogasMenubol = menubol;//Hogy  a végén mindenképpen viszsajelezze az erdményt, még ha nem is a felhasználó indította a kézfogást
            if (!KezfogasFolyamatban)
            {
                KezfogasFolyamatban = true;
                KezfogasMenubol = menubol;
                Thread kth = new Thread(KezfogasSajatszal);
                kth.Start();
            }

        }
        bool KezfogasMenubol = false;
        bool KezfogasFolyamatban = false;
        void KezfogasSajatszal()
        {
            try { Program.SorosPort.Close(); }
            catch { }
            Thread.Sleep(10);
            elsovissza = false; masodikvissza = false; rendben = true; sikeresport = false;
            #region KEZFOGAS A VEZERLOVEL
            Thread.Sleep(10);
            if (!Program.DEVCOMPortKotese)
            {
                string[] portok = SerialPort.GetPortNames();

                //Masodlagos hitelesitesi kulcs
                string MasHitKulcs = Convert.ToString((char)10) + Convert.ToString((char)20) + Convert.ToString((char)30) + Convert.ToString((char)40) + Convert.ToString((char)50) + Convert.ToString((char)60) + Convert.ToString((char)70) + Convert.ToString((char)80) + Convert.ToString((char)35) + Convert.ToString((char)36);

                Thread.Sleep(10);
                Stopwatch sw = new Stopwatch();
                for (int k = 0; k < 3 && !sikeresport; k++)
                {
                    for (int i = 0; i < portok.Length && !sikeresport; ++i)
                    {
                        try
                        {
                            Program.SorosPort = new SerialPort(".", 9600, Parity.None, 8, StopBits.One);
                            Program.SorosPort.Encoding = System.Text.Encoding.GetEncoding(28591); //8 bites karakterek

                            Program.SorosPort.DataReceived += COM_Kezfogas;

                            rendben = true;
                            sw.Reset();
                            Program.SorosPort.PortName = portok[i];
                            Program.SorosPort.WriteTimeout = 500; 
                            Program.SorosPort.Open();

                            byte[] b = new byte[] { 255 };
                            Program.SorosPort.Write(b, 0, 1);
                            Program.SorosPort.WriteLine("");
                            Thread.Sleep(50);
                            sw.Start();

                            while (sw.ElapsedMilliseconds < 300 * Math.Pow(3, k) && rendben)
                            {
                                if (elsovissza && masodikvissza)
                                {
                                    Program.SorosPort.DataReceived -= COM_Kezfogas;
                                    Thread.Sleep(500);
                                    if (Program.SorosPort.ReadExisting() == MasHitKulcs)
                                    {
                                        sikeresport = true;
                                        break;
                                    }
                                }
                                Thread.Sleep(10);
                            }
                        }
                        catch (Exception exc){ }
                        sw.Stop();
                        try
                        {
                            if (!sikeresport)
                                Program.SorosPort.Close();
                        }
                        catch { }
                        elsovissza = masodikvissza = false;
                    }
                }
                if (!sikeresport && KezfogasMenubol)
                    MessageBox.Show(((Program.KONFNyelv == "hun") ? "A Célhardver nincs megfelelően csatlakoztatva,\nvagy a COM port már foglalt!\nA program nem tud kommunikálni a vezérlővel." : Eszk.GetNyelvSzo("CélhNemTalalhSZOVEG")), ((Program.KONFNyelv == "hun") ? "Vezérlő nem található!" : Eszk.GetNyelvSzo("CélhNemTalalhCIM")), MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (sikeresport)
                    Thread.Sleep(10);

                Program.SorosPort.DataReceived -= COM_Kezfogas;
                //SorosPort.DataReceived += Fajlkezelo.COM_AdatFogad;
            }
            else
            {
                Program.SorosPort.PortName = Program.DEVKotottCOMPort;
                Program.SorosPort.Open();
            }
            KezfogasFolyamatban = false;
            Thread.Sleep(10);
            #endregion
        }
        static void COM_Kezfogas(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;

            try
            {
                if (!elsovissza)
                {
                    if (sp.ReadByte() == 125)
                        elsovissza = true;
                    else
                    {
                        elsovissza = masodikvissza = rendben = false;
                    }
                }
                else if (!masodikvissza)
                {
                    if (sp.ReadByte() == 150)
                        masodikvissza = true;
                    else
                    {
                        elsovissza = masodikvissza = rendben = false;
                    }
                }

            }
            catch (IndexOutOfRangeException) { }
            catch { }
        }
        public List<Attekinto.GVItemHomers> GVHomLista = new List<Attekinto.GVItemHomers>();
        public List<Attekinto.GVItemFordsz> GVFordszLista = new List<Attekinto.GVItemFordsz>();
        public List<Attekinto.GVItemSzabl> GVSzablLista = new List<Attekinto.GVItemSzabl>();
        public List<Attekinto.GVItemRiaszt> GVRiasztLista = new List<Attekinto.GVItemRiaszt>();
        void AttekintHomersFrissit()
        {
            //List<List<Program.HoMers>> csoportozo = new List<List<Program.HoMers>>();


            //Attekint.listViewHomers.Items.Clear();
            //bool megvan;
            int kivalasztott = Program.Attekint.listViewHomers.SelectedIndex;

            GVHomLista.Clear();
            foreach (Program.HoMers item in HommMersek)
            {
                GVHomLista.Add(new Attekinto.GVItemHomers { Csoport = "  " + item.Csop, Név = item.Nev, Érték = item.Ertek });
                //Attekint.listViewHomers.Items.Add(new Attekinto.GVItemHomers { Csoport = item.Csop, Név = item.Nev, Érték = item.Ertek });
            }

            Program.Attekint.listViewHomers.ItemsSource = GVHomLista;



            try
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Program.Attekint.listViewHomers.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("Csoport");
                view.GroupDescriptions.Clear();
                view.GroupDescriptions.Add(groupDescription);
            }
            catch { }

            try { Program.Attekint.listViewHomers.SelectedIndex = kivalasztott; } catch { }
            try { Program.Attekint.listViewHomers.ScrollIntoView(Program.Attekint.listViewHomers.SelectedIndex); } catch { }
        }
        public void AttekintFordszamFrissit()
        {
            int kivalasztott = Program.Attekint.listViewFordszamok.SelectedIndex;
            string VezTip = "";

            GVFordszLista.Clear();
            for (int i = 0; i < KitTenyezok.Length; i++)
            {
                if (DirektVez[i] == true)
                    VezTip = ((Program.KONFNyelv == "hun") ? "Manuális" : Eszk.GetNyelvSzo("Manuális"));
                else
                    VezTip = ((Program.KONFNyelv == "hun") ? "Listaalapú" : Eszk.GetNyelvSzo("Listaalapú"));

                GVFordszLista.Add(new Attekinto.GVItemFordsz { Csoport = "  " + ((Program.KONFNyelv == "hun") ? "Ventilátorok a Célhardveren" : Eszk.GetNyelvSzo("Ventilátorok a Célhardveren")), Kimenet = Convert.ToString(i + 1) + ". " + ((Program.KONFNyelv == "hun") ? "csatorna" : Eszk.GetNyelvSzo("csatorna")) + ((Program.CsatCimkekCelh[i] != "") ? "  -  " + Program.CsatCimkekCelh[i] : ""), Fordszam = KitTenyezok[i].ToString() + " %", VezTipus = VezTip, KontrolEMULALT = null, CsatIndex = i });
            }
            string fordsz;
            try
            {
                if (Program.BelsoVentiVezTip == Program.BelsoVentiTipus.Hardware)
                {
                    foreach (SensorNode snd in SndLsita)
                    {
                        try
                        {
                            IControl control = snd.Sensor.Control;
                            VezTip = "N/A";
                            try
                            {
                                switch (control.ControlMode)
                                {
                                    case ControlMode.Kezi:
                                        VezTip = ((Program.KONFNyelv == "hun") ? "Manuális" : Eszk.GetNyelvSzo("Manuális"));
                                        break;
                                    case ControlMode.Alapert:
                                        VezTip = ((Program.KONFNyelv == "hun") ? "Alapérte\nlmezett" : Eszk.GetNyelvSzo("Alapértelmezett"));
                                        break;
                                    case ControlMode.Listaalapu:
                                        VezTip = ((Program.KONFNyelv == "hun") ? "Listaalapú" : Eszk.GetNyelvSzo("Listaalapú"));
                                        break;
                                    default:
                                        break;
                                }
                            }
                            catch { }

                            try { fordsz = Convert.ToString(Math.Round(Convert.ToDecimal(snd.Value.Split(' ')[0]))) + " %"; } catch { fordsz = "----"; }

                            string KimenetNev = control.Identifier + " => " + snd.Text;
                            GVFordszLista.Add(new Attekinto.GVItemFordsz { Csoport = "  " + ((Program.KONFNyelv == "hun") ? "Belső Ventilátorok" : Eszk.GetNyelvSzo("Belső Ventilátorok")), Kimenet = KimenetNev + (Program.CsatCimkekBelso.ContainsKey(KimenetNev) ? ((Program.CsatCimkekBelso[KimenetNev] != "") ? "\n     -> " + Program.CsatCimkekBelso[KimenetNev] : "") : ""), Fordszam = fordsz, VezTipus = VezTip, Kontrol = control, KontrolEMULALT = null, CsatIndex = -99 });//.Replace(" ", "")
                        }
                        catch { }

                    }
                }
                else if (Program.BelsoVentiVezTip == Program.BelsoVentiTipus.Emulalt)
                {
                    foreach (Program.EmulaltBelsoVenti control in Program.EmulaltBelsoVentik)
                    {
                        try
                        {
                            VezTip = "N/A";
                            try
                            {
                                switch (control.ControlMode)
                                {
                                    case ControlMode.Kezi:
                                        VezTip = ((Program.KONFNyelv == "hun") ? "Manuális" : Eszk.GetNyelvSzo("Manuális"));
                                        break;
                                    case ControlMode.Alapert:
                                        VezTip = ((Program.KONFNyelv == "hun") ? "Alapértelmezett" : Eszk.GetNyelvSzo("Alapértelmezett"));
                                        break;
                                    case ControlMode.Listaalapu:
                                        VezTip = ((Program.KONFNyelv == "hun") ? "Listaalapú" : Eszk.GetNyelvSzo("Listaalapú"));
                                        break;
                                    default:
                                        break;
                                }
                            }
                            catch { }

                            try { fordsz = control.ertek + " %"; } catch { fordsz = "----"; }

                            GVFordszLista.Add(new Attekinto.GVItemFordsz { Csoport = "  " + ((Program.KONFNyelv == "hun") ? "Belső Ventilátorok" : Eszk.GetNyelvSzo("Belső Ventilátorok")), Kimenet = control.csoport + " => " + control.nev, Fordszam = fordsz, VezTipus = VezTip, Kontrol = null, KontrolEMULALT = control, CsatIndex = -99 });//.Replace(" ", "")
                        }
                        catch { }

                    }
                }
            }
            catch { }

            Program.Attekint.listViewFordszamok.ItemsSource = GVFordszLista;

            try
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Program.Attekint.listViewFordszamok.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("Csoport");
                view.GroupDescriptions.Clear();
                view.GroupDescriptions.Add(groupDescription);
            }
            catch { }

            try { Program.Attekint.listViewFordszamok.SelectedIndex = kivalasztott; } catch { }
            try { Program.Attekint.listViewFordszamok.ScrollIntoView(Program.Attekint.listViewFordszamok.SelectedIndex); } catch { }
        }
        public void AttekintSzablistFrissit()
        {
            int kivalasztott = Program.Attekint.listViewSzablistak.SelectedIndex;
            try
            {
                List<string> ErvNevek = new List<string>();
                GVSzablLista.Clear();
                foreach (Program.SzabLista item in Program.Ervenyesek)
                {
                    GVSzablLista.Add(new Attekinto.GVItemSzabl { Csoport = "  " + ((Program.KONFNyelv == "hun") ? "Aktív Szabályzólisták" : Eszk.GetNyelvSzo("Aktív Szabályzólisták")), Nev = item.Nev, Hoszenzor = item.Homero, Csatornak = Fajlkezelo.CsatbolString(item.Csatornak) });
                    ErvNevek.Add(item.Nev);
                }

                foreach (Program.SzabLista item in Program.SzabListak)
                    if (ErvNevek.Contains(item.Nev) == false)
                        GVSzablLista.Add(new Attekinto.GVItemSzabl { Csoport = "  " + ((Program.KONFNyelv == "hun") ? "Inaktív Szabályzólisták" : Eszk.GetNyelvSzo("Inaktív Szabályzólisták")), Nev = item.Nev, Hoszenzor = item.Homero, Csatornak = Fajlkezelo.CsatbolString(item.Csatornak) });

            }
            catch { }

            if (Program.Ervenyesek.Count == 0 && Program.SzabListak.Count == 0)
            {
                GVSzablLista.Add(new Attekinto.GVItemSzabl { Csoport = "  " + ((Program.KONFNyelv == "hun") ? "Hozzon létre új vezérlési sémákat!" : Eszk.GetNyelvSzo("Hozzon létre új vezérlési sémákat!")) });
                GVSzablLista.Add(new Attekinto.GVItemSzabl { Csoport = "  " + ((Program.KONFNyelv == "hun") ? "Ehhez kattintson erre a villanykörtére, jobbra fent!" : Eszk.GetNyelvSzo("Ehhez kattintson erre a villanykörtére, jobbra fent!")) });
            }
            try
            {
                Program.Attekint.listViewSzablistak.ItemsSource = GVSzablLista;

                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Program.Attekint.listViewSzablistak.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("Csoport");
                view.GroupDescriptions.Clear();
                view.GroupDescriptions.Add(groupDescription);
            }
            catch { }

            try { Program.Attekint.listViewSzablistak.SelectedIndex = kivalasztott; } catch { }
            try { Program.Attekint.listViewSzablistak.ScrollIntoView(Program.Attekint.listViewSzablistak.SelectedIndex); } catch { }
        }
        public void AttekintRiasztFrissit()
        {
            int kivalasztott = Program.Attekint.listViewRiaszt.SelectedIndex;
            try
            {
                GVRiasztLista.Clear();
                string SpecMuvSzoveg;
                foreach (Program.Riasztas item in Program.Riasztasok)
                {
                    switch (item.SpecMuvelet)
                    {
                        case "a":
                            SpecMuvSzoveg = (Program.KONFNyelv == "hun") ? "Alvó állapot" : Eszk.GetNyelvSzo("Alvás");
                            break;
                        case "h":
                            SpecMuvSzoveg = (Program.KONFNyelv == "hun") ? "Hibernálás" : Eszk.GetNyelvSzo("Hibernálás");
                            break;
                        case "l":
                            SpecMuvSzoveg = (Program.KONFNyelv == "hun") ? "Leállítás" : Eszk.GetNyelvSzo("Leállítás");
                            break;
                        case "u":
                            SpecMuvSzoveg = (Program.KONFNyelv == "hun") ? "Újraindítás" : Eszk.GetNyelvSzo("Újraindítás");
                            break;
                        default:
                            SpecMuvSzoveg = (Program.KONFNyelv == "hun") ? "Nincs művelet" : Eszk.GetNyelvSzo("Semmi");
                            break;
                    }
                    GVRiasztLista.Add(new Attekinto.GVItemRiaszt { Hangjelzes = (item.Hangjelzes) ? ((Program.KONFNyelv == "hun") ? "IGEN" : Eszk.GetNyelvSzo("IGEN")) : ((Program.KONFNyelv == "hun") ? "NEM" : Eszk.GetNyelvSzo("NEM")), Homero = item.Homero, Muvelet = item.Muvelet, RiasztPont = item.RiasztPont.ToString(), SpecMuv = SpecMuvSzoveg, Uzenet = item.Uzenet, EbresztIdo = (item.EbresztIdo > 0) ? item.EbresztIdo + ((Program.KONFNyelv == "hun") ? "p" : Eszk.GetNyelvSzo("p")) : "" });
                }

                if (Program.Riasztasok.Count == 0)
                {
                    GVRiasztLista.Add(new Attekinto.GVItemRiaszt { Csoport = "  " + ((Program.KONFNyelv == "hun") ? "Hozzon létre új riasztásokat!" : Eszk.GetNyelvSzo("Hozzon létre új riasztásokat!")) });
                    GVRiasztLista.Add(new Attekinto.GVItemRiaszt { Csoport = "  " + ((Program.KONFNyelv == "hun") ? "Ehhez kattintson erre a villanykörtére, jobbra fent!" : Eszk.GetNyelvSzo("Ehhez kattintson erre a villanykörtére, jobbra fent!")) });

                }
                try
                {
                    Program.Attekint.listViewRiaszt.ItemsSource = GVRiasztLista;
                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Program.Attekint.listViewRiaszt.ItemsSource);
                    view.GroupDescriptions.Clear();

                    if (Program.Riasztasok.Count == 0)
                    {
                        PropertyGroupDescription groupDescription = new PropertyGroupDescription("Csoport");
                        view.GroupDescriptions.Add(groupDescription);
                    }
                }
                catch { }

            }
            catch { }


            try { Program.Attekint.listViewRiaszt.SelectedIndex = kivalasztott; } catch { }
            try { Program.Attekint.listViewRiaszt.ScrollIntoView(Program.Attekint.listViewRiaszt.SelectedIndex); } catch { }
        }
        void SpecMuvVegrehajt(Program.Riasztas riasztas)
        {
            if (riasztas.EbresztIdo > 0)
            {
                ProcessStartInfo startInf = new ProcessStartInfo();
                startInf.FileName = "ebreszt.exe";
                startInf.Arguments = riasztas.EbresztIdo.ToString();
                startInf.WindowStyle = ProcessWindowStyle.Hidden;

                Process.Start(startInf);
                Thread.Sleep(1500);
            }

            switch (riasztas.SpecMuvelet)
            {
                case "a":
                    Application.SetSuspendState(PowerState.Suspend, true, false);
                    break;
                case "h":
                    /*Process.Start("shutdown", "/h");*/
                    Application.SetSuspendState(PowerState.Hibernate, true, false);
                    break;
                case "l":
                    Process.Start("shutdown", "/p");
                    break;
                case "u":
                    Process.Start("shutdown", "/r /t 0");
                    break;
                default:
                    break;
            }
        }

        void HomersMutatFrissit()
        {
            try
            {
                bool megvan = false;

                int kivalasztott = 0;
                if (HoMeRok.listView1.SelectedIndices.Count != 0) kivalasztott = HoMeRok.listView1.SelectedIndices[0];
                HoMeRok.listView1.Visible = false;
                foreach (Program.HoMers item in HommMersek)
                {
                    for (int i = 0; i < HoMeRok.listView1.Items.Count; ++i)
                    {
                        if (HoMeRok.listView1.Items[i].Group.Header == item.Csop && HoMeRok.listView1.Items[i].Text == item.Nev)
                        {
                            System.Windows.Forms.ListViewItem listViewItemx = new System.Windows.Forms.ListViewItem(new string[] {
                                                                                                                    item.Nev,
                                                                                                                    item.Ertek}, -1);
                            listViewItemx.Group = HoMeRok.listView1.Groups[item.Csop];
                            HoMeRok.listView1.Items[i] = listViewItemx;

                            megvan = true;
                            break;
                        }
                    }

                    if (!megvan)
                    {
                        System.Windows.Forms.ListViewGroup listViewGroupx = new System.Windows.Forms.ListViewGroup(item.Csop, System.Windows.Forms.HorizontalAlignment.Left);
                        listViewGroupx.Name = item.Csop;
                        HoMeRok.listView1.Groups.Add(listViewGroupx);

                        System.Windows.Forms.ListViewItem listViewItemy = new System.Windows.Forms.ListViewItem(new string[] {
                                                                                                                        item.Nev,
                                                                                                                        item.Ertek}, -1);

                        listViewItemy.Group = HoMeRok.listView1.Groups[item.Csop];
                        HoMeRok.listView1.Items.Add(listViewItemy);
                    }
                }
                HoMeRok.listView1.Items[kivalasztott].Selected = true;
                HoMeRok.listView1.Visible = true;
            }
            catch { }

        }
        void BetekintoFrissit()
        {
            try
            {
                while (betekintesToolStripMenuItem.DropDownItems.Count < HommMersek.Count)
                {
                    betekintesToolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem());
                }

                while ((betekintesToolStripMenuItem.DropDownItems.Count > HommMersek.Count))
                {
                    betekintesToolStripMenuItem.DropDownItems.RemoveAt(0);
                }

                int i = 0;
                foreach (Program.HoMers item in HommMersek)
                {
                    betekintesToolStripMenuItem.DropDownItems[i].Enabled = false;
                    betekintesToolStripMenuItem.DropDownItems[i].Text = item.Csop + " => " + item.Nev + " >>> " + item.Ertek;
                    ++i;
                }
            }
            catch { }

        }
        void KittenyMutatFrissit()
        {
            try
            {
                KMutato.label3.Text = KitTenyezok[0].ToString() + "%";
                KMutato.label4.Text = KitTenyezok[1].ToString() + "%";
                KMutato.label5.Text = KitTenyezok[2].ToString() + "%";
                KMutato.label6.Text = KitTenyezok[3].ToString() + "%";
                KMutato.label7.Text = KitTenyezok[4].ToString() + "%";
                KMutato.label8.Text = KitTenyezok[5].ToString() + "%";
                KMutato.label9.Text = KitTenyezok[6].ToString() + "%";
                KMutato.label10.Text = KitTenyezok[7].ToString() + "%";
            }
            catch { }
        }
        private void InitializePlotForm()
        {
            try
            {
                plotForm.Dispose();
            }
            catch
            { }

            plotForm = new Form();
            plotForm.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            plotForm.ShowInTaskbar = false;
            plotForm.StartPosition = FormStartPosition.Manual;
            this.AddOwnedForm(plotForm);
            plotForm.Bounds = new Rectangle
            {
                X = settings.GetValue("plotForm.Location.X", -100000),
                Y = settings.GetValue("plotForm.Location.Y", 100),
                Width = settings.GetValue("plotForm.Width", 600),
                Height = settings.GetValue("plotForm.Height", 400)
            };

            showPlot = new UserOption("plotMenuItem", false, plotMenuItem, settings);
            plotLocation = new UserRadioGroup("plotLocation", 0,
              new[] { plotWindowMenuItem, plotBottomMenuItem, plotRightMenuItem },
              settings);

            showPlot.Changed += delegate (object sender, EventArgs e)
            {
                try
                {
                    if (plotLocation.Value == 0)
                    {
                        if (showPlot.Value && this.Visible)
                        { plotForm.Show(); }
                        else
                        { plotForm.Hide(); }
                    }
                    else
                    {
                        splitContainer.Panel2Collapsed = !showPlot.Value;
                    }
                    treeView.Invalidate();
                }
                catch { }
            };
            plotLocation.Changed += delegate (object sender, EventArgs e)
            {
                switch (plotLocation.Value)
                {
                    case 0:
                        splitContainer.Panel2.Controls.Clear();
                        splitContainer.Panel2Collapsed = true;
                        plotForm.Controls.Add(plotPanel);
                        if (showPlot.Value && this.Visible)
                            plotForm.Show();
                        break;
                    case 1:
                        plotForm.Controls.Clear();
                        plotForm.Hide();
                        splitContainer.Orientation = Orientation.Horizontal;
                        splitContainer.Panel2.Controls.Add(plotPanel);
                        splitContainer.Panel2Collapsed = !showPlot.Value;
                        break;
                    case 2:
                        plotForm.Controls.Clear();
                        plotForm.Hide();
                        splitContainer.Orientation = Orientation.Vertical;
                        splitContainer.Panel2.Controls.Add(plotPanel);
                        splitContainer.Panel2Collapsed = !showPlot.Value;
                        break;
                }
            };

            plotForm.FormClosing += delegate (object sender, FormClosingEventArgs e)
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    // just switch off the plotting when the user closes the form
                    if (plotLocation.Value == 0)
                    {
                        showPlot.Value = false;
                    }
                    e.Cancel = true;
                }
            };

            EventHandler moveOrResizePlotForm = delegate (object sender, EventArgs e)
            {
                if (plotForm.WindowState != FormWindowState.Minimized)
                {
                    settings.SetValue("plotForm.Location.X", plotForm.Bounds.X);
                    settings.SetValue("plotForm.Location.Y", plotForm.Bounds.Y);
                    settings.SetValue("plotForm.Width", plotForm.Bounds.Width);
                    settings.SetValue("plotForm.Height", plotForm.Bounds.Height);
                }
            };
            plotForm.Move += moveOrResizePlotForm;
            plotForm.Resize += moveOrResizePlotForm;

            plotForm.VisibleChanged += delegate (object sender, EventArgs e)
            {
                Rectangle bounds = new Rectangle(plotForm.Location, plotForm.Size);
                Screen screen = Screen.FromRectangle(bounds);
                Rectangle intersection =
                  Rectangle.Intersect(screen.WorkingArea, bounds);
                if (intersection.Width < Math.Min(16, bounds.Width) ||
                    intersection.Height < Math.Min(16, bounds.Height))
                {
                    plotForm.Location = new Point(
                      screen.WorkingArea.Width / 2 - bounds.Width / 2,
                      screen.WorkingArea.Height / 2 - bounds.Height / 2);
                }
            };

            this.VisibleChanged += delegate (object sender, EventArgs e)
            {
                if (this.Visible && showPlot.Value && plotLocation.Value == 0)
                    plotForm.Show();
                else
                    plotForm.Hide();
            };
        }

        private void InsertSorted(Collection<Node> nodes, HardwareNode node)
        {
            int i = 0;
            while (i < nodes.Count && nodes[i] is HardwareNode &&
              ((HardwareNode)nodes[i]).Hardware.HardwareType <
                node.Hardware.HardwareType)
                i++;
            nodes.Insert(i, node);
        }

        private void SubHardwareAdded(IHardware hardware, Node node)
        {
            HardwareNode hardwareNode =
              new HardwareNode(hardware, settings, unitManager);
            hardwareNode.PlotSelectionChanged += PlotSelectionChanged;

            InsertSorted(node.Nodes, hardwareNode);

            foreach (IHardware subHardware in hardware.SubHardware)
                SubHardwareAdded(subHardware, hardwareNode);
        }

        private void HardwareAdded(IHardware hardware)
        {
            SubHardwareAdded(hardware, root);
            PlotSelectionChanged(this, null);
        }

        private void HardwareRemoved(IHardware hardware)
        {
            List<HardwareNode> nodesToRemove = new List<HardwareNode>();
            foreach (Node node in root.Nodes)
            {
                HardwareNode hardwareNode = node as HardwareNode;
                if (hardwareNode != null && hardwareNode.Hardware == hardware)
                    nodesToRemove.Add(hardwareNode);
            }
            foreach (HardwareNode hardwareNode in nodesToRemove)
            {
                root.Nodes.Remove(hardwareNode);
                hardwareNode.PlotSelectionChanged -= PlotSelectionChanged;
            }
            PlotSelectionChanged(this, null);
        }

        private void nodeTextBoxText_DrawText(object sender, DrawEventArgs e)
        {
            Node node = e.Node.Tag as Node;
            if (node != null)
            {
                Color color;
                if (node.IsVisible)
                {
                    SensorNode sensorNode = node as SensorNode;
                    if (plotMenuItem.Checked && sensorNode != null &&
                      sensorPlotColors.TryGetValue(sensorNode.Sensor, out color))
                        e.TextColor = color;
                }
                else
                {
                    e.TextColor = Color.DarkGray;
                }
            }
        }

        private void PlotSelectionChanged(object sender, EventArgs e)
        {
            List<ISensor> selected = new List<ISensor>();
            IDictionary<ISensor, Color> colors = new Dictionary<ISensor, Color>();
            int colorIndex = 0;
            foreach (TreeNodeAdv node in treeView.AllNodes)
            {
                SensorNode sensorNode = node.Tag as SensorNode;
                if (sensorNode != null)
                {
                    if (sensorNode.Plot)
                    {
                        if (!sensorNode.PenColor.HasValue)
                        {
                            colors.Add(sensorNode.Sensor,
                              plotColorPalette[colorIndex % plotColorPalette.Length]);
                        }
                        selected.Add(sensorNode.Sensor);
                    }
                    colorIndex++;
                }
            }

            // if a sensor is assigned a color that's already being used by another 
            // sensor, try to assign it a new color. This is done only after the 
            // previous loop sets an unchanging default color for all sensors, so that 
            // colors jump around as little as possible as sensors get added/removed 
            // from the plot
            var usedColors = new List<Color>();
            foreach (var curSelectedSensor in selected)
            {
                if (!colors.ContainsKey(curSelectedSensor)) continue;
                var curColor = colors[curSelectedSensor];
                if (usedColors.Contains(curColor))
                {
                    foreach (var potentialNewColor in plotColorPalette)
                    {
                        if (!colors.Values.Contains(potentialNewColor))
                        {
                            colors[curSelectedSensor] = potentialNewColor;
                            usedColors.Add(potentialNewColor);
                            break;
                        }
                    }
                }
                else
                {
                    usedColors.Add(curColor);
                }
            }

            foreach (TreeNodeAdv node in treeView.AllNodes)
            {
                SensorNode sensorNode = node.Tag as SensorNode;
                if (sensorNode != null && sensorNode.Plot && sensorNode.PenColor.HasValue)
                    colors.Add(sensorNode.Sensor, sensorNode.PenColor.Value);
            }

            sensorPlotColors = colors;
            plotPanel.SetSensors(selected, colors);
        }

        private void nodeTextBoxText_EditorShowing(object sender,
          CancelEventArgs e)
        {
            e.Cancel = !(treeView.CurrentNode != null &&
              (treeView.CurrentNode.Tag is SensorNode ||
               treeView.CurrentNode.Tag is HardwareNode));
        }

        private void nodeCheckBox_IsVisibleValueNeeded(object sender,
          NodeControlValueEventArgs e)
        {
            SensorNode node = e.Node.Tag as SensorNode;
            e.Value = (node != null) && plotMenuItem.Checked;
        }

        private void exitClick(object sender, EventArgs e)
        {
            Close();
        }

        private int delayCount = 0;
        private void timer_Tick(object sender, EventArgs e)
        {
            computer.Accept(updateVisitor);
            treeView.Invalidate();
            plotPanel.InvalidatePlot();
            //systemTray.Redraw();
            if (gadget != null)
                gadget.Redraw();

            //if (wmiProvider != null)
            //    wmiProvider.Update();


            if (logSensors != null && logSensors.Value && delayCount >= 4)
                logger.Log();

            if (delayCount < 4)
                delayCount++;
        }

        private void SaveConfiguration()
        {
            plotPanel.SetCurrentSettings();
            foreach (TreeColumn column in treeView.Columns)
                settings.SetValue("treeView.Columns." + column.Header + ".Width",
                  column.Width);

            this.settings.SetValue("listenerPort", server.ListenerPort);

            string fileName = Path.ChangeExtension(
                System.Windows.Forms.Application.ExecutablePath, ".config");
            try
            {
                settings.Save(fileName);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Access to the path '" + fileName + "' is denied. " +
                  "The current settings could not be saved.",
                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException)
            {
                MessageBox.Show("The path '" + fileName + "' is not writeable. " +
                  "The current settings could not be saved.",
                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Rectangle newBounds = new Rectangle
            //{
            //    X = settings.GetValue("mainForm.Location.X", Location.X),
            //    Y = settings.GetValue("mainForm.Location.Y", Location.Y),
            //    Width = settings.GetValue("mainForm.Width", 470),
            //    Height = settings.GetValue("mainForm.Height", 640)
            //};

            //Rectangle fullWorkingArea = new Rectangle(int.MaxValue, int.MaxValue,
            //  int.MinValue, int.MinValue);

            //foreach (Screen screen in Screen.AllScreens)
            //    fullWorkingArea = Rectangle.Union(fullWorkingArea, screen.Bounds);

            //Rectangle intersection = Rectangle.Intersect(fullWorkingArea, newBounds);
            //if (intersection.Width < 20 || intersection.Height < 20 ||
            //  !settings.Contains("mainForm.Location.X")
            //)
            //{
            //    newBounds.X = (Screen.PrimaryScreen.WorkingArea.Width / 2) -
            //                  (newBounds.Width / 2);

            //    newBounds.Y = (Screen.PrimaryScreen.WorkingArea.Height / 2) -
            //                  (newBounds.Height / 2);
            //}

            //this.Bounds = newBounds;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Visible = false;
            // systemTray.IsMainIconEnabled = false;
            //timer.Enabled = false;
            computer.Close();
            SaveConfiguration();
            if (runWebServer.Value)
                server.Quit();
            //  systemTray.Dispose();
        }

        private void treeView_Click(object sender, EventArgs e)
        {
            MouseEventArgs m = e as MouseEventArgs;
            if (m == null || m.Button != MouseButtons.Right)
                return;
            //NodeControlInfo info = treeView.GetNodeControlInfoAt(
            //  new Point(m.X, m.Y)
            //);
            NodeControlInfo info = treeView.GetNodeControlInfoAt(m.Location);

            treeView.SelectedNode = info.Node;
            if (info.Node != null)
            {
                SensorNode node = info.Node.Tag as SensorNode;
                if (node != null && node.Sensor != null)
                {
                    treeContextMenu.MenuItems.Clear();
                    //if (node.Sensor.Parameters.Length > 0)
                    //{
                    //    MenuItem item = new MenuItem("Parameters...");
                    //    item.Click += delegate(object obj, EventArgs args)
                    //    {
                    //        ShowParameterForm(node.Sensor);
                    //    };
                    //    treeContextMenu.MenuItems.Add(item);
                    //}
                    //if (nodeTextBoxText.EditEnabled)
                    //{
                    //    MenuItem item = new MenuItem("Rename");
                    //    item.Click += delegate(object obj, EventArgs args)
                    //    {
                    //        nodeTextBoxText.BeginEdit();
                    //    };
                    //    treeContextMenu.MenuItems.Add(item);
                    //}
                    if (node.IsVisible)
                    {
                        MenuItem item = new MenuItem(((Program.KONFNyelv == "hun") ? "Elrejtés" : Eszk.GetNyelvSzo("Elrejtés")));
                        item.Click += delegate (object obj, EventArgs args)
                        {
                            node.IsVisible = false;
                        };
                        treeContextMenu.MenuItems.Add(item);
                    }
                    else
                    {
                        MenuItem item = new MenuItem(((Program.KONFNyelv == "hun") ? "Mutatás" : Eszk.GetNyelvSzo("Mutatás")));
                        item.Click += delegate (object obj, EventArgs args)
                        {
                            node.IsVisible = true;
                        };
                        treeContextMenu.MenuItems.Add(item);
                    }
                    //treeContextMenu.MenuItems.Add(new MenuItem("-"));
                    //{
                    //    MenuItem item = new MenuItem("Show in Tray");
                    //    //item.Checked = systemTray.Contains(node.Sensor);
                    //    item.Click += delegate(object obj, EventArgs args)
                    //    {
                    //        //if (item.Checked)
                    //        //    systemTray.Remove(node.Sensor);
                    //        //else
                    //        //    systemTray.Add(node.Sensor, true);
                    //    };
                    //    treeContextMenu.MenuItems.Add(item);
                    //}
                    //if (gadget != null)
                    //{
                    //    MenuItem item = new MenuItem("Show in Gadget");
                    //    item.Checked = gadget.Contains(node.Sensor);
                    //    item.Click += delegate(object obj, EventArgs args)
                    //    {
                    //        if (item.Checked)
                    //        {
                    //            gadget.Remove(node.Sensor);
                    //        }
                    //        else
                    //        {
                    //            gadget.Add(node.Sensor);
                    //        }
                    //    };
                    //    treeContextMenu.MenuItems.Add(item);
                    //}


                    //if (node.Sensor.Control != null)
                    //{
                    //    treeContextMenu.MenuItems.Add(new MenuItem("-"));
                    //    IControl control = node.Sensor.Control;
                    //    MenuItem controlItem = new MenuItem("Vezérlés");
                    //    MenuItem defaultItem = new MenuItem("Alapértelmezett");
                    //    MenuItem listaalapItem = new MenuItem("Listaalapú");
                    //    defaultItem.Checked = control.ControlMode;// == ControlMode.Alapert;
                    //    controlItem.MenuItems.Add(defaultItem);
                    //    defaultItem.Click += delegate (object obj, EventArgs args)
                    //    {
                    //        control.SetDefault();
                    //        control.ControlMode = ControlMode.Alapert;
                    //    };
                    //    MenuItem manualItem = new MenuItem("Kézi");
                    //    controlItem.MenuItems.Add(manualItem);
                    //    manualItem.Checked = control.ControlMode;// == ControlMode.Kezi;
                    //    for (int i = 0; i <= 100; i += 5)
                    //    {
                    //        if (i <= control.MaxSoftwareValue &&
                    //            i >= control.MinSoftwareValue)
                    //        {
                    //            MenuItem item = new MenuItem(i + " %");
                    //            item.RadioCheck = true;
                    //            manualItem.MenuItems.Add(item);
                    //            item.Checked = control.ControlMode == ControlMode.Kezi &&
                    //              Math.Round(control.SoftwareValue) == i;
                    //            int softwareValue = i;
                    //            item.Click += delegate (object obj, EventArgs args)
                    //            {
                    //                control.SetSoftware(softwareValue);
                    //            };
                    //        }
                    //    }
                    //    treeContextMenu.MenuItems.Add(controlItem);
                    //}
                    if (node.Sensor.Control != null)
                    {
                        treeContextMenu.MenuItems.Add(new MenuItem("-"));
                        IControl control = node.Sensor.Control;
                        MenuItem controlItem = new MenuItem(((Program.KONFNyelv == "hun") ? "Vezérlés" : Eszk.GetNyelvSzo("Vezérlés")));
                        MenuItem defaultItem = new MenuItem(((Program.KONFNyelv == "hun") ? "Alapértelmezett" : Eszk.GetNyelvSzo("Alapértelmezett")));
                        MenuItem listaalapItem = new MenuItem(((Program.KONFNyelv == "hun") ? "Listaalapú" : Eszk.GetNyelvSzo("Listaalapú")));

                        //if (control.ControlMode == ControlMode.Undefined)
                        //    control.SetCMListaAlapu();

                        controlItem.MenuItems.Add(listaalapItem);
                        controlItem.MenuItems.Add(defaultItem);

                        defaultItem.Click += delegate (object obj, EventArgs args)
                        {
                            control.SetDefault();
                        };
                        listaalapItem.Click += delegate (object obj, EventArgs args)
                        {
                            control.SetCMListaAlapu();
                            Program.HisztDeaktiv = true;

                        };

                        MenuItem manualItem = new MenuItem(((Program.KONFNyelv == "hun") ? "Manuális" : Eszk.GetNyelvSzo("Manuális")));
                        controlItem.MenuItems.Add(manualItem);
                        for (int i = 0; i <= 100; i += 5)
                        {
                            if (i <= control.MaxSoftwareValue &&
                                i >= control.MinSoftwareValue)
                            {
                                MenuItem item = new MenuItem(i + " %");
                                item.RadioCheck = true;
                                manualItem.MenuItems.Add(item);

                                item.Checked = control.ControlMode == ControlMode.Kezi &&
                                  Math.Round(control.SoftwareValue) == i;

                                int softwareValue = i;
                                item.Click += delegate (object obj, EventArgs args)
                                {
                                    control.SetSoftware(softwareValue);
                                };
                            }
                        }

                        if (control.ControlMode == ControlMode.Alapert)
                            defaultItem.Checked = true;
                        else if (control.ControlMode == ControlMode.Listaalapu)
                            listaalapItem.Checked = true;

                        treeContextMenu.MenuItems.Add(controlItem);
                    }

                    treeContextMenu.MenuItems.Add(new MenuItem("-"));
                    {
                        MenuItem item = new MenuItem(((Program.KONFNyelv == "hun") ? "Vonalszín..." : Eszk.GetNyelvSzo("Vonalszín...")));
                        item.Click += delegate (object obj, EventArgs args)
                        {
                            ColorDialog dialog = new ColorDialog();
                            dialog.Color = node.PenColor.GetValueOrDefault();
                            if (dialog.ShowDialog() == DialogResult.OK)
                                node.PenColor = dialog.Color;
                        };
                        treeContextMenu.MenuItems.Add(item);
                    }
                    {
                        MenuItem item = new MenuItem(((Program.KONFNyelv == "hun") ? "Szín Visszaállítása" : Eszk.GetNyelvSzo("Szín Visszaállítása")));
                        item.Click += delegate (object obj, EventArgs args)
                        {
                            node.PenColor = null;
                        };
                        treeContextMenu.MenuItems.Add(item);
                    }

                    treeContextMenu.Show(treeView, new Point(m.X, m.Y));
                }

                //HardwareNode hardwareNode = info.Node.Tag as HardwareNode;
                //if (hardwareNode != null && hardwareNode.Hardware != null)
                //{
                //    treeContextMenu.MenuItems.Clear();

                //    //if (nodeTextBoxText.EditEnabled)
                //    //{
                //    //    MenuItem item = new MenuItem("Rename");
                //    //    item.Click += delegate(object obj, EventArgs args)
                //    //    {
                //    //        nodeTextBoxText.BeginEdit();
                //    //    };
                //    //    treeContextMenu.MenuItems.Add(item);
                //    //}

                //    treeContextMenu.Show(treeView, new Point(m.X, m.Y));
                //}
            }
        }

        private void saveReportMenuItem_Click(object sender, EventArgs e)
        {
            string report = computer.GetReport();

            this.saveFileDialog.FileName = ((Program.KONFNyelv == "hun") ? "Feverkill_Hardverkonfiguracio-jelentes_" : Eszk.GetNyelvSzo("Feverkill_Hardverkonfiguracio-jelentes_")) + "Y" + DateTime.Now.Year + "-M" + DateTime.Now.Month + "-D" + DateTime.Now.Day + "-h" + DateTime.Now.Hour + ".m" + DateTime.Now.Minute + ".s" + DateTime.Now.Second + ".txt";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (TextWriter w = new StreamWriter(saveFileDialog.FileName))
                {
                    w.Write(report);
                }
            }
        }

        private void SysTrayHideShow()
        {
            try
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    WindowState = FormWindowState.Normal;
                    Visible = true;
                    Activate();
                    return;
                }
            }
            catch
            { }

            try
            {
                if (!Visible)
                    WindowState = FormWindowState.Normal;
            }
            catch
            { }

            Visible = !Visible;
            if (Visible)
                Activate();
        }

        //protected override void WndProc(ref Message m)
        //{
        //    const int WM_SYSCOMMAND = 0x112;
        //    const int SC_MINIMIZE = 0xF020;
        //    const int SC_CLOSE = 0xF060;

        //    if (minimizeToTray.Value &&
        //      m.Msg == WM_SYSCOMMAND && m.WParam.ToInt64() == SC_MINIMIZE)
        //    {
        //        SysTrayHideShow();
        //    }
        //    //else if (minimizeOnClose.Value &&
        //    //m.Msg == WM_SYSCOMMAND && m.WParam.ToInt64() == SC_CLOSE)
        //    //{
        //    //    /*
        //    //     * Apparently the user wants to minimize rather than close
        //    //     * Now we still need to check if we're going to the tray or not
        //    //     * 
        //    //     * Note: the correct way to do this would be to send out SC_MINIMIZE,
        //    //     * but since the code here is so simple,
        //    //     * that would just be a waste of time.
        //    //     */
        //    //    //if (minimizeToTray.Value)
        //    //    //    SysTrayHideShow();
        //    //    //else
        //    //    //    WindowState = FormWindowState.Minimized;
        //    //}
        //    else
        //    {
        //        base.WndProc(ref m);
        //    }
        //}

        private void hideShowClick(object sender, EventArgs e)
        {
            SysTrayHideShow();
        }

        private void ShowParameterForm(ISensor sensor)
        {
            ParameterForm form = new ParameterForm();
            form.Parameters = sensor.Parameters;
            form.captionLabel.Text = sensor.Name;
            form.ShowDialog();
        }

        private void treeView_NodeMouseDoubleClick(object sender,
          TreeNodeAdvMouseEventArgs e)
        {
            //SensorNode node = e.Node.Tag as SensorNode;
            //if (node != null && node.Sensor != null &&
            //  node.Sensor.Parameters.Length > 0)
            //{
            //    ShowParameterForm(node.Sensor);
            //}
        }

        private void celsiusMenuItem_Click(object sender, EventArgs e)
        {
            celsiusMenuItem.Checked = true;
            fahrenheitMenuItem.Checked = false;
            unitManager.TemperatureUnit = TemperatureUnit.Celsius;
        }

        private void fahrenheitMenuItem_Click(object sender, EventArgs e)
        {
            celsiusMenuItem.Checked = false;
            fahrenheitMenuItem.Checked = true;
            unitManager.TemperatureUnit = TemperatureUnit.Fahrenheit;
        }

        private void sumbitReportMenuItem_Click(object sender, EventArgs e)
        {
            ReportForm form = new ReportForm();
            form.Report = computer.GetReport();
            form.ShowDialog();
        }

        private void resetMinMaxMenuItem_Click(object sender, EventArgs e)
        {
            computer.Accept(new SensorVisitor(delegate (ISensor sensor)
            {
                sensor.ResetMin();
                sensor.ResetMax();
            }));
        }

        private void MainForm_MoveOrResize(object sender, EventArgs e)
        {
            //if (WindowState != FormWindowState.Minimized)
            //{
            //    settings.SetValue("mainForm.Location.X", Bounds.X);
            //    settings.SetValue("mainForm.Location.Y", Bounds.Y);
            //    settings.SetValue("mainForm.Width", Bounds.Width);
            //    settings.SetValue("mainForm.Height", Bounds.Height);
            //}
        }

        private void resetClick(object sender, EventArgs e)
        {
            // disable the fallback MainIcon during reset, otherwise icon visibility
            // might be lost 
            //systemTray.IsMainIconEnabled = false;
            computer.Close();
            computer.Open();
            // restore the MainIcon setting
            //systemTray.IsMainIconEnabled = minimizeToTray.Value;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void menuItem6_Click(object sender, EventArgs e)
        {
            try
            {
                if (TopMost)
                {
                    TopMost = false;
                    this.menuItem6.Checked = false;
                    előtérbeToolStripMenuItem.Enabled = true;
                }
                else
                {
                    TopMost = true;
                    this.menuItem6.Checked = true;
                    előtérbeToolStripMenuItem.Enabled = false;
                }

                Program.KONFFelulMarado = felülMaradóToolStripMenuItem.Checked = this.TopMost;
            }
            catch { }

            try
            {
                if (HoMeRok.checkBox1.Checked != this.TopMost)
                    HoMeRok.checkBox1.Checked = this.TopMost;
            }
            catch { }

            try
            {
                if (Program.Attekint != null)
                    Program.Attekint.Dispatcher.Invoke(delegate () { Program.Attekint.Topmost = this.TopMost; });
            }
            catch { }

            for (int i = 0; i < Program.OsszesForm.Count; ++i)
            {
                try
                {
                    Program.OsszesForm[i].TopMost = Program.KONFFelulMarado;
                }
                catch
                {
                    try
                    {
                        Program.OsszesForm.RemoveAt(i);
                        --i;
                    }
                    catch { }
                }
            }

            //if (AzVez != null)
            //    AzVez.TopMost = this.TopMost;
            //if (SZListazo != null)
            //    SZListazo.TopMost = this.TopMost;
            //if (Lkiv != null)
            //    Lkiv.TopMost = this.TopMost;
            //if (SZListazo2 != null)
            //    SZListazo2.TopMost = this.TopMost;
            //if (Rtorlo != null)
            //    Rtorlo.TopMost = this.TopMost;
            //if (AlapFord != null)
            //    AlapFord.TopMost = this.TopMost;
            //if (BiztSzenz != null)
            //    BiztSzenz.TopMost = this.TopMost;
            try
            {
                HoMeRok.TopMost = KMutato.TopMost = Program.KONFFelulMarado;
            }
            catch { }

        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void treeView_MouseMove(object sender, MouseEventArgs e)
        {
            selectionDragging = selectionDragging &
              (e.Button & (MouseButtons.Left | MouseButtons.Right)) > 0;

            if (selectionDragging)
                treeView.SelectedNode = treeView.GetNodeAt(e.Location);
        }

        private void treeView_MouseDown(object sender, MouseEventArgs e)
        {
            selectionDragging = true;
        }

        private void treeView_MouseUp(object sender, MouseEventArgs e)
        {
            selectionDragging = false;
        }

        private void serverPortMenuItem_Click(object sender, EventArgs e)
        {
            new PortForm(this).ShowDialog();
        }

        public HttpServer Server
        {
            get { return server; }
        }

        public SzenzorListazo SZListazo;
        private void menuItem8_Click(object sender, EventArgs e)
        {
            GetTeljesverz.FreemiumClickTest();
            SZListazo = new SzenzorListazo(true, true);
            SZListazo.ShowDialog();
        }

        public SemaKezelo SemKezel;
        private void menuItem9_Click(object sender, EventArgs e)
        {
            GetTeljesverz.FreemiumClickTest();
            SemKezel = new SemaKezelo();
            SemKezel.ShowDialog();
        }

        ManualVez AzVez;
        private void menuItem11_Click(object sender, EventArgs e)
        {
            GetTeljesverz.FreemiumClickTest();
            AzVez = new ManualVez(this, KMutato);
            AzVez.Show();
        }

        private void menuItem12_Click(object sender, EventArgs e)
        {
            /*Program.AzonnaliVez = false;*/
            /*Fajlkezelo.Elnevezo(this);*/

            for (int i = 0; i < DirektVez.Length; ++i)
            {
                DirektVez[i] = false;
            }
            Program.HisztDeaktiv = true;
        }

        private void menuItem14_Click(object sender, EventArgs e)
        {
            GetTeljesverz.FreemiumClickTest();
            Bezar(false, "");
        }

        private void menuItem23_Click(object sender, EventArgs e)
        {
            new Visszajelzes(this).ShowDialog();
        }

        AlapBeallito AlapFord;
        private void menuItem26_Click(object sender, EventArgs e)
        {
            AlapFord = new AlapBeallito(this);
            AlapFord.ShowDialog();
        }

        private void gadgetMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void menuItem24_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Web: www.feverkill.com\nE-mail: info@feverkill.com\n\nBuild: " + Program.Verzioszam + "\n\nDo you want to visit the website?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    Process.Start("https://www.feverkill.com");
                }
                catch
                { }
            }
        }

        private void menuItem28_Click(object sender, EventArgs e)
        {
            Tutorial.MBShow(Tutorial.Mboxok.Alaplapi_Vez);
        }

        private void menuItem17_Click(object sender, EventArgs e)
        {
            Tutorial.MBShow(Tutorial.Mboxok.Feverkill);
        }

        private void menuItem18_Click(object sender, EventArgs e)
        {
            Tutorial.MBShow(Tutorial.Mboxok.Szabalyzolistak_Letreh);
        }

        private void menuItem19_Click(object sender, EventArgs e)
        {
            Tutorial.MBShow(Tutorial.Mboxok.Direkt_Vezerles);
        }

        private void menuItem20_Click(object sender, EventArgs e)
        {
            Tutorial.MBShow(Tutorial.Mboxok.Alapert_Fordszam);
        }

        private void menuItem29_Click(object sender, EventArgs e)
        {
            Tutorial.MBShow(Tutorial.Mboxok.Szabalyzolistak_Osszevetese);
        }

        private void menuItem30_Click(object sender, EventArgs e)
        {
            Tutorial.MBShow(Tutorial.Mboxok.Felul_Maradas);
        }
        private void menuItem43_Click(object sender, EventArgs e)
        {
            Tutorial.MBShow(Tutorial.Mboxok.Riasztasok);
        }

        private void menuItem32_Click(object sender, EventArgs e)
        {
            try
            {
                new Frissito(true).Show();
            }
            catch
            { }
        }
        private void kilépésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Bezar(false, "");
        }

        private void mutatásRejtésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SysTrayHideShow();
        }

        private void menuItem33_Click(object sender, EventArgs e)
        {
            SysTrayHideShow();
        }
        private void menuItem37_Click(object sender, EventArgs e)
        {
            //Program.KONFKittenyMutat = menuItem39.Checked = !menuItem39.Checked;
            //if (menuItem39.Checked)
            //    KMutato.Show();
            //else
            //    KMutato.Hide();
        }

        public SzenzorListazo SZListazo2;
        private void menuItem34_Click(object sender, EventArgs e)
        {
            GetTeljesverz.FreemiumClickTest();
            SZListazo2 = new SzenzorListazo(false, true);
            SZListazo2.ShowDialog();
        }

        private void menuItem39_Click(object sender, EventArgs e)
        {
            //Program.KONFHomersMutat = menuItem39.Checked = !menuItem39.Checked;
            //if (menuItem39.Checked)
            //    HoMeRok.Show();
            //else
            //    HoMeRok.Hide();
        }

        RiasztKezelo Rtorlo;
        private void menuItem35_Click(object sender, EventArgs e)
        {
            GetTeljesverz.FreemiumClickTest();
            Rtorlo = new RiasztKezelo();
            Rtorlo.ShowDialog();
        }
        private void menuItem79_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(((Program.KONFNyelv == "hun") ? "Biztos ebben a beállításban?\nEz az ablak így láthatatlan lesz!" : Eszk.GetNyelvSzo("OpacitasMbox1SZOVEG")), "0% " + ((Program.KONFNyelv == "hun") ? "Opacitás" : Eszk.GetNyelvSzo("Opacitás")), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                KMutato.Opacity = HoMeRok.Opacity = this.Opacity = Program.KONFOpacitas = 0;
                MessageBox.Show(((Program.KONFNyelv == "hun") ? "Az Insert gomb megnyomásával visszaállhat 100% opacitásra." : Eszk.GetNyelvSzo("OpacitasMbox2SZOVEG")), "0% " + ((Program.KONFNyelv == "hun") ? "Opacitás" : Eszk.GetNyelvSzo("Opacitás")), MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }
        private void menuItem45_Click(object sender, EventArgs e)
        {
            KMutato.Opacity = HoMeRok.Opacity = this.Opacity = Program.KONFOpacitas = 0.1;
            HoMeRok.trackBar1.Value = 10;
        }

        private void menuItem46_Click(object sender, EventArgs e)
        {
            KMutato.Opacity = HoMeRok.Opacity = this.Opacity = Program.KONFOpacitas = 0.2;
            HoMeRok.trackBar1.Value = 20;
        }

        private void menuItem47_Click(object sender, EventArgs e)
        {
            KMutato.Opacity = HoMeRok.Opacity = this.Opacity = Program.KONFOpacitas = 0.3;
            HoMeRok.trackBar1.Value = 30;
        }

        private void menuItem48_Click(object sender, EventArgs e)
        {
            KMutato.Opacity = HoMeRok.Opacity = this.Opacity = Program.KONFOpacitas = 0.4;
            HoMeRok.trackBar1.Value = 40;
        }

        private void menuItem49_Click(object sender, EventArgs e)
        {
            KMutato.Opacity = HoMeRok.Opacity = this.Opacity = Program.KONFOpacitas = 0.5;
            HoMeRok.trackBar1.Value = 50;
        }

        private void menuItem50_Click(object sender, EventArgs e)
        {
            KMutato.Opacity = HoMeRok.Opacity = this.Opacity = Program.KONFOpacitas = 0.6;
            HoMeRok.trackBar1.Value = 60;
        }

        private void menuItem51_Click(object sender, EventArgs e)
        {
            KMutato.Opacity = HoMeRok.Opacity = this.Opacity = Program.KONFOpacitas = 0.7;
            HoMeRok.trackBar1.Value = 70;
        }

        private void menuItem52_Click(object sender, EventArgs e)
        {
            KMutato.Opacity = HoMeRok.Opacity = this.Opacity = Program.KONFOpacitas = 0.8;
            HoMeRok.trackBar1.Value = 80;
        }

        private void menuItem53_Click(object sender, EventArgs e)
        {
            KMutato.Opacity = HoMeRok.Opacity = this.Opacity = Program.KONFOpacitas = 0.9;
            HoMeRok.trackBar1.Value = 90;
        }

        private void menuItem54_Click(object sender, EventArgs e)
        {
            KMutato.Opacity = HoMeRok.Opacity = this.Opacity = Program.KONFOpacitas = 1;
            HoMeRok.trackBar1.Value = 100;
        }

        private void SysTrayicon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                menuItem92.PerformClick();
        }

        private void SysTrayicon_BalloonTipClicked(object sender, EventArgs e)
        {
            //SysTrayHideShow();
            menuItem92.PerformClick();
            //Show();
            menuItem6_Click(sender, e);
            menuItem6_Click(sender, e);
        }

        private void menuItem55_Click(object sender, EventArgs e)
        {
            Program.KONFFrissitesInditaskor = menuItem55.Checked = !menuItem55.Checked;
        }

        private void startMinMenuItem_Click(object sender, EventArgs e)
        {
            Program.KONFKismeretIndit = startMinMenuItem.Checked = !startMinMenuItem.Checked;
        }

        private void hőmérsékletekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.HomerokNyitva.Value = !Program.HomerokNyitva.Value;
        }

        private void fordulatszámokToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.KitTenyMutNyitva.Value = !Program.KitTenyMutNyitva.Value;
        }

        private void felülMaradóToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menuItem6_Click(sender, e);
        }

        private void előtérbeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menuItem6_Click(sender, e);
            menuItem6_Click(sender, e);
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(((Program.KONFNyelv == "hun") ? "Be akarja kapcsolni a segítséget?\n\n\tEgyéb kérdés esetén lépjen kapcsolatba a fejlesztő\ncsapattal a \"Segítség>>Visszajelzés Küldése\" menüpont alatt." : Eszk.GetNyelvSzo("SugoMboxSZOVEG")), ((Program.KONFNyelv == "hun") ? "Súgó" : Eszk.GetNyelvSzo("Súgó")), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                menuItem86.PerformClick();
            }
        }

        private void startupMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                startupManager.Startup = !Program.KONFAutoIndul;

                Program.KONFAutoIndul = !Program.KONFAutoIndul;
                startupMenuItem.Checked = Program.KONFAutoIndul;
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show(((Program.KONFNyelv == "hun") ? "Az automatikus indítás beállításának frissítése sikertelen." : Eszk.GetNyelvSzo("AutoIndHibaMboxSZOVEG")), ((Program.KONFNyelv == "hun") ? "Hiba!" : Eszk.GetNyelvSzo("Hiba!")), MessageBoxButtons.OK, MessageBoxIcon.Error);
                //autoStart.Value = startupManager.Startup;
            }
            //if (Program.KONFAutoIndul == false)
            //{
            //    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            //    //Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.Users.OpenSubKey("S-1-5-21-3320475371-2024744428-282263555-1001\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            //    key.SetValue("Szelcsend", "\"" + System.IO.Directory.GetCurrentDirectory() + "\\Szelcsend.exe\"", Microsoft.Win32.RegistryValueKind.String);
            //    //key.SetValue("Szelcsend", "C:\\s.txt", Microsoft.Win32.RegistryValueKind.String);
            //    key.Close();

            //    startupMenuItem.Checked = Program.KONFAutoIndul = true;

            //    Fajlkezelo.FoKonfMento();
            //}
            //else
            //{
            //    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            //    string[] t = key.GetValueNames();
            //    for (int i = 0; i < t.Length; i++)
            //    {
            //        if (t[i] == "Szelcsend")
            //        {
            //            key.DeleteValue("Szelcsend");
            //            break;
            //        }
            //    }
            //    key.Close();
            //    startupMenuItem.Checked = Program.KONFAutoIndul = false;

            //    Fajlkezelo.FoKonfMento();
            //}
        }

        private void menuItem61_Click(object sender, EventArgs e)
        {
            MessageBox.Show(((Program.KONFNyelv == "hun") ? "Ez a beállítás adja meg, hogy a program mekkora\nidőközönként kérje le a hőmérsékleti értékeket\nés állítsa a fordulatszámokat.\n\nA kisebb időköz nagyobb érzékenységet,\nugyanakkor nagyobb erőforrásigényt is jelent." : Eszk.GetNyelvSzo("FrissIdoMboxSZOVEG")), ((Program.KONFNyelv == "hun") ? "Frissítési időköz" : Eszk.GetNyelvSzo("Frissítési Időköz")), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #region KONFFrisIdo
        private void menuItem62_Click(object sender, EventArgs e)
        {
            Program.KONFFrisIdo = 1000;
            Eszkozok.Eszk.Elnevezo();
            Fajlkezelo.FoKonfMento();
        }
        private void menuItem77_Click(object sender, EventArgs e)
        {
            Program.KONFFrisIdo = 1500;
            Eszkozok.Eszk.Elnevezo();
            Fajlkezelo.FoKonfMento();
        }

        private void menuItem63_Click(object sender, EventArgs e)
        {
            Program.KONFFrisIdo = 2000;
            Eszkozok.Eszk.Elnevezo();
            Fajlkezelo.FoKonfMento();
        }

        private void menuItem64_Click(object sender, EventArgs e)
        {
            Program.KONFFrisIdo = 3000;
            Eszkozok.Eszk.Elnevezo();
            Fajlkezelo.FoKonfMento();
        }

        private void menuItem65_Click(object sender, EventArgs e)
        {
            Program.KONFFrisIdo = 4000;
            Eszkozok.Eszk.Elnevezo();
            Fajlkezelo.FoKonfMento();
        }

        private void menuItem66_Click(object sender, EventArgs e)
        {
            Program.KONFFrisIdo = 5000;
            Eszkozok.Eszk.Elnevezo();
            Fajlkezelo.FoKonfMento();
        }

        private void menuItem67_Click(object sender, EventArgs e)
        {
            Program.KONFFrisIdo = 6000;
            Eszkozok.Eszk.Elnevezo();
            Fajlkezelo.FoKonfMento();
        }

        private void menuItem68_Click(object sender, EventArgs e)
        {
            Program.KONFFrisIdo = 7000;
            Eszkozok.Eszk.Elnevezo();
            Fajlkezelo.FoKonfMento();
        }

        private void menuItem69_Click(object sender, EventArgs e)
        {
            Program.KONFFrisIdo = 8000;
            Eszkozok.Eszk.Elnevezo();
            Fajlkezelo.FoKonfMento();
        }

        private void menuItem70_Click(object sender, EventArgs e)
        {
            Program.KONFFrisIdo = 9000;
            Eszkozok.Eszk.Elnevezo();
            Fajlkezelo.FoKonfMento();
        }

        private void menuItem71_Click(object sender, EventArgs e)
        {
            Program.KONFFrisIdo = 10000;
            Eszkozok.Eszk.Elnevezo();
            Fajlkezelo.FoKonfMento();
        }

        private void menuItem72_Click(object sender, EventArgs e)
        {
            Program.KONFFrisIdo = 12000;
            Eszkozok.Eszk.Elnevezo();
            Fajlkezelo.FoKonfMento();
        }

        private void menuItem73_Click(object sender, EventArgs e)
        {
            Program.KONFFrisIdo = 14000;
            Eszkozok.Eszk.Elnevezo();
            Fajlkezelo.FoKonfMento();
        }

        private void menuItem74_Click(object sender, EventArgs e)
        {
            Program.KONFFrisIdo = 16000;
            Eszkozok.Eszk.Elnevezo();
            Fajlkezelo.FoKonfMento();
        }

        private void menuItem75_Click(object sender, EventArgs e)
        {
            Program.KONFFrisIdo = 18000;
            Eszkozok.Eszk.Elnevezo();
            Fajlkezelo.FoKonfMento();
        }

        private void menuItem76_Click(object sender, EventArgs e)
        {
            Program.KONFFrisIdo = 20000;
            Eszkozok.Eszk.Elnevezo();
            Fajlkezelo.FoKonfMento();
        }
        #endregion

        private void menuItem81_Click(object sender, EventArgs e)
        {
            Tutorial.MBShow(Tutorial.Mboxok.Gyorsbillentyuk);
        }

        private void menuItem84_Click(object sender, EventArgs e)
        {
            Tutorial.MBShow(Tutorial.Mboxok.Celhardver_Elso_Haszn);
        }

        private void menuItem85_Click(object sender, EventArgs e)
        {
            Tutorial.MBShow(Tutorial.Mboxok.Celhardver);
        }

        private void menuItem56_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= 5 && !MindenAlaplapiListaalapura(i, true); i++)
            {

            }
        }

        bool MindenAlaplapiListaalapura(int probszam, bool MSGBox)
        {
            try
            {
                if (Program.BelsoVentiVezTip == Program.BelsoVentiTipus.Hardware)
                {
                    foreach (TreeNodeAdv item in treeView.AllNodes)
                    {
                        foreach (NodeControlInfo NCInfo in treeView.GetNodeControls(item))
                        {
                            SensorNode snd = NCInfo.Node.Tag as SensorNode;
                            if (snd != null)
                                if (snd.Sensor != null)
                                    if (snd.Sensor.Control != null)
                                    {
                                        IControl control = snd.Sensor.Control;
                                        //MessageBox.Show(control.Identifier.ToString() + "/" + snd.Text, "Control Azonosító");

                                        control.SetCMListaAlapu();
                                    }
                        }
                    }
                }
                else if (Program.BelsoVentiVezTip == Program.BelsoVentiTipus.Emulalt)
                {
                    foreach (Program.EmulaltBelsoVenti item in Program.EmulaltBelsoVentik)
                    {
                        item.ControlMode = ControlMode.Listaalapu;
                    }
                }

                List<string> KulcsList = new List<string>();
                foreach (KeyValuePair<string, float> item in HiszterezisesHomok)
                {
                    KulcsList.Add(item.Key);
                }
                for (int i = 0; i < KulcsList.Count; i++)
                {
                    HiszterezisesHomok[KulcsList[i]] = -2000;
                }
            }
            catch (InvalidOperationException)
            {
                if (probszam >= 5)
                    MessageBox.Show("Hiba történt a művelet során!", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            catch
            {
                if (probszam >= 5)
                    MessageBox.Show("Hiba történt a művelet során!", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (MSGBox)
                SysTrayicon.ShowBalloonTip(800, "Feverkill", "Belső ventilátorok Listaalapú vezérlésen!", ToolTipIcon.Info);
            return true;
        }

        public bool MindenAlaplapiAlapertelmezettre(int probszam, bool MSGBox)
        {
            try
            {
                if (Program.BelsoVentiVezTip == Program.BelsoVentiTipus.Hardware)
                {
                    foreach (TreeNodeAdv item in treeView.AllNodes)
                    {
                        foreach (NodeControlInfo NCInfo in treeView.GetNodeControls(item))
                        {
                            SensorNode snd = NCInfo.Node.Tag as SensorNode;
                            if (snd != null)
                                if (snd.Sensor != null)
                                    if (snd.Sensor.Control != null)
                                    {
                                        IControl control = snd.Sensor.Control;
                                        //MessageBox.Show(control.Identifier.ToString() + "/" + snd.Text, "Control Azonosító");

                                        control.SetDefault();
                                    }
                        }
                    }
                }
                else if (Program.BelsoVentiVezTip == Program.BelsoVentiTipus.Emulalt)
                {
                    foreach (Program.EmulaltBelsoVenti item in Program.EmulaltBelsoVentik)
                    {
                        item.ControlMode = ControlMode.Alapert;
                    }
                }
            }
            catch
            {
                if (probszam >= 5)
                    MessageBox.Show("Hiba történt a művelet során!", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (MSGBox)
                SysTrayicon.ShowBalloonTip(800, "Feverkill", "Belső ventilátorok Eredeti vezérlésen!", ToolTipIcon.Info);
            return true;
        }

        private void menuItem90_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= 5 && !MindenAlaplapiAlapertelmezettre(i, true); i++)
            {

            }
        }

        private void áttekintőToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menuItem92.PerformClick();
        }
        public bool TutorWPFMegjelenitve = false;
        public bool TutorWPFLeallit = false;
        public Thread TutorTH;
        private void menuItem86_Click(object sender, EventArgs e)
        {
            if (TutorWPFMegjelenitve)
            {
                try
                {
                    //TutorTH.Abort();
                    //Application ap =new  TutorWPF.TutorWPFAblak();

                    TutorWPFLeallit = true;

                    Program.KONFTutorialMegjelenit = false;
                    Fajlkezelo.FoKonfMento();
                    // TutorWPFMegjelenitve = false;
                    // Program.TutorialWPFAblak.Dispatcher.Invoke(Program.TutorialWPFAblak.Close);
                }
                catch
                { }
            }
            else
            {
                try
                {
                    TutorWPFLeallit = false;
                    TutorTH = new Thread(delegate ()
                    {
                        Program.TutorialWPFAblak = new UdvozloKepernyo.TutorWPFAblak();
                        ElementHost.EnableModelessKeyboardInterop(Program.TutorialWPFAblak);
                        //Program.TutorialWPFAblak.ShowDialog();

                        Program.TutorialWPFAblak.Show();
                        Application.Run();
                        //while (TutorWPFMegjelenitve)
                        //{

                        //}
                    });
                    TutorTH.SetApartmentState(ApartmentState.STA);
                    TutorTH.Start();

                    Program.KONFTutorialMegjelenit = true;
                    Fajlkezelo.FoKonfMento();
                }
                catch
                { }

            }
            //Messagebox-os tutorial
            //if (menuItem86.Checked == false)
            //{
            //    if (TutorTH.ThreadState != System.Threading.ThreadState.Running)
            //        try { TutorTH.Start(); } catch { try { TutorTH.Resume(); } catch { } }
            //    Tutorial.Statusz = Tutorial.TStat.NINCS;
            //    menuItem86.Checked = true;
            //    Tutorial.Leptet(this);

            //}
            //else
            //{
            //    Tutorial.Statusz = Tutorial.TStat.NINCS;
            //    menuItem86.Checked = false;

            //    try { TutorTH.Suspend(); } catch { }
            //    menuItem15.Text = ((Program.KONFNyelv == "hun") ? "Segítség" : Eszk.GetNyelvSzo("MFmenuItem15"]);
            //    menuItem87.Text = ((Program.KONFNyelv == "hun") ? "Tutorial Léptetése" : Eszk.GetNyelvSzo("MFmenuItem87"]);
            //}
        }

        public void HisztJelolo()
        {
            switch ((int)Math.Round(Program.KONFHiszterezis * 2))
            {

                case 0:
                    menuIthisz0.Checked = true;
                    menuIthisz05.Checked = false;
                    menuIthisz1.Checked = false;
                    menuIthisz15.Checked = false;
                    menuIthisz2.Checked = false;
                    menuIthisz25.Checked = false;
                    menuIthisz3.Checked = false;
                    menuIthisz35.Checked = false;
                    menuIthisz4.Checked = false;
                    menuIthisz45.Checked = false;
                    menuIthisz5.Checked = false;
                    break;
                case 1:
                    menuIthisz0.Checked = false;
                    menuIthisz05.Checked = true;
                    menuIthisz1.Checked = false;
                    menuIthisz15.Checked = false;
                    menuIthisz2.Checked = false;
                    menuIthisz25.Checked = false;
                    menuIthisz3.Checked = false;
                    menuIthisz35.Checked = false;
                    menuIthisz4.Checked = false;
                    menuIthisz45.Checked = false;
                    menuIthisz5.Checked = false;
                    break;
                case 2:
                    menuIthisz0.Checked = false;
                    menuIthisz05.Checked = false;
                    menuIthisz1.Checked = true;
                    menuIthisz15.Checked = false;
                    menuIthisz2.Checked = false;
                    menuIthisz25.Checked = false;
                    menuIthisz3.Checked = false;
                    menuIthisz35.Checked = false;
                    menuIthisz4.Checked = false;
                    menuIthisz45.Checked = false;
                    menuIthisz5.Checked = false;
                    break;
                case 3:
                    menuIthisz0.Checked = false;
                    menuIthisz05.Checked = false;
                    menuIthisz1.Checked = false;
                    menuIthisz15.Checked = true;
                    menuIthisz2.Checked = false;
                    menuIthisz25.Checked = false;
                    menuIthisz3.Checked = false;
                    menuIthisz35.Checked = false;
                    menuIthisz4.Checked = false;
                    menuIthisz45.Checked = false;
                    menuIthisz5.Checked = false;
                    break;
                case 4:
                    menuIthisz0.Checked = false;
                    menuIthisz05.Checked = false;
                    menuIthisz1.Checked = false;
                    menuIthisz15.Checked = false;
                    menuIthisz2.Checked = true;
                    menuIthisz25.Checked = false;
                    menuIthisz3.Checked = false;
                    menuIthisz35.Checked = false;
                    menuIthisz4.Checked = false;
                    menuIthisz45.Checked = false;
                    menuIthisz5.Checked = false;
                    break;
                case 5:
                    menuIthisz0.Checked = false;
                    menuIthisz05.Checked = false;
                    menuIthisz1.Checked = false;
                    menuIthisz15.Checked = false;
                    menuIthisz2.Checked = false;
                    menuIthisz25.Checked = true;
                    menuIthisz3.Checked = false;
                    menuIthisz35.Checked = false;
                    menuIthisz4.Checked = false;
                    menuIthisz45.Checked = false;
                    menuIthisz5.Checked = false;
                    break;
                case 6:
                    menuIthisz0.Checked = false;
                    menuIthisz05.Checked = false;
                    menuIthisz1.Checked = false;
                    menuIthisz15.Checked = false;
                    menuIthisz2.Checked = false;
                    menuIthisz25.Checked = false;
                    menuIthisz3.Checked = true;
                    menuIthisz35.Checked = false;
                    menuIthisz4.Checked = false;
                    menuIthisz45.Checked = false;
                    menuIthisz5.Checked = false;
                    break;
                case 7:
                    menuIthisz0.Checked = false;
                    menuIthisz05.Checked = false;
                    menuIthisz1.Checked = false;
                    menuIthisz15.Checked = false;
                    menuIthisz2.Checked = false;
                    menuIthisz25.Checked = false;
                    menuIthisz3.Checked = false;
                    menuIthisz35.Checked = true;
                    menuIthisz4.Checked = false;
                    menuIthisz45.Checked = false;
                    menuIthisz5.Checked = false;
                    break;
                case 8:
                    menuIthisz0.Checked = false;
                    menuIthisz05.Checked = false;
                    menuIthisz1.Checked = false;
                    menuIthisz15.Checked = false;
                    menuIthisz2.Checked = false;
                    menuIthisz25.Checked = false;
                    menuIthisz3.Checked = false;
                    menuIthisz35.Checked = false;
                    menuIthisz4.Checked = true;
                    menuIthisz45.Checked = false;
                    menuIthisz5.Checked = false;
                    break;
                case 9:
                    menuIthisz0.Checked = false;
                    menuIthisz05.Checked = false;
                    menuIthisz1.Checked = false;
                    menuIthisz15.Checked = false;
                    menuIthisz2.Checked = false;
                    menuIthisz25.Checked = false;
                    menuIthisz3.Checked = false;
                    menuIthisz35.Checked = false;
                    menuIthisz4.Checked = false;
                    menuIthisz45.Checked = true;
                    menuIthisz5.Checked = false;
                    break;
                case 10:
                    menuIthisz0.Checked = false;
                    menuIthisz05.Checked = false;
                    menuIthisz1.Checked = false;
                    menuIthisz15.Checked = false;
                    menuIthisz2.Checked = false;
                    menuIthisz25.Checked = false;
                    menuIthisz3.Checked = false;
                    menuIthisz35.Checked = false;
                    menuIthisz4.Checked = false;
                    menuIthisz45.Checked = false;
                    menuIthisz5.Checked = true;
                    break;

                case 12:
                    Program.KONFHiszterezis = 1;
                    HisztJelolo();
                    break;

                default:
                    Program.KONFHiszterezis = 2;
                    HisztJelolo();
                    break;
            }

            Program.Attekint.labelHiszt.Dispatcher.Invoke(delegate () { Program.Attekint.labelHiszt.Content = ((Program.KONFNyelv == "hun") ? "Hiszterézis: " : (Eszk.GetNyelvSzo("ATTEKUIHiszterezis") + ": ")) + Program.KONFHiszterezis + "°C"; });

            if (Fajlkezelo.KiirtKONFTESZT() == false)
            {
                Fajlkezelo.FoKonfMento();
            }
        }
        private void menuIthisz0_Click(object sender, EventArgs e)
        {
            Program.KONFHiszterezis = 0;
            HisztJelolo();
        }

        private void menuIthisz05_Click(object sender, EventArgs e)
        {
            Program.KONFHiszterezis = 0.5F;
            HisztJelolo();
        }

        private void menuIthisz1_Click(object sender, EventArgs e)
        {
            Program.KONFHiszterezis = 1;
            HisztJelolo();
        }

        private void menuIthisz15_Click(object sender, EventArgs e)
        {
            Program.KONFHiszterezis = 1.5F;
            HisztJelolo();
        }

        private void menuIthisz2_Click(object sender, EventArgs e)
        {
            Program.KONFHiszterezis = 2;
            HisztJelolo();
        }

        private void menuIthisz25_Click(object sender, EventArgs e)
        {
            Program.KONFHiszterezis = 2.5F;
            HisztJelolo();
        }
        private void menuIthisz3_Click(object sender, EventArgs e)
        {
            Program.KONFHiszterezis = 3;
            HisztJelolo();
        }

        private void menuIthisz35_Click(object sender, EventArgs e)
        {
            Program.KONFHiszterezis = 3.5F;
            HisztJelolo();
        }

        private void menuIthisz4_Click(object sender, EventArgs e)
        {
            Program.KONFHiszterezis = 4;
            HisztJelolo();
        }

        private void menuIthisz45_Click(object sender, EventArgs e)
        {
            Program.KONFHiszterezis = 4.5F;
            HisztJelolo();
        }

        private void menuIthisz5_Click(object sender, EventArgs e)
        {
            Program.KONFHiszterezis = 5;
            HisztJelolo();
        }

        private void menuItem95_Click(object sender, EventArgs e)
        {
            GetTeljesverz.FreemiumClickTest();
            menuItem95.Checked = !menuItem95.Checked;
            Program.KONFUdvKeperny = menuItem95.Checked;
        }

        private void menuItem96_Click(object sender, EventArgs e)
        {
            Nyelvvalaszto NYelvV = new Nyelvvalaszto(this);
            NYelvV.ShowDialog();
        }

        //TUTORIAL LÉPTETÉS
        private void menuItem87_Click(object sender, EventArgs e)
        {
            //if (menuItem86.Checked == false)
            //{
            //    menuItem86.Checked = true;
            //    Tutorial.Statusz = Tutorial.TStat.NINCS;
            //    try { TutorTH.Suspend(); } catch { }
            //    menuItem15.Text = "Segítség>";
            //    menuItem87.Text = "Tutorial Léptetése";
            //}


            //Tutorial.Leptet(this);
            //if (TutorTH.ThreadState != System.Threading.ThreadState.Running && menuItem15.Text != "Segítség>")
            //    try { TutorTH.Start(); } catch { try { TutorTH.Resume(); } catch { } }
            //else if (menuItem15.Text == "Segítség>")
            //{
            //    menuItem15.Text = "Segítség";
            //}
        }

        BiztSzenzorok BiztSzenz;
        private void menuItem97_Click(object sender, EventArgs e)
        {
            BiztSzenz = new BiztSzenzorok(this);
            BiztSzenz.ShowDialog();
        }

        public bool DevFormNyitva = false;
        private void menuItem99_Click(object sender, EventArgs e)
        {
            if (!DevFormNyitva)
            {
                DevForm = new Jatszoter(this);
                DevForm.Show();
                DevFormNyitva = true;
            }
            else
            {
                DevForm.Close();
                DevFormNyitva = false;
            }
        }

        private void menuItem100_Click(object sender, EventArgs e)
        {
            switch (MessageBox.Show(((Program.KONFNyelv == "hun") ? "A Kernel32 HDDinfo letiltására bizonyos, nem szabványos felépítésű alaplapoknál lehet szükség, amennyiben a Vezérlőszoftver futása közben akadozás érzékelhető, avagy a Vezérlőszoftverből való kilépés után az nem áll le megfelelően.\n\nLe akarja tiltani a Kernel32 HDDinfo lekérdezéseket?" : Eszk.GetNyelvSzo("Kern32HDDLetiltMboxSZOVEG")), ((Program.KONFNyelv == "hun") ? "HDDinfo letiltása" : Eszk.GetNyelvSzo("Kern32HDDLetiltMboxCIM")), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3))
            {
                case DialogResult.Yes:
                    Program.KONFHDDKernel32Tiltas = true;
                    menuItem100.Checked = true;
                    OpenHardwareMonitor.Seged.HDDKernel32Tiltas = true;
                    Fajlkezelo.FoKonfMento();
                    break;
                case DialogResult.No:
                    Program.KONFHDDKernel32Tiltas = false;
                    menuItem100.Checked = false;
                    OpenHardwareMonitor.Seged.HDDKernel32Tiltas = false;
                    Fajlkezelo.FoKonfMento();
                    break;
            }
        }

        private void timerVedelem_Tick(object sender, EventArgs e)
        {
            new Thread(Vedelem.IndulasiHitelesites).Start();
        }

        private void menuItem101_Click(object sender, EventArgs e)
        {
            string ervenyes = "N/A";
            try
            {
                ervenyes = Program.LICENSZERVENYESSEG;

                string[] erv = Program.LICENSZERVENYESSEG.Split('.');
                DateTime dt = new DateTime(int.Parse(erv[0]), int.Parse(erv[1]), int.Parse(erv[2]));//Nyelvterülethez valo dátumformázás
                ervenyes = dt.ToShortDateString();
            }
            catch { }
            MessageBox.Show("Név: " + Program.LICENSZNev
                          + "\nE-mail: " + Program.LICENSZEmail
                          + "\nID: " + Program.LICENSZID
                          + "\nJelszó: " + Program.LICENSZJelszo
                          + "\nÉrvényes: " + ervenyes
                          + "\nTípus: " + Vedelem.GetLicenszTipStringnevFromInt(),
                          "Licensz Adatai", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        bool GetFullelsotick = true;
        private void timerFreemiumReklam_Elapsed(object sender, EventArgs e)
        {
            if (!Eszkozok.Eszk.IsPremiumFuncEabled())
            {
                if (GetFullelsotick)
                {
                    timerFreemiumReklam.Interval = 15 * 60 * 1000;//Indításkor mindenképpen feldobja 15p után
                }
                else
                {
                    try
                    {
                        int db = Program.RandomObject.Next(1, 3);
                        for (int i = 0; i < db; i++)
                        {
                            new Thread(delegate ()
                            {

                                //this.Invoke(delegate(){ new GetTeljesverz().ShowDialog(); });
                                // Program.Attekint.Dispatcher.Invoke(delegate () {
                                new GetTeljesverz().Show();
                                // });
                            }).Start();
                        }
                    }
                    catch { }

                    timerFreemiumReklam.Interval = Program.RandomObject.Next(20 * 60 * 1000, 120 * 60 * 1000);//60 és 160 perc közti intervallum
                }

            }
            else
            {
                timerFreemiumReklam.Enabled = false;
            }
            GetFullelsotick = false;
        }
        private void menuItem1_Click(object sender, EventArgs e)
        {//DEVOPT
            AllocConsole();
            Console.WriteLine();
            Console.WriteLine("Console allocation successfull.");
            Console.WriteLine();
            Console.WriteLine("Teljes:" + string.Format("{0:N2}", idok));
            Console.WriteLine("01:    " + string.Format("{0:N2}", idok1));
            Console.WriteLine("02:    " + string.Format("{0:N2}", idok2));
            Console.WriteLine("03:    " + string.Format("{0:N2}", idok3));
            Console.WriteLine("04:    " + string.Format("{0:N2}", idok4));
            Console.WriteLine("05:    " + string.Format("{0:N2}", idok5));
            Console.WriteLine("06:    " + string.Format("{0:N2}", idok6));
            Console.WriteLine("07:    " + string.Format("{0:N2}", idok7));
            Console.WriteLine("08:    " + string.Format("{0:N2}", idok8));
            Console.WriteLine("09:    " + string.Format("{0:N2}", idok9));
            Console.WriteLine("10(3a):" + string.Format("{0:N2}", idok10));
            Console.WriteLine("11(5a):" + string.Format("{0:N2}", idok11));
            Console.WriteLine("12(sl):" + string.Format("{0:N2}", idok12));

            //Console.WriteLine(Program.THr.ThreadState);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();


        private void menuItem15_Click(object sender, EventArgs e)
        {
            new AutoIndKesl().ShowDialog();
        }

        private void menuItemSupervisor_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Feverkill Supervisor protects the Control Software against unplanned exiting.\nThe only acceptable reason to stop cooling controlling is the user's command!\n\n" + ((Program.VanSupervisor) ? "Supervisor is active so your computer is safe." : "Supervisor is inactive. If you want maximal protection, start Feverkill by the \"FeverkillSupervisor.exe\"!"), "Supervisor is " + ((Program.VanSupervisor) ? "ACTIVE" : "INACTIVE") + "!", MessageBoxButtons.OK, (Program.VanSupervisor) ? MessageBoxIcon.Information : MessageBoxIcon.Exclamation);
        }

        void ShowAttekinto()
        {
            menuItem92.Checked = Program.KONFAttekintMutat = true;
            AttekintoLathatosagAktival();
        }
        void HideAttekinto()
        {
            menuItem92.Checked = Program.KONFAttekintMutat = false;
            AttekintoLathatosagAktival();
        }

        void AttekintoLathatosagAktival()
        {
            try
            {
                if (Program.KONFAttekintMutat)
                {
                    Program.Attekint.listViewHomers.Items.Dispatcher.Invoke(AttekintHomersFrissit);
                    Program.Attekint.listViewFordszamok.Items.Dispatcher.Invoke(AttekintFordszamFrissit);
                    Program.Attekint.Dispatcher.Invoke(delegate { Program.Attekint.WindowState = System.Windows.WindowState.Normal; });
                    Program.Attekint.Dispatcher.Invoke(delegate { Program.Attekint.ShowHelppel(); });
                    előtérbeToolStripMenuItem.PerformClick();
                    Program.Attekint.Dispatcher.Invoke(delegate { Program.Attekint.Activate(); });
                }
                else
                    Program.Attekint.Dispatcher.Invoke(delegate { Program.Attekint.HideHelppel(); });

            }
            catch { }
        }
        private void menuItem92_Click(object sender, EventArgs e)
        {
            Program.KONFAttekintMutat = !Program.KONFAttekintMutat;
            menuItem92.Checked = Program.KONFAttekintMutat;

            AttekintoLathatosagAktival();
        }

        private void menuItem104_Click(object sender, EventArgs e)
        {
            HianyzoAlaplapiControl = true;
            SysTrayicon.ShowBalloonTip(3000, "Feverkill Mobo Controls", "Motherboard and other inner control channels will be updated in the next iteration.", ToolTipIcon.Info);
        }

        private void menuItem105_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Here you can set if you want to load and manage\n    -every PCI Device or\n    -just the ones with fan outputs or RPM sensors.\n\nManaging all of the PCI Devices heavily increases computing performance.\n\nNow loading every device is " + (Program.KONFMindenPCIEszkBetolt ? "enabled" : "disabled") + ". Do you want to " + (Program.KONFMindenPCIEszkBetolt ? "disable" : "enable") + " it?\n(This will restart the software.)", "Managing All PCI Devices", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                Program.KONFMindenPCIEszkBetolt = menuItem105.Checked = !Program.KONFMindenPCIEszkBetolt;
                try
                {
                    for (int i = 0; i <= 5 && !MindenAlaplapiAlapertelmezettre(i, false); i++)
                    {

                    }
                }
                catch { }

                try { Program.SorosPort.Close(); } catch { }
                try { SysTrayicon.Visible = false; } catch { }
                try { Program.HomersKuldTH.Abort(); } catch { }
                try { SysTrayicon.Dispose(); } catch { }

                try
                {
                    Fajlkezelo.FoKonfMento();
                }
                catch { }

                System.Diagnostics.Process.Start("FeverkillSupervisor.exe", "");

                System.Environment.Exit(19981001);
            }
        }

        private void menuItem106_Click(object sender, EventArgs e)
        {
            new BootIteracioBeallit().ShowDialog();
        }

        private void menuItem102_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(((Program.KONFNyelv == "hun") ? "Engedélyezésével láthatóvá válnak a hőmérsékletek az óra melleti ikon menüjében.\nEz némi erőforrásigény növekedéssel jár.\n\nAkarja, hogy megjelenjen a Betekintő?" : Eszk.GetNyelvSzo("BetekintMegjelenitMboxSZOVEG")), ((Program.KONFNyelv == "hun") ? "Betekintő" : Eszk.GetNyelvSzo("BetekintMegjelenitMboxCIM")), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    toolStripMenuItem3Elvalaszto.Visible = betekintesToolStripMenuItem.Visible = menuItem102.Checked = Program.KONFBetekintoMutat = true;
                }
                else
                {
                    toolStripMenuItem3Elvalaszto.Visible = betekintesToolStripMenuItem.Visible = menuItem102.Checked = Program.KONFBetekintoMutat = false;
                }
            }
            catch
            {

            }
        }

        private void TutorThread()
        {
            int varj = 150;
            while (true)
            {
                menuItemSegitseg.Text = "Segítség <";
                menuItem87.Text = "Tutorial Léptetése <";
                System.Threading.Thread.Sleep(varj);
                menuItemSegitseg.Text = "Segítség <<";
                menuItem87.Text = "Tutorial Léptetése <<";
                System.Threading.Thread.Sleep(varj);
                menuItemSegitseg.Text = "Segítség <<<";
                menuItem87.Text = "Tutorial Léptetése <<<";
                System.Threading.Thread.Sleep(varj);
                menuItemSegitseg.Text = "Segítség <<<=";
                menuItem87.Text = "Tutorial Léptetése <<<=";
                System.Threading.Thread.Sleep(varj);
                menuItemSegitseg.Text = "Segítség <<<==";
                menuItem87.Text = "Tutorial Léptetése <<<==";
                System.Threading.Thread.Sleep(varj);
                menuItemSegitseg.Text = "Segítség <<<==-";
                menuItem87.Text = "Tutorial Léptetése <<<==-";
                System.Threading.Thread.Sleep(varj);
                menuItemSegitseg.Text = "Segítség <<<==-!";
                menuItem87.Text = "Tutorial Léptetése <<<==-!";
                System.Threading.Thread.Sleep(varj);
                menuItemSegitseg.Text = "Segítség";
                menuItem87.Text = "Tutorial Léptetése";
                System.Threading.Thread.Sleep(varj);
            }
        }

        void Lokalizalj()
        {
            this.fileMenuItem.Text = Eszk.GetNyelvSzo("MFfileMenuItem"); //Fájl
            this.viewMenuItem.Text = Eszk.GetNyelvSzo("MFviewMenuItem"); //Nézet
            this.plotMenuItem.Text = Eszk.GetNyelvSzo("MFplotMenuItem"); //Szenzorgrafikon Mutatása
            this.saveReportMenuItem.Text = Eszk.GetNyelvSzo("MFsaveReportMenuItem"); //Hardverkonfiguráció Mentése
            this.optionsMenuItem.Text = Eszk.GetNyelvSzo("MFoptionsMenuItem"); //Beállítások
            this.hddMenuItem.Text = Eszk.GetNyelvSzo("MFhddMenuItem"); //Merevlemezek
            this.separatorMenuItem.Text = Eszk.GetNyelvSzo("MFseparatorMenuItem"); //-
            this.startMinMenuItem.Text = Eszk.GetNyelvSzo("MFstartMinMenuItem"); //Főablak Indítása Kis Méretben
            this.hiddenMenuItem.Text = Eszk.GetNyelvSzo("MFhiddenMenuItem"); //Mutasd a Rejtett Szenzorokat Is (Főablak)
            this.columnsMenuItem.Text = Eszk.GetNyelvSzo("MFcolumnsMenuItem"); //Megjelenő Oszlopok
            this.valueMenuItem.Text = Eszk.GetNyelvSzo("MFvalueMenuItem"); //Érték
            this.minMenuItem.Text = Eszk.GetNyelvSzo("MFminMenuItem"); //1min
            this.maxMenuItem.Text = Eszk.GetNyelvSzo("MFmaxMenuItem"); //Maximum
            this.temperatureUnitsMenuItem.Text = Eszk.GetNyelvSzo("MFtemperatureUnitsMenuItem"); //Mértékegység
            this.celsiusMenuItem.Text = Eszk.GetNyelvSzo("MFcelsiusMenuItem"); //Celsius
            this.fahrenheitMenuItem.Text = Eszk.GetNyelvSzo("MFfahrenheitMenuItem"); //Fahrenheit
            this.MenuItem2.Text = Eszk.GetNyelvSzo("MFMenuItem2"); //-
            this.resetMinMaxMenuItem.Text = Eszk.GetNyelvSzo("MFresetMinMaxMenuItem"); //Minimum és Maximum Értékek Nullázása
            this.plotLocationMenuItem.Text = Eszk.GetNyelvSzo("MFplotLocationMenuItem"); //Szenzorgrafikon Elhelyezkedése
            this.plotWindowMenuItem.Text = Eszk.GetNyelvSzo("MFplotWindowMenuItem"); //Saját Ablak
            this.plotBottomMenuItem.Text = Eszk.GetNyelvSzo("MFplotBottomMenuItem"); //Lent Dokkol
            this.plotRightMenuItem.Text = Eszk.GetNyelvSzo("MFplotRightMenuItem"); //Jobbra Dokkol
            this.webMenuItem.Text = Eszk.GetNyelvSzo("MFwebMenuItem"); //Távvezérlő  Webszerver
            this.runWebServerMenuItem.Text = Eszk.GetNyelvSzo("MFrunWebServerMenuItem"); //Futás
            this.serverPortMenuItem.Text = Eszk.GetNyelvSzo("MFserverPortMenuItem"); //Port
            this.menuItem5.Text = Eszk.GetNyelvSzo("MFmenuItem5"); //Hardverek
            this.mainboardMenuItem.Text = Eszk.GetNyelvSzo("MFmainboardMenuItem"); //Alaplap
            this.cpuMenuItem.Text = Eszk.GetNyelvSzo("MFcpuMenuItem"); //CPU
            this.gpuMenuItem.Text = Eszk.GetNyelvSzo("MFgpuMenuItem"); //GPU

            this.fanControllerMenuItem.Text = Eszk.GetNyelvSzo("MFfanControllerMenuItem"); //Ventillátor vezérlők
            this.ramMenuItem.Text = Eszk.GetNyelvSzo("MFramMenuItem"); //RAM
            this.logSensorsMenuItem.Text = Eszk.GetNyelvSzo("MFlogSensorsMenuItem"); //Szenzoradatok Naplózása
            this.logSeparatorMenuItem.Text = Eszk.GetNyelvSzo("MFlogSeparatorMenuItem"); //-
            this.loggingIntervalMenuItem.Text = Eszk.GetNyelvSzo("MFloggingIntervalMenuItem"); //Naplózási Sűrűség
            this.log1sMenuItem.Text = Eszk.GetNyelvSzo("MFlog1sMenuItem"); //1s
            this.log2sMenuItem.Text = Eszk.GetNyelvSzo("MFlog2sMenuItem"); //2s
            this.log5sMenuItem.Text = Eszk.GetNyelvSzo("MFlog5sMenuItem"); //5s
            this.log10sMenuItem.Text = Eszk.GetNyelvSzo("MFlog10sMenuItem"); //10s
            this.log30sMenuItem.Text = Eszk.GetNyelvSzo("MFlog30sMenuItem"); //30s
            this.log1minMenuItem.Text = Eszk.GetNyelvSzo("MFlog1minMenuItem"); //1min
            this.log2minMenuItem.Text = Eszk.GetNyelvSzo("MFlog2minMenuItem"); //2min
            this.log5minMenuItem.Text = Eszk.GetNyelvSzo("MFlog5minMenuItem"); //5min
            this.log10minMenuItem.Text = Eszk.GetNyelvSzo("MFlog10minMenuItem"); //10min
            this.log30minMenuItem.Text = Eszk.GetNyelvSzo("MFlog30minMenuItem"); //30min
            this.log1hMenuItem.Text = Eszk.GetNyelvSzo("MFlog1hMenuItem"); //1h
            this.log2hMenuItem.Text = Eszk.GetNyelvSzo("MFlog2hMenuItem"); //2h
            this.log6hMenuItem.Text = Eszk.GetNyelvSzo("MFlog6hMenuItem"); //6h
            this.menuItem4.Text = Eszk.GetNyelvSzo("MFmenuItem4"); //-
            this.menuItem6.Text = Eszk.GetNyelvSzo("MFmenuItem6"); //Felül Maradó
            this.startupMenuItem.Text = Eszk.GetNyelvSzo("MFstartupMenuItem"); //Indítás a Windowszal
            this.menuItem7.Text = Eszk.GetNyelvSzo("MFmenuItem7"); //
            this.vezerlesMenuItem.Text = Eszk.GetNyelvSzo("MFvezerlesMenuItem"); //VEZÉRLÉS
            this.menuItem8.Text = Eszk.GetNyelvSzo("MFmenuItem8"); //Szabályzólista Létrehozása
            this.menuItem9.Text = Eszk.GetNyelvSzo("MFmenuItem9"); //Szabályzólisták Kezelése
            this.menuItem10.Text = Eszk.GetNyelvSzo("MFmenuItem10"); //-
            this.menuItem11.Text = Eszk.GetNyelvSzo("MFmenuItem11"); //Manuális Vezérlés (Célh)
            this.menuItem12.Text = Eszk.GetNyelvSzo("MFmenuItem12"); //Minden Manuális Vezérlés Feloldása (Célh)
            this.menuItemKilepes.Text = Eszk.GetNyelvSzo("MFmenuItemKilepes"); //Kilépés
            this.menuItemSegitseg.Text = Eszk.GetNyelvSzo("MFmenuItemSegitseg"); //Segítség
            this.menuItem16.Text = Eszk.GetNyelvSzo("MFmenuItem16"); //Használati Útmutató
            this.menuItem17.Text = Eszk.GetNyelvSzo("MFmenuItem17"); //Feverkill
            this.menuItem18.Text = Eszk.GetNyelvSzo("MFmenuItem18"); //Szabályzólisták
            this.menuItem19.Text = Eszk.GetNyelvSzo("MFmenuItem19"); //Direkt Vezérlés

            this.menuItem20.Text = Eszk.GetNyelvSzo("MFmenuItem20"); //Alapértelmezett Fordulatszám
            this.menuItem21.Text = Eszk.GetNyelvSzo("MFmenuItem21"); //-
            this.menuItem22.Text = Eszk.GetNyelvSzo("MFmenuItem22"); //-
            this.menuItem23.Text = Eszk.GetNyelvSzo("MFmenuItem23"); //Visszajelzés Küldése
            this.menuItem24.Text = Eszk.GetNyelvSzo("MFmenuItem24"); //Kapcsolat
            this.menuItem25.Text = Eszk.GetNyelvSzo("MFmenuItem25"); //-
            this.menuItem26.Text = Eszk.GetNyelvSzo("MFmenuItem26"); //Indítási Fordulatszámok Beállítása (Célh)
            this.menuItem13.Text = Eszk.GetNyelvSzo("MFmenuItem13"); //-
            this.menuItem27.Text = Eszk.GetNyelvSzo("MFmenuItem27"); //-
            this.menuItem28.Text = Eszk.GetNyelvSzo("MFmenuItem28"); //Alaplapi Vezérlés
            this.menuItem29.Text = Eszk.GetNyelvSzo("MFmenuItem29"); //Szabályzólisták Összevetése
            this.menuItem30.Text = Eszk.GetNyelvSzo("MFmenuItem30"); //Felül Maradás
            this.menuItem32.Text = Eszk.GetNyelvSzo("MFmenuItem32"); //Frissítések Keresése
            this.menuItem31.Text = Eszk.GetNyelvSzo("MFmenuItem31"); //-
            this.menuItem33.Text = Eszk.GetNyelvSzo("MFmenuItem33"); //Főablak Mutatása/Rejtése
            this.menuItem36.Text = Eszk.GetNyelvSzo("MFmenuItem36"); //-
            this.menuItem37.Text = Eszk.GetNyelvSzo("MFmenuItem37"); //Aktuális Fordulatszámok Mutatása/Rejtése
            this.menuItem34.Text = Eszk.GetNyelvSzo("MFmenuItem34"); //Riasztás Létrehozása
            this.menuItem38.Text = Eszk.GetNyelvSzo("MFmenuItem38"); //-
            this.menuItem35.Text = Eszk.GetNyelvSzo("MFmenuItem35"); //Riasztások Kezelése
            this.menuItem39.Text = Eszk.GetNyelvSzo("MFmenuItem39"); //Hőmérsékletek Mutatása/Rejtése
            this.menuItem40.Text = Eszk.GetNyelvSzo("MFmenuItem40"); //-
            this.menuItem43.Text = Eszk.GetNyelvSzo("MFmenuItem43"); //Riasztások
            this.menuItem42.Text = Eszk.GetNyelvSzo("MFmenuItem42"); //-
            this.menuItem41.Text = Eszk.GetNyelvSzo("MFmenuItem41"); //-
            this.menuItem44.Text = Eszk.GetNyelvSzo("MFmenuItem44"); //Opacitás Beállítása
            this.menuItem45.Text = Eszk.GetNyelvSzo("MFmenuItem45"); //10%
            this.menuItem46.Text = Eszk.GetNyelvSzo("MFmenuItem46"); //20%
            this.menuItem47.Text = Eszk.GetNyelvSzo("MFmenuItem47"); //30%
            this.menuItem48.Text = Eszk.GetNyelvSzo("MFmenuItem48"); //40%
            this.menuItem49.Text = Eszk.GetNyelvSzo("MFmenuItem49"); //50%
            this.menuItem50.Text = Eszk.GetNyelvSzo("MFmenuItem50"); //60%
            this.menuItem51.Text = Eszk.GetNyelvSzo("MFmenuItem51"); //70%
            this.menuItem52.Text = Eszk.GetNyelvSzo("MFmenuItem52"); //80%
            this.menuItem53.Text = Eszk.GetNyelvSzo("MFmenuItem53"); //90%
            this.menuItem54.Text = Eszk.GetNyelvSzo("MFmenuItem54"); //100%
            this.menuItem55.Text = Eszk.GetNyelvSzo("MFmenuItem55"); //Keresés Minden Indításkor
            this.menuItem3.Text = Eszk.GetNyelvSzo("MFmenuItem3"); //Súgó
            this.menuItem57.Text = Eszk.GetNyelvSzo("MFmenuItem57"); //Célhardver Csatlakoztatva
            this.menuItem58.Text = Eszk.GetNyelvSzo("MFmenuItem58"); //-
            this.menuItem59.Text = Eszk.GetNyelvSzo("MFmenuItem59"); //Nyitottakat Előtérbe!
            this.menuItem61.Text = Eszk.GetNyelvSzo("MFmenuItem61"); //Mi ez?

            this.menuItem62.Text = Eszk.GetNyelvSzo("MFmenuItem62"); //1 sec
            this.menuItem63.Text = Eszk.GetNyelvSzo("MFmenuItem63"); //2 sec
            this.menuItem64.Text = Eszk.GetNyelvSzo("MFmenuItem64"); //3 sec (ajánlott)
            this.menuItem65.Text = Eszk.GetNyelvSzo("MFmenuItem65"); //4 sec
            this.menuItem66.Text = Eszk.GetNyelvSzo("MFmenuItem66"); //5 sec
            this.menuItem67.Text = Eszk.GetNyelvSzo("MFmenuItem67"); //6 sec (nagyobb NEM ajánlott) 
            this.menuItem68.Text = Eszk.GetNyelvSzo("MFmenuItem68"); //7 sec
            this.menuItem69.Text = Eszk.GetNyelvSzo("MFmenuItem69"); //8 sec
            this.menuItem70.Text = Eszk.GetNyelvSzo("MFmenuItem70"); //9 sec
            this.menuItem71.Text = Eszk.GetNyelvSzo("MFmenuItem71"); //10 sec
            this.menuItem72.Text = Eszk.GetNyelvSzo("MFmenuItem72"); //12 sec
            this.menuItem73.Text = Eszk.GetNyelvSzo("MFmenuItem73"); //14 sec
            this.menuItem74.Text = Eszk.GetNyelvSzo("MFmenuItem74"); //16 sec
            this.menuItem75.Text = Eszk.GetNyelvSzo("MFmenuItem75"); //18 sec
            this.menuItem76.Text = Eszk.GetNyelvSzo("MFmenuItem76"); //20 sec
            this.menuItem77.Text = Eszk.GetNyelvSzo("MFmenuItem77"); //1,5 sec
            this.menuItem78.Text = Eszk.GetNyelvSzo("MFmenuItem78"); //-
            this.menuItem79.Text = Eszk.GetNyelvSzo("MFmenuItem79"); //0%
            this.menuItem80.Text = Eszk.GetNyelvSzo("MFmenuItem80"); //-
            this.menuItem81.Text = Eszk.GetNyelvSzo("MFmenuItem81"); //Gyorsbillenytűk
            this.menuItem82.Text = Eszk.GetNyelvSzo("MFmenuItem82"); //-
            this.menuItem83.Text = Eszk.GetNyelvSzo("MFmenuItem83"); //-
            this.menuItem84.Text = Eszk.GetNyelvSzo("MFmenuItem84"); //Célhardver Első Használata

            this.menuItem85.Text = Eszk.GetNyelvSzo("MFmenuItem85"); //Célhardver
            this.menuItem87.Text = Eszk.GetNyelvSzo("MFmenuItem87"); //Tutorial Léptetése
            this.menuItem88.Text = Eszk.GetNyelvSzo("MFmenuItem88"); //-
            this.menuItem86.Text = Eszk.GetNyelvSzo("MFmenuItem86"); //Tutorial Mód Aktív
            this.menuItem89.Text = Eszk.GetNyelvSzo("MFmenuItem89"); //-
            this.menuItem56.Text = Eszk.GetNyelvSzo("MFmenuItem56"); //Minden Alaplapi Ventilátor Listaalapú Vezérlésre
            this.menuItem90.Text = Eszk.GetNyelvSzo("MFmenuItem90"); //Minden Alaplapi Ventilátor Eredeti Vezérlésre
            this.menuItem91.Text = Eszk.GetNyelvSzo("MFmenuItem91"); //-
            this.menuItem92.Text = Eszk.GetNyelvSzo("MFmenuItem92"); //Áttekintő Mutatása
            this.menuItem60.Text = Eszk.GetNyelvSzo("MFmenuItem60"); //Frissítési Időköz
            this.menuItem93.Text = Eszk.GetNyelvSzo("MFmenuItem93"); //Hiszterézis
            this.menuItem94.Text = Eszk.GetNyelvSzo("MFmenuItem94"); //-
            this.menuIthisz0.Text = Eszk.GetNyelvSzo("MFmenuIthisz0"); //1°C
            this.menuIthisz1.Text = Eszk.GetNyelvSzo("MFmenuIthisz1"); //1°C
            this.menuIthisz2.Text = Eszk.GetNyelvSzo("MFmenuIthisz2"); //2°C (ajánlott)
            this.menuIthisz3.Text = Eszk.GetNyelvSzo("MFmenuIthisz3"); //3°C
            this.menuIthisz4.Text = Eszk.GetNyelvSzo("MFmenuIthisz4"); //4°C
            this.menuIthisz5.Text = Eszk.GetNyelvSzo("MFmenuIthisz5"); //5°C
            this.menuIthisz0.Text = Eszk.GetNyelvSzo("MFmenuIthisz0"); //0°C
            this.menuItem95.Text = Eszk.GetNyelvSzo("MFmenuItem95"); //Üdvözlőképernyő Megjelenítése Indításkor
            this.menuItem96.Text = Eszk.GetNyelvSzo("MFmenuItem96"); //Nyelv
            this.menuItem97.Text = Eszk.GetNyelvSzo("MFmenuItem97"); //Biztonsági Hőszenzorok Beállítása (Célh)
            this.menuItem99.Text = Eszk.GetNyelvSzo("MFmenuItem99"); //Játszótér
            this.menuItem100.Text = Eszk.GetNyelvSzo("MFmenuItem100"); //Kernel32 HDDinfo Letiltása
            this.menuItem101.Text = Eszk.GetNyelvSzo("MFmenuItem101"); //Licensz Információk
            this.menuItem102.Text = Eszk.GetNyelvSzo("MFmenuItem102"); //Betekintő Megjelenítése
            this.menuItem15.Text = Eszk.GetNyelvSzo("MFmenuItem15"); //Auto Indítás Késleltetése



            this.contextMenuStripTrayicon.SuspendLayout();
            this.kilépésToolStripMenuItem.Text = Eszk.GetNyelvSzo("MFkilépésToolStripMenuItem"); //Kilépés
            this.mutatásRejtésToolStripMenuItem.Text = Eszk.GetNyelvSzo("MFmutatásRejtésToolStripMenuItem"); //Főablak
            this.fordulatszámokToolStripMenuItem.Text = Eszk.GetNyelvSzo("MFfordulatszámokToolStripMenuItem"); //Fordulatszámok
            this.hőmérsékletekToolStripMenuItem.Text = Eszk.GetNyelvSzo("MFhőmérsékletekToolStripMenuItem"); //Hőmérsékletek
            this.felülMaradóToolStripMenuItem.Text = Eszk.GetNyelvSzo("MFfelülMaradóToolStripMenuItem"); //Felül Maradó
            this.előtérbeToolStripMenuItem.Text = Eszk.GetNyelvSzo("MFelőtérbeToolStripMenuItem"); //Nyitottakat Előtérbe!
            this.betekintesToolStripMenuItem.Text = Eszk.GetNyelvSzo("MFbetekintesToolStripMenuItem"); //Betekintés
            this.áttekintőToolStripMenuItem.Text = Eszk.GetNyelvSzo("MFáttekintőToolStripMenuItem"); //Áttekintő
            this.contextMenuStripTrayicon.ResumeLayout(false);



            saveFileDialog.Title = Eszk.GetNyelvSzo("JelentMentDialogCIM");
            treeView.Columns[0].Header = Eszk.GetNyelvSzo("Szenzor");
            treeView.Columns[1].Header = Eszk.GetNyelvSzo("Érték");
        }
    }
}
