using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Thorium_Shared
{
    public class Config
    {
        StringDictionary dict = new StringDictionary();

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

        public Config()
        {

        }

        public string GetString(string name)
        {
            return dict[name];
        }

        public bool GetBool(string name)
        {
            return bool.Parse(dict[name]);
        }

        public int GetInt(string name)
        {
            return int.Parse(dict[name]);
        }

        public float GetFloat(string name)
        {
            return float.Parse(dict[name]);
        }

        public double GetDouble(string name)
        {
            return double.Parse(dict[name]);
        }

        public string this[string name]
        {
            get
            {
                return dict[name];
            }
            set
            {
                dict[name] = value;
            }
        }

        public void Set<T>(string name, T obj)
        {
            dict[name] = obj.ToString();
        }
    }
}
