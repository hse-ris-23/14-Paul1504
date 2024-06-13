using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4
{
        public class MyCollection<T> : IEnumerable<T>, ICollection<T> where T : ICloneable
        {
            public Point<T>[] positions;
            public int count;

            public int Count => count;

            public bool IsReadOnly => false;

            public MyCollection(int capacity)
            {
                positions = new Point<T>[capacity];
            }

            public MyCollection()
            {
                positions = new Point<T>[11];
            }

            public MyCollection(MyCollection<T> table)
            {
                positions = new Point<T>[table.positions.Length];
                count = table.count;

                for (int i = 0; i < table.positions.Length; i++)
                {
                    if (table.positions[i] != null && table.positions[i].IsFound == false)
                    {
                        Point<T> p = new Point<T>(table.positions[i].Data);
                        positions[i] = p;
                    }
                }
            }

            private int HashPos(T data)
            {
                return Math.Abs(data.GetHashCode()) % positions.Length;
            }

            public void Add(T data)
            {
                if (count == positions.Length)
                    throw new Exception("Таблица полная");

                int position = HashPos(data);
                Point<T> d = new Point<T>(data);

                if (positions[position] == null)
                {
                    positions[position] = d;
                }
                else
                {
                    int pos = position;
                    while (positions[pos] != null)
                    {
                        if (positions[pos].IsFound)
                            break;
                        pos = (pos + 1) % positions.Length;
                    }
                    positions[pos] = d;
                }
                count++;
            }

            public bool Contains(T data)
            {
                int pos = HashPos(data);

                while (positions[pos] != null)
                {
                    if (!positions[pos].IsFound && positions[pos].Data.Equals(data))
                    {
                        return true;
                    }
                    pos = (pos + 1) % positions.Length;
                }

                return false;
            }

            public bool Remove(T data)
            {
                int pos = HashPos(data);
                while (positions[pos] != null)
                {
                    if (positions[pos].Data.Equals(data))
                    {
                        positions[pos].IsFound = true;
                        count--;
                        return true;
                    }
                    pos = (pos + 1) % positions.Length;
                }

                return false;
            }
            public void Show()
            {
                for (int i = 0; i < positions.Length; i++)
                {
                    if (positions[i] != null && !positions[i].IsFound)
                    {
                        Console.WriteLine(positions[i].Data.ToString());
                    }
                    else
                    {
                        Console.WriteLine("Пусто");
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return Enumerator();
            }


            public IEnumerator<T> Enumerator()
            {
                for (int i = 0; i < positions.Length; i++)
                {
                    if (positions[i] != null && !positions[i].IsFound)
                    {
                        yield return positions[i].Data;
                    }
                }
            }

        public void AddRange(IEnumerable<T> items)
            {
                    foreach (var item in items)
                    {
                        Add(item);
                    }
            }

            public int RemoveInRange(IEnumerable<T> items)
            {
                int count = 0;
                foreach (var item in items)
                {
                    if (Remove(item))
                        count++;
                }
                return count;
            }

            public void Clear()
            {
                count = 0;
                positions = new Point<T>[positions.Length];
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                for (int i = 0; i < positions.Length; i++)
                {
                    if (positions[i] != null && !positions[i].IsFound)
                    {
                        array[arrayIndex] = positions[i].Data;
                        arrayIndex++;
                    }
                }
            }

            public MyCollection<T> DeepClone()
            {
                MyCollection<T> clone = new MyCollection<T>(positions.Length);
                for (int i = 0; i < positions.Length; i++)
                {
                    if (positions[i] != null && !positions[i].IsFound)
                    {
                        clone.positions[i] = new Point<T>((T)positions[i].Data.Clone());
                    }
                }
                return clone;
            }

            public MyCollection<T> ShallowClone()
            {
                return new MyCollection<T>(this);
            }

        public IEnumerator<T> GetEnumerator()
        {
            return Enumerator();
        }

        public T this[T data]
            {
                get
                {
                    int pos = HashPos(data);

                    while (positions[pos] != null)
                    {
                        if (!positions[pos].IsFound && positions[pos].Data.Equals(data))
                        {
                            return positions[pos].Data;
                        }
                        pos = (pos + 1) % positions.Length;
                    }

                    throw new KeyNotFoundException("Элемент не найден в коллекции.");
                }
                set
                {
                    Remove(data);

                    Add(value);
                }
            }
    }
}
