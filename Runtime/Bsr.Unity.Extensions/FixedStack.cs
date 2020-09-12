using System;
using System.Collections.Generic;

namespace Bsr.Unity.Extensions
{
    /// <summary>
    ///  Represents a fixed size last-in-first-out (LIFO) collection of instances of the same specified type <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FixedStack<T>
    {
        /// <summary>
        /// Stack size.
        /// </summary>
        private readonly int _size;

        /// <summary>
        /// Top element index.
        /// </summary>
        private int _top;

        /// <summary>
        /// Internal array of <see cref="T"/>.
        /// </summary>
        private readonly T[] _array;
        
        public int Size => _size;
        
        /// <summary>
        /// Amount of elements currently in stack.
        /// </summary>
        public int Count => _top + 1;
        
        /// <summary>
        /// Free space available.
        /// </summary>
        public int FreeCount => _size - (_top + 1);

        public bool IsEmpty => Count == 0;
        public bool IsFull => FreeCount == 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedStack{T}" /> class that is empty and has the provided capacity.
        /// </summary>
        /// <param name="size">Stack fixed size. Must be greater than 0.</param>
        /// <exception cref="ArgumentException">The size is less or equals 0.</exception>
        public FixedStack(int size)
        {
            if (size <= 0)
                throw new ArgumentException("Can not create empty stack. The size is less or equals 0.");
            _size = size;
            _array = new T[_size];
            _top = -1;
        }

        public FixedStack(T[] values)
        {
            if (values.Length <= 0)
                throw new ArgumentException("Can not create empty stack. The size is less or equals 0.");
            _size = values.Length;
            _array = new T[_size];
            values.CopyTo(_array, 0);
            _top = _size - 1;
        }

        /// <summary>
        /// Inserts an object at the top of the <see cref="FixedStack{T}" />.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Push(T value)
        {
            if (_size - (_top + 1) <= 0)
            {
                return false;
            }

            _array[++_top] = value;
            
            return true;
        }

        /// <summary>
        /// Inserts an array of objects at the top of the <see cref="FixedStack{T}" />.
        /// Returns array of items which do not fit due to stack free space limit.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public T[] Push(T[] values)
        {

            if (_top >= _size)
            {
                return values;
            }

            var i = 0;
            while (_top < _size && i < values.Length)
                _array[++_top] = values[i++];

            if (i == values.Length)
            {
                return Array.Empty<T>();
            }

            var arr = new T[values.Length - i];
            Array.Copy(values, i, arr, 0, arr.Length);

            return arr;
        }

        /// <summary>
        /// Removes and returns the object at the top of the <see cref="FixedStack{T}" />.
        /// </summary>
        /// <returns></returns>
        public bool Pop(out T obj)
        {
            if (_top < 0)
            {
                obj = default;

                return false;
            }

            obj = _array[_top];
            _array[_top--] = default;
            
            return true;
        }

        /// <summary>
        /// Returns the object at the top of the <see cref="FixedStack{T}" /> without removing it.
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            return _top < 0 ? default : _array[_top];
        }

        public void Clear()
        {
            Array.Clear(_array, 0, _size);
            _top = 0;
        }

        /// <summary>
        /// Copies the <see cref="FixedStack{T}" /> to a new array.
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            if (_top <= 0)
            {
                return default;
            }
            var arr = new T[_top];
            Array.Copy(_array, 0, arr, 0, _top);
            
            return arr;
        }

        /// <summary>
        /// Copies the <see cref="FixedStack{T}" /> to a FixedStack by reference.
        /// </summary>
        /// <param name="destinationStack"></param>
        /// <returns></returns>
        public bool CopyToFixedStack(ref FixedStack<T> destinationStack)
        {
            if (_top < 0)
            {
                return false;
            }
            
            var targetLength = destinationStack.Size < _top ? destinationStack.Size : _top;
            Array.Copy(_array, 0, destinationStack._array, 0, targetLength);
            
            return true;
        }

        /// <summary>
        /// Determines whether an element is in the <see cref="FixedStack{T}" />.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            var size = _size;
            var equalityComparer = EqualityComparer<T>.Default;
            while (size-- > 0)
            {
                // ReSharper disable once IsExpressionAlwaysTrue
                if (!(item is object))
                {
                    if (!(_array[size] is object))
                        return true;
                }
                else if (_array[size] is object && equalityComparer.Equals(_array[size], item))
                    return true;
            }

            return false;
        }
    }
}