using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MapleGame.Core
{
    public class ZDictionary<TKey,TValue>
    {
        private ConcurrentDictionary<TKey, TValue> m_dictionary;

        public ZDictionary()
        {
            m_dictionary = new ConcurrentDictionary<TKey, TValue>();
        }


        public bool Add(TKey key, TValue value)
        {
            return m_dictionary.TryAdd(key, value);
        }

        public TValue Get(TKey key)
        {
            TValue value;
            m_dictionary.TryGetValue(key, out value);
            return value;
        }

        public void ForEach(Action<KeyValuePair<TKey, TValue>,object> action,object state)
        {
            foreach (var kvp in m_dictionary)
            {
                action(kvp, state);
            }
        }

        public bool Remove(TKey key)
        {
            TValue value;
            return m_dictionary.TryRemove(key, out value);
        }

        public void Clear()
        {
            m_dictionary.Clear();
        }
    }
}
