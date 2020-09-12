using System.Runtime.CompilerServices;
using UnityEngine;

namespace Bsr.Unity.Extensions
{
    public static class MathHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Vector3ToIndex(Vector3 dimentions, Vector3 pos)
        {
            return ((int) pos.z * (int) dimentions.x * (int) dimentions.y) + ((int) pos.y * (int) dimentions.z) + (int) pos.x;
        }

        public static Vector3 IndexToVector3(Vector3 dimentions, int idx)
        {
            // ReSharper disable SuggestVarOrType_BuiltInTypes
            int z = idx / (int) (dimentions.x * dimentions.y);
            idx -= (int) (z * dimentions.x * dimentions.y);
            int y = (int) (idx / dimentions.x);
            int x = (int) (idx % dimentions.x);
            // ReSharper restore SuggestVarOrType_BuiltInTypes

            return new Vector3(x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Vector2Half()
        {
            return new Vector2(0.5f, 0.5f);
        }
    }
}