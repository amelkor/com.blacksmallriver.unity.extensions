using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bsr.Unity.Extensions.Unity
{
    #region auxiliary stuff

    /// <summary>
    /// Debug label appearing on the left of the screen column.
    /// </summary>
    internal struct DebugLabelRow
    {
        public const float X = 5f;
        public const float Y = 20f;

        public readonly string Text;

        private readonly float _duration;
        private float _timeLeft;

        public DebugLabelRow(string text, float duration = 10f)
        {
            Text = text;

            _duration = duration;
            _timeLeft = Time.time + _duration;
        }

        public bool Expired => Time.time >= _timeLeft;

        public void Draw(int order)
        {
            // ReSharper disable once PossibleLossOfFraction
            var position = new Rect(X, Y * order, Screen.width - X * 2, Y);

            if (position.y >= Screen.height - Y)
            {
                //no room to draw more labels. Renew the time left and wait for the screen space to free.
                //trying to draw a label outside the screen causes Unity crush.
                _timeLeft = Time.time + _duration;
                return;
            }

            GUI.Label(position, Text);
        }
    }

    #endregion

    public static class DebugTools
    {
        private static readonly Queue<Action> _gizmosQueue = new Queue<Action>();
        private static readonly DualQueue<DebugLabelRow> _labelsQueue = new DualQueue<DebugLabelRow>();

        #region notify

        /// <summary>
        /// Print onscreen label with green text.
        /// </summary>
        public static void NotifyOk(string text, float duration = 5f)
        {
            if (!CanDraw)
                return;

            _labelsQueue.Current.Enqueue(new DebugLabelRow(RichText.Colored(text, Colors.CodeGreen), duration));
        }

        /// <summary>
        /// Print onscreen label with yellow text.
        /// </summary>
        public static void NotifyWarn(string text, float duration = 5f)
        {
            if (!CanDraw)
                return;

            _labelsQueue.Current.Enqueue(new DebugLabelRow(RichText.Colored(text, Colors.CodeYellow), duration));
        }

        /// <summary>
        /// Print onscreen label with red text.
        /// </summary>
        public static void NotifyError(string text, float duration = 5f)
        {
            if (!CanDraw)
                return;

            _labelsQueue.Current.Enqueue(new DebugLabelRow(RichText.Colored(text, Colors.CodeRed), duration));
        }

        /// <summary>
        /// Print onscreen label with gray text.
        /// </summary>
        public static void NotifyTrace(string text, float duration = 5f)
        {
            if (!CanDraw)
                return;

            _labelsQueue.Current.Enqueue(new DebugLabelRow(RichText.Colored(text, Colors.CodeGray), duration));
        }

        #endregion

#if UNITY_EDITOR

        public static void VisualiseSphereCast(Vector3 position, Vector3 direction, float radius, float distance, Color color = default, Color castColor = default)
        {
            _gizmosQueue.Enqueue(() => GizmosExtensions.DrawSphereCast(position, direction, radius, distance, color, castColor));
        }

        public static void VisualiseCapsuleCast(CapsulecastCommand cast, Color color = default, Color castColor = default)
        {
            _gizmosQueue.Enqueue(() => GizmosExtensions.DrawCapsuleCast(cast.point1, cast.point2, cast.radius, cast.direction, cast.distance, color, castColor));
        }

        public static void VisualiseCapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, float distance, Color color = default, Color castColor = default)
        {
            _gizmosQueue.Enqueue(() => GizmosExtensions.DrawCapsuleCast(point1, point2, radius, direction, distance, color, castColor));
        }

        public static void VisualiseCapsuleGizmos(CapsulecastCommand cast, Color color = default)
        {
            var offset = cast.direction * cast.distance;
            _gizmosQueue.Enqueue(() => GizmosExtensions.DrawWireCapsule(cast.point1 + offset, cast.point2 + offset, cast.radius, color));
        }

        public static void VisualizeDeselectedCharacterControllerGizmos(GameObject gameObject)
        {
            gameObject.DrawCharacterControllerGizmos();
        }
#endif

        internal static bool CanDraw;

        /// <summary>
        /// Draws debug tools GUI stuff.
        /// </summary>
        internal static void ExecuteOnGUI()
        {
            var index = 0;
            var queue = _labelsQueue.Current;
            var next = _labelsQueue.Next;
            while (queue.Count > 0)
            {
                var label = queue.Dequeue();

                label.Draw(index);

                if (!label.Expired)
                    next.Enqueue(label);

                index++;
            }

            if (next.Count > 0)
                _labelsQueue.SwitchQueues();
        }

        /// <summary>
        /// Draws debug tools Gizmos stuff.
        /// </summary>
        internal static void ExecuteOnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            try
            {
                while (_gizmosQueue.Count > 0)
                {
                    _gizmosQueue.Dequeue().Invoke();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Gizmos Drawer error: failed to draw gizmos");
                Debug.LogException(ex);
            }
        }
    }
}