using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Codolith.Config
{
    public class INIConfigParser : IConfigParser
    {
        public Stream Stream
        {
            get;set;
        }

        public void FillDictionary(Dictionary<string,string> dict)
        {
            using(StreamReader sr = new StreamReader(Stream))
            {
                string line;
                while((line = sr.ReadLine()) != null)
                {
                    if(line.Length > 1 && line.Contains('='))
                    {
                        string[] parts = line.Split(new char[] { '=' }, 2);
                        dict[parts[0]] = parts[1];
                    }
                }
            }
        }
    }
}
