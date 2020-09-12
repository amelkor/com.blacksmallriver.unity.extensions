using System;
using UnityEngine;

namespace Bsr.Unity.Extensions.Unity
{
    public static class TransformExtensions
    {
        public static TransformOrientation GetOrientation(this Transform transform, Vector3 positionCorrection = default, Vector3 angleCorrection = default)
        {
            return new TransformOrientation(transform.position + positionCorrection, transform.rotation * Quaternion.Euler(angleCorrection));
        }

        public static Transform GetFirstChildByName(this GameObject obj, string name, bool includeInactive = false)
        {
            return GetFirstChildByName(obj.transform, name);
        }

        public static Transform GetFirstChildByName(this Transform t, string name, bool includeInactive = false)
        {
            int i = 0;
            while (i < t.childCount)
            {
                var c = t.GetChild(i);
                if (c.name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return c;
                i++;
            }

            return default;
        }

        public static Transform GetChildByNameInHierarchy(this Transform t, string name, bool includeInactive = false)
        {
            int i = 0;
            while (i < t.childCount)
            {
                var c = t.GetChild(i);
                if (c.name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return c;

                var child = GetChildByNameInHierarchy(c, name, includeInactive);
                if (child)
                    return child;
                
                i++;
            }

            return default;
        }

        public static void SetPositionAndRotation(this Transform t, Transform transform)
        {
            t.SetPositionAndRotation(transform.position, transform.rotation);
        }
        
        public static void SetPositionAndRotation(this Transform t, TransformOrientation orientation)
        {
            t.SetPositionAndRotation(orientation.position, orientation.rotation);
        }
    }
}