using System.Net;

namespace Thorium_Net
{
    public static class NetUtils
    {
        //TODO: this isnt optimal, but works for now...
        public static string GetExternalIP()
        {
            WebClient wc = new WebClient();

            string response = wc.DownloadString("http://checkip.dyndns.org");

            string[] partsAroundColon = response.Split(':');
            string secondPartTrimmed = partsAroundColon[1].Trim();
            string[] splitByTagStart = secondPartTrimmed.Split('<');
            string ip = splitByTagStart[0];
            return ip;
        }
    }
}
