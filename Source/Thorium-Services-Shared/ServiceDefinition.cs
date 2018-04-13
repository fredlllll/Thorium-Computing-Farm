using System;
using Newtonsoft.Json.Linq;
using Thorium_Extensions_JSON;
using Thorium_Utils;

namespace Thorium_Services_Shared
{
    public class ServiceDefinition : IJSONConvertable
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public ServiceInterfaceDefinition[] InterfaceDefinitions { get; private set; }

        public ServiceDefinition()
        {

        }

        public ServiceDefinition(string name, ServiceInterfaceDefinition[] interfaceDefinitions)
        {
            Id = Utils.GetRandomGUID();
            Name = name;
            InterfaceDefinitions = interfaceDefinitions;
        }

        public ServiceDefinition(string id, string name, ServiceInterfaceDefinition[] interfaceDefinitions)
        {
            Id = id;
            Name = name;
            InterfaceDefinitions = interfaceDefinitions;
        }

        public void FromJSON(JToken json)
        {
            if(json is JObject jo)
            {
                Id = jo.Get<string>("id");
                Name = jo.Get<string>("name");
                JArray interfaceDefinitions = jo["interfaceDefinitions"] as JArray;

                InterfaceDefinitions = new ServiceInterfaceDefinition[interfaceDefinitions.Count];
                for(int i = 0; i < interfaceDefinitions.Count; i++)
                {
                    var id = new ServiceInterfaceDefinition();
                    InterfaceDefinitions[i] = id;
                    id.FromJSON(interfaceDefinitions[i]);
                }
            }
            else
            {
                throw new ArgumentException("i expected a jobject");
            }
        }

        public JToken ToJSON()
        {
            JArray interfaceDefinitions = new JArray();
            foreach(var sid in InterfaceDefinitions)
            {
                interfaceDefinitions.Add(sid.ToJSON());
            }

            JObject jo = new JObject()
            {
                ["id"] = Id,
                ["name"] = Name,
                ["interfaceDefinitions"] = interfaceDefinitions
            };
            return jo;
        }
    }
}
