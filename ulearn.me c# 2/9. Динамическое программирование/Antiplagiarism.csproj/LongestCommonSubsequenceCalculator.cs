using System;
using System.Collections.Generic;

namespace Antiplagiarism
{
    public static class LongestCommonSubsequenceCalculator
    {
        public static List<string> Calculate(List<string> first, List<string> second)
        {
            var opt = CreateOptimizationTable(first, second);
            return RestoreAnswer(opt, first, second);
        }

        private static int[,] CreateOptimizationTable(List<string> first, List<string> second)
        {
            var optTable = new int[first.Count + 1, second.Count + 1];
            for (var i = 0; i <= first.Count; i++)
                optTable[i, 0] = 0;
            for (var i = 0; i <= second.Count; i++)
                optTable[0, i] = 0;

            for (var i = 1; i <= first.Count; i++)
            for (var j = 1; j <= second.Count; j++)
            {
                if (first[i - 1] == second[j - 1])
                {
                    optTable[i, j] = optTable[i - 1, j - 1] + 1;
                }
                else
                {
                    optTable[i, j] = Math.Max(optTable[i, j - 1], optTable[i - 1, j]);
                }
            }

            return optTable;
        }

        private static List<string> RestoreAnswer(int[,] opt, List<string> first, List<string> second)
        {
            var i = first.Count;
            var j = second.Count;
            var result = new List<string>();

            while (i > 0 && j > 0)
            {
                if (first[i - 1] == second[j - 1])
                {
                    result.Add(first[i - 1]);
                    i--;
                    j--;
                }
                else
                {
                    if (opt[i - 1, j] == opt[i, j])
                        i--;
                    else j--;
                }
            }

            result.Reverse();

            return result;
        }
    }
}