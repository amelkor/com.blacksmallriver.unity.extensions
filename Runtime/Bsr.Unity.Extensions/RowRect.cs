using UnityEngine;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ConvertToAutoPropertyWhenPossible

namespace Bsr.Unity.Extensions
{
    /// <summary>
    /// Creates a set <see cref="Rect"/> aligned in row (with height offset) one next after each other.
    /// </summary>
    public struct RowRect
    {
        public readonly float X;
        public readonly float Y;
        public readonly float Width;
        public readonly float Height;
        public readonly float Offset;
        public int Rows => _rows;

        private int _rows;
        private float _y;

        /// <summary>
        /// Creates <see cref="Rect"/> with incremented offset by height.
        /// </summary>
        public RowRect(float x, float y, float width, float height, float offset)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Offset = offset;

            _rows = 0;
            _y = y;
        }

        public Rect NextRow()
        {
            _rows++;
            return new Rect(X, _y += Y + Offset, Width, Height);
        }
    }
}