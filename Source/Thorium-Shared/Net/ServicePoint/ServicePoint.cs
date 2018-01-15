using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;
using NLog;
using Thorium_Shared.Config;

namespace Thorium_Shared.Net.ServicePoint
{
    public class ServicePoint
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private bool started = false;

        private readonly Dictionary<string, Routine> routines = new Dictionary<string, Routine>();
        private readonly List<IServiceInvokationReceiver> invokationReceivers = new List<IServiceInvokationReceiver>();

        public ServicePoint(string configName = null)
        {
            if(configName != null)
            {
                var config = ConfigFile.GetConfig(configName);
                JArray invokationReceivers = config.InvokationReceivers;
                foreach(var val in invokationReceivers)
                {
                    if(val is JObject jo && jo.Get("load", false))
                    {
                        var type = jo.Get<string>("type");

                        Type t = ReflectionHelper.GetType(type);
                        if(t == null)
                        {
                            logger.Warn("Couldn't find type: " + type);
                            continue;
                        }
                        ConstructorInfo ci = null;
                        ci = t.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(string) }, null);
                        if(ci != null)
                        {
                            AddFromConstructor(ci, new object[] { jo.Get<string>("args") });
                        }
                        else
                        {
                            ci = t.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
                            if(ci == null)
                            {
                                logger.Warn("Couldn't find constructor for " + t.AssemblyQualifiedName + ". Provide either a default constructor or one that takes a string argument");
                                continue;
                            }
                            AddFromConstructor(ci, new object[0]);
                        }
                    }
                }

                JArray routineHandlers = config.RoutineHandlers;
                foreach(var val in routineHandlers)
                {
                    if(val is JObject jo && jo.Get("load", false))
                    {
                        var name = jo.Get<string>("name");
                        var handler = jo.Get<string>("handler");

                        int lastDot = handler.LastIndexOf('.');
                        string typeName = handler.Substring(0, lastDot);
                        string methodName = handler.Substring(lastDot + 1);
                        Type type = ReflectionHelper.GetType(typeName);

                        if(type == null)
                        {
                            logger.Warn("Couldn't find type: " + typeName);
                            continue;
                        }

                        MethodInfo mi = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[] { typeof(JToken) }, null);
                        if(mi == null)
                        {
                            logger.Warn("Couldn't find suitable method " + methodName + " that takes a JToken");
                            continue;
                        }
                        if(!mi.ReturnType.Equals(typeof(JToken)))
                        {
                            logger.Warn("The routine handler " + methodName + " has to return JToken");
                            continue;
                        }
                        RoutineHandler routineHandler = (RoutineHandler)Delegate.CreateDelegate(typeof(RoutineHandler), mi);
                        Routine routine = new Routine(name, routineHandler);
                        RegisterRoutine(routine);
                    }
                }
            }
        }

        private void AddFromConstructor(ConstructorInfo ci, object[] args)
        {
            try
            {
                object obj = ci.Invoke(args);
                if(obj is IServiceInvokationReceiver sir)
                {
                    RegisterInvokationReceiver(sir);
                }
                else
                {
                    logger.Warn(ci.ReflectedType.AssemblyQualifiedName + " Is not a " + nameof(IServiceInvokationReceiver));
                }
            }
            catch(TargetInvocationException ex)
            {
                logger.Error("Error thrown when creating service invokation receiver: " + ci.ReflectedType.AssemblyQualifiedName);
                logger.Error(ex);
            }
        }

        private void RequireNotStarted()
        {
            if(started)
            {
                throw new InvalidOperationException("Can't register things after start");
            }
        }

        public void RegisterRoutine(Routine routine)
        {
            RequireNotStarted();
            if(routines.ContainsKey(routine.Name))
            {
                throw new ArgumentException("There is already a routine named '" + routine.Name + "' registered");
            }
            routines[routine.Name] = routine;
        }

        public void RegisterInvokationReceiver(IServiceInvokationReceiver si)
        {
            invokationReceivers.Add(si);
        }

        public void Start()
        {
            if(started)
            {
                throw new InvalidOperationException("Can't start more than once");
            }

            foreach(var si in invokationReceivers)
            {
                si.InvokationReceived += HandleInvokationReceived;
                si.Start();
            }
        }

        public void Stop()
        {
            if(!started)
            {
                throw new InvalidOperationException("Can't stop before starting");
            }

            foreach(var si in invokationReceivers)
            {
                si.InvokationReceived -= HandleInvokationReceived;
                si.Stop();
            }
        }

        private InvokationResult HandleInvokationReceived(IServiceInvokationReceiver sender, string routine, JToken arg)
        {
            if(routines.TryGetValue(routine, out Routine r))
            {
                try
                {
                    JToken retval = r.Invoke(arg);
                    return new InvokationResult() { ReturnValue = retval };
                }
                catch(Exception ex)
                {
                    return new InvokationResult() { Exception = ex };
                }
            }
            return new InvokationResult() { Exception = new Exception("No routine named '" + routine + "' registered") };
        }
    }
}
