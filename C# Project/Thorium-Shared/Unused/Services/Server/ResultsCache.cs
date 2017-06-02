/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared.Services.Server
{
    public class ResultsCache : IResultsCache
    {
        Dictionary<string, byte[]> cache = new Dictionary<string, byte[]>();
        public void RegisterResult(string name, byte[] bytes)
        {
            cache[name] = bytes;
        }

        public byte[] GetResult(string name, bool delete = false)
        {
            byte[] bytes = cache[name];
            if(delete)
            {
                cache.Remove(name);
            }
            return bytes;
        }

        public void DeleteResult(string name)
        {
            cache.Remove(name);
        }
    }
}
*/