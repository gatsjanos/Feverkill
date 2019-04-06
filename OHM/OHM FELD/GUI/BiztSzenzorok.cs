using OpenHardwareMonitor.Eszkozok;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenHardwareMonitor.GUI
{
    public partial class BiztSzenzorok : Form
    {
        FoAblak MF;
        public BiztSzenzorok(FoAblak MFbe)
        {
            InitializeComponent();
            Program.OsszesForm.Add(this);
            this.TopMost = Program.KONFFelulMarado;
            MF = MFbe;

            if (Program.KONFNyelv != "hun")
            {
                this.Text = Eszk.GetNyelvSzo("BiztSzenzCIM");
                Lokalizalj();
            }
            BiztLeker();
            HomLeker();
        }
        void Lokalizalj()
        {
            label1.Text = Eszk.GetNyelvSzo("Szenzor") + " 1:";
            label2.Text = Eszk.GetNyelvSzo("Szenzor") + " 2:";
            labelHat1.Text = Eszk.GetNyelvSzo("BIZHLabHatár");
            labelHat2.Text = Eszk.GetNyelvSzo("BIZHLabHatár");
            button2.Text = Eszk.GetNyelvSzo("Frissítés");
        }

        const double SZENZARANY = 2.55;
        void HomLeker()
        {
            try
            {
                for (int i = 0; i < 10 && Program.SorosKuldes == true; ++i)
                {
                    System.Threading.Thread.Sleep(50);
                }
                Program.SorosKuldes = true;

                for (int k = 0; k < 3; k++)
                {
                    Program.SorosPort.ReadExisting();

                    Fajlkezelo.UARTbajtKuldo((byte)(115 + Program.UARTKod100));

                    string bejovo = "";
                    for (int i = 0; i < 10; i++)
                    {
                        Thread.Sleep(50);

                        bejovo += Program.SorosPort.ReadExisting();

                        if (bejovo.Length >= 8)
                            break;
                    }
                    if (bejovo.Length >= 8)
                    {
                        if (CRCTesztHomLeker(bejovo))
                        {
                            #region Logaritmikus
                            //double Roszt1 = (5 / ((double)((byte)bejovo[1] | (byte)bejovo[2] << 6) * ((double)5 / 1024))) - (double)1;
                            //double Roszt2 = (5 / ((double)((byte)bejovo[3] | (byte)bejovo[4] << 6) * ((double)5 / 1024))) - (double)1;

                            //if (Roszt1 < 0.13726)
                            //{
                            //    double a = 1.09742 * Math.Pow(10, 8), b = -5.66757 * Math.Pow(10, 7), c = 1.20244 * Math.Pow(10, 7), d = -1.36339 * Math.Pow(10, 6), e = 90847.61862, f = -3870.48053, g = 174.69727;
                            //    try { labelS1Ert.Text = Math.Round(a * Math.Pow(Roszt1, 6) + b * Math.Pow(Roszt1, 5) + c * Math.Pow(Roszt1, 4) + d * Math.Pow(Roszt1, 3) + e * Math.Pow(Roszt1, 2) + f * Roszt1 + g).ToString() + "°C"; } catch { }
                            //}
                            //else if (Roszt1 < 0.61822)
                            //{
                            //    double a = 12979.45085, b = -29382.10459, c = 27256.64504, d = -13492.89839, e = 3927.45688, f = -734.46067, g = 123.3825;
                            //    try { labelS1Ert.Text = Math.Round(a * Math.Pow(Roszt1, 6) + b * Math.Pow(Roszt1, 5) + c * Math.Pow(Roszt1, 4) + d * Math.Pow(Roszt1, 3) + e * Math.Pow(Roszt1, 2) + f * Roszt1 + g).ToString() + "°C"; } catch { }
                            //}
                            //else
                            //{
                            //    double a = 0, b = 0, c = 0, d = -11.33434, e = 45.64721, f = -77.38692, g = 68.07405;
                            //    //decimal dec = (decimal)Math.Round(a * Math.Pow(Roszt1, 6) + b * Math.Pow(Roszt1, 5) + c * Math.Pow(Roszt1, 4) + d * Math.Pow(Roszt1, 3) + e * Math.Pow(Roszt1, 2) + f * Roszt1 + g);
                            //    try { labelS1Ert.Text = Math.Round(a * Math.Pow(Roszt1, 6) + b * Math.Pow(Roszt1, 5) + c * Math.Pow(Roszt1, 4) + d * Math.Pow(Roszt1, 3) + e * Math.Pow(Roszt1, 2) + f * Roszt1 + g).ToString() + "°C"; } catch { }
                            //}

                            //if (Roszt2 < 0.13726)
                            //{
                            //    double a = 1.09742 * Math.Pow(10, 8), b = -5.66757 * Math.Pow(10, 7), c = 1.20244 * Math.Pow(10, 7), d = -1.36339 * Math.Pow(10, 6), e = 90847.61862, f = -3870.48053, g = 174.69727;
                            //    try { labelS2Ert.Text = Math.Round(a * Math.Pow(Roszt2, 6) + b * Math.Pow(Roszt2, 5) + c * Math.Pow(Roszt2, 4) + d * Math.Pow(Roszt2, 3) + e * Math.Pow(Roszt2, 2) + f * Roszt2 + g).ToString() + "°C"; } catch { }
                            //}
                            //else if (Roszt2 < 0.61822)
                            //{
                            //    double a = 12979.45085, b = -29382.10459, c = 27256.64504, d = -13492.89839, e = 3927.45688, f = -734.46067, g = 123.3825;
                            //    try { labelS2Ert.Text = Math.Round(a * Math.Pow(Roszt2, 6) + b * Math.Pow(Roszt2, 5) + c * Math.Pow(Roszt2, 4) + d * Math.Pow(Roszt2, 3) + e * Math.Pow(Roszt2, 2) + f * Roszt2 + g).ToString() + "°C"; } catch { }
                            //}
                            //else
                            //{
                            //    double a = 0, b = 0, c = 0, d = -11.33434, e = 45.64721, f = -77.38692, g = 68.07405;
                            //    //decimal dec = (decimal)Math.Round(a * Math.Pow(Roszt1, 6) + b * Math.Pow(Roszt1, 5) + c * Math.Pow(Roszt1, 4) + d * Math.Pow(Roszt1, 3) + e * Math.Pow(Roszt1, 2) + f * Roszt1 + g);
                            //    try { labelS2Ert.Text = Math.Round(a * Math.Pow(Roszt2, 6) + b * Math.Pow(Roszt2, 5) + c * Math.Pow(Roszt2, 4) + d * Math.Pow(Roszt2, 3) + e * Math.Pow(Roszt2, 2) + f * Roszt2 + g).ToString() + "°C"; } catch { }
                            //}
                            #endregion

                            double ADC1 = (double)((byte)bejovo[1] | (byte)bejovo[2] << 6);
                            double ADC2 = (double)((byte)bejovo[3] | (byte)bejovo[4] << 6);

                            if (Math.Round((ADC1 * ((double)5 / 1024) * 100) - 273) > 180)
                                labelS1Ert.Text = "N/A" + "°C";
                            else
                                labelS1Ert.Text = Math.Round((ADC1 * ((double)5 / 1024) * 100) - 273).ToString() + "°C";

                            if (Math.Round((ADC2 * ((double)5 / 1024) * 100) - 273) > 180)
                                labelS2Ert.Text = "N/A" + "°C";
                            else
                                labelS2Ert.Text = Math.Round((ADC2 * ((double)5 / 1024) * 100) - 273).ToString() + "°C";

                            break; // felső for-ból
                        }

                    }
                }
            }
            catch { }
            Program.SorosKuldes = false;

        }

        bool CRCTesztHomLeker(string bejovo)
        {
            byte CRC1 = 0, CRC2 = 0, CRC3 = 0;
            for (int i = 1; i < 5; i++)
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

            if (CRC1 == bejovo[5] && CRC2 == bejovo[6] && CRC3 == bejovo[7])
                return true;

            return false;
        }
        void BiztLeker()
        {
            try
            {
                for (int i = 0; i < 10 && Program.SorosKuldes == true; ++i)
                {
                    System.Threading.Thread.Sleep(50);
                }
                Program.SorosKuldes = true;

                for (int k = 0; k < 3; k++)
                {
                    Program.SorosPort.ReadExisting();
                    Fajlkezelo.UARTbajtKuldo((byte)(118 + Program.UARTKod100));

                    string bejovo = "";
                    for (int i = 0; i < 10; i++)
                    {
                        Thread.Sleep(50);

                        bejovo += Program.SorosPort.ReadExisting();

                        if (bejovo.Length >= 8)
                            break;
                    }
                    if (bejovo.Length >= 8)
                    {
                        if (CRCTesztBiztLeker(bejovo))
                        {
                            #region Logaritmikus
                            //double Roszt1 = (5 / ((double)((byte)bejovo[1] | (byte)bejovo[2] << 6) * ((double)5 / 1024))) - (double)1;
                            //double Roszt2 = (5 / ((double)((byte)bejovo[3] | (byte)bejovo[4] << 6) * ((double)5 / 1024))) - (double)1;

                            //if (Roszt1 < 0.13726)
                            //{
                            //    double a = 1.09742 * Math.Pow(10, 8), b = -5.66757 * Math.Pow(10, 7), c = 1.20244 * Math.Pow(10, 7), d = -1.36339 * Math.Pow(10, 6), e = 90847.61862, f = -3870.48053, g = 174.69727;
                            //    decimal dec = (decimal)Math.Round(a * Math.Pow(Roszt1, 6) + b * Math.Pow(Roszt1, 5) + c * Math.Pow(Roszt1, 4) + d * Math.Pow(Roszt1, 3) + e * Math.Pow(Roszt1, 2) + f * Roszt1 + g);
                            //    try { numericUpDown1.Value = (dec> numericUpDown1.Maximum)?numericUpDown1.Maximum : dec; }  catch { }
                            //}
                            //else if (Roszt1 < 0.61822)
                            //{
                            //    double a = 12979.45085, b = -29382.10459, c = 27256.64504, d = -13492.89839, e = 3927.45688, f = -734.46067, g = 123.3825;
                            //    decimal dec = (decimal)Math.Round(a * Math.Pow(Roszt1, 6) + b * Math.Pow(Roszt1, 5) + c * Math.Pow(Roszt1, 4) + d * Math.Pow(Roszt1, 3) + e * Math.Pow(Roszt1, 2) + f * Roszt1 + g);
                            //    try { numericUpDown1.Value = (dec > numericUpDown1.Maximum) ? numericUpDown1.Maximum : dec; } catch { }
                            //}
                            //else
                            //{
                            //    double a = 0, b = 0, c = 0, d = -11.33434, e = 45.64721, f = -77.38692, g = 68.07405;
                            //    decimal dec = (decimal)Math.Round(a * Math.Pow(Roszt1, 6) + b * Math.Pow(Roszt1, 5) + c * Math.Pow(Roszt1, 4) + d * Math.Pow(Roszt1, 3) + e * Math.Pow(Roszt1, 2) + f * Roszt1 + g);
                            //    try { numericUpDown1.Value = (dec > numericUpDown1.Maximum) ? numericUpDown1.Maximum : dec; } catch { }
                            //}

                            //if (Roszt2 < 0.13726)
                            //{
                            //    double a = 1.09742 * Math.Pow(10, 8), b = -5.66757 * Math.Pow(10, 7), c = 1.20244 * Math.Pow(10, 7), d = -1.36339 * Math.Pow(10, 6), e = 90847.61862, f = -3870.48053, g = 174.69727;
                            //    decimal dec = (decimal)Math.Round(a * Math.Pow(Roszt2, 6) + b * Math.Pow(Roszt2, 5) + c * Math.Pow(Roszt2, 4) + d * Math.Pow(Roszt2, 3) + e * Math.Pow(Roszt2, 2) + f * Roszt2 + g);
                            //    try { numericUpDown2.Value = (dec > numericUpDown2.Maximum) ? numericUpDown2.Maximum : dec; } catch { }
                            //}
                            //else if (Roszt2 < 0.61822)
                            //{
                            //    double a = 12979.45085, b = -29382.10459, c = 27256.64504, d = -13492.89839, e = 3927.45688, f = -734.46067, g = 123.3825;
                            //    decimal dec = (decimal)Math.Round(a * Math.Pow(Roszt2, 6) + b * Math.Pow(Roszt2, 5) + c * Math.Pow(Roszt2, 4) + d * Math.Pow(Roszt2, 3) + e * Math.Pow(Roszt2, 2) + f * Roszt2 + g);
                            //    try { numericUpDown2.Value = (dec > numericUpDown2.Maximum) ? numericUpDown2.Maximum : dec; } catch { }
                            //}
                            //else
                            //{
                            //    double a = 0, b = 0, c = 0, d = -11.33434, e = 45.64721, f = -77.38692, g = 68.07405;
                            //    decimal dec = (decimal)Math.Round(a * Math.Pow(Roszt2, 6) + b * Math.Pow(Roszt2, 5) + c * Math.Pow(Roszt2, 4) + d * Math.Pow(Roszt2, 3) + e * Math.Pow(Roszt2, 2) + f * Roszt2 + g);
                            //    try { numericUpDown2.Value = (dec > numericUpDown2.Maximum) ? numericUpDown2.Maximum : dec; } catch { }
                            //}
                            #endregion

                            double ADC1 = (double)((byte)bejovo[1] | (byte)bejovo[2] << 5);
                            double ADC2 = (double)((byte)bejovo[3] | (byte)bejovo[4] << 5);
                            decimal Ho1 = (decimal)Math.Round((ADC1 * ((double)5 / 1024) * 100) - 273);
                            decimal Ho2 = (decimal)Math.Round((ADC2 * ((double)5 / 1024) * 100) - 273);

                            numericUpDown1.Value = (Ho1 < numericUpDown1.Minimum || Ho1 > numericUpDown1.Maximum) ? 60 : Ho1;
                            numericUpDown2.Value = (Ho2 < numericUpDown2.Minimum || Ho2 > numericUpDown2.Maximum) ? 60 : Ho2;

                            break; // felső for-ból
                        }

                    }
                }
            }
            catch { }
            Program.SorosKuldes = false;

        }

        bool CRCTesztBiztLeker(string bejovo)
        {
            byte CRC1 = 0, CRC2 = 0, CRC3 = 0;
            for (int i = 1; i < 5; i++)
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

            if (CRC1 == bejovo[5] && CRC2 == bejovo[6] && CRC3 == bejovo[7])
                return true;

            return false;
        }

        private void button1_Click(object sender, EventArgs ea)
        {
            for (int i = 0; i < 10 && Program.SorosKuldes == true; ++i)
            {
                System.Threading.Thread.Sleep(50);
            }
            Program.SorosKuldes = true;

            #region Logaritmikus
            //double x1 = (double)numericUpDown1.Value, x2 = (double)numericUpDown2.Value;

            //double a = 6.71704, b = -3.3054, c = 6.76037, d = -7.49996, e = 0.00492, f = -0.19097, g = 3.63119;
            //double s1 = a * Math.Pow(10, -12) * Math.Pow(x1, 6) + b * Math.Pow(10, -9) * Math.Pow(x1, 5) + c * Math.Pow(10, -7) * Math.Pow(x1, 4) + d * Math.Pow(10, -5) * Math.Pow(x1, 3) + e * Math.Pow(x1, 2) + f * x1 + g;
            //double s2 = a * Math.Pow(10, -12) * Math.Pow(x2, 6) + b * Math.Pow(10, -9) * Math.Pow(x2, 5) + c * Math.Pow(10, -7) * Math.Pow(x2, 4) + d * Math.Pow(10, -5) * Math.Pow(x2, 3) + e * Math.Pow(x2, 2) + f * x2 + g;
            //s1 = (double)5 / (s1 + 1);
            //s2 = (double)5 / (s2 + 1);
            //s1 = s1 / ((double)5 / 1024);
            //s2 = s2 / ((double)5 / 1024);
            //if (s1 > 1020)
            //    s1 = 1020;
            //if (s2 > 1020)
            //    s2 = 1020;
            #endregion

            double s1 = Math.Round(((double)numericUpDown1.Value + 273) / ((((double)5 / 1024) * 100)));
            double s2 = Math.Round(((double)numericUpDown2.Value + 273) / ((((double)5 / 1024) * 100)));

           
                //MessageBox.Show("S1: " + s1 + "\ns2: " + s2);
                byte[] kuldendo = new byte[] { (byte)((ushort)Math.Round(s1) & Convert.ToUInt16("0000000000111111", 2)), (byte)(((ushort)Math.Round(s1) & Convert.ToUInt16("0000111111000000", 2)) >> 6), (byte)((ushort)Math.Round(s2) & Convert.ToUInt16("0000000000111111", 2)), (byte)(((ushort)Math.Round(s2) & Convert.ToUInt16("0000111111000000", 2)) >> 6) };

                Enabled = false;
                bool siker = false;
            try
            {

                for (int i = 0; i < 2; i++)
                {
                    if (Fajlkezelo.UARTbajtKuldo(120, kuldendo))
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
                        if (Fajlkezelo.UARTbajtKuldo(120, kuldendo))
                        {
                            siker = true;
                            break;
                        }
                    }
                }
            }
            catch { }

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

        private void button2_Click(object sender, EventArgs e)
        {
            HomLeker();
        }
    }
}
