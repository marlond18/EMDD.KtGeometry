using KtExtensions;
using EMDD.KtGeometry.KtLines._2D;
using EMDD.KtGeometry.KtPoints;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace KtGeometryTest.KtLines._2D
{
    [TestClass]
    public class KtSegment2DTests
    {
        [TestMethod]
        public void InlineTest()
        {
            var segment = new KtSegment2D(1, 2, -5, -2);
            Assert.IsFalse(segment.Inline(new KtPoint2D(-23, -24)));
            Assert.IsTrue(segment.Inline(new KtPoint2D(-2, 0)));
        }

        [TestMethod]
        public void DistanceTest()
        {
            var segment = new KtSegment2D(1, 2, -5, -2);
            Assert.IsTrue((segment.Distance(new KtPoint2D(20, -5)) + 16.3636557886443).NearZero(8));
        }

        [TestMethod]
        public void PointInsideLineTest()
        {
            var segment = new KtSegment2D(1, 2, -5, -2);
            Assert.AreEqual(segment.PointInsideLine(.5), new KtPoint2D(-2, 0));
        }

        [TestMethod]
        public void EqualsTest()
        {
            var segment = new KtSegment2D(1, 2, -5, -2);
            Assert.AreEqual(segment, new KtSegment2D(1, 2, -5, -2));
        }

        [TestMethod]
        public void EqualsTest1()
        {
            var segment = new KtSegment2D(1, 2, -5, -2);
            Assert.AreNotEqual(segment, new KtLine2D(3, 5, 3, 6));
        }

        [TestMethod]
        public void IntersectionTest()
        {
            var line1 = new KtSegment2D(-1, 2, -.5, 3);
            var line2 = new KtSegment2D(1, 2, .5, 3);
            var line3 = new KtSegment2D(1, 2, -1, 3);
            Assert.AreEqual(line2.IntersectionWith(line1), null);
            Assert.IsFalse(line1.HasIntersection(line2));
            Assert.AreEqual(line3.IntersectionWith(line1), new KtPoint2D(-0.6, 2.8));
            Assert.IsTrue(line1.HasIntersection(line3));
        }

        private const int testIteration = 2000;
        [TestMethod]
        public void TestIntersectionCheckSpeedA()
        {
            var line1 = new KtSegment2D(-1, 2, -.5, 3);
            var line2 = new KtSegment2D(1, 2, -1, 3);
            for (int i = 0; i < testIteration; i++)
            {
                Assert.IsTrue(line1.HasIntersection(line2));
            }
        }

        [TestMethod]
        public void TestIntersectionCheckSpeedB()
        {
            var line1 = new KtSegment2D(-1, 2, -.5, 3);
            var line2 = new KtSegment2D(1, 2, -1, 3);
            for (int i = 0; i < testIteration; i++)
            {
                Assert.IsTrue(line1.IntersectionWith(line2) != null);
            }
        }

        [TestMethod]
        public void IntersectionBarelyTouchingTest()
        {
            var line1 = new KtSegment2D(0, 0, 1, 1);
            var line2 = new KtSegment2D(.5, 0.5, 0, 1);
            var line3 = new KtSegment2D(0, 0, 0, 1);
            Assert.AreEqual(line2.IntersectionWith(line1),  new KtPoint2D(0.5, 0.5));
            Assert.AreEqual(line3.IntersectionWith(line1), new KtPoint2D(0, 0));
        }

        [TestMethod]
        public void LineIntersectionTests()
        {
            var watch = new Stopwatch();
            var intersection = new KtPoint2D(5, 7);
            var line1 = new KtLine2D((1, 1), (3, 4));
            var line2 = new KtLine2D((9, 3), (7, 5));
            var line3 = new KtLine2D((3, 1), (5, 4));
            var ray1 = new KtRay2D((9, 3), (7, 5));
            var ray2 = new KtRay2D((7, 5), (9, 3));
            var ray3 = new KtRay2D((1, 1), (3, 4));
            var ray4 = new KtRay2D((3, 4), (1, 1));
            var segment1 = new KtSegment2D((7, 5), (9, 3));
            var segment2 = new KtSegment2D((4, 8), (9, 3));
            var segment3 = new KtSegment2D((1, 1), (3, 4));
            var segment4 = new KtSegment2D((1, 1), (7, 10));
            watch.Start();
            for (int i = 0; i < 100001; i++)
            {
                // line to line with intersection
#pragma warning disable CS0618 // Type or member is obsolete
                Assert.AreEqual(line1.IntersectWith3(line2), intersection);
                              //line to line parallel
                Assert.IsNull(line1.IntersectWith3(line3));
                // line to ray with intersection
                Assert.AreEqual(line1.IntersectWith3(ray1), new KtPoint2D(5, 7));
                // line to ray no Intersection
                Assert.AreEqual(line1.IntersectWith3(ray2), null);
                //line to segment no intersection
                Assert.AreEqual(line1.IntersectWith3(segment1), null);
                //line to segment with intersection
                Assert.AreEqual(line1.IntersectWith3(segment2), new KtPoint2D(5, 7));
                //ray to ray with intersection
                Assert.AreEqual(ray3.IntersectWith3(ray1), new KtPoint2D(5, 7));
                //ray to ray no intersection
                Assert.AreEqual(ray3.IntersectWith3(ray2), null);
                Assert.AreEqual(ray4.IntersectWith3(ray2), null);
                Assert.AreEqual(ray4.IntersectWith3(ray1), null);
                //ray to segment with intersection
                Assert.AreEqual(ray3.IntersectWith3(segment2), new KtPoint2D(5, 7));
                //ray to segment no intersection
                Assert.AreEqual(ray3.IntersectWith3(segment1), null);
                Assert.AreEqual(ray4.IntersectWith3(segment2), null);
                //segment to segment no intersection
                Assert.AreEqual(segment3.IntersectWith3(segment1), null);
                //segement to segment with intersection
                Assert.AreEqual(segment4.IntersectWith3(segment2), new KtPoint2D(5, 7));
#pragma warning restore CS0618 // Type or member is obsolete
            }
            watch.Stop();
            System.Console.WriteLine(((double)watch.ElapsedMilliseconds).ToString());
            //System.Windows.Forms.MessageBox.Show(watch.ElapsedMilliseconds.ToString());
        }

        [TestMethod]
        public void LineIntersectionTests2()
        {
            var watch = new Stopwatch();
            var intersection = new KtPoint2D(5, 7);
            var line1 = new KtLine2D((1, 1), (3, 4));
            var line2 = new KtLine2D((9, 3), (7, 5));
            var line3 = new KtLine2D((3, 1), (5, 4));
            var ray1 = new KtRay2D((9, 3), (7, 5));
            var ray2 = new KtRay2D((7, 5), (9, 3));
            var ray3 = new KtRay2D((1, 1), (3, 4));
            var ray4 = new KtRay2D((3, 4), (1, 1));
            var segment1 = new KtSegment2D((7, 5), (9, 3));
            var segment2 = new KtSegment2D((4, 8), (9, 3));
            var segment3 = new KtSegment2D((1, 1), (3, 4));
            var segment4 = new KtSegment2D((1, 1), (7, 10));
            watch.Start();
            for (int i = 0; i < 100001; i++)
            {
                // line to line with intersection
                Assert.AreEqual(line1.IntersectionWith(line2), intersection);
                //line to line parallel
                Assert.IsNull(line1.IntersectionWith(line3));
                // line to ray with intersection
                Assert.AreEqual(line1.IntersectionWith(ray1), new KtPoint2D(5, 7));
                // line to ray no Intersection
                Assert.AreEqual(line1.IntersectionWith(ray2), null);
                //line to segment no intersection
                Assert.AreEqual(line1.IntersectionWith(segment1), null);
                //line to segment with intersection
                Assert.AreEqual(line1.IntersectionWith(segment2), new KtPoint2D(5, 7));
                //ray to ray with intersection
                Assert.AreEqual(ray3.IntersectionWith(ray1), new KtPoint2D(5, 7));
                //ray to ray no intersection
                Assert.AreEqual(ray3.IntersectionWith(ray2), null);
                Assert.AreEqual(ray4.IntersectionWith(ray2), null);
                Assert.AreEqual(ray4.IntersectionWith(ray1), null);
                //ray to segment with intersection
                Assert.AreEqual(ray3.IntersectionWith(segment2), new KtPoint2D(5, 7));
                //ray to segment no intersection
                Assert.AreEqual(ray3.IntersectionWith(segment1), null);
                Assert.AreEqual(ray4.IntersectionWith(segment2), null);
                //segment to segment no intersection
                Assert.AreEqual(segment3.IntersectionWith(segment1), null);
                //segement to segment with intersection
                Assert.AreEqual(segment4.IntersectionWith(segment2), new KtPoint2D(5, 7));
            }
            watch.Stop();
            System.Console.WriteLine(((double)watch.ElapsedMilliseconds).ToString());
            //System.Windows.Forms.MessageBox.Show(watch.ElapsedMilliseconds.ToString());
        }

        [TestMethod]
        public void IntersectionBarelyTouchingTest2()
        {
            var line1 = new KtSegment2D(0, 0, 1, 1);
            var line2 = new KtSegment2D(.5, 0.5, 0, 1);
            var line3 = new KtSegment2D(0, 0, 0, 1);
            Assert.AreEqual(line2.IntersectionWith(line1),new KtPoint2D (0.5, 0.5));
            Assert.AreEqual(line3.IntersectionWith(line1), new KtPoint2D(0, 0));
        }

        [TestMethod]
        public void InsectionHorizontalSegmentWithYAxis2()
        {
            var segment = new KtSegment2D(-3, 3, 5, 3);
            var yaxis = KtLine2D.YAxis();
            Assert.AreEqual(yaxis.IntersectionWith(segment), new KtPoint2D(0, 3));
        }

        [TestMethod]
        public void InsectionHorizontalSegmentWithYAxis()
        {
            var segment = new KtSegment2D(-3, 3, 5, 3);
            var yaxis = KtLine2D.YAxis();
#pragma warning disable CS0618 // Type or member is obsolete
            Assert.AreEqual(yaxis.IntersectWith3(segment), new KtPoint2D(0, 3));
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [TestMethod]
        public void IntersectionHorizontalSegmentWithVerticalSegment()
        {
            var segmentH = new KtSegment2D(-3, 3, 5, 3);
            var segmentV = new KtSegment2D(2, 0, 2, 5);
#pragma warning disable CS0618 // Type or member is obsolete
            Assert.AreEqual(segmentV.IntersectWith3(segmentH), new KtPoint2D(2, 3));
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [TestMethod]
        public void IntersectionHorizontalSegmentWithVerticalSegment2()
        {
            var segmentH = new KtSegment2D(-3, 3, 5, 3);
            var segmentV = new KtSegment2D(2, 0, 2, 5);
            Assert.AreEqual(segmentV.IntersectionWith(segmentH), new KtPoint2D(2, 3));
        }

        [TestMethod]
        public void CloneTest()
        {
            var segment = new KtSegment2D(3, 2, 4, 7);
            Assert.AreEqual(segment, segment.Clone());
        }
    }
}