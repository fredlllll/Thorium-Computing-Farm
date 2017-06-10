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

            ReferencingSerializer rs = new ReferencingSerializer();
            rs.AddObject(ji);
            var sds = rs.GetSerializationDataSet();

            using(FileStream fs = new FileStream("job.bin", FileMode.Create))
            {
                IFormatter xf = new BinaryFormatter(fs);
                xf.Write(sds);
            }

            SerializationDataSet sds_in;
            using(FileStream fs = new FileStream("job.bin", FileMode.Open))
            {
                IFormatter xf = new BinaryFormatter(fs);
                sds_in = xf.Read();
            }

            ReferencingSerializer rs_in = new ReferencingSerializer();
            rs_in.ReadSerializationDataSet(sds_in);
        }
    }
}
