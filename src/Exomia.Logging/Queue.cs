#region License

// Copyright (c) 2018-2019, exomia
// All rights reserved.
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

#endregion

using System;
using System.Runtime.CompilerServices;

namespace Exomia.Logging
{
    sealed class Queue<T>
    {
        private const int DEFAULT_CAPACITY = 8;
        private const int MAX_CAPACITY     = 0X7FEFFFFF;

        private T[] _array;
        private int _head;
        private int _tail;
        private int _count;

        /// <summary> Gets the number of. </summary>
        /// <value> The count. </value>
        public int Count
        {
            get { return _count; }
        }

        public Queue(int capacity)
        {
            if (capacity < 0) { throw new ArgumentOutOfRangeException(nameof(capacity)); }

            _array = new T[capacity];
            _head  = 0;
            _tail  = 0;
            _count = 0;
        }

        public Queue(Queue<T> other)
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }

            _array = new T[other._array.Length];

            if (other._count > 0)
            {
                if (other._head < other._tail)
                {
                    Array.Copy(other._array, other._head, _array, 0, other._count);
                }
                else
                {
                    Array.Copy(other._array, _head, _array, 0, other._array.Length - other._head);
                    Array.Copy(other._array, 0, _array, other._array.Length        - other._head, other._tail);
                }
            }

            _count = other._count;
            _head  = 0;
            _tail  = _count == _array.Length ? 0 : _count;
        }

        public void Enqueue(T item)
        {
            EnsureCapacity(_count + 1);

            _array[_tail] = item;
            _tail         = (_tail + 1) % _array.Length;
            _count++;
        }

        public T Dequeue()
        {
            if (_count == 0) { throw new IndexOutOfRangeException(); }

            T removed = _array[_head];
            _array[_head] = default!;
            _head         = (_head + 1) % _array.Length;
            _count--;

            return removed;
        }

        public void Clear()
        {
            if (_head < _tail)
            {
                Array.Clear(_array, _head, _count);
            }
            else
            {
                Array.Clear(_array, _head, _array.Length - _head);
                Array.Clear(_array, 0, _tail);
            }

            _head  = 0;
            _tail  = 0;
            _count = 0;
        }

        public void Clear(Queue<T> other)
        {
            if (_array.Length < other._count)
            {
                _array = new T[other._count];
            }
            else
            {
                Array.Clear(_array, other._count, _array.Length - other._count);
            }

            if (other._count > 0)
            {
                if (other._head < other._tail)
                {
                    Array.Copy(other._array, other._head, _array, 0, other._count);
                }
                else
                {
                    Array.Copy(other._array, _head, _array, 0, other._array.Length - other._head);
                    Array.Copy(other._array, 0, _array, other._array.Length        - other._head, other._tail);
                }
            }

            _count = other._count;
            _head  = 0;
            _tail  = _count == _array.Length ? 0 : _count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureCapacity(int min)
        {
            if (_array.Length < min)
            {
                int newCapacity = _array.Length == 0 ? DEFAULT_CAPACITY : _array.Length * 2;
                if (newCapacity > MAX_CAPACITY) { newCapacity = MAX_CAPACITY; }
                if (newCapacity < min) { throw new OutOfMemoryException(); }

                T[] buffer = new T[newCapacity];
                if (_head < _tail)
                {
                    Array.Copy(_array, _head, buffer, 0, _count);
                }
                else
                {
                    Array.Copy(_array, _head, buffer, 0, _array.Length - _head);
                    Array.Copy(_array, 0, buffer, _array.Length        - _head, _tail);
                }
                _array = buffer;
                _head  = 0;
                _tail  = _count == newCapacity ? 0 : _count;
            }
        }
    }
}