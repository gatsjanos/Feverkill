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
    public partial class TopMostDisabledUzenet : Form
    {
        public TopMostDisabledUzenet()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Dispose(true);
            }
            catch { }
            try
            {
                Close();
            }
            catch { }
        }
    }
}
