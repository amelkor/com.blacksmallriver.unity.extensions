using System;

namespace Bsr.Unity.Extensions
{
    public static class ArrayExtensions
    {
        public static bool IsEmpty<T>(this Array array)
        {
            return array == Array.Empty<T>();
        }
    }
}