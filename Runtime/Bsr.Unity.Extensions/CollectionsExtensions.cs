using System.Collections.Generic;

namespace Bsr.Unity.Extensions
{
    public static class CollectionsExtensions
    {
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            if (dict.TryGetValue(key, out var value))
                return value;
            
            throw new KeyNotFoundException();
        }
    }
}