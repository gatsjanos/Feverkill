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
    public partial class BootIteracioBeallit : Form
    {
        public BootIteracioBeallit()
        {
            InitializeComponent();

            numericUpDownIteracioszam.Value = Program.KONFBootIteracioSzam;
            numericUpDownFrissido.Value = Program.KONFBootFrisIdo;
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            label3.Text = "The new interval you apply for the first " + numericUpDownIteracioszam.Value + " iterations:";

            if (numericUpDownIteracioszam.Value == 0)
                numericUpDownFrissido.Enabled = false;
            else
                numericUpDownFrissido.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Program.KONFBootFrisIdo = (int)numericUpDownFrissido.Value;
                Program.KONFBootIteracioSzam = (int)numericUpDownIteracioszam.Value;

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
