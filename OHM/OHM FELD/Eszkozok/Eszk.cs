using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenHardwareMonitor.Eszkozok
{
    class Eszk
    {
        static public void GetFullVersion()
        {
            try
            {
                System.Diagnostics.Process.Start(Program.SzerverDomain + "/register/?feladat=getpremiumszoftverbol#tologin");
            }
            catch { }
        }
        static public bool IsPremiumFuncEabled()
        {
            return (Program.LICENSZTipus == 20 || Program.LICENSZProbTeljVerz);
        }
        static public void Elnevezo()
        {
            string[] verzszamdarabok = Program.Verzioszam.Split('.');
            string kiirandoverzszam = verzszamdarabok[0] + "." + verzszamdarabok[1] + "." + verzszamdarabok[2];

            Program.Attekint.labelFrissId.Dispatcher.Invoke(delegate () { Program.Attekint.labelFrissId.Content = ((Program.KONFNyelv == "hun") ? "Frissítési időköz: " : Eszk.GetNyelvSzo("ATTEKUIFrissido") + ": ") + ((double)Program.KONFFrisIdo / (double)1000).ToString() + " sec"; });
            Program.Attekint.labelCelh.Dispatcher.Invoke(delegate () { Program.Attekint.labelCelh.Content = ((Program.KONFNyelv == "hun") ? ("Célhardver: ") : (Eszk.GetNyelvSzo("Celhardver") + ": ")) + ((Program.KONFVanVezerlo) ? ((Program.FoAblak.NTFYI_szamlalo > Program.FoAblak.NTFYMax) ? ((Program.KONFNyelv == "hun") ? ("Hiba!") : Eszk.GetNyelvSzo("Hiba!")) : ("OK")) : ((Program.KONFNyelv == "hun") ? ("Leválasztva") : Eszk.GetNyelvSzo("Levalasztva"))); });

            Program.FoAblak.SysTrayicon.Text = "Feverkill   V" + kiirandoverzszam + "  (" + (double)Program.KONFFrisIdo / (double)1000 + " sec)";
            Program.FoAblak.Text = ((Program.KONFNyelv == "hun") ? "Haladó" : Eszk.GetNyelvSzo("Halado")) + " - Feverkill     V" + kiirandoverzszam /*+ "  (" + (double)Program.KONFFrisIdo / (double)1000 + " sec)"*/;
            if (/*Program.AzonnaliVez ||*/ !Program.KONFVanVezerlo)
            {
                Program.FoAblak.Text += "    --";
                Program.FoAblak.SysTrayicon.Text += "   --";
            }

            //if (Program.AzonnaliVez)
            //    MF.SysTrayicon.Text = MF.Text += "DIREKT!";

            //if (Program.AzonnaliVez && !Program.KONFVanVezerlo)
            //    MF.SysTrayicon.Text = MF.Text += "  ||  ";

            if (!Program.KONFVanVezerlo)
            {
                Program.FoAblak.Text += (Program.KONFNyelv == "hun") ? "LEVÁLASZTVA!" : (Eszk.GetNyelvSzo("LEVALASZTVA") + "!");
                Program.FoAblak.SysTrayicon.Text += (Program.KONFNyelv == "hun") ? "LEVÁLASZTVA!" : (Eszk.GetNyelvSzo("LEVALASZTVA") + "!");
            }
        }
        static public void CreateTOBBIGetFullVersionMenuitem()
        {//Ez csak a "többi" menuitemet csinálja meg, az első már az induláskor (FoAblak.cs) elkészült
            try
            {
                if (Program.LICENSZTipus != 20)
                {
                    MenuItem Menitx;
                    string szovegplusz = "....";

                    for (int i = 0; i < 29; i++)
                    {
                        Menitx = new MenuItem();
                        Menitx.Text = "Get Full Version " + szovegplusz;
                        Menitx.Click += new System.EventHandler(delegate (object sender, EventArgs e) { GetFullVersion(); });
                        Program.FoAblak.menuItemSegitseg.MenuItems.Add(Menitx);
                        szovegplusz += ".";
                    }

                    Program.FoAblak.menuItem14.Visible = true;
                }
            }
            catch
            { }
            Program.FoAblak.GetFullVersionCreateKellMeg = false;
        }

        public static string GetNyelvSzo(string index)
        {
            try
            {
                return Program.LocDic[index];
            }
            catch
            { }
            return "<" + index + "> >>!Missing!";
        }
    }
}
