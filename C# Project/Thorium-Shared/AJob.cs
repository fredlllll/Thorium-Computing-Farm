using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using Codolith.Config;

namespace Thorium_Shared
{
    [Serializable]
    public abstract class AJob
    {
        public delegate void JobAbortedHandler(AJob job, string reason);
        public delegate void JobFinishedHandler(AJob job);
        public event JobFinishedHandler JobFinished;
        public event JobAbortedHandler JobAborted;

        protected string id;
        public string ID { get { return id; } protected set { id = value; } }
        protected string name;
        public string Name { get { return name; } protected set { name = value; } }

        public ATaskInformationProducer TaskInformationProducer { get; protected set; }

        protected readonly Config config;

        public AJob(Config config)
        {
            this.config = config;
            ID = Util.GetRandomID();
        }

        /// <summary>
        /// initialize your task information producer here
        /// </summary>
        public abstract void Initialize();

        public delegate void TaskAbortedHandler(AJob job, string id, string reason);
        public delegate void TaskFinishedHandler(AJob job, string id);
        public event TaskAbortedHandler TaskAborted;
        public void SignalTaskAborted(string id, string reason = default(string))
        {
            TaskAborted?.Invoke(this, id, reason);
        }
        public event TaskFinishedHandler TaskFinished;
        public void SignalTaskFinished(string id)
        {
            TaskFinished?.Invoke(this, id);
        }

        /*public AJob GetNewJob(Config config)
        {
            var jobType = config.Get(Key_JobType);
            Type type = Codolith.Reflection.ReflectionHelper.GetTypeByShortName(jobType).FirstOrDefault();
            if(type != null)
            {
                return (AJob)Activator.CreateInstance(type, config);
            }
            return null;
        }*/
    }
}
