using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Codolith.Config;
using Codolith.Serialization;
using Thorium_Shared.WCFInterfaces;

namespace Thorium_Shared
{
    [DataContract]
    public abstract class AJob
    {
        public delegate void JobAbortedHandler(AJob job, string reason);
        public delegate void JobFinishedHandler(AJob job);
        public event JobFinishedHandler JobFinished;
        public event JobAbortedHandler JobAborted;

        public delegate void TaskAbortedHandler(AJob job, string id, string reason);
        public delegate void TaskFinishedHandler(AJob job, string id);
        public event TaskAbortedHandler TaskAborted;
        public event TaskFinishedHandler TaskFinished;

        public string ID { get { return JobInformation.ID; } protected set { JobInformation.ID = value; } }

        [DataMember]
        protected string name;
        public string Name { get { return name; } protected set { name = value; } }

        [DataMember]
        protected ATaskInformationProducer TaskInformationProducer { get; set; }

        [DataMember]
        protected JobInformation JobInformation { get; set; }

        public AJob(JobInformation information)
        {
            JobInformation = information;
        }

        /// <summary>
        /// initialize your task information producer here
        /// </summary>
        public abstract void Initialize();

        public void SignalTaskAborted(string id, string reason = default(string))
        {
            TaskAborted?.Invoke(this, id, reason);
            TaskInformationProducer.SignalTaskAborted(id, reason);
        }

        public void SignalTaskFinished(string id)
        {
            TaskFinished?.Invoke(this, id);
            TaskInformationProducer.SignalTaskFinished(id);
        }

        MultiDictionary<string, TaskInformation, IThoriumClientInterfaceForServer> currentlyProcessingTasks = new MultiDictionary<string, TaskInformation, IThoriumClientInterfaceForServer>();
        public TaskInformation GetFreeTask(IThoriumClientInterfaceForServer client)
        {
            lock(TaskInformationProducer)
            {
                if(TaskInformationProducer.RemainingTaskInformationCount > 0)
                {
                    var task = TaskInformationProducer.GetNextTaskInformation();
                    var mt = new ModTuple<string, TaskInformation, IThoriumClientInterfaceForServer>(task.ID, task, client);
                    return task;
                }
            }
            return null;
        }

        public static AJob JobFromInformation(JobInformation information)
        {
            return (AJob)Activator.CreateInstance(information.JobType, information);
        }

        public void Cancel()
        {
            TaskInformationProducer.Stop();
            foreach(var t in currentlyProcessingTasks)
            {
                t.Value.Value3.AbortTask(t.Key);
            }
        }
    }
}
