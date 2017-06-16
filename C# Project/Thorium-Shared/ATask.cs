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

    public abstract class ATask
    {
        protected TaskInformation TaskInformation { get; set; }

        public string GetJobID() { return TaskInformation.JobID; }
        public string GetID() { return TaskInformation.ID; }

        public ATask(TaskInformation info)
        {
            TaskInformation = info;
        }

        /*protected Config Data { get; }

        public ATask(Config data)
        {
            Data = data;
            jobId = parentJobID;
            id = Util.GetRandomID();
        }*/

        public abstract void Run();

        public static ATask TaskFromInformation(TaskInformation information)
        {
            return (ATask)Activator.CreateInstance(information.TaskType, information);
        }

        public abstract void Abort();
    }
}
