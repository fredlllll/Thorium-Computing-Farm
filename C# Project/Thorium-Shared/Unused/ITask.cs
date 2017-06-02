/*using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared
{
    [ServiceContract]
    public interface ITask
    {
        string GetJobID();
        string GetID();
        string GetProcessingClientID();
        void SetProcessingClientID(string id);
        TaskState GetState();
        void SetState(TaskState state);

        ITaskExecutionInfo GetExecutionInfo();
        void FinalizeTask();
    }
}
*/