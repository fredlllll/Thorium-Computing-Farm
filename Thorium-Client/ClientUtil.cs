using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Client
{
    public static class ClientUtil
    {
        public static void ShutdownSystem()
        {
            switch(Environment.OSVersion.Platform)
            {
                case PlatformID.MacOSX://probably the same as linux?
                case PlatformID.Unix:
                    ProcessStartInfo pri = new ProcessStartInfo();
                    pri.FileName = "shutdown";
                    pri.UseShellExecute = false;
                    pri.Arguments = "-h +1";
                    Process.Start(pri);
                    break;
                case PlatformID.Win32NT:
                    pri = new ProcessStartInfo();
                    pri.FileName = "shutdown";
                    pri.UseShellExecute = false;
                    pri.Arguments = "/s /t 30";
                    Process.Start(pri);
                    break;
            }
        }
    }
}
