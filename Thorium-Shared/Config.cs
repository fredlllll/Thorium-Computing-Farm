using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Thorium_Shared
{
    public class Config
    {
        System.Collections.Specialized.StringDictionary dict = new System.Collections.Specialized.StringDictionary();
            
        public Config(FileInfo file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file.FullName);
            foreach(XmlNode node in doc.SelectNodes("/config/option"))
            {
                string name = node.Attributes["name"].Value;
                string value = node.InnerText;
                dict[name] = value;
            }
        }

        public string GetString(string name)
        {
            return dict[name];
        }

        public int GetInt(string name)
        {
            return int.Parse(dict[name]);
        }

        public string this[string name]
        {
            get
            {
                return dict[name];
            }
        }
    }
}
