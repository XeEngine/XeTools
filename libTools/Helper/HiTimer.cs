using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace libTools
{
    public static partial class Helper
    {
        public class HiTimer
        {
            [DllImport("Kernel32.dll")]
            private static extern bool QueryPerformanceCounter(
                out long lpPerformanceCount);
            [DllImport("Kernel32.dll")]
            private static extern bool QueryPerformanceFrequency(
                out long lpFrequency);
            [DllImport("Winmm.dll")]
            private static extern uint timeBeginPeriod(
                uint uPeriod);
            [DllImport("Winmm.dll")]
            private static extern uint timeEndPeriod(
                uint uPeriod);

            private long startTime;
            private long freq;

            // Constructor
            public HiTimer()
            {
                System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;
                startTime = 0;

                timeBeginPeriod(1);
                if (QueryPerformanceFrequency(out freq) == false)
                {
                    // high-performance counter not supported
                    throw new Win32Exception();
                }
                QueryPerformanceCounter(out startTime);
            }
            ~HiTimer()
            {
                timeEndPeriod(1);
            }

            public double GetElapsedTime()
            {
                long counter;
                QueryPerformanceCounter(out counter);
                double timer = (counter - startTime) / (double)freq;
                startTime = counter;
                return timer;
            }
            public double PeekElapsedTime()
            {
                long counter;
                QueryPerformanceCounter(out counter);
                double timer = (counter - startTime) / (double)freq;
                return timer;
            }
            public double GetTimer()
            {
                long counter;
                QueryPerformanceCounter(out counter);
                return counter / freq;
            }
        }
    }
}
