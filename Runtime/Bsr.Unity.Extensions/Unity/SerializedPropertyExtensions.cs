#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;

namespace Bsr.Unity.Extensions.Unity
{
    public static class SerializedPropertyExtensions
    {
        public static void SetFieldValue<T>(this SerializedProperty property, T value)
        {
            var type = property.serializedObject.targetObject.GetType();
            var field = type.GetField(property.propertyPath, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (field == null)
                throw new ArgumentNullException(property.propertyPath);

            field.SetValue(property.serializedObject.targetObject, value);
        }

        public static T GetFieldValue<T>(this SerializedProperty property)
        {
            var type = property.serializedObject.targetObject.GetType();
            var field = type.GetField(property.propertyPath, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (field == null)
                throw new ArgumentNullException(property.propertyPath);

            return (T) field.GetValue(property.serializedObject.targetObject);
        }
    }
}
#endif