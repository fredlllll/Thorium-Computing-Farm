using System.Collections.Generic;
using System.IO;
using Codolith.Serialization;
using Codolith.Serialization.Formatters;
using Thorium_Shared;
using Thorium_Shared.Jobtypes.SimpleExecution;

namespace Thorium_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            JobInformation ji = new JobInformation();
            ji.ID = Thorium_Shared.Utils.GetRandomID();
            ji.JobType = typeof(SEJob);

            ji.Config.Set("count", 10);
            ji.Config.Set("program", "echo");

            var jl = new List<JobInformation>();
            jl.Add(ji);

            ReferencingSerializer rs = new ReferencingSerializer();
            rs.AddObject(jl);
            var sds = rs.GetSerializationDataSet();

            string file = "job.xml";
            using(FileStream fs = new FileStream(file, FileMode.Create))
            {
                IFormatter xf = new XMLFormatter(fs);
                xf.Write(sds);
            }

            SerializationDataSet sds_in;
            using(FileStream fs = new FileStream(file, FileMode.Open))
            {
                IFormatter xf = new XMLFormatter(fs);
                sds_in = xf.Read();
            }

            ReferencingSerializer rs_in = new ReferencingSerializer();
            rs_in.ReadSerializationDataSet(sds_in);
        }
    }
}
