using System;
using Newtonsoft.Json.Linq;

namespace Thorium_Shared.ServicePoint
{
    public class Routine
    {
        public string Name { get; }
        public Func<JToken, JToken> Handler { get; }

        public Routine(string name, Func<JToken, JToken> handler)
        {
            Name = name;
            Handler = handler;
        }

        public JToken Invoke(JToken arg)
        {
            return Handler.Invoke(arg);
        }

        public static explicit operator Routine(Func<JToken, JToken> func)
        {
            return new Routine(func.Method.Name, func);
        }
    }
}
