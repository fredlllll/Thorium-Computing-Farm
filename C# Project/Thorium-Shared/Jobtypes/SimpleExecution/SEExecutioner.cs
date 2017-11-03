using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared.Jobtypes.SimpleExecution
{
    public class SEExecutioner : AExecutioner
    {
        public SEExecutioner(Task t) : base(t)
        {
        }

        public override void Execute()
        {
            int index = Task.Information.Get<int>("index");
            string program = Task.Information.Get<string>("program");

            Process p = new Process();
            p.StartInfo.FileName = program;
            p.StartInfo.Arguments = index.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
