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
    public partial class KitTenyMutato : Form
    {
        public KitTenyMutato(FoAblak MF)
        {
            InitializeComponent();
            Program.OsszesForm.Add(this);
            this.TopMost = Program.KONFFelulMarado;

            this.Menu = MF.mainMenu.CloneMenu();
            for (int i = 0; i < this.Menu.MenuItems.Count; i++)
            {
                this.Menu.MenuItems[i].Visible = false;
            }
            this.Size = new Size(Width, Height - 39); ;

            if (Program.KONFNyelv != "hun")
                Lokalizalj();
        }

        void Lokalizalj()
        {
            this.Text = Eszk.GetNyelvSzo("KITMUTCIM");
            this.label1.Text = Eszk.GetNyelvSzo("Csatorna") + ":               1        2        3        4        5        6        7        8";
            this.label2.Text = Eszk.GetNyelvSzo("Ventilátor Fordulatszám") + ":";
        }
    }
}
