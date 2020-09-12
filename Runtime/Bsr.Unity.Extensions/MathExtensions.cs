using UnityEngine;

namespace Bsr.Unity.Extensions
{
    public static class MathExtensions
    {
        public static float HorizontalMagnitude(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.z).magnitude;
        }
    }
}