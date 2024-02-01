using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium.Shared.DTOs;

namespace Thorium.Server
{
    public class Job
    {
        public ThoriumJob ThoriumJob { get; private set; }
        private readonly Task[] tasks;

        private readonly LinkedList<Task> queuedTasks = new();
        private readonly List<Task> runningTasks = new();
        private readonly List<Task> finishedTasks = new();

        public Job(ThoriumJob job)
        {
            ThoriumJob = job;
            tasks = new Task[job.TaskCount];
            for (int i = 0; i < tasks.Length; i++)
            {
                queuedTasks.AddFirst(tasks[i] = new Task(this, i));
            }
        }


        public bool HasTasks
        {
            get { return queuedTasks.Count > 0; }
        }

        public Task GetNextTask()
        {
            if (queuedTasks.Count > 0)
            {
                var task = queuedTasks.First.Value;
                queuedTasks.RemoveFirst();
                runningTasks.Add(task);
                return task;
            }
            return null;
        }

        public void RequeueTask(int taskNum)
        {
            var task = tasks[taskNum];
            if (!runningTasks.Remove(task))
            {
                if (!finishedTasks.Remove(task))
                {
                    return; //task already queued
                }
            }
            queuedTasks.AddLast(task);
        }

        public void TurnInTask(int taskNum, string status)
        {
            var task = tasks[taskNum];
            if (!runningTasks.Remove(task))
            {
                return; //task wasnt running
            }
            task.Status = status;
            finishedTasks.Add(task);
        }
    }
}
