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
    public partial class Homerok : Form
    {
        public Homerok()
        {
            InitializeComponent();
            SajatHelpekHozzaadas();

            Program.OsszesForm.Add(this);
            checkBox1.Checked = this.TopMost = Program.KONFFelulMarado;
            trackBar1.Value = (int)(Program.FoAblak.Opacity * 100);

            this.Menu = Program.FoAblak.mainMenu.CloneMenu();
            for (int i = 0; i < this.Menu.MenuItems.Count; i++)
            {
               this.Menu.MenuItems[i].Visible = false; 
            }

            this.Size = new Size(Program.KONFHomerokMeret.Width, Program.KONFHomerokMeret.Height-39);
            listView1.Size = new Size(ClientSize.Width + 2, ClientSize.Height + 2);
            CheckForIllegalCrossThreadCalls = false;

            if (Program.KONFNyelv != "hun")
                Lokalizalj();
        }
        void SajatHelpekHozzaadas()
        {
            // SegTestKezelo.SegtestHozzaad("A1", labelCim, "I'm CEO, Bitch...", 70, "Segoe Print", Brushes.Red, false, null, true);

            SegTestKezelo.SegtestHozzaad("Hm1", checkBox1, "Toggle always on top", 28, "Comic Sans MS", System.Windows.Media.Brushes.CornflowerBlue, false, false, new System.Windows.Point(-242, 30), true);
            SegTestKezelo.SegtestHozzaad("Hm2", trackBar1, "Adjust opacity", 28, "Comic Sans MS", System.Windows.Media.Brushes.CornflowerBlue, false, false, new System.Windows.Point(-179, -113), true);
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
            SegTestKezelo.SetMegjelenites("Hm1", lathato);
            SegTestKezelo.SetMegjelenites("Hm2", lathato);
        }

        void Lokalizalj()
        {
            this.Text = Eszk.GetNyelvSzo("HomerokCIM");
            this.Nev.Text = Eszk.GetNyelvSzo("Név");
            this.Ertek.Text = Eszk.GetNyelvSzo("Érték");
        }
        private void SzenzorListazo_Resize(object sender, EventArgs e)
        {

            listView1.Size = new Size(ClientSize.Width + 2, ClientSize.Height + 2);
            Program.KONFHomerokMeret = this.Size;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count != 0)
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    item.BackColor = SystemColors.Window;
                    item.ForeColor = SystemColors.WindowText;
                }

                listView1.Items[listView1.SelectedIndices[0]].BackColor = Color.Silver;
                listView1.Items[listView1.SelectedIndices[0]].ForeColor = Color.White;
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            Program.FoAblak.KMutato.Opacity = Program.FoAblak.Opacity = this.Opacity = Program.KONFOpacitas = (double)trackBar1.Value / (double)100;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(Program.FoAblak.TopMost != checkBox1.Checked)
                Program.FoAblak.menuItem6.PerformClick();
        }

        private void listView1_SizeChanged(object sender, EventArgs e)
        {
            trackBar1.Location = new Point(trackBar1.Location.X, this.Size.Height - 63);
            checkBox1.Location = new Point(checkBox1.Location.X, this.Size.Height - 59);
        }
        public void ShowHelppel()
        {
            Show();
            SajatHelpekMegejenites(true);
        }
        public void HideHelppel()
        {
            SajatHelpekMegejenites(false);
            Hide();
        }
    }
}
