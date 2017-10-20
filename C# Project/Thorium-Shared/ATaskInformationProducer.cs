using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared
{
    /// <summary>
    /// a task creator whose state can be saved and restored
    /// </summary>
    /*[DataContract]
    public abstract class ATaskInformationProducer
    {
        [DataMember]
        protected bool stopped = false;

        public AJob Job
        {
            get;
        }

        public ATaskInformationProducer(AJob job)
        {
            Job = job;
        }

        /// <summary>
        /// returns how many more tasks this producer can provide
        /// </summary>
        public abstract int RemainingTaskInformationCount
        {
            get;
        }
        /// <summary>
        /// returns the next task
        /// </summary>
        /// <returns>next task</returns>
        public abstract TaskInformation GetNextTaskInformation();

        /// <summary>
        /// will be called once a task is finished
        /// </summary>
        /// <param name="id"></param>
        public abstract void SignalTaskFinished(string id);

        /// <summary>
        /// will be called once a task is aborted
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reason"></param>
        public abstract void SignalTaskAborted(string id, string reason);

        /// <summary>
        /// gives the producer a signal to not return any more task informations
        /// </summary>
        public virtual void Stop()
        {
            stopped = true;
        }
    }*/
}
