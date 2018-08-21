using System.Threading;
using Thorium.Config;
using Thorium.Server.Data;
using Thorium.Threading;

namespace Thorium.Server
{
    public class ThoriumServer : RestartableThreadClass
    {
        /// <summary>
        /// for control commands
        /// </summary>
        ServerController serverController;
        /// <summary>
        /// for comms between server and clients
        /// </summary>
        ClientsServicePoint clientsServicePoint;

        public TaskManager TaskManager { get; private set; }
        public ClientManager ClientManager { get; private set; }
        public ClientTaskRelationManager ClientTaskRelationManager { get; private set; }

        public DataManager DataManager { get; private set; }


        public ThoriumServer() : base(false)
        {
            DataManager = new DataManager();

            TaskManager = new TaskManager(DataManager.TaskSerializer);
            ClientManager = new ClientManager();
            ClientTaskRelationManager = new ClientTaskRelationManager(DataManager.ClientTaskRelationSerializer);

            serverController = new ServerController(this);
            clientsServicePoint = new ClientsServicePoint(this);
        }


        public override void Start()
        {
            ClientManager.Start();
            ClientTaskRelationManager.Start();
            serverController.Start();
            clientsServicePoint.Start();
            base.Start();
        }

        public override void Stop(int joinTimeoutms = -1)
        {
            ClientManager.Stop(joinTimeoutms);
            ClientTaskRelationManager.Stop();
            serverController.Stop();
            clientsServicePoint.Stop();
            base.Stop(joinTimeoutms);
        }

        protected override void Run()
        {
            while(true)
            {
                //get a client and a task
                var client = ClientManager.GetFreeClient();
                var task = TaskManager.GetAssignableTask();
                //if one of them is null, return the other, so a task cant possibly get stuck
                if(task == null && client != null)
                {
                    ClientManager.ReturnFreeClient(client.Id);
                }
                if(client == null && task != null)
                {
                    TaskManager.ReturnAssignableTask(task.Id);
                }
                if(task != null && client != null)
                {
                    var lt = task.ToLightweightTask();
                    if(client.AssignTask(lt))
                    {
                        var rel = new ClientTaskRelation(client.Id, lt.Id);
                        ClientTaskRelationManager.Add(rel);
                    }
                    else
                    {
                        //return both if it could not be assigned
                        TaskManager.ReturnAssignableTask(task.Id);
                        ClientManager.ReturnFreeClient(client.Id);
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
