using NLog;
using Thorium_Shared;
using Thorium_Shared.Data.Serializers;

namespace Thorium_Server
{
    public class TaskManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly TaskSerializer serializer;

        public TaskManager(TaskSerializer serializer)
        {
            this.serializer = serializer;
        }

        public Task CheckoutTask()
        {
            Task t = serializer.CheckoutTask();
            if(t != null)
            {
                logger.Info("Task checked out: " + t.ID);
            }
            return t;
        }

        public void TurnInTask(string id)
        {
            serializer.UpdateStatus(id, TaskStatus.Finished);
            logger.Info("Task turned in: " + id);
        }

        public void AbandonTask(string id, string reason = null)
        {
            serializer.UpdateStatus(id, TaskStatus.Waiting);
            logger.Info("Task abandoned: " + id + (reason != null ? " reason: " + reason : ""));
        }

        public void FailTask(string id, string reason = null)
        {
            serializer.UpdateStatus(id, TaskStatus.Failed);
            logger.Info("Task failed: " + id + (reason != null ? " reason: " + reason : ""));
        }

        public void AbortTask(string iD)
        {
            //TODO: abort task on its machine and put in finished?
        }
    }
}