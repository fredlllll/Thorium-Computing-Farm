using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared.Jobtypes.SimpleExecution
{
    public class SETask : ATask
    {
        public SETask(TaskInformation info) : base(info)
        {
        }

        public override void Run()
        {
            int index = TaskInformation.Config.Get<int>("index");
            string program = TaskInformation.Config.Get("program");

            Process p = new Process();
            p.StartInfo.FileName = program;
            p.StartInfo.Arguments = index.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
