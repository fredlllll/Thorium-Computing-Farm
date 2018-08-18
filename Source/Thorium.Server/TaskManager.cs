using NLog;
using Thorium.Shared;
using Thorium.Data.Implementation.Serializers;
using System;
using Newtonsoft.Json.Linq;

namespace Thorium.Server
{
    public class TaskManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly TaskDataSerializer serializer;

        public TaskManager(TaskDataSerializer serializer)
        {
            this.serializer = serializer;
        }

        /*public LightweightTask CheckoutTask()
        {
            LightweightTask t = serializer.CheckoutTask().ToLightweightTask();
            if(t != null)
            {
                logger.Info("Task checked out: " + t.Id);
            }
            return t;
        }*/

        public TaskData GetAssignableTask()
        {
            return serializer.CheckoutTask();
        }

        public void TurnInTask(string id)
        {
            serializer.UpdateStatus(id, TaskStatus.Finished);
            logger.Info("Task turned in: " + id);
        }

        public void AbandonTask(string id, string reason = null)
        {
            serializer.UpdateStatus(id, TaskStatus.WaitingForExecution);
            logger.Info("Task abandoned: " + id + (reason != null ? " reason: " + reason : ""));
        }

        public void FailTask(string id, string reason = null)
        {
            serializer.UpdateStatus(id, TaskStatus.Failed);
            logger.Info("Task failed: " + id + (reason != null ? " reason: " + reason : ""));
        }

        public bool AbortTask(string id)
        {
            //TODO: abort task on its machine and update in database
            return false;
        }

        public void AddTask(string id, JObject information, TaskStatus status)
        {
            throw new NotImplementedException();
        }

        public TaskStatus GetTaskStatus(string id)
        {
            throw new NotImplementedException();
        }

        public bool QueueTask(string id)
        {
            throw new NotImplementedException();
        }
    }
}