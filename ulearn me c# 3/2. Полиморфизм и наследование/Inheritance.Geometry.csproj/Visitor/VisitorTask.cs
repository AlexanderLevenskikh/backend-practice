using System;
using System.Collections.Generic;
using System.Linq;

namespace Inheritance.Geometry.Visitor
{
    public interface IVisitor
    {
        Body Visit(Ball element);
        Body Visit(RectangularCuboid element);
        Body Visit(Cylinder element);
        Body Visit(CompoundBody element);
    }

    public abstract class Body
    {
        public Vector3 Position { get; }

        protected Body(Vector3 position)
        {
            Position = position;
        }

        public abstract Body Accept(IVisitor visitor);
    }

    public class Ball : Body
    {
        public double Radius { get; }

        public Ball(Vector3 position, double radius) : base(position)
        {
            Radius = radius;
        }

        public override Body Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
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

        public override Body Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
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

        public override Body Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class CompoundBody : Body
    {
        public IReadOnlyList<Body> Parts { get; }

        public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
        {
            Parts = parts;
        }

        public override Body Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class BoundingBoxVisitor : IVisitor
    {
        public Body Visit(Ball element)
        {
            return new RectangularCuboid(element.Position, 2 * element.Radius, 2 * element.Radius, 2 * element.Radius);
        }

        public Body Visit(RectangularCuboid element)
        {
            return element;
        }

        public Body Visit(Cylinder element)
        {
            return new RectangularCuboid(element.Position, 2 * element.Radius, 2 * element.Radius, element.SizeZ);
        }

        public Body Visit(CompoundBody element)
        {
            var boundingBoxes = element.Parts.Select(part => (RectangularCuboid) part.Accept(this));
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

    public class BoxifyVisitor : IVisitor
    {
        public Body Visit(Ball element)
        {
            return new RectangularCuboid(element.Position, 2 * element.Radius, 2 * element.Radius, 2 * element.Radius);
        }

        public Body Visit(RectangularCuboid element)
        {
            return element;
        }

        public Body Visit(Cylinder element)
        {
            return new RectangularCuboid(element.Position, 2 * element.Radius, 2 * element.Radius, element.SizeZ);
        }

        public Body Visit(CompoundBody element)
        {
            var newParts = element.Parts
                .Select(part => part.Accept(this))
                .ToList();

            return new CompoundBody(newParts);
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