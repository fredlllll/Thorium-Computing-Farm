using System.Collections;
using System.Collections.Generic;

namespace Codolith.Serialization
{
    class MultiDictionary<T1, T2> : IEnumerable<KeyValuePair<T1, ModTuple<T1, T2>>>
    {
        Dictionary<T1, ModTuple<T1, T2>> T1AsKey = new Dictionary<T1, ModTuple<T1, T2>>();
        Dictionary<T2, ModTuple<T1, T2>> T2AsKey = new Dictionary<T2, ModTuple<T1, T2>>();
        public int Count { get { return T1AsKey.Count; } }
        public ModTuple<T1, T2> this[T1 key]
        {
            get { return T1AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
            }
        }
        public bool TryGetValue(T1 key, out ModTuple<T1, T2> outval)
        {
            return T1AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2> this[T2 key]
        {
            get { return T2AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
            }
        }
        public bool TryGetValue(T2 key, out ModTuple<T1, T2> outval)
        {
            return T2AsKey.TryGetValue(key, out outval);
        }
        public bool ContainsKey(T1 key)
        {
            return T1AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T2 key)
        {
            return T2AsKey.ContainsKey(key);
        }
        public IEnumerator<KeyValuePair<T1, ModTuple<T1, T2>>> GetEnumerator()
        {
            return T1AsKey.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return T1AsKey.GetEnumerator();
        }
    }
    class MultiDictionary<T1, T2, T3> : IEnumerable<KeyValuePair<T1, ModTuple<T1, T2, T3>>>
    {
        Dictionary<T1, ModTuple<T1, T2, T3>> T1AsKey = new Dictionary<T1, ModTuple<T1, T2, T3>>();
        Dictionary<T2, ModTuple<T1, T2, T3>> T2AsKey = new Dictionary<T2, ModTuple<T1, T2, T3>>();
        Dictionary<T3, ModTuple<T1, T2, T3>> T3AsKey = new Dictionary<T3, ModTuple<T1, T2, T3>>();
        public int Count { get { return T1AsKey.Count; } }
        public ModTuple<T1, T2, T3> this[T1 key]
        {
            get { return T1AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
            }
        }
        public bool TryGetValue(T1 key, out ModTuple<T1, T2, T3> outval)
        {
            return T1AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3> this[T2 key]
        {
            get { return T2AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
            }
        }
        public bool TryGetValue(T2 key, out ModTuple<T1, T2, T3> outval)
        {
            return T2AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3> this[T3 key]
        {
            get { return T3AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
            }
        }
        public bool TryGetValue(T3 key, out ModTuple<T1, T2, T3> outval)
        {
            return T3AsKey.TryGetValue(key, out outval);
        }
        public bool ContainsKey(T1 key)
        {
            return T1AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T2 key)
        {
            return T2AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T3 key)
        {
            return T3AsKey.ContainsKey(key);
        }
        public IEnumerator<KeyValuePair<T1, ModTuple<T1, T2, T3>>> GetEnumerator()
        {
            return T1AsKey.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return T1AsKey.GetEnumerator();
        }
    }
    class MultiDictionary<T1, T2, T3, T4> : IEnumerable<KeyValuePair<T1, ModTuple<T1, T2, T3, T4>>>
    {
        Dictionary<T1, ModTuple<T1, T2, T3, T4>> T1AsKey = new Dictionary<T1, ModTuple<T1, T2, T3, T4>>();
        Dictionary<T2, ModTuple<T1, T2, T3, T4>> T2AsKey = new Dictionary<T2, ModTuple<T1, T2, T3, T4>>();
        Dictionary<T3, ModTuple<T1, T2, T3, T4>> T3AsKey = new Dictionary<T3, ModTuple<T1, T2, T3, T4>>();
        Dictionary<T4, ModTuple<T1, T2, T3, T4>> T4AsKey = new Dictionary<T4, ModTuple<T1, T2, T3, T4>>();
        public int Count { get { return T1AsKey.Count; } }
        public ModTuple<T1, T2, T3, T4> this[T1 key]
        {
            get { return T1AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
            }
        }
        public bool TryGetValue(T1 key, out ModTuple<T1, T2, T3, T4> outval)
        {
            return T1AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4> this[T2 key]
        {
            get { return T2AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
            }
        }
        public bool TryGetValue(T2 key, out ModTuple<T1, T2, T3, T4> outval)
        {
            return T2AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4> this[T3 key]
        {
            get { return T3AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
            }
        }
        public bool TryGetValue(T3 key, out ModTuple<T1, T2, T3, T4> outval)
        {
            return T3AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4> this[T4 key]
        {
            get { return T4AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
            }
        }
        public bool TryGetValue(T4 key, out ModTuple<T1, T2, T3, T4> outval)
        {
            return T4AsKey.TryGetValue(key, out outval);
        }
        public bool ContainsKey(T1 key)
        {
            return T1AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T2 key)
        {
            return T2AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T3 key)
        {
            return T3AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T4 key)
        {
            return T4AsKey.ContainsKey(key);
        }
        public IEnumerator<KeyValuePair<T1, ModTuple<T1, T2, T3, T4>>> GetEnumerator()
        {
            return T1AsKey.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return T1AsKey.GetEnumerator();
        }
    }
    class MultiDictionary<T1, T2, T3, T4, T5> : IEnumerable<KeyValuePair<T1, ModTuple<T1, T2, T3, T4, T5>>>
    {
        Dictionary<T1, ModTuple<T1, T2, T3, T4, T5>> T1AsKey = new Dictionary<T1, ModTuple<T1, T2, T3, T4, T5>>();
        Dictionary<T2, ModTuple<T1, T2, T3, T4, T5>> T2AsKey = new Dictionary<T2, ModTuple<T1, T2, T3, T4, T5>>();
        Dictionary<T3, ModTuple<T1, T2, T3, T4, T5>> T3AsKey = new Dictionary<T3, ModTuple<T1, T2, T3, T4, T5>>();
        Dictionary<T4, ModTuple<T1, T2, T3, T4, T5>> T4AsKey = new Dictionary<T4, ModTuple<T1, T2, T3, T4, T5>>();
        Dictionary<T5, ModTuple<T1, T2, T3, T4, T5>> T5AsKey = new Dictionary<T5, ModTuple<T1, T2, T3, T4, T5>>();
        public int Count { get { return T1AsKey.Count; } }
        public ModTuple<T1, T2, T3, T4, T5> this[T1 key]
        {
            get { return T1AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
            }
        }
        public bool TryGetValue(T1 key, out ModTuple<T1, T2, T3, T4, T5> outval)
        {
            return T1AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5> this[T2 key]
        {
            get { return T2AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
            }
        }
        public bool TryGetValue(T2 key, out ModTuple<T1, T2, T3, T4, T5> outval)
        {
            return T2AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5> this[T3 key]
        {
            get { return T3AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
            }
        }
        public bool TryGetValue(T3 key, out ModTuple<T1, T2, T3, T4, T5> outval)
        {
            return T3AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5> this[T4 key]
        {
            get { return T4AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
            }
        }
        public bool TryGetValue(T4 key, out ModTuple<T1, T2, T3, T4, T5> outval)
        {
            return T4AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5> this[T5 key]
        {
            get { return T5AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
            }
        }
        public bool TryGetValue(T5 key, out ModTuple<T1, T2, T3, T4, T5> outval)
        {
            return T5AsKey.TryGetValue(key, out outval);
        }
        public bool ContainsKey(T1 key)
        {
            return T1AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T2 key)
        {
            return T2AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T3 key)
        {
            return T3AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T4 key)
        {
            return T4AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T5 key)
        {
            return T5AsKey.ContainsKey(key);
        }
        public IEnumerator<KeyValuePair<T1, ModTuple<T1, T2, T3, T4, T5>>> GetEnumerator()
        {
            return T1AsKey.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return T1AsKey.GetEnumerator();
        }
    }
    class MultiDictionary<T1, T2, T3, T4, T5, T6> : IEnumerable<KeyValuePair<T1, ModTuple<T1, T2, T3, T4, T5, T6>>>
    {
        Dictionary<T1, ModTuple<T1, T2, T3, T4, T5, T6>> T1AsKey = new Dictionary<T1, ModTuple<T1, T2, T3, T4, T5, T6>>();
        Dictionary<T2, ModTuple<T1, T2, T3, T4, T5, T6>> T2AsKey = new Dictionary<T2, ModTuple<T1, T2, T3, T4, T5, T6>>();
        Dictionary<T3, ModTuple<T1, T2, T3, T4, T5, T6>> T3AsKey = new Dictionary<T3, ModTuple<T1, T2, T3, T4, T5, T6>>();
        Dictionary<T4, ModTuple<T1, T2, T3, T4, T5, T6>> T4AsKey = new Dictionary<T4, ModTuple<T1, T2, T3, T4, T5, T6>>();
        Dictionary<T5, ModTuple<T1, T2, T3, T4, T5, T6>> T5AsKey = new Dictionary<T5, ModTuple<T1, T2, T3, T4, T5, T6>>();
        Dictionary<T6, ModTuple<T1, T2, T3, T4, T5, T6>> T6AsKey = new Dictionary<T6, ModTuple<T1, T2, T3, T4, T5, T6>>();
        public int Count { get { return T1AsKey.Count; } }
        public ModTuple<T1, T2, T3, T4, T5, T6> this[T1 key]
        {
            get { return T1AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
            }
        }
        public bool TryGetValue(T1 key, out ModTuple<T1, T2, T3, T4, T5, T6> outval)
        {
            return T1AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6> this[T2 key]
        {
            get { return T2AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
            }
        }
        public bool TryGetValue(T2 key, out ModTuple<T1, T2, T3, T4, T5, T6> outval)
        {
            return T2AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6> this[T3 key]
        {
            get { return T3AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
            }
        }
        public bool TryGetValue(T3 key, out ModTuple<T1, T2, T3, T4, T5, T6> outval)
        {
            return T3AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6> this[T4 key]
        {
            get { return T4AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
            }
        }
        public bool TryGetValue(T4 key, out ModTuple<T1, T2, T3, T4, T5, T6> outval)
        {
            return T4AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6> this[T5 key]
        {
            get { return T5AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
            }
        }
        public bool TryGetValue(T5 key, out ModTuple<T1, T2, T3, T4, T5, T6> outval)
        {
            return T5AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6> this[T6 key]
        {
            get { return T6AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
            }
        }
        public bool TryGetValue(T6 key, out ModTuple<T1, T2, T3, T4, T5, T6> outval)
        {
            return T6AsKey.TryGetValue(key, out outval);
        }
        public bool ContainsKey(T1 key)
        {
            return T1AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T2 key)
        {
            return T2AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T3 key)
        {
            return T3AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T4 key)
        {
            return T4AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T5 key)
        {
            return T5AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T6 key)
        {
            return T6AsKey.ContainsKey(key);
        }
        public IEnumerator<KeyValuePair<T1, ModTuple<T1, T2, T3, T4, T5, T6>>> GetEnumerator()
        {
            return T1AsKey.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return T1AsKey.GetEnumerator();
        }
    }
    class MultiDictionary<T1, T2, T3, T4, T5, T6, T7> : IEnumerable<KeyValuePair<T1, ModTuple<T1, T2, T3, T4, T5, T6, T7>>>
    {
        Dictionary<T1, ModTuple<T1, T2, T3, T4, T5, T6, T7>> T1AsKey = new Dictionary<T1, ModTuple<T1, T2, T3, T4, T5, T6, T7>>();
        Dictionary<T2, ModTuple<T1, T2, T3, T4, T5, T6, T7>> T2AsKey = new Dictionary<T2, ModTuple<T1, T2, T3, T4, T5, T6, T7>>();
        Dictionary<T3, ModTuple<T1, T2, T3, T4, T5, T6, T7>> T3AsKey = new Dictionary<T3, ModTuple<T1, T2, T3, T4, T5, T6, T7>>();
        Dictionary<T4, ModTuple<T1, T2, T3, T4, T5, T6, T7>> T4AsKey = new Dictionary<T4, ModTuple<T1, T2, T3, T4, T5, T6, T7>>();
        Dictionary<T5, ModTuple<T1, T2, T3, T4, T5, T6, T7>> T5AsKey = new Dictionary<T5, ModTuple<T1, T2, T3, T4, T5, T6, T7>>();
        Dictionary<T6, ModTuple<T1, T2, T3, T4, T5, T6, T7>> T6AsKey = new Dictionary<T6, ModTuple<T1, T2, T3, T4, T5, T6, T7>>();
        Dictionary<T7, ModTuple<T1, T2, T3, T4, T5, T6, T7>> T7AsKey = new Dictionary<T7, ModTuple<T1, T2, T3, T4, T5, T6, T7>>();
        public int Count { get { return T1AsKey.Count; } }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7> this[T1 key]
        {
            get { return T1AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
            }
        }
        public bool TryGetValue(T1 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7> outval)
        {
            return T1AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7> this[T2 key]
        {
            get { return T2AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
            }
        }
        public bool TryGetValue(T2 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7> outval)
        {
            return T2AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7> this[T3 key]
        {
            get { return T3AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
            }
        }
        public bool TryGetValue(T3 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7> outval)
        {
            return T3AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7> this[T4 key]
        {
            get { return T4AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
            }
        }
        public bool TryGetValue(T4 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7> outval)
        {
            return T4AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7> this[T5 key]
        {
            get { return T5AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
            }
        }
        public bool TryGetValue(T5 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7> outval)
        {
            return T5AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7> this[T6 key]
        {
            get { return T6AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
            }
        }
        public bool TryGetValue(T6 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7> outval)
        {
            return T6AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7> this[T7 key]
        {
            get { return T7AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
            }
        }
        public bool TryGetValue(T7 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7> outval)
        {
            return T7AsKey.TryGetValue(key, out outval);
        }
        public bool ContainsKey(T1 key)
        {
            return T1AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T2 key)
        {
            return T2AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T3 key)
        {
            return T3AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T4 key)
        {
            return T4AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T5 key)
        {
            return T5AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T6 key)
        {
            return T6AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T7 key)
        {
            return T7AsKey.ContainsKey(key);
        }
        public IEnumerator<KeyValuePair<T1, ModTuple<T1, T2, T3, T4, T5, T6, T7>>> GetEnumerator()
        {
            return T1AsKey.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return T1AsKey.GetEnumerator();
        }
    }
    class MultiDictionary<T1, T2, T3, T4, T5, T6, T7, T8> : IEnumerable<KeyValuePair<T1, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8>>>
    {
        Dictionary<T1, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8>> T1AsKey = new Dictionary<T1, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8>>();
        Dictionary<T2, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8>> T2AsKey = new Dictionary<T2, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8>>();
        Dictionary<T3, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8>> T3AsKey = new Dictionary<T3, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8>>();
        Dictionary<T4, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8>> T4AsKey = new Dictionary<T4, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8>>();
        Dictionary<T5, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8>> T5AsKey = new Dictionary<T5, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8>>();
        Dictionary<T6, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8>> T6AsKey = new Dictionary<T6, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8>>();
        Dictionary<T7, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8>> T7AsKey = new Dictionary<T7, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8>>();
        Dictionary<T8, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8>> T8AsKey = new Dictionary<T8, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8>>();
        public int Count { get { return T1AsKey.Count; } }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8> this[T1 key]
        {
            get { return T1AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
            }
        }
        public bool TryGetValue(T1 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8> outval)
        {
            return T1AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8> this[T2 key]
        {
            get { return T2AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
            }
        }
        public bool TryGetValue(T2 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8> outval)
        {
            return T2AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8> this[T3 key]
        {
            get { return T3AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
            }
        }
        public bool TryGetValue(T3 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8> outval)
        {
            return T3AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8> this[T4 key]
        {
            get { return T4AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
            }
        }
        public bool TryGetValue(T4 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8> outval)
        {
            return T4AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8> this[T5 key]
        {
            get { return T5AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
            }
        }
        public bool TryGetValue(T5 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8> outval)
        {
            return T5AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8> this[T6 key]
        {
            get { return T6AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
            }
        }
        public bool TryGetValue(T6 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8> outval)
        {
            return T6AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8> this[T7 key]
        {
            get { return T7AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
            }
        }
        public bool TryGetValue(T7 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8> outval)
        {
            return T7AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8> this[T8 key]
        {
            get { return T8AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
            }
        }
        public bool TryGetValue(T8 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8> outval)
        {
            return T8AsKey.TryGetValue(key, out outval);
        }
        public bool ContainsKey(T1 key)
        {
            return T1AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T2 key)
        {
            return T2AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T3 key)
        {
            return T3AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T4 key)
        {
            return T4AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T5 key)
        {
            return T5AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T6 key)
        {
            return T6AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T7 key)
        {
            return T7AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T8 key)
        {
            return T8AsKey.ContainsKey(key);
        }
        public IEnumerator<KeyValuePair<T1, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8>>> GetEnumerator()
        {
            return T1AsKey.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return T1AsKey.GetEnumerator();
        }
    }
    class MultiDictionary<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IEnumerable<KeyValuePair<T1, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>>>
    {
        Dictionary<T1, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>> T1AsKey = new Dictionary<T1, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>>();
        Dictionary<T2, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>> T2AsKey = new Dictionary<T2, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>>();
        Dictionary<T3, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>> T3AsKey = new Dictionary<T3, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>>();
        Dictionary<T4, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>> T4AsKey = new Dictionary<T4, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>>();
        Dictionary<T5, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>> T5AsKey = new Dictionary<T5, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>>();
        Dictionary<T6, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>> T6AsKey = new Dictionary<T6, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>>();
        Dictionary<T7, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>> T7AsKey = new Dictionary<T7, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>>();
        Dictionary<T8, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>> T8AsKey = new Dictionary<T8, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>>();
        Dictionary<T9, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>> T9AsKey = new Dictionary<T9, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>>();
        public int Count { get { return T1AsKey.Count; } }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> this[T1 key]
        {
            get { return T1AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
                T9AsKey[value.Value9] = value;
            }
        }
        public bool TryGetValue(T1 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> outval)
        {
            return T1AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> this[T2 key]
        {
            get { return T2AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
                T9AsKey[value.Value9] = value;
            }
        }
        public bool TryGetValue(T2 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> outval)
        {
            return T2AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> this[T3 key]
        {
            get { return T3AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
                T9AsKey[value.Value9] = value;
            }
        }
        public bool TryGetValue(T3 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> outval)
        {
            return T3AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> this[T4 key]
        {
            get { return T4AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
                T9AsKey[value.Value9] = value;
            }
        }
        public bool TryGetValue(T4 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> outval)
        {
            return T4AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> this[T5 key]
        {
            get { return T5AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
                T9AsKey[value.Value9] = value;
            }
        }
        public bool TryGetValue(T5 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> outval)
        {
            return T5AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> this[T6 key]
        {
            get { return T6AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
                T9AsKey[value.Value9] = value;
            }
        }
        public bool TryGetValue(T6 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> outval)
        {
            return T6AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> this[T7 key]
        {
            get { return T7AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
                T9AsKey[value.Value9] = value;
            }
        }
        public bool TryGetValue(T7 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> outval)
        {
            return T7AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> this[T8 key]
        {
            get { return T8AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
                T9AsKey[value.Value9] = value;
            }
        }
        public bool TryGetValue(T8 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> outval)
        {
            return T8AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> this[T9 key]
        {
            get { return T9AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
                T9AsKey[value.Value9] = value;
            }
        }
        public bool TryGetValue(T9 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> outval)
        {
            return T9AsKey.TryGetValue(key, out outval);
        }
        public bool ContainsKey(T1 key)
        {
            return T1AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T2 key)
        {
            return T2AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T3 key)
        {
            return T3AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T4 key)
        {
            return T4AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T5 key)
        {
            return T5AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T6 key)
        {
            return T6AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T7 key)
        {
            return T7AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T8 key)
        {
            return T8AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T9 key)
        {
            return T9AsKey.ContainsKey(key);
        }
        public IEnumerator<KeyValuePair<T1, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>>> GetEnumerator()
        {
            return T1AsKey.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return T1AsKey.GetEnumerator();
        }
    }
    class MultiDictionary<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IEnumerable<KeyValuePair<T1, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>>
    {
        Dictionary<T1, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> T1AsKey = new Dictionary<T1, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>();
        Dictionary<T2, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> T2AsKey = new Dictionary<T2, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>();
        Dictionary<T3, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> T3AsKey = new Dictionary<T3, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>();
        Dictionary<T4, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> T4AsKey = new Dictionary<T4, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>();
        Dictionary<T5, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> T5AsKey = new Dictionary<T5, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>();
        Dictionary<T6, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> T6AsKey = new Dictionary<T6, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>();
        Dictionary<T7, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> T7AsKey = new Dictionary<T7, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>();
        Dictionary<T8, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> T8AsKey = new Dictionary<T8, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>();
        Dictionary<T9, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> T9AsKey = new Dictionary<T9, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>();
        Dictionary<T10, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> T10AsKey = new Dictionary<T10, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>();
        public int Count { get { return T1AsKey.Count; } }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> this[T1 key]
        {
            get { return T1AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
                T9AsKey[value.Value9] = value;
                T10AsKey[value.Value10] = value;
            }
        }
        public bool TryGetValue(T1 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> outval)
        {
            return T1AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> this[T2 key]
        {
            get { return T2AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
                T9AsKey[value.Value9] = value;
                T10AsKey[value.Value10] = value;
            }
        }
        public bool TryGetValue(T2 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> outval)
        {
            return T2AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> this[T3 key]
        {
            get { return T3AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
                T9AsKey[value.Value9] = value;
                T10AsKey[value.Value10] = value;
            }
        }
        public bool TryGetValue(T3 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> outval)
        {
            return T3AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> this[T4 key]
        {
            get { return T4AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
                T9AsKey[value.Value9] = value;
                T10AsKey[value.Value10] = value;
            }
        }
        public bool TryGetValue(T4 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> outval)
        {
            return T4AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> this[T5 key]
        {
            get { return T5AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
                T9AsKey[value.Value9] = value;
                T10AsKey[value.Value10] = value;
            }
        }
        public bool TryGetValue(T5 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> outval)
        {
            return T5AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> this[T6 key]
        {
            get { return T6AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
                T9AsKey[value.Value9] = value;
                T10AsKey[value.Value10] = value;
            }
        }
        public bool TryGetValue(T6 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> outval)
        {
            return T6AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> this[T7 key]
        {
            get { return T7AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
                T9AsKey[value.Value9] = value;
                T10AsKey[value.Value10] = value;
            }
        }
        public bool TryGetValue(T7 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> outval)
        {
            return T7AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> this[T8 key]
        {
            get { return T8AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
                T9AsKey[value.Value9] = value;
                T10AsKey[value.Value10] = value;
            }
        }
        public bool TryGetValue(T8 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> outval)
        {
            return T8AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> this[T9 key]
        {
            get { return T9AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
                T9AsKey[value.Value9] = value;
                T10AsKey[value.Value10] = value;
            }
        }
        public bool TryGetValue(T9 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> outval)
        {
            return T9AsKey.TryGetValue(key, out outval);
        }
        public ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> this[T10 key]
        {
            get { return T10AsKey[key]; }
            set
            {
                T1AsKey[value.Value1] = value;
                T2AsKey[value.Value2] = value;
                T3AsKey[value.Value3] = value;
                T4AsKey[value.Value4] = value;
                T5AsKey[value.Value5] = value;
                T6AsKey[value.Value6] = value;
                T7AsKey[value.Value7] = value;
                T8AsKey[value.Value8] = value;
                T9AsKey[value.Value9] = value;
                T10AsKey[value.Value10] = value;
            }
        }
        public bool TryGetValue(T10 key, out ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> outval)
        {
            return T10AsKey.TryGetValue(key, out outval);
        }
        public bool ContainsKey(T1 key)
        {
            return T1AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T2 key)
        {
            return T2AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T3 key)
        {
            return T3AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T4 key)
        {
            return T4AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T5 key)
        {
            return T5AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T6 key)
        {
            return T6AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T7 key)
        {
            return T7AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T8 key)
        {
            return T8AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T9 key)
        {
            return T9AsKey.ContainsKey(key);
        }
        public bool ContainsKey(T10 key)
        {
            return T10AsKey.ContainsKey(key);
        }
        public IEnumerator<KeyValuePair<T1, ModTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>> GetEnumerator()
        {
            return T1AsKey.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return T1AsKey.GetEnumerator();
        }
    }
}

