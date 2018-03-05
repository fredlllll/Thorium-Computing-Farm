using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Thorium_Processes
{
    public static class ProcessUtil
    {
        public static Process BeginRunExecutableWithRedirect(string filename, string arguments, Dictionary<string, string> environmentVariables = null, string workingDirectory = null, bool start = true)
        {
            Process p = CreateProcess(filename, arguments, redirectStdOut: true, redirectStdErr: true, enableRisingEvents: true, environmentVariables: environmentVariables, workingDirectory: workingDirectory);

            if(start)
            {
                p.Start();
            }

            return p;
        }

        public static LoggedProcessInfo BeginRunExecutableWithLog(string filename, string arguments, string logPath, Dictionary<string, string> environmentVariables = null, string workingDirectory = null)
        {
            Process p = BeginRunExecutableWithRedirect(filename, arguments, environmentVariables, workingDirectory, start: false);
            LoggedProcessInfo lpi = new LoggedProcessInfo(p, logPath);
            lpi.Start();
            return lpi;
        }

        public static Process BeginRunExecutable(string filename, string arguments, Dictionary<string, string> environmentVariables = null, string workingDirectory = null)
        {
            Process p = CreateProcess(filename, arguments, environmentVariables: environmentVariables, workingDirectory: workingDirectory);
            p.Start();
            return p;
        }

        private static Process CreateProcess(string filename, string arguments, bool useShellExecute = false, bool createNoWindow = true, bool redirectStdOut = false, bool redirectStdErr = false, bool enableRisingEvents = false, Dictionary<string, string> environmentVariables = null, string workingDirectory = null)
        {
            Process p = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = filename,
                    Arguments = arguments,
                    UseShellExecute = useShellExecute,
                    CreateNoWindow = createNoWindow,
                    RedirectStandardOutput = redirectStdOut,
                    RedirectStandardError = redirectStdErr
                },
                EnableRaisingEvents = enableRisingEvents,
            };

            if(environmentVariables != null)
            {
                foreach(var kv in environmentVariables)
                {
                    p.StartInfo.EnvironmentVariables.Add(kv.Key, kv.Value);
                }
            }

            if(workingDirectory != null)
            {
                p.StartInfo.WorkingDirectory = workingDirectory;
            }

            return p;
        }

        /// <summary>
        /// rudimentary method to escape strings for process args. why isnt there a OS level method for this?
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string EscapeArgument(string arg)
        {
            if(string.IsNullOrEmpty(arg))
            {
                return arg;
            }

            if(arg.Contains("\\"))
            {
                arg = arg.Replace("\\", "\\\\");
            }
            bool needsQuotes = false;
            if(arg.Contains("\""))
            {
                needsQuotes = true;
                arg = arg.Replace("\"", "\\\"");
            }
            if(arg.Contains(" "))
            {
                needsQuotes = true;
            }

            if(needsQuotes)
            {
                return "\"" + arg + "\"";
            }
            else
            {
                return arg;
            }
        }

        public static void ThrowIfExitCode(Process p)
        {
            if(p.ExitCode != 0)
            {
                throw new Exception("Process " + p.StartInfo.FileName + " " + p.StartInfo.Arguments + " Ended with ExitCode" + p.ExitCode);
            }
        }

        public static void StartASyncReads(Process p)
        {
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
        }
    }
}
