using EMDD.KtGeometry.Angles;

using EMDD.KtGeometry.KtPoints;
using EMDD.KtGeometry.KtVectors;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace KtGeometryTest.KtVectors
{
    [TestClass]
    public class KtVector2DTests
    {
        [TestMethod]
        public void KtVector2DTest()
        {
            var vector = new KtVector2D(2, 4);
            Assert.AreEqual(vector, new KtVector2D(2, 4));
        }

        [TestMethod]
        public void KtVector2DTest1()
        {
            var vector = new KtVector2D(Degrees.Create(45));
            Assert.AreEqual(vector, new KtVector2D(Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
        }

        [TestMethod]
        public void NormalizeTest()
        {
            var vector = new KtVector2D(Degrees.Create(45)) * 20;
            vector.Normalize();
            Assert.AreEqual(vector, new KtVector2D(Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
        }

        [TestMethod]
        public void RotateTest()
        {
            var vector = new KtVector2D(1, 0);
            vector.Rotate( Degrees.Create(45));
            Assert.AreEqual(vector, new KtVector2D(Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            var vector2 = new KtVector2D(-1, 1);
            vector2.Rotate(Degrees.Create(-135));
            Assert.AreEqual(vector2, new KtVector2D(Math.Sqrt(2), 0));
        }

        [TestMethod]
        public void AngleBetweenTest()
        {
            var vector = new KtVector2D(3, 2);
            var vector2 = new KtVector2D(2, 3);
            Assert.AreEqual(vector.AngleBetween(vector2), Degrees.Create(-(Math.Atan2(2, 3) - Math.Atan2(3, 2)) * 180 / Math.PI));
            Assert.AreEqual(vector2.AngleBetween(vector), Degrees.Create((Math.Atan2(2, 3) - Math.Atan2(3, 2)) * 180 / Math.PI));
        }

        [TestMethod]
        public void MagnitudeTest()
        {
            var vector = new KtVector2D(3, 2);
            Assert.AreEqual(vector.Magnitude, Math.Sqrt((3 * 3) + (2 * 2)));
        }

        [TestMethod]
        public void DirectionTest()
        {
            var vector = new KtVector2D(3, 2);
            Assert.AreEqual(vector.Direction, Degrees.Create(Math.Atan2(2, 3) * 180 / Math.PI));
        }

        [TestMethod]
        public void DotTest()
        {
            var vector = new KtVector2D(3, 2);
            Assert.AreEqual(vector.Dot(vector), (3 * 3) + (2 * 2));
        }

        [TestMethod]
        public void CrossTest()
        {
            var vector = new KtVector2D(3, 2);
            Assert.AreEqual(vector.Cross(vector), 0);
        }

        [TestMethod]
        public void CloneTest()
        {
            var vector = new KtVector2D(3, 2);
            Assert.AreEqual(vector.Clone(), vector);
        }

        [TestMethod]
        public void ToKtPoint2DTest()
        {
            var vector = new KtVector2D(3, 2);
            Assert.AreEqual(vector.ToKtPoint2D(), new KtPoint2D(3, 2));
        }
    }
}