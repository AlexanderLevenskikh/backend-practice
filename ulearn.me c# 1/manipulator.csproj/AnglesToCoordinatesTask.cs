using System;
using System.Drawing;
using NUnit.Framework;

namespace Manipulation
{
    public static class AnglesToCoordinatesTask
    {
        /// <summary>
        /// По значению углов суставов возвращает массив координат суставов
        /// в порядке new []{elbow, wrist, palmEnd}
        /// </summary>
        public static PointF[] GetJointPositions(double shoulder, double elbow, double wrist)
        {
            var initialPoint = new Point
            {
                X = 0,
                Y = 0
            };

            var elbowPos = CalculatePoint(initialPoint, shoulder, Manipulator.UpperArm);
            var wristPos = CalculatePoint(elbowPos, elbow + shoulder - Math.PI, Manipulator.Forearm);
            var palmEndPos = CalculatePoint(wristPos, elbow + shoulder + wrist - 2 * Math.PI, Manipulator.Palm);

            return new[]
            {
                elbowPos,
                wristPos,
                palmEndPos
            };
        }

        public static PointF CalculatePoint(PointF prevPoint, double angle, double length)
        {
            var x = prevPoint.X + Math.Cos(angle) * length;
            var y = prevPoint.Y + Math.Sin(angle) * length;

            return new PointF
            {
                X = (float) x,
                Y = (float) y
            };
        }
    }

    [TestFixture]
    public class AnglesToCoordinatesTask_Tests
    {
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
        [TestCase(0, 0, 0, -1 * Manipulator.Forearm + Manipulator.Palm + Manipulator.UpperArm, 0)]
        [TestCase(0, Math.PI, 0, Manipulator.Forearm + -1 * Manipulator.Palm + Manipulator.UpperArm, 0)]
        [TestCase(0, Math.PI, Math.PI, Manipulator.Forearm + Manipulator.Palm + Manipulator.UpperArm, 0)]
        public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
            Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
            Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
        }
    }
}