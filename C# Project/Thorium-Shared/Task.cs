using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared
{
    public enum TaskState
    {
        NotStarted,
        Processing,
        Finished
    }

    public abstract class Task : MarshalByRefObject
    {
        public string JobID { get; }
        public string ID { get; }
        public string ProcessingClientID { get; set; }
        public TaskState State { get; set; }
        protected Config Data { get; }

        public Task(string parentJobID, Config data)
        {
            Data = data;
            JobID = parentJobID;
            ID = Util.GetRandomID();
            GetExecutionInfo();
        }

        public abstract ITaskExecutionInfo GetExecutionInfo();
        public abstract void FinalizeTask();
    }
}
