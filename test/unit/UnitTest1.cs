using EMDD.KtGeometry.KtPoints;
using EMDD.KtGeometry.KtPolygons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KtExtensions;
using System.Diagnostics;
using System.Linq;

namespace KtGeometryTest
{
    [TestClass]
    public class OtherTests
    {
        [TestMethod]
        public void SplitFirstTest()
        {
            const int testRun = 100;
            var testList = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < testRun; i++)
            {
                var (take, remaining) = testList.SplitFirst(t => t == 5);
                Assert.AreEqual(take, 5);
                Assert.IsTrue(remaining.SequenceEqual(new[] { 1, 2, 3, 4, 6, 7, 8, 9, 10 }));
            }
            watch.Stop();
            System.Console.WriteLine(watch.ElapsedMilliseconds.ToString());
            watch.Start();
            for (int i = 0; i < testRun; i++)
            {
                var first = testList.First(t => t == 5);
                var remaining = testList.Where(e => e != first);
                Assert.AreEqual(first, 5);
                Assert.IsTrue(remaining.SequenceEqual(new[] { 1, 2, 3, 4, 6, 7, 8, 9, 10 }));
            }
            watch.Stop();
            System.Console.WriteLine(watch.ElapsedMilliseconds.ToString());
        }
    }

    [TestClass]
    public class KtRegion2DMethodTest
    {
        [TestMethod]
        public void CreatingRectangularRegion()
        {
            var solid = CreateSpecialRegionMethods.Rectangle(new KtPoint2D(), 20, 30, true);
            Assert.AreEqual(solid.Count, 4);
            Assert.AreEqual(solid.Area(), 600.0);
            Assert.AreEqual(solid.Centroid(), new KtPoint2D());
            var hollow = CreateSpecialRegionMethods.Rectangle(new KtPoint2D(), 20, 30, false);
            Assert.AreEqual(hollow.Count, 4);
            Assert.AreEqual(hollow.Area(), -600.0);
            Assert.AreEqual(hollow.Centroid(), new KtPoint2D());
        }

        [TestMethod]
        public void CreatingRegularPolygon()
        {
            var solid = CreateSpecialRegionMethods.RegularPolygon(new KtPoint2D(), 7, 30, true);
            Assert.AreEqual(solid.Count, 7);
            Assert.AreEqual(solid.Centroid(), new KtPoint2D());
            var hollow = CreateSpecialRegionMethods.RegularPolygon(new KtPoint2D(), 7, 30, false);
            Assert.AreEqual(hollow.Count, 7);
            Assert.AreEqual(hollow.Centroid(), new KtPoint2D());
        }
    }
}