using System;
using System.Diagnostics;

namespace Thorium_Shared
{
    public static class Util
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
