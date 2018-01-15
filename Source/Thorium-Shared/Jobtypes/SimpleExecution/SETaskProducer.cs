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

        public override IEnumerator<Task> GetTasks()
        {
            JObject ji = Job.Information;
            int maxCount = ji.Get<int>("tasksCount");
            for(int i =0; i< maxCount; i++)
            {
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
