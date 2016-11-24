using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codolith.Util.Concurrent;
using Thorium_Shared;

namespace Thorium_Server
{
    public class TaskManager
    {
        public JobManager JobManager
        {
            get;
            set;
        }

        public IEnumerable<Task> Tasks { get { return tasks; } }
        ConcurrentPriorityList<Task> tasks = new ConcurrentPriorityList<Task>();
        public IEnumerable<Task> ProcessingTasks { get { return processingTasks.Values; } }
        ConcurrentDictionary<string, Task> processingTasks = new ConcurrentDictionary<string, Task>();
        public IEnumerable<Task> FinishedTasks { get { return finishedTasks.Values; } }
        ConcurrentDictionary<string, Task> finishedTasks = new ConcurrentDictionary<string, Task>();

        public Task GetTask(IThoriumClientInterfaceForServer client)
        {
            Task t = tasks.RemoveFirst();
            t.State = TaskState.Processing;
            t.ProcessingClientID = client.ID;
            client.currentTaskID = t.ID;
            processingTasks[t.ID] = t;
            return t;
        }

        public void TurnInTask(Task task)
        {
            processingTasks.TryRemove(task.ID, out task);
            finishedTasks[task.ID] = task;
            task.FinalizeTask();
            task.State = TaskState.Finished;
            //Job job = JobManager.GetJobById(task.JobID);
            //gotta somehow check if a job is done now
        }

        public void ReturnUnfinishedTask(Task task)
        {
            task.State = TaskState.NotStarted;
            processingTasks.TryRemove(task.ID, out task);
            tasks.Add(task, 0);
        }

        /// <summary>
        /// registers a task, so it can be given out to clients. 
        /// </summary>
        /// <param name="task"></param>
        public void RegisterTask(Task task, int priority = 0)
        {
            tasks.Add(task, priority);
        }

        public void UnregisterTask(Task task)
        {
            tasks.Remove(task);
        }
    }
}
