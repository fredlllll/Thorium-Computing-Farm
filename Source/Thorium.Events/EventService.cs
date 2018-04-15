using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Thorium_Events
{
    public class EventService
    {
        public delegate void EvHandler(JToken arg);

        private readonly IEventQueueProvider queue;

        private object lockObj = new object(); // to prevent stray events from being fired

        Dictionary<string, List<EvHandler>> subscriptions = new Dictionary<string, List<EvHandler>>();

        public EventService(IEventQueueProvider queue)
        {
            this.queue = queue;
        }

        public void Dispatch(string ev, JToken arg)
        {
            var q = queue.GetQueue(ev);
            q.Send(arg);
        }

        private int GetSubCountForQueue(string queue)
        {
            lock(lockObj)
            {
                if(!subscriptions.TryGetValue(queue, out List<EvHandler> handlers))
                {
                    return 0;
                }
                return handlers.Count;
            }
        }

        private void AddSubscription(string queue, EvHandler handler)
        {
            if(!subscriptions.TryGetValue(queue, out List<EvHandler> handlers))
            {
                handlers = new List<EvHandler>();
                subscriptions[queue] = handlers;
            }

            handlers.Add(handler);
        }

        /// <summary>
        /// removes handler from queue and returns count of remaining handlers for said queue
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        private int RemoveSubscription(string queue, EvHandler handler)
        {
            if(subscriptions.TryGetValue(queue, out List<EvHandler> handlers))
            {
                handlers.Remove(handler);
                if(handlers.Count == 0)
                {
                    subscriptions.Remove(queue);
                }
                return handlers.Count;
            }
            return 0;
        }

        public void Subscribe(string queueName, EvHandler handler)
        {
            lock(lockObj)
            {
                var q = queue.GetQueue(queueName);
                if(GetSubCountForQueue(queueName) == 0)
                {
                    q.AttachReceiver();
                    q.EventReceivedHandler = Queue_EventReceived;
                }
                AddSubscription(queueName, handler);
            }
        }

        private void Queue_EventReceived(IEventQueue sender, JToken arg)
        {
            lock(lockObj)
            {
                if(subscriptions.TryGetValue(sender.Name, out List<EvHandler> handlers))
                {
                    foreach(var h in handlers)
                    {
                        h(arg);//TODO: handle errors?
                    }
                }
                else
                {
                    throw new Exception("no handler exist for queue " + sender.Name + " but an event was received. this shouldnt happen. check yo code");
                }
            }
        }

        public void Unsubscribe(string ev, EvHandler handler)
        {
            lock(lockObj)
            {
                var q = queue.GetQueue(ev);
                if(RemoveSubscription(ev, handler) == 0)
                {
                    q.DetachReceiver();
                    q.EventReceivedHandler = null;
                }
            }
        }
    }
}
