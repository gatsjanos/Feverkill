using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace OpenHardwareMonitor
{
    static class Szalak
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenThread(uint dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport("kernel32.dll")]
        public static extern uint SuspendThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        public static extern uint ResumeThread(IntPtr hThread);

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        static extern bool TerminateThread(IntPtr hThread, uint dwExitCode);


        public static void OsszesSzalSzuneteltetHivotKiveve()
        {
            ProcessThreadCollection processThreads = Process.GetCurrentProcess().Threads;
            MessageBox.Show("=> Total threads: " + processThreads.Count);

            foreach (ProcessThread pt in processThreads)
            {
                IntPtr ptrThread = OpenThread(2, false, (uint)pt.Id);//2: hozzáférési szint: THREAD_SUSPEND_RESUME
                
                if (Thread.CurrentThread.ManagedThreadId != pt.Id)
                {
                    try
                    {
                        ResumeThread(ptrThread);
                        //TerminateThread(ptrThread, 1);
                    }
                    catch (Exception e)
                    {
                        Console.Out.WriteLine(e.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("SAJÁT");
                }


            }
            MessageBox.Show("=> Total threads now: " + Process.GetCurrentProcess().Threads.Count);
        }
    }
}
