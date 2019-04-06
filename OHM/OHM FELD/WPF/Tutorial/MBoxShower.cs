using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UdvozloKepernyo
{
    public partial class MBoxShower : Form
    {
        public MBoxShower(string torzs, string cim)
        {
            InitializeComponent();

            this.Opacity = 0;
            
            Timer tmr = new Timer() { Interval = 50 };
            tmr.Tick += delegate (object sender, EventArgs e)
            {
                tmr.Stop(); MessageBox.Show(torzs, cim); try
                {
                    Dispose(true);
                }
                catch { }
                try
                {
                    Close();
                }
                catch { }
            };
            tmr.Start();
        }
    }
}
