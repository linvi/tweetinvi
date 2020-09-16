using System.Collections.Generic;
using System.Linq;

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

        public static Dictionary<T1, T2> MergeWith<T1, T2>(this Dictionary<T1, T2> source, Dictionary<T1, T2> other)
        {
            var dictionaries = new[] { source, other };
            return dictionaries.SelectMany(dict => dict)
                   .ToLookup(pair => pair.Key, pair => pair.Value)
                   .ToDictionary(group => group.Key, group => group.First());
        }
    }
}