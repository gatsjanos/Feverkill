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
    public partial class Klonozo : Form
    {
        int ListIndex;
        List<Program.SzabLista> SzabListakBe;
        public Klonozo(List<Program.SzabLista> Szlistak, int ListIndexbe)
        {
            InitializeComponent();
            Program.OsszesForm.Add(this);
            this.TopMost = Program.KONFFelulMarado;
            if (Program.KONFNyelv != "hun")
                Lokalizalj();
            

            SzabListakBe = Szlistak;
            ListIndex = ListIndexbe;


            textBox1.Text = SzabListakBe[ListIndex].Nev;
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (textBox1.Text.Contains("Adja meg a szabályzólista kívánt nevét!") || (textBox1.Text == "") || textBox1.Text == SzabListakBe[ListIndex].Nev)
                {
                    MessageBox.Show(((Program.KONFNyelv == "hun") ? "Adjon meg egy, az eredetitől eltérő\nnevet a Klónozott Szabályzólistának!" : Eszk.GetNyelvSzo("NevhibaNemelteroKlon")), ((Program.KONFNyelv == "hun") ? "Névhiba!" : Eszk.GetNyelvSzo("Névhiba!")), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    bool vanmarilyen = false;
                    foreach (Program.SzabLista item in Program.SzabListak)
                    {
                        if (item.Nev == textBox1.Text)
                        {
                            vanmarilyen = true;
                            MessageBox.Show(((Program.KONFNyelv == "hun") ? "Ilyen nevű szabályzólista már létezik!" : Eszk.GetNyelvSzo("NevhibaVanilyen")), ((Program.KONFNyelv == "hun") ? "Névhiba!" : Eszk.GetNyelvSzo("Névhiba!")), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        }
                    }

                    if (!vanmarilyen)
                    {
                        //DeepCopy, majd névcsere
                        Program.SzabLista SzabListMent = Fajlkezelo.DeepCopySzablista(SzabListakBe[ListIndex]);
                        SzabListMent.Nev = textBox1.Text;
                        
                        Program.SZLIST_SZERK_MENT = SzabListMent;
                        Program.Dolgozott = true;

                        try { this.Close(); }
                        catch { }
                    }
                }
            }
        }

        void Lokalizalj()
        {
            this.Text = Eszk.GetNyelvSzo("KlónozóCÍM");
        }
    }
}
