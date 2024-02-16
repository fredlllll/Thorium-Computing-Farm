using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Thorium.Shared;
using Thorium.Shared.DTOs.OperationData;

namespace Thorium.Client.Operations
{
    public class Exe : ClientOperation
    {
        ExeDTO data;

        private ProcessStartInfo processStartInfo;

        public Exe(ExeDTO data)
        {
            this.data = data;
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
            if (data.Arguments != null)
            {
                foreach (var arg in data.Arguments)
                {
                    processStartInfo.ArgumentList.Add(arg.Replace("{taskNumber}", taskNumber.ToString()));
                }
            }
            process.Start();
            process.WaitForExit();
        }
    }
}
