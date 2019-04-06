using OpenHardwareMonitor.Eszkozok;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenHardwareMonitor.GUI
{
    public partial class FuggvenyGorbekeszito : Form
    {
        SemaSzerkeszto LSzerk;
        public FuggvenyGorbekeszito(SemaSzerkeszto lSzerk)
        {
            InitializeComponent();
            LSzerk = lSzerk;
            comboBox1.SelectedIndex = 0;
        }

        private void AutoGorbekeszito_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double minhom;
                if (checkBoxPasszivmod.Checked)
                {
                    if (numericUpDownMaxHom.Value <= numericUpDownPassziv.Value)
                    {
                        MessageBox.Show(((Program.KONFNyelv == "hun") ? "A passzív mód határa kisebb kell, hogy legyen a maximális hőmérsékletnél." : Eszk.GetNyelvSzo("FugvGorbKeszMBoxSzoveg")));
                        return;
                    }
                    else
                    {
                        minhom = (double)numericUpDownPassziv.Value;
                    }
                }
                else
                {
                    minhom = 20;
                }

                double a = (double)numericUpDowna.Value;
                double maxhom = (double)numericUpDownMaxHom.Value;
                double interv = maxhom - minhom;
                double minfordszam = (double)numericUpDownMinAktFordsz.Value;

                double EKMax = 100 - minfordszam;

                List<double> ertekek = new List<double>();
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        {//Nem számít az <a>
                            double m = EKMax / interv;
                            double kivonando = m * minhom;

                            for (int x = 20; x <= 110; x += 2)
                            {
                                if (x < minhom)
                                {
                                    ertekek.Add(0);
                                }
                                else if (x > maxhom)
                                {
                                    ertekek.Add(100);
                                }
                                else
                                {
                                    double ertek = m * x - kivonando + minfordszam;

                                    if (ertek <= 0)
                                        ertekek.Add(0);
                                    else if(ertek >= 100)
                                        ertekek.Add(100);
                                    else
                                        ertekek.Add(ertek);
                                }
                            }
                            break;
                        }
                    case 1:
                        {//Nem számít az <a>
                            double n = interv / (Math.Log(EKMax + 1) /Math.Log(2));

                            for (int x = 20; x <= 110; x += 2)
                            {
                                if (x < minhom)
                                {
                                    ertekek.Add(0);
                                }
                                else if (x > maxhom)
                                {
                                    ertekek.Add(100);
                                }
                                else
                                {
                                    double ertek = Math.Pow(2, (x - minhom)/n) - 1 + minfordszam;

                                    if (ertek <= 0)
                                        ertekek.Add(0);
                                    else if (ertek >= 100)
                                        ertekek.Add(100);
                                    else
                                        ertekek.Add(ertek);
                                }
                            }
                            break;
                        }
                    case 2:
                        {

                            //double n = 100 / Math.Log((interv), 2);

                            double n = EKMax / Math.Pow(interv, a);

                            for (int x = 20; x <= 110; x += 2)
                            {
                                if (x < minhom)
                                {
                                    ertekek.Add(0);
                                }
                                else if (x > maxhom)
                                {
                                    ertekek.Add(100);
                                }
                                else
                                {
                                    double ertek = Math.Pow((x - minhom), a)*n + minfordszam;

                                    if (ertek <= 0)
                                        ertekek.Add(0);
                                    else if (ertek >= 100)
                                        ertekek.Add(100);
                                    else
                                        ertekek.Add(ertek);
                                }
                            }
                            break;
                        }
                    case 3:
                        {
                            // double n = interv / Math.Pow(a, 100);
                            double n = EKMax / Math.Log((interv), 2);

                            for (int x = 20; x <= 110; x += 2)
                            {
                                if (x < minhom)
                                {
                                    ertekek.Add(0);
                                }
                                else if (x > maxhom)
                                {
                                    ertekek.Add(100);
                                }
                                else
                                {
                                    double ertek = Math.Log((x-minhom), 2)*n + minfordszam;

                                    if (ertek <= 0)
                                        ertekek.Add(0);
                                    else if (ertek >= 100)
                                        ertekek.Add(100);
                                    else
                                        ertekek.Add(ertek);
                                }
                            }
                            break;
                        }
                    case 4:
                        {
                            //double n = interv / (Math.Pow(100, a));

                            double n = EKMax / Math.Pow(interv, 1/a);

                            for (int x = 20; x <= 110; x += 2)
                            {
                                if (x < minhom)
                                {
                                    ertekek.Add(0);
                                }
                                else if (x > maxhom)
                                {
                                    ertekek.Add(100);
                                }
                                else
                                {
                                    double ertek = Math.Pow((x - minhom), 1/a)*n + minfordszam;

                                    if (ertek <= 0)
                                        ertekek.Add(0);
                                    else if (ertek >= 100)
                                        ertekek.Add(100);
                                    else
                                        ertekek.Add(ertek);
                                }
                            }
                            break;
                        }
                }

                LSzerk.GorbeBeallito(ertekek);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error - Please, check the parameters!\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBoxPasszivmod_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownPassziv.Enabled = checkBoxPasszivmod.Checked;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex == 0 || comboBox1.SelectedIndex == 1 || comboBox1.SelectedIndex == 3)
            {
                numericUpDowna.Enabled = false;
            }
            else
            {
                numericUpDowna.Enabled = true;
            }
        }
    }
}
