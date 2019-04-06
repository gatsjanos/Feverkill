using OpenHardwareMonitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace UdvozloKepernyo
{
    public class SegTestInfo
    {
        public bool Lathato = false;
        public bool Villogas = false;
        public Point? KezdEltolas = null;
        public string Szoveg = "";
        public double Betumeret = 36;
        public FontFamily Betutipus = new FontFamily("Segoe Print");
        public Brush Szin = Brushes.Red;
        public System.Windows.Forms.Control FromsControl = null;
        public FrameworkElement WPFControl = null;

    }
    public class SegTestKezelo
    {
        public static Dictionary<string, SegTestInfo> SegTestInfDic = new Dictionary<string, SegTestInfo>();

        public static bool vanWPFTutorUjAdat = false;

        public static void SegedTestFeltolto()
        {
            //SegTestInfDic.Add("A1", new SegTestInfo() { WPFControl = Program.FoAblak.Attekint.labelCim, szoveg = "I'm CEO, Bitch...", Lathato = false });
        }

        public static List<string> AdatmuvLathatosagBe = new List<string>();
        public static List<string> AdatmuvLathatosagKi = new List<string>();
        public static List<string> AdatmuvHozzaadas = new List<string>();

        public static void SegtestHozzaad(string azonosito, System.Windows.Forms.Control fromsControl, string szoveg, double betumeret, string betutipus, Brush szin, bool villogas, bool lathato, Point? kezdEltolas, bool FELULIRAS)
        {
            SegtestHozzaad(azonosito, fromsControl, null, szoveg, betumeret, betutipus, szin, villogas, lathato, kezdEltolas, FELULIRAS);
        }
        public static void SegtestHozzaad(string azonosito, FrameworkElement wPFControl, string szoveg, double betumeret, string betutipus, Brush szin, bool villogas, bool lathato, Point? kezdEltolas, bool FELULIRAS)
        {
            SegtestHozzaad(azonosito, null, wPFControl, szoveg, betumeret, betutipus, szin, villogas, lathato, kezdEltolas, FELULIRAS);
        }

        public static void SegtestHozzaad(string azonosito, System.Windows.Forms.Control fromsControl, FrameworkElement wPFControl, string szoveg, double betumeret, string betutipus, Brush szin, bool villogas, bool lathato, Point? kezdEltolas, bool FELULIRAS)
        {
            if (SegTestInfDic.ContainsKey(azonosito))
            {
                if (FELULIRAS)
                    SegTestInfDic[azonosito] = new SegTestInfo() { FromsControl = fromsControl, WPFControl = wPFControl, Szoveg = szoveg, Betumeret = betumeret, Betutipus = new FontFamily(betutipus), Szin = szin, KezdEltolas = kezdEltolas, Villogas = villogas, Lathato = lathato };
            }
            else
            {
                SegTestInfDic.Add(azonosito, new SegTestInfo() { FromsControl = fromsControl, WPFControl = wPFControl, Szoveg = szoveg, Betumeret = betumeret, Betutipus = new FontFamily(betutipus), Szin = szin, KezdEltolas = kezdEltolas, Villogas = villogas, Lathato = lathato });
            }

            try
            {
                AdatmuvHozzaadas.Add(azonosito);
            }
            catch { }

            vanWPFTutorUjAdat = true;
            //try
            //{
            //    if (Program.TutorialWPFAblak != null)
            //    {
            //        Program.TutorialWPFAblak.Dispatcher.Invoke(delegate ()
            //        {
            //            if (Program.TutorialWPFAblak.WPFTutSegTestDic.ContainsKey(azonosito))
            //            {

            //                Program.TutorialWPFAblak.WPFTutSegTestDic[azonosito] = Program.TutorialWPFAblak.CreateSegedtest(fromsControl, wPFControl, szoveg, betumeret, betutipus, szin, lathato);

            //            }
            //            else
            //            {

            //                Program.TutorialWPFAblak.WPFTutSegTestDic.Add(azonosito, Program.TutorialWPFAblak.CreateSegedtest(fromsControl, wPFControl, szoveg, betumeret, betutipus, szin, lathato));

            //            }
            //        });
            //    }
            //}
            //catch
            //{ }
            //vanWPFTutorUjAdat = false;
        }

        public static void SetMegjelenites(string azonosito, bool lathato)
        {
            if (SegTestInfDic.ContainsKey(azonosito))
            {
                SegTestInfDic[azonosito].Lathato = lathato;
            }

            try
            {
                if (lathato)
                    AdatmuvLathatosagBe.Add(azonosito);
                else
                    AdatmuvLathatosagKi.Add(azonosito);

            }
            catch { }

            vanWPFTutorUjAdat = true;
            //try
            //{
            //    if (Program.TutorialWPFAblak != null)
            //    {
            //        Program.TutorialWPFAblak.Dispatcher.Invoke(delegate ()
            //        {
            //            if (Program.TutorialWPFAblak.WPFTutSegTestDic.ContainsKey(azonosito))
            //            {
            //                Program.TutorialWPFAblak.WPFTutSegTestDic[azonosito].Lathato = lathato;
            //            }
            //        });
            //    }
            //}
            //catch
            //{ }
            //vanWPFTutorUjAdat = false;
        }
        public static void Megjelenit(string azonosito)
        {
            SetMegjelenites(azonosito, true);
        }
        public static void Elrejt(string azonosito)
        {

            SetMegjelenites(azonosito, false);

        }
    }
}
