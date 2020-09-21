using System.Collections;
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

        public static void Add<TKey, TValue>(this IDictionary<TKey, IList> dictionary, (TKey key, TValue value) tuple) where TValue : class
        {
            var (key, value) = tuple;

            if (value == null)
                return;

            dictionary[key].Add(value);
        }

        public static void ToArray<T>(this IList list, ref T[] array)
        {
            if (array == null)
                array = new T[list.Count];

            list.CopyTo(array, 0);
        }
    }
}