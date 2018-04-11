using System;
using Newtonsoft.Json.Linq;
using Thorium_Extensions_JSON;
using Thorium_Utils;

namespace Thorium_Services_Shared
{
    public class ServiceInterfaceDefinition : IJSONConvertable
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public JObject InterfaceInfo { get; private set; }

        public ServiceInterfaceDefinition() { }

        public ServiceInterfaceDefinition(string name, JObject interfaceInfo)
        {
            Id = Utils.GetRandomGUID();
            Name = name;
            InterfaceInfo = interfaceInfo;
        }

        public ServiceInterfaceDefinition(string id, string name, JObject interfaceInfo)
        {
            Id = id;
            Name = name;
            InterfaceInfo = interfaceInfo;
        }

        public void FromJSON(JToken json)
        {
            if(json is JObject jo)
            {
                Id = jo.Get<string>("id");
                Name = jo.Get<string>("name");
                InterfaceInfo = (JObject)jo["interfaceInfo"];
            }
            else
            {
                throw new ArgumentException("im expecting a jobject");
            }
        }

        public JToken ToJSON()
        {
            JObject jo = new JObject
            {
                ["id"] = Id,
                ["name"] = Name,
                ["interfaceInfo"] = InterfaceInfo
            };

            return jo;
        }
    }
}
