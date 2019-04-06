using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OpenHardwareMonitor.Hardware;
using OpenHardwareMonitor;
using UdvozloKepernyo;
using OpenHardwareMonitor.Eszkozok;

namespace OpenHardwareMonitor.GUI
{
    public partial class SzenzorListazo : Form
    {
        static bool Ujlista;
        static bool HivashelyeAlapablakok;
        public SzenzorListazo(bool ujlist, bool hivashelyeAlapablakok)
        {
            InitializeComponent();
            Ujlista = ujlist;
            HivashelyeAlapablakok = hivashelyeAlapablakok;

            if (HivashelyeAlapablakok)
                Attekinto.AttekintoWPF.SajatHelpekMegejenites(false);
            else
            {
                if (Ujlista)
                    SemaKezelo.SajatHelpekMegejenites(false);
                else
                    RiasztKezelo.SajatHelpekMegejenites(false);
            }

            this.SajatHelpekHozzaadas();
            SajatHelpekMegejenites(true);


            Program.OsszesForm.Add(this);
            this.TopMost = Program.KONFFelulMarado;

            listView1.Size = new Size(ClientSize.Width + 2, ClientSize.Height + 2);


            this.Menu = Program.FoAblak.mainMenu.CloneMenu();
            for (int i = 0; i < this.Menu.MenuItems.Count; i++)
            {
                this.Menu.MenuItems[i].Visible = false;
            }
            this.Size = new Size(Width, Height - 39); ;

            //CheckForIllegalCrossThreadCalls = false;
            if (ujlist == true)
                Text = (Program.KONFNyelv == "hun") ? "Új Szabályzólista" : Eszk.GetNyelvSzo("Új Szabályzólista");
            else
                Text = (Program.KONFNyelv == "hun") ? "Új Riasztás" : Eszk.GetNyelvSzo("Új Riasztás");
            //Program.Th1 = new System.Threading.Thread(Frissits);


            if (Program.KONFNyelv != "hun")
                Lokalizalj();

            Frissits();

            frisstmr.Tick += Frisstmr_Tick;
            frisstmr.Start();

            if (Program.SzabListak.Count == 0 && Program.KONFTutorialMegjelenit)
            {
                Program.TutorialWPFAblak.MutassMessagebox("Also note that you can move the help labels by dragging the node on their left side.\n\nIf you click the label text it will collapse. Click the '>' to expand it again.", "Moveable hints");
            }
        }
        void SajatHelpekHozzaadas()
        {
            // SegTestKezelo.SegtestHozzaad("A1", labelCim, "I'm CEO, Bitch...", 70, "Segoe Print", Brushes.Red, false, null, true);

            SegTestKezelo.SegtestHozzaad("SZL1", listView1, "DoubleClick a sensor to continue\n(or close the window)", 55, "Comic Sans MS", System.Windows.Media.Brushes.Red, true, false, new System.Windows.Point(250, 0), true);
            //SegTestKezelo.SegtestHozzaad("A3", labelCelh, "Click here to at/detach the Target Hardware\n(to disable error messages)", 28, "Comic Sans MS", Brushes.White, false, new Point(170, 45), true);
            //SegTestKezelo.SegtestHozzaad("A4", labelFrissId, "Change it to balance\nperformance and sensitivity", 28, "Comic Sans MS", Brushes.White, false, new Point(-249, 45), true);

            //SegTestKezelo.SegtestHozzaad("A5", VillFordszRectHelper, "Click here to open\na manual control window", 28, "Comic Sans MS", Brushes.GreenYellow, false, new Point(-193, -183), true);
            //SegTestKezelo.SegtestHozzaad("A6", VillSemaRectHelper, "Click here to manage Control Schemes", 28, "Comic Sans MS", Brushes.GreenYellow, false, new Point(-331, -169), true);
            //SegTestKezelo.SegtestHozzaad("A7", VillHomRectHelper, "Click here to open\ntemperatures window", 28, "Comic Sans MS", Brushes.GreenYellow, false, new Point(-397, 60), true);
            //SegTestKezelo.SegtestHozzaad("A8", VillRiasztRectHelper, "Click here to manage Alerts", 28, "Comic Sans MS", Brushes.GreenYellow, false, new Point(-380, 14), true);

            //SegTestKezelo.SegtestHozzaad("A9", OldalTutorRectHelper, "Show/Hide hints", 28, "Segoe Print", Brushes.DeepSkyBlue, false, new Point(68, -67), true);
            //SegTestKezelo.SegtestHozzaad("A10", OldalFoablRectHelper, "Show/Hide Advanced window", 28, "Comic Sans MS", Brushes.DeepSkyBlue, false, new Point(107, -71), true);
            //SegTestKezelo.SegtestHozzaad("A11", OldalElrejtRectHelper, "Hide this window", 28, "Comic Sans MS", Brushes.DeepSkyBlue, false, new Point(78, -60), true);
        }
        public static void SajatHelpekMegejenites(bool lathato)
        {
            SegTestKezelo.SetMegjelenites("SZL1", lathato);
        }
        private void Frisstmr_Tick(object sender, EventArgs e)
        {
            Frissits();
        }

        void Lokalizalj()
        {
            this.Nev.Text = Eszk.GetNyelvSzo("Név");
            this.Ertek.Text = Eszk.GetNyelvSzo("Érték");
        }

        List<Program.HoMers> HoMersek;

        Timer frisstmr = new Timer() { Interval = Program.KONFFrisIdo };
        public void Frissits()
        {
            //TUTORIAL
            //if (Ujlista == true)
            //{
            //    if (Tutorial.Statusz == Tutorial.TStat.Szablistak_Letrehoz)
            //        MessageBox.Show((Program.KONFNyelv == "hun") ? "Kattintson duplán arra a hőmérőre, amelyhez szabályzólistát, vagy PID vezérlést kíván létrehozni!\nA két lehetőség közti váltáshoz kattintson a\"Váltás ... vezérlésre\" gombra a következő ablakban." : Eszk.GetNyelvSzo("SzenzListazoTutor"]);
            //}
            try
            {
                string[] Ertekek = Program.FoAblak.computer.Szenzorertekek();

                HoMersek = Program.FoAblak.HommMersek;


                //HoMeRok.listView1.Items.Clear();
                //HoMeRok.listView1.Groups.Clear();
                bool megvan = false;

                int kivalasztott = 0;
                if (listView1.SelectedIndices.Count != 0) kivalasztott = listView1.SelectedIndices[0];

                listView1.Visible = false;
                foreach (Program.HoMers item in HoMersek)
                {
                    for (int i = 0; i < listView1.Items.Count; ++i)
                    {
                        if (listView1.Items[i].Group.Header == item.Csop && listView1.Items[i].Text == item.Nev)
                        {
                            System.Windows.Forms.ListViewItem listViewItemx = new System.Windows.Forms.ListViewItem(new string[] {
                                                                                                                    item.Nev,
                                                                                                                    item.Ertek}, -1);
                            listViewItemx.Group = listView1.Groups[item.Csop];
                            listView1.Items[i] = listViewItemx;

                            megvan = true;
                            break;
                        }
                    }

                    if (!megvan)
                    {
                        System.Windows.Forms.ListViewGroup listViewGroupx = new System.Windows.Forms.ListViewGroup(item.Csop, System.Windows.Forms.HorizontalAlignment.Left);
                        listViewGroupx.Name = item.Csop;
                        listView1.Groups.Add(listViewGroupx);

                        System.Windows.Forms.ListViewItem listViewItemy = new System.Windows.Forms.ListViewItem(new string[] {
                                                                                                                        item.Nev,
                                                                                                                        item.Ertek}, -1);

                        listViewItemy.Group = listView1.Groups[item.Csop];
                        listView1.Items.Add(listViewItemy);
                    }
                }

                try
                {
                    listView1.Items[kivalasztott].Selected = true;
                }
                catch { }

                listView1.Visible = true;
                //foreach (Program.HoMers item in HommMersek)
                //{
                //    System.Windows.Forms.ListViewGroup listViewGroupx = new System.Windows.Forms.ListViewGroup(item.Csop, System.Windows.Forms.HorizontalAlignment.Left);
                //    listViewGroupx.Name = item.Csop;
                //    HoMeRok.listView1.Groups.Add(listViewGroupx);

                //    System.Windows.Forms.ListViewItem listViewItemx = new System.Windows.Forms.ListViewItem(new string[] {
                //                                                                                    item.Nev,
                //                                                                                    item.Ertek}, -1);

                //    listViewItemx.Group = HoMeRok.listView1.Groups[item.Csop];
                //    HoMeRok.listView1.Items.Add(listViewItemx);
                //}
            }
            catch { }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SzenzorListazo_Resize(object sender, EventArgs e)
        {
            listView1.Size = new Size(ClientSize.Width + 2, ClientSize.Height + 2);
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (Ujlista)
            {
                try
                {
                    SajatHelpekMegejenites(false);
                    new SemaSzerkeszto(listView1.SelectedIndices[0]).ShowDialog();
                    SemaSzerkeszto.SajatHelpekMegejenites(false);
                    SajatHelpekMegejenites(true);
                }
                catch { }
            }
            else
            {
                SajatHelpekMegejenites(false);
                new RiasztasLetr(listView1.SelectedIndices[0], HoMersek).ShowDialog();
                SajatHelpekMegejenites(true);
            }
        }

        private void SzenzorListazo_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (HivashelyeAlapablakok)
                Attekinto.AttekintoWPF.SajatHelpekMegejenites(false);
            else
            {
                if (Ujlista)
                {
                    SemaKezelo.SajatHelpekMegejenites(true);
                }
                else
                {
                    RiasztKezelo.SajatHelpekMegejenites(true);
                }
            }

            SajatHelpekMegejenites(false);
        }
    }
}
