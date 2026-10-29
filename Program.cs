using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab4
{
    public class MyDynamicArray<T> : IEnumerable<T>
    {
        private T[] _items;
        private int _count;

        public int Count => _count;
        public int Capacity => _items.Length;

        public MyDynamicArray(int initialCapacity = 4)
        {
            if (initialCapacity < 1) initialCapacity = 4;
            _items = new T[initialCapacity];
            _count = 0;
        }

        public void Add(T item)
        {
            if (_count == _items.Length)
            {
                Resize(_items.Length * 2);
            }
            _items[_count++] = item;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count) throw new IndexOutOfRangeException("Індекс за межами діапазону!");
                return _items[index];
            }
            set
            {
                if (index < 0 || index >= _count) throw new IndexOutOfRangeException("Індекс за межами діапазону!");
                _items[index] = value;
            }
        }

        public T PopBack()
        {
            if (_count == 0) throw new InvalidOperationException("Масив порожній!");
            T item = _items[--_count];
            _items[_count] = default!;
            return item;
        }

        public void InsertAt(int index, T item)
        {
            if (index < 0 || index > _count) throw new IndexOutOfRangeException("Індекс за межами діапазону!");
            
            if (_count == _items.Length)
            {
                Resize(_items.Length * 2);
            }

            for (int i = _count; i > index; i--)
            {
                _items[i] = _items[i - 1];
            }
            _items[index] = item;
            _count++;
        }

        private void Resize(int newCapacity)
        {
            Console.WriteLine($"  ⚡ [RESIZE] Збільшення місткості: {_items.Length} ➔ {newCapacity} (Елементів: {_count})");
            T[] newArray = new T[newCapacity];
            Array.Copy(_items, newArray, _count);
            _items = newArray;
        }

        // ВАРІАНТ 1: Видалення довільного елемента
        public T RemoveAt(int index)
        {
            if (index < 0 || index >= _count) throw new IndexOutOfRangeException("Індекс за межами діапазону!");
            
            T removedItem = _items[index];
            
            for (int i = index; i < _count - 1; i++)
            {
                _items[i] = _items[i + 1];
            }
            
            _count--;
            _items[_count] = default!;

            // ВАРІАНТ 2: Зменшення ємності
            if (_count <= _items.Length / 4 && _items.Length > 4)
            {
                Shrink();
            }

            return removedItem;
        }

        // ВАРІАНТ 2: Зменшення ємності
        private void Shrink()
        {
            int newCapacity = _items.Length / 2;
            if (newCapacity < 4) newCapacity = 4;
            
            Console.WriteLine($"  📉 [SHRINK] Зменшення місткості: {_items.Length} ➔ {newCapacity} (Елементів: {_count})");
            T[] newArray = new T[newCapacity];
            Array.Copy(_items, newArray, _count);
            _items = newArray;
        }

        // ВАРІАНТ 3: Реалізація foreach
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
            {
                yield return _items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Print()
        {
            Console.Write($"[Count={_count}, Capacity={_items.Length}]: [ ");
            for (int i = 0; i < _count; i++) 
            {
                Console.Write($"{_items[i]} ");
            }
            Console.WriteLine("]");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            MyDynamicArray<int> vec = new MyDynamicArray<int>(initialCapacity: 2);
            
            for (int i = 1; i <= 10; i++)
            {
                vec.Add(i * 10);
            }
            vec.Print();

            vec.RemoveAt(3);
            vec.Print();

            foreach (var item in vec)
            {
                Console.Write($"{item} ");
            }
            Console.WriteLine();

            while (vec.Count > 0)
            {
                vec.RemoveAt(vec.Count - 1);
            }
            vec.Print();
        }
    }
}
