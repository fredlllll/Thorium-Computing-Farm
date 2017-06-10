using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Thorium_Shared
{
    public static class Utils
    {
        public static Random R { get; } = new Random();

        public static void ShutdownSystem()
        {
            switch(Environment.OSVersion.Platform)
            {
                case PlatformID.MacOSX://probably the same as linux?
                case PlatformID.Unix:
                    ProcessStartInfo procInfo = new ProcessStartInfo();
                    procInfo.FileName = "shutdown";
                    procInfo.UseShellExecute = false;
                    procInfo.Arguments = "-h +1";
                    Process.Start(procInfo);
                    break;
                case PlatformID.Win32NT:
                    procInfo = new ProcessStartInfo();
                    procInfo.FileName = "shutdown";
                    procInfo.UseShellExecute = false;
                    procInfo.Arguments = "/s /t 30";
                    Process.Start(procInfo);
                    break;
            }
        }

        public static string GetRandomID()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }

        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[R.Next(s.Length)]).ToArray());
        }

        /*public static string GetWCFClientHost()
        {
            var properties = OperationContext.Current.IncomingMessageProperties;
            var endpointProperty = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            if(endpointProperty != null)
            {
                return endpointProperty.Address;
            }
            return null;
        }*/
    }
}
