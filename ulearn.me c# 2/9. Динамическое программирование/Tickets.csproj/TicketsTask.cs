using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tickets
{
    public static class TicketsTask
    {
        public static BigInteger Solve(int halfLen, int totalSum)
        {
            if (totalSum % 2 != 0)
            {
                return 0;
            }

            var halfSum = totalSum / 2;
            var opt = new BigInteger[halfLen, halfSum + 1];

            for (var i = 0; i <= halfSum; i++)
                opt[0, i] = i < 10 ? 1 : 0;
            for (var i = 0; i < halfLen; i++)
                opt[i, 0] = 1;

            for (var i = 1; i < halfLen; i++)
            for (var j = 1; j <= halfSum; j++)
            {
                BigInteger sum = 0;
                var startFrom = Math.Max(0, j - 9);

                for (var k = startFrom; k <= j; k++)
                    sum += opt[i - 1, k];
                
                opt[i, j] = sum;
            }

            return opt[halfLen - 1, halfSum] * opt[halfLen - 1, halfSum];
        }
    }
}