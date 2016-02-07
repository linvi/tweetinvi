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
    }
}