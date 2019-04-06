using OpenHardwareMonitor.Eszkozok;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using UdvozloKepernyo;

namespace OpenHardwareMonitor.GUI
{
    public partial class RiasztKezelo : Form
    {
        public RiasztKezelo()
        {
            InitializeComponent();
            Attekinto.AttekintoWPF.SajatHelpekMegejenites(false);
            SajatHelpekHozzaadas();
            SajatHelpekMegejenites(true);


            Program.OsszesForm.Add(this);
            this.TopMost = Program.KONFFelulMarado;
            if (Program.KONFNyelv != "hun")
                Lokalizalj();

            this.Menu = Program.FoAblak.mainMenu.CloneMenu();
            for (int i = 0; i < this.Menu.MenuItems.Count; i++)
            {
                this.Menu.MenuItems[i].Visible = false;
            }
            this.Size = new Size(Width, Height - 39);

            Frissits();
        }
        void SajatHelpekHozzaadas()
        {
            // SegTestKezelo.SegtestHozzaad("A1", labelCim, "I'm CEO, Bitch...", 70, "Segoe Print", Brushes.Red, false, null, true);

            SegTestKezelo.SegtestHozzaad("RK1", listView1, "Click to create a new alert", 55, "Comic Sans MS", System.Windows.Media.Brushes.Red, true, false, new System.Windows.Point(-390, 43), true);
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
            SegTestKezelo.SetMegjelenites("RK1", lathato);
        }
        void Lokalizalj()
        {
            this.Text = Eszk.GetNyelvSzo("RKEZCIM");

            this.hoszenzor.Text = Eszk.GetNyelvSzo("RKEZHoszenzor");
            this.muvelet.Text = Eszk.GetNyelvSzo("RKEZMuvelet");
            this.riasztpont.Text = Eszk.GetNyelvSzo("RKEZRiasztpont");
            this.uzenet.Text = Eszk.GetNyelvSzo("RKEZUzenet");
            this.hangjelzes.Text = Eszk.GetNyelvSzo("RKEZHangjelzes");
            this.specmuvelet.Text = Eszk.GetNyelvSzo("RKEZSpecMuv");
            this.EbresztIdo.Text = Eszk.GetNyelvSzo("RKEZEbresztes");
            this.törlésToolStripMenuItem.Text = Eszk.GetNyelvSzo("Törlés");
            this.újRiasztásToolStripMenuItem.Text = Eszk.GetNyelvSzo("Új Riasztás");
        }
        void Frissits()
        {
            listView1.Items.Clear();

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
                System.Windows.Forms.ListViewItem listViewItemx = new System.Windows.Forms.ListViewItem(new string[] {
                                                                                                        item.Homero,
                                                                                                        item.Muvelet,
                                                                                                        item.RiasztPont.ToString(),
                                                                                                        item.Uzenet,
                                                                                                        item.Hangjelzes? ((Program.KONFNyelv == "hun") ? "IGEN" : Eszk.GetNyelvSzo("IGEN")) : ((Program.KONFNyelv == "hun") ? "NEM" : Eszk.GetNyelvSzo("NEM")),
                                                                                                        SpecMuvSzoveg,
                                                                                                        (item.EbresztIdo > 0)? item.EbresztIdo + ((Program.KONFNyelv == "hun") ? "p" : Eszk.GetNyelvSzo("p")) : "-"}, -1);
                listView1.Items.Add(listViewItemx);
            }
        }
        private void RiasztTorlo_Resize(object sender, EventArgs e)
        {
            try { listView1.Size = new Size(this.Size.Width - 20, this.Size.Height - 50); } catch { }
        }

        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete && MessageBox.Show(((Program.KONFNyelv == "hun") ? "Biztosan törli a kijelölt " : Eszk.GetNyelvSzo("RKEZTorl1")) + listView1.SelectedIndices.Count + ((Program.KONFNyelv == "hun") ? "db riasztást?" : Eszk.GetNyelvSzo("RKEZTorl2")), ((Program.KONFNyelv == "hun") ? "Törlés" : Eszk.GetNyelvSzo("Törlés")), MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                for (int i = listView1.SelectedIndices.Count - 1; i > -1; --i)
                {
                    Program.Riasztasok.RemoveAt(listView1.SelectedIndices[i]);
                }

                Fajlkezelo.RiasztasIr(Program.Riasztasok);
                Program.Attekint.listViewRiaszt.Dispatcher.Invoke(Program.FoAblak.AttekintRiasztFrissit);

                Frissits();
            }
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedIndices.Count != 0)
                törlésToolStripMenuItem.Enabled = true;
            else
                törlésToolStripMenuItem.Enabled = false;

            contextMenuStrip1.Show(new Point(this.Location.X + listView1.Location.X + e.Location.X - 40, this.Location.Y + listView1.Location.Y + e.Location.Y + 8));
        }

        private void törlésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1_KeyUp(12, new KeyEventArgs(Keys.Delete));
        }

        private void újRiasztásToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SajatHelpekMegejenites(false);

            Program.FoAblak.SZListazo2 = new SzenzorListazo(false, false);
            Program.FoAblak.SZListazo2.ShowDialog();
            Frissits();

            SajatHelpekMegejenites(true);
            SzenzorListazo.SajatHelpekMegejenites(false);
        }

        private void RiasztKezelo_FormClosing(object sender, FormClosingEventArgs e)
        {
            SajatHelpekMegejenites(false);
            Attekinto.AttekintoWPF.SajatHelpekMegejenites(true);
        }
    }
}
