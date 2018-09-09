using System;
using System.Collections;
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

        public static T GetOrDefault<T>(this NameValueCollection nameValueCollection, string keyName, T defaultValue = default(T))
        {
            if (nameValueCollection.AllKeys.Contains(keyName))
            {
                try
                {
                    return (T)Convert.ChangeType(nameValueCollection[keyName], typeof(T));
                }
                catch
                {
                }
            }

            return defaultValue;
        }

        public static T GetValueOrDefault<T>(this Hashtable hashtable, object key, T defaultValue = default(T))
            where T : class
        {
            if (!hashtable.ContainsKey(key))
            {
                return defaultValue;
            }

            try
            {
                return (T)hashtable[key];
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}
