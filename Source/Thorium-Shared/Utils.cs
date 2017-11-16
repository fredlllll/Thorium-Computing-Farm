using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

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

        public static void CopyDirectory(string source, string target)
        {
            var stack = new Stack<Tuple<string, string>>();
            stack.Push(new Tuple<string, string>(source, target));

            while(stack.Count > 0)
            {
                var folders = stack.Pop();
                Directory.CreateDirectory(folders.Item2);
                foreach(var file in Directory.GetFiles(folders.Item1, "*.*", SearchOption.TopDirectoryOnly))
                {
                    File.Copy(file, Path.Combine(folders.Item2, Path.GetFileName(file)));
                }

                foreach(var folder in Directory.GetDirectories(folders.Item1))
                {
                    stack.Push(new Tuple<string, string>(folder, Path.Combine(folders.Item2, Path.GetFileName(folder))));
                }
            }
        }
    }
}
