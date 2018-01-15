using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using static Thorium_Shared.JobAndTaskProperties;

namespace Thorium_Shared.Jobtypes.SimpleExecution
{
    public class SETaskProducer : ATaskProducer
    {
        public SETaskProducer(Job job) : base(job)
        {
        }

        /*int providedTasks = 0;
        public override Task GetNextTask()
        {
            JObject ji = Job.Information;
            int maxCount = ji.Get<int>("tasksCount");
            if(providedTasks < maxCount)
            {
                //TODO: additional arguments?

                JObject info = new JObject
                {
                    ["index"] = providedTasks,
                    ["program"] = Job.Information["program"],
                    ["args"] = Job.Information["args"],
                    [ExecutionerType] = typeof(SEExecutioner).AssemblyQualifiedName
                };

                providedTasks++;
                return new Task(Job.ID, Utils.GetRandomID(), info);
            }
            return null;
        }*/

        public override IEnumerator<Task> GetTasks()
        {
            JObject ji = Job.Information;
            int maxCount = ji.Get<int>("tasksCount");
            for(int i =0; i< maxCount; i++)
            {
                //TODO: additional arguments?

                JObject info = new JObject
                {
                    ["index"] = i,
                    ["executable"] = Job.Information["executable"],
                    ["args"] = Job.Information["args"],
                    [ExecutionerType] = typeof(SEExecutioner).AssemblyQualifiedName
                };

                yield return new Task(Job.ID, Utils.GetRandomID(), info);
            }
        }
    }
}
