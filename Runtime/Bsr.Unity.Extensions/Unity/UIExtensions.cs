using UnityEngine;

namespace Bsr.Unity.Extensions.Unity
{
    public static class UIExtensions
    {
        public static RectTransform RectTransform(this Canvas canvas) => (RectTransform) canvas.transform;
    }
}