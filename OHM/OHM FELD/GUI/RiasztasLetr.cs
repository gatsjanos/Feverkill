using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Runtime.InteropServices;
using OpenHardwareMonitor.Eszkozok;

namespace OpenHardwareMonitor.GUI
{
    public partial class RiasztasLetr : Form
    {
        List<Program.HoMers> HoMersek;
        public RiasztasLetr(int SzenzorIndex, List<Program.HoMers> Homersek)
        {
            HoMersek = Homersek;
            InitializeComponent();
            Program.OsszesForm.Add(this);
            this.TopMost = Program.KONFFelulMarado;

            this.Menu = Program.FoAblak.mainMenu.CloneMenu();
            for (int i = 0; i < this.Menu.MenuItems.Count; i++)
            {
                this.Menu.MenuItems[i].Visible = false;
            }
            this.Size = new Size(Width, Height - 39); ;

            for (int i = 0; i < HoMersek.Count; i++)
            {
                comboBox1.Items.Add(HoMersek[i].Csop + " =->> " + HoMersek[i].Nev + " >>> " + HoMersek[i].Ertek);
            }
            comboBox1.SelectedIndex = SzenzorIndex;

            if (Program.KONFNyelv != "hun")
                Lokalizalj();

            if (Program.Riasztasok.Count == 0 && Program.KONFTutorialMegjelenit)
            {
                Program.TutorialWPFAblak.MutassMessagebox("Set your first alert and save it!", "Creating Alerts");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Eszkozok.Eszk.IsPremiumFuncEabled() && Program.Riasztasok.Count >= 2)
            {
                if (MessageBox.Show(((Program.KONFNyelv == "hun") ? "A program ingyenes verziójában legfeljebb 2db riasztás hozható létre.\nSzeretne korlátlan számú riasztást létrehozni?" : Eszk.GetNyelvSzo("MBoxSzovegFreeRiasztletrehoz")), "Freemium", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    Eszkozok.Eszk.GetFullVersion();
                }
            }
            else
            {
                bool elotteNemVoltRiasztas = false;
                if (Program.Riasztasok.Count == 0)
                    elotteNemVoltRiasztas = true;

                Program.Riasztas RiasztasMent = new Program.Riasztas();

                RiasztasMent.Homero = HoMersek[comboBox1.SelectedIndex].Csop + " =->> " + HoMersek[comboBox1.SelectedIndex].Nev;
                RiasztasMent.RiasztPont = (int)numericUpDown1.Value;
                RiasztasMent.Muvelet = comboBox2.Items[comboBox2.SelectedIndex].ToString();
                RiasztasMent.Uzenet = textBox1.Text;
                RiasztasMent.Hangjelzes = checkBoxHangj.Checked;

                if (radioButtonAlvo.Checked)
                    RiasztasMent.SpecMuvelet = "a";
                else if (radioButtonHibern.Checked)
                    RiasztasMent.SpecMuvelet = "h";
                else if (radioButtonLeall.Checked)
                    RiasztasMent.SpecMuvelet = "l";
                else if (radioButtonUjraind.Checked)
                    RiasztasMent.SpecMuvelet = "u";
                else
                    RiasztasMent.SpecMuvelet = "n";

                RiasztasMent.EbresztIdo = -1;
                if (checkBoxEbreszt.Enabled && checkBoxEbreszt.Checked)
                {
                    RiasztasMent.EbresztIdo = (int)numericUpDownEbresztIdo.Value;
                }


                Program.Riasztasok.Add(RiasztasMent);

                Fajlkezelo.RiasztasIr(Program.Riasztasok);
                Program.Attekint.listViewRiaszt.Dispatcher.Invoke(Program.FoAblak.AttekintRiasztFrissit);

                if (elotteNemVoltRiasztas && Program.KONFTutorialMegjelenit)
                {
                    Program.TutorialWPFAblak.MutassMessagebox("You have created your first alert!\nIt will be active until you delete it in the Alert Manager.", "Alert created! Well done!");
                    Program.TutorialSzenzgrafBemutatasjon = true;
                }

                try { this.Close(); } catch { }
            }
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonAlvo.Checked || radioButtonHibern.Checked)
            {
                checkBoxEbreszt.Enabled = true;
            }
            else
                checkBoxEbreszt.Enabled = false;
        }

        private void buttonKerdojel_Click(object sender, EventArgs e)
        {
            MessageBox.Show((Program.KONFNyelv != "hun") ? Eszk.GetNyelvSzo("RiaLetrKerdojelGombTEST") : "    Ezzel a beállítással megadható, hogy a PC mennyi idő után ébredjen fel, miután a Hibernált, vagy Alvó állapotba került.\n\n    A felébresztés lehetősége különösen akkor hasznos, ha számítógépét egy hosszan tartó, erőforrásigényes művelet során (pl. videó renderelése, szoftver telepítése, stb...) magára hagyja, és a maximális hűtési teljesítmény nem elegendő ahhoz, hogy megfelelő hőmérsékleten tartsa a hardvereket, így hibernálás, vagy alvó állapotba lépés szükséges a hardverek épségének megőrzéséhez.\n    Miután a számítógép hibernált, vagy alvó állapotba került, és néhány percen át hült, felébred, és folytatja az általa végzett feladatot, ahol az a hibernálás, vagy alvó állapotba lépés miatt meg lett szakítva, míg ismét életbe nem lép ez a riasztás és meg nem történik ismét az előbb leírt folyamat.\n\n   Ezen módon a magára hagyott PC biztosan fogja végezni feladatát, túlmelegedés pedig nem következhet be, mivel ahányszor csak szükséges, ez a \"kényszerpihenő\" lesz beiktatva.", (Program.KONFNyelv != "hun") ? Eszk.GetNyelvSzo("RiaLetrKerdojelGombCIM") : "Számítógép automatikus felébresztése", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        void Lokalizalj()
        {
            this.Text = Eszk.GetNyelvSzo("RiaLetrCIM");
            labelSzenz.Text = Eszk.GetNyelvSzo("RiaLetrLabelSzenzKiv");
            labelUzen.Text = Eszk.GetNyelvSzo("RiaLetrLabelUzenet");
            labelRPont.Text = Eszk.GetNyelvSzo("RiaLetrLabelRiaPont");
            labelRelacio.Text = Eszk.GetNyelvSzo("RiaLetrLabelRelacio");
            checkBoxHangj.Text = Eszk.GetNyelvSzo("RiaLetrCheckboxHangjelz");
            checkBoxEbreszt.Text = Eszk.GetNyelvSzo("RiaLetrCheckboxEbresztes");
            button1.Text = Eszk.GetNyelvSzo("RiaLetrButtonMentes");
            groupBox1.Text = Eszk.GetNyelvSzo("RiaLetrGroupboxCIM");
            radioButtonNincsMuv.Text = Eszk.GetNyelvSzo("RiaLetrRadioNincsMuv");
            radioButtonAlvo.Text = Eszk.GetNyelvSzo("RiaLetrRadioAlvoAll");
            radioButtonHibern.Text = Eszk.GetNyelvSzo("RiaLetrRadioHibernal");
            radioButtonLeall.Text = Eszk.GetNyelvSzo("RiaLetrRadioLeallit");
            radioButtonUjraind.Text = Eszk.GetNyelvSzo("RiaLetrRadioUjraind");
        }
    }
}
