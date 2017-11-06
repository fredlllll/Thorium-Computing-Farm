using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Thorium_Shared.Jobtypes.SimpleExecution
{
    public class SETaskProducer : ATaskProducer
    {
        public SETaskProducer(Job job) : base(job)
        {
        }

        int providedTasks = 0;
        public override Task GetNextTask()
        {
            JObject ji = Job.Information;
            int maxCount = ji.Get<int>("tasksCount");
            if(providedTasks < maxCount)
            {
                //TODO: additional arguments?

                JObject info = new JObject
                {
                    ["index"] = providedTasks.ToString(),
                    ["program"] = Job.Information["program"],
                    ["executioner"] = typeof(SEExecutioner).AssemblyQualifiedName
                };

                providedTasks++;
                return new Task(Job, Utils.GetRandomID(), info);
            }
            return null;
        }
    }
}
