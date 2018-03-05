using System;
using System.Diagnostics;

namespace Thorium_Shared
{
    public static class ThoriumUtils
    {
        public static void ShutdownSystem()
        {
            switch(Environment.OSVersion.Platform)
            {
                case PlatformID.MacOSX://probably the same as linux?
                case PlatformID.Unix:
                    ProcessStartInfo procInfo = new ProcessStartInfo
                    {
                        FileName = "shutdown",
                        UseShellExecute = false,
                        Arguments = "-h +1"
                    };
                    Process.Start(procInfo);
                    break;
                case PlatformID.Win32NT:
                    procInfo = new ProcessStartInfo
                    {
                        FileName = "shutdown",
                        UseShellExecute = false,
                        Arguments = "/s /t 30"
                    };
                    Process.Start(procInfo);
                    break;
            }
        }
    }
}
