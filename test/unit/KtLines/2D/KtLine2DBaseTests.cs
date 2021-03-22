using KtExtensions;
using  EMDD.KtGeometry.KtLines._2D;
using  EMDD.KtGeometry.KtLines._3D;
using  EMDD.KtGeometry.KtPoints;
using  EMDD.KtGeometry.KtVectors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace KtGeometryTest.KtLines._2D
{
    [TestClass]
    public class KtLine3DTests
    {
        [TestMethod]
        public void DistanceBetweenLines()
        {
            var line1 = new KtLine3D(new KtPoint3D(2, 6, -9), new KtVector3D(3, 4, -4));
            var line2 = new KtLine3D(new KtPoint3D(-1, -2, 3), new KtVector3D(2, -6, 1));
            var actual = line2.Distance(line1);
            Assert.IsTrue(actual.NearEqual(4.74020116673185d));
        }

        [TestMethod]
        public void DistanceBetweenLines2()
        {
            var line1 = new KtLine3D(new KtPoint3D(-2, 1, 0), new KtVector3D(2, 3, 1));
            var line2 = new KtLine3D(new KtPoint3D(3, 0, -1), new KtVector3D(-1, 1, 2));
            var actual = line2.Distance(line1);
            var expected = 5d * Math.Sqrt(3) / 3;
            Assert.IsTrue(actual.NearEqual(expected));
        }

        [TestMethod]
        public void DistanceIntersectionBetweenLines()
        {
            var line1 = new KtLine3D(new KtPoint3D(-2, 1, 0), new KtVector3D(2, 3, 1));
            var line2 = new KtLine3D(new KtPoint3D(3, 0, -1), new KtVector3D(-1, 1, 2));
            var (actualDistance, inter1, inter2) = line2.DistanceIntersection(line1);
            var expected = 5d * Math.Sqrt(3) / 3;
            Assert.IsTrue(actualDistance.NearEqual(expected));
            Assert.AreEqual(inter1, new KtPoint3D(1.2666666666666666666666666666666666666666666666666666666666666666666666666666666666d, 1.733333333333333333333333333333333333333333d, 2.4666666666666666666666666d));
            Assert.AreEqual(inter2, new KtPoint3D(-0.4, 3.4, 0.8));
        }

        [TestMethod]
        public void IntersectionBetweenLines3()
        {
            var line1 = new KtLine3D(new KtPoint3D(-2, 1, 0), new KtVector3D(2, 3, 1));
            var line2 = new KtLine3D(new KtPoint3D(3, 0, -1), new KtVector3D(-1, 1, 2));
            var (inter1, inter2) = line2.Intersect2(line1);
            Assert.AreEqual(inter1, new KtPoint3D(1.2666666666666666666666666666666666666666666666666666666666666666666666666666666666d, 1.733333333333333333333333333333333333333333d, 2.4666666666666666666666666d));
            Assert.AreEqual(inter2, new KtPoint3D(-0.4, 3.4, 0.8));
        }

        [TestMethod]
        public void IntersectionBetweenLines4()
        {
            var line1 = new KtLine3D(new KtPoint3D(-2, 1, 0), new KtVector3D(2, 3, 1));
            var line2 = new KtLine3D(new KtPoint3D(3, 0, -1), new KtVector3D(-1, 1, 2));
            var (inter1, inter2) = line2.Intersect(line1);
            Assert.AreEqual(inter1, new KtPoint3D(1.2666666666666666666666666666666666666666666666666666666666666666666666666666666666d, 1.733333333333333333333333333333333333333333d, 2.4666666666666666666666666d));
            Assert.AreEqual(inter2, new KtPoint3D(-0.4, 3.4, 0.8));
        }

        [TestMethod]
        public void IntersectionBetweenSegmentsNoIntersection()
        {
            var line1 = new KtLineSegment3D(new KtPoint3D(-3.62375, 2.39651, 0), new KtPoint3D(-0.47586, 5.02421, 2.80943));
            var line2 = new KtLineSegment3D(new KtPoint3D(-1.89282, -4.87833, 0), new KtPoint3D(-6.6778, -2.78548, -2));
            var (inter1, inter2) = line2.Intersect(line1);
            Assert.IsNull(inter1);
            Assert.IsNull(inter2);
        }

        [TestMethod]
        public void Timetester()
        {
            var line1 = new KtLine3D(new KtPoint3D(-2, 1, 0), new KtVector3D(2, 3, 1));
            var line2 = new KtLine3D(new KtPoint3D(3, 0, -1), new KtVector3D(-1, 1, 2));
            var timer = new Stopwatch();
            var total1 = 0d;
            var total2 = 0d;
            for (int j = 0; j < 100; j++)
            {
                timer.Start();
                for (int i = 0; i < 100000; i++)
                {
                    var (_, _) = line2.Intersect2(line1);
                }
                timer.Stop();
                //Console.WriteLine("Elapsed time {0} ms for Intersect1", timer.ElapsedMilliseconds);
                total2 += timer.ElapsedMilliseconds;
                timer.Reset();

                timer.Start();
                for (int i = 0; i < 100000; i++)
                {
                    var (_, _) = line2.Intersect(line1);
                }
                timer.Stop();
                //Console.WriteLine("Elapsed time {0} ms for Intersect2", timer.ElapsedMilliseconds);
                total1 += timer.ElapsedMilliseconds;
                timer.Reset();
            }
            Console.WriteLine("Elapsed time {0} ms for Intersect1", total1 / 100);
            Console.WriteLine("Elapsed time {0} ms for Intersect2", total2 / 100);
        }

        [TestMethod]
        public void IntersectionBetweenLines()
        {
            var line1 = new KtLine3D(new KtPoint3D(6, 8, 4), (new KtPoint3D(12, 15, 4) - new KtPoint3D(6, 8, 4)).ToKtVector3D());
            var line2 = new KtLine3D(new KtPoint3D(6, 8, 2), (new KtPoint3D(12, 15, 6) - new KtPoint3D(6, 8, 2)).ToKtVector3D());
            var (a, b) = line2.Intersect(line1);
            var expected = new KtPoint3D(9, 23d / 2, 4);
            Assert.AreEqual(a, expected);
            Assert.AreEqual(b, expected);
        }

        [TestMethod]
        public void IntersectionBetweenLines2()
        {
            var line1 = new KtLine3D(new KtPoint3D(6, 8, 4), (new KtPoint3D(12, 15, 4) - new KtPoint3D(6, 8, 4)).ToKtVector3D());
            var line2 = new KtLine3D(new KtPoint3D(6, 8, 2), (new KtPoint3D(12, 15, 6) - new KtPoint3D(6, 8, 2)).ToKtVector3D());
            var (a, b) = line2.Intersect2(line1);
            var expected = new KtPoint3D(9, 23d / 2, 4);
            Assert.AreEqual(a, expected);
            Assert.AreEqual(b, expected);
        }
    }
    [TestClass]
    public class KtLine2DBaseTests
    {
        [TestMethod]
        public void TestContainsPoint()
        {
            var segment = new KtSegment2D((0, 0), (1, 2));
            Assert.IsFalse(segment.HasThePoint((-1, -2)));
            Assert.IsFalse(segment.HasThePoint((2, 4)));
            Assert.IsTrue(segment.HasThePoint((.5, 1)));
            Assert.IsFalse(segment.HasThePoint((0, 1)));
            var ray = new KtRay2D((0, 0), (1, 2));
            Assert.IsFalse(ray.HasThePoint((-1, -2)));
            Assert.IsTrue(ray.HasThePoint((2, 4)));
            Assert.IsTrue(ray.HasThePoint((.5, 1)));
            Assert.IsFalse(ray.HasThePoint((0, 1)));
            var line = new KtLine2D((0, 0), (1, 2));
            Assert.IsTrue(line.HasThePoint((-1, -2)));
            Assert.IsTrue(line.HasThePoint((2, 4)));
            Assert.IsTrue(line.HasThePoint((.5, 1)));
            Assert.IsFalse(line.HasThePoint((0, 1)));
        }

        [TestMethod]
        public void TestIntersectionsOfLineTypes()
        {
            var line = new KtLine2D(2, 4, 5);
            var segment = new KtSegment2D(0, 2, 30, 4);
#pragma warning disable CS0618 // Type or member is obsolete
            Assert.AreEqual(line.IntersectWith3(segment), null);
            segment = new KtSegment2D(0, 4, -2, 2);
            Assert.AreEqual(line.IntersectWith3(segment), new KtPoint2D(-11.0 / 6, 13.0 / 6));
            segment = new KtSegment2D(-2, 2, 0, 4);
            Assert.AreEqual(line.IntersectWith3(segment), new KtPoint2D(-11.0 / 6, 13.0 / 6));
            var ray = new KtRay2D(new KtPoint2D(0, 2), new KtVector2D(1, 2));
            Assert.AreEqual(line.IntersectWith3(ray), null);
            ray = new KtRay2D(new KtPoint2D(0, 2), new KtVector2D(1, -2));
            Assert.AreEqual(line.IntersectWith3(ray), new KtPoint2D(.5, 1.0));
            var yaxis = KtLine2D.YAxis();
            var line2 = new KtRay2D(new KtPoint2D(0, 20), new KtVector2D(3, 4));
            Assert.AreEqual(yaxis.IntersectWith3(line2), new KtPoint2D(0, 20));
            var line45 = new KtRay2D(new KtPoint2D(), new KtVector2D(30, 30));
            var line45N = new KtRay2D(new KtPoint2D(0, 50), new KtVector2D(30, -30));
            Assert.AreEqual(line45.IntersectWith3(line45N), new KtPoint2D(25, 25));
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [TestMethod]
        public void Inequality()
        {
            var line = new KtLine2D(0, 0, 20, 1);
            var segment = new KtSegment2D(0, 0, 20, 1);
            var ray = new KtRay2D(new KtPoint2D(0, 0), new KtPoint2D(20, 1));
            Assert.AreNotEqual(line, segment);
            Assert.AreNotEqual(line, ray);
            Assert.AreNotEqual(segment, ray);
        }
    }
}