using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    public class BfsTask
    {
        private static MoveDirection[] directions =
        {
            MoveDirection.Up, MoveDirection.Right, MoveDirection.Down, MoveDirection.Left
        };

        public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
        {
            var queue = new Queue<SinglyLinkedList<Point>>();
            var visited = new HashSet<Point> {start};
            var chestsHashSet = chests.ToHashSet();
            queue.Enqueue(new SinglyLinkedList<Point>(start));

            while (queue.Count != 0)
            {
                var currentPosition = queue.Dequeue();
                var walker = new Walker(currentPosition.Value);

                var nextPositions = directions
                    .Select(direction => walker.WalkInDirection(map, direction))
                    .Where(w => w.PointOfCollision == null && !visited.Contains(w.Position))
                    .Select(w => w.Position);

                foreach (var nextPosition in nextPositions)
                {
                    var nextPath = new SinglyLinkedList<Point>(nextPosition, currentPosition);
                    queue.Enqueue(nextPath);
                    visited.Add(nextPosition);
                    if (chestsHashSet.Contains(nextPosition))
                    {
                        yield return nextPath;
                    }
                }
            }
        }
    }
}