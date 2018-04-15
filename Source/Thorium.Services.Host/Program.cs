namespace Thorium_Services_Host
{
    public static class Program
    {
        static ServicesHost servicesHost;

        public static void Main(string[] args)
        {
            servicesHost = new ServicesHost();
            servicesHost.Start();
        }
    }
}
