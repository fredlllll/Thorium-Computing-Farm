using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Codolith.Config
{
    [DataContract]
    public class Config
    {
        [DataMember]
        Dictionary<string, string> dict = new Dictionary<string, string>();
        [IgnoreDataMember] //ignore as it could be confidential
        public string Filename { get; protected set; }
        [DataMember]
        public ConfigType ConfigType { get; protected set; }

        public Config(string filename = default(string), ConfigType configType = ConfigType.XML)
        {
            Filename = filename;
            ConfigType = configType;
            Load();
        }

        void Load(string filename = default(string))
        {
            if(filename == default(string))
            {
                filename = Filename;
            }

            if(File.Exists(filename))
            {
                using(FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    Type parserType = GetParserType(ConfigType);
                    if(parserType == null)
                    {
                        throw new ArgumentException("no parser available for " + ConfigType);
                    }

                    IConfigParser parser = Activator.CreateInstance(parserType) as IConfigParser;
                    if(parser == null)
                    {
                        throw new Exception("the type " + parserType + " has to be convertible to " + nameof(IConfigParser));
                    }
                    parser.Stream = fs;

                    parser.FillDictionary(dict);
                }
            }
        }

        public string Get(string key, string defaultValue = null)
        {
            string retval;
            if(dict.TryGetValue(key, out retval))
            {
                return retval;
            }
            return defaultValue;
        }

        public T Get<T>(string key, T defaultValue = default(T))
        {
            T retval = defaultValue;
            string val;
            if(dict.TryGetValue(key, out val))
            {
                retval = (T)Convert.ChangeType(val, typeof(T), System.Globalization.CultureInfo.InvariantCulture);
            }
            return retval;
        }

        public void Set(string key, string value)
        {
            dict[key] = value;
        }

        public void Set<T>(string key, T value)
        {
            if(value == null)
            {
                dict.Remove(key);
            }
            else
            {
                if(value is IConvertible)
                {
                    dict[key] = ((IConvertible)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
                if(value is IFormattable)
                {
                    dict[key] = ((IFormattable)value).ToString(null, System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    dict[key] = value.ToString();
                }
            }
        }

        public void Save(string filename = default(string))
        {
            if(filename == default(string))
            {
                filename = Filename;
            }

            if(filename != null)
            {
                using(FileStream fs = new FileStream(filename, FileMode.Truncate))
                {
                    Type saverType = GetSaverType(ConfigType);
                    if(saverType == null)
                    {
                        throw new ArgumentException("no saver available for " + ConfigType);
                    }

                    IConfigSaver saver = Activator.CreateInstance(saverType) as IConfigSaver;
                    if(saver == null)
                    {
                        throw new Exception("the type " + saverType + " has to be convertible to " + nameof(IConfigSaver));
                    }

                    saver.SaveDictionary(dict);
                }
            }
        }

        #region static
        static Dictionary<ConfigType, Type> parserTypes = new Dictionary<ConfigType, Type>(),
            saverTypes = new Dictionary<ConfigType, Type>();

        static Config()
        {
            SetParserType(ConfigType.XML, typeof(XMLConfigParser));
            SetParserType(ConfigType.INI, typeof(INIConfigParser));
        }

        public static void SetParserType(ConfigType ctype, Type t)
        {
            parserTypes[ctype] = t;
        }

        public static Type GetParserType(ConfigType ctype)
        {
            Type retval;
            parserTypes.TryGetValue(ctype, out retval);
            return retval;
        }

        public static void UnsetParserType(ConfigType ctype)
        {
            parserTypes.Remove(ctype);
        }

        public static void SetSaverType(ConfigType ctype, Type t)
        {
            saverTypes[ctype] = t;
        }

        public static Type GetSaverType(ConfigType ctype)
        {
            Type retval;
            saverTypes.TryGetValue(ctype, out retval);
            return retval;
        }

        public static void UnsetSaverType(ConfigType ctype)
        {
            saverTypes.Remove(ctype);
        }
        #endregion
    }
}
