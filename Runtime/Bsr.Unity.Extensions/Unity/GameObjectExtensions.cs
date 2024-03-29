﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

// ReSharper disable ForCanBeConvertedToForeach

namespace Bsr.Unity.Extensions.Unity
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Returns found components where key is <see cref="GameObject"/> name, value is <see cref="Component"/> itself.
        /// </summary>
        /// <typeparam name="T">Unity <see cref="Component"/> type.</typeparam>
        /// <param name="obj">This game object</param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public static Dictionary<string, T> GetComponentsInChildrenAsNameDictionary<T>(this Behaviour obj, bool includeInactive = false) where T : Component
        {
            var components = obj.GetComponentsInChildren<T>(includeInactive);
            if (components.Length == 0)
                return null;
            var dict = new Dictionary<string, T>(components.Length);
            for (var i = 0; i < components.Length; i++)
            {
                if (dict.ContainsKey(components[i].name))
                    dict.Add(components[i].name + components[i].GetInstanceID().ToString(), components[i]);
                else
                    dict.Add(components[i].name, components[i]);
            }

            return dict;
        }

        public static bool TryGetComponentInChildren<T>(this Behaviour obj, out T component, bool includeInactive = false) where T : Component
        {
            var c = obj.GetComponentInChildren<T>(includeInactive);
            if (c)
            {
                component = c;
                return true;
            }

            component = default;
            return false;
        }

        public static bool TryGetComponentInChildrenWithName<T>(this Behaviour obj, string childName, out T component, bool includeInactive = true) where T : Component
        {
            var components = obj.GetComponentsInChildren<T>(includeInactive);
            if (components.Length == 0)
            {
                component = default;
                return false;
            }

            for (var i = 0; i < components.Length; i++)
            {
                // ReSharper disable once InvertIf
                if (components[i].name.Equals(childName, StringComparison.InvariantCulture))
                {
                    component = components[i];
                    return true;
                }
            }

            component = default;
            return false;
        }

        /// <summary>
        /// Similar to TryGetComponent but throws exception if component missed. Use this for initialization.
        /// </summary>
        /// <exception cref="MissingComponentException">when failed to get component.</exception>
        public static T EnsureHasComponent<T>(this Behaviour behaviour) where T : Component
        {
#if UNITY_2019_3_OR_NEWER
            if (behaviour.TryGetComponent(typeof(T), out var component))
                return (T) component;
#else
            var component = behaviour.GetComponent<T>();
            if (component != null)
                return component;
#endif
            throw new MissingComponentException($"Requested component {typeof(T).Name} not exists");
        }

        public static T EnsureHasComponentInParent<T>(this Behaviour behaviour) where T : Component
        {
            var component = behaviour.GetComponentInParent(typeof(T));

            if (component == null)
                throw new MissingComponentException($"Requested component {typeof(T).Name} not exists in any parent of this gameobject");

            return (T) component;
        }

        public static T EnsureHasComponentInChildren<T>(this Behaviour behaviour) where T : Component
        {
            var component = behaviour.GetComponentInChildren(typeof(T), true);

            if (component == null)
                throw new MissingComponentException($"Requested component {typeof(T).Name} not exists in any parent of this gameobject");

            return (T) component;
        }

        public static T EnsureHasComponentInChildren<T>(this Behaviour behaviour, string name) where T : Component
        {
            if (!behaviour.TryGetComponentInChildrenWithName<T>(name, out var component, true))
                throw new MissingComponentException($"Requested component {typeof(T).Name} not exists in any parent of this gameobject");

            return (T) component;
        }

        public static void SetParent(this GameObject gameObject, GameObject parent, bool worldPositionStays = false)
        {
            gameObject.transform.SetParent(parent.transform, worldPositionStays);
        }

        public static void SetLayer(this GameObject gameObject, string layerName, bool withChildren)
        {
            var layer = LayerMask.NameToLayer(layerName);

            if (!withChildren)
            {
                gameObject.layer = layer;
                return;
            }

            SetLayerRecursively(gameObject.transform, layer);
        }

        private static void SetLayerRecursively(Transform transform, LayerMask layer)
        {
            transform.gameObject.layer = layer;

            for (var i = 0; i < transform.childCount; i++)
            {
                SetLayerRecursively(transform.GetChild(i), layer);
            }
        }

        /// <summary>
        /// Destroys the <paramref name="gameObject"/> depending on is running in UnityEditor or not.
        /// </summary>
        /// <param name="gameObject">GameObject to destroy.</param>
        public static void DestroyMe(this Behaviour gameObject)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                Object.Destroy(gameObject);
            }
            else
            {
                Object.DestroyImmediate(gameObject);
            }
#else
            Object.Destroy(gameObject);
#endif
        }
    }
}