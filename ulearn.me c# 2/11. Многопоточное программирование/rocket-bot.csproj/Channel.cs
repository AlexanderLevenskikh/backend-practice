using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace rocket_bot
{
    public class Channel<T> where T : class
    {
        private List<T> list = new List<T>();

        /// <summary>
        /// Возвращает элемент по индексу или null, если такого элемента нет.
        /// При присвоении удаляет все элементы после.
        /// Если индекс в точности равен размеру коллекции, работает как Append.
        /// </summary>
        public T this[int index]
        {
            get
            {
                lock (list)
                {
                    if (index > list.Count - 1 || index < 0)
                    {
                        return null;
                    }

                    return list[index];
                }
            }
            set
            {
                lock (list)
                {
                    if (index > list.Count || index < 0)
                    {
                        throw new ArgumentOutOfRangeException();
                    }

                    if (index == list.Count)
                    {
                        list.Add(value);
                    }
                    else
                    {
                        list[index] = value;
                        list.RemoveRange(index + 1, list.Count - index - 1);
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает последний элемент или null, если такого элемента нет
        /// </summary>
        public T LastItem()
        {
            lock (list)
            {
                return !list.Any() ? null : list[list.Count - 1];
            }
        }

        /// <summary>
        /// Добавляет item в конец только если lastItem является последним элементом
        /// </summary>
        public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
        {
            lock (list)
            {
                if ((list.Count == 0 && knownLastItem == null) || (list.Count > 0 && knownLastItem != null &&
                                                                   knownLastItem.Equals(list[list.Count - 1])))
                {
                    list.Add(item);
                }
            }
        }

        /// <summary>
        /// Возвращает количество элементов в коллекции
        /// </summary>
        public int Count
        {
            get
            {
                lock (list)
                {
                    return list.Count;
                }
            }
        }
    }
}