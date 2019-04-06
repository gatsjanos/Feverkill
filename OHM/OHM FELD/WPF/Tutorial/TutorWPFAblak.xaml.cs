using OpenHardwareMonitor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UdvozloKepernyo
{
    /// <summary>
    /// Interaction logic for TutorWPF.xaml
    /// </summary>
    public partial class TutorWPFAblak : Window
    {
        public bool HintSzinVillanas = false;
        public TutorWPFAblak()
        {
            OpenHardwareMonitor.Program.FoAblak.TutorWPFMegjelenitve = true;
            InitializeComponent();

            //if (System.Windows.Forms.Screen.AllScreens.Length > 1)
            {
                System.Drawing.Rectangle totalSize = System.Drawing.Rectangle.Empty;
                foreach (System.Windows.Forms.Screen s in System.Windows.Forms.Screen.AllScreens)
                    totalSize = System.Drawing.Rectangle.Union(totalSize, s.Bounds);


                this.Width = totalSize.Width;
                this.Height = totalSize.Height;

                this.Left = totalSize.Left;
                this.Top = totalSize.Top;
            }

            WPFSegedtest.SajatDropShadowEffect = null;//Kikapcsolja az ányék effektet -> teljesítményigény. Lesz egy gomb, amivel ki-be lehet kapcsolni
                                                      //WPFSegedtest.SajatDropShadowEffect = new DropShadowEffect() { BlurRadius = 6, Color = Colors.Black, Direction = 225, Opacity = 100, ShadowDepth = 2, RenderingBias = RenderingBias.Performance };//Bekapcsolja az árnyék effektet.
            foreach (KeyValuePair<string, SegTestInfo> item in SegTestKezelo.SegTestInfDic)
            {
                WPFTutSegTestDic.Add(item.Key, CreateSegedtest(item.Value.FromsControl, item.Value.WPFControl, item.Value.Szoveg, item.Value.Betumeret, item.Value.Betutipus, item.Value.Szin, item.Value.KezdEltolas, item.Value.Villogas, item.Value.Lathato));
            }
            {
                // Program.WPFTutSegTestDic.Add("A1",CreateSegedtest(null, Program.FoAblak.Attekint.labelCelh, "This is for the label.\nmásodik sor\n3. sor"));
                //  Program.WPFTutSegTestDic.Add("A2", CreateSegedtest(null, Program.FoAblak.Attekint.labelCim, "I'm CEO, Bitch..."));
            }

            tmrHintek.Interval = new TimeSpan(0, 0, 0, 0, 10);
            tmrHintek.Tick += Tmr_Tick;
            tmrHintek.Start();

            tmrVillanas.Interval = new TimeSpan(0, 0, 0, 0, 400);
            tmrVillanas.Tick += delegate (object sender, EventArgs e) { HintSzinVillanas = !HintSzinVillanas; };
            tmrVillanas.Start();

            SegTestKezelo.SegtestHozzaad("TutorWPFAblak1", null, null, "<= You can move the hint grips!\n =>Click the text to collapse!", 100, "Impact", Brushes.DarkOrange, false, true, null, true);

            if (Program.SzabListak.Count == 0)
            {
                MutassMessagebox("Create your first control scheme!\n\nClick the appropriate lightbulb to do this. It's the one in the center shown by the green label with blinking arrow: \"Click here to manage Control Schemes\".", "Welcome feverkiller!");
                
                //MutassMessagebox("Also note that you can move the help labels by dragging the node on their left side.\n\nIf you click the label text it will collapse. Click the '>' to expand it again.", "Moveable hints");
            }
            

        }
        System.Windows.Threading.DispatcherTimer tmrVillanas = new System.Windows.Threading.DispatcherTimer();


        public void MutassMessagebox(string torzs, string cim)
        {
            try
            {
                mboxtorzs = torzs;
                mboxcim = cim;
                mutassMbox = true;
            }
            catch { }
        }
        string mboxtorzs = "", mboxcim = "";
        bool mutassMbox = false;

        public Dictionary<string, UdvozloKepernyo.WPFSegedtest> WPFTutSegTestDic = new Dictionary<string, UdvozloKepernyo.WPFSegedtest>();
        System.Windows.Threading.DispatcherTimer tmrHintek = new System.Windows.Threading.DispatcherTimer();
        public bool ElsoTopmostDeakt = true;
        private void Tmr_Tick(object sender, EventArgs e)
        {
            try
            {
                if (mutassMbox)
                {
                    this.Dispatcher.BeginInvoke((Action)delegate () { new MBoxShower(mboxtorzs, mboxcim).ShowDialog();/*MessageBox.Show(mboxtorzs, mboxcim);*/});
                    mutassMbox = false;
                }

                if (Program.KONFFelulMarado)
                {
                    Program.FoAblak.menuItem6.PerformClick();

                    if (!ElsoTopmostDeakt)
                    {
                        this.Dispatcher.BeginInvoke((Action)delegate () { new TopMostDisabledUzenet().ShowDialog(); });
                    }

                    ElsoTopmostDeakt = false;

                }

                if (_currentControl != null)
                {
                    System.Drawing.Point point;
                    if (!GetCursorPos(out point))
                        throw new InvalidOperationException("GetCursorPos failed");

                    Point p = new Point(point.X, point.Y);
                    p = AlapGrid.PointFromScreen(p);
                    p = new Point(p.X - RelPozGridhez.X, p.Y - RelPozGridhez.Y);
                    _currentControl.Margin = new Thickness(p.X, p.Y, 0, 0);
                }

                foreach (KeyValuePair<string, WPFSegedtest> item in WPFTutSegTestDic)
                {
                    item.Value.OsszekotoRajzol();
                }
            }
            catch { }

            try
            {
                if (SegTestKezelo.vanWPFTutorUjAdat)
                {
                    string kulcs;
                    while (SegTestKezelo.AdatmuvHozzaadas.Count != 0)
                    {
                        kulcs = SegTestKezelo.AdatmuvHozzaadas[0];

                        if (!WPFTutSegTestDic.ContainsKey(kulcs))
                            WPFTutSegTestDic.Add(kulcs, CreateSegedtest(SegTestKezelo.SegTestInfDic[kulcs].FromsControl, SegTestKezelo.SegTestInfDic[kulcs].WPFControl, SegTestKezelo.SegTestInfDic[kulcs].Szoveg, SegTestKezelo.SegTestInfDic[kulcs].Betumeret, SegTestKezelo.SegTestInfDic[kulcs].Betutipus, SegTestKezelo.SegTestInfDic[kulcs].Szin, SegTestKezelo.SegTestInfDic[kulcs].KezdEltolas, SegTestKezelo.SegTestInfDic[kulcs].Villogas, SegTestKezelo.SegTestInfDic[kulcs].Lathato));
                        else
                        {
                            WPFTutSegTestDic[kulcs].Lathato = false;
                            WPFTutSegTestDic[kulcs].OsszekotoRajzol();
                            WPFTutSegTestDic[kulcs] = CreateSegedtest(SegTestKezelo.SegTestInfDic[kulcs].FromsControl, SegTestKezelo.SegTestInfDic[kulcs].WPFControl, SegTestKezelo.SegTestInfDic[kulcs].Szoveg, SegTestKezelo.SegTestInfDic[kulcs].Betumeret, SegTestKezelo.SegTestInfDic[kulcs].Betutipus, SegTestKezelo.SegTestInfDic[kulcs].Szin, SegTestKezelo.SegTestInfDic[kulcs].KezdEltolas, SegTestKezelo.SegTestInfDic[kulcs].Villogas, SegTestKezelo.SegTestInfDic[kulcs].Lathato);
                        }
                        SegTestKezelo.AdatmuvHozzaadas.RemoveAt(0);
                    }

                    while (SegTestKezelo.AdatmuvLathatosagKi.Count != 0)
                    {
                        kulcs = SegTestKezelo.AdatmuvLathatosagKi[0];

                        if (WPFTutSegTestDic.ContainsKey(kulcs))
                            WPFTutSegTestDic[kulcs].Lathato = false;

                        SegTestKezelo.AdatmuvLathatosagKi.RemoveAt(0);
                    }

                    while (SegTestKezelo.AdatmuvLathatosagBe.Count != 0)
                    {
                        kulcs = SegTestKezelo.AdatmuvLathatosagBe[0];

                        if (WPFTutSegTestDic.ContainsKey(kulcs))
                            WPFTutSegTestDic[kulcs].Lathato = true;

                        SegTestKezelo.AdatmuvLathatosagBe.RemoveAt(0);
                    }


                    //foreach (KeyValuePair<string, WPFSegedtest> item in WPFTutSegTestDic)
                    //{

                    //}
                    //WPFTutSegTestDic.Clear();
                    //AlapGrid.Children.Clear();

                    //foreach (KeyValuePair<string, SegTestInfo> item in SegTestKezelo.SegTestInfDic)
                    //{
                    //    WPFTutSegTestDic.Add(item.Key, CreateSegedtest(item.Value.FromsControl, item.Value.WPFControl, item.Value.Szoveg, item.Value.Betumeret, item.Value.Betutipus, item.Value.Szin, item.Value.Lathato));
                    //}

                    SegTestKezelo.vanWPFTutorUjAdat = false;
                }
            }
            catch { }
            try
            {
                if (Program.FoAblak.TutorWPFLeallit)
                    this.Close();
            }
            catch { }

            try
            {
                if (!Program.FoAblak.TutorWPFMegjelenitve)
                    tmrHintek.Stop();
            }
            catch { }
        }
        public WPFSegedtest CreateSegedtest(System.Windows.Forms.Control formcontrol, FrameworkElement wpfcontrol, string szoveg, double betumeret, FontFamily betutipus, Brush szin, Point? kezdEltolas, bool villogas, bool lathato)
        {
            double pozx, pozy;
            Grid grid = fogantyurajzolo(25, 4, szin, out pozx, out pozy);
            WPFSegedtest ki = new WPFSegedtest(this) { FogX = pozx, FogY = pozy, FogA = 25, FogV = 4, Szin = szin, KezdEltolas = kezdEltolas };
            ki.Lathato = lathato;
            ki.Villogas = villogas;

            ki.grid = grid;

            if (true)
            {
                grid.MouseUp += delegate (object sender, MouseButtonEventArgs e)
                {
                    try
                    {
                        if (e.ChangedButton == MouseButton.Right)
                            MessageBox.Show("Coordinate offset to controlcenter:\n\tdX: " + (grid.Margin.Left - ki.ControlKozepe.X) + "\n\tdY: " + (grid.Margin.Top - ki.ControlKozepe.Y), "DEVELOPER DEBUGGING TOOL");
                    }
                    catch { }
                };
            }

            //if (kezdEltolas == null)
            //    kezdEltolas = new Point(Program.RandomObject.Next(30, (int)this.Width - 300), Program.RandomObject.Next(30, (int)this.Height - 150));


            Label l = new Label();
            l.Effect = WPFSegedtest.SajatDropShadowEffect;
            l.Content = szoveg;
            l.HorizontalAlignment = HorizontalAlignment.Left;
            l.VerticalAlignment = VerticalAlignment.Top;
            l.Background = null;
            l.Foreground = szin;
            l.FontFamily = betutipus;// "Segoe Print";
            l.FontSize = betumeret;
            l.IsHitTestVisible = true;



            l.MouseDown += delegate (object sender, MouseButtonEventArgs e) { if ((string)l.Content == ">") { l.FontSize = betumeret; l.Content = szoveg; } else { l.Content = ">"; l.FontSize = betumeret * 3; } };

            l.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            l.Margin = new Thickness(pozx + 40, pozy - (l.DesiredSize.Height / 2), 0, 0);


            try { grid.Margin = new Thickness(Program.RandomObject.Next(30, (int)(this.Width - l.DesiredSize.Width)), Program.RandomObject.Next(30, (int)(this.Height - l.DesiredSize.Height)), 0, 0); }
            catch
            {
                grid.Margin = new Thickness(Program.RandomObject.Next(30, (int)this.Width - 300), Program.RandomObject.Next(30, (int)this.Height - 150), 0, 0);
            }

            grid.Children.Add(l);

            if (formcontrol != null)
            {
                ki.FormsControl = formcontrol;
            }
            else if (wpfcontrol != null)
            {
                ki.WPFControl = wpfcontrol;
            }

            return ki;
        }
        Grid fogantyurajzolo(double a, double v, Brush szin)
        {
            double x, y;
            return fogantyurajzolo(25, 4, szin, out x, out y);
        }
        Grid fogantyurajzolo(double a, double v, Brush szin, out double x, out double y)
        {
            //szükséges távolság a gid szélétől: kiskörátmérő/2 + a
            //v*2.6 : kiskörök mérete

            x = (v * 2.6) / 2 + a;
            y = (v * 2.6) / 2 + a;

            Grid grid = new Grid();
            grid.HorizontalAlignment = HorizontalAlignment.Left;
            grid.VerticalAlignment = VerticalAlignment.Top;
            Ellipse el;
            Line Linex = null;
            for (int i = 0; i < 4; i++)
            {
                Linex = new Line();
                Linex.Effect = WPFSegedtest.SajatDropShadowEffect;
                Linex.Stroke = szin;
                switch (i)
                {
                    case 0:
                        {
                            Linex.X1 = x - a;
                            Linex.Y1 = y;
                            Linex.X2 = x;
                            Linex.Y2 = y - a;
                            break;
                        }
                    case 1:
                        {
                            Linex.X1 = x;
                            Linex.Y1 = y - a;
                            Linex.X2 = x + a;
                            Linex.Y2 = y;
                            break;
                        }
                    case 2:
                        {
                            Linex.X1 = x + a;
                            Linex.Y1 = y;
                            Linex.X2 = x;
                            Linex.Y2 = y + a;
                            break;
                        }
                    case 3:
                        {
                            Linex.X1 = x;
                            Linex.Y1 = y + a;
                            Linex.X2 = x - a;
                            Linex.Y2 = y;
                            break;
                        }
                }

                Linex.HorizontalAlignment = HorizontalAlignment.Left;
                Linex.VerticalAlignment = VerticalAlignment.Top;
                Linex.StrokeThickness = v;
                grid.Children.Insert(0, Linex);

                el = new Ellipse();
                el.Effect = WPFSegedtest.SajatDropShadowEffect;
                el.Height = el.Width = Linex.StrokeThickness * 2.6;
                el.Margin = new Thickness(Linex.X1 - el.Width / 2, Linex.Y1 - el.Height / 2, 0, 0);
                el.Fill = szin;
                el.HorizontalAlignment = HorizontalAlignment.Left;
                el.VerticalAlignment = VerticalAlignment.Top;
                el.StrokeThickness = 0;
                grid.Children.Add(el);

                //el = new Ellipse();
                //el.Height = el.Width = Linex.StrokeThickness * 2.6;
                //el.Margin = new Thickness(Linex.X2 - el.Width / 2, Linex.Y2 - el.Height / 2, 0, 0);
                //el.Fill = Brushes.Red;
                //el.HorizontalAlignment = HorizontalAlignment.Left;
                //el.VerticalAlignment = VerticalAlignment.Top;
                //el.StrokeThickness = 0;
                //grid.Children.Add(el);
            }

            el = new Ellipse();
            el.Effect = WPFSegedtest.SajatDropShadowEffect;
            el.Height = el.Width = a / 1.15;
            el.Margin = new Thickness(x - el.Width / 2, y - el.Height / 2, 0, 0);
            el.Fill = Brushes.DarkOrange;
            el.HorizontalAlignment = HorizontalAlignment.Left;
            el.VerticalAlignment = VerticalAlignment.Top;
            el.StrokeThickness = 0;
            grid.Children.Add(el);
            //cvsMain.Children.Add(el);


            AlapGrid.Children.Add(grid);

            return grid;
        }

        int szamlalo = 0;
        Point RelPozGridhez = new Point(0, 0);
        private Grid _currentControl;
        private void AlapGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var result = VisualTreeHelper.HitTest(AlapGrid, e.GetPosition(AlapGrid));
            if (result != null && VisualTreeHelper.GetParent(result.VisualHit) is Grid && VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(result.VisualHit)) is Grid)
            {
                Grid sajatgrid = VisualTreeHelper.GetParent(result.VisualHit) as Grid;
                RelPozGridhez = e.GetPosition(sajatgrid);
                //RelPozGridhez = new Point(((Shape)result.VisualHit).Margin.Left,((Shape)result.VisualHit).Margin.Top);
                _currentControl = sajatgrid;
            }
        }
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out System.Drawing.Point lpPoint);
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OpenHardwareMonitor.Program.FoAblak.TutorWPFMegjelenitve = false;
        }

        private void AlapGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_currentControl != null)
            {
                if (Mouse.LeftButton == MouseButtonState.Released)
                {
                    _currentControl = null;
                }
            }
        }

        private void AlapGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (_currentControl != null)
            {
                if (Mouse.LeftButton == MouseButtonState.Released)
                {
                    _currentControl = null;
                }
            }
        }
    }
    public class TextPath : Shape
    {
        private Geometry _textGeometry;

        #region Dependency Properties
        public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(TextPath),
                                                     new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily,
                                                             FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits,
                                                             OnPropertyChanged));
        [Bindable(true), Category("Appearance")]
        [Localizability(LocalizationCategory.Font)]
        [TypeConverter(typeof(FontFamilyConverter))]
        public FontFamily FontFamily { get { return (FontFamily)GetValue(FontFamilyProperty); } set { SetValue(FontFamilyProperty, value); } }

        public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(TextPath),
                                                        new FrameworkPropertyMetadata(SystemFonts.MessageFontSize,
                                                             FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                                                             OnPropertyChanged));
        [Bindable(true), Category("Appearance")]
        [TypeConverter(typeof(FontSizeConverter))]
        [Localizability(LocalizationCategory.None)]
        public double FontSize { get { return (double)GetValue(FontSizeProperty); } set { SetValue(FontSizeProperty, value); } }

        public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof(TextPath),
                                                     new FrameworkPropertyMetadata(TextElement.FontStretchProperty.DefaultMetadata.DefaultValue,
                                                             FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits,
                                                             OnPropertyChanged));
        [Bindable(true), Category("Appearance")]
        [TypeConverter(typeof(FontStretchConverter))]
        public FontStretch FontStretch { get { return (FontStretch)GetValue(FontStretchProperty); } set { SetValue(FontStretchProperty, value); } }

        public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(TextPath),
                                                     new FrameworkPropertyMetadata(SystemFonts.MessageFontStyle,
                                                             FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits,
                                                             OnPropertyChanged));
        [Bindable(true), Category("Appearance")]
        [TypeConverter(typeof(FontStyleConverter))]
        public FontStyle FontStyle { get { return (FontStyle)GetValue(FontStyleProperty); } set { SetValue(FontStyleProperty, value); } }

        public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(TextPath),
                                                     new FrameworkPropertyMetadata(SystemFonts.MessageFontWeight,
                                                             FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits,
                                                             OnPropertyChanged));
        [Bindable(true), Category("Appearance")]
        [TypeConverter(typeof(FontWeightConverter))]
        public FontWeight FontWeight { get { return (FontWeight)GetValue(FontWeightProperty); } set { SetValue(FontWeightProperty, value); } }

        public static readonly DependencyProperty OriginPointProperty =
                                        DependencyProperty.Register("Origin", typeof(Point), typeof(TextPath),
                                                new FrameworkPropertyMetadata(new Point(0, 0),
                                                        FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                                                             OnPropertyChanged));
        [Bindable(true), Category("Appearance")]
        [TypeConverter(typeof(PointConverter))]
        public Point Origin { get { return (Point)GetValue(OriginPointProperty); } set { SetValue(OriginPointProperty, value); } }

        public static readonly DependencyProperty TextProperty =
                                        DependencyProperty.Register("Text", typeof(string), typeof(TextPath),
                                                new FrameworkPropertyMetadata(string.Empty,
                                                        FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                                                             OnPropertyChanged));
        [Bindable(true), Category("Appearance")]
        public string Text { get { return (string)GetValue(TextProperty); } set { SetValue(TextProperty, value); } }
        #endregion

        protected override Geometry DefiningGeometry => _textGeometry ?? Geometry.Empty;

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextPath)d).CreateTextGeometry();

        private void CreateTextGeometry()
        {
            var formattedText = new FormattedText(Text, Thread.CurrentThread.CurrentUICulture, FlowDirection.LeftToRight,
                                    new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Brushes.Black);
            _textGeometry = formattedText.BuildGeometry(Origin);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (_textGeometry == null) CreateTextGeometry();
            if (_textGeometry.Bounds == Rect.Empty)
                return new Size(0, 0);
            // return the desired size
            return new Size(Math.Min(availableSize.Width, _textGeometry.Bounds.Width), Math.Min(availableSize.Height, _textGeometry.Bounds.Height));
        }
    }
}
