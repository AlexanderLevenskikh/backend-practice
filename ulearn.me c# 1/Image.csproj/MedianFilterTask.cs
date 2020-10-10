using System.Collections.Generic;
using System.Linq;

namespace Recognizer
{
    internal static class MedianFilterTask
    {
        /* 
         * Для борьбы с пиксельным шумом, подобным тому, что на изображении,
         * обычно применяют медианный фильтр, в котором цвет каждого пикселя, 
         * заменяется на медиану всех цветов в некоторой окрестности пикселя.
         * https://en.wikipedia.org/wiki/Median_filter
         * 
         * Используйте окно размером 3х3 для не граничных пикселей,
         * Окно размером 2х2 для угловых и 3х2 или 2х3 для граничных.
         */
        public static double[,] MedianFilter(double[,] original)
        {
            var (firstDimension, secondDimension) = (original.GetLength(0), original.GetLength(1));
            var medians = new double[firstDimension, secondDimension];

            for (var i = 0; i < firstDimension; i++)
            {
                for (var j = 0; j < secondDimension; j++)
                {
                    var medianList = new List<double>(new[] {original[i, j]});

                    for (var x = -1; x <= 1; x++)
                    {
                        for (var y = -1; y <= 1; y++)
                        {
                            if (i + y >= 0 && i + y < firstDimension && j + x >= 0 && j + x < secondDimension && (x != 0 || y != 0))
                            {
                                medianList.Add(original[i + y, j + x]);
                            }
                        }
                    }

                    medianList.Sort();
                    var medianListSize = medianList.Count;

                    if (medianListSize % 2 == 0 && medianListSize > 1)
                    {
                        medians[i, j] = (medianList[medianListSize / 2 - 1] + medianList[medianListSize / 2]) / 2;
                    }
                    else
                    {
                        medians[i, j] = medianList[medianListSize / 2];
                    }
                }
            }

            return medians;
        }
    }
}