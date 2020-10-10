using System;

namespace Recognizer
{
    internal static class SobelFilterTask
    {
        public static double[,] SobelFilter(double[,] g, double[,] sx)
        {
            var width = g.GetLength(0);
            var height = g.GetLength(1);
            var dimension = sx.GetLength(0);

            var result = new double[width, height];
            var sy = GetMatrixTransposition(sx, dimension);
            var shift = dimension / 2;

            for (int x = shift; x < width - shift; x++)
            for (int y = shift; y < height - shift; y++)
            {
                var gx = CalculateConvolution(sx, g, x, y, shift);
                var gy = CalculateConvolution(sy, g, x, y, shift);

                result[x, y] = Math.Sqrt(gx * gx + gy * gy);
            }

            return result;
        }

        public static double CalculateConvolution(double[,] matrix, double[,] pixels, int x, int y, int shift)
        {
            var result = 0.0;
            var sideLength = matrix.GetLength(0);

            for (int i = 0; i < sideLength; i++)
            {
                for (int j = 0; j < sideLength; j++)
                {
                    result += matrix[i, j] * pixels[x - shift + i, y - shift + j];
                }
            }

            return result;
        }

        public static double[,] GetMatrixTransposition(double[,] a, int dimension)
        {
            var result = new double[dimension, dimension];

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    result[j, i] = a[i, j];
                }
            }

            return result;
        }
    }
}