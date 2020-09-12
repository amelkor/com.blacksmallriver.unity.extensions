using System;
using System.Reflection;

namespace Bsr.Unity.Extensions.Reflection
{
    public static class DelegateExtensions
    {
        private static readonly MethodInfo _createDelegateInternal = typeof(Delegate).GetMethod(
            "CreateDelegate_internal",
            BindingFlags.NonPublic | BindingFlags.Static);

        private static readonly MethodInfo _combineDelegateInternal = typeof(Delegate).GetMethod(
            "CombineImpl",
            BindingFlags.NonPublic | BindingFlags.Instance,
            null,
            new[] {typeof(Delegate)},
            null);

        public static T CreateDelegateNoTypeCheck<T>(object firstArgument, MethodInfo method)
        {
            return (T) _createDelegateInternal.Invoke(null, new[] {typeof(T), firstArgument, method, false});
        }

        public static T CombineDelegateNoTypeCheck<T>(Delegate @delegate, Delegate follow)
        {
            return (T) _combineDelegateInternal.Invoke(@delegate, new object[] {follow});
        }
    }
}