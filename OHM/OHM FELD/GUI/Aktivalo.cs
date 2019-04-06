using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace OpenHardwareMonitor.GUI
{
    public partial class Aktivalo : Form
    {
        public Aktivalo()
        {
            InitializeComponent();

            try
            {
                if (Program.LICENSZNev != "-")
                {
                    textBoxNev.Text = Program.LICENSZNev;
                }
                if (Program.LICENSZEmail != "-")
                {
                    textBoxEmail.Text = Program.LICENSZEmail;
                }
                if (Program.LICENSZID != "-")
                {
                    textBoxLicID.Text = Program.LICENSZID;
                }
                if (Program.LICENSZJelszo != "-")
                {
                    textBoxJelszo.Text = Program.LICENSZJelszo;
                }
            }
            catch { }

            AddPlaceholder(textBoxEmail, new EventArgs());
            AddPlaceholder(textBoxNev, new EventArgs());
            AddPlaceholder(textBoxJelszo, new EventArgs());
            AddPlaceholder(textBoxLicID, new EventArgs());

            textBoxEmail.GotFocus += ReomvePlaceholder;
            textBoxNev.GotFocus += ReomvePlaceholder;
            textBoxJelszo.GotFocus += ReomvePlaceholder;
            textBoxLicID.GotFocus += ReomvePlaceholder;

            textBoxEmail.LostFocus += AddPlaceholder;
            textBoxNev.LostFocus += AddPlaceholder;
            textBoxJelszo.LostFocus += AddPlaceholder;
            textBoxLicID.LostFocus += AddPlaceholder;


            timerClipboardFigyelo.Elapsed += delegate { this.BeginInvoke((Action)delegate() { HitAdatBeilleszto(); }); };
            timerClipboardFigyelo.Enabled = true;
        }
        System.Timers.Timer timerClipboardFigyelo = new System.Timers.Timer() { Interval = 300, AutoReset = true };

        string PlaceholderText = "Some Magical Text here...";
        void ReomvePlaceholder(object sender, EventArgs e)
        {
            try
            {
                if ((sender as TextBox).Text == PlaceholderText)
                {
                    (sender as TextBox).Text = "";
                    (sender as TextBox).ForeColor = Color.Black;
                }
            }
            catch { }
        }

        void AddPlaceholder(object sender, EventArgs e)
        {
            try
            {
                if ((sender as TextBox).Text == "")
                {
                    (sender as TextBox).ForeColor = Color.LightGray;
                    (sender as TextBox).Text = PlaceholderText;
                }
            }
            catch { }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(19981001);
        }

        public string BezarasiUzenet = "";
        bool belsobezaras = false;
        private void Aktivalo_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!belsobezaras)
                e.Cancel = true;

            belsobezaras = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Program.LICENSZID = textBoxLicID.Text;
                Program.LICENSZNev = textBoxNev.Text;
                Program.LICENSZEmail = textBoxEmail.Text;
                Program.LICENSZJelszo = textBoxJelszo.Text;

                Vedelem.cookieJar = new CookieContainer();
                string[] sorok = Vedelem.Onlinehitelesites(2).Split('\n');

                switch (sorok[0])
                {
                    case "ervenyes_nullmach":
                    case "ervenyes_egyezomach":
                    case "ervenyes_nemkellmach":
                        {
                            Program.LICENSZERVENYESSEG = sorok[1];

                            try
                            {
                                Program.LICENSZTipus = Convert.ToInt32(sorok[3]);
                            }
                            catch
                            {
                                Program.LICENSZTipus = 10;
                            }

                            try
                            {
                                if (sorok.Length > 4 && sorok[4] == "1")
                                    Program.LICENSZProbTeljVerz = true;
                                else
                                    Program.LICENSZProbTeljVerz = false;
                            }
                            catch
                            { }

                            Fajlkezelo.HitelesitoAdatTitkositATLANIr();

                            byte[] bt = (Vedelem.HitfajlTitkosit(Program.LICENSZJelszo + Vedelem.GetMachID(), Program.LICENSZNev + Program.LICENSZEmail + Program.LICENSZID + Program.LICENSZJelszo + Program.LICENSZERVENYESSEG + Program.LICENSZTipus.ToString()));
                            if (!Fajlkezelo.HitelesitoAdatTitkositOTTBeolvas().SequenceEqual(bt))
                                Fajlkezelo.HitelesitoAdatTitkositOTTIr(bt);

                            string ervenyesseg = "";
                            try
                            {
                                string[] erv = Program.LICENSZERVENYESSEG.Split('.');
                                DateTime dt = new DateTime(int.Parse(erv[0]), int.Parse(erv[1]), int.Parse(erv[2]));//Nyelvterülethez valo dátumformázás
                                ervenyesseg = dt.ToShortDateString();
                            }
                            catch
                            {
                                try { ervenyesseg = Program.LICENSZERVENYESSEG; } catch { }
                            }

                            string aktsz = "Number of activation: N/A";
                            if (sorok[0] == "ervenyes_nemkellmach")
                            {
                                aktsz = "Special licence. Unlimited Physical Devices.";
                            }
                            else
                            {
                                try
                                {
                                    aktsz = "Number of activation: " + sorok[2];
                                }
                                catch
                                { }
                            }


                            BezarasiUzenet = "Activation succeeded!\n" + Vedelem.GetLicenszTipStringnevFromInt() + "\nValid until " + ervenyesseg + "\n" + aktsz;

                            belsobezaras = true;
                            this.Close();
                            break;
                        }
                    case "lejart":
                        {
                            string ervenyesseg = "";
                            try
                            {
                                string[] erv = sorok[1].Split('.');
                                DateTime dt = new DateTime(int.Parse(erv[0]), int.Parse(erv[1]), int.Parse(erv[2]));//Nyelvterülethez valo dátumformázás
                                ervenyesseg = dt.ToShortDateString();
                            }
                            catch
                            {
                                try { ervenyesseg = Program.LICENSZERVENYESSEG; } catch { }
                            }

                            MessageBox.Show("Your licence expired!\n" + ervenyesseg, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            break;
                        }
                    case "foglaltmach":
                        {//TÖRLŐKÓD E-MAIL KÉRÉS
                         //if (MessageBox.Show("Az eszközazonosító megváltozott, ezért a folytatáshoz újra kell aktiválnia a példányt.\nEzt egy e-mail-ben kapott linkkel teheti meg.\n\nAkarja, hogy újraaktiváló linket küldjünk?", "Vezérlőszoftver újraaktiválása", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                            if (MessageBox.Show("Machine ID has changed. You have to reactivate this instance to continue.\nYou can do this with a link received in e-mail.\n\nDo you want us to send you a reactivating e-mail?", "Reactivating control software", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                            {
                                string valasz = "";
                                try
                                {
                                    string postdata = "hostmuv=" + "torloemailkuld" +
                                                "&licid=" + Uri.EscapeDataString(Vedelem.EncodeBase64(Vedelem.AESTitkosit(Encoding.UTF8.GetBytes(textBoxLicID.Text), Vedelem.BKommInfo.AESKulcs, Vedelem.BKommInfo.AESIV, Vedelem.BKommInfo.AESBlokkMeret, Vedelem.BKommInfo.AESKulcsMeret))) +
                                                "&nev=" + Uri.EscapeDataString(Vedelem.EncodeBase64(Vedelem.AESTitkosit(Encoding.UTF8.GetBytes(textBoxNev.Text), Vedelem.BKommInfo.AESKulcs, Vedelem.BKommInfo.AESIV, Vedelem.BKommInfo.AESBlokkMeret, Vedelem.BKommInfo.AESKulcsMeret))) +
                                                "&email=" + Uri.EscapeDataString(Vedelem.EncodeBase64(Vedelem.AESTitkosit(Encoding.UTF8.GetBytes(textBoxEmail.Text), Vedelem.BKommInfo.AESKulcs, Vedelem.BKommInfo.AESIV, Vedelem.BKommInfo.AESBlokkMeret, Vedelem.BKommInfo.AESKulcsMeret))) +
                                                "&jelszo=" + Uri.EscapeDataString(Vedelem.EncodeBase64(Vedelem.AESTitkosit(Encoding.UTF8.GetBytes(textBoxJelszo.Text), Vedelem.BKommInfo.AESKulcs, Vedelem.BKommInfo.AESIV, Vedelem.BKommInfo.AESBlokkMeret, Vedelem.BKommInfo.AESKulcsMeret))) +
                                                "&machid=" + Uri.EscapeDataString(Vedelem.EncodeBase64(Vedelem.AESTitkosit(Encoding.UTF8.GetBytes(Vedelem.EncodeBase64(Vedelem.GetMachID())), Vedelem.BKommInfo.AESKulcs, Vedelem.BKommInfo.AESIV, Vedelem.BKommInfo.AESBlokkMeret, Vedelem.BKommInfo.AESKulcsMeret)));
                                    valasz = Vedelem.RequestPOST(Program.SzerverDomain + "/SZCSHost/LicenszHost.php", postdata, 27000);
                                }
                                catch (TimeoutException)
                                {
                                    //MessageBox.Show("Az e-mail küldése feltehetőleg tovább tart a szokásosnál.\nAmennyiben 10+ prec után sem kap levelet, kérjük, ismételje meg a küldést!\n\nSzíves elnézését kérjük a kellemetlenségért!", "Szerver túlterheltség", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    MessageBox.Show("Sending will be probably longer than general.\nIf you do not receive yor message in 10+ minutes, please, repeat the process!\n\nWe apologise for the failure!", "Server is overloaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                try
                                {
                                    sorok = valasz.Split('\n');
                                    switch (sorok[0])
                                    {
                                        case "torloemailnemlettelkuldve":
                                            {
                                                //MessageBox.Show("Az e-mail az adatok tulajdonságaiból kifolyólag nem került elküldésre.", "Szükségtelen küldés", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                MessageBox.Show("E-mail was not sent according to the properties of your licence data.", "Unnecessary sending", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                break;
                                            }
                                        case "torloemailsikeres":
                                            {
                                                //MessageBox.Show("Sikeres küldés.\nMiután az e-mail segítségével deaktiválta a régi eszközazonosítót, kattintson ebben az ablakban ismét az Aktiválás gombra.\n\nHa nem kapta meg az e-mailt, kérjük, próbálja újra.", "Siker", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                MessageBox.Show("Sending succeeded.\nAfter you deactivate your old machine ID by using the e-mail, click Activation button in this window again.\n\nIf you didn't receive an e-mail, please, try again.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                break;
                                            }
                                        case "torloemailkuldhiba":
                                        default:
                                            {
                                                MessageBox.Show("Server error during sending of the e-mail!\nPlease try again!", "Server error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                break;
                                            }
                                    }
                                }
                                catch
                                {
                                    MessageBox.Show("Error during sending of the e-mail!\nPlease try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }

                            break;
                        }
                    case "nincsilyen1":
                    case "nincsilyen2":
                        {
                            MessageBox.Show("Invalid licence!", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            break;
                        }
                    case "akttullepes":
                        {

                            MessageBox.Show("Reactivation limit is reached!", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            break;
                        }
                    default:
                        {
                            MessageBox.Show("Authentication failed!\nPlease, try again!\n\n(You should check your internet connection!)", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                }
            }
            catch
            {
                MessageBox.Show("Authentication failed!\nPlease, try again!\n\n(You should check your internet connection!)", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
        void RegisterAccountOnline()
        {
            try
            {
                System.Diagnostics.Process.Start(Program.SzerverDomain + "/register/#toregister");
            }
            catch { }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            RegisterAccountOnline();
        }

        int MegjelenesKod = 1;
        void SetVisibleMegjelenites(int i)
        {
            switch (i)
            {
                case 1:
                    {
                        textBoxNev.Visible = textBoxEmail.Visible = textBoxJelszo.Visible = textBoxLicID.Visible = label4.Visible = label3.Visible = label6.Visible = label5.Visible = label2.Visible = label1.Visible = button1.Visible = button2.Visible = button3.Visible = false;
                        button4.Visible = button5.Visible = true;
                        MegjelenesKod = 1;
                        break;
                    }
                case 2:
                    {
                        textBoxNev.Visible = textBoxEmail.Visible = textBoxJelszo.Visible = textBoxLicID.Visible = label4.Visible = label3.Visible = label6.Visible = label5.Visible = label2.Visible = label1.Visible = button1.Visible = button2.Visible = button3.Visible = true;
                        button4.Visible = button5.Visible = false;
                        MegjelenesKod = 2;
                        break;
                    }
            }
        }
        void HitAdatBeilleszto()
        {
            if (Clipboard.ContainsText())
            {
                string s = Clipboard.GetText();

                if (s.StartsWith("FeverkillHitAdat19981001"))
                {
                    s = s.Remove(0, "FeverkillHitAdat19981001".Length);
                    
                    string s2 = "";
                    foreach (var item in s)
                    {
                        if (item != '\r')
                            s2 += item;
                    }
                    if(s2[0] == '\n')
                    {
                        s2 = s2.Remove(0, 1);
                    }

                    string[] buff = s2.Split('\n');

                    if (buff.Length == 4)
                    {
                        textBoxNev.Text = buff[0];
                        textBoxEmail.Text = buff[1];
                        textBoxJelszo.Text = buff[2];
                        textBoxLicID.Text = buff[3];

                        textBoxEmail.ForeColor = Color.Black;
                        textBoxNev.ForeColor = Color.Black;
                        textBoxJelszo.ForeColor = Color.Black;
                        textBoxLicID.ForeColor = Color.Black;

                        Clipboard.Clear();

                        if (MegjelenesKod == 1)
                            SetVisibleMegjelenites(2);
                    }
                }
            }
        }

        private void Aktivalo_MouseEnter(object sender, EventArgs e)
        {
            HitAdatBeilleszto();
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            HitAdatBeilleszto();
        }

        private void textBoxLicID_MouseEnter(object sender, EventArgs e)
        {
            HitAdatBeilleszto();
        }

        private void textBoxJelszo_MouseEnter(object sender, EventArgs e)
        {
            HitAdatBeilleszto();
        }

        private void textBoxEmail_MouseEnter(object sender, EventArgs e)
        {
            HitAdatBeilleszto();
        }

        private void textBoxNev_MouseEnter(object sender, EventArgs e)
        {
            HitAdatBeilleszto();
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            HitAdatBeilleszto();
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            HitAdatBeilleszto();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MegjelenesKod == 1)
                SetVisibleMegjelenites(2);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            RegisterAccountOnline();
        }
    }
}
