using OpenHardwareMonitor.Eszkozok;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UdvozloKepernyo;

namespace OpenHardwareMonitor.GUI
{
    public partial class SemaKezelo : Form
    {
        bool NincsErvenyesitve = false;
        static List<Program.SzabLista> LV1SzabListak, LV2SzabListak;
        public SemaKezelo()
        {
            InitializeComponent();

            Attekinto.AttekintoWPF.SajatHelpekMegejenites(false);
            SajatHelpekHozzaadas();
            SajatHelpekMegejenites(true);

            Program.OsszesForm.Add(this);
            this.TopMost = Program.KONFFelulMarado;

            SegitsegBeallito();

            if (Program.KONFNyelv != "hun")
                Lokalizalj();

            LV1SzabListak = Fajlkezelo.DeepCopySzablista(Program.SzabListak);//Fajlkezelo.SZabListBeolvas();
            LV2SzabListak = Fajlkezelo.DeepCopySzablista(Program.Ervenyesek); //Fajlkezelo.ErvenyListBeolvas();

            bool vanilyen;
            for (int i = 0; i < LV2SzabListak.Count; i++)
            {
                vanilyen = false;
                for (int x = 0; x < Program.SzabListak.Count; x++)
                    if (LV2SzabListak[i].Nev == Program.SzabListak[x].Nev)
                    {
                        vanilyen = true;
                        break;
                    }
                if (!vanilyen)
                {
                    LV2SzabListak.RemoveAt(i);
                    --i;
                }
            }

            for (int i = 0; i < LV1SzabListak.Count; i++)
            {
                for (int x = 0; x < LV2SzabListak.Count; x++)
                    if (LV1SzabListak[i].Nev == LV2SzabListak[x].Nev)
                    {
                        LV1SzabListak.RemoveAt(i);
                        --i;
                        break;
                    }
            }

            //////////////////////////////////////////////////////////////////

            this.Menu = Program.FoAblak.mainMenu.CloneMenu();
            for (int i = 0; i < this.Menu.MenuItems.Count; i++)
            {
                this.Menu.MenuItems[i].Visible = false;
            }
            this.Size = new Size(Width, Height - 39); ;

            //Fajlkezelo.SZenzorBetolto(MFbe);

            Frissits();
            
            if (Program.Ervenyesek.Count == 0 && Program.SzabListak.Count != 0 && Program.KONFTutorialMegjelenit)
            {
                Program.TutorialWPFAblak.MutassMessagebox("To make the created control schemes working, you have to Activate them.\n\nSelect some schemes in the left side and move them to the right side. After that, click the Activation button. (The big red one in the center.)", "Activate control schemes!");
            }
            else if (Program.SzabListak.Count == 0 && Program.KONFTutorialMegjelenit)
            {
                Program.TutorialWPFAblak.MutassMessagebox("Here you can manage your control schemes.\n\nFollow the red \"Create new Control Scheme\" label now!", "Scheme management!");
            }
        }
        void SajatHelpekHozzaadas()
        {
            // SegTestKezelo.SegtestHozzaad("A1", labelCim, "I'm CEO, Bitch...", 70, "Segoe Print", Brushes.Red, false, null, true);

            SegTestKezelo.SegtestHozzaad("SK1", button7, "Create new Control Scheme", 40, "Comic Sans MS", System.Windows.Media.Brushes.Red, true, false, new System.Windows.Point(52, 47), true);
            SegTestKezelo.SegtestHozzaad("SK2", button6, "Activate schemes\nin the Active List", 32, "Comic Sans MS", System.Windows.Media.Brushes.Red, false, false, new System.Windows.Point(114, 9), true);
            SegTestKezelo.SegtestHozzaad("SK3", button2, "Move schemes into the Active list", 28, "Comic Sans MS", System.Windows.Media.Brushes.GreenYellow, false, false, new System.Windows.Point(-602, -123), true);
            SegTestKezelo.SegtestHozzaad("SK4", button4, "Move schemes into the Inctive list", 28, "Comic Sans MS", System.Windows.Media.Brushes.DeepSkyBlue, false, false, new System.Windows.Point(-635, -173), true);
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
            SegTestKezelo.SetMegjelenites("SK1", lathato);
            SegTestKezelo.SetMegjelenites("SK2", lathato);
            SegTestKezelo.SetMegjelenites("SK3", lathato);
            SegTestKezelo.SetMegjelenites("SK4", lathato);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.SelectedIndices.Count; i++)
            {
                LV2SzabListak.Add(LV1SzabListak[listView1.SelectedIndices[i]]);

            }

            for (int i = listView1.SelectedIndices.Count - 1; i >= 0; i--)
            {
                LV1SzabListak.RemoveAt(listView1.SelectedIndices[i]);
                NincsErvenyesitve = true;
            }

            Frissits();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView2.SelectedIndices.Count; i++)
            {
                LV1SzabListak.Add(LV2SzabListak[listView2.SelectedIndices[i]]);
            }

            for (int i = listView2.SelectedIndices.Count - 1; i >= 0; i--)
            {
                LV2SzabListak.RemoveAt(listView2.SelectedIndices[i]);
                NincsErvenyesitve = true;
            }

            Frissits();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; ++i)
            {
                LV2SzabListak.Add(LV1SzabListak[i]);
                NincsErvenyesitve = true;
            }
            LV1SzabListak.Clear();

            Frissits();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView2.Items.Count; ++i)
            {
                LV1SzabListak.Add(LV2SzabListak[i]);
                NincsErvenyesitve = true;
            }

            LV2SzabListak.Clear();

            Frissits();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label4.Text = listView1.SelectedIndices.Count + "/" + listView1.Items.Count + ((Program.KONFNyelv == "hun") ? "db" : Eszk.GetNyelvSzo("db"));
            label5.Text = listView2.SelectedIndices.Count + "/" + listView2.Items.Count + ((Program.KONFNyelv == "hun") ? "db" : Eszk.GetNyelvSzo("db"));
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            label4.Text = listView1.SelectedIndices.Count + "/" + listView1.Items.Count + ((Program.KONFNyelv == "hun") ? "db" : Eszk.GetNyelvSzo("db"));
            label5.Text = listView2.SelectedIndices.Count + "/" + listView2.Items.Count + ((Program.KONFNyelv == "hun") ? "db" : Eszk.GetNyelvSzo("db"));
        }

        private void törlésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.Focused && listView1.SelectedIndices.Count != 0)
            {
                if (MessageBox.Show(((Program.KONFNyelv == "hun") ? "Biztosan törli a kijelölt " : Eszk.GetNyelvSzo("LKIVTörl1")) + listView1.SelectedIndices.Count + ((Program.KONFNyelv == "hun") ? "db Szabályzósémát?\nEz a művelet nem vonható vissza!" : Eszk.GetNyelvSzo("LKIVTörl2")), ((Program.KONFNyelv == "hun") ? "SZABÁLYZÓSÉMÁK TÖRLÉSE" : Eszk.GetNyelvSzo("LKIVTörlCIM")), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    for (int i = 0; i < listView1.SelectedIndices.Count; i++)
                    {
                        for (int x = 0; x < LV1SzabListak.Count; x++)
                        {
                            if (LV1SzabListak[x].Nev == listView1.Items[listView1.SelectedIndices[i]].SubItems[0].Text)
                            {
                                LV1SzabListak.RemoveAt(x);
                                break;
                            }
                        }
                    }

                    for (int i = 0; i < listView1.SelectedIndices.Count; i++)
                    {
                        try
                        {
                            Program.SzabListak.RemoveAt(Fajlkezelo.HanyadikEzAzElem(listView1.Items[listView1.SelectedIndices[i]].SubItems[0].Text, Program.SzabListak));
                        }
                        catch (IndexOutOfRangeException) { }
                        catch (ArgumentOutOfRangeException) { }
                    }

                    Fajlkezelo.SZabListIr(Program.SzabListak);

                    for (int i = listView1.SelectedIndices.Count - 1; i >= 0; i--)
                    {
                        listView1.Items.RemoveAt(listView1.SelectedIndices[i]);
                    }

                    label4.Text = listView1.SelectedIndices.Count + "/" + listView1.Items.Count + ((Program.KONFNyelv == "hun") ? "db" : Eszk.GetNyelvSzo("db"));
                    Frissits();
                }
            }
            else if (listView2.SelectedIndices.Count != 0)
            {
                if (MessageBox.Show(((Program.KONFNyelv == "hun") ? "Biztosan törli a kijelölt " : Eszk.GetNyelvSzo("LKIVTörl1")) + listView2.SelectedIndices.Count + ((Program.KONFNyelv == "hun") ? "db Szabályzósémát?\nEz a művelet nem vonható vissza!" : Eszk.GetNyelvSzo("LKIVTörl2")), ((Program.KONFNyelv == "hun") ? "SZABÁLYZÓSÉMÁK TÖRLÉSE" : Eszk.GetNyelvSzo("LKIVTörlCIM")), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    for (int i = 0; i < listView2.SelectedIndices.Count; i++)
                    {
                        for (int x = 0; x < LV2SzabListak.Count; x++)
                        {
                            if (LV2SzabListak[x].Nev == listView2.Items[listView2.SelectedIndices[i]].SubItems[0].Text)
                            {
                                LV2SzabListak.RemoveAt(x);
                                break;
                            }
                        }
                    }

                    for (int i = 0; i < listView2.SelectedIndices.Count; i++)
                    {
                        try { Program.SzabListak.RemoveAt(Fajlkezelo.HanyadikEzAzElem(listView2.Items[listView2.SelectedIndices[i]].SubItems[0].Text, Program.SzabListak)); }
                        catch (IndexOutOfRangeException) { }
                        catch (ArgumentOutOfRangeException) { }
                    }

                    Fajlkezelo.SZabListIr(Program.SzabListak);

                    for (int i = listView2.SelectedIndices.Count - 2; i >= 0; i--)
                    {
                        listView2.Items.RemoveAt(listView2.SelectedIndices[i]);
                    }

                    label5.Text = listView2.SelectedIndices.Count + "/" + listView2.Items.Count + ((Program.KONFNyelv == "hun") ? "db" : Eszk.GetNyelvSzo("db"));
                    Frissits();
                    button6_Click(0, new EventArgs());
                }
            }
        }
        void Frissits()
        {
            listView2.Items.Clear();
            listView1.Items.Clear();


            foreach (Program.SzabLista item in LV1SzabListak)
            {
                System.Windows.Forms.ListViewItem listViewItemx = new System.Windows.Forms.ListViewItem(new string[] {
                    item.Nev,
                     item.Homero,
                        Fajlkezelo.CsatbolString(item.Csatornak)}, -1);

                listView1.Items.Add(listViewItemx);
            }

            foreach (Program.SzabLista item in LV2SzabListak)
            {
                System.Windows.Forms.ListViewItem listViewItemx = new System.Windows.Forms.ListViewItem(new string[] {
                    item.Nev,
                     item.Homero,
                        Fajlkezelo.CsatbolString(item.Csatornak)}, -1);

                listView2.Items.Add(listViewItemx);
            }


            label4.Text = listView1.SelectedIndices.Count + "/" + listView1.Items.Count + ((Program.KONFNyelv == "hun") ? "db" : Eszk.GetNyelvSzo("db"));
            label5.Text = listView2.SelectedIndices.Count + "/" + listView2.Items.Count + ((Program.KONFNyelv == "hun") ? "db" : Eszk.GetNyelvSzo("db"));

            Program.Attekint.listViewSzablistak.Dispatcher.Invoke(Program.FoAblak.AttekintSzablistFrissit);
        }

        private void szerkesztésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.Focused && listView1.SelectedIndices.Count != 0)
            {
                if (listView1.SelectedIndices.Count > 1)
                {
                    MessageBox.Show("Egyszerre legfeljebb egy sémát jelölhet ki szerkesztésre.", "TÚL SOK KIJELÖLT ELEM", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    Program.Dolgozott = false;
                    string szeresztendonev = listView1.Items[listView1.SelectedIndices[0]].SubItems[0].Text;
                    new SemaSzerkeszto(szeresztendonev).ShowDialog();
                    System.Threading.Thread.Sleep(10);
                    //try { Program.Th2.Abort(); } catch { }

                    if (Program.SZLIST_SZERK_MENT != null)
                    {

                        int SzerkesztettListaIndex = Fajlkezelo.HanyadikEzAzElemSZERKESZTES(szeresztendonev, Program.SzabListak);
                        Program.SzabListak.RemoveAt(SzerkesztettListaIndex);
                        Program.SzabListak.Insert(SzerkesztettListaIndex, Program.SZLIST_SZERK_MENT);

                        Fajlkezelo.SZabListIr(Program.SzabListak);

                        LV1SzabListak = Program.SzabListak;
                        System.Threading.Thread.Sleep(10);


                        for (int i = 0; i < LV1SzabListak.Count; i++)
                        {
                            for (int x = 0; x < LV2SzabListak.Count; x++)
                                if (LV1SzabListak[i].Nev == LV2SzabListak[x].Nev)
                                {
                                    LV1SzabListak.RemoveAt(i);
                                    --i;
                                    break;
                                }
                        }
                        System.Threading.Thread.Sleep(10);
                        Frissits();
                    }
                }
            }
            else if (listView2.SelectedIndices.Count != 0)
            {
                if (listView2.SelectedIndices.Count > 1)
                {
                    MessageBox.Show(((Program.KONFNyelv == "hun") ? "Egyszerre legfeljebb egy sémát jelölhet ki szerkesztésre." : Eszk.GetNyelvSzo("LKIVSzerkTulSokListaMboxSZOVEG")), ((Program.KONFNyelv == "hun") ? "TÚL SOK KIJELÖLT ELEM" : Eszk.GetNyelvSzo("LKIVSzerkTulSokListaMboxCIM")), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    Program.Dolgozott = false;
                    string szeresztendonev = listView2.Items[listView2.SelectedIndices[0]].SubItems[0].Text;
                    new SemaSzerkeszto(szeresztendonev).ShowDialog();
                    System.Threading.Thread.Sleep(10);
                    //try { Program.Th2.Abort(); } catch { }

                    //LV2SzabListak = Program.SzabListak;

                    //System.Threading.Thread.Sleep(10);

                    //if (Program.Dolgozott)
                    //{
                    //    for (int i = 0; i < LV2SzabListak.Count; i++)
                    //    {
                    //        for (int x = 0; x < LV1SzabListak.Count; x++)
                    //            if (LV2SzabListak[i].Nev == LV1SzabListak[x].Nev)
                    //            {
                    //                LV2SzabListak.RemoveAt(i);
                    //                --i;
                    //                break;
                    //            }
                    //    }
                    //    System.Threading.Thread.Sleep(10);
                    //    Frissits();
                    if (Program.SZLIST_SZERK_MENT != null)
                    {

                        Program.Ervenyesek.Clear();
                        for (int i = 0; i < listView2.Items.Count; i++)
                        {
                            try { Program.Ervenyesek.Add(Program.SzabListak[Fajlkezelo.HanyadikEzAzElem(listView2.Items[i].SubItems[0].Text, Program.SzabListak)]); }
                            catch (IndexOutOfRangeException) { }
                            catch (ArgumentOutOfRangeException) { }
                        }

                        int SzerkesztettListaIndex = Fajlkezelo.HanyadikEzAzElemSZERKESZTES(szeresztendonev, Program.SzabListak);
                        Program.SzabListak.RemoveAt(SzerkesztettListaIndex);
                        Program.SzabListak.Insert(SzerkesztettListaIndex, Program.SZLIST_SZERK_MENT);

                        SzerkesztettListaIndex = Fajlkezelo.HanyadikEzAzElemSZERKESZTES(szeresztendonev, Program.Ervenyesek);
                        Program.Ervenyesek.RemoveAt(SzerkesztettListaIndex);
                        Program.Ervenyesek.Insert(SzerkesztettListaIndex, Program.SZLIST_SZERK_MENT);

                        Fajlkezelo.SZabListIr(Program.SzabListak);

                        LV1SzabListak = Program.SzabListak;
                        LV2SzabListak = Program.Ervenyesek;

                        System.Threading.Thread.Sleep(10);

                        for (int i = 0; i < LV1SzabListak.Count; i++)
                        {
                            for (int x = 0; x < LV2SzabListak.Count; x++)
                                if (LV1SzabListak[i].Nev == LV2SzabListak[x].Nev)
                                {
                                    LV1SzabListak.RemoveAt(i);
                                    --i;
                                    break;
                                }
                        }

                        System.Threading.Thread.Sleep(10);
                        Frissits();
                        System.Threading.Thread.Sleep(10);
                        button6_Click(0, new EventArgs());
                    }

                }

            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            //for (int i = 0; i < listView1.SelectedIndices.Count; i++)
            //{
            //    LV2SzabListak.Add(LV1SzabListak[listView1.SelectedIndices[i]]);

            //}

            //for (int i = listView1.SelectedIndices.Count - 1; i >= 0; i--)
            //{
            //    LV1SzabListak.RemoveAt(listView1.SelectedIndices[i]);
            //}
            szerkesztésToolStripMenuItem_Click(12, new EventArgs());
            Frissits();
        }

        private void listView2_DoubleClick(object sender, EventArgs e)
        {
            //for (int i = 0; i < listView2.SelectedIndices.Count; i++)
            //{
            //    LV1SzabListak.Add(LV2SzabListak[listView2.SelectedIndices[i]]);

            //}

            //for (int i = listView2.SelectedIndices.Count - 1; i >= 0; i--)
            //{
            //    LV2SzabListak.RemoveAt(listView2.SelectedIndices[i]);
            //}
            szerkesztésToolStripMenuItem_Click(12, new EventArgs());
            Frissits();
        }

        private void klónozásToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.SZLIST_SZERK_MENT = null;

            if (listView1.Focused && listView1.SelectedIndices.Count != 0)
            {
                for (int i = 0; i < listView1.SelectedIndices.Count; i++)
                {
                    Program.Dolgozott = false;
                    new Klonozo(LV1SzabListak, Fajlkezelo.HanyadikEzAzElem(listView1.Items[listView1.SelectedIndices[i]].SubItems[0].Text, LV1SzabListak)).ShowDialog();
                    if (Program.SZLIST_SZERK_MENT != null)
                    {
                        Program.SzabListak.Add(Program.SZLIST_SZERK_MENT);
                        LV1SzabListak.Add(Program.SZLIST_SZERK_MENT);

                        Fajlkezelo.SZabListIr(Program.SzabListak);
                        System.Threading.Thread.Sleep(1);
                    }
                }
            }
            else if (listView2.SelectedIndices.Count != 0)
            {
                for (int i = 0; i < listView2.SelectedIndices.Count; i++)
                {
                    Program.Dolgozott = false;
                    new Klonozo(LV2SzabListak, Fajlkezelo.HanyadikEzAzElem(listView2.Items[listView2.SelectedIndices[i]].SubItems[0].Text, LV2SzabListak)).ShowDialog();
                    if (Program.SZLIST_SZERK_MENT != null)
                    {
                        Program.SzabListak.Add(Program.SZLIST_SZERK_MENT);

                        Fajlkezelo.SZabListIr(Program.SzabListak);
                        System.Threading.Thread.Sleep(1);

                        LV2SzabListak.Add(Program.SZLIST_SZERK_MENT);
                        button6_Click(0, new EventArgs());
                    }
                }
            }
            Frissits();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (labelH6.Visible == false)
            {
                labelH6.Visible = true;
                labelH7.Visible = true;
                labelH8.Visible = true;
                labelH9.Visible = true;
                labelH10.Visible = true;
                labelH11.Visible = true;
                labelH12.Visible = true;
                labelH13.Visible = true;
                labelH14.Visible = true;
                labelH3.Visible = true;
                labelH15.Visible = true;
                labelH16.Visible = true;
                labelH17.Visible = true;
                labelH18.Visible = true;

                if (Tutorial.Statusz == Tutorial.TStat.Szablistak_Szerkeszt)
                    MessageBox.Show(((Program.KONFNyelv == "hun") ? "A program az érvényben lévő szabályzósémák alapján állítja be a fordulatszámokat.\nSémákat úgy helyezhet érvénybe, hogy a jobb oldali felsorolásba helyezi őket és az ÉRVÉNYESÍTÉS gombra kattint.\nAmennyiben nem akarja megtartani változtatásait, úgy zárja be az ablakot és erősítse meg a mentés nélküli kilépést!\n\nKattintson a \"?\" gombra (Középen Fent)." : Eszk.GetNyelvSzo("LKIVTutorialMboxSZOVEG")), ((Program.KONFNyelv == "hun") ? "Tutorial" : Eszk.GetNyelvSzo("LKIVTutorialMboxCIM")));
            }
            else
            {
                labelH6.Visible = false;
                labelH7.Visible = false;
                labelH8.Visible = false;
                labelH9.Visible = false;
                labelH10.Visible = false;
                labelH11.Visible = false;
                labelH12.Visible = false;
                labelH13.Visible = false;
                labelH14.Visible = false;
                labelH3.Visible = false;
                labelH15.Visible = false;
                labelH16.Visible = false;
                labelH17.Visible = false;
                labelH18.Visible = false;
            }
        }

        void SegitsegBeallito()
        {
            // 
            // labelH6
            // 
            this.labelH6.AutoSize = true;
            this.labelH6.BackColor = System.Drawing.SystemColors.Highlight;
            this.labelH6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelH6.ForeColor = System.Drawing.Color.White;
            this.labelH6.Location = new System.Drawing.Point(176, 141);
            this.labelH6.Name = "labelH6";
            this.labelH6.Size = new System.Drawing.Size(226, 39);
            this.labelH6.TabIndex = 14;
            this.labelH6.Text = "Itt található az összes inaktív séma,\r\namikből kiválaszthatja, hogy melyeket\r\nhelyezi érvénybe.";
            this.labelH6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelH6.Visible = false;
            // 
            // labelH11
            // 
            this.labelH11.AutoSize = true;
            this.labelH11.BackColor = System.Drawing.SystemColors.Highlight;
            this.labelH11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelH11.ForeColor = System.Drawing.Color.White;
            this.labelH11.Location = new System.Drawing.Point(593, 249);
            this.labelH11.Name = "labelH11";
            this.labelH11.Size = new System.Drawing.Size(101, 13);
            this.labelH11.TabIndex = 15;
            this.labelH11.Text = "ÉRVÉNYESÍTÉS";
            this.labelH11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelH11.Visible = false;
            // 
            // labelH7
            // 
            this.labelH7.AutoSize = true;
            this.labelH7.BackColor = System.Drawing.SystemColors.Highlight;
            this.labelH7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelH7.ForeColor = System.Drawing.Color.White;
            this.labelH7.Location = new System.Drawing.Point(583, 162);
            this.labelH7.Name = "labelH7";
            this.labelH7.Size = new System.Drawing.Size(121, 13);
            this.labelH7.TabIndex = 16;
            this.labelH7.Text = "Minden elem jobbra.";
            this.labelH7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelH7.Visible = false;
            // 
            // labelH8
            // 
            this.labelH8.AutoSize = true;
            this.labelH8.BackColor = System.Drawing.SystemColors.Highlight;
            this.labelH8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelH8.ForeColor = System.Drawing.Color.White;
            this.labelH8.Location = new System.Drawing.Point(580, 113);
            this.labelH8.Name = "labelH8";
            this.labelH8.Size = new System.Drawing.Size(132, 13);
            this.labelH8.TabIndex = 17;
            this.labelH8.Text = "Kijelölt elemek jobbra.";
            this.labelH8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelH8.Visible = false;
            // 
            // labelH9
            // 
            this.labelH9.AutoSize = true;
            this.labelH9.BackColor = System.Drawing.SystemColors.Highlight;
            this.labelH9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelH9.ForeColor = System.Drawing.Color.White;
            this.labelH9.Location = new System.Drawing.Point(579, 311);
            this.labelH9.Name = "labelH9";
            this.labelH9.Size = new System.Drawing.Size(125, 13);
            this.labelH9.TabIndex = 18;
            this.labelH9.Text = "Kijelölt elemek balra.";
            this.labelH9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelH9.Visible = false;
            // 
            // labelH10
            // 
            this.labelH10.AutoSize = true;
            this.labelH10.BackColor = System.Drawing.SystemColors.Highlight;
            this.labelH10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelH10.ForeColor = System.Drawing.Color.White;
            this.labelH10.Location = new System.Drawing.Point(884, 133);
            this.labelH10.Name = "labelH10";
            this.labelH10.Size = new System.Drawing.Size(277, 26);
            this.labelH10.TabIndex = 19;
            this.labelH10.Text = "Az ÉRVÉNYESÍTÉS gomb megnyomásával ezek\r\na szabályzósémák kerülnek érvénybe.";
            this.labelH10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelH10.Visible = false;
            // 
            // labelH12
            // 
            this.labelH12.AutoSize = true;
            this.labelH12.BackColor = System.Drawing.SystemColors.Highlight;
            this.labelH12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelH12.ForeColor = System.Drawing.Color.White;
            this.labelH12.Location = new System.Drawing.Point(585, 362);
            this.labelH12.Name = "labelH12";
            this.labelH12.Size = new System.Drawing.Size(114, 13);
            this.labelH12.TabIndex = 20;
            this.labelH12.Text = "Minden elem balra.";
            this.labelH12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelH12.Visible = false;
            // 
            // labelH13
            // 
            this.labelH13.AutoSize = true;
            this.labelH13.BackColor = System.Drawing.SystemColors.Highlight;
            this.labelH13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelH13.ForeColor = System.Drawing.Color.White;
            this.labelH13.Location = new System.Drawing.Point(487, 41);
            this.labelH13.Name = "labelH13";
            this.labelH13.Size = new System.Drawing.Size(323, 13);
            this.labelH13.TabIndex = 21;
            this.labelH13.Text = "Kattintson ismét a ? gombra a segítség  eltüntetéséhez.";
            this.labelH13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelH13.Visible = false;
            // 
            // labelH14
            // 
            this.labelH14.AutoSize = true;
            this.labelH14.BackColor = System.Drawing.SystemColors.Highlight;
            this.labelH14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelH14.ForeColor = System.Drawing.Color.White;
            this.labelH14.Location = new System.Drawing.Point(170, 262);
            this.labelH14.Name = "labelH14";
            this.labelH14.Size = new System.Drawing.Size(239, 26);
            this.labelH14.TabIndex = 22;
            this.labelH14.Text = "Jelöljön ki sémákat, vagy kattintson egyre\r\ntovábbi műveletekhez.";
            this.labelH14.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelH14.Visible = false;
            // 
            // labelH16
            // 
            this.labelH16.AutoSize = true;
            this.labelH16.BackColor = System.Drawing.SystemColors.Highlight;
            this.labelH16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelH16.ForeColor = System.Drawing.Color.White;
            this.labelH16.Location = new System.Drawing.Point(529, 1);
            this.labelH16.Name = "labelH16";
            this.labelH16.Size = new System.Drawing.Size(244, 13);
            this.labelH16.TabIndex = 23;
            this.labelH16.Text = "Kijelölt/ a csoportban lévő elemek száma.";
            this.labelH16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelH16.Visible = false;
            // 
            // labelH15
            // 
            this.labelH15.AutoSize = true;
            this.labelH15.BackColor = System.Drawing.SystemColors.Highlight;
            this.labelH15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelH15.ForeColor = System.Drawing.Color.White;
            this.labelH15.Location = new System.Drawing.Point(184, 347);
            this.labelH15.Name = "labelH15";
            this.labelH15.Size = new System.Drawing.Size(237, 26);
            this.labelH15.TabIndex = 24;
            this.labelH15.Text = "Kattintson duplán egy elemre,\r\nhogy megtekinthesse és szerkeszthesse!";
            this.labelH15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelH15.Visible = false;
            // 
            // labelH17
            // 
            this.labelH17.AutoSize = true;
            this.labelH17.BackColor = System.Drawing.SystemColors.Highlight;
            this.labelH17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelH17.ForeColor = System.Drawing.Color.White;
            this.labelH17.Location = new System.Drawing.Point(894, 347);
            this.labelH17.Name = "labelH17";
            this.labelH17.Size = new System.Drawing.Size(237, 26);
            this.labelH17.TabIndex = 25;
            this.labelH17.Text = "Kattintson duplán egy elemre,\r\nhogy megtekinthesse és szerkeszthesse!";
            this.labelH17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelH17.Visible = false;
            // 
            // labelH3
            // 
            this.labelH3.AutoSize = true;
            this.labelH3.BackColor = System.Drawing.SystemColors.Highlight;
            this.labelH3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelH3.ForeColor = System.Drawing.Color.White;
            this.labelH3.Location = new System.Drawing.Point(884, 262);
            this.labelH3.Name = "labelH3";
            this.labelH3.Size = new System.Drawing.Size(239, 26);
            this.labelH3.TabIndex = 22;
            this.labelH3.Text = "Jelöljön ki sémákat, vagy kattintson egyre\r\ntovábbi műveletekhez.";
            this.labelH3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelH3.Visible = false;
            // 
            // labelH18
            // 
            this.labelH18.AutoSize = true;
            this.labelH18.BackColor = System.Drawing.SystemColors.Highlight;
            this.labelH18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelH18.ForeColor = System.Drawing.Color.White;
            this.labelH18.Location = new System.Drawing.Point(584, 72);
            this.labelH18.Name = "labelH18";
            this.labelH18.Size = new System.Drawing.Size(119, 13);
            this.labelH18.TabIndex = 25;
            this.labelH18.Text = "Új séma létrehozása!";
            this.labelH18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelH18.Visible = false;

        }

        void button6_Click(object sender, EventArgs e)
        {
            bool elottenemvoltervenyeslista = false;
            if (Program.Ervenyesek.Count == 0)
            {
                elottenemvoltervenyeslista = true;
            }

            if (!Eszkozok.Eszk.IsPremiumFuncEabled() && LV2SzabListak.Count > 2)
            {
                if (MessageBox.Show(((Program.KONFNyelv == "hun") ? "A program ingyenes verziójában legfeljebb 2db aktív szabályzóséma használható.\nSzeretne korlátlan számú listát aktiválni?" : Eszk.GetNyelvSzo("MBoxSzovegFreeAktivszablist")), "Freemium", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    Eszkozok.Eszk.GetFullVersion();
                }
            }
            else
            {
                GetTeljesverz.FreemiumClickTest();

                Enabled = false;
                Program.SzabListak = Fajlkezelo.SZabListBeolvas();
                Program.Attekint.listViewSzablistak.Dispatcher.Invoke(Program.FoAblak.AttekintSzablistFrissit);
                for (int i = 0; i < 1500 / 5 && Program.ErvenyVanOlvasas; ++i)
                    System.Threading.Thread.Sleep(5);
                Program.ErvenyVanIras = true;

                for (int i = 0; i < 2; i++)
                {
                    if (Ervenyesito())
                    {
                        Program.HisztDeaktiv = true;
                        MessageBox.Show(((Program.KONFNyelv == "hun") ? "Az érvényesítés sikeres!" : Eszk.GetNyelvSzo("LKIVErvenyesitesSikeresSZOVEG")), ((Program.KONFNyelv == "hun") ? "Sikeres Érvényesítés" : Eszk.GetNyelvSzo("LKIVErvenyesitesSikeresCIM")), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        if(elottenemvoltervenyeslista && Program.KONFTutorialMegjelenit)
                        {
                            Program.TutorialWPFAblak.MutassMessagebox("Now the activated schemes will be taken into account by the Control Logic.\n\nYou can create unlimited number of schemes. You also can Edit, Clone or Delete them.\nPlay with them or close scheme manager!", "Scheme(s) activated! Good job!");
                            Program.TutorialRiasztasBemutatasjon = true;
                        }
                        NincsErvenyesitve = false;
                        Enabled = true;
                        Program.HisztDeaktiv = true;
                        break;
                    }
                }

                if (Enabled == false)
                {
                    MessageBox.Show(((Program.KONFNyelv == "hun") ? "Az érvényesítés sikertelen!\nPróbálja meg újra!" : Eszk.GetNyelvSzo("LKIVErvenyesitesSikertelenSZOVEG")), ((Program.KONFNyelv == "hun") ? "Sikertelen Érvényesítés" : Eszk.GetNyelvSzo("LKIVErvenyesitesSikertelenCIM")), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    Enabled = true;
                }
                Program.ErvenyVanIras = false;
            }
        }

        bool Ervenyesito()
        {
            try
            {
                Program.Ervenyesek = Fajlkezelo.DeepCopySzablista(LV2SzabListak);
                Program.Attekint.listViewSzablistak.Dispatcher.Invoke(Program.FoAblak.AttekintSzablistFrissit);
                //Program.Ervenyesek.Clear();
                //for (int i = 0; i < listView2.Items.Count; i++)
                //{
                //    try { Program.Ervenyesek.Add(Program.SzabListak[Fajlkezelo.HanyadikEzAzElem(listView2.Items[i].SubItems[0].Text, Program.SzabListak)]); }
                //    catch (IndexOutOfRangeException) { }
                //    catch (ArgumentOutOfRangeException) { }
                //}

                System.Threading.Thread.Sleep(10);
                Fajlkezelo.ErvenyListIr(Program.Ervenyesek);
            }
            catch
            {
                try
                {
                    Program.Ervenyesek.Clear();

                    for (int i = 0; i < listView2.Items.Count; i++)
                    {
                        try { Program.Ervenyesek.Add(Program.SzabListak[Fajlkezelo.HanyadikEzAzElem(listView2.Items[i].SubItems[0].Text, Program.SzabListak)]); }
                        catch (IndexOutOfRangeException) { }
                        catch (ArgumentOutOfRangeException) { }
                    }

                    System.Threading.Thread.Sleep(10);
                    Fajlkezelo.ErvenyListIr(Program.Ervenyesek);
                }
                catch (Exception e)
                {
                    MessageBox.Show(((Program.KONFNyelv == "hun") ? "Az érvényesítés sikertelen!\nPróbálja meg újra!" : Eszk.GetNyelvSzo("LKIVErvenyesitesSikertelenSZOVEG")) + "\n" + e.ToString(), ((Program.KONFNyelv == "hun") ? "Sikertelen Érvényesítés" : Eszk.GetNyelvSzo("LKIVErvenyesitesSikertelenCIM")), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return false;
                }
            }
            return true;
        }

        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
                törlésToolStripMenuItem_Click(0, e);
        }
        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            toolStripMenuItem2.Text = ">>>";
            if (listView1.SelectedIndices.Count != 0)
            {
                if (e != null && e.Button != System.Windows.Forms.MouseButtons.Right)
                    contextMenuStrip1.Show(new Point(this.Location.X + listView1.Location.X + e.Location.X + 10, this.Location.Y + listView1.Location.Y + e.Location.Y));
            }
        }

        private void listView2_MouseUp(object sender, MouseEventArgs e)
        {
            toolStripMenuItem2.Text = "<<<";
            if (listView2.SelectedIndices.Count != 0)
            {
                if (e != null && e.Button != System.Windows.Forms.MouseButtons.Right)
                    contextMenuStrip1.Show(new Point(this.Location.X + listView2.Location.X + e.Location.X + 10, this.Location.Y + listView2.Location.Y + e.Location.Y));
            }
        }

        private void listView1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Tutorial.Statusz == Tutorial.TStat.Szablistak_Szerkeszt)
                MessageBox.Show(((Program.KONFNyelv == "hun") ? "Kattintson a \"?\" gombra a kezelőfelület megismeréséhez. (Középen Fent)" : Eszk.GetNyelvSzo("LKIVTutorial2MboxSZOVEG")), ((Program.KONFNyelv == "hun") ? "Tutorial" : Eszk.GetNyelvSzo("LKIVTutorial2MboxCIM")));

            listView1.MouseMove -= listView1_MouseMove;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Program.FoAblak.SZListazo = new SzenzorListazo(true, false);
            Program.FoAblak.SZListazo.ShowDialog();
            SzenzorListazo.SajatHelpekMegejenites(false);

            LV1SzabListak = Fajlkezelo.SZabListBeolvas();

            for (int i = 0; i < LV1SzabListak.Count; i++)
            {
                for (int x = 0; x < LV2SzabListak.Count; x++)
                    if (LV1SzabListak[i].Nev == LV2SzabListak[x].Nev)
                    {
                        LV1SzabListak.RemoveAt(i);
                        --i;
                        break;
                    }
            }

            Frissits();

            if (Program.Ervenyesek.Count == 0 && Program.SzabListak.Count != 0 && Program.KONFTutorialMegjelenit)
            {
                Program.TutorialWPFAblak.MutassMessagebox("To make the created control schemes working, you have to Activate them.\n\nSelect some schemes in the left side and move them to the right side. After that, click the Activation button. (The big red one in the center.)", "Activate control listst!");
            }
            else if (Program.SzabListak.Count == 0 && Program.KONFTutorialMegjelenit)
            {
                Program.TutorialWPFAblak.MutassMessagebox("Here you can manage your control schemes.\n\nFollow the \"Create new Control Scheme\" label now!", "Scheme management!");
            }

        }

        private void ListaKivalaszto_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (NincsErvenyesitve == true)
            {
                if (MessageBox.Show(((Program.KONFNyelv == "hun") ? "Nem minden módosítását érvényesítette.\nHa most bezárja ez az ablakot, ezek a módosítások elvesznek.\n(Szabályozósémák mozgatása a két felsorolás között.)\n\nBiztosan kilép?" : Eszk.GetNyelvSzo("LKIVBiztosanKilepMboxSZOVEG")), ((Program.KONFNyelv == "hun") ? "Nem mentett módosítások!" : Eszk.GetNyelvSzo("LKIVBiztosanKilepMboxCIM")), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    e.Cancel = true;
                    if (MessageBox.Show(((Program.KONFNyelv == "hun") ? "Érvényesít most?" : Eszk.GetNyelvSzo("LKIVErvenyesitMostMboxSZOVEG")), ((Program.KONFNyelv == "hun") ? "Azonnali érvényesítés" : Eszk.GetNyelvSzo("LKIVErvenyesitMostMboxCIM")), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        button6_Click(12, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
                        e.Cancel = false;
                    }
                }
                else
                {
                    NincsErvenyesitve = false;
                }
            }

            if (e.Cancel == false)
            {
                Attekinto.AttekintoWPF.SajatHelpekMegejenites(true);
                SajatHelpekMegejenites(false);
            }

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (listView1.Focused)
                button1_Click(12, e);
            else if (listView2.Focused)
                button3_Click(12, e);
        }

        private void Lokalizalj()
        {
            label1.Text = Eszk.GetNyelvSzo("InaktivListak");
            label2.Text = Eszk.GetNyelvSzo("AktivListak");
            label4.Text = "0/0" + Eszk.GetNyelvSzo("db");
            label5.Text = "0/0" + Eszk.GetNyelvSzo("db");

            labelH3.Text = Eszk.GetNyelvSzo("LKIVH3");
            labelH6.Text = Eszk.GetNyelvSzo("LKIVH6");
            labelH7.Text = Eszk.GetNyelvSzo("LKIVH7");
            labelH8.Text = Eszk.GetNyelvSzo("LKIVH8");
            labelH9.Text = Eszk.GetNyelvSzo("LKIVH9");
            labelH10.Text = Eszk.GetNyelvSzo("LKIVH10");
            labelH11.Text = Eszk.GetNyelvSzo("LKIVH11");
            labelH12.Text = Eszk.GetNyelvSzo("LKIVH12");
            labelH13.Text = Eszk.GetNyelvSzo("LKIVH13");
            labelH14.Text = Eszk.GetNyelvSzo("LKIVH14");
            labelH15.Text = Eszk.GetNyelvSzo("LKIVH15");
            labelH16.Text = Eszk.GetNyelvSzo("LKIVH16");
            labelH17.Text = Eszk.GetNyelvSzo("LKIVH17");
            labelH18.Text = Eszk.GetNyelvSzo("LKIVH18");

            this.Text = Eszk.GetNyelvSzo("SzabalyzolistakKezelese");

            this.Nev.Text = this.Nev2.Text = Eszk.GetNyelvSzo("Név");
            this.Homero.Text = this.Homero2.Text = Eszk.GetNyelvSzo("Hoszenzor");
            this.Csatornak.Text = this.Csatornak2.Text = Eszk.GetNyelvSzo("Csatornák");

            szerkesztésToolStripMenuItem.Text = Eszk.GetNyelvSzo("Szerkesztés");
            klónozásToolStripMenuItem.Text = Eszk.GetNyelvSzo("Klónozás");
            törlésToolStripMenuItem.Text = Eszk.GetNyelvSzo("Törlés");

        }

    }
}
