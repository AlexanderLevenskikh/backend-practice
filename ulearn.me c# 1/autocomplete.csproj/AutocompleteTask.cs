using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Autocomplete
{
    internal class AutocompleteTask
    {
        /// <returns>
        /// Возвращает первую фразу словаря, начинающуюся с prefix.
        /// </returns>
        /// <remarks>
        /// Эта функция уже реализована, она заработает, 
        /// как только вы выполните задачу в файле LeftBorderTask
        /// </remarks>
        public static string FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            if (index < phrases.Count && phrases[index].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return phrases[index];

            return null;
        }

        /// <returns>
        /// Возвращает первые в лексикографическом порядке count (или меньше, если их меньше count) 
        /// элементов словаря, начинающихся с prefix.
        /// </returns>
        /// <remarks>Эта функция должна работать за O(log(n) + count)</remarks>
        public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
        {
            var actualCount = Math.Min(GetCountByPrefix(phrases, prefix), count);
            var startIndex = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            var result = new string[actualCount];
            Array.Copy(phrases.ToArray(), startIndex, result, 0, actualCount);
           
            return result;
        }

        /// <returns>
        /// Возвращает количество фраз, начинающихся с заданного префикса
        /// </returns>
        public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var leftBorder = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);
            var rightBorder = RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count);

            return rightBorder - leftBorder - 1;
        }
    }

    [TestFixture]
    public class AutocompleteTests
    {
        [Test]
        public void TopByPrefix_IsEmpty_WhenNoPhrases()
        {
            // ...
            //CollectionAssert.IsEmpty(actualTopWords);
        }

        // ...

        [Test]
        public void CountByPrefix_IsTotalCount_WhenEmptyPrefix()
        {
            // ...
            //Assert.AreEqual(expectedCount, actualCount);
        }

        // ...
    }
}