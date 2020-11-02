using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    internal class DungeonPath
    {
        public int Length { get; set; }
        public IEnumerable<Point> List { get; set; }
    }
    
    public class DungeonTask
    {
        public static MoveDirection[] FindShortestPath(Map map)
        {
            var pathsFromInitial = BfsTask.FindPaths(
                map, map.InitialPosition, map.Chests);
            var pathsFromExit = BfsTask.FindPaths(
                map, map.Exit, map.Chests);

            var pathsFromInitialToExit = pathsFromInitial.Join(
                pathsFromExit,
                x => x.Value,
                y => y.Value,
                (x, y) => new DungeonPath
                {
                    List = x.Reverse().Concat(y.Skip(1)),
                    Length = x.Length + y.Length - 1,
                });

            var shortestPath = GetShortestPath(pathsFromInitialToExit);
            if (shortestPath.Any())
            {
                return TransformToDirection(shortestPath);
            }
            
            pathsFromInitialToExit = BfsTask
                .FindPaths(map, map.InitialPosition, new[] {map.Exit})
                .Select(x => new DungeonPath
                {
                    List = x.Reverse(),
                    Length = x.Length,
                });
            
            shortestPath = GetShortestPath(pathsFromInitialToExit);
            
            return shortestPath.Any() ? TransformToDirection(shortestPath) : new MoveDirection[0];
        }

        private static IEnumerable<Point> GetShortestPath(IEnumerable<DungeonPath> pathsFromInitialToExit)
        {
            var shortestPathLength = pathsFromInitialToExit.Any() 
                ? (int?)pathsFromInitialToExit.Min(x => x.Length)
                : null;
            
            return shortestPathLength != null ? pathsFromInitialToExit
                .FirstOrDefault(x => x.Length == shortestPathLength)
                .List : new Point[0];
        }

        private static MoveDirection[] TransformToDirection(IEnumerable<Point> path)
        {
            var previousPoint = path.First();

            return path.Skip(1).Select(currentPoint =>
            {
                var size = new Size
                {
                    Width = currentPoint.X - previousPoint.X,
                    Height = currentPoint.Y - previousPoint.Y
                };

                var direction = Walker.ConvertOffsetToDirection(size);
                previousPoint = currentPoint;

                return direction;
            }).ToArray();
        }
    }
}