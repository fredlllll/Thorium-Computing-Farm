using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Thorium_Shared
{
    public class Task
    {
        public Job Job { get; protected set; }

        public string ID { get; protected set; }
        public JObject Information { get; protected set; }
        public TaskStatus Status { get; protected set; }

        public Task(Job job, string id, JObject information, TaskStatus status = TaskStatus.Waiting)
        {
            Job = job;
            ID = id;
            Information = information;
            Status = status;
        }
    }
}
