using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json.Linq;
using Thorium.IO;
using Thorium.Processes;
using Thorium.Shared.Unused;

namespace Thorium.Jobs.SimpleExecution
{
    public class SEExecutioner : AExecutioner
    {
        public SEExecutioner(LightweightTask t) : base(t)
        {
        }

        public override ExecutionResult Execute()
        {
            int index = Task.GetInfo<int>("index");
            string executable = Task.GetInfo<string>("executable");
            JArray args = Task.GetInfo<JArray>("args");

            Process p = new Process();
            p.StartInfo.FileName = Files.GetExecutablePath(executable);
            p.StartInfo.EnvironmentVariables["THORIUM_SE_INDEX"] = index.ToString(System.Globalization.CultureInfo.InvariantCulture);
            string argString = string.Join(" ", args.Select(x => ProcessUtil.EscapeArgument(x.Value<string>())));
            p.StartInfo.Arguments = argString;
            p.Start();
            p.WaitForExit();

            return new ExecutionResult(FinalAction.TurnIn, "done");
        }
    }
}
