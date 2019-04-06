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
    public partial class GetTeljesverz : Form
    {
        List<Color> Szinek = new List<Color>();

        Timer timer = new Timer() { Interval = 490, Enabled = true };
        Bitmap hatter;
        public GetTeljesverz()
        {
            InitializeComponent();

            Szinek.Add(Color.Red);
            Szinek.Add(Color.Green);
            Szinek.Add(Color.Blue);
            Szinek.Add(Color.Yellow);

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            hatter = (Bitmap)pictureBox1.Image;
            timer.Tick += Timer_Tick;
        }

        int negyzetoldal = 60;
        private void Timer_Tick(object sender, EventArgs e)
        {
            Negyzetszinezo(Program.RandomObject.Next(0, (pictureBox1.Width - 1) / negyzetoldal), Program.RandomObject.Next(0, (pictureBox1.Height - 1) / negyzetoldal), negyzetoldal, GetRandomSzin());
        }

        void Negyzetszinezo(int x, int y, int negyzold, Color szin)
        {
            try
            {
                for (int i = x * negyzetoldal; i < x * negyzetoldal + negyzetoldal; i++)
                {
                    for (int a = y * negyzetoldal; a < y * negyzetoldal + negyzetoldal; a++)
                    {
                        hatter.SetPixel(i, a, szin);
                    }
                }
                pictureBox1.Refresh();
            }
            catch { }
        }

        Color GetRandomSzin()
        {
            return Szinek[Program.RandomObject.Next(0, Szinek.Count)];
        }

        bool direktbezaras = false;
        private void GetTeljesverz_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !direktbezaras;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Eszkozok.Eszk.GetFullVersion();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Negyzetszinezo(((MouseEventArgs)e).X / negyzetoldal, ((MouseEventArgs)e).Y / negyzetoldal, negyzetoldal, Color.White);

            if (VegeTeszt())
            {
                direktbezaras = true;
                try
                {
                    this.Close();
                }
                catch { }
                try
                {
                    this.Dispose();
                }
                catch { }
            }
        }
        bool VegeTeszt()
        {
            for (int i = 0; i < (pictureBox1.Width - 1) / negyzetoldal; i++)
            {
                for (int a = 0; a < (pictureBox1.Height - 1) / negyzetoldal; a++)
                {
                    Color pixszin = hatter.GetPixel(i * negyzetoldal + 1, a * negyzetoldal + 1);
                    if (pixszin.A != 0 && !SzinEgyenlo(pixszin, Color.White))
                        return false;
                }
            }
            return true;
        }
        bool SzinEgyenlo(Color A, Color B)
        {
            if (A.R == B.R && A.G == B.G && A.B == B.B)
            {
                return true;
            }
            return false;
        }

        public static void FreemiumClickTest()
        {
            if(!Eszkozok.Eszk.IsPremiumFuncEabled())
            {
                if(Program.RandomObject.Next(0, 1000) > 960)
                new GetTeljesverz().ShowDialog();
            }
        }
    }
}
