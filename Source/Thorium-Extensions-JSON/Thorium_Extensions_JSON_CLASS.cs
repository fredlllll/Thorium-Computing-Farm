using System;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Thorium_Reflection;

public static class Thorium_Extensions_JSON_CLASS
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

    public static object ConvertTo<T>(this T token, Type u) where T : JToken
    {
        if(token == null)
        {
            return ReflectionHelper.GetDefault(u);
        }
        if(u.IsAssignableFrom(token.GetType()) && u != typeof(IComparable) && u != typeof(IFormattable))
        {
            return token;
        }
        JValue jValue = token as JValue;
        if(jValue == null)
        {
            throw new InvalidCastException(String.Format(CultureInfo.InvariantCulture, "Cannot cast {0} to {1}.", token.GetType(), typeof(T)));
        }
        if(u.IsAssignableFrom(jValue.Value.GetType()))
        {
            return jValue.Value;
        }
        if(ReflectionHelper.IsNullableType(u))
        {
            if(jValue.Value == null)
            {
                return ReflectionHelper.GetDefault(u);
            }
            u = Nullable.GetUnderlyingType(u);
        }
        return System.Convert.ChangeType(jValue.Value, u, CultureInfo.InvariantCulture);
    }

    public static object Get(this JObject jobj, Type t, string key, object def = null)
    {
        var token = jobj[key];

        if(token != null && !token.IsNull())
        {
            return token.ConvertTo(t);
        }
        return def;
    }

    public static bool HasValue(this JObject jobj, string key)
    {
        var token = jobj[key];
        return token != null;
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
