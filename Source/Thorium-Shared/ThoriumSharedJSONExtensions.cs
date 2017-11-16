using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;


public static class ThoriumSharedJSONExtensions
{
    public static T Get<T>(this JObject jobj, string key, T def = default(T))
    {
        var token = jobj[key];
        if(token != null && !token.IsNull())
        {
            return token.Value<T>();
        }
        return def;
    }

    public static bool IsNull(this JToken token)
    {
        return token.Type == JTokenType.Null;
    }

    public static bool IsNullOrEmpty(this JToken token)
    {
        return (token.Type == JTokenType.Array && !token.HasValues) ||
               (token.Type == JTokenType.Object && !token.HasValues) ||
               (token.Type == JTokenType.String && token.ToString() == String.Empty) ||
               (token.Type == JTokenType.Null);
    }
}

