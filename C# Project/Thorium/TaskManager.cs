using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thorium_Shared;

namespace Thorium_Server
{
    public class TaskManager
    {
        public ConcurrentQueue<Task> Tasks { get; } = new ConcurrentQueue<Task>();

        public Task GetTask(IThoriumClientInterfaceForServer client)
        {
            Task ret;
            if(Tasks.TryDequeue(out ret))
            {
                return ret;
            }
            return null;
        }

        public void TurnInTask(Task task)
        {
            //uhhh wut now? tell the job i guess
            
        }

        /// <summary>
        /// registers a task, so it can be given out to clients. 
        /// </summary>
        /// <param name="task"></param>
        public void RegisterTask(Task task)
        {
            Tasks.Enqueue(task);
        }
    }
}
