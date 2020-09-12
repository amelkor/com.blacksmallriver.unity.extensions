using UnityEngine;

namespace Bsr.Unity.Extensions.Unity
{
    public static class RectTransformExtensions
    {
        public static void ResetPosition(this RectTransform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.sizeDelta = Vector2.zero;
            transform.anchoredPosition = Vector2.zero;
        }
    }
}