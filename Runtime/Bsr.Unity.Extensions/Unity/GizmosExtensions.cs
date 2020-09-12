using UnityEditor;
using UnityEngine;

namespace Bsr.Unity.Extensions.Unity
{
    public static class GizmosExtensions
    {
#if UNITY_EDITOR
        public static void DrawCharacterControllerGizmos(this GameObject gameObject)
        {
            if (gameObject.SelectedInEditor())
                return;

#if UNITY_2019_OR_NEWER
            if (gameObject.TryGetComponent<CharacterController>(out var cc))
#else
            var cc = gameObject.GetComponent<CharacterController>();
            // ReSharper disable once InvertIf
            if (cc != null)
#endif
            {
                if (cc.height - cc.radius < cc.height / 2)
                    Gizmos.DrawWireSphere(gameObject.transform.position, cc.radius);
                else
                    DrawCapsuleGizmos(gameObject.transform.position + Vector3.up * cc.center.y, cc.height, cc.radius, Color.yellow);
            }
        }

        public static void DrawCapsuleGizmos(Vector3 pos, float height, float radius, Color color)
        {
            DrawCapsuleGizmos(pos, Quaternion.identity, height, radius, color);
        }

        public static void DrawCapsuleGizmos(Vector3 pos, Quaternion rot, float height, float radius, Color color)
        {
            var angleMatrix = Matrix4x4.TRS(pos, rot, Handles.matrix.lossyScale);
            using (new Handles.DrawingScope(color, angleMatrix))
            {
                Gizmos.color = color;
                var pointOffset = (height - (radius * 2)) / 2;

                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, radius);
                Handles.DrawLine(new Vector3(0, pointOffset, -radius), new Vector3(0, -pointOffset, -radius));
                Handles.DrawLine(new Vector3(0, pointOffset, radius), new Vector3(0, -pointOffset, radius));
                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, radius);

                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, radius);
                Handles.DrawLine(new Vector3(-radius, pointOffset, 0), new Vector3(-radius, -pointOffset, 0));
                Handles.DrawLine(new Vector3(radius, pointOffset, 0), new Vector3(radius, -pointOffset, 0));
                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, radius);

                Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, radius);
                Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, radius);
            }
        }

        public static void DrawSphereCast(Vector3 position, Vector3 direction, float radius, float distance, Color color = default, Color castColor = default)
        {
            var positionCast = position + (direction * distance);

            Gizmos.color = color == default ? Colors.White : color;
            Gizmos.DrawWireSphere(position, radius);


            Gizmos.color = castColor == default ? Colors.Gray : castColor;
            Gizmos.DrawWireSphere(positionCast, radius);

            var angleMatrix1 = Matrix4x4.TRS(position, Quaternion.LookRotation(positionCast - position), Handles.matrix.lossyScale);

            using (new Handles.DrawingScope(angleMatrix1))
            {
                const float space = 1.5f;
                DrawDottedLine(radius, 0f, distance, space);
                DrawDottedLine(-radius, 0f, distance, space);
                DrawDottedLine(0f, radius, distance, space);
                DrawDottedLine(0f, -radius, distance, space);
            }
        }

        public static void DrawCapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, float distance, Color color = default, Color castColor = default)
        {
            Handles.color = color == default ? Colors.White : color;

            var forward = point2 - point1;
            var rotation = Quaternion.LookRotation(forward);
            var pointOffset = radius / 2f;
            var length = forward.magnitude;
            var center2 = new Vector3(0f, 0, length);

            var angleMatrix1 = Matrix4x4.TRS(point1, rotation, Handles.matrix.lossyScale);
            var angleMatrix2 = Matrix4x4.TRS(point1 + direction * distance, rotation, Handles.matrix.lossyScale);

            //draw first capsule
            using (new Handles.DrawingScope(angleMatrix1))
            {
                Handles.DrawWireDisc(Vector3.zero, Vector3.forward, radius);
                Handles.DrawWireArc(Vector3.zero, Vector3.up, Vector3.left * pointOffset, -180f, radius);
                Handles.DrawWireArc(Vector3.zero, Vector3.left, Vector3.down * pointOffset, -180f, radius);
                Handles.DrawWireDisc(center2, Vector3.forward, radius);
                Handles.DrawWireArc(center2, Vector3.up, Vector3.right * pointOffset, -180f, radius);
                Handles.DrawWireArc(center2, Vector3.left, Vector3.up * pointOffset, -180f, radius);

                DrawLine(radius, 0f, length);
                DrawLine(-radius, 0f, length);
                DrawLine(0f, radius, length);
                DrawLine(0f, -radius, length);
            }

            Handles.color = castColor == default ? Colors.White : castColor;

            //draw second capsule
            using (new Handles.DrawingScope(angleMatrix2))
            {
                Handles.DrawWireDisc(Vector3.zero, Vector3.forward, radius);
                Handles.DrawWireArc(Vector3.zero, Vector3.up, Vector3.left * pointOffset, -180f, radius);
                Handles.DrawWireArc(Vector3.zero, Vector3.left, Vector3.down * pointOffset, -180f, radius);
                Handles.DrawWireDisc(center2, Vector3.forward, radius);
                Handles.DrawWireArc(center2, Vector3.up, Vector3.right * pointOffset, -180f, radius);
                Handles.DrawWireArc(center2, Vector3.left, Vector3.up * pointOffset, -180f, radius);

                const float lineSpace = 0.05f;
                DrawDottedLine(radius, 0f, length, lineSpace);
                DrawDottedLine(-radius, 0f, length, lineSpace);
                DrawDottedLine(0f, radius, length, lineSpace);
                DrawDottedLine(0f, -radius, length, lineSpace);
            }
        }

        public static void DrawWireCapsule(Vector3 point1, Vector3 point2, float radius, Color color = default)
        {
            if (color != default)
                Handles.color = color;

            var forward = point2 - point1;
            var rotation = Quaternion.LookRotation(forward);
            var pointOffset = radius / 2f;
            var length = forward.magnitude;
            var center2 = new Vector3(0f, 0, length);

            var angleMatrix = Matrix4x4.TRS(point1, rotation, Handles.matrix.lossyScale);

            using (new Handles.DrawingScope(angleMatrix))
            {
                Handles.DrawWireDisc(Vector3.zero, Vector3.forward, radius);
                Handles.DrawWireArc(Vector3.zero, Vector3.up, Vector3.left * pointOffset, -180f, radius);
                Handles.DrawWireArc(Vector3.zero, Vector3.left, Vector3.down * pointOffset, -180f, radius);
                Handles.DrawWireDisc(center2, Vector3.forward, radius);
                Handles.DrawWireArc(center2, Vector3.up, Vector3.right * pointOffset, -180f, radius);
                Handles.DrawWireArc(center2, Vector3.left, Vector3.up * pointOffset, -180f, radius);

                DrawLine(radius, 0f, length);
                DrawLine(-radius, 0f, length);
                DrawLine(0f, radius, length);
                DrawLine(0f, -radius, length);
            }
        }

        private static void DrawLine(float arg1, float arg2, float forward)
        {
            Handles.DrawLine(new Vector3(arg1, arg2, 0f), new Vector3(arg1, arg2, forward));
        }

        private static void DrawDottedLine(float arg1, float arg2, float forward, float space)
        {
            Handles.DrawDottedLine(new Vector3(arg1, arg2, 0f), new Vector3(arg1, arg2, forward), space);
        }
#endif
    }
}