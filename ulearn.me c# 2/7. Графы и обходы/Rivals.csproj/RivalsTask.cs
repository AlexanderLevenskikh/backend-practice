using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rivals
{
    public class RivalsTask
    {
        public static IEnumerable<OwnedLocation> AssignOwners(Map map)
        {
            var queue = new Queue<OwnedLocation>();
            
            for (var i = 0; i < map.Players.Length; i++)
            {
                queue.Enqueue(new OwnedLocation(i, map.Players[i], 0));
                map.Maze[map.Players[i].X, map.Players[i].Y] = MapCell.Wall;
            }
            
            while (queue.Count > 0)
            {
                var currentPoint = queue.Dequeue();
                yield return currentPoint;

                foreach (var direction in Directions)
                    if (TryGoInDirection(map, currentPoint, direction, out var newPoint))
                        queue.Enqueue(newPoint);
            }
        }

        private static readonly MoveDirection[] Directions =
        {
            MoveDirection.Right,
            MoveDirection.Up,
            MoveDirection.Down,
            MoveDirection.Left
        };

        private static readonly Dictionary<MoveDirection, Size> OffsetToDirection = new Dictionary<MoveDirection, Size>
        {
            {MoveDirection.Up, new Size(0, -1)},
            {MoveDirection.Down, new Size(0, 1)},
            {MoveDirection.Left, new Size(-1, 0)},
            {MoveDirection.Right, new Size(1, 0)}
        };

        private static bool TryGoInDirection(Map map, OwnedLocation position, MoveDirection direction,
            out OwnedLocation potentialPoint)
        {
            potentialPoint = new OwnedLocation(
                position.Owner,
                position.Location + OffsetToDirection[direction],
                position.Distance + 1);
            if (!map.InBounds(potentialPoint.Location) ||
                map.Maze[potentialPoint.Location.X, potentialPoint.Location.Y] != MapCell.Empty)
                return false;

            map.Maze[potentialPoint.Location.X, potentialPoint.Location.Y] = MapCell.Wall;
            return true;
        }

        private enum MoveDirection
        {
            Right,
            Up,
            Down,
            Left
        }
    }
}
