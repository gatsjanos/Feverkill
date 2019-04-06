using OpenHardwareMonitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace UdvozloKepernyo
{
    public class WPFSegedtest
    {
        public WPFSegedtest(UdvozloKepernyo.TutorWPFAblak MWbe)
        {
            MW = MWbe;
        }
        TutorWPFAblak MW;

        public bool Lathato = false;
        public Point? KezdEltolas = null;
        public bool ElsoKirajzolas = true;

        public Brush Szin = Brushes.Red;
        public Brush SzinPillanatnyi = Brushes.Red;
        public bool Villogas = false;

        public System.Windows.Controls.Grid grid;

        public System.Windows.Forms.Control FormsControl;
        public FrameworkElement WPFControl;

        public System.Windows.Shapes.Line OsszekotoVonal;
        public System.Windows.Shapes.Polygon OsszekotoHegy;

        public double FogX = 100, FogY = 100, FogA = 25, FogV = 4;
        public static DropShadowEffect SajatDropShadowEffect = new DropShadowEffect() { BlurRadius = 6, Color = Colors.Black, Direction = 225, Opacity = 100, ShadowDepth = 2, RenderingBias = RenderingBias.Performance };
        
        public Point ControlKozepe;
        public void OsszekotoRajzol()
        {
            try
            {
                if (!Lathato)
                {
                    if (grid != null)
                        grid.Visibility = Visibility.Hidden;
                    if (OsszekotoVonal != null)
                        OsszekotoVonal.Visibility = Visibility.Hidden;
                    if (OsszekotoHegy != null)
                        OsszekotoHegy.Visibility = Visibility.Hidden;
                    return;
                }
                else
                {
                    if (grid != null)
                        grid.Visibility = Visibility.Visible;
                    if (OsszekotoVonal != null)
                        OsszekotoVonal.Visibility = Visibility.Visible;
                    if (OsszekotoHegy != null)
                        OsszekotoHegy.Visibility = Visibility.Visible;
                }

                PresentationSource source = null;
                Point eredetihely = new Point(0, 0);
                Point targetPoints = new Point(0, 0);
                double controlwidth = 0, controlheight = 0;

                if (WPFControl != null)
                {
                    WPFControl.Dispatcher.Invoke(delegate ()
                    {
                        source = PresentationSource.FromVisual(Window.GetWindow(WPFControl));
                        eredetihely = WPFControl.PointToScreen(new Point(0, 0));
                        controlwidth = WPFControl.Width;
                        controlheight = WPFControl.Height;
                        targetPoints = source.CompositionTarget.TransformFromDevice.Transform(eredetihely);
                    });

                    bool merve = false;
                    if (Double.IsNaN(controlwidth))
                    {
                        WPFControl.Dispatcher.Invoke(delegate ()
                        {
                            merve = true;
                            WPFControl.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                            controlwidth = WPFControl.DesiredSize.Width;
                        });
                    }
                    if (Double.IsNaN(controlheight))
                    {
                        WPFControl.Dispatcher.Invoke(delegate ()
                        {
                            if (!merve)
                                WPFControl.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                            controlheight = WPFControl.DesiredSize.Height;
                        });
                    }
                }
                else if (FormsControl != null)
                {
                    bool FormsControlAsyncInvokeKesz = false;
                    FormsControl.BeginInvoke((Action)delegate ()
                        {
                            var buff = FormsControl.PointToScreen(new System.Drawing.Point(0, 0));
                            targetPoints = new Point(buff.X, buff.Y);
                            controlwidth = FormsControl.Width;
                            controlheight = FormsControl.Height;
                            FormsControlAsyncInvokeKesz = true;
                        });
                    for (int i = 0; i < 150 && !FormsControlAsyncInvokeKesz; ++i)
                    {
                        Thread.Sleep(1);
                    }

                    if(!FormsControlAsyncInvokeKesz)
                    {
                        return;
                    }
                }
                else
                    return;


                if(MW.HintSzinVillanas && Villogas)
                {
                    SzinPillanatnyi = Brushes.DarkOrange;
                }
                else
                {
                    SzinPillanatnyi = Szin;
                }


                ControlKozepe = new Point(MW.AlapGrid.PointFromScreen(targetPoints).X + controlwidth / 2, MW.AlapGrid.PointFromScreen(targetPoints).Y + controlheight / 2);

                if (ElsoKirajzolas && KezdEltolas != null)
                {
                    double Xtrim = 0, Ytrim = 0;

                    if (ControlKozepe.X + KezdEltolas.Value.X < 30)
                    {
                        Xtrim = 30 - (ControlKozepe.X + KezdEltolas.Value.X);
                    }
                    if (ControlKozepe.Y + KezdEltolas.Value.Y < 30)
                    {
                        Ytrim = 30 - (ControlKozepe.Y + KezdEltolas.Value.Y);
                    }

                    if (Program.TutorialWPFAblak != null)
                    {
                        if (ControlKozepe.X + KezdEltolas.Value.X > Program.TutorialWPFAblak.Width - 250)
                        {
                            Xtrim = (Program.TutorialWPFAblak.Width - 250) - (ControlKozepe.X + KezdEltolas.Value.X);
                        }
                        if (ControlKozepe.Y + KezdEltolas.Value.Y > Program.TutorialWPFAblak.Height - 100)
                        {
                            Ytrim = (Program.TutorialWPFAblak.Height - 100) - (ControlKozepe.Y + KezdEltolas.Value.Y);
                        }
                    }
                    grid.Margin = new Thickness(ControlKozepe.X + KezdEltolas.Value.X + Xtrim, ControlKozepe.Y + KezdEltolas.Value.Y + Ytrim, 0, 0);
                    ElsoKirajzolas = false;
                }

                Point kezdoHely = new Point(0, 0);
                double mintavolsag = Double.MaxValue;
                double tavolsag = 0;
                for (int i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0:
                            {
                                tavolsag = Tavolsag(ControlKozepe, MW.AlapGrid.PointFromScreen(grid.PointToScreen(new Point(FogX - FogA, FogY))));
                                if (mintavolsag > tavolsag)
                                {
                                    kezdoHely = new Point(FogX - FogA, FogY);
                                    mintavolsag = tavolsag;
                                }
                                break;
                            }
                        case 1:
                            {
                                tavolsag = Tavolsag(ControlKozepe, MW.AlapGrid.PointFromScreen(grid.PointToScreen(new Point(FogX, FogY - FogA))));
                                if (mintavolsag > tavolsag)
                                {
                                    kezdoHely = new Point(FogX, FogY - FogA);
                                    mintavolsag = tavolsag;
                                }
                                break;
                            }
                        case 2:
                            {
                                tavolsag = Tavolsag(ControlKozepe, MW.AlapGrid.PointFromScreen(grid.PointToScreen(new Point(FogX + FogA, FogY))));
                                if (mintavolsag > tavolsag)
                                {
                                    kezdoHely = new Point(FogX + FogA, FogY);
                                    mintavolsag = tavolsag;
                                }
                                break;
                            }
                        case 3:
                            {
                                tavolsag = Tavolsag(ControlKozepe, MW.AlapGrid.PointFromScreen(grid.PointToScreen(new Point(FogX, FogY + FogA))));
                                if (mintavolsag > tavolsag)
                                {
                                    kezdoHely = new Point(FogX, FogY + FogA);
                                    mintavolsag = tavolsag;
                                }
                                break;
                            }
                    }
                }

                kezdoHely = MW.AlapGrid.PointFromScreen(grid.PointToScreen(kezdoHely));

                Point vect = new Point(kezdoHely.X - ControlKozepe.X, kezdoHely.Y - ControlKozepe.Y);

                bool forditott = false;
                if (vect.X == 0 || Math.Abs(vect.Y / vect.X) > 2)
                {
                    forditott = true;
                    vect = new Point(vect.Y, vect.X);

                    double buff = controlwidth;
                    controlwidth = controlheight;
                    controlheight = buff;
                }
                double m = vect.Y / vect.X;
                //y = m*x
                //x = y/m

                double y = 0;
                int x = 0;
                for (x = 0; !(controlwidth / 2 < Math.Abs(x) || controlheight / 2 < Math.Abs(y)); ++x)
                {
                    y = x * m;
                }

                double c = Math.Sqrt(x * x + y * y) + 13;//+13: távolság a nyílhegy és a vonal várt vége között
                double xline = c / (Math.Sqrt(m * m + 1));
                double yline = m * xline;
                //for (xline = x; !(controlwidth / 2 + 10 < Math.Abs(xline) || controlheight / 2 + 10< Math.Abs(yline)); ++xline)
                //{
                //    yline = xline * m;
                //}

                if (vect.X < 0)
                {
                    x = -x;
                    y = -y;
                    xline = -xline;
                    yline = -yline;
                }

                try
                {
                    int yi = Convert.ToInt32(y);
                    int yiline = Convert.ToInt32(yline);
                    int xiline = Convert.ToInt32(xline);

                    if (forditott)
                    {
                        //vect = new Point(vect.Y, vect.X);

                        int buff = x;
                        x = yi;
                        yi = buff;

                        buff = xiline;
                        xiline = yiline;
                        yiline = buff;
                    }

                    if (OsszekotoHegy == null)
                    {
                        //m - mv = mn
                        //m + mv = mn

                        OsszekotoHegy = new Polygon();
                        OsszekotoHegy.Effect = SajatDropShadowEffect;
                        OsszekotoHegy.StrokeThickness = 0;
                        OsszekotoHegy.Fill = SzinPillanatnyi;
                        OsszekotoHegy.HorizontalAlignment = HorizontalAlignment.Left;
                        OsszekotoHegy.VerticalAlignment = VerticalAlignment.Top;

                        OsszekotoHegy.Points.Add(new Point(0, 0));
                        OsszekotoHegy.Points.Add(new Point(-13, -30));
                        OsszekotoHegy.Points.Add(new Point(13, -30));
                        OsszekotoHegy.RenderTransformOrigin = new Point(0, 1);

                        TransformGroup tfGroup = new TransformGroup();

                        if (forditott)
                            tfGroup.Children.Add(new RotateTransform(270 - (Math.Atan(-1 / m) * 180 / Math.PI) + ((vect.Y >= 0) ? 180 : 0)));
                        else
                            tfGroup.Children.Add(new RotateTransform((Math.Atan(-1 / m) * 180 / Math.PI) + ((vect.Y >= 0) ? 180 : 0)));
                        tfGroup.Children.Add(new TranslateTransform(ControlKozepe.X + x, ControlKozepe.Y + yi));
                        OsszekotoHegy.RenderTransform = tfGroup;

                        MW.AlapGrid.Children.Insert(0, OsszekotoHegy);
                    }
                    else
                    {
                        OsszekotoHegy.Fill = SzinPillanatnyi;

                        TransformGroup tfGroup = new TransformGroup();

                        if (forditott)
                            tfGroup.Children.Add(new RotateTransform(270 - (Math.Atan(-1 / m) * 180 / Math.PI) + ((vect.Y >= 0) ? 180 : 0)));
                        else
                            tfGroup.Children.Add(new RotateTransform((Math.Atan(-1 / m) * 180 / Math.PI) + ((vect.Y >= 0) ? 180 : 0)));
                        tfGroup.Children.Add(new TranslateTransform(ControlKozepe.X + x, ControlKozepe.Y + yi));
                        OsszekotoHegy.RenderTransform = tfGroup;
                    }

                    if (OsszekotoVonal == null)
                    {
                        OsszekotoVonal = new Line();
                        OsszekotoVonal.Effect = SajatDropShadowEffect;
                        OsszekotoVonal.Stroke = SzinPillanatnyi;
                        OsszekotoVonal.X1 = kezdoHely.X;
                        OsszekotoVonal.Y1 = kezdoHely.Y;
                        OsszekotoVonal.X2 = ControlKozepe.X + xiline;
                        OsszekotoVonal.Y2 = ControlKozepe.Y + yiline;
                        OsszekotoVonal.HorizontalAlignment = HorizontalAlignment.Left;
                        OsszekotoVonal.VerticalAlignment = VerticalAlignment.Top;
                        OsszekotoVonal.StrokeThickness = FogV;
                        MW.AlapGrid.Children.Insert(0, OsszekotoVonal);
                    }
                    else
                    {
                        OsszekotoVonal.Stroke = SzinPillanatnyi;
                        OsszekotoVonal.X1 = kezdoHely.X;
                        OsszekotoVonal.Y1 = kezdoHely.Y;
                        OsszekotoVonal.X2 = ControlKozepe.X + xiline;
                        OsszekotoVonal.Y2 = ControlKozepe.Y + yiline;
                    }

                }
                catch (OverflowException)
                { }
            }
            catch { }
        }

        private double Tavolsag(Point a, Point b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }
    }
}
