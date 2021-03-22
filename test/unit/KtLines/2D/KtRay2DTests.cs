using KtExtensions;
using EMDD.KtGeometry.KtLines._2D;
using EMDD.KtGeometry.KtPoints;
using EMDD.KtGeometry.KtVectors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KtGeometryTest.KtLines._2D
{
    [TestClass]
    public class KtRay2DTests
    {
        [TestMethod]
        public void InlineTest()
        {
            var ray1 = new KtRay2D(new KtPoint2D(0, 2), new KtPoint2D(30, 4));
            Assert.IsTrue(ray1.Inline(new KtPoint2D(120, 10)));
            Assert.IsFalse(ray1.Inline(new KtPoint2D(-120, -6)));
        }

        [TestMethod]
        public void DistanceTest()
        {
            var ray1 = new KtRay2D(new KtPoint2D(4, -2), new KtVector2D(-2, 4));
            Assert.IsTrue((ray1.Distance(new KtPoint2D(4, 3)) - 2.23606797749979).NearZero(7));
        }

        [TestMethod]
        public void PointInsideLineTest()
        {
            var ray1 = new KtRay2D(new KtPoint2D(0, 2), new KtPoint2D(30, 4));
            Assert.AreEqual(ray1.PointInsideLine(4), new KtPoint2D(120, 10));
            Assert.AreEqual(ray1.PointInsideLine(-4), new KtPoint2D(-120, -6));
        }

        [TestMethod]
        public void IntersectionTest()
        {
            var ray1 = new KtRay2D(new KtPoint2D(0, 2), new KtPoint2D(30, 4));
            var ray2 = new KtRay2D(new KtPoint2D(5, 0), new KtPoint2D(15, 12));
#pragma warning disable CS0618 // Type or member is obsolete
            Assert.AreEqual(ray1.IntersectWith3(ray2), new KtPoint2D(120.0 / 17, 42.0 / 17));
            var ray3 = new KtRay2D(new KtPoint2D(4, -2), new KtPoint2D(20, -4));
            var ray4 = new KtRay2D(new KtPoint2D(-5, 6), new KtPoint2D(15, 12));
            Assert.IsNull(ray3.IntersectWith3(ray4));
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [TestMethod]
        public void KtRay2DTest()
        {
            var ray = new KtRay2D(new KtPoint2D(4, 6), new KtPoint2D(5, -10));
            Assert.AreEqual(ray.Start, new KtPoint2D(4, 6));
            Assert.AreEqual(ray.End, new KtPoint2D(5, -10));
            Assert.AreEqual(ray, new KtRay2D(new KtPoint2D(4, 6), new KtPoint2D(5, -10)));
            var ray2 = new KtRay2D(new KtPoint2D(3, 6), new KtVector2D(2, -1));
            var ray3 = new KtRay2D(new KtPoint2D(3, 6), new KtVector2D(4, -2));
            Assert.AreEqual(ray2.Start, new KtPoint2D(3, 6));
            Assert.AreEqual(ray2.End, new KtPoint2D(3, 6) + new KtPoint2D(2, -1));
            Assert.AreEqual(ray2, ray3);
        }

        [TestMethod]
        public void CloneTest()
        {
            var testray = new KtRay2D(new KtPoint2D(1, 4), new KtVector2D(3, 5));
            Assert.AreEqual(testray.Clone(), testray);
        }
    }
}