using System;
using System.Collections.Generic;

namespace SimpleDiscord.Extensions
{
#nullable enable
    public static class DictionaryExtension
    {
        public static TValue? ForceGet<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary is null)
                throw new ArgumentNullException(nameof(dictionary));

            if (dictionary.TryGetValue(key, out TValue value))
                return value;
            return default;
        }
    }
}
