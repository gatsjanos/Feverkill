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
    public partial class AutoIndKesl : Form
    {
        public AutoIndKesl()
        {
            InitializeComponent();

            numericUpDown1.Value = (decimal)(Program.KONFAutoIndKesleltetes / (double)1000);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Program.KONFAutoIndKesleltetes = (int)(numericUpDown1.Value * 1000);

                try
                {
                    Program.FoAblak.menuItem15.Text = ((Program.KONFNyelv == "hun") ? "Auto Indítás Késleltetése" : Eszk.GetNyelvSzo("MFmenuItem15")) + " (" + (Program.KONFAutoIndKesleltetes / (double)1000) + " sec)";
                }
                catch
                { }

                if (Fajlkezelo.KiirtKONFTESZT() == false)
                {
                    Fajlkezelo.FoKonfMento();
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sorry for this. Something went wrong:\n" + ex.Message, "Pease, try again!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
