using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codolith.Config;

namespace Thorium_Shared
{
    public enum TaskState
    {
        NotStarted,
        Processing,
        Finished
    }

    public abstract class Task :ITask
    {
        string jobId;
        public string GetJobID()
        {
            return jobId;
        }
        string id;
        public string GetID()
        {
            return id;
        }
        string processingClientId;
        public string GetProcessingClientID()
        {
            return processingClientId;
        }
        public void SetProcessingClientID(string id)
        {
            processingClientId = id;
        }
        TaskState state;
        public TaskState GetState()
        {
            return state;
        }
        public void SetState(TaskState state)
        {
            this.state = state;
        }

        protected Config Data { get; }

        public Task(string parentJobID, Config data)
        {
            Data = data;
            jobId = parentJobID;
            id = Util.GetRandomID();
        }

        public abstract ITaskExecutionInfo GetExecutionInfo();
        public abstract void FinalizeTask();
    }
}
