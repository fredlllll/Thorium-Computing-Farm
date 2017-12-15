using System.Collections.Generic;

namespace System.Collections.Concurrent
{
    public class ConcurrentList<T> : IEnumerable<T>
    {
        ConcurrentDictionary<T, T> dict = new ConcurrentDictionary<T, T>();

        public int Count => dict.Count;

        public void Add(T item)
        {
            dict[item] = item;
        }

        public void Remove(T item)
        {
            dict.TryRemove(item, out item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            var enumerator = dict.GetEnumerator();
            while(enumerator.MoveNext())
            {
                yield return enumerator.Current.Key;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            dict.Clear();
        }
    }
}
