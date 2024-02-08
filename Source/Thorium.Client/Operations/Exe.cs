using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Thorium.Shared;

namespace Thorium.Client.Operations
{
    public class Exe : ClientOperation
    {
        Shared.DTOs.OperationData.ExeDTO data;

        private ProcessStartInfo processStartInfo;

        public Exe(JsonDocument operationData)
        {
            var data = operationData.Deserialize<Shared.DTOs.OperationData.ExeDTO>();

            processStartInfo = new ProcessStartInfo()
            {
                FileName = data.FilePath,
                WorkingDirectory = data.WorkingDir,
            };
        }

        public override void Execute(int taskNumber)
        {
            Process process = new()
            {
                StartInfo = processStartInfo
            };

            processStartInfo.ArgumentList.Clear();
            foreach (var arg in data.Arguments)
            {
                processStartInfo.ArgumentList.Add(arg.Replace("{taskNumber}", taskNumber.ToString()));
            }
            process.Start();
            process.WaitForExit();
        }
    }
}
