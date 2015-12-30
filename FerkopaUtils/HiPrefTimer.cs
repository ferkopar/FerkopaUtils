using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;


namespace FerkopaUtils
{
    /// <summary>
    /// HiPerfTimer myProfiler = new HiPerfTimer();
    /// myProfiler.Start();
    /// // rendezés, számítás, renderelés, stb
    /// myProfiler.Stop();
    /// </summary>
    public class HiPerfTimer
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        private long startTime, stopTime;
        private long frequency;
        private long calibrationTime;

        // Constructor
        public HiPerfTimer()
        {
            startTime = 0;
            stopTime = 0;
            calibrationTime = 0;
            Calibrate();
        }

        public void Calibrate()
        {
            if (QueryPerformanceFrequency(out frequency) == false)
            {
                // high-performance counter not supported
                throw new Win32Exception();
            }

            for (int i = 0; i < 10000; i++)
            {
                Start();
                Stop();
                calibrationTime += stopTime - startTime;
            }

            calibrationTime /= 10000;
        }

        // Start the timer
        public void Start()
        {
            // lets do the waiting threads there work
            Thread.Sleep(0);

            QueryPerformanceCounter(out startTime);
        }

        // Stop the timer
        public void Stop()
        {
            QueryPerformanceCounter(out stopTime);
        }

        // Returns the duration of the timer (in seconds)
        public double Duration
        {
            get
            {
                return (double)(stopTime - startTime - calibrationTime) / (double)frequency;
            }
        }
    }
}
