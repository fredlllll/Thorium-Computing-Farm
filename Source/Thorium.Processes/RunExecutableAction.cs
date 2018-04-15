using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Thorium_Processes
{
    public class RunExecutableAction
    {
        public List<string> Arguments { get; } = new List<string>();
        public Dictionary<string, string> Environment { get; } = new Dictionary<string, string>();
        public string FileName { get; set; }

        public Process Process { get; protected set; }
        bool started = false;

        public void AddArgument(string arg)
        {
            Arguments.Add(arg);
        }

        public void CreateProcess()
        {
            if(Process != null)
            {
                return;
            }
            StringBuilder argsBuilder = new StringBuilder();

            foreach(var arg in Arguments)
            {
                argsBuilder.Append(ProcessUtil.EscapeArgument(arg));
                argsBuilder.Append(" ");
            }

            Process = new Process
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = FileName,
                    Arguments = argsBuilder.ToString(),
                    UseShellExecute = false,
                }
            };

            foreach(var kv in Environment)
            {
                Process.StartInfo.Environment.Add(kv.Key, kv.Value);
            }
        }

        public void Start()
        {
            CreateProcess();
            if(started)
            { return; }
            Process.Start();
            started = true;
        }

        public void StartAndWait()
        {
            Start();
            Process.WaitForExit();
        }
    }
}
