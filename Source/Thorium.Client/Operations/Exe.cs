using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace Thorium.Client.Operations
{
    public class Exe : ClientOperation
    {
        private readonly Dictionary<string, string> data;
        private readonly List<string> rawArgs = new();

        private ProcessStartInfo processStartInfo;

        public Exe(Dictionary<string, string> data)
        {
            this.data = data;
            processStartInfo = new ProcessStartInfo()
            {
                FileName = data["fileName"],
                WorkingDirectory = data["workingDir"],
            };
            if (data.TryGetValue("arguments", out string? argumentsData))
            {
                var parsed = JsonNode.Parse(argumentsData);
                if (parsed == null)
                {
                    throw new Exception("argumentsData evaluates to null");
                }
                var arguments = parsed.AsArray();
                foreach (var argument in arguments)
                {
                    //honestly, screw the nullchecks here
                    rawArgs.Add((string)argument);
                }
            }
        }

        public override void Execute(int taskNumber)
        {
            Process process = new()
            {
                StartInfo = processStartInfo
            };

            processStartInfo.ArgumentList.Clear();
            foreach (var rawArg in rawArgs)
            {
                processStartInfo.ArgumentList.Add(rawArg.Replace("{taskNumber}", taskNumber.ToString()));
            }
            process.Start();
            process.WaitForExit();
        }
    }
}
