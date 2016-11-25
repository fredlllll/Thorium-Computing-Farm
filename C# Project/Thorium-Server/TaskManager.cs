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

        public ITask GetTask(IThoriumClientInterfaceForServer client)
        {
            Task t = tasks.RemoveFirst();
            t.SetState(TaskState.Processing);
            t.SetProcessingClientID(client.GetID());
            client.SetCurrentTaskID(t.GetID());
            processingTasks[t.GetID()] = t;
            return t;
        }

        public void TurnInTask(ITask task)
        {
            Task tsk;
            processingTasks.TryRemove(task.GetID(), out tsk);
            finishedTasks[task.GetID()] = tsk;
            task.FinalizeTask();
            task.SetState(TaskState.Finished);
            //Job job = JobManager.GetJobById(task.JobID);
            //gotta somehow check if a job is done now
        }

        public void ReturnUnfinishedTask(ITask task)
        {
            task.SetState(TaskState.NotStarted);
            Task tsk;
            processingTasks.TryRemove(task.GetID(), out tsk);
            tasks.Add(tsk, 0);
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
