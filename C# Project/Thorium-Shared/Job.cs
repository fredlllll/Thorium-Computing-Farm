using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;

namespace Thorium_Shared
{
    public abstract class Job
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public int TasksCount
        {
            get { return tasks.Count; }
        }
        public IEnumerable<Task> Tasks
        {
            get
            {
                return tasks;
            }
        }
        protected List<Task> tasks { get; } = new List<Task>();
        protected Config data { get; }

        public Job(Config data)
        {
            this.data = data;
            ID = Util.GetRandomID();
        }

        /*public Job(string ID)
        {
            this.ID = ID;
            //todo: load stuff
        }*/

        public abstract void InitializeTasks();
    }
}
