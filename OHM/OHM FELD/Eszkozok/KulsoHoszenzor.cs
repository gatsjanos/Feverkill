using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace OpenHardwareMonitor
{
    class KulsoHoszenzor
    {

        static SerialPort SorosPort;
        static bool elsovissza = false, masodikvissza = false, rendben = true, sikeresport = false;
        public static void Kapcsolodas()
        {
            #region KEZFOGAS A VEZERLOVEL
            try { SorosPort.Close(); } catch { }
            Thread.Sleep(10);
            SorosPort = new SerialPort(".", 9600, Parity.None, 8, StopBits.One);
            SorosPort.Encoding = System.Text.Encoding.GetEncoding(28591); //8 bites karakterek
            elsovissza = false; masodikvissza = false; rendben = true; sikeresport = false;

            SorosPort.DataReceived -= COM_AdatFogad;
            string[] portok = SerialPort.GetPortNames();

            //Masodlagos hitelesítési kulcs
            string MasHitKulcs = "ArduinoHoszenzor";


            Stopwatch sw = new Stopwatch();
            for (int k = 0; k < 3 && !sikeresport; k++)
            {
                for (int i = 0; i < portok.Length && !sikeresport; ++i)
                {
                    try
                    {
                        SorosPort.DataReceived += COM_Kezfogas;

                        rendben = true;
                        sw.Reset();
                        SorosPort.PortName = portok[i];
                        SorosPort.Open();

                        byte[] b = new byte[] { 67 };
                        SorosPort.Write(b, 0, 1);
                        SorosPort.WriteLine("");
                        Thread.Sleep(50);
                        sw.Start();
                        while (sw.ElapsedMilliseconds < 300 && rendben)
                        {
                            if (elsovissza && masodikvissza)
                            {
                                SorosPort.DataReceived -= COM_Kezfogas;
                                Thread.Sleep(500);
                                if (SorosPort.ReadExisting() == MasHitKulcs)
                                {
                                    sikeresport = true;
                                    break;
                                }
                            }
                        }
                        sw.Stop();
                        if (!sikeresport)
                            SorosPort.Close();
                        elsovissza = masodikvissza = false;
                    }
                    catch
                    { }
                }
            }

            SorosPort.DataReceived -= COM_Kezfogas;
            SorosPort.DataReceived += COM_AdatFogad;


            //if (!sikeresport)
            //    MessageBox.Show("Arduino nem található!", "Arduino", MessageBoxButton.OK, MessageBoxImage.Error);
            
            #endregion

        }
        public static List<Program.HoMers> HomListAd()
        {
            List<Program.HoMers> ListVissza = new List<Program.HoMers>();
            Stopwatch sw = new Stopwatch();
            Fogadott = "";
            SorfogadasKesz = false;

            for (int x = 0; x < 2; x++)
            {
                try
                {

                    for (int i = 0; i < 3; ++i)
                    {

                        Thread.Sleep(1000);
                        SorosPort.Write(new byte[] { 99 }, 0, 1);
                        SorosPort.WriteLine("");


                        sw.Restart();
                        while (true)
                        {
                            Thread.Sleep(100);
                            if (SorfogadasKesz)
                            {
                                if (Fogadott != "")
                                    i = 100;

                                break;
                            }
                            if (sw.ElapsedMilliseconds > 3000)
                            {
                                Fogadott = "";
                                SorfogadasKesz = false;
                                break;
                            }
                        }

                    }
                    //Fogadott = "===\n------:0:25.19\n-------:1:24.87";
                    string[] sorok = Fogadott.Split('\n');
                    //MessageBox.Show(Fogadott);

                    for (int i = 1; i < sorok.Length - 1; i++)
                    {
                        Program.HoMers buff;
                        string[] tagok = sorok[i].Split(':');

                        buff.Csop = "External Test Sensors";
                        buff.Nev = "Sensor #" + (int.Parse(tagok[1]) + 1);
                        buff.Ertek = tagok[2] + " °C";

                        ListVissza.Add(buff);
                    }
                    //MessageBox.Show(Fogadott);
                    return ListVissza;
                }
                catch { Kapcsolodas(); }
            }
            return new List<Program.HoMers>();
        }
        static string Fogadott = "";
        static bool SorfogadasKesz = false;
        static void COM_AdatFogad(object sender, SerialDataReceivedEventArgs e)
        {
            //SerialPort sp = (SerialPort)sender;
            int hanysortolvas = 3, hanyadikmost = 0;
            try
            {
                while (!SorfogadasKesz)
                {
                    char b = (char)SorosPort.ReadByte();
                    Fogadott += b;
                    if (b == '\n')
                    {
                        ++hanyadikmost;
                        if (hanyadikmost >= hanysortolvas)
                        {
                            SorfogadasKesz = true;
                            break;
                        }
                    }
                }

            }
            catch (IndexOutOfRangeException) { }
        }
        static void COM_Kezfogas(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;

            try
            {
                if (!elsovissza)
                {
                    byte b = Convert.ToByte(sp.ReadExisting()[0]);
                    if (b == 80)
                        elsovissza = true;
                    else
                    {
                        elsovissza = masodikvissza = rendben = false;
                    }
                }
                else if (!masodikvissza)
                {
                    if (sp.ReadByte() == 100)
                        masodikvissza = true;
                    else
                    {
                        elsovissza = masodikvissza = rendben = false;
                    }
                }

            }
            catch (IndexOutOfRangeException) { }
            catch { }
        }
    }
}
