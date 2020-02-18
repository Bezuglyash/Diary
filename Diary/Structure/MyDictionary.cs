using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Diary.Structure
{
    [XmlRoot("Dictionary")]
    public class MyDictionary<TKey, TValue>
    {
        [XmlElement(ElementName = "Keys")]
        public List<TKey> keys;
        [XmlElement(ElementName = "Values")]
        public List<TValue> values;

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

        public TValue this[TKey key]
        {
            get => values[keys.LastIndexOf(key)];
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