using System;
using System.Collections.Generic;
using System.Linq;

namespace Recognizer
{
    public static class ThresholdFilterTask
    {
        public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
        {
            var rows = original.GetLength(0);
            var columns = original.GetLength(1);
            var totalPixelsCount = rows * columns;
            var minimalWhitePixelsCount = (int) (totalPixelsCount * whitePixelsFraction);

            var values = new List<double>();
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    values.Add(original[i, j]);
                }
            }

            values.Sort();

            var threshold = 0.0;

            if (minimalWhitePixelsCount == totalPixelsCount)
            {
                threshold = double.NegativeInfinity;
            }
            else if (minimalWhitePixelsCount == 0)
            {
                threshold = double.PositiveInfinity;
            }
            else
            {
                threshold = values[Math.Max(totalPixelsCount - minimalWhitePixelsCount, 0)];
            }
            
            var blackAndWhite = new double[rows, columns];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    var pixel = original[i, j];
                    blackAndWhite[i, j] = pixel >= threshold ? 1.0 : 0.0;
                }
            }

            return blackAndWhite;
        }
    }
}