using System;

namespace Bsr.Unity.Extensions
{
    public class Selector<T>
    {
        private struct Carret
        {
            private readonly int _length;

            public Carret(int length)
            {
                if (length <= 0)
                    throw new ArgumentException("Carret length must be greater than 0");
                _length = length;
                Index = 0;
            }

            public int Index { get; internal set; }

            public static Carret operator ++(Carret p)
            {
                var p1 = p.Index;
                if (p1 > p._length - 1)
                {
                    p.Index = 0;
                    return p;
                }

                p.Index++;
                return p;
            }

            public static Carret operator --(Carret p)
            {
                var p1 = p.Index;
                if (p1 < 0)
                {
                    p.Index = p._length - 1;
                    return p;
                }

                p.Index--;
                return p;
            }

            public static bool operator ==(Carret p1, Carret p2)
            {
                return p1.Index == p2.Index;
            }

            public static bool operator !=(Carret p1, Carret p2)
            {
                return p1.Index != p2.Index;
            }

            public override bool Equals(object obj)
            {
                return obj is Carret pos &&
                       _length == pos._length &&
                       Index == pos.Index;
            }

            public bool Equals(Carret other)
            {
                return _length == other._length && Index == other.Index;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (_length * 397) ^ Index;
                }
            }
        }

        private readonly T[] _array;
        private readonly int _length;
        private Carret _index;

        public Selector(T[] arr)
        {
            _array = arr;
            _length = _array.Length;
            _index = new Carret(_length);
        }

        /// <summary>
        /// Get <see cref="T"/> by index. The index starts from 1 not 0.
        /// </summary>
        /// <param name="index">Starts from 1.</param>
        public T this[int index]
        {
            get
            {
                if (index > _length || index < 0)
                    return default;

                _index.Index = index;
                return _array[_index.Index];
            }
        }

        public const int EmptyIndex = -1;

        public int Index => _index.Index;

        public T Current => _array[_index.Index];

        public T Next => _array[(++_index).Index];

        public T Prev => _array[(--_index).Index];

        public int NextFirst(out T element, Func<T, bool> condition)
        {
            var started = _index.Index;
            while ((++_index).Index != started)
            {
                // ReSharper disable once InvertIf
                if (condition.Invoke(_array[_index.Index]))
                {
                    element = _array[_index.Index];
                    return _index.Index;
                }
            }

            element = default;
            return EmptyIndex;
        }

        public int PrevFirst(out T element, Func<T, bool> condition)
        {
            var started = _index.Index;
            while ((--_index).Index != started)
            {
                // ReSharper disable once InvertIf
                if (condition.Invoke(_array[_index.Index]))
                {
                    element = _array[_index.Index];
                    return _index.Index;
                }
            }

            element = default;
            return EmptyIndex;
        }
    }
}