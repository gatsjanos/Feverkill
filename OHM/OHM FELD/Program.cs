/*
 
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
 
  Copyright (C) 2009-2013 Michael Möller <mmoeller@openhardwaremonitor.org>
	
*/

#define VANVEZERLO

using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using OpenHardwareMonitor.GUI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Text;
using System.ComponentModel;
using System.Management;
using System.Linq;
using OpenHardwareMonitor.Eszkozok;

namespace OpenHardwareMonitor
{
    public static class Program
    {
        ////////////////// ˇˇ BEMUTATO/DEBUG BEALLITASOK ˇˇ ///////////////////////////////////////////////////////////////////////////////////////

        public const bool DEBUG_gyorsinditas = false, Soros8BitesKodolas = false, CsakEgyPeldanyEgyidoben = false;
        public static byte UARTKod100 = 0; //HOZZÁADVA A KOMMUNIKÁCIÓKEZDŐ KÓDOKHOZ. 0, VAGY 100. 7 BITES, ÉS 8 BITES KÓDOLÁS. 8 BITESHEZ UARTKod100 = 100, 7 BITESHEZ UARTKod100 = 0 
        public enum SzenzorBemenet { BelsoSzenzor, KulsoSzenzor, EmulaltSzenzor }
        public enum BelsoVentiTipus { Hardware, Emulalt }
        ////----------------DEVFORM---------------------
        public static SzenzorBemenet BEMUTATO_SzenzBemenet = SzenzorBemenet.BelsoSzenzor;
        public static BelsoVentiTipus BelsoVentiVezTip = BelsoVentiTipus.Hardware;
        public static bool DEVRandomDefSpeeds = false, DEVTopMost = true;
        public static bool DEVCOMPortKotese = false, DEVCOMReconnect = false;
        public static string DEVKotottCOMPort = "COM10";
        public static int DEVEmulaltVentiszam = 3;
        ////----------------=======---------------------
        public static List<EmulaltBelsoVenti> EmulaltBelsoVentik = new List<EmulaltBelsoVenti>();
        public class EmulaltBelsoVenti
        {
            public EmulaltBelsoVenti()
            { }

            public Hardware.ControlMode ControlMode;
            public string csoport, nev;
            public byte ertek;
        }
        ////////////////////////////// ^^ DEBUG BEALLITASOK VÉGE ^^ //////////////////////////////////////////////////////////////////////////////

        public static bool VanSupervisor = false;

        public static Thread HomersKuldTH;
        public static bool Dolgozott, ErvenyVanIras, ErvenyVanOlvasas, SorosKuldes, RiasztHangMegy = false, HisztDeaktiv = false;
        public static Program.SzabLista SZLIST_SZERK_MENT = null;
        public static List<Program.SzabLista> SzabListak, Ervenyesek;
        public static List<Program.Riasztas> Riasztasok;
        //public static List<string> Szenzorok;
        public static UserOption KitTenyMutNyitva, HomerokNyitva;
        public static System.Media.SoundPlayer RiasztHang = new System.Media.SoundPlayer("Rsx\\riaszt.wav");
        public static string[] CsatCimkekCelh = new string[8];
        public static Dictionary<string, string> CsatCimkekBelso = new Dictionary<string, string>();

        #region Konfigurációs Változók
        public static double KONFOpacitas = 1;
        public static bool KONFFrissitesInditaskor = true, KONFFelulMarado = true, KONFHomersMutat = true, KONFKittenyMutat = false, KONFKismeretIndit = true,
                           KONFVanVezerlo = false, KONFAutoIndul = true, KONFAttekintMutat = true, KONFUdvKeperny = true, KONFHDDKernel32Tiltas = false,
                           KONFKellMegNyelvvalasztas = true, KONFTutorialMegjelenit = true, KONFBetekintoMutat = true, KONFMindenPCIEszkBetolt = false;
        public static Size KONFHomerokMeret;
        public static float KONFHiszterezis = 2;
        public static int KONFAutoIndKesleltetes = 16000;

        public static int KONFFrisIdo = 4000, KONFBootFrisIdo = 1000, KONFBootIteracioSzam = 4;
        //public static int KONFFrisIdo
        //{
        //    get
        //    {
        //        return KONFFrisIdoerteke;
        //    }
        //    set
        //    {
        //        KONFFrisIdoerteke = value;
        //        try
        //        {
        //            FoAblak.timer.Interval = value;
        //        }
        //        catch
        //        { }
        //    }
        //}

        public static List<object> KIIRTKONF = new List<object>();
        public static string KONFNyelv = "en";
        #endregion

        public static SerialPort SorosPort = new SerialPort(".", 9600, Parity.None, 8, StopBits.One);
        static bool elsovissza = false, masodikvissza = false, rendben = true, sikeresport = false;

        public static bool? naprakesz = null;
        public static bool frissitve = false;
        public static string hibafrissiteskor;

        public const string SzerverDomain = "https://www.feverkill.com";

        public static Aga.Controls.Tree.TreeViewAdv TreeViewKLONOZOTT;

        public static Dictionary<string, string> LocDic = new Dictionary<string, string>();

        public static FoAblak FoAblak;
        public static Attekinto.AttekintoWPF Attekint;

        public static List<Form> OsszesForm = new List<Form>();

        public static Random RandomObject = new Random();

        public static UdvozloKepernyo.TutorWPFAblak TutorialWPFAblak;
        public static bool TutorialRiasztasBemutatasjon = false;
        public static bool TutorialSzenzgrafBemutatasjon = false;

        public static string LICENSZNev = "", LICENSZEmail = "", LICENSZID = "", LICENSZJelszo = "", LICENSZERVENYESSEG = ""; //Védelem és hitelesítés későbbiekben
        public static int LICENSZTipus = 20;//Freemium: 10, Teljesverz: 20
        public static bool LICENSZProbTeljVerz = false;//Teljes verziós próbaidő

        public static Stopwatch FelallasOtaEltelIdo = new Stopwatch();

        public const int MENTVERZSZAM_fokonf = 5, MENTVERZSZAM_listak = 6, MENTVERZSZAM_riaszt = 4, MENTVERZSZAM_csatcimke = 2, MENTVERZSZAM_HitAdat = 2;
        public const string Verzioszam = "1.8.9.1";

        public struct Riasztas
        {
            public string Homero, Uzenet, Muvelet; //"<", "=", ">"
            public string SpecMuvelet; //h: hibernálás, l: leállítás, u: újraindítás, n: nincs művelet , a: alvó állapot
            public int RiasztPont, EbresztIdo;
            public bool Hangjelzes;

        }
        public struct HoMers
        {
            public string Csop, Nev, Ertek;
        }
        public class SzabLista
        {
            public bool VezTipListaalapu = true;
            public string Nev = "", Csatornak = "", Homero = "";
            public byte[] PWM = new byte[46];
            public PIDKezelo PIDObjektum = new PIDKezelo(250, 0, 0, 0, 0, 0);

            public int GetAlaplapiControlDarabszam()
            {
                int mennyiseg = 0;
                string kezdemeny = "";
                foreach (char c in Csatornak)
                {
                    switch (c)
                    {
                        case ' ':
                            if (kezdemeny == "")
                            {
                                kezdemeny += c;
                            }
                            else if (kezdemeny == " =>")
                            {
                                ++mennyiseg;
                                kezdemeny = "";
                            }
                            else
                                kezdemeny = "";
                            break;
                        case '=':
                            if (kezdemeny == " ")
                                kezdemeny += c;
                            else
                                kezdemeny = "";
                            break;
                        case '>':
                            if (kezdemeny == " =")
                                kezdemeny += c;
                            else
                                kezdemeny = "";
                            break;
                        default:
                            kezdemeny = "";
                            break;
                    }
                }
                return mennyiseg;
            }
        }

        [STAThread]
        public static void Main(string[] args)
        {
#if !DEBUG
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += delegate { Environment.Exit(101); };//A windows-os dialogbox nem jelenik meg, így a Supervisor újra tud indítani
            AppDomain.CurrentDomain.UnhandledException += delegate { Environment.Exit(102); };
#endif

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += delegate { Environment.Exit(101); };//A windows-os dialogbox nem jelenik meg, így a Supervisor újra tud indítani
            AppDomain.CurrentDomain.UnhandledException += delegate { Environment.Exit(102); };

            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();

            VanSupervisor = args.Contains("SUPERVISOR");

            if (!VanSupervisor)
            {
                try
                {
                    string argstring = "";
                    foreach (var item in args)
                    {
                        argstring += item + " ";
                    }

                    Process.Start("FeverkillSupervisor.exe", argstring);
                    Environment.Exit(19981001);
                }
                catch
                { }
            }


            if (args.Contains("FRISSITVE"))
                frissitve = true;

            for (int i = 0; i < CsatCimkekCelh.Length; i++)
            {
                CsatCimkekCelh[i] = "--";
            }

            Fajlkezelo.LstkTeszt();

            SzabListak = Fajlkezelo.SZabListBeolvas();
            Ervenyesek = Fajlkezelo.ErvenyListBeolvas();
            Riasztasok = Fajlkezelo.RiasztasBeolvas();
            CsatCimkekCelh = Fajlkezelo.CsCimkeBeolvas();
            Fajlkezelo.HitelesitoAdatTitkositATLANBeolvas();

            SorosPort.Encoding = System.Text.Encoding.GetEncoding(28591); //8 bites karakterek
            if (Soros8BitesKodolas)
            {
                UARTKod100 = 100;
            }

            KONFHomerokMeret = new Size(252, 420);
            #region Főkonfigurációsfájl Beolvasása
            {
                Fajlkezelo.FoKonfBeolvas();
                Fajlkezelo.KiirtKONFSync();
            }
            Seged.HDDKernel32Tiltas = KONFHDDKernel32Tiltas;
            Seged.MindenPCIEszkBetolt = KONFMindenPCIEszkBetolt;
            #endregion

            if (args.Contains("AUTOSTART"))
            {
                Thread.Sleep(KONFAutoIndKesleltetes);
            }

            try
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Program.KONFNyelv);
            }
            catch
            { }

            Fajlkezelo.NyelvBeolvas(Program.KONFNyelv);

            if (KONFFrissitesInditaskor)
            {
                try
                {
                    new Frissito(false).Show();
                }
                catch
                { }
            }
            if (KONFUdvKeperny)
            {
                 Thread THr = new Thread(delegate ()
               {
                   if (Program.KONFNyelv == "hun")
                       UdvozloKepernyo.UdvKepernyokMegjelenito.Inicializalas(Verzioszam, LICENSZNev, LICENSZEmail, LICENSZID, LICENSZERVENYESSEG, Program.KONFNyelv, "", "");
                   else
                       UdvozloKepernyo.UdvKepernyokMegjelenito.Inicializalas(Verzioszam, LICENSZNev, LICENSZEmail, LICENSZID, LICENSZERVENYESSEG, Program.KONFNyelv, Eszk.GetNyelvSzo("Vezérlőszoftver"), Eszk.GetNyelvSzo("Licenszelve:"));
               });
                THr.SetApartmentState(ApartmentState.STA);
                THr.Priority = ThreadPriority.Highest;
                THr.Start();
            }

            if (CsakEgyPeldanyEgyidoben)
            {
                int hanypeldanyfut = HanyPeldanyFut(Directory.GetCurrentDirectory(), "Feverkill.exe");
                for (int i = 0; hanypeldanyfut > 1 && i < 30; ++i)
                {
                    Thread.Sleep(100);
                    hanypeldanyfut = HanyPeldanyFut(Directory.GetCurrentDirectory(), "Feverkill.exe");

                }
                if (hanypeldanyfut > 1)
                {
                    MessageBox.Show("A vezérlőszoftver már fut!\nEgyszerre csak egy példány futhat.\n\n\nThe Control Software is already running!", "A szoftver már fut!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    Environment.Exit(19981001);
                }
            }

            if (BEMUTATO_SzenzBemenet == SzenzorBemenet.KulsoSzenzor)
                KulsoHoszenzor.Kapcsolodas();

            if (!AllRequiredFilesAvailable())
                Environment.Exit(19981001);


            //Program.Szenzorok = new List<string>();
            Ervenyesek = new List<SzabLista>();
            Dolgozott = false;
            SorosKuldes = ErvenyVanIras = ErvenyVanOlvasas = false;

            /*AzonnaliVez = false;*/

            new FoAblak();
            //FoAblak.FormClosed += delegate(Object sender, FormClosedEventArgs e)
            //{
            //    Application.Exit();
            //};
            while (true)
            {
                try { Application.Run(); }
                catch { }
                Program.HisztDeaktiv = true;
            }
        }
       // public static Thread THr;
        static int HanyPeldanyFut(string mappa, string fajlnev)
        {
            int db = 0;

            var wmiQueryString = "SELECT ProcessId, ExecutablePath, CommandLine FROM Win32_Process";
            using (var searcher = new ManagementObjectSearcher(wmiQueryString))
            using (var results = searcher.Get())
            {
                var query = from p in Process.GetProcesses()
                            join mo in results.Cast<ManagementObject>()
                            on p.Id equals (int)(uint)mo["ProcessId"]
                            select new
                            {
                                Process = p,
                                Path = (string)mo["ExecutablePath"],
                                CommandLine = (string)mo["CommandLine"],
                            };
                foreach (var item in query)
                {
                    try
                    {
                        if (item.Path.StartsWith(mappa + "\\" + fajlnev))
                        {
                            ++db;
                        }
                    }
                    catch { }
                }
            }
            return db;
        }
        private static bool IsFileAvailable(string fileName)
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath) +
              Path.DirectorySeparatorChar;

            if (!File.Exists(path + fileName))
            {
                MessageBox.Show("This file cannot be found: " + fileName +
                  "\nYou should try to reinstall the software!", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private static bool AllRequiredFilesAvailable()
        {
            if (!IsFileAvailable("Aga.Controls.dll"))
                return false;
            if (!IsFileAvailable("OpenHardwareMonitorLib.dll"))
                return false;
            if (!IsFileAvailable("OxyPlot.dll"))
                return false;
            if (!IsFileAvailable("OxyPlot.WindowsForms.dll"))
                return false;
            if (!IsFileAvailable("Rsx\\riaszt.wav"))
                return false;
            if (!IsFileAvailable("ebreszt.exe"))
                return false;
            if (!IsFileAvailable("UdvozloKepernyo.exe"))
                return false;

            return true;
        }

        private static void ReportException(Exception e)
        {
            CrashForm form = new CrashForm();
            form.Exception = e;
            form.ShowDialog();
        }

        public static void Application_ThreadException(object sender,
          ThreadExceptionEventArgs e)
        {
            try
            {
                ReportException(e.Exception);
            }
            catch
            {
            }
            finally
            {
                Application.Exit();
            }
        }

        public static void CurrentDomain_UnhandledException(object sender,
          UnhandledExceptionEventArgs args)
        {
            try
            {
                Exception e = args.ExceptionObject as Exception;
                if (e != null)
                    ReportException(e);
            }
            catch
            {
            }
            finally
            {
                Environment.Exit(0);
            }
        }
    }
}
