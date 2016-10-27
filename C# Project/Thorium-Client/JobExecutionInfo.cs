using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared;
using Thorium_Shared.ExecutionActions;

namespace Thorium_Client
{
    public class JobExecutionInfo : MarshalByRefObject, IJobPartExecutionInfo
    {
        public IExecutionAction SetupAction { get; set; }
        public IExecutionAction RunAction { get; set; }
        public IExecutionAction TeardownAction { get; set; }

        public void Setup()
        {
            SetupAction.Execute();
        }

        public void Run()
        {
            RunAction.Execute();
        }

        public void Teardown()
        {
            TeardownAction.Execute();
        }
    }
}
