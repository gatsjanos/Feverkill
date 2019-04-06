using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FeverkillSupervisor
{
    class Program
    {
        //[DllImport("kernel32.dll")]
        //static extern IntPtr GetConsoleWindow();

        //[DllImport("user32.dll")]
        //static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        //const int SW_HIDE = 0;
        //const int SW_SHOW = 5;

        static void Main(string[] args)
        {
            //var handle = GetConsoleWindow();
            //ShowWindow(handle, SW_HIDE);

            Console.WriteLine("ARGUMENTS IN: ");

            string argstring = "", csakelsoargumentum = "";
            foreach (var item in args)
            {
                Console.WriteLine(item);

                if (item == "AUTOSTART")
                    csakelsoargumentum += item + " ";
                else
                    argstring += item + " ";
            }
            argstring += "SUPERVISOR ";

            Console.WriteLine();
            Console.WriteLine("Lined arguments OUT: " + argstring + csakelsoargumentum);
            Console.WriteLine();


            Process p = new Process();
            p.EnableRaisingEvents = true;

            p.StartInfo = new ProcessStartInfo("Feverkill.exe", argstring + csakelsoargumentum);

            int ecode = 0;
            while (ecode != 19981001)
            {
                Console.WriteLine("Running a new instance of Feverkill.exe");
                p.Start();
                p.WaitForExit();
                ecode = p.ExitCode;

                Console.WriteLine("Instance exited. ExitCode: " + ecode);
                Console.WriteLine();

                p.StartInfo = new ProcessStartInfo("Feverkill.exe", argstring);
            }

            Console.WriteLine("Supervisor Exiting...");
        }
    }
}
