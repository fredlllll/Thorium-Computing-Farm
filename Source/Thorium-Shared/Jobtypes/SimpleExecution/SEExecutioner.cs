using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Thorium_Shared.Jobtypes.SimpleExecution
{
    public class SEExecutioner : AExecutioner
    {
        public SEExecutioner(LightweightTask t) : base(t)
        {
        }

        public override void Execute()
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
        }
    }
}
