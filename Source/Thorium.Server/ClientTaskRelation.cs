namespace Thorium.Server
{
    public class ClientTaskRelation
    {
        public string Client { get; set; }
        public string Task { get; set; }

        public ClientTaskRelation(string client, string task)
        {
            Client = client;
            Task = task;
        }
    }
}
