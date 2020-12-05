using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Greedy.Architecture;
using System.Drawing;
using NUnit.Framework;

namespace Greedy
{
    public class DijkstraCell
    {
        public Point? Point { get; set; }
        public int Cost { get; set; }
    }
    
    public static class StateExtensions
    {
        public static List<Point> ToPointList(this State state)
        {
            var result = new List<Point>();
            for (var y = 0; y < state.MapHeight; y++)
            {
                for (var x = 0; x < state.MapWidth; x++)
                {
                    result.Add(new Point(x, y));
                }
            }

            return result;
        }

        public static List<DijkstraCell> GetIncidentCells(this State state, Point point)
        {
            var result = new List<DijkstraCell>();
            var movementOffsets = new[] {(-1, 0), (1, 0), (0, -1), (0, 1)};

            foreach (var (offsetX, offsetY) in movementOffsets)
            {
                var incidentPoint = new Point(point.X + offsetX, point.Y + offsetY);
                if (state.InsideMap(incidentPoint) && !state.IsWallAt(incidentPoint))
                {
                    result.Add(new DijkstraCell
                    {
                        Cost = state.CellCost[incidentPoint.X, incidentPoint.Y],
                        Point = incidentPoint
                    });
                }
            }

            return result;
        }
    }
    
    public class DijkstraPathFinder
    {
        public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start,
            IEnumerable<Point> targets)
        {
            var visited = new Dictionary<Point, bool>();
            var toVisit = new List<Point>(new []{start});
            var track = new Dictionary<Point, DijkstraCell>();
            track[start] = new DijkstraCell
            {
                Cost = 0,
                Point = null
            };
            
            if (!targets.Any()) yield break;

            while (true)
            {
                Point? toOpen = null;
                var bestPrice = double.PositiveInfinity;
                foreach (var point in toVisit)
                {
                    if (!visited.ContainsKey(point) && track.ContainsKey(point) && track[point].Cost < bestPrice)
                    {
                        bestPrice = track[point].Cost;
                        toOpen = point;
                    }
                }
                
                if (toOpen == null) yield break;
                if (targets.Contains((Point) toOpen)) yield return GetPathForPoint((Point) toOpen, track);
                
                foreach (var cell in state.GetIncidentCells((Point) toOpen))
                {
                    var currentPrice = track[(Point) toOpen].Cost + cell.Cost;
                    if (!track.ContainsKey((Point)cell.Point) || track[(Point)cell.Point].Cost > currentPrice)
                    {
                        track[(Point) cell.Point] = new DijkstraCell
                        {
                            Point = toOpen,
                            Cost = currentPrice
                        };
                        if (!visited.ContainsKey((Point) cell.Point) && !toVisit.Contains((Point) cell.Point))
                        {
                            toVisit.Add((Point) cell.Point);
                        }
                    }
                }

                toVisit.Remove((Point)toOpen);
                visited.Add((Point)toOpen, true);
            }
        }

        private PathWithCost GetPathForPoint(Point point, Dictionary<Point, DijkstraCell> track)
        {
            Point? currentPoint = point;
            var path = new List<Point>();
            while (currentPoint != null)
            {
                path.Add((Point)currentPoint);
                currentPoint = track[(Point)currentPoint].Point;
            }
            
            path.Reverse();
            return new PathWithCost(track[point].Cost, path.ToArray());
        }
    }
}
