using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBot.Utils
{
    class HardwareUsage
    {
        private static PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private static PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");

        public static string getCurrentCpuUsage()
        {
            return cpuCounter.NextValue() + "%";
        }

        public static string getAvailableRAM()
        {
            return ramCounter.NextValue() + "MB";
        }
    }
}
