using System;
using System.Reflection;
using System.Text;

namespace Bsr.Unity.Extensions.Reflection
{
    public static class ObjectExtensions
    {
        public static string FieldsToString(this object obj)
        {
            var sb = new StringBuilder();
            var fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            if (fields.Length == 0)
                return string.Empty;

            for (var i = 0; i < fields.Length; i++)
            {
                sb.Append(fields[i].Name).Append(": ").Append(fields[i].GetValue(obj).ToString()).Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public static bool TryGetFirstGenericType(this object obj, out Type type)
        {
            var typeArguments = obj.GetType().GenericTypeArguments;
            if (typeArguments.Length > 0)
            {
                type = typeArguments[0];
                return true;
            }

            type = default;
            return false;
        }
    }
}