using System;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Thorium_Net
{
    public class HttpServiceInvoker : IServiceInvoker
    {
        private WebClient wc = new WebClient();
        private string host;

        public HttpServiceInvoker(string host)
        {
            this.host = host;
        }

        private string ToB64(string str)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
        }

        public JToken Invoke(string routine, JToken arg)
        {
            UriBuilder ub = new UriBuilder("http", host)
            {
                Query = "routine=" + ToB64(routine) + "&arg=" + ToB64(arg.ToString(Newtonsoft.Json.Formatting.None))
            };
            string retval = wc.DownloadString(ub.Uri);
            JObject response = JObject.Parse(retval);//should catch parse exception here in case the service returns crap
            switch(response.Get<string>("status"))
            {
                case "success":
                    return response["returnValue"];
                case "exception":
                    throw new Exception("Exception when invoking: " + response.Get<string>("exception"));
                default:
                    throw new Exception("service response did not have a valid state: " + retval);
            }
        }
    }
}
