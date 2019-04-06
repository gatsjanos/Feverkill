using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Text;
using OpenHardwareMonitor.Eszkozok;

namespace OpenHardwareMonitor.GUI
{
    public partial class Visszajelzes : Form
    {
        OpenHardwareMonitor.GUI.FoAblak MF;
        public Visszajelzes(OpenHardwareMonitor.GUI.FoAblak MFbe)
        {
            InitializeComponent();
            Program.OsszesForm.Add(this);
            this.TopMost = Program.KONFFelulMarado;
            MF = MFbe;

            this.Menu = MF.mainMenu.CloneMenu();
            for (int i = 0; i < this.Menu.MenuItems.Count; i++)
            {
                this.Menu.MenuItems[i].Visible = false;
            }
            this.Size = new Size(Width, Height - 39); ;

            textBoxJelentes.Text = MF.computer.GetReport();

            if (Program.KONFNyelv != "hun")
                Lokalizalj();
        }

        void Lokalizalj()
        {
            this.Text = Eszk.GetNyelvSzo("VJELZCIM");
            this.label1.Text = Eszk.GetNyelvSzo("VJELZLab1");
            this.label2.Text = Eszk.GetNyelvSzo("VJELZLab2");
            this.label3.Text = Eszk.GetNyelvSzo("VJELZLab3");
            this.label4.Text = Eszk.GetNyelvSzo("VJELZLab4");
            this.label5.Text = Eszk.GetNyelvSzo("VJELZLab5");
            this.button1.Text = Eszk.GetNyelvSzo("VJELZBut1");
            this.checkBox1.Text = Eszk.GetNyelvSzo("VJELZCbox");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                button1.Enabled = false;
                string uzenet = "Feladó LicID-je: " + Program.LICENSZID + "\nFeladó megadott elérhetősége:" + textBoxEmailcim.Text + "\nVERZIÓSZÁM: " + Program.Verzioszam + "\n\n\n\n==============================\nVISSZAJELZÉS SZÖVEGE:\n" + textBoxUzenet.Text;
                if (checkBox1.Checked)
                    uzenet += "\n\n\n\n==============================\nHARDVER JELENTÉS:\n" + textBoxJelentes.Text;


                string valasz = "";
                try
                {
                    //string kurva = Uri.EscapeDataString(uzenet);
                    string kodoltuzenet = "";
                    for (int i = 0; i < uzenet.Length;)
                    {
                        kodoltuzenet += Uri.EscapeDataString(uzenet.Substring(i, (i + 50000 < uzenet.Length) ? 50000 : uzenet.Length - i));
                        i += 50000;
                    }

                    //string postdata = "hostmuv=visszajelzeskuld&szoveg=" + Uri.EscapeDataString(Vedelem.EncodeBase64(uzenet));
                    string postdata = "hostmuv=visszajelzeskuld&szoveg=" + kodoltuzenet;
                    valasz = Vedelem.RequestPOST(Program.SzerverDomain + "/SZCSHost/LicenszHost.php", postdata, 30000);
                }
                catch (TimeoutException)
                {
                    MessageBox.Show("Az e-mail küldése feltehetőleg tovább tart a szokásosnál, vagy sikertelen lehet.\nKérjük, ismételje meg a küldést később!\n\nSzíves elnézését kérjük a kellemetlenségért!", "Szerver túlterheltség", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    button1.Enabled = true;
                    return;
                }

                string[] sorok = valasz.Split('\n');
                switch (sorok[0])
                {
                    case "sikeresvisszajelzes":
                        {
                            Opacity = 0;

                            MessageBox.Show(((Program.KONFNyelv == "hun") ? "Köszönjük, hogy fáradozásával segíti munkánkat és segít jobbá tenni a Feverkill Ventilátorvezérlő rendszert!" : Eszk.GetNyelvSzo("VJELZMboxKoszonjukSZOVEG")), "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            Close();

                            break;
                        }
                    case "sikertelenvisszajelzes":
                    default:
                        {
                            throw new Exception();
                            break;
                        }
                }
                //MailMessage level = new MailMessage();
                //level.To.Add("gatsjanos@gmail.com");
                //level.From = new MailAddress("ventivezkuldo@gmail.com");
                //level.Subject = "Feverkill VENTILÁTOR VEZÉRLŐ VISSZAJELZÉS   " + DateTime.Now.ToString();
                //level.Body = uzenet;


                //SmtpClient smtp = new SmtpClient();
                //smtp.Host = "smtp.gmail.com";
                //smtp.Port = 587;
                //smtp.Credentials = new NetworkCredential(
                //    "ventivezkuldo@gmail.com", "qwertzuiopAbCd");
                //smtp.EnableSsl = true;
                //smtp.Send(level);

            }
            catch (Exception ex) { MessageBox.Show(((Program.KONFNyelv == "hun") ? "Hiba a küldés során!\nKérjük, próbálja újra!" : Eszk.GetNyelvSzo("VJELZMboxHibaSZOVEG")) + "\n\n" + ex.Message, ((Program.KONFNyelv == "hun") ? "Hiba törént!" : Eszk.GetNyelvSzo("Hiba!")), MessageBoxButtons.OK, MessageBoxIcon.Error); button1.Enabled = true; }
        }
    }
}
