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
            try
            {
                serializer.UpdateStatus(id, TaskStatus.Aborted);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void AddTask(string id, JObject information, TaskStatus status)
        {
            TaskData td = new TaskData(id, information, status);
            serializer.Save(id, td);
        }

        public TaskStatus GetTaskStatus(string id)
        {
            return serializer.Load(id).Status;
        }

        public bool QueueTask(string id)
        {
            try
            {
                serializer.UpdateStatus(id, TaskStatus.WaitingForExecution);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ReturnAssignableTask(string id)
        {
            serializer.UpdateStatus(id, TaskStatus.WaitingForExecution);
        }
    }
}