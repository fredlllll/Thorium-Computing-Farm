using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NLog;
using Thorium_Shared;

namespace Thorium_Server
{
    public class TaskManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //TODO: add some kind of task holding class that can also keep info about processing like status or the instance its computed on
        //best use a task state instead of different lists. then create an extra class that is allowed to change task state and manages the different collections for speed increase

        ConcurrentBag<Task> waitingTasks = new ConcurrentBag<Task>();
        ConcurrentDictionary<string, Task> computingTasks = new ConcurrentDictionary<string, Task>();
        ConcurrentDictionary<string, Task> finishedTasks = new ConcurrentDictionary<string, Task>();

        public IEnumerable<Task> Tasks { get { return waitingTasks.Concat(computingTasks.Concat(finishedTasks).Select((x) => x.Value)); } }

        public Task CheckoutTask()
        {
            if(waitingTasks.TryTake(out Task result))
            {
                computingTasks[result.ID] = result;
                logger.Info("Task checked out: " + result.ID);
                return result;
            }
            return null;
        }

        public void TurnInTask(string id)
        {
            computingTasks.TryRemove(id, out Task t);
            logger.Info("Task turned in: " + id);
            finishedTasks[id] = t;
        }

        public void AbandonTask(string id)
        {
            logger.Info("Task abandoned: " + id);
            computingTasks.TryRemove(id, out Task t);
            waitingTasks.Add(t);
        }

        public void AddTask(Task t)
        {
            waitingTasks.Add(t);
        }

        public void AbortTask(string iD)
        {
            //TODO: abort task on its machine and put in finished?
        }
    }
}