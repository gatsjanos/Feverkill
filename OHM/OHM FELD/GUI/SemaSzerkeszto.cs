using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Aga.Controls.Tree;
using OpenHardwareMonitor.Hardware;
using UdvozloKepernyo;
using System.Diagnostics;
using OpenHardwareMonitor.Eszkozok;

namespace OpenHardwareMonitor.GUI
{
    public partial class SemaSzerkeszto : Form
    {
        internal TrackBar[] Csuszkak;
        Label[] Szazalekok;
        Label[] Hofokcimkek;
        CheckBox[] CBInt;
        List<Program.HoMers> HoMersek;


        int SzerkesztendoListaIndex;
        string EredetiListanev;

        bool UjSemaLetrehozas = false;
        public SemaSzerkeszto(string Listanev)//SZERKESZTÉS..SZERKESZTÉS..SZERKESZTÉS..SZERKESZTÉS..SZERKESZTÉS..SZERKESZTÉS..SZERKESZTÉS..SZERKESZTÉS..SZERKESZTÉS..SZERKESZTÉS..SZERKESZTÉS__
        {
            Program.SZLIST_SZERK_MENT = null;

            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Program.KONFNyelv);
            InitializeComponent();

            SemaKezelo.SajatHelpekMegejenites(false);
            SajatHelpekHozzaadas();
            SajatHelpekMegejenites(true);

            Program.OsszesForm.Add(this);
            this.TopMost = Program.KONFFelulMarado;

            if (Program.KONFNyelv != "hun")
            {
                this.Text = Eszk.GetNyelvSzo("LSZERKModCIM");
                button1.Text = Eszk.GetNyelvSzo("LSZERKModMentGomb");
                Lokalizalj();
            }

            //CheckForIllegalCrossThreadCalls = false;

            this.Menu = Program.FoAblak.mainMenu.CloneMenu();
            for (int i = 0; i < this.Menu.MenuItems.Count; i++)
            {
                this.Menu.MenuItems[i].Visible = false;
            }
            this.Size = new Size(Width, Height - 39); ;

            EredetiListanev = Listanev;
            textBox1.Text = EredetiListanev;

            CsuszkaKeszito(new Size(32, 70));

            Program.SzabListak = Fajlkezelo.SZabListBeolvas();
            Program.Attekint.listViewSzablistak.Dispatcher.Invoke(Program.FoAblak.AttekintSzablistFrissit);

            SzerkesztendoListaIndex = Fajlkezelo.HanyadikEzAzElemSZERKESZTES(EredetiListanev, Program.SzabListak);
            HomersLekero();
            if (HoMersek.Count != 0)
            {
                comboBox1.SelectedIndex = Fajlkezelo.HomeroIndexMegallapito(HoMersek, Program.SzabListak[SzerkesztendoListaIndex].Homero);
            }
            else
            {
                MessageBox.Show(((Program.KONFNyelv == "hun") ? "Nem található hőszenzor a számítógépben. Módosíthatja a szabályzólistát, de hozzárendelt hőmérő nélkül az nem lesz hatással a vezérlésre.\n\nKésőbb bármikor beállíthatja a listához tartozó hőszenzort." : Eszk.GetNyelvSzo("LSZERKNincsSzenzorMboxSZOVEG")), ((Program.KONFNyelv == "hun") ? "Nem található hőmérő!" : Eszk.GetNyelvSzo("LSZERKNincsSzenzorMboxCIM")), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Program.Dolgozott = false;
            }

            string[] alaplapi;
            try
            {
                if (Program.BelsoVentiVezTip == Program.BelsoVentiTipus.Hardware)
                {
                    foreach (TreeNodeAdv item in Program.FoAblak.treeView.AllNodes)
                    {
                        foreach (NodeControlInfo NCInfo in Program.FoAblak.treeView.GetNodeControls(item))
                        {
                            SensorNode snd = NCInfo.Node.Tag as SensorNode;
                            if (snd != null)
                                if (snd.Sensor != null)
                                    if (snd.Sensor.Control != null)
                                    {
                                        IControl control = snd.Sensor.Control;
                                        //MessageBox.Show(control.Identifier.ToString() + "/" + snd.Text, "Control Azonosító");

                                        if (control.Identifier.ToString().ToLower().Contains("control"))
                                        {

                                            ListViewItem LVItemx = new System.Windows.Forms.ListViewItem(control.Identifier + " => " + snd.Text);
                                            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] { LVItemx });

                                            alaplapi = Program.SzabListak[SzerkesztendoListaIndex].Csatornak.Split('|');
                                            for (int i = 1; i < alaplapi.Length; i++)
                                            {
                                                if (alaplapi[i] == control.Identifier + " => " + snd.Text)
                                                {
                                                    listView1.Items[listView1.Items.Count - 1].Checked = true;
                                                    break;
                                                }
                                            }

                                            break;
                                        }
                                    }
                        }
                    }
                }
                else if (Program.BelsoVentiVezTip == Program.BelsoVentiTipus.Emulalt)
                {
                    foreach (Program.EmulaltBelsoVenti control in Program.EmulaltBelsoVentik)
                    {
                        ListViewItem LVItemx = new System.Windows.Forms.ListViewItem(control.csoport + " => " + control.nev);
                        this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] { LVItemx });

                        alaplapi = Program.SzabListak[SzerkesztendoListaIndex].Csatornak.Split('|');
                        for (int i = 1; i < alaplapi.Length; i++)
                        {
                            if (alaplapi[i] == control.csoport + " => " + control.nev)
                            {
                                listView1.Items[listView1.Items.Count - 1].Checked = true;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Sajnálatos hiba történt:\n" + e.Message, "Hiba Történt! -- Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Program.Dolgozott = false;
                try { Dispose(true); } catch { }
            }


            VezTipListaalapu = Program.SzabListak[SzerkesztendoListaIndex].VezTipListaalapu;

            Csuszka_Cimke_CheckBoxErtekBeallito(SzerkesztendoListaIndex);


            if (textBox1.Text == "" && !textBox1.Focused)
                textBox1.Text = TextBoxNevAlapszoveg;

        }

        public SemaSzerkeszto(int SzenzorIndex)//LÉTREHOZÁS..LÉTREHOZÁS..LÉTREHOZÁS..LÉTREHOZÁS..LÉTREHOZÁS..LÉTREHOZÁS..LÉTREHOZÁS..LÉTREHOZÁS..LÉTREHOZÁS..LÉTREHOZÁS..LÉTREHOZÁS..LÉTREHOZÁS__
        {
            UjSemaLetrehozas = true;

            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Program.KONFNyelv);
            InitializeComponent();

            SzenzorListazo.SajatHelpekMegejenites(false);
            SajatHelpekHozzaadas();
            SajatHelpekMegejenites(true);

            Program.OsszesForm.Add(this);
            this.TopMost = Program.KONFFelulMarado;

            this.Text = (Program.KONFNyelv == "hun") ? "Állítsa be a szabályzólistát!" : Eszk.GetNyelvSzo("LSZERKLetrCIM");
            button1.Text = (Program.KONFNyelv == "hun") ? "Szabályzólista Mentése" : Eszk.GetNyelvSzo("LSZERKLetrMentGomb");

            if (Program.KONFNyelv != "hun")
                Lokalizalj();

            this.Menu = Program.FoAblak.mainMenu.CloneMenu();
            for (int i = 0; i < this.Menu.MenuItems.Count; i++)
            {
                this.Menu.MenuItems[i].Visible = false;
            }
            this.Size = new Size(Width, Height - 39); ;

            HomersLekero();
            comboBox1.SelectedIndex = SzenzorIndex;


            try
            {
                if (Program.BelsoVentiVezTip == Program.BelsoVentiTipus.Hardware)
                {
                    foreach (TreeNodeAdv item in Program.FoAblak.treeView.AllNodes)
                    {
                        foreach (NodeControlInfo NCInfo in Program.FoAblak.treeView.GetNodeControls(item))
                        {
                            SensorNode snd = NCInfo.Node.Tag as SensorNode;
                            if (snd != null)
                                if (snd.Sensor != null)
                                    if (snd.Sensor.Control != null)
                                    {
                                        IControl control = snd.Sensor.Control;
                                        //MessageBox.Show(control.Identifier.ToString() + "/" + snd.Text, "Control Azonosító");

                                        if (control.Identifier.ToString().ToLower().Contains("control"))
                                        {
                                            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] { new System.Windows.Forms.ListViewItem(control.Identifier + " => " + snd.Text) });
                                            break;
                                        }
                                    }
                        }
                    }
                }
                else if (Program.BelsoVentiVezTip == Program.BelsoVentiTipus.Emulalt)
                {
                    foreach (Program.EmulaltBelsoVenti control in Program.EmulaltBelsoVentik)
                    {
                        this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] { new System.Windows.Forms.ListViewItem(control.csoport + " => " + control.nev) });
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Sajnálatos hiba történt:\n" + e.Message, "Hiba Történt! --Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try { Dispose(true); } catch { }
            }
            CsuszkaKeszito(new Size(32, 70));

            Program.SzabListak = Fajlkezelo.SZabListBeolvas();
            Program.Attekint.listViewSzablistak.Dispatcher.Invoke(Program.FoAblak.AttekintSzablistFrissit);

            if (textBox1.Text == "" && !textBox1.Focused)
                textBox1.Text = TextBoxNevAlapszoveg;

            if(Program.SzabListak.Count == 0 && Program.KONFTutorialMegjelenit)
            {
                Program.TutorialWPFAblak.MutassMessagebox("Set your first control scheme and save it!", "Creating control shemes");
            }
        }

        void Lokalizalj()
        {
            button2.Text = Eszk.GetNyelvSzo("LSZERKInterpolacio");
            button3.Text = Eszk.GetNyelvSzo("LSZERKMindxre");
            label1.Text = Eszk.GetNyelvSzo("LSZERKDefSzablist");
            this.Ventilator.Text = Eszk.GetNyelvSzo("LSZERKAlaplapiVentik");
            this.groupBox1.Text = Eszk.GetNyelvSzo("LSZERKCelhCsat");
            this.label3.Text = Eszk.GetNyelvSzo("LSZERKFordszam");
            TextBoxNevAlapszoveg = Eszk.GetNyelvSzo("LSZERKTextboxNevAlapszoveg");
            button4.Text = Eszk.GetNyelvSzo("LSZERKFuggGorbe");
            buttonPIDValt.Text = Eszk.GetNyelvSzo("LSZERKValtPID");
        }

        void SajatHelpekHozzaadas()
        {
            // SegTestKezelo.SegtestHozzaad("A1", labelCim, "I'm CEO, Bitch...", 70, "Segoe Print", Brushes.Red, false, null, true);

            SegTestKezelo.SegtestHozzaad("SSzerk1", listView1, "Hover the mouse\nand select internal fans", 36, "Comic Sans MS", System.Windows.Media.Brushes.Red, false, false, new System.Windows.Point(-589, -102), true);
            SegTestKezelo.SegtestHozzaad("SSzerk2", groupBox1, "Select the channels\non the target hardware", 36, "Comic Sans MS", System.Windows.Media.Brushes.Red, false, false, new System.Windows.Point(-366, -131), true);
            SegTestKezelo.SegtestHozzaad("SSzerk3", buttonPIDValt, "Change between List/PID Control\n(advanced function)", 28, "Comic Sans MS", System.Windows.Media.Brushes.GreenYellow, false, false, new System.Windows.Point(80, 24), true);
            SegTestKezelo.SegtestHozzaad("SSzerk4", button4, "Try out mathematical curves", 28, "Comic Sans MS", System.Windows.Media.Brushes.GreenYellow, false, false, new System.Windows.Point(-268, -73), true);
            SegTestKezelo.SegtestHozzaad("SSzerk5", pictureBoxHelpSegitoCsuszkakhoz, "Set the speeds on the slides!\nEach slide corresponds to a temperature.\nSet them to create the characteristic of the current list!\n(try Interpolate and Each to ...% buttons)", 26, "Comic Sans MS", System.Windows.Media.Brushes.DeepSkyBlue, false, false, new System.Windows.Point(-331, -41), true);
            //SegTestKezelo.SegtestHozzaad("A3", labelCelh, "Click here to at/detach the Target Hardware\n(to disable error messages)", 28, "Comic Sans MS", Brushes.White, false, new Point(170, 45), true);
            //SegTestKezelo.SegtestHozzaad("A4", labelFrissId, "Change it to balance\nperformance and sensitivity", 28, "Comic Sans MS", Brushes.White, false, new Point(-249, 45), true);

            //SegTestKezelo.SegtestHozzaad("A5", VillFordszRectHelper, "Click here to open\na manual control window", 28, "Comic Sans MS", Brushes.GreenYellow, false, new Point(-193, -183), true);
            //SegTestKezelo.SegtestHozzaad("A6", VillSemaRectHelper, "Click here to manage Control Schemes", 28, "Comic Sans MS", Brushes.GreenYellow, false, new Point(-331, -169), true);
            //SegTestKezelo.SegtestHozzaad("A7", VillHomRectHelper, "Click here to open\ntemperatures window", 28, "Comic Sans MS", Brushes.GreenYellow, false, new Point(-397, 60), true);
            //SegTestKezelo.SegtestHozzaad("A8", VillRiasztRectHelper, "Click here to manage Alerts", 28, "Comic Sans MS", Brushes.GreenYellow, false, new Point(-380, 14), true);

            //SegTestKezelo.SegtestHozzaad("A9", OldalTutorRectHelper, "Show/Hide hints", 28, "Segoe Print", Brushes.DeepSkyBlue, false, new Point(68, -67), true);
            //SegTestKezelo.SegtestHozzaad("A10", OldalFoablRectHelper, "Show/Hide Advanced window", 28, "Comic Sans MS", Brushes.DeepSkyBlue, false, new Point(107, -71), true);
            //SegTestKezelo.SegtestHozzaad("A11", OldalElrejtRectHelper, "Hide this window", 28, "Comic Sans MS", Brushes.DeepSkyBlue, false, new Point(78, -60), true);
        }
        public static void SajatHelpekMegejenites(bool lathato)
        {
            SegTestKezelo.SetMegjelenites("SSzerk1", lathato);
            SegTestKezelo.SetMegjelenites("SSzerk2", lathato);
            SegTestKezelo.SetMegjelenites("SSzerk3", lathato);
            SegTestKezelo.SetMegjelenites("SSzerk4", lathato);
            SegTestKezelo.SetMegjelenites("SSzerk5", lathato);
        }
        public static void SajatHelpekListaMegejenitesre()
        {
            SegTestKezelo.SetMegjelenites("SSzerk1", true);
            SegTestKezelo.SetMegjelenites("SSzerk2", true);
            SegTestKezelo.SetMegjelenites("SSzerk3", true);
            SegTestKezelo.SetMegjelenites("SSzerk4", true);
            SegTestKezelo.SetMegjelenites("SSzerk5", true);
        }
        public static void SajatHelpekPIDMegejenitesre()
        {
            SegTestKezelo.SetMegjelenites("SSzerk1", true);
            SegTestKezelo.SetMegjelenites("SSzerk2", true);
            SegTestKezelo.SetMegjelenites("SSzerk3", true);
            SegTestKezelo.SetMegjelenites("SSzerk4", false);
            SegTestKezelo.SetMegjelenites("SSzerk5", false);
        }

        void HomersLekero()
        {
            HoMersek = Program.FoAblak.HommMersek;

            for (int i = 0; i < HoMersek.Count; i++)
            {
                comboBox1.Items.Add(HoMersek[i].Csop + " =->> " + HoMersek[i].Nev + " >>> " + HoMersek[i].Ertek);
            }
        }
        void CsuszkaKeszito(Size kezdhely)
        {
            string PIDlink = "https://www.csimn.com/CSI_pages/PIDforDummies.html";
            linkLabel1.Links.Add(0, PIDlink.Length, PIDlink);

            Csuszkak = new TrackBar[46];
            Szazalekok = new Label[46];
            Hofokcimkek = new Label[46];
            CBInt = new CheckBox[46];

            for (int i = 0; i < 46; i++)
            {
                Label labelx = new Label();
                labelx.AutoSize = true;
                labelx.BackColor = Color.White;
                labelx.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                labelx.Location = new System.Drawing.Point(kezdhely.Width + i * 26, kezdhely.Height);
                if (i * 2 + 20 < 100)
                    labelx.Text = Convert.ToString(i * 2 + 20) + " °C";
                else
                    labelx.Text = Convert.ToString(i * 2 + 20) + " °";

                labelx.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

                if (i % 5 == 0)
                    labelx.ForeColor = Color.Salmon;
                if (i % 15 == 0)
                    labelx.ForeColor = Color.Red;

                Controls.Add(labelx);
                Hofokcimkek[i] = labelx;
            }

            for (int i = 45; i >= 0; --i)
            {
                Label labelx = new Label();
                labelx.AutoSize = true;
                labelx.BackColor = Color.White;
                labelx.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                labelx.Location = new System.Drawing.Point(kezdhely.Width + 5 + i * 26, kezdhely.Height + 182);
                labelx.Text = "0\n%";
                labelx.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                if (i % 5 == 0)
                    labelx.ForeColor = Color.Salmon;
                if (i % 15 == 0)
                    labelx.ForeColor = Color.Red;

                Controls.Add(labelx);
                Szazalekok[i] = labelx;
            }


            for (int i = 45; i >= 0; --i)
            {
                TrackBar trackbarx = new TrackBar();
                trackbarx.BackColor = Color.White;
                trackbarx.MouseEnter += new System.EventHandler(ListaSzerkeszto_MouseEnter);
                trackbarx.Location = new System.Drawing.Point(kezdhely.Width + 2 + i * 26, kezdhely.Height + 5);
                trackbarx.Maximum = 100;
                trackbarx.SmallChange = 1;
                trackbarx.LargeChange = 5;
                trackbarx.Orientation = System.Windows.Forms.Orientation.Vertical;
                trackbarx.Size = new System.Drawing.Size(45, 180);
                trackbarx.TabIndex = 4;
                trackbarx.TickStyle = System.Windows.Forms.TickStyle.None;
                int index = i;
                trackbarx.ValueChanged += delegate (object sender, EventArgs e)
                    {
                        Szazalekok[index].Text = trackbarx.Value + "\n%";
                    };

                Controls.Add(trackbarx);
                Csuszkak[i] = trackbarx;
            }

            for (int i = 45; i >= 0; --i)
            {
                CheckBox CBx = new CheckBox();
                CBx.AutoSize = true;
                CBx.Location = new System.Drawing.Point(kezdhely.Width + 5 + i * 26, kezdhely.Height + 210);
                CBx.Name = "checkBoxIntOssz";
                CBx.Size = new System.Drawing.Size(15, 14);
                CBx.TabIndex = 11;
                CBx.UseVisualStyleBackColor = true;
                Controls.Add(CBx);
                CBInt[i] = CBx;
            }

        }
        void Csuszka_Cimke_CheckBoxErtekBeallito(int ListIndex)
        {
            if (VezTipListaalapu)
            {
                for (int i = 0; i < 46; i++)
                {
                    Csuszkak[i].Value = Program.SzabListak[ListIndex].PWM[i];
                }
            }
            else
            {
                numericUpDownCel.Value = (decimal)Program.SzabListak[ListIndex].PIDObjektum.Setpoint;
                numericUpDownP.Value = (decimal)Program.SzabListak[ListIndex].PIDObjektum.Kp;
                numericUpDownI.Value = (decimal)Program.SzabListak[ListIndex].PIDObjektum.Ki;
                numericUpDownD.Value = (decimal)Program.SzabListak[ListIndex].PIDObjektum.Kd;
                numericUpDownIntegralBeleszamitas.Value = (decimal)Program.SzabListak[ListIndex].PIDObjektum.BeleszamitasIntegralVisszamenolegms / (decimal)1000;
                numericUpDownDerivaltBeleszamitas.Value = (decimal)Program.SzabListak[ListIndex].PIDObjektum.BeleszamitasDerivaltVisszamenolegms / (decimal)1000;

                Timer timerPIDraValto = new Timer() { Interval = 20 };//Timerezni kell, mert ha itt azonnal átváltok PID megjelenítésre, akkor a csúszkák összebaszódnak
                timerPIDraValto.Tick += delegate { timerPIDraValto.Stop(); PIDMegjelenitesre(); };
                timerPIDraValto.Start();

            }
            if (Program.SzabListak[ListIndex].Csatornak.Contains(",1,"))
            {
                checkBox1.Checked = true;
            }
            if (Program.SzabListak[ListIndex].Csatornak.Contains(",2,"))
            {
                checkBox2.Checked = true;
            }
            if (Program.SzabListak[ListIndex].Csatornak.Contains(",3,"))
            {
                checkBox3.Checked = true;
            }
            if (Program.SzabListak[ListIndex].Csatornak.Contains(",4,"))
            {
                checkBox4.Checked = true;
            }
            if (Program.SzabListak[ListIndex].Csatornak.Contains(",5,"))
            {
                checkBox5.Checked = true;
            }
            if (Program.SzabListak[ListIndex].Csatornak.Contains(",6,"))
            {
                checkBox6.Checked = true;
            }
            if (Program.SzabListak[ListIndex].Csatornak.Contains(",7,"))
            {
                checkBox7.Checked = true;
            }
            if (Program.SzabListak[ListIndex].Csatornak.Contains(",8,"))
            {
                checkBox8.Checked = true;
            }
        }

        string TextBoxNevAlapszoveg = "Adja meg a szabályzóséma kívánt nevét!";
        private void textBox1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Contains(TextBoxNevAlapszoveg))
            {
                textBox1.Text = textBox1.Text.Replace(TextBoxNevAlapszoveg, "");
                textBox1.ForeColor = Color.Black;
            }
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBox1.Text.Contains(TextBoxNevAlapszoveg))
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }
        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = TextBoxNevAlapszoveg;
                textBox1.ForeColor = Color.Gray;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.Dolgozott = false;
            if (textBox1.Text.Contains(TextBoxNevAlapszoveg) || (textBox1.Text == ""))
            {
                MessageBox.Show(((Program.KONFNyelv == "hun") ? "Adjon meg egy nevet a szabályzólistának!" : Eszk.GetNyelvSzo("NevhibaNincs")), ((Program.KONFNyelv == "hun") ? "Névhiba!" : Eszk.GetNyelvSzo("Névhiba!")), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                textBox1.Text = TextBoxNevAlapszoveg;
                textBox1.ForeColor = Color.Gray;
            }
            else
            {
                bool vanmarilyen = false;
                foreach (Program.SzabLista item in Program.SzabListak)
                {
                    if (item.Nev == textBox1.Text && textBox1.Text != EredetiListanev)
                    {
                        vanmarilyen = true;
                        MessageBox.Show(((Program.KONFNyelv == "hun") ? "Ilyen nevű szabályzólista már létezik!" : Eszk.GetNyelvSzo("NevhibaVanilyen")), ((Program.KONFNyelv == "hun") ? "Névhiba!" : Eszk.GetNyelvSzo("Névhiba!")), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    }
                }

                if (!vanmarilyen)
                {
                    Program.SzabLista SzabListMent = new Program.SzabLista();

                    SzabListMent.Nev = textBox1.Text;
                    try { SzabListMent.Homero = HoMersek[comboBox1.SelectedIndex].Csop + " =->> " + HoMersek[comboBox1.SelectedIndex].Nev; } catch { SzabListMent.Homero = ""; }
                    SzabListMent.Csatornak = CsatornaMegallapit();

                    if (VezTipListaalapu)
                    {
                        SzabListMent.VezTipListaalapu = true;

                        SzabListMent.PWM = new byte[46];
                        for (int i = 0; i < 46; i++)
                        {
                            SzabListMent.PWM[i] = (byte)Csuszkak[i].Value;
                        }
                    }
                    else
                    {
                        SzabListMent.VezTipListaalapu = false;

                        SzabListMent.PIDObjektum.Setpoint = (double)numericUpDownCel.Value;
                        SzabListMent.PIDObjektum.Kp = (double)numericUpDownP.Value;
                        SzabListMent.PIDObjektum.Ki = (double)numericUpDownI.Value;
                        SzabListMent.PIDObjektum.Kd = (double)numericUpDownD.Value;
                        SzabListMent.PIDObjektum.BeleszamitasIntegralVisszamenolegms = (int)(numericUpDownIntegralBeleszamitas.Value * 1000);
                        SzabListMent.PIDObjektum.BeleszamitasDerivaltVisszamenolegms = (int)(numericUpDownDerivaltBeleszamitas.Value * 1000);
                    }
                    if (UjSemaLetrehozas)
                    {
                        Program.SzabListak.Add(SzabListMent);
                        Fajlkezelo.SZabListIr(Program.SzabListak);
                    }
                    else
                    {
                        Program.SZLIST_SZERK_MENT = SzabListMent;
                        Program.Dolgozott = true;
                    }

                    mentesselkilepes = true;
                    try { Dispose(true); }
                    catch { }
                }
            }
        }
        string CsatornaMegallapit()
        {
            string vissza = ",";

            if (checkBox1.Checked)
                vissza += "1,";
            if (checkBox2.Checked)
                vissza += "2,";
            if (checkBox3.Checked)
                vissza += "3,";
            if (checkBox4.Checked)
                vissza += "4,";
            if (checkBox5.Checked)
                vissza += "5,";
            if (checkBox6.Checked)
                vissza += "6,";
            if (checkBox7.Checked)
                vissza += "7,";
            if (checkBox8.Checked)
                vissza += "8,";


            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Checked == true)
                    vissza += "|" + item.Text;
            }

            return vissza;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == EredetiListanev)
                textBox1.ForeColor = Color.Blue;
            else if (textBox1.Text != TextBoxNevAlapszoveg)
                textBox1.ForeColor = Color.Black;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<int> Tampontok = new List<int>();
            for (int i = 0; i < CBInt.Length; i++)
            {
                if (CBInt[i].Checked)
                    Tampontok.Add(i);
            }

            if (Tampontok.Count == 0)
            {
                MessageBox.Show(((Program.KONFNyelv == "hun") ? "Állítson be legalább egy csúszkát a lineáris interpoláció támpontjának!\n(Az alatta lévő jelölőnégyzettel teheti meg ezt.)\n\nAz intepoláció során a megjelölt négyzetek közti csúszkák\n a szélső csúszkák értékein átmenő egyenesre illesztődnek." : Eszk.GetNyelvSzo("LSZERKInterpHibaSZOVEG")), ((Program.KONFNyelv == "hun") ? "Nem lehet interpolálni" : Eszk.GetNyelvSzo("LSZERKInterpHibaCIM")));
            }
            else if (Tampontok.Count == 1)
            {
                for (int x = 0; x < Csuszkak.Length; x++)
                {
                    Csuszkak[x].Value = Csuszkak[Tampontok[0]].Value;
                }
            }
            else
            {
                for (int x = 0; x < Tampontok[0]; x++)
                {
                    Csuszkak[x].Value = Csuszkak[Tampontok[0]].Value;
                }

                for (int i = 1; i < Tampontok.Count; i++)
                {
                    int[] interpolalt = interpolalj(Csuszkak[Tampontok[i - 1]].Value, Csuszkak[Tampontok[i]].Value, Tampontok[i] - Tampontok[i - 1]);

                    for (int x = 0; x < interpolalt.Length; x++)
                    {

                        Csuszkak[x + Tampontok[i - 1]].Value = interpolalt[x];
                    }
                }

                if (Tampontok[Tampontok.Count - 1] < Csuszkak.Length - 1)
                {
                    for (int x = Tampontok[Tampontok.Count - 1]; x < Csuszkak.Length; x++)
                    {
                        Csuszkak[x].Value = Csuszkak[Tampontok[Tampontok.Count - 1]].Value;
                    }
                }
            }
        }
        static int[] interpolalj(int Ertek1, int Ertek2, int hossz)
        {
            int maxminkul = 0;
            maxminkul = Ertek2 - Ertek1;

            int[] kitomb = new int[hossz];
            for (int i = 0; i < hossz; i++)
            {
                kitomb[i] = Ertek1 + (i * maxminkul / hossz);
            }
            return kitomb;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(((Program.KONFNyelv == "hun") ? "Biztos benne, hogy az összes csúszka értékét " : Eszk.GetNyelvSzo("LSZERKMindxMbox1")) + numericUpDown1.Value + ((Program.KONFNyelv == "hun") ? "%-ra állítja?" : Eszk.GetNyelvSzo("LSZERKMindxMbox2")), "", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                for (int x = 0; x < Csuszkak.Length; x++)
                {
                    Csuszkak[x].Value = (int)numericUpDown1.Value;
                }
            }
        }

        private void checkBoxIntOssz_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < CBInt.Length; i++)
            {
                CBInt[i].Checked = checkBoxIntOssz.Checked;
            }
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {

            try
            {
                if (listView1.Size.Height > 100)
                { listView1.Size = new Size(314, 47); listView1.Scrollable = false; }
                else
                { listView1.Size = new Size(314, 286); listView1.Scrollable = true; }
            }
            catch
            {
            }
        }
        private void listView1_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                listView1.Size = new Size(314, 286);
                listView1.Scrollable = true;
            }
            catch { }
        }

        private void ListaSzerkeszto_MouseEnter(object sender, EventArgs e)
        {

            try
            {
                listView1.Size = new Size(314, 47);
                listView1.Scrollable = false;
            }
            catch
            {
            }
            //if (!VezTipListaalapu)
            //    PIDMegjelenitesre();//Külön kellett választani a többi PID-s control megjelenítésétől, mert valamiért máshogy a csúszkák összecsúsztak
        }

        bool VezTipListaalapu = true;
        private void buttonPIDValt_Click(object sender, EventArgs e)
        {
            VezTipListaalapu = !VezTipListaalapu;
            if (VezTipListaalapu)
            {
                ListaalapuMegjelenitesre();
            }
            else
            {
                PIDMegjelenitesre();
            }

        }
        private void PIDMegjelenitesre()
        {
            SajatHelpekPIDMegejenitesre();
            buttonPIDValt.Text = ((Program.KONFNyelv == "hun") ? "Váltás Listaalapú vezérlésre" : Eszk.GetNyelvSzo("LSZERKValtLista"));
            groupBoxPID.Visible = labelPIDTutor.Visible = linkLabel1.Visible = true;
            label3.Visible = checkBoxIntOssz.Visible = button2.Enabled = button3.Enabled = button4.Enabled = numericUpDown1.Enabled = false;
            for (int i = 0; i < Csuszkak.Length; i++)
            {
                Csuszkak[i].Visible = Szazalekok[i].Visible = Hofokcimkek[i].Visible = CBInt[i].Visible = false;
            }
        }
        private void ListaalapuMegjelenitesre()
        {
            SajatHelpekListaMegejenitesre();
            buttonPIDValt.Text = ((Program.KONFNyelv == "hun") ? "Váltás PID alapú vezérlésre" : Eszk.GetNyelvSzo("LSZERKValtPID"));
            label3.Visible = checkBoxIntOssz.Visible = button2.Enabled = button3.Enabled = button4.Enabled = numericUpDown1.Enabled = true;
            groupBoxPID.Visible = labelPIDTutor.Visible = linkLabel1.Visible = false;
            for (int i = 0; i < Csuszkak.Length; i++)
            {
                Csuszkak[i].Visible = Szazalekok[i].Visible = Hofokcimkek[i].Visible = CBInt[i].Visible = true;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
                linkLabel1.LinkVisited = true;
            }
            catch { }
        }

        public void GorbeBeallito(List<double> ertekek)
        {
            List<int> ki = new List<int>();

            foreach (double item in ertekek)
            {
                ki.Add((int)Math.Round(item));
            }

            GorbeBeallito(ki);
        }
        public void GorbeBeallito(List<int> ertekek)
        {
            try
            {
                for (int i = 0; i < ertekek.Count; i++)
                {
                    Csuszkak[i].Value = ertekek[i];
                }
            }
            catch (IndexOutOfRangeException)
            { }
        }

        FuggvenyGorbekeszito FGGorbkeszito;
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (FGGorbkeszito != null)
                {
                    FGGorbkeszito.Dispose();
                }
            }
            catch
            { }
            try
            {
                FGGorbkeszito = new FuggvenyGorbekeszito(this);
                FGGorbkeszito.Show();
            }
            catch
            { }
        }

        bool mentesselkilepes = false;
        private void SemaSzerkeszto_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!mentesselkilepes)
            {
                if (MessageBox.Show("Do you want to quit without saving?\nYour operations on this control scheme will not be saved!", "What about saving?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (UjSemaLetrehozas)
                    {
                        SzenzorListazo.SajatHelpekMegejenites(true);
                    }
                    else
                    {
                        SemaKezelo.SajatHelpekMegejenites(true);
                    }
                    SajatHelpekMegejenites(false);
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

    }
}
