using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;
using System.Diagnostics;
using OpenHardwareMonitor.Eszkozok;

namespace OpenHardwareMonitor.GUI
{
    public partial class Frissito : Form
    {
        bool szallepjki;
        WebClient WCfo;
        bool FelhAltalKeresve;
        public Frissito(bool felhAltalKeresve)
        {

            FelhAltalKeresve = felhAltalKeresve;

            InitializeComponent();
            Program.OsszesForm.Add(this);
            this.TopMost = Program.KONFFelulMarado;

            if (!FelhAltalKeresve)
            {
                Opacity = 0;
                ShowInTaskbar = false;
            }

            if (Program.KONFNyelv != "hun")
                Lokalizalj();
            CheckForIllegalCrossThreadCalls = false;
            szallepjki = false;


            new Thread(PBarJatek) { Priority = ThreadPriority.Highest }.Start();

            //WCverzio.DownloadFileAsync(new Uri("https://docs.google.com/document/export?format=txt&id=1mWbX1Q2a1pj4bn92Ik_6h-3J1g6yj3s65-IodvcWkx8&token=AC4w5Vgug53JnSJY-EAzG0FvUpRYBmeI_Q%3A1427223457154"), "vinf.vif");

            new Thread(VanUjVerzTeszt).Start();
        }

        void Lokalizalj()
        {
            label1.Text = Eszk.GetNyelvSzo("FrissitoLabelKeres");
            buttonMegse.Text = Eszk.GetNyelvSzo("FrissitoMegse");
        }

        private void PBarJatek()
        {
            while (!szallepjki)
            {
                for (int i = 0; i < 100 && !szallepjki; i += 2)
                {
                    progressBar1.Value = i;
                    progressBar1.Refresh();
                    Thread.Sleep(22);
                }
                Thread.Sleep(350);
                for (int i = 100; i > 0 && !szallepjki; i -= 2)
                {
                    progressBar1.Value = i;
                    progressBar1.Refresh();
                    Thread.Sleep(22);
                }
            }
        }

        bool letoltesMegszakitva = false;
        private void buttonMegse_Click(object sender, EventArgs e)
        {
            szallepjki = true;
            letoltesMegszakitva = true;
            try
            {
                WCfo.CancelAsync();
            }
            catch { }
            try { Close(); }
            catch { }
        }


        private void LETOLTOCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //MessageBox.Show(((Program.KONFNyelv == "hun") ? "Frissítés letöltve.\nMiután az OK gombra kattint, a vezérlőszoftver bezárul\nés a frissítés befejeződéséig nem lesz használható." : Eszk.GetNyelvSzo("FrissitoLetolveMboxSZOVEG"]), ((Program.KONFNyelv == "hun") ? "Letöltés Kész!" : Eszk.GetNyelvSzo("FrissitoLetolveMboxCIM"]), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

            if (!letoltesMegszakitva)//Mert a WCfo.CancelAsync() -nél is meghívódik ez az eseménykezelő
            {
                try
                {
                    Process.Start("Frissito.exe");
                    Environment.Exit(19981001);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), ((Program.KONFNyelv == "hun") ? "Hiba Történt!" : Eszk.GetNyelvSzo("Hiba!")), MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void VanUjVerzTeszt()
        {
            try
            {
                string valasz = Vedelem.RequestGET(Program.SzerverDomain + "/verziok/FrissitoHost.php?hostmuv=legujverzioleker");

                string verzbe = valasz.Split('\n')[0];
                string URL = valasz.Split('\n')[1];

                if (Vteszt(verzbe))
                {
                    //UJABB VERZIO VAN
                    if (MessageBox.Show(((Program.KONFNyelv == "hun") ? "Új programverzió áll rendelkezésre: " + verzbe + "\nLetöltse és telepítse most a program?" : Eszk.GetNyelvSzo("FrissitoUjMboxSZOVEG") + "\n" + verzbe), ((Program.KONFNyelv == "hun") ? "Új Verzió Észlelve!" : Eszk.GetNyelvSzo("FrissitoUjMboxCIM")), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        Opacity = 100;
                        ShowInTaskbar = true;

                        szallepjki = true;
                        label1.Text = ((Program.KONFNyelv == "hun") ? "Frissítés letöltése..." : Eszk.GetNyelvSzo("FrissitoLabelLetoltes"));
                        WCfo = new WebClient();
                        WCfo.DownloadFileCompleted += new AsyncCompletedEventHandler(LETOLTOCompleted);
                        WCfo.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                        WCfo.DownloadFileAsync(new Uri(URL), "Frissito.exe");
                    }
                    else
                    {
                        Program.naprakesz = false;
                        szallepjki = true;
                        try { Close(); }
                        catch { }
                    }
                }
                else
                {
                    Program.naprakesz = true;
                    if (FelhAltalKeresve)
                    {
                        MessageBox.Show(((Program.KONFNyelv == "hun") ? "A program naprakész!" : Eszk.GetNyelvSzo("FrissitoNaprakesz")) + "\n\n                           V" + Program.Verzioszam, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    szallepjki = true;
                    try { Close(); }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                Program.hibafrissiteskor = ex.Message;
                if (FelhAltalKeresve)
                    MessageBox.Show("Hiba történt a frissítések keresése során!\n(" + ex.Message.ToString() + ")", "Hiba a Kapcsolatban!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                szallepjki = true;
                try { Close(); }
                catch { }
            }

        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }
        bool Vteszt(string VerzBeolvasott)
        {
            string[] UJverz = VerzBeolvasott.Split('.');
            string[] REGverz = Program.Verzioszam.Split('.');

            for (int i = 0; i < UJverz.Length && i < REGverz.Length; ++i)
            {
                if (int.Parse(UJverz[i]) > int.Parse(REGverz[i]))
                {
                    return true;
                }
                if (int.Parse(UJverz[i]) < int.Parse(REGverz[i]))
                {
                    return false;
                }
            }

            if (UJverz.Length > REGverz.Length)
            {
                return true;
            }

            return false;
        }
    }
}
