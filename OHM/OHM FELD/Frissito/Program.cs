using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Frissito
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Title = "Feverkill Updater";
                Console.CursorVisible = false;
                Console.ForegroundColor = ConsoleColor.White;


                int hanyPeldanyFut = 9999;
                for (int i = 0; i < 1300 / 50 && hanyPeldanyFut != 0; ++i)
                {
                    hanyPeldanyFut = HanyPeldanyFut(Directory.GetCurrentDirectory(), "FeverkillSupervisor.exe");
                    Console.WriteLine("Is running >>test - Supervisor: " + i + " ::: " + hanyPeldanyFut + "ins");
                    Thread.Sleep(50);
                }

                if (hanyPeldanyFut != 0)
                {
                    for (int i = 0; i < 4 && hanyPeldanyFut != 0; ++i)
                    {
                        Process[] TobbiExe = Process.GetProcessesByName("FeverkillSupervisor");

                        foreach (Process item in TobbiExe)
                        {
                            try
                            {

                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write("Killing");
                                Console.ForegroundColor = ConsoleColor.White;
                                item.Kill();
                                Console.WriteLine(" - Done");
                            }
                            catch (Exception e)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(" - Failed");
                                Console.WriteLine("ERROR: " + e.Message);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }

                        hanyPeldanyFut = HanyPeldanyFut(Directory.GetCurrentDirectory(), "FeverkillSupervisor.exe");
                    }
                    Thread.Sleep(200);
                }

                hanyPeldanyFut = 9999;
                for (int i = 0; i < 5000 / 50 && hanyPeldanyFut != 0; ++i)
                {
                    hanyPeldanyFut = HanyPeldanyFut(Directory.GetCurrentDirectory(), "Szelcsend.exe");
                    Console.WriteLine("Is running >>test - Old: " + i + " ::: " + hanyPeldanyFut + "ins");
                    Thread.Sleep(50);
                }

                if (hanyPeldanyFut != 0)
                {
                    for (int i = 0; i < 4 && hanyPeldanyFut != 0; ++i)
                    {
                        Process[] TobbiExe = Process.GetProcessesByName("Szelcsend");

                        foreach (Process item in TobbiExe)
                        {
                            try
                            {

                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write("Killing");
                                Console.ForegroundColor = ConsoleColor.White;
                                item.Kill();
                                Console.WriteLine(" - Done");
                            }
                            catch (Exception e)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(" - Failed");
                                Console.WriteLine("ERROR: " + e.Message);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }

                        hanyPeldanyFut = HanyPeldanyFut(Directory.GetCurrentDirectory(), "Szelcsend.exe");
                    }
                    Thread.Sleep(200);
                }

                hanyPeldanyFut = 9999;
                for (int i = 0; i < 5000 / 50 && hanyPeldanyFut != 0; ++i)
                {
                    hanyPeldanyFut = HanyPeldanyFut(Directory.GetCurrentDirectory(), "Feverkill.exe");
                    Console.WriteLine("Is running >>test - New: " + i + " ::: " + hanyPeldanyFut + "ins");
                    Thread.Sleep(50);
                }

                if (hanyPeldanyFut != 0)
                {
                    for (int i = 0; i < 4 && hanyPeldanyFut != 0; ++i)
                    {
                        Process[] TobbiExe = Process.GetProcessesByName("Feverkill");

                        foreach (Process item in TobbiExe)
                        {
                            try
                            {

                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write("Killing");
                                Console.ForegroundColor = ConsoleColor.White;
                                item.Kill();
                                Console.WriteLine(" - Done");
                            }
                            catch (Exception e)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(" - Failed");
                                Console.WriteLine("ERROR: " + e.Message);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }

                        hanyPeldanyFut = HanyPeldanyFut(Directory.GetCurrentDirectory(), "Feverkill.exe");
                    }
                    Thread.Sleep(200);
                }

            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: " + e.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n =================================\n" +
                                "== Updater file-commands started ==\n" +
                                " =================================\n");
            Console.ForegroundColor = ConsoleColor.White;
            try
            {
                KonyvtarTeszt();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: " + e.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }

            ResourceFajlbairo("Aga.Controls.dll", ResourceFrissito.Aga_Controls, 5);
            ResourceFajlbairo("ebreszt.exe", ResourceFrissito.ebreszt, 5);
            ResourceFajlbairo("loc\\en.loc", ResourceFrissito.en, 5);
            ResourceFajlbairo("lic\\ModLicense.html", ResourceFrissito.ModLicense, 5);
            ResourceFajlbairo("OpenHardwareMonitorLib.dll", ResourceFrissito.OpenHardwareMonitorLib, 5);
            ResourceFajlbairo("OxyPlot.dll", ResourceFrissito.OxyPlot, 5);
            ResourceFajlbairo("OxyPlot.WindowsForms.dll", ResourceFrissito.OxyPlot_WindowsForms, 5);
            ResourceFajlbairo("Rsx\\riaszt.wav", ResourceFrissito.riaszt, 5);
            ResourceFajlbairo("Feverkill.exe", ResourceFrissito.Feverkill, 5);
            ResourceFajlbairo("UdvozloKepernyo.exe", ResourceFrissito.UdvozloKepernyo, 5);
            ResourceFajlbairo("MathNet.Numerics.dll", ResourceFrissito.MathNet_Numerics, 5);
            ResourceFajlbairo("en\\Feverkill.resources.dll", ResourceFrissito.Feverkill_resources, 5);
            ResourceFajlbairo("FeverkillSupervisor.exe", ResourceFrissito.FeverkillSupervisor, 5);


            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n =================================\n" +
                                "== Updater file-operations ended ==\n" +
                                " =================================\n");
            Console.ForegroundColor = ConsoleColor.White;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Starting FeverkillSupervisor.exe...");
            Console.ForegroundColor = ConsoleColor.White;

            try
            {
                bool talalt = false;
                string[] fajlok = Directory.GetFiles(Directory.GetCurrentDirectory());
                string[] buff;
                foreach (string item in fajlok)
                {
                    buff = item.Split('\\');
                    if(buff[buff.Length-1] == "Szelcsend.exe")
                    {
                        talalt = true;
                        break;
                    }
                }

                if (talalt)
                {

                    Console.WriteLine();
                    Console.WriteLine("Deprecated file(s) founded: \"Szelcsend.exe\"");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Disposing \"Szelcsend.exe\" ...");
                    Console.ForegroundColor = ConsoleColor.White;

                    try
                    {
                        File.Delete("Szelcsend.exe");

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Disposal succeeded");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("ERROR: " + e.Message);
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: " + e.Message);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            }

            try
            {
                Thread.Sleep(100);
                Process.Start("FeverkillSupervisor.exe", "FRISSITVE");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Update session ended");

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Finalizing: ");

                int toppos = Console.CursorTop, leftpos = Console.CursorLeft;
                for (int i = 5000; i > 0; i -= 20)
                {
                    if (i > 3700)
                        Console.ForegroundColor = ConsoleColor.Green;
                    else if (i > 1900)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else
                        Console.ForegroundColor = ConsoleColor.Red;

                    Console.Write((double)i / 1000);
                    Thread.Sleep(20);
                    Console.SetCursorPosition(leftpos, toppos);
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: " + e.Message);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("==============================================================");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Meglehet, hogy kézzel kell elindítania az alkalmazást.\nA frissítő bezárásához üssön ENTER-t!");
                Console.WriteLine();
                Console.WriteLine("You probably have to start the application manually.\nPress ENTER to close the updater!");

                Console.ReadLine();
            }

            //Console.ReadLine();
        }

        static void KonyvtarTeszt()
        {
            if (!Directory.Exists("loc"))
            {
                Directory.CreateDirectory("loc");
            }
            if (!Directory.Exists("lic"))
            {
                Directory.CreateDirectory("lic");
            }
            if (!Directory.Exists("Rsx"))
            {
                Directory.CreateDirectory("Rsx");
            }
            if (!Directory.Exists("en"))
            {
                Directory.CreateDirectory("en");
            }
        }
        static int HanyPeldanyFut(string mappa, string fajlnev)
        {
            int db = 0;

            var wmiQueryString = "SELECT ProcessId, ExecutablePath, CommandLine FROM Win32_Process";
            using (var searcher = new ManagementObjectSearcher(wmiQueryString))
            using (var results = searcher.Get())
            {
                var query = from p in Process.GetProcesses()
                            join mo in results.Cast<ManagementObject>()
                            on p.Id equals (int)(uint)mo["ProcessId"]
                            select new
                            {
                                Process = p,
                                Path = (string)mo["ExecutablePath"],
                                CommandLine = (string)mo["CommandLine"],
                            };
                foreach (var item in query)
                {
                    try
                    {
                        if (item.Path.StartsWith(mappa + "\\" + fajlnev))
                        {
                            ++db;
                        }
                    }
                    catch { }
                }
            }
            return db;
        }
        static void ResourceFajlbairo(string fajlnev, string irando, int maxProbalkozasSzam)
        {
            for (int i = 0; i < maxProbalkozasSzam && !ResourceFajlbairoEgyszerprobal(fajlnev, irando); ++i)
            { }
        }
        static bool ResourceFajlbairoEgyszerprobal(string fajlnev, string irando)
        {
            try
            {
                Console.WriteLine("WRITE: " + fajlnev + "\t\t\t\t| String");

                StreamWriter bw = new StreamWriter(new FileStream(fajlnev, FileMode.OpenOrCreate));
                bw.Write(irando);
                bw.Close();

                Console.WriteLine("Succeeded");
                return true;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: " + e.Message);
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
        }
        static void ResourceFajlbairo(string fajlnev, byte[] irando, int maxProbalkozasSzam)
        {
            for (int i = 0; i < maxProbalkozasSzam && !ResourceFajlbairoEgyszerprobal(fajlnev, irando); ++i)
            { }
        }
        static bool ResourceFajlbairoEgyszerprobal(string fajlnev, byte[] irando)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("WRITE: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(fajlnev + "\t\t\t\t| Byte[]");

                BinaryWriter bw = new BinaryWriter(new FileStream(fajlnev, FileMode.OpenOrCreate));
                bw.Write(irando);
                bw.Close();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Succeeded");
                Console.ForegroundColor = ConsoleColor.White;
                return true;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: " + e.Message);
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
        }
        static void ResourceFajlbairo(string fajlnev, Stream stream, int maxProbalkozasSzam)
        {
            for (int i = 0; i < maxProbalkozasSzam && !ResourceFajlbairoEgyszerprobal(fajlnev, stream); ++i)
            { }
        }
        static bool ResourceFajlbairoEgyszerprobal(string fajlnev, Stream stream)
        {
            try
            {
                Console.WriteLine("WRITE: " + fajlnev + "\t\t\t\t| Stream");

                byte[] buff = new byte[stream.Length];

                BinaryReader br = new BinaryReader(stream);
                br.Read(buff, 0, (int)stream.Length);
                br.Close();

                BinaryWriter bw = new BinaryWriter(new FileStream(fajlnev, FileMode.OpenOrCreate));
                bw.Write(buff);
                bw.Close();

                Console.WriteLine("Succeeded");
                return true;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: " + e.Message);
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
        }
    }
}
