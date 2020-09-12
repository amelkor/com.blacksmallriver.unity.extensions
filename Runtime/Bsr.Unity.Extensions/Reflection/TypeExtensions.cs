using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// ReSharper disable LoopCanBeConvertedToQuery

namespace Bsr.Unity.Extensions.Reflection
{
    public static class TypeExtensions
    {
        private const int FATAL_EXIT_CODE = -1;

        public static Type[] GetConstructorArgumentsTypes(this Type type, Type ofType = null)
        {
            var ctors = type.GetConstructors();
            if (ctors.Length == 1)
                return CheckCtor(ctors[0]);

            for (var i = 0; i < ctors.Length; i++)
            {
                var types = CheckCtor(ctors[i]);
                if (types != Array.Empty<Type>())
                    return types;
            }

            return Array.Empty<Type>();


            Type[] CheckCtor(MethodBase ctorInfo)
            {
                try
                {
                    var ctorParams = ctorInfo.GetParameters();
                    if (ctorParams.Length == 0)
                        return Array.Empty<Type>();

                    var foundPartTypes = new List<Type>(ctorParams.Length);
                    for (var i = 0; i < ctorParams.Length; i++)
                    {
                        var paramType = ctorParams[i].ParameterType;
                        if (ofType == null)
                        {
                            foundPartTypes.Add(paramType);
                            continue;
                        }

                        if (ofType.IsInterface)
                        {
                            if (paramType.IsImplementsInterface(ofType))
                                foundPartTypes.Add(paramType);
                        }
                        else
                        {
                            if (paramType.IsAssignableFrom(ofType))
                                foundPartTypes.Add(paramType);
                        }
                    }

                    if (foundPartTypes.Count == 0)
                        return Array.Empty<Type>();

                    return foundPartTypes.ToArray();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to get constructor arguments. {e.Message}");
                    Application.Quit(FATAL_EXIT_CODE);
                    return null;
                }
            }
        }

        /// <summary>
        /// Returns true if type implements provided interface type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static bool IsImplementsInterface(this Type type, Type interfaceType)
        {
            if (type == null)
                return false;

            while (true)
            {
                if (!interfaceType.IsInterface)
                    return false;

                if (type == null)
                    return false;

                var interfaces = type.GetInterfaces();
                if (interfaces.Length == 0)
                {
                    var baseType = type.BaseType;
                    type = baseType;
                    continue;
                }

                for (var i = 0; i < interfaces.Length; i++)
                {
                    if (interfaces[i] == interfaceType)
                        return true;
                }

                return false;
            }
        }
    }
}