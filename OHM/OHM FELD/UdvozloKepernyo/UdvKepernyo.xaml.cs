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
using System.Threading;
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace UdvozloKepernyo
{

    public class UdvKepernyokMegjelenito
    {
        static UdvKepernyo[] UdvKepernyok = new UdvKepernyo[0];
        public static bool[] Vegeztek;
        public static void Inicializalas(string VerzioSzam, string LNev, string LEmail, string LID, string LErvenyesseg, string Nyelv, string VezSzoft, string Licenszelve)
        {
            UdvKepernyok = new UdvKepernyo[System.Windows.Forms.Screen.AllScreens.Length];
            Vegeztek = new bool[UdvKepernyok.Length];

            for (int i = 0; i < UdvKepernyok.Length; i++)
            {
                try
                {
                    Vegeztek[i] = false;

                    UdvKepernyo UKx = new UdvKepernyo(i);
                    UKx.Left = System.Windows.Forms.Screen.AllScreens[i].Bounds.X + ((System.Windows.Forms.Screen.AllScreens[i].WorkingArea.Width - UKx.Width) / 2);
                    UKx.Top = System.Windows.Forms.Screen.AllScreens[i].Bounds.Y + ((System.Windows.Forms.Screen.AllScreens[i].WorkingArea.Height - UKx.Height) / 2);

                    UdvKepernyok[i] = UKx;

                    UKx.Kitolt(VerzioSzam, LNev, LEmail, LID, LErvenyesseg, Nyelv, VezSzoft, Licenszelve);
                }
                catch { }
            }

            new Application().Run();

        }
        public static void Elhalvanyito()
        {
            foreach (var item in UdvKepernyok)
            {
                try
                {
                    item.Dispatcher.Invoke(() => item.Elhalvanyito());
                }
                catch { }
            }
        }
    }

    public partial class UdvKepernyo : Window
    {
        int Azonosito = 0;
        public UdvKepernyo(int azonosito)
        {
            Azonosito = azonosito;
            InitializeComponent();
        }
        internal void Elhalvanyito()
        {

            da.RepeatBehavior = new RepeatBehavior(1);
            da.From = Opacity;
            da.To = 0;
            da.Duration = new Duration(TimeSpan.FromMilliseconds(1000));
            da.AutoReverse = false;
            da.Completed += Da_MasodikCompleted;
            //da.RepeatBehavior=new RepeatBehavior(3);
            this.BeginAnimation(OpacityProperty, da);
        }

        private void Da_MasodikCompleted(object sender, EventArgs e)
        {
            UdvKepernyokMegjelenito.Vegeztek[Azonosito] = true;

            foreach (var item in UdvKepernyokMegjelenito.Vegeztek)
            {
                if (!item)
                    return;
            }
            try
            {
                Thread.CurrentThread.Abort();
            }
            catch { }
        }

        DoubleAnimation da = new DoubleAnimation();
        void Megjelenito()
        {
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromMilliseconds(800));
            //da.AutoReverse = true;
            //da.RepeatBehavior = RepeatBehavior.Forever;
            //da.RepeatBehavior=new RepeatBehavior(3);
            da.Completed += Da_ElsoCompleted;
            this.BeginAnimation(OpacityProperty, da);

        }

        private void Da_ElsoCompleted(object sender, EventArgs e)
        {
            da.Completed -= Da_ElsoCompleted;
            da.From = imageGif.Opacity;
            da.To = 0;
            da.Duration = new Duration(TimeSpan.FromMilliseconds(2200));
            da.AutoReverse = true;
            da.RepeatBehavior = RepeatBehavior.Forever;
            //da.RepeatBehavior=new RepeatBehavior(3);
            imageGif.BeginAnimation(OpacityProperty, da);
        }
        internal void Kitolt(string VerzioSzam, string LNev, string LEmail, string LID, string LErvenyesseg, string Nyelv, string VezSzoft, string Licenszelve)
        {
            string[] verzszamdarabok = VerzioSzam.Split('.');
            string kiirandoverzszam = verzszamdarabok[0] + "." + verzszamdarabok[1] + "." + verzszamdarabok[2];

            labelVerzio.Content = "V" + kiirandoverzszam;
            labelNev.Content = LNev;
            labelEmail.Content = LEmail;
            labelID.Content = LID;
            try
            {
                string[] erv = LErvenyesseg.Split('.');
                DateTime dt = new DateTime(int.Parse(erv[0]), int.Parse(erv[1]), int.Parse(erv[2]));//Nyelvterülethez való dátumformázás
                labelErvenyesseg.Content = dt.ToShortDateString();
            }
            catch { }
            if (Nyelv != "hun")
                Lokalizalj(VezSzoft, Licenszelve);
            
            Opacity = 0;
            imageGif.Opacity = 0.8;
            Show();
            Topmost = true;
            Topmost = false;
            Megjelenito();
        }

        void Lokalizalj(string VezSzoft, string Licenszelve)
        {
            label_Copy1.Margin = new Thickness(10, 69, 0, -10);
            label_Copy1.Content = VezSzoft;
            label.Content = Licenszelve;
        }
    }

}
