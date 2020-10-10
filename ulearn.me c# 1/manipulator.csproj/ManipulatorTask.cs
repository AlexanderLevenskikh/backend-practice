using System;
using NUnit.Framework;

namespace Manipulation
{
    public static class ManipulatorTask
    {
        /// <summary>
        /// Возвращает массив углов (shoulder, elbow, wrist),
        /// необходимых для приведения эффектора манипулятора в точку x и y 
        /// с углом между последним суставом и горизонталью, равному alpha (в радианах)
        /// См. чертеж manipulator.png!
        /// </summary>
        public static double[] MoveManipulatorTo(double x, double y, double alpha)
        {
            var (wristX, wristY) = (
                x + Manipulator.Palm * Math.Cos(Math.PI - alpha),
                y + Manipulator.Palm * Math.Sin(Math.PI - alpha)
            );
            var fromShoulderToWrist = Math.Sqrt(wristX * wristX + wristY * wristY);

            var elbow = TriangleTask.GetABAngle(
                Manipulator.UpperArm,
                Manipulator.Forearm,
                fromShoulderToWrist);

            if (double.IsNaN(elbow))
            {
                return new[] {double.NaN, double.NaN, double.NaN};
            }

            var wristOxAngle = Math.Atan2(wristY, wristX);

            if (double.IsNaN(wristOxAngle))
            {
                return new[] {double.NaN, double.NaN, double.NaN};
            }

            var shoulderAngle = TriangleTask.GetABAngle(
                Manipulator.UpperArm,
                fromShoulderToWrist,
                Manipulator.Forearm
            ) + wristOxAngle;

            var wrist = -alpha - shoulderAngle - elbow;

            return new[] {shoulderAngle, elbow, wrist};
        }
    }

    [TestFixture]
    public class ManipulatorTask_Tests
    {
        public const double Length = Manipulator.UpperArm +
                                       Manipulator.Forearm +
                                       Manipulator.Palm;

        [Test]
        public void TestMoveManipulatorTo()
        {
            var rng = new Random(312312);
            for (var testNo = 0; testNo < 1000; ++testNo)
            {
                var x = rng.NextDouble() * 2 * Length - Length;
                var y = rng.NextDouble() * 2 * Length - Length;
                var a = rng.NextDouble() * 2 * Math.PI;
                var angles = ManipulatorTask.MoveManipulatorTo(x, y, a);
                Assert.AreEqual(3, angles.Length);
                
                if (double.IsNaN(angles[0])) continue;
                
                var joints = AnglesToCoordinatesTask.GetJointPositions(
                    angles[0], angles[1], angles[2]);
                
                Assert.AreEqual(3, joints.Length);
                Assert.AreEqual(joints[2].X, x, 0.001);
                Assert.AreEqual(joints[2].Y, y, 0.001);
            }
        }
    }
}