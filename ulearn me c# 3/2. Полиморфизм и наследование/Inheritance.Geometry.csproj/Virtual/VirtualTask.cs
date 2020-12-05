using System;
using System.Collections.Generic;
using System.Linq;

namespace Inheritance.Geometry.Virtual
{
    public abstract class Body
    {
        public Vector3 Position { get; }

        protected Body(Vector3 position)
        {
            Position = position;
        }

        public abstract bool ContainsPoint(Vector3 point);

        public abstract RectangularCuboid GetBoundingBox();
    }

    public class Ball : Body
    {
        public double Radius { get; }

        public Ball(Vector3 position, double radius) : base(position)
        {
            Radius = radius;
        }


        public override bool ContainsPoint(Vector3 point)
        {
            var vector = point - Position;
            var length2 = vector.GetLength2();
            return length2 <= Radius * Radius;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            return new RectangularCuboid(Position, 2 * Radius, 2 * Radius, 2 * Radius);
        }
    }

    public class RectangularCuboid : Body
    {
        public double SizeX { get; }
        public double SizeY { get; }
        public double SizeZ { get; }

        public RectangularCuboid(Vector3 position, double sizeX, double sizeY, double sizeZ) : base(position)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            var minPoint = new Vector3(
                Position.X - SizeX / 2,
                Position.Y - SizeY / 2,
                Position.Z - SizeZ / 2);
            var maxPoint = new Vector3(
                Position.X + SizeX / 2,
                Position.Y + SizeY / 2,
                Position.Z + SizeZ / 2);

            return point >= minPoint && point <= maxPoint;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            return this;
        }
    }

    public class Cylinder : Body
    {
        public double SizeZ { get; }

        public double Radius { get; }

        public Cylinder(Vector3 position, double sizeZ, double radius) : base(position)
        {
            SizeZ = sizeZ;
            Radius = radius;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            var vectorX = point.X - Position.X;
            var vectorY = point.Y - Position.Y;
            var length2 = vectorX * vectorX + vectorY * vectorY;
            var minZ = Position.Z - SizeZ / 2;
            var maxZ = minZ + SizeZ;

            return length2 <= Radius * Radius && point.Z >= minZ && point.Z <= maxZ;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            return new RectangularCuboid(Position, 2 * Radius, 2 * Radius, SizeZ);
        }
    }

    public class CompoundBody : Body
    {
        public IReadOnlyList<Body> Parts { get; }

        public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
        {
            Parts = parts;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            return Parts.Any(body => body.ContainsPoint(point));
        }

        public override RectangularCuboid GetBoundingBox()
        {
            var boundingBoxes = Parts.Select(part => part.GetBoundingBox());
            var (minPosX, maxPosX) = boundingBoxes.GetRectangularListMaxAndMin(
                x => x.SizeX, x => x.Position.X);
            var (minPosY, maxPosY) = boundingBoxes.GetRectangularListMaxAndMin(
                x => x.SizeY, x => x.Position.Y);
            var (minPosZ, maxPosZ) = boundingBoxes.GetRectangularListMaxAndMin(
                x => x.SizeZ, x => x.Position.Z);
            
            var position = new Vector3((minPosX + maxPosX) / 2, (minPosY + maxPosY) / 2, (minPosZ + maxPosZ) / 2);
            return new RectangularCuboid(position, maxPosX - minPosX, maxPosY - minPosY, maxPosZ - minPosZ);
        }
    }

    public static class RectangularExtensions
    {
        public static (double, double) GetRectangularListMaxAndMin(
            this IEnumerable<RectangularCuboid> rectangularCuboids,
            Func<RectangularCuboid, double> sizeSelector,
            Func<RectangularCuboid, double> positionSelector)
        {
            var positions = rectangularCuboids.SelectMany(
                box =>
                {
                    var halfSize = sizeSelector(box) / 2;
                    var position = positionSelector(box);
                    return new[] {position + halfSize, position - halfSize};
                });

            return (positions.Min(), positions.Max());
        }
    }
}