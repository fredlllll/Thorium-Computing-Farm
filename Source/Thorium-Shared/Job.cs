using System;
using System.Reflection;
using Newtonsoft.Json.Linq;
using static Thorium_Shared.JobProperties;

namespace Thorium_Shared
{
    public class Job
    {
        public string ID { get; protected set; }
        public string Name { get; protected set; }
        public JObject Information { get; protected set; }
        public JobStatus Status { get; set; }

        public Job(string id, string name, JObject info, JobStatus status)
        {
            ID = id;
            Name = name;
            Information = info;
            Status = status;
        }

        protected ATaskProducer taskProducer = null;
        public ATaskProducer TaskProducer
        {
            get
            {
                if(taskProducer == null)
                {
                    string producerClass = Information.Get<string>(TaskProducerType);
                    Type producerType = ReflectionHelper.GetType(producerClass);
                    if(producerType == null)
                    {
                        throw new Exception("Could not find type for producer: " + producerClass);
                    }
                    var ci = producerType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(Job) }, null);
                    if(ci == null)
                    {
                        throw new Exception("the type " + producerClass + " does not have a constructor that takes a job object");
                    }
                    taskProducer = (ATaskProducer)ci.Invoke(new object[] { this });
                }
                return taskProducer;
            }
        }
    }
}
