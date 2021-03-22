using EMDD.KtGeometry.KtPoints;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;

namespace KtGeometryTest.KtPoints
{
    [TestClass]
    public class KtPoint2DTest
    {
        [TestMethod]
        public void TestQuadrant()
        {
            Assert.AreEqual(new KtPoint2D().Quadrant(), Quadrant2D.Origin);
            Assert.AreEqual(new KtPoint2D(1, 0).Quadrant(), Quadrant2D.Posx);
            Assert.AreEqual(new KtPoint2D(0, 1).Quadrant(), Quadrant2D.Posy);
            Assert.AreEqual(new KtPoint2D(-1, 0).Quadrant(), Quadrant2D.Negx);
            Assert.AreEqual(new KtPoint2D(0, -1).Quadrant(), Quadrant2D.Negy);
            Assert.AreEqual(new KtPoint2D(1, 1).Quadrant(), Quadrant2D.I);
            Assert.AreEqual(new KtPoint2D(-1, 1).Quadrant(), Quadrant2D.Ii);
            Assert.AreEqual(new KtPoint2D(-1, -1).Quadrant(), Quadrant2D.Iii);
            Assert.AreEqual(new KtPoint2D(1, -1).Quadrant(), Quadrant2D.Iv);
        }

        [TestMethod]
        public void Casting2DPointToTuple()
        {
            var list = new List<KtPoint2D>() { (1, 1), (2, 3), (3, 4) };
            _ = ((double x, double y))list[0];
            //var list2 = list.Cast<ValueTuple<double, double>>().ToList();

        }
    }
}