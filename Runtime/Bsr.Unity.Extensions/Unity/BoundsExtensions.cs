using UnityEngine;

namespace Bsr.Unity.Extensions.Unity
{
    public static class BoundsExtensions
    {
        public static bool Contains(this BoundsInt bounds, Vector2Int position)
        {
            return bounds.Contains(new Vector3Int(position.x, position.y, 0));
        }
    }
}