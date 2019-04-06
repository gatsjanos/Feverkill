using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OpenHardwareMonitor
{
    public class PIDErtekek
    {
        public double integral, hiba;
        public int hossz;
        public bool integralando;
    }
    public class PIDKezelo
    {
        public PIDKezelo(double setpoint, double P, double I, double D, int beleszamitasIntegralVisszamenolegms, int beleszamitasDerivaltVisszamenolegms)
        {
            Setpoint = setpoint;
            Kp = P;
            Ki = I;
            Kd = D;
            BeleszamitasIntegralVisszamenolegms = beleszamitasIntegralVisszamenolegms;
            BeleszamitasDerivaltVisszamenolegms = beleszamitasDerivaltVisszamenolegms;
        }
        public PIDKezelo()
        {
        }

        public double Setpoint = 250;
        public double Kp = 0, Ki = 0, Kd = 0; // PID constant multipliers
        public double error = 0;   // how much SP and PV are diff (SP - PV)
        public double integral = 0; // curIntegral + (error * Delta Time)
        public double derivative = 0;  //(error - prev error) / Delta time
        public double preError = 0; // error from last time (previous Error)
        public double Output = 0; // the drive amount that effects the PV.

        public int BeleszamitasIntegralVisszamenolegms = 15000;
        public int BeleszamitasDerivaltVisszamenolegms = 10000;
        public List<PIDErtekek> erteknaplo = new List<PIDErtekek>();

        double SajatVisszamenoleges = 0;
        //Dt: időintervallum, PV: a mért érték
        public double GetKovetkFordszam(double Dt, float PV)
        {
            /*
             * Pseudocode from Wikipedia
             * 
                previous_error = 0
                integral = 0 
            start:
                error = setpoint - PV(actual_position)
                integral = integral + error*dt
                derivative = (error - previous_error)/dt
                output = Kp*error + Ki*integral + Kd*derivative
                previous_error = error
                wait(dt)
                goto start
             */
            // calculate the difference between the desired value and the actual value
            error = PV - Setpoint;//Hűtés => meg van fordítva
            // track error over time, scaled to the timer interval

            double aktintegral = 0;

            if ((preError >= 0 && error >= 0) || (preError < 0 && error < 0))
            {
                if (preError > error)
                    aktintegral = ((error * Dt) + (Dt * (error - preError) / 2));
                else
                    aktintegral = ((error * Dt) - (Dt * (error - preError) / 2));
            }
            else if (error != preError)
            {
                double arany = Math.Abs(preError / (preError + (-error)));

                double elso = arany * Dt * preError / 2;
                double masodik = (1 - arany) * Dt * error / 2;

                aktintegral = elso + masodik;
            }

            erteknaplo.Add(new PIDErtekek { integral = aktintegral, hossz = (int)Dt, hiba = error, integralando = true });

            bool elsofelulvan = aktintegral >= 0;
            bool kikelllepni = false;

            int hosszosszeg = 0;
            integral = 0;
            for (int i = erteknaplo.Count - 1; i >= 0; --i)//A legutolsó setpointon való áthaladásig összegzi visszafele
            {
                if (!erteknaplo[i].integralando)
                    break;

                if ((erteknaplo[i].integral < 0 && elsofelulvan) || (erteknaplo[i].integral >= 0 && !elsofelulvan))
                    kikelllepni = true;

                integral += erteknaplo[i].integral;
                hosszosszeg += erteknaplo[i].hossz;

                if (hosszosszeg >= BeleszamitasIntegralVisszamenolegms + SajatVisszamenoleges && kikelllepni)
                    break;
            }
            SajatVisszamenoleges = hosszosszeg - BeleszamitasIntegralVisszamenolegms - Dt * 2;
            if (SajatVisszamenoleges < 0)
            {
                SajatVisszamenoleges = 0;
            }
            // determin the amount of change from the last time checked

            //double maxderivaltabs = 0;

            int dervivHosszIndex = erteknaplo.Count - 1;
            hosszosszeg = 0;
            for (; dervivHosszIndex >= 0; --dervivHosszIndex)//DERIVÁLANDÓ elemek kezdetének megállapítása
            {
                hosszosszeg += erteknaplo[dervivHosszIndex].hossz;
                if (hosszosszeg > BeleszamitasDerivaltVisszamenolegms)
                    break;
            }
            if (dervivHosszIndex < 0)
                dervivHosszIndex = 0;

            double b;
            LinRegresszio(erteknaplo, dervivHosszIndex, out derivative, out b);

            //derivative = (error - preError) / Dt;

            if (erteknaplo.Count >= 40000)
            {
                erteknaplo.RemoveAt(0);
                erteknaplo.RemoveAt(0);
            }

            // calculate how much drive the output in order to get to the 
            // desired setpoint. 

            double szamolt = (Kp * error) + (Ki * integral) + (Kd * derivative);

            if (Math.Abs(Ki * integral) > 100)//Ez a ciklus nem hagyja "nagyságrendileg elszaladni" az integrált
            {
                if (Math.Abs((Ki * integral) / (Kd * derivative)) > 15)
                {
                    if (Math.Abs((Ki * integral) / (Kp * error)) > 15)
                    {
                        integral = 0;

                        int szazalekkompenzalo = 0;
                        if (Math.Sign(derivative) != Math.Sign(integral) && Math.Sign(derivative) != 0)
                        {
                            szazalekkompenzalo += 7;// 7 = 100/15
                        }
                        if (Math.Sign(error) != Math.Sign(integral) && Math.Sign(error) != 0)
                        {
                            szazalekkompenzalo += 7;// 7 = 100/15
                        }

                        for (int i = erteknaplo.Count - 1; i >= 0; --i)
                        {
                            integral += erteknaplo[i].integral;
                            if (Math.Abs(integral) * Ki > 100 + szazalekkompenzalo)
                            {
                                //erteknaplo.RemoveRange(0, i + 1);
                                for (int x = 0; x < i + 1; x++)
                                {
                                    erteknaplo[x].integralando = false;
                                }
                                break;
                            }
                        }
                    }
                }
            }

            if (szamolt < 0)
                szamolt = 0;
            else if (szamolt > 100)
                szamolt = 100;

            Output = szamolt;

            // remember the error for the next time around.
            preError = error;

            return Output;
        }
        public void LinRegresszio(List<PIDErtekek> meresAdatsor, int belevettKezdoindex, out double m, out double b)
        {
            try
            {
                int hossz = meresAdatsor.Count;//a rövidebb lista hosszáig fog számolni

                double[] XadatsorDC = new double[hossz - belevettKezdoindex];
                double[] YadatsorDC = new double[hossz - belevettKezdoindex];

                int hosszosszeg = 0;
                for (int i = belevettKezdoindex; i < hossz; i++)//DeepCopy
                {
                    hosszosszeg += meresAdatsor[i].hossz;
                    XadatsorDC[i - belevettKezdoindex] = (double)hosszosszeg;
                    YadatsorDC[i - belevettKezdoindex] = (double)meresAdatsor[i].hiba;
                }


                Tuple<double, double> p = Fit.Line(XadatsorDC, YadatsorDC);
                b = p.Item1; // == 10; intercept
                m = p.Item2; // == 0.5; slope
            }
            catch (ArgumentException)//Ha kevesebb, mint 2 pont van megadva
            {
                m = b = 0;
            }
        }
    }
}
