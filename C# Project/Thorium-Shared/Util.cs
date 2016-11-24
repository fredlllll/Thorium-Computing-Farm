using System;
using System.Diagnostics;
using System.Linq;

namespace Thorium_Shared
{
    public static class Util
    {
        public static Random R { get; } = new Random();

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

        public static string GetRandomID()
        {
            Guid guid = new Guid();
            return guid.ToString();
        }

        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[R.Next(s.Length)]).ToArray());
        }
    }
}
