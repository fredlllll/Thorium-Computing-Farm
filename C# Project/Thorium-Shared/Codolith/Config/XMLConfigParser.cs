using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Codolith.Config
{
    public class XMLConfigParser : IConfigParser
    {
        protected XmlDocument xdoc = null;

        protected Stream stream;
        public Stream Stream
        {
            get
            {
                return stream;
            }
            set
            {
                stream = value;
                xdoc = new XmlDocument();
                xdoc.Load(stream);
            }
        }

        public void FillDictionary(Dictionary<string, string> dict)
        {
            var nodes = xdoc.SelectNodes("/config/element");
            foreach(XmlNode node in nodes)
            {
                try
                {
                    dict[node.Attributes["key"].Value] = node.Attributes["value"].Value;
                }
                catch
                {
                    //ignore, should write to log, but dont want to add dependency atm. i should think about something more dynamic than a static dep
                }
            }
        }
    }
}
