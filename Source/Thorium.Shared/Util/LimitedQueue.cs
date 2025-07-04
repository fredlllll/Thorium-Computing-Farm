using System.Collections.Generic;

namespace Thorium.Shared.Util
{
    /// <summary>
    /// Queue that throws away from the end of the list once it grows too large
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LimitedQueue<T> : Queue<T>
    {
        public int Limit { get; set; }

        public LimitedQueue(int limit) : base(limit)
        {
            Limit = limit;
        }

        public new void Enqueue(T item)
        {
            base.Enqueue(item);
            while (Count > Limit)
            {
                Dequeue();
            }
        }
    }
}
