using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Threading;
using System.ComponentModel;
namespace ConsoleApplication1
{
    class Program
    {
        [DllImport("kernel32.dll")]
        public static extern SafeWaitHandle CreateWaitableTimer(IntPtr lpTimerAttributes, bool bManualReset, string lpTimerName);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWaitableTimer(SafeWaitHandle hTimer, [In] ref long pDueTime, int lPeriod, IntPtr pfnCompletionRoutine, IntPtr lpArgToCompletionRoutine, bool fResume);

        static void Main(string[] args)
        {
            if(args.Length != 0)
                try
                {
                    if (int.Parse(args[0]) > 0)
                    {
                        SetWaitForWakeUpTime(Convert.ToDouble(args[0]));
                    }
                }
                catch {}
        }

        static void SetWaitForWakeUpTime(double varidoPERC)
        {
            DateTime utc = DateTime.Now.AddMinutes(varidoPERC);
            long duetime = utc.ToFileTime();

            using (SafeWaitHandle handle = CreateWaitableTimer(IntPtr.Zero, true, "MyWaitabletimer"))
            {
                if (SetWaitableTimer(handle, ref duetime, 0, IntPtr.Zero, IntPtr.Zero, true))
                {
                    using (EventWaitHandle wh = new EventWaitHandle(false, EventResetMode.AutoReset))
                    {
                        wh.SafeWaitHandle = handle;
                        wh.WaitOne();
                    }
                }
                else
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            
            //Console.WriteLine("Wake up call");
            //Console.ReadLine();
        }
    }
}