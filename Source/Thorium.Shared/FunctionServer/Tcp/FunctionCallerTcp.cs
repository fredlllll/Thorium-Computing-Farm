using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thorium.Shared.Aether;
using Thorium.Shared.Messages;

namespace Thorium.Shared.FunctionServer.Tcp
{
    public class FunctionCallerTcp
    {
        protected int callIdCounter = 0;
        protected readonly Dictionary<int, AutoResetEvent> answerEvents = [];
        protected readonly Dictionary<int, FunctionCallAnswer> answers = [];
        protected readonly AetherStream aether;

        public FunctionCallerTcp(AetherStream aether)
        {
            this.aether = aether;
        }


        protected int GetNextCallId()
        {
            callIdCounter++;
            return callIdCounter;
        }

        public void Stop()
        {
            lock (answers)
            {
                answers.Clear();
            }
            lock (answerEvents)
            {
                foreach (var kv in answerEvents)
                {
                    kv.Value.Dispose();
                }
                answerEvents.Clear();
            }
        }

        public void HandleFunctionCallAnswer(FunctionCallAnswer answer)
        {
            lock (answerEvents)
            {
                int answerId = answer.Id;
                if (answerEvents.TryGetValue(answerId, out var answerEvent))
                {
                    lock (answers)
                    {
                        answers[answerId] = answer;
                    }
                    answerEvent.Set();
                }
            }
        }

        public T RemoteFunctionCall<T>(string functionName, bool needsAnswer, int timeoutMs = 5000, params object[] args)
        {
            int id = GetNextCallId();

            var call = new FunctionCall
            {
                Id = id,
                FunctionName = functionName,
                NeedsAnwer = needsAnswer,
                FunctionArguments = args
            };

            if (needsAnswer)
            {
                var answerEvent = new AutoResetEvent(false);
                lock (answerEvents)
                {
                    answerEvents[id] = answerEvent;
                }

                aether.Write(call);

                var start = DateTime.UtcNow;
                try
                {
                    while (!answerEvent.WaitOne(100, true))
                    {
                        if ((DateTime.UtcNow - start).TotalMilliseconds > timeoutMs)
                        {
                            throw new TimeoutException();
                        }
                        //wait for answer to arrive
                    }
                }
                catch (ObjectDisposedException)
                {
                    //happens when connection is lost
                    throw new TimeoutException();
                }
                finally
                {
                    lock (answerEvents)
                    {
                        answerEvents.Remove(id);
                    }
                    answerEvent.Dispose();
                }
                FunctionCallAnswer answer;
                lock (answers)
                {
                    answer = answers[id];
                    answers.Remove(id);
                }
                if (answer.Exception != null)
                {
                    throw new Exception(answer.Exception);
                }
                return (T)answer.ReturnValue;
            }
            else
            {
                aether.Write(call);
            }
            return default;
        }
    }
}
