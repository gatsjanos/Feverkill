using OpenHardwareMonitor.Eszkozok;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace OpenHardwareMonitor
{
    public partial class AlapBeallito : Form
    {
        static GUI.FoAblak MF;
        public AlapBeallito(GUI.FoAblak mf)
        {
            InitializeComponent();
            Program.OsszesForm.Add(this);
            this.TopMost = Program.KONFFelulMarado;

            if (Program.KONFNyelv != "hun")
                this.Text = Eszk.GetNyelvSzo("IndFordszCIM");
            
            MF = mf;

            this.Menu = MF.mainMenu.CloneMenu();
            for (int i = 0; i < this.Menu.MenuItems.Count; i++)
            {
                this.Menu.MenuItems[i].Visible = false;
            }
            this.Size = new Size(Width, Height - 39); ;
            
            AlapLeker();
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

        void AlapLeker()
        {
            try
            {
                for (int i = 0; i < 10 && Program.SorosKuldes == true; ++i)
                {
                    System.Threading.Thread.Sleep(50);
                }
                Program.SorosKuldes = true;

                for (int a = 0; a < 3; a++)
                {
                    Program.SorosPort.ReadExisting();
                    Fajlkezelo.UARTbajtKuldo((byte)(110 + Program.UARTKod100));

                    string bejovo = "";
                    for (int i = 0; i < 10; i++)
                    {
                        Thread.Sleep(50);

                        bejovo += Program.SorosPort.ReadExisting();

                        if (bejovo.Length >= 12)
                            break;
                    }
                    if (bejovo.Length >= 12)
                    {
                        if (CRCTeszt(bejovo))
                        {
                            numericUpDown1.Value = bejovo[1];
                            numericUpDown2.Value = bejovo[2];
                            numericUpDown3.Value = bejovo[3];
                            numericUpDown4.Value = bejovo[4];
                            numericUpDown5.Value = bejovo[5];
                            numericUpDown6.Value = bejovo[6];
                            numericUpDown7.Value = bejovo[7];
                            numericUpDown8.Value = bejovo[8];
                            break; // felső for-ból
                        }

                    }
                }
                Program.SorosKuldes = false;
            }
            catch
            { }
        }

        bool CRCTeszt(string bejovo)
        {
            byte CRC1 = 0, CRC2 = 0, CRC3 = 0;
            for (int i = 1; i < 9; i++)
            {
                if ((bejovo[i] & Convert.ToByte("00000001", 2)) != 0)
                { ++CRC1; ++CRC3; }
                if ((bejovo[i] & Convert.ToByte("00000010", 2)) != 0)
                { ++CRC1; ++CRC2; }
                if ((bejovo[i] & Convert.ToByte("00000100", 2)) != 0)
                { ++CRC1; ++CRC3; }
                if ((bejovo[i] & Convert.ToByte("00001000", 2)) != 0)
                { ++CRC1; ++CRC2; }
                if ((bejovo[i] & Convert.ToByte("00010000", 2)) != 0)
                { ++CRC1; ++CRC3; }
                if ((bejovo[i] & Convert.ToByte("00100000", 2)) != 0)
                { ++CRC1; ++CRC2; }
                if ((bejovo[i] & Convert.ToByte("01000000", 2)) != 0)
                { ++CRC1; ++CRC3; }
                if ((bejovo[i] & Convert.ToByte("10000000", 2)) != 0)
                { ++CRC1; ++CRC2; }

            }

            if(CRC1 == bejovo[9] && CRC2 == bejovo[10] && CRC3 == bejovo[11])
                return true;

            return false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10 && Program.SorosKuldes == true; ++i)
            {
                System.Threading.Thread.Sleep(50);
            }
            Program.SorosKuldes = true;


            byte[] kuldendo = new byte[] { (byte)numericUpDown1.Value, (byte)numericUpDown2.Value, (byte)numericUpDown3.Value, (byte)numericUpDown4.Value, (byte)numericUpDown5.Value, (byte)numericUpDown6.Value, (byte)numericUpDown7.Value, (byte)numericUpDown8.Value };
            
            Enabled = false;
            bool siker = false;
            
                for (int i = 0; i < 2; i++)
                {
                    if (Fajlkezelo.UARTbajtKuldo(125, kuldendo))
                    {
                        siker = true;
                        break;
                    }
                }
                if (!siker)
                {
                    MF.KezfogasSzalIndito(false);

                    for (int i = 0; i < 2; i++)
                    {
                        if (Fajlkezelo.UARTbajtKuldo(125, kuldendo))
                        {
                            siker = true;
                            break;
                        }
                    }
                }
                if (!siker)
                    MessageBox.Show((Program.KONFNyelv == "hun") ? "A feltöltés sikertelen!\nPróbálja meg újra!" : Eszk.GetNyelvSzo("IndFordszHibaSZOVEG"), (Program.KONFNyelv == "hun") ? "Sikertelen Feltöltés" : Eszk.GetNyelvSzo("IndFordszHibaCIM"), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                else
                {
                    MF.menuItem57.Checked = true;
                    Program.KONFVanVezerlo = true;
                Eszkozok.Eszk.Elnevezo();

                    Program.SorosKuldes = false;
                    try { Close(); } catch { }
            }

                Program.SorosKuldes = false;
            Enabled = true;
        }

        private void trackBar8_Scroll(object sender, EventArgs e)
        {
            numericUpDown8.Value = trackBar8.Value;
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            trackBar8.Value = (int)numericUpDown8.Value;
        }
    }
}
