using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared.ExecutionActions
{
    [Serializable]
    public class RunExecutableAction : IExecutionAction
    {
        public string ExecutableFile { get; set; }
        public string ExecutionFolder { get; set; }
        public string[] Arguments { get; set; }

        public void Execute()
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ExecutableFile;
            psi.WorkingDirectory = ExecutionFolder;
            StringBuilder sb = new StringBuilder();
            foreach(string s in Arguments)
            {
                if(s.Contains(' '))
                {
                    sb.Append('"');
                    sb.Append(s);
                    sb.Append('"');
                }
                else
                {
                    sb.Append(s);
                }
                sb.Append(' ');
            }
            psi.Arguments = sb.ToString();
            psi.UseShellExecute = false;
            Process p = new Process();
            p.StartInfo = psi;
            p.Start();
            p.WaitForExit();
        }
    }
}
