using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codolith.Serialization
{
    class BidirectionalDict<T, U> : IEnumerable<KeyValuePair<T, U>>
    {
        Dictionary<T, U> TtoU = new Dictionary<T, U>();
        Dictionary<U, T> UtoT = new Dictionary<U, T>();

        public void Add(T key, U value)
        {
            this[key] = value;
        }

        public int Count
        {
            get { return TtoU.Count; }
        }

        public U Get(T key)
        {
            return TtoU[key];
        }

        public T Get(U key)
        {
            return UtoT[key];
        }

        public U this[T key]
        {
            get
            {
                return TtoU[key];
            }
            set
            {
                TtoU[key] = value;
                UtoT[value] = key;
            }
        }

        public T this[U key]
        {
            get
            {
                return UtoT[key];
            }
            set
            {
                UtoT[key] = value;
                TtoU[value] = key;
            }
        }

        public bool TryGetValue(T key, out U value)
        {
            return TtoU.TryGetValue(key, out value);
        }

        public bool TryGetValue(U key, out T value)
        {
            return UtoT.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<T, U>> GetEnumerator()
        {
            return TtoU.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return TtoU.GetEnumerator();
        }
    }
}
