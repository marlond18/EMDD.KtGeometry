using KtExtensions;
using EMDD.KtGeometry.KtLines._2D;
using EMDD.KtGeometry.KtPoints;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KtGeometryTest.KtLines._2D
{
    [TestClass]
    public class KtLine2DTests
    {
        [TestMethod]
        public void InlineTest()
        {
            var line = new KtLine2D(1, 2, 4, 5);
            Assert.IsTrue(line.Inline(line.Start));
            Assert.IsTrue(line.Inline(line.End));
            Assert.IsTrue(line.Inline(new KtPoint2D(13, 14)));
            Assert.IsFalse(line.Inline(new KtPoint2D(-4, 20)));
        }

        [TestMethod]
        public void DistanceTest()
        {
            var line = new KtLine2D(1, 2, 4, 5);
            Assert.IsTrue((line.Distance(new KtPoint2D(-4, 20)) + 16.2634559672906).NearZero(4));
        }

        [TestMethod]
        public void PointInsideLineTest()
        {
            var line = new KtLine2D(1, 3, 4, 5);
            Assert.AreEqual(line.PointInsideLine(4), new KtPoint2D(13, 11));
        }

        [TestMethod]
        public void IntersectionTest()
        {
            var line3 = new KtLine2D(3.0 / 4.0, -1, 0);
            var line2 = new KtLine2D(100, -1, 75 * 100);
#pragma warning disable CS0618 // Type or member is obsolete
            Assert.AreEqual(line3.IntersectWith3(line2), new KtPoint2D(30000.0 / 397.0, 22500.0 / 397.0));
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [TestMethod]
        public void KtLine2DTest()
        {
            var start = new KtPoint2D(1, 2);
            var end = new KtPoint2D(2, 4);
            var line = new KtLine2D(start, end);
            Assert.AreEqual(line.Start, start);
            Assert.AreEqual(line.End, end);
        }

        [TestMethod]
        public void KtLine2DTest1()
        {
            var start = new KtPoint2D(1, 2);
            var end = new KtPoint2D(3, 4);
            var line = new KtLine2D(1, 2, 3, 4);
            Assert.AreEqual(line.Start, start);
            Assert.AreEqual(line.End, end);
        }

        [TestMethod]
        public void KtLine2DTest2()
        {
            var line = new KtLine2D(-3, 12, 5);
            Assert.AreEqual(line.A, -3);
            Assert.AreEqual(line.B, 12);
            Assert.AreEqual(line.C, 5);
            var line2 = new KtLine2D(1, 2, 4);
            Assert.AreEqual(line2.A, 1);
            Assert.AreEqual(line2.B, 2);
            Assert.AreEqual(line2.C, 4);
            var line3 = new KtLine2D(0, 2, 4);
            Assert.AreEqual(line3.A, 0);
            Assert.AreEqual(line3.B, 2);
            Assert.AreEqual(line3.C, 4);
            var line4 = new KtLine2D(-1, 0, 4);
            Assert.AreEqual(line4.A, -1);
            Assert.AreEqual(line4.B, 0);
            Assert.AreEqual(line4.C, 4);
            var line5 = new KtLine2D(-1, -4, 0);
            Assert.AreEqual(line5.A, -1);
            Assert.AreEqual(line5.B, -4);
            Assert.AreEqual(line5.C, 0);
        }

        [TestMethod]
        public void CloneTest()
        {
            var line4 = new KtLine2D(-1, 0, 4);
            Assert.AreEqual(new KtLine2D(-1, 0, 4), line4);
        }

        [TestMethod]
        public void CreateHorizontalLineTest()
        {
            var line = KtLine2D.CreateHorizontalLine(3);
            Assert.AreEqual(line, new KtLine2D(0, 3, 12, 3));
        }

        [TestMethod]
        public void CreateVerticalLineTest()
        {
            var line = KtLine2D.CreateVerticalLine(3);
            Assert.AreEqual(line, new KtLine2D(3, 0, 3, 12));
        }

        [TestMethod]
        public void CreateAxisTest()
        {
            var line = KtLine2D.YAxis();
            Assert.AreEqual(line, new KtLine2D(0, 0, 0, 12));
            var line2 = KtLine2D.XAxis();
            Assert.AreEqual(line2, new KtLine2D(0, 0, 12, 0));
        }
    }
}