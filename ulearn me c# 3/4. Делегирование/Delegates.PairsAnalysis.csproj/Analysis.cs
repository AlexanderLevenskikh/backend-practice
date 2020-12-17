using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.PairsAnalysis
{
    public static class Analysis
    {
        public static IEnumerable<Tuple<T, T>> Pairs<T>(this IEnumerable<T> seq)
        {
            var enumerator = seq.GetEnumerator();
            enumerator.MoveNext();
            var prev = enumerator.Current;

            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                yield return new Tuple<T, T>(prev, current);
                prev = current;
            }
        }

        public static int MaxIndex<T>(this IEnumerable<T> seq) where T : IComparable
        {
            var enumerator = seq.GetEnumerator();
            enumerator.MoveNext();
            var current = enumerator.Current;
            var currentIndex = 0;

            var (maxIndex, maxElement) = (currentIndex, current);

            while (enumerator.MoveNext())
            {
                current = enumerator.Current;
                currentIndex++;

                if (current.CompareTo(maxElement) > 0)
                {
                    (maxIndex, maxElement) = (currentIndex, current);
                }
            }

            return maxIndex;
        }
        
        public static int FindMaxPeriodIndex(params DateTime[] data)
        {
            if (data.Length < 2)
            {
                throw new InvalidOperationException();
            }
            
            return data
                .Pairs()
                .Select(x => (x.Item2 - x.Item1).TotalSeconds)
                .MaxIndex();
        }

        public static double FindAverageRelativeDifference(params double[] data)
        {
            if (data.Length < 2)
            {
                throw new InvalidOperationException();
            }
            
            var items = data
                .Pairs()
                .Select(x => (x.Item2 - x.Item1) / x.Item1);

            return items.Sum() / items.Count();
        }
    }
}
