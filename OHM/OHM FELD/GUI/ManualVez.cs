using OpenHardwareMonitor.Eszkozok;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OpenHardwareMonitor
{
    public partial class ManualVez : Form
    {
        OpenHardwareMonitor.GUI.FoAblak MF;
        GUI.KitTenyMutato KMutato;
        public ManualVez(OpenHardwareMonitor.GUI.FoAblak MFbe, GUI.KitTenyMutato KMutatobe)
        {
            InitializeComponent();
            Program.OsszesForm.Add(this);
            this.TopMost = Program.KONFFelulMarado;

            if(!Program.KONFFelulMarado)
            {
                this.TopMost = true;
                this.TopMost = false;
            }

            if (Program.KONFNyelv != "hun")
                this.Text = Eszk.GetNyelvSzo("Manuális Vezérlés - Célhardver");

            MF = MFbe;
            KMutato = KMutatobe;

            this.Menu = MF.mainMenu.CloneMenu();
            for (int i = 0; i < this.Menu.MenuItems.Count; i++)
            {
                this.Menu.MenuItems[i].Visible = false;
            }
            this.Size = new Size(Width, Height - 39); ;

            numericUpDown1.Value = trackBar1.Value = MF.KitTenyezok[0];
            numericUpDown2.Value = trackBar2.Value = MF.KitTenyezok[1];
            numericUpDown3.Value = trackBar3.Value = MF.KitTenyezok[2];
            numericUpDown4.Value = trackBar4.Value = MF.KitTenyezok[3];
            numericUpDown5.Value = trackBar5.Value = MF.KitTenyezok[4];
            numericUpDown6.Value = trackBar6.Value = MF.KitTenyezok[5];
            numericUpDown7.Value = trackBar7.Value = MF.KitTenyezok[6];
            numericUpDown8.Value = trackBar8.Value = MF.KitTenyezok[7];

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            numericUpDown1.Value = trackBar1.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            numericUpDown2.Value = trackBar2.Value;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            numericUpDown3.Value = trackBar3.Value;
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            numericUpDown4.Value = trackBar4.Value;
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            numericUpDown5.Value = trackBar5.Value;
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            numericUpDown6.Value = trackBar6.Value;
        }

        private void trackBar7_Scroll(object sender, EventArgs e)
        {
            numericUpDown7.Value = trackBar7.Value;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            trackBar1.Value = (int)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            trackBar2.Value = (int)numericUpDown2.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            trackBar3.Value = (int)numericUpDown3.Value;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            trackBar4.Value = (int)numericUpDown4.Value;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            trackBar5.Value = (int)numericUpDown5.Value;
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            trackBar6.Value = (int)numericUpDown6.Value;
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            trackBar7.Value = (int)numericUpDown7.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Program.DirektKuld = true;
            //for (int i = 0; i < 10 && Program.FolyamKuldes == true; ++i)
            //{
            //    System.Threading.Thread.Sleep(50);
            //}

            //Program.AzonnaliVez = true;
            //byte [] kuldendo = new byte[]{(byte)numericUpDown1.Value, (byte)numericUpDown2.Value, (byte)numericUpDown3.Value, (byte)numericUpDown4.Value, (byte)numericUpDown5.Value, (byte)numericUpDown6.Value, (byte)numericUpDown7.Value, (byte)numericUpDown8.Value};
            //Enabled = false;

            //for (int i = 0; i < kuldendo.Length; i++)
            //{
            //    MF.KitTenyezok[i] = kuldendo[i];
            //}

            //bool siker = false;
            //    for (int i = 0; i < 2; ++i)
            //    {
            //            if (Fajlkezelo.UARTbajtKuldo(107, kuldendo))
            //            {
            //                siker = true;
            //                break;
            //            }
            //    }
            //    if (!siker)
            //    {
            //        MF.Kezfogas(false);

            //        for (int i = 0; i < 2; ++i)
            //        {
            //            if (Fajlkezelo.UARTbajtKuldo(107, kuldendo))
            //            {
            //                siker = true;
            //                break;
            //            }
            //        }
            //    }
            //    if (!siker)
            //    {
            //        //MessageBox.Show("A feltöltés sikertelen!\nPróbálja meg újra!", "Sikertelen Feltöltés", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            //        //Program.AzonnaliVez = false;
            //        MF.SysTrayicon.ShowBalloonTip(3000, "Hiba a kapcsolatban!", "A ventilátor-fordulatszámok\nküldése nem sikerült!\nAz értékek feltehetően\nnem megfelelőek a célhardveren.", ToolTipIcon.Error);
            //    }
            //    else
            //    {

            //        KMutato.label3.Text = kuldendo[0].ToString() + "%";
            //        KMutato.label4.Text = kuldendo[1].ToString() + "%";
            //        KMutato.label5.Text = kuldendo[2].ToString() + "%";
            //        KMutato.label6.Text = kuldendo[3].ToString() + "%";
            //        KMutato.label7.Text = kuldendo[4].ToString() + "%";
            //        KMutato.label8.Text = kuldendo[5].ToString() + "%";
            //        KMutato.label9.Text = kuldendo[6].ToString() + "%";
            //        KMutato.label10.Text = kuldendo[7].ToString() + "%";
            //    }

            //        MF.menuItem11.Checked = true;
            //        MF.menuItem57.Checked = true;
            //        Program.KONFVanVezerlo = true;
            //        Fajlkezelo.Elnevezo(MF);
            //        Program.DirektKuld = false;
            //Enabled = true;


            if (checkBox1.Checked)
            {
                MF.DirektVez[0] = true;
                MF.KitTenyezok[0] = (byte)numericUpDown1.Value;
            }
            if (checkBox2.Checked)
            {
                MF.DirektVez[1] = true;
                MF.KitTenyezok[1] = (byte)numericUpDown2.Value;
            }
            if (checkBox3.Checked)
            {
                MF.DirektVez[2] = true;
                MF.KitTenyezok[2] = (byte)numericUpDown3.Value;
            }
            if (checkBox4.Checked)
            {
                MF.DirektVez[3] = true;
                MF.KitTenyezok[3] = (byte)numericUpDown4.Value;
            }
            if (checkBox5.Checked)
            {
                MF.DirektVez[4] = true;
                MF.KitTenyezok[4] = (byte)numericUpDown5.Value;
            }
            if (checkBox6.Checked)
            {
                MF.DirektVez[5] = true;
                MF.KitTenyezok[5] = (byte)numericUpDown6.Value;
            }
            if (checkBox7.Checked)
            {
                MF.DirektVez[6] = true;
                MF.KitTenyezok[6] = (byte)numericUpDown7.Value;
            }
            if (checkBox8.Checked)
            {
                MF.DirektVez[7] = true;
                MF.KitTenyezok[7] = (byte)numericUpDown8.Value;
            }
            
            
            MF.menuItem57.Checked = true;
            Program.KONFVanVezerlo = true;
            Eszkozok.Eszk.Elnevezo();
        }

        private void trackBar8_Scroll(object sender, EventArgs e)
        {
            numericUpDown8.Value = trackBar8.Value;
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            trackBar8.Value = (int)numericUpDown8.Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show((Program.KONFNyelv == "hun")?"Biztosan visszaállítja a Célhardver összes csatornáját Listaalapú vezérlésre?" : Eszk.GetNyelvSzo("ManualDeaktMboxSZOVEG"), (Program.KONFNyelv == "hun") ? "Manuális vezérlés feloldása" : Eszk.GetNyelvSzo("ManualDeaktMboxCIM"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                MF.menuItem12.PerformClick();
            }
        }

        private void AzonnaliVez_Load(object sender, EventArgs e)
        {

        }
    }
}
