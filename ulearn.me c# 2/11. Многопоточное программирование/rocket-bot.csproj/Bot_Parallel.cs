using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace rocket_bot
{
    public partial class Bot
    {
        public Rocket GetNextMove(Rocket rocket)
        {
            // TODO: распараллелить запуск SearchBestMove
            var tasks = Enumerable.Range(0, threadsCount).Select(_ =>
            {
                return new Task<Tuple<Turn, double>>(() =>
                    SearchBestMove(rocket, new Random(random.Next()), iterationsCount / threadsCount));
            }).ToArray();
            foreach (var t in tasks)
                t.Start();
            Task.WaitAll(tasks);

            var bestScore = tasks.Max(x => x.Result.Item2);
            var bestMove = tasks.First(x => Math.Abs(x.Result.Item2 - bestScore) < 0.000000001).Result.Item1;
            var newRocket = rocket.Move(bestMove, level);
            return newRocket;
        }
    }
}