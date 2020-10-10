using System;
using NUnit.Framework;

namespace Manipulation
{
    public class TriangleTask
    {
        /// <summary>
        /// Возвращает угол (в радианах) между сторонами a и b в треугольнике со сторонами a, b, c 
        /// </summary>
        public static double GetABAngle(double a, double b, double c)
        {
            if (a < 0 || b < 0 || c < 0) return double.NaN;
            if (b + c < a || a + c < b || a + b < c) return double.NaN;
            var divisor = 2 * a * b;
            
            return divisor == 0 ? Double.NaN : Math.Acos((a * a + b * b - c * c) / divisor);
        }
    }

    [TestFixture]
    public class TriangleTask_Tests
    {
        [TestCase(3, 4, 5, Math.PI / 2)]
        [TestCase(4, 5, 3, 0.6435)]
        [TestCase(3, 5, 4, 0.9273)]
        [TestCase(1, 1, 1, Math.PI / 3)]
        [TestCase(0, 0, 0, double.NaN)]
        [TestCase(1, 3, 5, double.NaN)] 
        [TestCase(3, 5, 1, double.NaN)]
        [TestCase(5, 1, 3, double.NaN)]

        [TestCase(1, 2, 3, Math.PI)]
        [TestCase(2, 3, 1, 0)]
        [TestCase(3, 1, 2, 0)]
        public void TestGetABAngle(double a, double b, double c, double expectedAngle)
        {
            var angle = TriangleTask.GetABAngle(a, b, c);
            
            if (double.IsNaN(expectedAngle))
                Assert.IsNaN(angle);
            
            Assert.AreEqual(expectedAngle, angle, 0.0001);
        }
    }
}