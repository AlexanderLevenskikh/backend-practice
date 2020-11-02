using System;
using System.Collections.Generic;
using System.Linq;

namespace GaussAlgorithm
{
    public class Solver
    {
        public double[] Solve(double[][] matrix, double[] freeMembers)
        {
            var rowsCount = matrix.Length;
            var rowsRange = Enumerable.Range(0, rowsCount);
            var columnsCount = matrix[0].Length;
            var columnsRange = Enumerable.Range(0, columnsCount);

            var isAnyValue = columnsRange.Select(x => false).ToArray();

            var freeRowsDictionary = rowsRange.ToDictionary(x => x, _ => true);

            foreach (var columnIndex in columnsRange)
            {
                var rowsWithValues = rowsRange
                    .Where(rowIndex => matrix[rowIndex][columnIndex] != 0);
                var freeRowsWithValues = rowsWithValues
                    .Where(rowIndex => freeRowsDictionary[rowIndex]);

                if (freeRowsWithValues.Any())
                {
                    var freeRowIndex = freeRowsWithValues.First();
                    freeRowsDictionary[freeRowIndex] = false;

                    foreach (var otherRowIndex in rowsWithValues.Where(rowIndex => rowIndex != freeRowIndex))
                    {
                        var freeRowMultiplier = matrix[otherRowIndex][columnIndex];
                        var otherRowMultiplier = matrix[freeRowIndex][columnIndex];

                        foreach (var operationColumnIndex in columnsRange)
                        {
                            matrix[freeRowIndex][operationColumnIndex] *= freeRowMultiplier;
                            matrix[otherRowIndex][operationColumnIndex] =
                                matrix[otherRowIndex][operationColumnIndex] * otherRowMultiplier -
                                matrix[freeRowIndex][operationColumnIndex];
                        }
                        
                        freeMembers[freeRowIndex] *= freeRowMultiplier;
                        freeMembers[otherRowIndex] = freeMembers[otherRowIndex] * otherRowMultiplier -
                                                     freeMembers[freeRowIndex];
                    }
                }
                else
                {
                    isAnyValue[columnIndex] = true;
                }
            }

            return columnsRange.Select(columnIndex =>
            {
                if (isAnyValue[columnIndex])
                {
                    return 0;
                }

                var rowIndex = rowsRange.First(r => matrix[r][columnIndex] != 0);

                return freeMembers[rowIndex] / matrix[rowIndex][columnIndex];
            }).ToArray();
        }
    }
}