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
    public partial class Jatszoter : Form
    {
        FoAblak MF;
        public Jatszoter(FoAblak MFbe)
        {
            InitializeComponent();
            MF = MFbe;

            switch (Program.BEMUTATO_SzenzBemenet)
            {
                case Program.SzenzorBemenet.BelsoSzenzor:
                    radioButton1.Checked = true;
                    break;
                case Program.SzenzorBemenet.KulsoSzenzor:
                    radioButton2.Checked = true;
                    break;
                case Program.SzenzorBemenet.EmulaltSzenzor:
                    radioButton3.Checked = true;
                    break;
                default:
                    break;
            }

            if (Program.BelsoVentiVezTip == Program.BelsoVentiTipus.Emulalt)
                checkBoxEmuFans.Checked = true;

            checkBoxRandomSpeeds.Checked = Program.DEVRandomDefSpeeds;
            this.TopMost = checkBoxTopMost.Checked = Program.DEVTopMost;
            checkBoxKotottCOM.Checked = Program.DEVCOMPortKotese;
            textBoxCOM.Text = Program.DEVKotottCOMPort;
            numericUpDownFanNumber.Value = Program.DEVEmulaltVentiszam;

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                Program.BEMUTATO_SzenzBemenet = Program.SzenzorBemenet.BelsoSzenzor;
            }
            else if(radioButton2.Checked)
            {
                Program.BEMUTATO_SzenzBemenet = Program.SzenzorBemenet.KulsoSzenzor;
            }
            else if(radioButton3.Checked)
            {
                Program.BEMUTATO_SzenzBemenet = Program.SzenzorBemenet.EmulaltSzenzor;
            }
            try
            {
                MF.HoMeRok.listView1.Invoke(new ControlStringConsumer(SetText), new object[] { MF.HoMeRok.listView1 });
            }
            catch { }
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                Program.BEMUTATO_SzenzBemenet = Program.SzenzorBemenet.BelsoSzenzor;
            }
            else if (radioButton2.Checked)
            {
                Program.BEMUTATO_SzenzBemenet = Program.SzenzorBemenet.KulsoSzenzor;
            }
            else if (radioButton3.Checked)
            {
                Program.BEMUTATO_SzenzBemenet = Program.SzenzorBemenet.EmulaltSzenzor;
            }
            try
            {
                MF.HoMeRok.listView1.Invoke(new ControlStringConsumer(SetText), new object[] { MF.HoMeRok.listView1 });
            }
            catch { }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                Program.BEMUTATO_SzenzBemenet = Program.SzenzorBemenet.BelsoSzenzor;
            }
            else if (radioButton2.Checked)
            {
                Program.BEMUTATO_SzenzBemenet = Program.SzenzorBemenet.KulsoSzenzor;
            }
            else if (radioButton3.Checked)
            {
                Program.BEMUTATO_SzenzBemenet = Program.SzenzorBemenet.EmulaltSzenzor;
            }
            try
            {
                MF.HoMeRok.listView1.Invoke(new ControlStringConsumer(SetText), new object[] { MF.HoMeRok.listView1 });
            }
            catch { }
        }

        public delegate void ControlStringConsumer(ListView control);  // defines a delegate type

        public void SetText(ListView control)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new ControlStringConsumer(SetText), new object[] { control });  // invoking itself
            }
            else
            {
                control.Items.Clear();   // the "functional part", executing only on the main thread
            }
        }

        private void checkBoxEmuFans_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxEmuFans.Checked)
                Program.BelsoVentiVezTip = Program.BelsoVentiTipus.Emulalt;
            else
                Program.BelsoVentiVezTip = Program.BelsoVentiTipus.Hardware;
        }

        private void checkBoxRandomSpeeds_CheckedChanged(object sender, EventArgs e)
        {
            Program.DEVRandomDefSpeeds = checkBoxRandomSpeeds.Checked;
        }

        private void checkBoxTopMost_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = Program.DEVTopMost = checkBoxTopMost.Checked;
        }

        private void checkBoxKotottCOM_CheckedChanged(object sender, EventArgs e)
        {
            Program.DEVCOMReconnect = Program.DEVCOMPortKotese = checkBoxKotottCOM.Checked;
        }

        private void textBoxCOM_TextChanged(object sender, EventArgs e)
        {
            Program.DEVKotottCOMPort = textBoxCOM.Text;
        }

        private void numericUpDownFanNumber_ValueChanged(object sender, EventArgs e)
        {
            Program.DEVEmulaltVentiszam = (int)numericUpDownFanNumber.Value;
        }
    }
}
