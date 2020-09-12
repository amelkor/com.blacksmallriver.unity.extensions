using UnityEngine;

namespace Bsr.Unity.Extensions
{
    /// <summary>
    /// Contains position and rotation.
    /// </summary>
    public readonly struct TransformOrientation
    {
        public readonly Vector3 position;
        public readonly Quaternion rotation;

        public TransformOrientation(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }
}