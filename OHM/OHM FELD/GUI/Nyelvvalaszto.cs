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
    public partial class Nyelvvalaszto : Form
    {
        public Nyelvvalaszto()
        {
            InitializeComponent();
            Program.OsszesForm.Add(this);
            this.TopMost = Program.KONFFelulMarado;
        }
        FoAblak MF = null;
        public Nyelvvalaszto(FoAblak MFbe)
        {
            InitializeComponent();
            Program.OsszesForm.Add(this);
            this.TopMost = Program.KONFFelulMarado;

            MF = MFbe;
        }
        void NyelvetValt(string ujnyelv)
        {
            if (Program.KONFKellMegNyelvvalasztas)
            {
                Program.KONFKellMegNyelvvalasztas = false;
            }

            if (ujnyelv != Program.KONFNyelv)
            {
                Program.KONFNyelv = ujnyelv;
                try
                {
                    for (int i = 0; i <= 5 && !MF.MindenAlaplapiAlapertelmezettre(i, false); i++)
                    {

                    }
                }
                catch { }

                //try { Program.SorosPort.Close(); } catch { }
                try { MF.SysTrayicon.Visible = false; } catch { }
                try { Program.HomersKuldTH.Abort(); } catch { }
                try { MF.SysTrayicon.Dispose(); } catch { }

                try
                {
                    Fajlkezelo.FoKonfMento();
                }
                catch { }

                System.Diagnostics.Process.Start("FeverkillSupervisor.exe", "");

                System.Environment.Exit(19981001);
            }
            else
            {
                Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            NyelvetValt("hun");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NyelvetValt("en");
        }
    }
}
