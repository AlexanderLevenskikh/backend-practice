using System;

namespace GeometryTasks
{
    public class Vector
    {
        public double X;
        public double Y;
        
        public double GetLength()
        {
            return Geometry.GetLength(this);
        }
        
        public Vector Add(Vector vector)
        {
            return Geometry.Add(this, vector);
        }
        
        public bool Belongs(Segment segment)
        {
            return Geometry.IsVectorInSegment(this, segment);
        }
    }

    public class Segment
    {
        public Vector Begin;
        public Vector End;

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }
        
        public bool Contains(Vector vector)
        {
            return Geometry.IsVectorInSegment(vector, this);
        }
    }

    public class Geometry
    {
        public static double GetLength(Vector vector)
        {
            return Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public static double GetLength(Segment segment)
        {
            return Math.Sqrt(
                (segment.Begin.X - segment.End.X) * (segment.Begin.X - segment.End.X) +
                (segment.Begin.Y - segment.End.Y) * (segment.Begin.Y - segment.End.Y));
        }

        public static bool IsVectorInSegment(Vector vector, Segment segment)
        {
            var (x1, y1, x2, y2) = (segment.Begin.X, segment.Begin.Y, segment.End.X, segment.End.Y);

            if (Math.Abs(x1 - x2) < 0.00001)
            {
                return (Math.Abs(vector.X - x1) < 0.00001) &&
                       vector.Y >= Math.Min(y1, y2) && vector.Y <= Math.Max(y1, y2);
            }

            var k = (y2 - y1) / (x2 - x1);
            var b = (y1 * x2 - y2 * x1) / (x2 - x1);

            var isOnLine = Math.Abs(vector.Y - k * vector.X - b) < 0.00001;

            return isOnLine &&
                   (vector.X >= Math.Min(x1, x2) && vector.X <= Math.Max(x1, x2)) &&
                   (vector.Y >= Math.Min(y1, y2) && vector.Y <= Math.Max(y1, y2));
        }

        public static Vector Add(Vector vectorA, Vector vectorB)
        {
            return new Vector
            {
                X = vectorA.X + vectorB.X,
                Y = vectorA.Y + vectorB.Y
            };
        }
    }
}