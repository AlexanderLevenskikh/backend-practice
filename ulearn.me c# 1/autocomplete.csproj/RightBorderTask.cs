using System;
using System.Collections.Generic;
using System.Linq;

namespace Autocomplete
{
    public class RightBorderTask
    {
        /// <returns>
        /// Возвращает индекс правой границы. 
        /// То есть индекс минимального элемента, который не начинается с prefix и большего prefix.
        /// Если такого нет, то возвращает items.Length
        /// </returns>
        /// <remarks>
        /// Функция должна быть НЕ рекурсивной
        /// и работать за O(log(items.Length)*L), где L — ограничение сверху на длину фразы
        /// </remarks>
        public static int GetRightBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
        {
            while (left < right - 1)
            {
                var middle = (left + right) / 2;
                
                if (string.Compare(phrases[middle], prefix, StringComparison.OrdinalIgnoreCase) >= 0
                    && !phrases[middle].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    right = middle;
                }
                else
                {
                    left = middle;
                }
            }

            return right;
        }
    }
}