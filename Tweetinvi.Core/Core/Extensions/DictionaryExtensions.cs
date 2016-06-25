using System.Collections.Generic;

namespace Tweetinvi.Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool TryAdd<T1, T2>(this Dictionary<T1, T2> dictionary, T1 key, T2 value)
        {
            if (dictionary.ContainsKey(key))
            {
                return false;
            }

            dictionary.Add(key, value);

            return true;
        }

        public static void AddOrUpdate<T1, T2>(this Dictionary<T1, T2> dictionary, KeyValuePair<T1, T2> keyValuePair)
        {
            dictionary.AddOrUpdate(keyValuePair.Key, keyValuePair.Value);
        }

        public static void AddOrUpdate<T1, T2>(this Dictionary<T1, T2> dictionary, T1 key, T2 value)
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
    }
}