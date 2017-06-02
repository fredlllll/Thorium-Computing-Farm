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
        public delegate void JobCanceledHandler(AJob job);
        public delegate void JobFinishedHandler(AJob job);
        public event JobFinishedHandler JobFinished;
        public event JobCanceledHandler JobCanceled;

        protected string id;
        public string ID { get { return id; } protected set { id = value; } }
        protected string name;
        public string Name { get { return name; } protected set { name = value; } }

        protected ATaskInformationProducer taskInformationProducer;

        protected readonly Config config;

        public AJob(Config config)
        {
            this.config = config;
            ID = Util.GetRandomID();
        }

        public abstract ATaskInformation GetFreeTaskInformation();

        public abstract void Initialize();
    }
}
