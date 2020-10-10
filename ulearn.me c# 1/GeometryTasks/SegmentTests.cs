using NUnit.Framework;

namespace GeometryTasks
{
    [TestFixture]
    public class SegmentTests
    {
        [TestCase(0, 0, 2, 2, 1, 1, true)]
        [TestCase(0, 0, 2, 2, 0, 0, true)]
        [TestCase(0, 0, 2, 2, 2, 2, true)]
        [TestCase(0, 0, 0, 0, 0, 0, true)]
        [TestCase(0, 0, 2, 2, -1, -1, false)]
        [TestCase(0, 0, 2, 2, 3, 3, false)]
        public void TestIsVectorInSegment(double x1, double y1, double x2, double y2, double x, double y, bool inSegment)
        {
            var segment = new Segment
            {
                Begin = new Vector
                {
                    X = x1,
                    Y = y1
                },
                End = new Vector
                {
                    X = x2,
                    Y = y2
                }
            };
            var vector = new Vector
            {
                X = x,
                Y = y
            };

            var result = Geometry.IsVectorInSegment(vector, segment);
            
            Assert.AreEqual(inSegment, result);
        }
    }
}