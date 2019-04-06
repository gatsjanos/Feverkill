using OpenHardwareMonitor.Eszkozok;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OpenHardwareMonitor.GUI
{
    public partial class Felcimkezo : Form
    {
        bool CelHardverCsatorna = false;
        int CsatIndex;
        string KimenetAlapnev;
        FoAblak MF;
        public Felcimkezo(int CsatIndexbe, FoAblak MFbe)
        {
            CelHardverCsatorna = true;

            InitializeComponent();
            Program.OsszesForm.Add(this);
            this.TopMost = Program.KONFFelulMarado;

            if (Program.KONFNyelv != "hun")
                Lokalizalj();

            MF = MFbe;

            CsatIndex = CsatIndexbe;

            textBox1.Text = Program.CsatCimkekCelh[CsatIndex];
        }

        public Felcimkezo(string Kimenetbe, FoAblak MFbe)
        {
            CelHardverCsatorna = false;

            InitializeComponent();
            Program.OsszesForm.Add(this);
            this.TopMost = Program.KONFFelulMarado;

            if (Program.KONFNyelv != "hun")
                Lokalizalj();

            MF = MFbe;


            try
            {
                string[] buffnevkezel = Kimenetbe.Split('\n');
                KimenetAlapnev = buffnevkezel[0];

                if (buffnevkezel.Length > 1)
                    textBox1.Text = buffnevkezel[1].Remove(0, 8);
            }
            catch { }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (!Eszkozok.Eszk.IsPremiumFuncEabled() && CsatIndex != 0)
                {
                    if (MessageBox.Show(((Program.KONFNyelv == "hun") ? "A program ingyenes verziójában csak az 1. csatorna címkézhető fel.\nSzeretné a többi listát is felcímkézni?" : Eszk.GetNyelvSzo("MBoxSzovegFreeCimkezes")), "Freemium", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        Eszkozok.Eszk.GetFullVersion();
                    }
                }
                else
                {
                    if (CelHardverCsatorna)
                        Program.CsatCimkekCelh[CsatIndex] = textBox1.Text;
                    else
                       if (Program.CsatCimkekBelso.ContainsKey(KimenetAlapnev))
                        Program.CsatCimkekBelso[KimenetAlapnev] = textBox1.Text;
                    else
                        Program.CsatCimkekBelso.Add(KimenetAlapnev, textBox1.Text);

                    Program.Attekint.listViewFordszamok.Items.Dispatcher.Invoke(MF.AttekintFordszamFrissit);

                    Fajlkezelo.CsCimkeIr();

                    try { this.Close(); }
                    catch { }
                }
            }
        }

        void Lokalizalj()
        {
            this.Text = Eszk.GetNyelvSzo("FelcimkezoCÍM");
        }
    }
}
