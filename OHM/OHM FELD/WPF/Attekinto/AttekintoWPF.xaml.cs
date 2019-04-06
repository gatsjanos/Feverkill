using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenHardwareMonitor.GUI;
using OpenHardwareMonitor;
using OpenHardwareMonitor.Hardware;
using System.ComponentModel;
using UdvozloKepernyo;
using OpenHardwareMonitor.Eszkozok;

namespace Attekinto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public class GVItemHomers
    {
        public string Érték { get; set; }
        public string Név { get; set; }
        public string Csoport { get; set; }
    }
    public class GVItemFordsz
    {
        public string Csoport { get; set; }
        public string Kimenet { get; set; }
        public string Fordszam { get; set; }
        public string VezTipus { get; set; }
        public IControl Kontrol { get; set; }
        public Program.EmulaltBelsoVenti KontrolEMULALT { get; set; }
        public int CsatIndex;

    }
    public class GVItemSzabl
    {
        public string Csoport { get; set; }
        public string Nev { get; set; }
        public string Hoszenzor { get; set; }
        public string Csatornak { get; set; }
    }
    public class GVItemRiaszt
    {
        public string Csoport { get; set; }
        public string Homero { get; set; }
        public string Muvelet { get; set; }
        public string RiasztPont { get; set; }
        public string Uzenet { get; set; }
        public string Hangjelzes { get; set; }
        public string SpecMuv { get; set; }
        public string EbresztIdo { get; set; }
    }

    public partial class AttekintoWPF : Window
    {
        FoAblak MF;

        public AttekintoWPF(FoAblak MFbe)
        {
            //COLHatter = new Brush();
            MF = MFbe;
            InitializeComponent();
            Topmost = Program.KONFFelulMarado;

            /////////////////////////////////
            Lokalizalas Lok = new Lokalizalas();
            this.DataContext = Lok;
            Lok.Lokalizalj();
            if (Program.KONFNyelv != "hun")
            {
                imageHelp.ToolTip = Eszk.GetNyelvSzo("ATTEKHelpToolTip");
                imageFoabl.ToolTip = Eszk.GetNyelvSzo("ATTEKHalAblToolTip");
                imageHide.ToolTip = Eszk.GetNyelvSzo("ATTEKHideToolTip");
                imageFeedback.ToolTip = Eszk.GetNyelvSzo("ATTEKFeedbackToolTip");
            }
            //////////////////////////////////////
            button.Visibility = Visibility.Hidden;

            SajatHelpekHozzaadas();

            if (Program.KONFAttekintMutat)
                SajatHelpekMegejenites(true);

        }

        void SajatHelpekHozzaadas()
        {
            // SegTestKezelo.SegtestHozzaad("A1", labelCim, "I'm CEO, Bitch...", 70, "Segoe Print", Brushes.Red, false, null, true);

            SegTestKezelo.SegtestHozzaad("A2", labelHiszt, "Use it to avoid the oscillating of the fans", 28, "Comic Sans MS", Brushes.White, false, false, new Point(-366, -107), true);
            SegTestKezelo.SegtestHozzaad("A3", labelCelh, "Click here to at/detach the Target Hardware\n(to disable connection error messages)", 28, "Comic Sans MS", Brushes.White, false, false, new Point(170, 45), true);
            SegTestKezelo.SegtestHozzaad("A4", labelFrissId, "Change it to balance\nperformance and sensitivity", 28, "Comic Sans MS", Brushes.White, false, false, new Point(-249, 45), true);

            SegTestKezelo.SegtestHozzaad("A5", VillFordszRectHelper, "Click here to open\na manual control window", 28, "Comic Sans MS", Brushes.GreenYellow, false, false, new Point(-193, -183), true);
            SegTestKezelo.SegtestHozzaad("A6", VillSemaRectHelper, "Click here to manage Control Schemes", 28, "Comic Sans MS", Brushes.GreenYellow, true, false, new Point(-331, -169), true);
            SegTestKezelo.SegtestHozzaad("A7", VillHomRectHelper, "Click here to open\ntemperatures window", 28, "Comic Sans MS", Brushes.GreenYellow, false, false, new Point(-397, 60), true);
            SegTestKezelo.SegtestHozzaad("A8", VillRiasztRectHelper, "Click here to manage Alerts", 28, "Comic Sans MS", Brushes.GreenYellow, true, false, new Point(-380, 14), true);

            SegTestKezelo.SegtestHozzaad("A9", OldalTutorRectHelper, "Show/Hide hints", 28, "Segoe Print", Brushes.DeepSkyBlue, false, false, new Point(68, -67), true);
            SegTestKezelo.SegtestHozzaad("A10", OldalFoablRectHelper, "Show/Hide Advanced window", 28, "Comic Sans MS", Brushes.DeepSkyBlue, false, false, new Point(107, -71), true);
            SegTestKezelo.SegtestHozzaad("A11", OldalElrejtRectHelper, "Hide this window", 28, "Comic Sans MS", Brushes.DeepSkyBlue, false, false, new Point(78, -60), true);
        }
        public static void SajatHelpekMegejenites(bool lathato)
        {
            SegTestKezelo.SetMegjelenites("A1", lathato);
            SegTestKezelo.SetMegjelenites("A2", lathato);
            SegTestKezelo.SetMegjelenites("A3", lathato);
            SegTestKezelo.SetMegjelenites("A4", lathato);
            SegTestKezelo.SetMegjelenites("A5", lathato);
            SegTestKezelo.SetMegjelenites("A6", lathato);
            SegTestKezelo.SetMegjelenites("A7", lathato);
            SegTestKezelo.SetMegjelenites("A8", lathato);
            SegTestKezelo.SetMegjelenites("A9", lathato);
            SegTestKezelo.SetMegjelenites("A10", lathato);
            SegTestKezelo.SetMegjelenites("A11", lathato);
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //imageBackgnd.Height = gridBackgnd.Height;
            //imageBackgnd.Width = gridBackgnd.Width;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            MF.Bezar(false, "");
        }

        private void listViewHomers_MouseEnter(object sender, MouseEventArgs e)
        {
            //    ScrollViewer.SetVerticalScrollBarVisibility(listViewHomers, ScrollBarVisibility.Auto);
            //    ScrollViewer.SetHorizontalScrollBarVisibility(listViewHomers, ScrollBarVisibility.Auto);
        }

        private void listViewHomers_MouseLeave(object sender, MouseEventArgs e)
        {
            ScrollViewer.SetVerticalScrollBarVisibility(listViewHomers, ScrollBarVisibility.Hidden);
            ScrollViewer.SetHorizontalScrollBarVisibility(listViewHomers, ScrollBarVisibility.Hidden);
        }

        private void listViewHomers_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ScrollViewer.SetVerticalScrollBarVisibility(listViewHomers, ScrollBarVisibility.Auto);
            ScrollViewer.SetHorizontalScrollBarVisibility(listViewHomers, ScrollBarVisibility.Auto);
        }

        private void listViewHomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //listViewHomers.ScrollIntoView(listViewHomers.SelectedIndex);
        }

        private void listViewHomers_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            listViewHomers_MouseDown(sender, e);
        }

        private void frame_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void KilepesMenuitem_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(19981001);
        }

        private void frame1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        Point ElozoEgerhely = new Point(0, 0);
        int Fan1Szog = 0;
        int TurbineSzog = 0;
        private void imageBackgnd_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = PointToScreen(e.GetPosition(null));
            if (Math.Abs(ElozoEgerhely.Y - p.Y) > 2 || Math.Abs(ElozoEgerhely.X - p.X) > 2)
            {
                if (Math.Abs(ElozoEgerhely.Y - p.Y) > 10 || Math.Abs(ElozoEgerhely.X - p.X) > 10)
                    ElozoEgerhely = p;
                else
                {
                    Fan1Szog += (int)(ElozoEgerhely.X - p.X);
                    imageFan1.RenderTransform = new RotateTransform(Fan1Szog);

                    TurbineSzog += (int)(ElozoEgerhely.Y - p.Y);
                    imageTurbine.RenderTransform = new RotateTransform(TurbineSzog);

                    ElozoEgerhely = p;
                }
            }
        }

        private void imageBackgnd_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void imageHelp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/HelpL.png");
            try { logo.EndInit(); } catch { }
            try { imageHelp.Source = logo; } catch { }
        }

        private void imageHelp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/HelpE.png");
            try { logo.EndInit(); } catch { }
            try { imageHelp.Source = logo; } catch { }

            //MF.Show();
            //if (MF.TopMost != true)
            //{
            //    MF.TopMost = true;
            //    MF.TopMost = false;
            //}
            //System.Windows.Forms.MessageBox.Show("\n   A tutorial elindításához a Főablakban kattintson a \"Segítség>>Tutorial Mód Aktív\" menüpontra!\nA tutorialban bemutatott leírások fontosabbjai\nmegatlálhatók a \"Segítség>>Használati Utasítás\" menüpont alatt.\nJelenleg az Áttekintő ablakban van. A menüket is tartalmazó Főablakot autómatikusan megnyitottuk, egyébként megnyitásához kattintson a jobbra fent található Ház ikonra!\n\n\n\tEgyéb kérdés esetén lépjen kapcsolatba a fejlesztő\ncsapattal a \"Segítség>>Visszajelzés Küldése\" menüponttal\n(adjon meg elérhetőséget az E-mail cím mezőben).", "Súgó", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            //if (System.Windows.Forms.MessageBox.Show("El akarja indítani a tutorialt?", "Tutorial", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                MF.menuItem86.PerformClick();
            }
        }

        private void imageHelp_MouseLeave(object sender, MouseEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/HelpA.png");
            try { logo.EndInit(); } catch { }
            try { imageHelp.Source = logo; } catch { }
        }

        private void MainWindow1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            MF.menuItem92.PerformClick();
        }

        private void imageBackgnd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try { this.DragMove(); } catch { }
        }

        private void imageHelp_MouseEnter(object sender, MouseEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/HelpE.png");
            try { logo.EndInit(); } catch { }
            try { imageHelp.Source = logo; } catch { }
        }

        private void imageHide_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/HideL.png");
            try { logo.EndInit(); } catch { }
            try { imageHide.Source = logo; } catch { }
        }

        private void imageHide_MouseEnter(object sender, MouseEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/HideE.png");
            try { logo.EndInit(); } catch { }
            try { imageHide.Source = logo; } catch { }
        }

        private void imageHide_MouseLeave(object sender, MouseEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/HideA.png");
            try { logo.EndInit(); } catch { }
            try { imageHide.Source = logo; } catch { }
        }

        private void imageHide_MouseUp(object sender, MouseButtonEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/HideE.png");
            try { logo.EndInit(); } catch { }
            try { imageHide.Source = logo; } catch { }

            MF.menuItem92.PerformClick();
        }

        private void imageFoabl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/FoablL.png");
            try { logo.EndInit(); } catch { }
            try { imageFoabl.Source = logo; } catch { }
        }

        private void imageFoabl_MouseEnter(object sender, MouseEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/FoablE.png");
            try { logo.EndInit(); } catch { }
            try { imageFoabl.Source = logo; } catch { }
        }

        private void imageFoabl_MouseLeave(object sender, MouseEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/FoablA.png");
            try { logo.EndInit(); } catch { }
            try { imageFoabl.Source = logo; } catch { }
        }

        private void imageFoabl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/FoablE.png");
            try { logo.EndInit(); } catch { }
            try { imageFoabl.Source = logo; } catch { }

            GetTeljesverz.FreemiumClickTest();

            if (MF.WindowState == System.Windows.Forms.FormWindowState.Minimized)
                MF.WindowState = System.Windows.Forms.FormWindowState.Normal;
            MF.Show();
            if (MF.TopMost != true)
            {
                MF.TopMost = true;
                MF.TopMost = false;
            }
            if (Program.TutorialSzenzgrafBemutatasjon && Program.KONFTutorialMegjelenit)
            {
                Program.TutorialWPFAblak.MutassMessagebox("This is the Advanced Window!\nYou can find here infinite amount of very useful functions. Some of them are only relevant if you have got our USB Target Hardware. These ones aren't discussed in this tutorial.\n\nFirst of all open Sensor Graph by clicking \"View>>Show Sensor Graph\" menuitem!", "Advanced Window");
                if (MessageBox.Show("Let me help you a little bit!\nDo you want me to click that menuitem for you?", "Help for a talented Padawan", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Program.FoAblak.plotMenuItem.PerformClick();
                }
                Program.TutorialSzenzgrafBemutatasjon = false;
            }
        }

        private void imageTurbine_MouseMove(object sender, MouseEventArgs e)
        {
            imageBackgnd_MouseMove(12, e);
        }

        private void imageFan1_MouseMove(object sender, MouseEventArgs e)
        {
            imageBackgnd_MouseMove(12, e);
        }

        private void labelCim_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try { this.DragMove(); } catch { }
        }

        private void imageVillFordsz_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/VillanyL.png");
            try { logo.EndInit(); } catch { }
            try { imageVillFordsz.Source = logo; } catch { }
        }

        private void imageVillFordsz_MouseEnter(object sender, MouseEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/VillanyE.png");
            try
            {
                logo.EndInit();
                imageVillFordsz.Source = logo;
            }
            catch { }

            //////////////ScrollBarElrejtes
            ScrollViewer.SetVerticalScrollBarVisibility(listViewFordszamok, ScrollBarVisibility.Hidden);
            ScrollViewer.SetHorizontalScrollBarVisibility(listViewFordszamok, ScrollBarVisibility.Hidden);
        }

        private void imageVillFordsz_MouseLeave(object sender, MouseEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/VillanyA.png");
            try { logo.EndInit(); } catch { }
            try { imageVillFordsz.Source = logo; } catch { }
        }

        private void imageVillFordsz_MouseUp(object sender, MouseButtonEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/VillanyA.png");
            try { logo.EndInit(); } catch { }
            try { imageVillFordsz.Source = logo; } catch { }

            GetTeljesverz.FreemiumClickTest();

            MF.menuItem11.PerformClick();
        }

        private void listViewSzablistak_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ScrollViewer.SetVerticalScrollBarVisibility(listViewSzablistak, ScrollBarVisibility.Auto);
            ScrollViewer.SetHorizontalScrollBarVisibility(listViewSzablistak, ScrollBarVisibility.Auto);
        }

        private void listViewSzablistak_MouseLeave(object sender, MouseEventArgs e)
        {
            ScrollViewer.SetVerticalScrollBarVisibility(listViewSzablistak, ScrollBarVisibility.Hidden);
            ScrollViewer.SetHorizontalScrollBarVisibility(listViewSzablistak, ScrollBarVisibility.Hidden);
        }

        private void imageVillSzabl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/VillanyL.png");
            try { logo.EndInit(); } catch { }
            try { imageVillSema.Source = logo; } catch { }
        }

        private void imageVillSzabl_MouseEnter(object sender, MouseEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/VillanyE.png");
            try { logo.EndInit(); } catch { }
            try { imageVillSema.Source = logo; } catch { }
        }

        private void imageVillSzabl_MouseLeave(object sender, MouseEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/VillanyA.png");
            try { logo.EndInit(); } catch { }
            try { imageVillSema.Source = logo; } catch { }
        }


        private void imageVillSzabl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/VillanyE.png");
            try { logo.EndInit(); } catch { }
            try { imageVillSema.Source = logo; } catch { }

            GetTeljesverz.FreemiumClickTest();
            MF.menuItem9.PerformClick();
            if (Program.Riasztasok.Count == 0 && Program.TutorialRiasztasBemutatasjon && Program.KONFTutorialMegjelenit)
            {
                Program.TutorialWPFAblak.MutassMessagebox("Now try out an other great function! Create some Alerts by clicking the lightbulb bottom right!", "Alerts");
                Program.TutorialRiasztasBemutatasjon = false;
            }
        }
        private void imageVillHom_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/VillanyL.png");
            try { logo.EndInit(); } catch { }
            try { imageVillHom.Source = logo; } catch { }
        }

        private void imageVillHom_MouseEnter(object sender, MouseEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/VillanyE.png");
            try { logo.EndInit(); } catch { }
            try { imageVillHom.Source = logo; } catch { }
        }

        private void imageVillHom_MouseLeave(object sender, MouseEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/VillanyA.png");
            try { logo.EndInit(); } catch { }
            try { imageVillHom.Source = logo; } catch { }
        }

        private void imageVillHom_MouseUp(object sender, MouseButtonEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/VillanyE.png");
            try { logo.EndInit(); } catch { }
            try { imageVillHom.Source = logo; } catch { }


            GetTeljesverz.FreemiumClickTest();
            MF.menuItem39.PerformClick();
        }

        private void listViewRiaszt_MouseLeave(object sender, MouseEventArgs e)
        {
            ScrollViewer.SetVerticalScrollBarVisibility(listViewRiaszt, ScrollBarVisibility.Hidden);
            ScrollViewer.SetHorizontalScrollBarVisibility(listViewRiaszt, ScrollBarVisibility.Hidden);
        }

        private void listViewRiaszt_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ScrollViewer.SetVerticalScrollBarVisibility(listViewRiaszt, ScrollBarVisibility.Auto);
            ScrollViewer.SetHorizontalScrollBarVisibility(listViewRiaszt, ScrollBarVisibility.Auto);
        }

        private void imageVillRiaszt_MouseUp(object sender, MouseButtonEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/VillanyE.png");
            try { logo.EndInit(); } catch { }
            try { imageVillRiaszt.Source = logo; } catch { }


            GetTeljesverz.FreemiumClickTest();
            MF.menuItem35.PerformClick();

            RiasztKezelo.SajatHelpekMegejenites(false);
            if (Program.TutorialSzenzgrafBemutatasjon && Program.KONFTutorialMegjelenit)
            {
                Program.TutorialWPFAblak.MutassMessagebox("Very good, feverkiller!\n\nDo you want to meet one more interesting function? Then open the Advanced Window!\n\n\n(To do it see the hints above right!)", "Sensor Graph");
                Program.TutorialRiasztasBemutatasjon = false;
            }
        }

        private void imageVillRiaszt_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/VillanyL.png");
            try { logo.EndInit(); } catch { }
            try { imageVillRiaszt.Source = logo; } catch { }
        }

        private void imageVillRiaszt_MouseEnter(object sender, MouseEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/VillanyE.png");
            try { logo.EndInit(); } catch { }
            try { imageVillRiaszt.Source = logo; } catch { }
        }

        private void imageVillRiaszt_MouseLeave(object sender, MouseEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/VillanyA.png");
            try { logo.EndInit(); } catch { }
            try { imageVillRiaszt.Source = logo; } catch { }
        }

        System.Windows.Forms.ContextMenuStrip Frissidmenu = new System.Windows.Forms.ContextMenuStrip();
        private void labelFrissId_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Frissidmenu.Items.Clear();

            System.Windows.Forms.ToolStripMenuItem beallItem = new System.Windows.Forms.ToolStripMenuItem((Program.KONFNyelv == "hun") ? "Beállítás" : (Eszk.GetNyelvSzo("Beallitas")));

            System.Windows.Forms.ToolStripMenuItem itemx = new System.Windows.Forms.ToolStripMenuItem((Program.KONFNyelv == "hun") ? "Mi ez?" : (Eszk.GetNyelvSzo("Mi ez?")));
            beallItem.DropDownItems.Add(itemx);
            itemx.Click += delegate (object obj, EventArgs args)
            {
                System.Windows.Forms.MessageBox.Show(((Program.KONFNyelv == "hun") ? "Ez a beállítás adja meg, hogy a program mekkora\nidőközönként kérje le a hőmérsékleti értékeket\nés állítsa a fordulatszámokat.\n\nA kisebb időköz nagyobb érzékenységet,\nugyanakkor nagyobb erőforrásigényt is jelent." : Eszk.GetNyelvSzo("ATTEKFrissIdokozMboxSZOVEG")), ((Program.KONFNyelv == "hun") ? "Frissítési időköz" : Eszk.GetNyelvSzo("ATTEKFrissIdokozMboxCIM")), System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            };

            for (int i = 1; i <= 20; ++i)
            {
                System.Windows.Forms.ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem(i + " sec");
                beallItem.DropDownItems.Add(item);

                item.Checked = Program.KONFFrisIdo == 1000 * i;

                int iErtek = i;
                switch (iErtek)
                {
                    case 4:
                        item.Text = "4 sec " + ((Program.KONFNyelv == "hun") ? "(ajánlott)" : ("(" + Eszk.GetNyelvSzo("ajanlott")) + ")");
                        item.Click += delegate (object obj, EventArgs args)
                    {
                        Program.KONFFrisIdo = iErtek * 1000;
                        labelFrissId.Content = ((Program.KONFNyelv == "hun") ? "Frissítési időköz: " : (Eszk.GetNyelvSzo("ATTEKUIFrissido") + ": ")) + iErtek.ToString() + " sec";
                        OpenHardwareMonitor.Eszkozok.Eszk.Elnevezo();
                    };
                        break;

                    case 8:
                        item.Text = "8 sec " + ((Program.KONFNyelv == "hun") ? "(nagyobb nem ajánlott)" : ("(" + Eszk.GetNyelvSzo("NagyobbNemAjanlott")) + ")");
                        item.Click += delegate (object obj, EventArgs args)
                        {
                            Program.KONFFrisIdo = iErtek * 1000;
                            labelFrissId.Content = ((Program.KONFNyelv == "hun") ? "Frissítési időköz: " : (Eszk.GetNyelvSzo("ATTEKUIFrissido") + ": ")) + iErtek.ToString() + " sec";
                            OpenHardwareMonitor.Eszkozok.Eszk.Elnevezo();
                        };
                        break;

                    default:
                        item.Click += delegate (object obj, EventArgs args)
                        {
                            Program.KONFFrisIdo = iErtek * 1000;
                            labelFrissId.Content = ((Program.KONFNyelv == "hun") ? "Frissítési időköz: " : (Eszk.GetNyelvSzo("ATTEKUIFrissido") + ": ")) + iErtek.ToString() + " sec";
                            OpenHardwareMonitor.Eszkozok.Eszk.Elnevezo();
                        };
                        break;
                }

                if (i == 1)
                {
                    System.Windows.Forms.ToolStripMenuItem itemy = new System.Windows.Forms.ToolStripMenuItem("1,5 sec");
                    beallItem.DropDownItems.Add(itemy);

                    itemy.Checked = Program.KONFFrisIdo == 1500;

                    int iErtek2 = i;
                    itemy.Click += delegate (object obj, EventArgs args)
                    {
                        Program.KONFFrisIdo = 1500;
                        labelFrissId.Content = "Frissítési időköz: 1,5 sec";
                        OpenHardwareMonitor.Eszkozok.Eszk.Elnevezo();
                    };
                }

            }
            Frissidmenu.Items.Add(beallItem);
            MouseEventArgs ev = e as MouseEventArgs;
            Frissidmenu.Show(new System.Drawing.Point((int)labelFrissId.PointToScreen(ev.GetPosition(labelFrissId)).X, (int)labelFrissId.PointToScreen(ev.GetPosition(labelFrissId)).Y));
        }

        private void labelCelh_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MF.menuItem57.PerformClick();
        }

        System.Windows.Forms.ContextMenuStrip Hiszterzmenu = new System.Windows.Forms.ContextMenuStrip();
        private void labelHiszt_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Hiszterzmenu.Items.Clear();

            System.Windows.Forms.ToolStripMenuItem beallItem = new System.Windows.Forms.ToolStripMenuItem((Program.KONFNyelv == "hun") ? "Beállítás" : (Eszk.GetNyelvSzo("Beallitas")));

            for (int i = 0; i <= 10; ++i)
            {
                System.Windows.Forms.ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem((i / (float)2) + "°C");
                beallItem.DropDownItems.Add(item);

                item.Checked = (int)Math.Round(Program.KONFHiszterezis * 2) == i;

                int iErtek = i;
                switch (iErtek)
                {
                    case 0:
                        item.Text = "0°C " + ((Program.KONFNyelv == "hun") ? "(kikapcsolva)" : ("(" + Eszk.GetNyelvSzo("kikapcsolva")) + ")");
                        item.Click += delegate (object obj, EventArgs args)
                        {
                            Program.KONFHiszterezis = 0;
                            labelHiszt.Content = ((Program.KONFNyelv == "hun") ? "Hiszterézis: " : (Eszk.GetNyelvSzo("ATTEKUIHiszterezis") + ": ")) + "0°C";
                            MF.HisztJelolo();
                        };
                        break;

                    case 4:
                        item.Text = "2°C " + ((Program.KONFNyelv == "hun") ? "(ajánlott)" : ("(" + Eszk.GetNyelvSzo("ajanlott")) + ")");
                        item.Click += delegate (object obj, EventArgs args)
                        {
                            Program.KONFHiszterezis = 2;
                            labelHiszt.Content = ((Program.KONFNyelv == "hun") ? "Hiszterézis: " : (Eszk.GetNyelvSzo("ATTEKUIHiszterezis") + ": ")) + "2°C";
                            MF.HisztJelolo();
                        };
                        break;


                    default:
                        item.Click += delegate (object obj, EventArgs args)
                        {
                            Program.KONFHiszterezis = iErtek / (float)2;
                            labelHiszt.Content = ((Program.KONFNyelv == "hun") ? "Hiszterézis: " : (Eszk.GetNyelvSzo("ATTEKUIHiszterezis") + ": ")) + (iErtek / (float)2).ToString() + "°C";
                            MF.HisztJelolo();
                        };
                        break;
                }

            }
            Hiszterzmenu.Items.Add(beallItem);
            MouseEventArgs ev = e as MouseEventArgs;
            Hiszterzmenu.Show(new System.Drawing.Point((int)labelHiszt.PointToScreen(ev.GetPosition(labelHiszt)).X, (int)labelHiszt.PointToScreen(ev.GetPosition(labelHiszt)).Y));
        }

        private void labelCim_MouseMove(object sender, MouseEventArgs e)
        {
            imageBackgnd_MouseMove(12, e);
        }

        private void imageVillFordsz_MouseMove(object sender, MouseEventArgs e)
        {
            imageBackgnd_MouseMove(12, e);
        }

        private void imageVillSzabl_MouseMove(object sender, MouseEventArgs e)
        {
            imageBackgnd_MouseMove(12, e);
        }
        private void imageVillHom_MouseMove(object sender, MouseEventArgs e)
        {
            imageBackgnd_MouseMove(12, e);
        }

        private void imageVillRiaszt_MouseMove(object sender, MouseEventArgs e)
        {
            imageBackgnd_MouseMove(12, e);
        }

        private void imageBackgnd_MouseEnter(object sender, MouseEventArgs e)
        {
            ScrollViewer.SetVerticalScrollBarVisibility(listViewFordszamok, ScrollBarVisibility.Hidden);
            ScrollViewer.SetHorizontalScrollBarVisibility(listViewFordszamok, ScrollBarVisibility.Hidden);
        }

        private void listViewFordszamok_MouseLeave(object sender, MouseEventArgs e)
        {
            //ScrollViewer.SetVerticalScrollBarVisibility(listViewFordszamok, ScrollBarVisibility.Hidden);
            //ScrollViewer.SetHorizontalScrollBarVisibility(listViewFordszamok, ScrollBarVisibility.Hidden);
        }

        private void listViewFordszamok_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ScrollViewer.SetVerticalScrollBarVisibility(listViewFordszamok, ScrollBarVisibility.Auto);
            ScrollViewer.SetHorizontalScrollBarVisibility(listViewFordszamok, ScrollBarVisibility.Auto);
            listViewFordszamok_MouseRightButtonUp(sender, e);
        }

        System.Windows.Forms.ContextMenuStrip Vezerlesmenu = new System.Windows.Forms.ContextMenuStrip();
        private void listViewFordszamok_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            GVItemFordsz SelItem = (GVItemFordsz)listViewFordszamok.SelectedItem;

            if (SelItem.Kontrol != null)
            {
                Vezerlesmenu.Items.Clear();

                IControl control = SelItem.Kontrol;

                System.Windows.Forms.ToolStripMenuItem controlItem = new System.Windows.Forms.ToolStripMenuItem(((Program.KONFNyelv == "hun") ? "Vezérlés" : Eszk.GetNyelvSzo("Vezérlés")));
                System.Windows.Forms.ToolStripMenuItem defaultItem = new System.Windows.Forms.ToolStripMenuItem(((Program.KONFNyelv == "hun") ? "Alapértelmezett" : Eszk.GetNyelvSzo("Alapértelmezett")));
                System.Windows.Forms.ToolStripMenuItem listaalapItem = new System.Windows.Forms.ToolStripMenuItem(((Program.KONFNyelv == "hun") ? "Listaalapú" : Eszk.GetNyelvSzo("Listaalapú")));

                //if (control.ControlMode == ControlMode.Undefined)
                //    control.SetCMListaAlapu();

                controlItem.DropDownItems.Add(defaultItem);

                defaultItem.Click += delegate (object obj, EventArgs args)
                {
                    control.SetDefault();
                };

                if (control.MaxSoftwareValue != control.MinSoftwareValue)
                {
                    controlItem.DropDownItems.Add(listaalapItem);
                    listaalapItem.Click += delegate (object obj, EventArgs args)
                    {
                        control.SetCMListaAlapu();
                        Program.HisztDeaktiv = true;
                    };
                    System.Windows.Forms.ToolStripMenuItem manualItem = new System.Windows.Forms.ToolStripMenuItem(((Program.KONFNyelv == "hun") ? "Manuális" : Eszk.GetNyelvSzo("Manuális")));
                    controlItem.DropDownItems.Add(manualItem);
                    for (int i = 0; i <= 100; i += 5)
                    {
                        if (i <= control.MaxSoftwareValue &&
                            i >= control.MinSoftwareValue)
                        {
                            System.Windows.Forms.ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem(i + " %");
                            manualItem.DropDownItems.Add(item);

                            item.Checked = control.ControlMode == ControlMode.Kezi &&
                              Math.Round(control.SoftwareValue) == i;
                            int softwareValue = i;
                            item.Click += delegate (object obj, EventArgs args)
                            {
                                control.SetSoftware(softwareValue);

                            };
                        }
                    }
                }
                if (control.ControlMode == ControlMode.Alapert)
                    defaultItem.Checked = true;
                else if (control.ControlMode == ControlMode.Listaalapu)
                    listaalapItem.Checked = true;

                Vezerlesmenu.Items.Add(controlItem);

                System.Windows.Forms.ToolStripMenuItem FelcimkezItem = new System.Windows.Forms.ToolStripMenuItem(((Program.KONFNyelv == "hun") ? "Felcímkézés" : Eszk.GetNyelvSzo("Felcímkézés")));
                controlItem.DropDownItems.Add(FelcimkezItem);
                FelcimkezItem.Click += delegate (object obj, EventArgs args)
                {
                    new Felcimkezo(SelItem.Kimenet, MF).ShowDialog();
                };

                MouseEventArgs ev = e as MouseEventArgs;
                Vezerlesmenu.Show(new System.Drawing.Point((int)listViewFordszamok.PointToScreen(ev.GetPosition(listViewFordszamok)).X, (int)listViewFordszamok.PointToScreen(ev.GetPosition(listViewFordszamok)).Y));
            }
            else if (SelItem.CsatIndex != -99)
            {
                Vezerlesmenu.Items.Clear();

                System.Windows.Forms.ToolStripMenuItem controlItem = new System.Windows.Forms.ToolStripMenuItem(((Program.KONFNyelv == "hun") ? "Vezérlés" : Eszk.GetNyelvSzo("Vezérlés")));
                System.Windows.Forms.ToolStripMenuItem listaalapItem = new System.Windows.Forms.ToolStripMenuItem(((Program.KONFNyelv == "hun") ? "Listaalapú" : Eszk.GetNyelvSzo("Listaalapú")));


                controlItem.DropDownItems.Add(listaalapItem);

                listaalapItem.Click += delegate (object obj, EventArgs args)
                {
                    MF.DirektVez[SelItem.CsatIndex] = false;
                    Program.HisztDeaktiv = true;
                };

                System.Windows.Forms.ToolStripMenuItem manualItem = new System.Windows.Forms.ToolStripMenuItem(((Program.KONFNyelv == "hun") ? "Manuális" : Eszk.GetNyelvSzo("Manuális")));
                controlItem.DropDownItems.Add(manualItem);
                for (byte i = 0; i <= 100; i += 5)
                {
                    System.Windows.Forms.ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem(i + " %");
                    manualItem.DropDownItems.Add(item);
                    byte softwareValue = i;
                    item.Checked = MF.KitTenyezok[SelItem.CsatIndex] == i &&
                        MF.DirektVez[SelItem.CsatIndex];
                    item.Click += delegate (object obj, EventArgs args)
                    {
                        MF.DirektVez[SelItem.CsatIndex] = true;
                        MF.KitTenyezok[SelItem.CsatIndex] = softwareValue;

                    };
                }

                if (MF.DirektVez[SelItem.CsatIndex] == false)
                    listaalapItem.Checked = true;

                Vezerlesmenu.Items.Add(controlItem);

                System.Windows.Forms.ToolStripMenuItem FelcimkezItem = new System.Windows.Forms.ToolStripMenuItem(((Program.KONFNyelv == "hun") ? "Felcímkézés" : Eszk.GetNyelvSzo("Felcímkézés")));
                controlItem.DropDownItems.Add(FelcimkezItem);
                FelcimkezItem.Click += delegate (object obj, EventArgs args)
                {
                    new Felcimkezo(SelItem.CsatIndex, MF).ShowDialog();
                };

                MouseEventArgs ev = e as MouseEventArgs;
                Vezerlesmenu.Show(new System.Drawing.Point((int)listViewFordszamok.PointToScreen(ev.GetPosition(listViewFordszamok)).X, (int)listViewFordszamok.PointToScreen(ev.GetPosition(listViewFordszamok)).Y));
            }
            else if (SelItem.KontrolEMULALT != null)
            {
                Vezerlesmenu.Items.Clear();

                Program.EmulaltBelsoVenti control = SelItem.KontrolEMULALT;

                System.Windows.Forms.ToolStripMenuItem controlItem = new System.Windows.Forms.ToolStripMenuItem(((Program.KONFNyelv == "hun") ? "Vezérlés" : Eszk.GetNyelvSzo("Vezérlés")));
                System.Windows.Forms.ToolStripMenuItem defaultItem = new System.Windows.Forms.ToolStripMenuItem(((Program.KONFNyelv == "hun") ? "Alapértelmezett" : Eszk.GetNyelvSzo("Alapértelmezett")));
                System.Windows.Forms.ToolStripMenuItem listaalapItem = new System.Windows.Forms.ToolStripMenuItem(((Program.KONFNyelv == "hun") ? "Listaalapú" : Eszk.GetNyelvSzo("Listaalapú")));

                //if (control.ControlMode == ControlMode.Undefined)
                //    control.SetCMListaAlapu();

                controlItem.DropDownItems.Add(listaalapItem);
                controlItem.DropDownItems.Add(defaultItem);

                defaultItem.Click += delegate (object obj, EventArgs args)
                {
                    control.ControlMode = ControlMode.Alapert;
                };
                listaalapItem.Click += delegate (object obj, EventArgs args)
                {
                    control.ControlMode = ControlMode.Listaalapu;
                    Program.HisztDeaktiv = true;
                };


                System.Windows.Forms.ToolStripMenuItem manualItem = new System.Windows.Forms.ToolStripMenuItem(((Program.KONFNyelv == "hun") ? "Manuális" : Eszk.GetNyelvSzo("Manuális")));
                controlItem.DropDownItems.Add(manualItem);
                for (int i = 0; i <= 100; i += 5)
                {

                    System.Windows.Forms.ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem(i + " %");
                    manualItem.DropDownItems.Add(item);

                    item.Checked = control.ControlMode == ControlMode.Kezi &&
                        control.ertek == i;
                    int softwareValue = i;
                    item.Click += delegate (object obj, EventArgs args)
                    {
                        control.ertek = (byte)softwareValue;
                        control.ControlMode = ControlMode.Kezi;

                    };
                }

                if (control.ControlMode == ControlMode.Alapert)
                    defaultItem.Checked = true;
                else if (control.ControlMode == ControlMode.Listaalapu)
                    listaalapItem.Checked = true;

                Vezerlesmenu.Items.Add(controlItem);


                MouseEventArgs ev = e as MouseEventArgs;
                Vezerlesmenu.Show(new System.Drawing.Point((int)listViewFordszamok.PointToScreen(ev.GetPosition(listViewFordszamok)).X, (int)listViewFordszamok.PointToScreen(ev.GetPosition(listViewFordszamok)).Y));
            }
            ScrollViewer.SetVerticalScrollBarVisibility(listViewFordszamok, ScrollBarVisibility.Auto);
            ScrollViewer.SetHorizontalScrollBarVisibility(listViewFordszamok, ScrollBarVisibility.Auto);
        }

        public void ShowHelppel()
        {
            Show();
            SajatHelpekMegejenites(true);
        }
        public void HideHelppel()
        {
            SajatHelpekMegejenites(false);
            Hide();
        }

        private void imageFeedback_MouseEnter(object sender, MouseEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/FeedbackE.png");
            try { logo.EndInit(); } catch { }
            try { imageFeedback.Source = logo; } catch { }
        }

        private void imageFeedback_MouseLeave(object sender, MouseEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/Feverkill;component/WPF/Attekinto/Rescources/FeedbackA.png");
            try { logo.EndInit(); } catch { }
            try { imageFeedback.Source = logo; } catch { }
        }

        private void imageFeedback_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MF.menuItem23.PerformClick();
        }
    }


    public class Lokalizalas : Window, INotifyPropertyChanged
    {

        public void Lokalizalj()
        {
            string[] verzszamdarabok = Program.Verzioszam.Split('.');
            string kiirandoverzszam = verzszamdarabok[0] + "." + verzszamdarabok[1] + "." + verzszamdarabok[2];

            if (Program.KONFNyelv == "hun")
            {
                UICim = "Feverkill Ventilátorvezérlő" + "  V" + kiirandoverzszam + "      >>Beta<";
                LVFKimenet = "Kimenet";
                LVFFordulatszam = "Fordulatszám";
                LVFVezerles = "Vezérlés Típusa";
                LVHSzenzor = "Szenzor";
                LVHErtek = "Érték";
                LVLNev = "Név";
                LVLHoszenzor = "Hőszenzor";
                LVLCsatornak = "Csatornák";
                LVRHoszenzor = "Hőszenzor";
                LVRRel = "R";
                LVRFok = "°C";
                LVRUzenet = "Üzenet";
                LVRHangjelzes = "Hangjelzés";
                LVRSpecMuv = "Speciális Művelet";
                LVREbresztes = "Ébresztés";
                UIHiszterezis = "Hiszterézis: ?°C";
                UICelhardver = "Célhardver: N/A";
                UIFrissido = "Frissítési időköz: ?sec";
            }
            else
            {
                NyelvBeolvas(kiirandoverzszam);
            }
        }
        void NyelvBeolvas(string kiirandoverzszam)
        {
            UICim = Eszk.GetNyelvSzo("ATTEKUICim") + "  V" + kiirandoverzszam + "      >>Beta<";
            LVFKimenet = Eszk.GetNyelvSzo("ATTEKLVFKimenet");
            LVFFordulatszam = Eszk.GetNyelvSzo("ATTEKLVFFordulatszam");
            LVFVezerles = Eszk.GetNyelvSzo("ATTEKLVFVezerles");
            LVHSzenzor = Eszk.GetNyelvSzo("ATTEKLVHSzenzor");
            LVHErtek = Eszk.GetNyelvSzo("ATTEKLVHErtek");
            LVLNev = Eszk.GetNyelvSzo("ATTEKLVLNev");
            LVLHoszenzor = Eszk.GetNyelvSzo("ATTEKLVLHoszenzor");
            LVLCsatornak = Eszk.GetNyelvSzo("ATTEKLVLCsatornak");
            LVRHoszenzor = Eszk.GetNyelvSzo("ATTEKLVRHoszenzor");
            LVRRel = Eszk.GetNyelvSzo("ATTEKLVRRel");
            LVRFok = Eszk.GetNyelvSzo("ATTEKLVRFok");
            LVRUzenet = Eszk.GetNyelvSzo("ATTEKLVRUzenet");
            LVRHangjelzes = Eszk.GetNyelvSzo("ATTEKLVHangjelzes");
            LVRSpecMuv = Eszk.GetNyelvSzo("ATTEKLVSpecMuv");
            LVREbresztes = Eszk.GetNyelvSzo("ATTEKLVEbresztes");
            UIHiszterezis = Eszk.GetNyelvSzo("ATTEKUIHiszterezis") + ": ?°C";
            UICelhardver = Eszk.GetNyelvSzo("ATTEKUICelhardver") + ": N/A";
            UIFrissido = Eszk.GetNyelvSzo("ATTEKUIFrissido") + ": ?sec";
        }
        public Lokalizalas()
        {
        }

        private string _LVFKimenet, _LVFFordulatszam, _LVFVezerles;
        private string _LVHSzenzor, _LVHErtek;
        private string _LVLNev, _LVLHoszenzor, _LVLCsatornak;
        private string _LVRHoszenzor, _LVRRel, _LVRFok, _LVRUzenet, _LVRHangjelzes, _LVRSpecMuv, _LVREbresztes;
        private string _UICim, _UIHiszterezis, _UICelhardver, _UIFrissido;

        #region PROPERTYK
        #region LVFKimenet
        public string LVFKimenet
        {
            get
            {
                return _LVFKimenet;
            }
            set
            {
                _LVFKimenet = value;
                OnPropertyChanged("LVFKimenet");
            }
        }
        #endregion


        #region LVFFordulatszam
        public string LVFFordulatszam
        {
            get
            {
                return _LVFFordulatszam;
            }
            set
            {
                _LVFFordulatszam = value;
                OnPropertyChanged("LVFFordulatszam");
            }
        }
        #endregion


        #region LVFVezerles
        public string LVFVezerles
        {
            get
            {
                return _LVFVezerles;
            }
            set
            {
                _LVFVezerles = value;
                OnPropertyChanged("LVFVezerles");
            }
        }
        #endregion


        #region LVHSzenzor
        public string LVHSzenzor
        {
            get
            {
                return _LVHSzenzor;
            }
            set
            {
                _LVHSzenzor = value;
                OnPropertyChanged("LVHSzenzor");
            }
        }
        #endregion


        #region LVHErtek
        public string LVHErtek
        {
            get
            {
                return _LVHErtek;
            }
            set
            {
                _LVHErtek = value;
                OnPropertyChanged("LVHErtek");
            }
        }
        #endregion


        #region LVLNev
        public string LVLNev
        {
            get
            {
                return _LVLNev;
            }
            set
            {
                _LVLNev = value;
                OnPropertyChanged("LVLNev");
            }
        }
        #endregion


        #region LVLHoszenzor
        public string LVLHoszenzor
        {
            get
            {
                return _LVLHoszenzor;
            }
            set
            {
                _LVLHoszenzor = value;
                OnPropertyChanged("LVLHoszenzor");
            }
        }
        #endregion


        #region LVLCsatornak
        public string LVLCsatornak
        {
            get
            {
                return _LVLCsatornak;
            }
            set
            {
                _LVLCsatornak = value;
                OnPropertyChanged("LVLCsatornak");
            }
        }
        #endregion


        #region LVRHoszenzor
        public string LVRHoszenzor
        {
            get
            {
                return _LVRHoszenzor;
            }
            set
            {
                _LVRHoszenzor = value;
                OnPropertyChanged("LVRHoszenzor");
            }
        }
        #endregion


        #region LVRRel
        public string LVRRel
        {
            get
            {
                return _LVRRel;
            }
            set
            {
                _LVRRel = value;
                OnPropertyChanged("LVRRel");
            }
        }
        #endregion


        #region LVRFok
        public string LVRFok
        {
            get
            {
                return _LVRFok;
            }
            set
            {
                _LVRFok = value;
                OnPropertyChanged("LVRFok");
            }
        }
        #endregion


        #region LVRUzenet
        public string LVRUzenet
        {
            get
            {
                return _LVRUzenet;
            }
            set
            {
                _LVRUzenet = value;
                OnPropertyChanged("LVRUzenet");
            }
        }
        #endregion


        #region LVRHangjelzes
        public string LVRHangjelzes
        {
            get
            {
                return _LVRHangjelzes;
            }
            set
            {
                _LVRHangjelzes = value;
                OnPropertyChanged("LVRHangjelzes");
            }
        }
        #endregion


        #region LVRSpecMuv
        public string LVRSpecMuv
        {
            get
            {
                return _LVRSpecMuv;
            }
            set
            {
                _LVRSpecMuv = value;
                OnPropertyChanged("LVRSpecMuv");
            }
        }
        #endregion


        #region LVREbresztes
        public string LVREbresztes
        {
            get
            {
                return _LVREbresztes;
            }
            set
            {
                _LVREbresztes = value;
                OnPropertyChanged("LVREbresztes");
            }
        }
        #endregion


        #region UICim
        public string UICim
        {
            get
            {
                return _UICim;
            }
            set
            {
                _UICim = value;
                OnPropertyChanged("UICim");
            }
        }
        #endregion


        #region UIHiszterezis
        public string UIHiszterezis
        {
            get
            {
                return _UIHiszterezis;
            }
            set
            {
                _UIHiszterezis = value;
                OnPropertyChanged("UIHiszterezis");
            }
        }
        #endregion


        #region UICelhardver
        public string UICelhardver
        {
            get
            {
                return _UICelhardver;
            }
            set
            {
                _UICelhardver = value;
                OnPropertyChanged("UICelhardver");
            }
        }
        #endregion


        #region UIFrissido
        public string UIFrissido
        {
            get
            {
                return _UIFrissido;
            }
            set
            {
                _UIFrissido = value;
                OnPropertyChanged("UIFrissido");
            }
        }
        #endregion
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class ListBoxScroll : ListBox
    {
        public ListBoxScroll() : base()
        {
            SelectionChanged += new SelectionChangedEventHandler(ListBoxScroll_SelectionChanged);
        }

        void ListBoxScroll_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ScrollIntoView(SelectedItem);
        }
    }
}
