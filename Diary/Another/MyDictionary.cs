using System.Collections.Generic;

namespace Diary.Another
{
    class MyDictionary <TKey, TValue>
    {
        private List<TKey> keys;
        private List<TValue> values;

        public MyDictionary()
        {
            keys = new List<TKey>();
            values = new List<TValue>();
        }

        public MyDictionary(TKey key, TValue value)
        {
            keys = new List<TKey>() { key };
            values = new List<TValue>() { value };
        }

        public void Add(TKey key, TValue value)
        {
            keys.Add(key);
            values.Add(value);
        }

        public TValue this [TKey key]
        {
            get { return values[keys.LastIndexOf(key)]; }
            set
            {
                values[keys.LastIndexOf(key)] = value;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for (int i = 0; i < keys.Count; i++)
            {
                yield return new KeyValuePair<TKey, TValue>(keys[i], values[i]);
            }
        }
    }
}