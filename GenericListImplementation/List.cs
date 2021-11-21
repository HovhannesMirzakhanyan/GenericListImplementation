using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericListImplementation
{
    public class List<T>
    {
        private const int _defaultCapacity = 4;
        private T[] _items;
        private int _size;
        private int _version;

        static readonly T[] _emptyArray = new T[0];
        public List()
        {
            _items = _emptyArray;
        }
        public List(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException();
            if (capacity == 0)
                _items = _emptyArray;
            else
                _items = new T[capacity];
        }
        public int Capacity
        {
            get
            {

                return _items.Length;
            }
            set
            {
                if (value < _size)
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (value != _items.Length)
                {
                    if (value > 0)
                    {
                        T[] _newItems = new T[value];
                        if (_size > 0)
                        {
                            Array.Copy(_items, 0, _newItems, 0, _size);
                        }
                        _items = _newItems;
                    }
                    else
                    {
                        _items = _emptyArray;
                    }
                }

            }
        }
        public override string ToString()
        {
            return $"Count={_size}";
        }
        public int Count
        {
            get
            {
                return _size;
            }
        }

        public T this[int index]
        {
            get
            {
                if (index > _size)
                {
                    throw new ArgumentOutOfRangeException();
                }
                return _items[index];
            }
            set
            {
                if ((uint)index > (uint)_size)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _items[index] = value;
                _version++;
            }
        }

        public void Add(T item)
        {
            if (_size == _items.Length) EnsureCapacity(_size + 1);
            _items[_size++] = item;
            _version++;
        }
        private void EnsureCapacity(int min)
        {
            if (_items.Length < min)
            {
                int newCapacity = _items.Length;
                if (_items.Length == 0)
                {
                    newCapacity = _defaultCapacity;
                }
                else
                {
                    newCapacity = _items.Length * 2;
                }
                if ((uint)newCapacity > 0X7FEFFFFF)
                    newCapacity = 0X7FEFFFFF;
                if (newCapacity < min)
                    newCapacity = min;
                Capacity = newCapacity;
            }
        }

        public void Clear()
        {
            if (_size > 0)
            {
                Array.Clear(_items, 0, _size);
                _size = 0;
            }
            _version++;
        }

        public bool Contains(string item)
        {
            if ((Object)item == null)
            {
                for (int i = 0; i < _size; i++)
                    if ((Object)_items[i] == null)
                        return true;
                return false;
            }
            else
            {

                for (int i = 0; i < _size; i++)
                {
                    if (int.Equals(_items[i], item)) return true;
                }
            }
            return false;
        }
        public void CopyTo(int[] array)
        {
            Array.Copy(_items, array, 0);
        }

        private void CopyTo(int index, int[] array, int arrayIndex, int count)
        {
            if (_size - index < count)
            {
                throw new ArgumentOutOfRangeException();
            }
            Array.Copy(_items, index, array, arrayIndex, count);
        }
        public int IndexOf(T item)
        {

            return Array.IndexOf(_items, item, 0, _size);
        }
        public int IndexOf(T item, int index)
        {
            if (index > _size)
                throw new ArgumentOutOfRangeException();

            return Array.IndexOf(_items, item, index, _size - index);
        }

        public int IndexOf(T item, int index, int count)
        {
            if (index > _size)
                throw new ArgumentOutOfRangeException();

            if (count < 0 || index > _size - count) throw new ArgumentOutOfRangeException();
            return Array.IndexOf(_items, item, index, count);
        }
        public void Insert(int index, T item)
        {

            if ((uint)index > (uint)_size)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (_size == _items.Length) EnsureCapacity(_size + 1);
            if (index < _size)
            {
                Array.Copy(_items, index, _items, index + 1, _size - index);
            }
            _items[index] = item;
            _size++;
            _version++;
        }
        public void RemoveAt(int index)
        {
            if ((uint)index >= (uint)_size)
            {
                throw new ArgumentOutOfRangeException();
            }

            _size--;
            if (index < _size)
            {
                Array.Copy(_items, index + 1, _items, index, _size - index);
            }
            _items[_size] = default(T);
            _version++;
        }
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }
        public _ListEnumerator GetEnumerator()
        {
            return new _ListEnumerator(_items, _size);
        }
        public class _ListEnumerator
        {
            private T[] _items;
            private int _size;
            private int _count;
            public _ListEnumerator(T[] items, int size)
            {
                _items = items;
                _size = size;
            }
            public object Current
            {
                get
                {
                    return _items[_count++];
                }
            }
            public bool MoveNext()
            {
                return _count < _size;
            }

        }
    }
}
