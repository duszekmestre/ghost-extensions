using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Ghost.Extensions.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key, TValue defaultValue = default(TValue))
        {
            return source?.ContainsKey(key) == true ? source[key] : defaultValue;
        }

        public static T GetOrDefault<T>(this NameValueCollection configurationManager, string keyName, T defaultValue = default(T))
        {
            if (configurationManager.AllKeys.Contains(keyName))
            {
                try
                {
                    return (T)Convert.ChangeType(configurationManager[keyName], typeof(T));
                }
                catch
                {
                }
            }

            return defaultValue;
        }
    }
}
