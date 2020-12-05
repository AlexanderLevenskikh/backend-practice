using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;

namespace Greedy
{
    public class GreedyPathFinder : IPathFinder
    {
        public List<Point> FindPathToCompleteGoal(State state)
        {
            var currentEnergy = 0;
            var currentGoal = 0;
            var chests = state.Chests.ToHashSet();
            var pathFinder = new DijkstraPathFinder();
            var path = new List<Point>();
            var start = state.Position;

            while (currentEnergy < state.Energy && currentGoal < state.Goal)
            {
                var pathsWithCosts = pathFinder.GetPathsByDijkstra(state, start, chests);

                var shortestWay = pathsWithCosts.FirstOrDefault();
                if (shortestWay == null)
                    return new List<Point>();

                currentEnergy += shortestWay.Cost;
                currentGoal++;
                path.AddRange(shortestWay.Path.Skip(1));
                start = shortestWay.End;
                chests.Remove(start);
            }

            if (currentGoal < state.Goal || currentEnergy > state.Energy)
            {
                return new List<Point>();
            }

            return path;
        }
    }
}