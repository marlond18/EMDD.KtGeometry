using EMDD.KtGeometry.KtVectors;
using EMDD.KtGeometry.Quaternions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace KtGeometryTest.KtVectors
{
    [TestClass]
    public class KtVector3DTests
    {
        [TestMethod]
        public void DotProductTest()
        {
            var vec1 = new KtVector3D(3, -1, 4);
            var yaxis = new KtVector3D(0, 1, 0);
            var xaxis = new KtVector3D(1, 0, 0);
            var dot = vec1.Cross(yaxis);
            var dotx = vec1.Cross(xaxis);
            Console.WriteLine(dot);
            Console.WriteLine(dotx);
            Assert.AreEqual(dot, new KtVector3D(-4, 0, 3));
        }

        [TestMethod]
        public void RotationTest()
        {
            var expected = new KtVector3D(2, 3, 4).Normalize();
            var initial = new KtVector3D(0, 0, 1);
            var angle = expected.AngleBetween(initial);
            var perp2 = expected.Cross(initial).Cross(initial).Cross(initial).Normalize();
            var q4 = new KtQuaternion(angle, perp2);
            var q4p = expected.RotationFrom(initial);
            Assert.AreEqual(q4, q4p);
            Assert.AreEqual(expected, initial.RotateBy(q4p));
        }
    }
}