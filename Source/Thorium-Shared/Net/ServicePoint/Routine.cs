using System;
using Newtonsoft.Json.Linq;

namespace Thorium_Shared.Net.ServicePoint
{
    public delegate JToken RoutineHandler(JToken arg);

    public class Routine
    {
        public string Name { get; }
        public RoutineHandler Handler { get; }

        public Routine(string name, RoutineHandler handler)
        {
            Name = name;
            Handler = handler;
        }

        public JToken Invoke(JToken arg)
        {
            return Handler.Invoke(arg);
        }

        public static explicit operator Routine(RoutineHandler func)
        {
            return new Routine(func.Method.Name, func);
        }
    }
}
