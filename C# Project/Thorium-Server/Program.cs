using System;
using System.Linq;
using System.Threading;
using Newtonsoft.Json.Linq;
using Thorium_Shared;
using Thorium_Shared.Logging;
using Thorium_Shared.Net;
using Thorium_Shared.Net.Comms;

namespace Thorium_Server
{
    class Program
    {
        static ThoriumServer server;
        static ConsoleMenu menu;
        static void Main(string[] args)
        {
            Logging.SetupLogging();

            PluginLoader.LoadPlugins();

            server = new ThoriumServer();
            server.Start();

            Thread.Sleep(1000);

            //AddBlenderJob();

            if(args.Contains("-menu"))
            {
                menu = new ConsoleMenu();
                menu.AddMethod("stop", Stop);
                menu.AddMethod("listjobs", ListJobs);
                menu.AddMethod("help", Help);
                Help(null);
                menu.Run();
            }
        }

        static void AddBlenderJob()
        {
            var client = CommsFactory.CreateClient("localhost", ThoriumServerConfig.ListeningPort);

            JObject info = new JObject
            {
                [JobProperties.TaskProducerType] = "Thorium_Blender.BlenderTaskProducer",//typeof(BlenderTaskProducer).AssemblyQualifiedName,
                [JobAndTaskProperties.ExecutionerType] = "Thorium_Blender.BlenderExecutioner",//typeof(BlenderExecutioner).AssemblyQualifiedName,
                ["fileName"] = "sarfis_test.blend",
                ["framesStart"] = 34,
                ["framesEnd"] = 90,
            };

            JObject arg = new JObject
            {
                ["jobName"] = "test blender job",
                ["jobInformation"] = info
            };

            client.Invoke(ServerControlCommands.AddJob, arg);

        }

        static void Stop(string[] args)
        {
            server.Stop();
            menu.Stop();
        }

        static void ListJobs(string[] args)
        {
            Console.WriteLine("Jobs:");
            /*foreach(var j in server.JobManager.Jobs) {
                Console.WriteLine(j.Value.ID);
            }*/
        }

        static void Help(string[] args)
        {
            Console.WriteLine("available commands:");
            Console.WriteLine("Stop");
            Console.WriteLine("ListTasks");
            Console.WriteLine("Help");
        }
    }
}
