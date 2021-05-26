using EMDD.KtGeometry.KtPoints;
using EMDD.KtGeometry.KtPolygons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using PolygonMethods = EMDD.KtGeometry.KtPolygons.CreateSpecialRegionMethods;
using KtExtensions;
using CircularStack;
using EMDD.KtGeometry.KtLines._2D;
using EMDD.KtGeometry.Angles;
using EMDD.KtGeometry.KtPolygons.Regions;

namespace KtGeometryTest.KtPolygons
{
    [TestClass]
    public class ListToStackTest
    {
        [TestMethod]
        public void ListToStackTest1()
        {
            var L = new List<int>();
            for (int i = 0; i < 200; i++)
            {
                L.Add(i);
            }
            var stack = (CircularStack<int>)L;
            Assert.AreEqual(stack[0].Previous.Value, L[199]);
            Assert.AreEqual(stack[0].Value, 0);
            Assert.AreEqual(stack.Count, 200);
        }
    }

    [TestClass]
    public class SplitTests
    {
        private const int limit = 7000000;
        [TestMethod]
        public void SplitTest1()
        {
            var number = CreateAListOfnumbers();
            var grouped = number.GroupBy(i => i % 2 == 0).ToList();
            var odd = grouped.First(i => !i.Key).ToList();
            var even = grouped.First(i => i.Key).ToList();
            var expectedEven = CreateEvenNumber().ToList();
            var expectedOdd = CreateOddNumber().ToList();
            Assert.IsTrue(expectedEven.SequenceEqual(even));
            Assert.IsTrue(expectedOdd.SequenceEqual(odd));
        }

        [TestMethod]
        public void SplitTest2()
        {
            var number = CreateAListOfnumbers();
            var (odd, even) = number.Fork(i => i % 2 == 1);
            var expectedEven = CreateEvenNumber().ToList();
            var expectedOdd = CreateOddNumber().ToList();
            Assert.IsTrue(expectedEven.SequenceEqual(even));
            Assert.IsTrue(expectedOdd.SequenceEqual(odd));
        }

        [TestMethod]
        public void SplitTest3()
        {
            var number = CreateAListOfnumbers();
            var odd = number.Where(i => i % 2 == 1).ToList();
            var even = number.Except(odd).ToList();
            var expectedEven = CreateEvenNumber().ToList();
            var expectedOdd = CreateOddNumber().ToList();
            Assert.IsTrue(expectedEven.SequenceEqual(even));
            Assert.IsTrue(expectedOdd.SequenceEqual(odd));
        }

        private static IEnumerable<int> CreateAListOfnumbers()
        {
            for (int i = 1; i < limit; i++) yield return i;
        }

        private static IEnumerable<int> CreateOddNumber()
        {
            for (int i = 1; i < limit; i += 2) yield return i;
        }

        private static IEnumerable<int> CreateEvenNumber()
        {
            for (int i = 2; i < limit; i += 2) yield return i;
        }
    }

    [TestClass]
    public class KtSolidRegion2DTests
    {
        [TestMethod]
        public void MyTestMethod()
        {
            var list2DP1 = new List<KtPoint2D>
            {
                new KtPoint2D(),
                new KtPoint2D(171.000, 198.000),
                new KtPoint2D(288.000, 192.000),
                new KtPoint2D(269.000, 87.000),
                new KtPoint2D(140.000, 123.000),
                new KtPoint2D(148.000, 214.000),
                new KtPoint2D(221.000, 209.000),
                new KtPoint2D(117.000, 41.000),
                new KtPoint2D(112.000, 159.000),
                new KtPoint2D(316.000, 149.000),
                new KtPoint2D(223.000, 84.000)
            };
            var list2DP2 = new List<KtPoint2D>
            {
                new KtPoint2D(),
                new KtPoint2D(74.000, 177.000),
                new KtPoint2D(321.000, 177.000),
                new KtPoint2D(66.000, 53.000)
            };
            _ = list2DP1;
            _ = list2DP2;
        }

        [TestMethod]
        public void EqualsTest()
        {
            var solid = new KtSolidRegion(CreateArbitraryRegion());
            var solid2 = new KtSolidRegion(CreateArbitraryRegion());
            Assert.AreEqual(solid2, solid);
        }

        private static List<KtPoint2D> CreateArbitraryRegion()
        {
            return new List<KtPoint2D>
            {
                new KtPoint2D(0, 4),
                new KtPoint2D(5, 5),
                new KtPoint2D(0, 6),
                new KtPoint2D(-1, -1),
                new KtPoint2D(3, -4)
            };
        }

        [TestMethod]
        public void AddTest()
        {
            //var listPoint = CreateArbitraryRegion();
            //var region = new KtRegion(listPoint)
            //{ new KtPoint2D(-20, -40) };
            //Assert.AreEqual(region.Corners[0].Previous.Value, new KtPoint2D(-20, -40));
        }

        [TestMethod]
        public void AreaTest()
        {
            var region = PolygonMethods.Rectangle(new KtPoint2D(2, 2), 40, 30, true);
            Assert.AreEqual(region.Area(), 40 * 30);
            KtRegion rec = (new List<KtPoint2D>
            {
                new KtPoint2D(0, 0),
                new KtPoint2D(20, 0),
                new KtPoint2D(20, 5),
                new KtPoint2D(0, 5)
            });
            Assert.AreEqual(rec.Area(), 20 * 5.0);
            KtRegion triangle = (new List<KtPoint2D>
            {
                new KtPoint2D(0, 0),
                new KtPoint2D(20, 5),
                new KtPoint2D(0, 10)
            });
            Assert.AreEqual(triangle.Area(), 20 * 10.0 / 2);
            KtRegion triangle2 = (new List<KtPoint2D>
            {
                new KtPoint2D(0, 0),
                new KtPoint2D(20, 0),
                new KtPoint2D(0, 10)
            });
            Assert.AreEqual(triangle2.Area(), 20 * 10.0 / 2);
        }

        [TestMethod]
        public void AreaCentroidTest()
        {
            var region = PolygonMethods.Rectangle(new KtPoint2D(2, 2), 40, 30, true);
            Assert.AreEqual(region.AreaCentroid(), new KtPoint2D(2, 2) * 30 * 40);
        }

        [TestMethod]
        public void CentroidTest()
        {
            var region = PolygonMethods.Rectangle(new KtPoint2D(2, 2), 40, 30, true);
            Assert.AreEqual(region.Centroid(), new KtPoint2D(2, 2));
            KtRegion rec = (new List<KtPoint2D>
            {
                new KtPoint2D(0, 0),
                new KtPoint2D(20, 0),
                new KtPoint2D(20, 5),
                new KtPoint2D(0, 5)
            });
            Assert.AreEqual(rec.Centroid(), new KtPoint2D(10, 2.5));
            KtRegion triangle = (new List<KtPoint2D>
            {
                new KtPoint2D(0, 0),
                new KtPoint2D(20, 0),
                new KtPoint2D(0, 10)
            });
            Assert.AreEqual(triangle.Centroid(), new KtPoint2D(20.0 / 3, 10.0 / 3));
        }

        [TestMethod]
        public void ContainsTest()
        {
            var region = PolygonMethods.Rectangle(new KtPoint2D(2, 2), 40, 30, true);
            Assert.AreEqual(region.Contains(new KtPoint2D(2 - 20, 2 - 15)), true);
        }

        [TestMethod]
        public void CornerCountTest()
        {
            var region = PolygonMethods.Rectangle(new KtPoint2D(2, 2), 40, 30, true);
            Assert.AreEqual(region.Count, 4);
        }

        [TestMethod]
        public void ClearTest()
        {
            var region = PolygonMethods.Rectangle(new KtPoint2D(2, 2), 40, 30, true);
            region.Clear();
            Assert.AreEqual(region.Count, 0);
        }

        [TestMethod]
        public void IsConvexTest()
        {
            var region = PolygonMethods.Rectangle(new KtPoint2D(2, 2), 40, 30, true);
            Assert.AreEqual(region.IsConvex, false);
            KtRegion region2 = (CreateArbitraryRegion());
            Assert.AreEqual(region2.IsConvex, true);
            KtRegion reg = (new List<KtPoint2D>
            {
                new KtPoint2D(0, 0),
                new KtPoint2D(0, 10),
                new KtPoint2D(5, 10),
                new KtPoint2D(0, 20),
                new KtPoint2D(10, 20),
                new KtPoint2D(10, 0)
            });
            Assert.AreEqual(reg.IsConvex, true);
            KtRegion rec = (new List<KtPoint2D>
            {
                new KtPoint2D(0, 0),
                new KtPoint2D(20, 0),
                new KtPoint2D(20, 5),
                new KtPoint2D(0, 5)
            });
            Assert.AreEqual(rec.IsConvex, false);
        }

        [TestMethod]
        public void PointInRegionTest()
        {
            var region = PolygonMethods.Rectangle(new KtPoint2D(2, 2), 40, 30, true);
            Assert.AreEqual(region.Inscribes(new KtPoint2D(3, 10)), true);
        }

        [TestMethod]
        public void PointInCornerOfRegionTest()
        {
            var region = PolygonMethods.Rectangle(new KtPoint2D(2, 2), 40, 30, true);
            Assert.AreEqual(region.Inscribes(new KtPoint2D(22, -13)), false);
            Assert.AreEqual(region.Inscribes(new KtPoint2D(22, 17)), false);
            Assert.AreEqual(region.Inscribes(new KtPoint2D(-18, 17)), false);
            Assert.AreEqual(region.Inscribes(new KtPoint2D(-18, -13)), false);
            Assert.IsFalse(region.Inscribes(new KtPoint2D(22, -14)));
            Assert.IsFalse(region.Inscribes(new KtPoint2D(22, 18)));
            Assert.IsFalse(region.Inscribes(new KtPoint2D(-18, 18)));
            Assert.IsFalse(region.Inscribes(new KtPoint2D(-18, -14)));
        }

        [TestMethod]
        public void RemoveTest()
        {
        }

        [TestMethod]
        public void FixCornersTest()
        {
        }

        [TestMethod]
        public void CloneTest()
        {
        }
    }

    [TestClass]
    public class PolygonCreations
    {
        [TestMethod]
        public void CreatePolygon1b()
        {
            var actualPolygon = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (433, 252), (359, 328), (128, 224), (99, 70), (211, 115), (187, 213), (340, 262), (419, 173) }),
                new KtSolidRegion(new KtPoint2D[] { (91, 42), (525, 197), (457, 332), (58, 87) })
            };
            var expectedPolygon = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (525, 197), (457, 332), (393.2311471, 292.8436868), (359, 328), (128, 224), (107.9805488, 117.6898106), (58, 87), (91, 42) }),
                new KtHollowRegion(new KtPoint2D[] { (187, 213), (340, 262), (341.0583057, 260.8077316), (196.9607843, 172.3267974) })
            };
            Assert.AreEqual(expectedPolygon, actualPolygon);
        }

        [TestMethod]
        public void CreatePolygonAndCutInside()
        {
            var actualPolygon = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (524, 351), (69, 346), (3, 42), (514, 41), (555, 271) }),
                new KtHollowRegion(new KtPoint2D[] { (98, 330), (511, 338), (539, 266), (501, 62), (25, 63) }),
                new KtSolidRegion(new KtPoint2D[] { (123, 307), (42, 78), (488, 72), (523, 269), (488, 322) })
            };
            Assert.AreEqual(2, actualPolygon.Count());
        }

        [TestMethod]
        public void CreatePolygon2b()
        {
            var actualPolygon = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (524, 351), (69, 346), (3, 42), (514, 41), (555, 271) }),
                new KtHollowRegion(new KtPoint2D[] { (98, 330), (511, 338), (539, 266), (501, 62), (25, 63) }),
                new KtSolidRegion(new KtPoint2D[] { (123, 307), (42, 78), (488, 72), (523, 269), (488, 322) }),
                new KtHollowRegion(new KtPoint2D[] { (139, 292), (474, 307), (507, 262), (475, 85), (70, 98) }),
                new KtSolidRegion(new KtPoint2D[] { (167, 278), (101, 129), (449, 101), (477, 260), (445, 292) }),
                new KtHollowRegion(new KtPoint2D[] { (236, 269), (400, 269), (455, 254), (431, 121), (145, 146), (190, 250) }),
                new KtSolidRegion(new KtPoint2D[] { (467, 377), (407, 376), (500, 98) })
            };
            var expectedPolygon = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (555, 271), (524, 351), (470.1452679, 350.4081898), (467, 377), (407, 376), (415.7612146, 349.8105628), (69, 346), (3, 42), (514, 41) }),
                new KtHollowRegion(new KtPoint2D[] { (25, 63), (98, 330), (420.2999706, 336.2430987), (425.9182424, 319.4486949), (123, 307), (42, 78), (488, 72), (495.1794232, 112.4098963), (500, 98), (497.050166, 122.9395057), (523, 269), (488, 322), (473.5754911, 321.407212), (471.7029379, 337.2387978), (511, 338), (539, 266), (501, 62) }),
                new KtHollowRegion(new KtPoint2D[] { (70, 98), (139, 292), (430.7308649, 305.062576), (435.2647291, 291.5097346), (167, 278), (101, 129), (449, 101), (466.2421249, 198.9106376), (485.2964862, 141.9524391), (475, 85) }),
                new KtHollowRegion(new KtPoint2D[] { (475.525641, 304.9195803), (507, 262), (491.0422507, 173.733699) }),
                new KtHollowRegion(new KtPoint2D[] { (145, 146), (190, 250), (236, 269), (400, 269), (447.0913998, 256.156891), (452.4816449, 240.0441154), (431, 121) })
            };
            Assert.AreEqual(expectedPolygon, actualPolygon);
        }

        [TestMethod]
        public void CreatePolygon3a()
        {
            var actualPolygon = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (437, 331), (196, 360), (13, 167), (25, 19), (428, 136) }),
                new KtHollowRegion(new KtPoint2D[] { (207, 281), (318, 300), (367, 236), (98, 86), (71, 163), (92, 212) }),
                new KtSolidRegion(new KtPoint2D[] { (370, 300), (353, 316), (244, 237) })
            };
            var expectedPolygon = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (428, 136), (437, 331), (196, 360), (13, 167), (25, 19) }),
                new KtHollowRegion(new KtPoint2D[] { (71, 163), (92, 212), (207, 281), (318, 300), (322.612243, 293.9758459), (244, 237), (332.3954802, 281.1977401), (367, 236), (98, 86) })
            };
            Assert.AreEqual(expectedPolygon, actualPolygon);
        }

        [TestMethod]
        public void CreatePolygon3Polygons2HolesIntersecting1()
        {
            var actualPolygon = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (497, 209), (355, 348), (95, 335), (109, 98), (436, 95) }),
                new KtHollowRegion(new KtPoint2D[] { (301, 305), (455, 208), (421, 144), (166, 137), (151, 295), (207, 321) }),
                new KtHollowRegion(new KtPoint2D[] { (248, 232), (253, 281), (395, 285) })
            };
            var expectedPolygon = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (497, 209), (355, 348), (95, 335), (109, 98), (436, 95) }),
                new KtHollowRegion(new KtPoint2D[] { (151, 295), (207, 321), (301, 305), (335.4172342, 283.3216122), (395, 285), (355.4127381, 270.7270416), (455, 208), (421, 144), (166, 137) })
            };
            Assert.AreEqual(expectedPolygon, actualPolygon);
        }

        [TestMethod]
        public void CreatePolygon4a()
        {
            var actualPolygon = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (506, 173), (451, 350), (25, 373), (-5, 67), (121, 146), (139, 261), (392, 290), (427, 152), (450, 89) }),
                new KtSolidRegion(new KtPoint2D[] { (6, 42), (17, 43), (536, 49), (547, 198), (525, 256), (-19, 156) }),
                new KtSolidRegion(new KtPoint2D[] { (502, 199), (540, 167), (577, 258), (570, 368), (505, 396), (396, 384), (391, 323), (511, 353), (540, 270) }),
                new KtSolidRegion(new KtPoint2D[] { (509, 113), (566, 99), (575, 132), (564, 162), (555, 112), (514, 127) }),
                new KtHollowRegion(new KtPoint2D[] { (578, 118), (284, 293), (309, 312) }),
                new KtSolidRegion(new KtPoint2D[] { (579, 130), (451, 394), (425, 394), (567, 136) })
            };
            var expectedPolygon = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (17, 43), (536, 49), (540.1598242, 105.3467098), (566, 99), (572.1340782, 121.4916201), (558.2012903, 129.7849462), (555, 112), (541.028371, 117.1115716), (542.647482, 139.0431654), (388.3582022, 230.8820225), (126.759052, 182.7939434), (139, 261), (305.6653851, 280.1039374), (284, 293), (309, 312), (346.7043581, 284.8080094), (392, 290), (403.7512858, 243.666359), (542.9595878, 143.2707806), (545.3295548, 175.3730622), (561.2126937, 146.5149649), (558.5985786, 131.9921032), (572.3024569, 122.1090088), (575, 132), (579, 130), (565.5982906, 157.6410256), (564, 162), (563.8605287, 161.2251595), (549.6059021, 190.6253269), (577, 258), (570, 368), (505, 396), (452.8157825, 390.2549485), (451, 394), (425, 394), (428.5326225, 387.5815731), (396, 384), (393.4677215, 353.1062028), (25, 373), (4.1425634, 160.2541477), (-19, 156), (-3.2953929, 84.3869919), (-5, 67), (-0.1493971, 70.041251), (6, 42) }),
                new KtHollowRegion(new KtPoint2D[] { (525, 256), (518.4893617, 254.8031915), (475.2162162, 344.054054), (511, 353), (540, 270), (528.1136484, 247.7912905) }),
                new KtHollowRegion(new KtPoint2D[] { (454.4600262, 338.8650066), (455.2385008, 339.0596253), (503.1628449, 251.9858171), (482.6292687, 248.2112626) }),
                new KtHollowRegion(new KtPoint2D[] { (475.2162162, 344.054054), (511, 353), (540, 270), (528.1136484, 247.7912905), (525, 256), (518.4893617, 254.8031915) }),
                new KtHollowRegion(new KtPoint2D[] { (454.4600262, 338.8650066), (455.2385008, 339.0596253), (503.1628449, 251.9858171), (482.6292687, 248.2112626) })
            };
            Assert.AreEqual(expectedPolygon, actualPolygon);
        }

        [TestMethod]
        public void CloneASolidRegionTest()
        {
            var reg = new KtSolidRegion(new KtPoint2D[] { (1, 1), (5, 1), (20, -11) });
            Assert.AreEqual(reg, reg.Clone());
        }

        [TestMethod]
        public void TestPOint()
        {
            var n = new KtPoint2D[] { (25, 63), (98, 330), (420.2999706, 336.2430987), (425.9182424, 319.4486949), (123, 307), (42, 78), (488, 72), (495.1794232, 112.4098963), (500, 98), (497.050166, 122.9395057), (523, 269), (488, 322), (473.5754911, 321.407212), (471.7029379, 337.2387978), (511, 338), (539, 266), (501, 62) };
            _ = new KtHollowRegion(n);
        }
    }

    [TestClass]
    public class RegionMethodsTest
    {
        [TestMethod]
        public void PointIsIncribedTest()
        {
            var listOfPoints = new KtPoint2D[] { (1.0, 2.0), (3.0, 6.0), (4.0, 0.0) };
            var solid = new KtSolidRegion(listOfPoints);
            var hollow = new KtHollowRegion(listOfPoints);

            var point1 = (2.5, 1.0);
            var point2 = (2.0, 1.35);
            Assert.IsFalse(solid.Inscribes(point1));
            Assert.IsTrue(solid.Inscribes(point2));
            Assert.IsFalse(hollow.Inscribes(point1));
            Assert.IsTrue(hollow.Inscribes(point2));
        }

        [TestMethod]
        public void SegmentInscribedTest()
        {
            var listOfPoints = new List<KtPoint2D> { (1.0, 2.0), (3, 4), (3.0, 6.0), (4.0, 0.0) };
            var region = new KtSolidRegion(listOfPoints);
            var segmentTouchingFalse = new KtSegment2D((2.5, 1), (3, 5));
            Assert.IsFalse(region.Inscribes(segmentTouchingFalse));
            var segmentTouchingPass = new KtSegment2D((2.5, 1), (3.5, 3));
            Assert.IsTrue(region.Inscribes(segmentTouchingPass));
            var segmentInside = new KtSegment2D((2, 2), (3, 3));
            Assert.IsTrue(region.Inscribes(segmentInside));
            var segmentTouch1Pass = new KtSegment2D((2.5, 1), (3, 3));
            Assert.IsTrue(region.Inscribes(segmentTouch1Pass));
            var segmentIsOnTheEdge = new KtSegment2D((1.6, 1.6), (2.2, 1.2));
            Assert.IsFalse(region.Inscribes(segmentIsOnTheEdge));
        }

        [TestMethod]
        public void PolygonUnionTest()
        {
            var polygon1 = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (266, 303), (46, 263), (5, 106), (27, 17), (219, 29), (341, 38), (369, 152), (364, 194) }),
                new KtHollowRegion(new KtPoint2D[] { (314, 179), (316, 88), (75, 53), (39, 132), (74, 237), (223, 278) }),
                new KtSolidRegion(new KtPoint2D[] { (128, 202), (67, 136), (78, 114), (95, 79), (277, 92), (287, 158), (261, 197), (221, 237) }),
                new KtHollowRegion(new KtPoint2D[] { (204, 207), (205, 206), (246, 149), (247, 148), (244, 112), (245, 111), (119, 104), (120, 103), (99, 127), (100, 126), (115, 165), (116, 164), (151, 182), (152, 181) })
            };
            var polygon2 = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (399, 356), (264, 328), (209, 164), (262, 7), (545, 30), (554, 281), (479, 348) }),
                new KtHollowRegion(new KtPoint2D[] { (504, 296), (519, 211), (499, 68), (305, 20), (230, 148), (314, 315), (353, 311), (430, 324) })
            };
            var expectedPolygon = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (545, 30), (554, 281), (479, 348), (399, 356), (264, 328), (254.941558441558, 300.989374262102), (46, 263), (5, 106), (27, 17), (219, 29), (253.708878814121, 31.5604910600581), (262, 7) }),
                new KtHollowRegion(new KtPoint2D[] { (221, 237), (128, 202), (67, 136), (78, 114), (95, 79), (234.334517992004, 88.9524655708574), (238.457648896503, 76.7386627028117), (75, 53), (39, 132), (74, 237), (223, 278), (240.754136187344, 258.685060631352), (230.34703196347, 227.65296803653) }),
                new KtHollowRegion(new KtPoint2D[] { (369, 152), (364, 194), (292.909489851892, 273.070057205548), (314, 315), (353, 311), (430, 324), (504, 296), (519, 211), (499, 68), (305, 20), (296.381744521515, 34.7084893499478), (341, 38) }),
                new KtHollowRegion(new KtPoint2D[] { (277, 92), (287, 158), (261, 197), (256.772908366534, 201.227091633466), (269.786841321822, 227.100029770765), (314, 179), (316, 88), (269.143488782003, 81.1951124787142), (263.382431708623, 91.027316550616) }),
                new KtHollowRegion(new KtPoint2D[] { (119, 104), (120, 103), (99, 127), (100, 126), (115, 165), (116, 164), (151, 182), (152, 181), (204, 207), (205, 206), (217.334516685262, 188.852013388782), (209, 164), (227.225078152136, 110.012504341785) }),
                new KtHollowRegion(new KtPoint2D[] { (246, 149), (247, 148), (244.88326848249, 122.599221789883), (230, 148), (236.88027503223, 161.678642028363) })
            };
            Assert.AreEqual(polygon1 + polygon2, expectedPolygon);
        }

        [TestMethod]
        public void PolygonUnionTest2()
        {
            var polygon1 = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (266, 303), (46, 263), (5, 106), (27, 17), (219, 29), (341, 38), (369, 152), (364, 194) }),
                new KtHollowRegion(new KtPoint2D[] { (314, 179), (316, 88), (75, 53), (39, 132), (74, 237), (223, 278) }),
                new KtSolidRegion(new KtPoint2D[] { (128, 202), (67, 136), (78, 114), (95, 79), (277, 92), (287, 158), (261, 197), (221, 237) }),
                new KtHollowRegion(new KtPoint2D[] { (204, 207), (205, 206), (246, 149), (247, 148), (244, 112), (245, 111), (119, 104), (120, 103), (99, 127), (100, 126), (115, 165), (116, 164), (151, 182), (152, 181) })
            };
            Assert.AreEqual(null & polygon1, polygon1);
            Assert.AreEqual(polygon1 & null, polygon1);
            Assert.AreEqual(null + polygon1, polygon1);
            Assert.AreEqual(polygon1 + null, polygon1);
        }

        [TestMethod]
        public void PolygonUnionTest3()
        {
            var polygon1 = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (45, 186), (109, 28), (194, 76), (165, 191), (328, 282), (384, 164), (448, 221), (353, 340) }),
                new KtHollowRegion(new KtPoint2D[] { (342, 324), (429, 231), (396, 193), (329, 297), (124, 181), (165, 82), (113, 48), (76, 175) })
            };

            var polygon2 = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (-6, 32), (103.070796460177, -9.64521319388576), (109, -18), (239, -6), (515, 140), (553, 165), (466, 300), (43, 75), (43.5238095238095, 74.2619047619048), (-3, 51) }),
                new KtHollowRegion(new KtPoint2D[] { (525, 177), (239, 9), (143, -1), (41, 35), (46, 61), (462, 279) })
            };

            var expectedPolygon = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (43, 75), (43.5238095238095, 74.2619047619048), (-3, 51), (-6, 32), (103.070796460177, -9.64521319388576), (109, -18), (239, -6), (515, 140), (553, 165), (466, 300), (409.096260040161, 269.732053212851), (353, 340), (45, 186), (81.6372701085752, 95.5517394194549) }),
                new KtHollowRegion(new KtPoint2D[] { (96.7970699158689, 103.615462721207), (76, 175), (342, 324), (398.19248395967, 263.932172318973), (362.525521865001, 244.960383970745), (329, 297), (124, 181), (145.352060584713, 129.4425854174) }),
                new KtHollowRegion(new KtPoint2D[] { (384, 164), (448, 221), (419.48403452303, 256.719998860626), (462, 279), (525, 177), (239, 9), (143, -1), (41, 35), (46, 61), (86.9421686746988, 82.4552710843374), (109, 28), (194, 76), (180.065950297256, 131.255714338466), (355.828243278517, 223.361915948838) }),
                new KtHollowRegion(new KtPoint2D[] { (409.724008741805, 251.605369965657), (429, 231), (396, 193), (371.237670641092, 231.437048557111) }),
                new KtHollowRegion(new KtPoint2D[] { (150.925421970392, 115.984956705638), (165, 82), (113, 48), (100.840027587113, 89.7382836874774) }),
                new KtHollowRegion(new KtPoint2D[] { (176.363458401305, 145.938009787928), (165, 191), (328, 282), (348.993953354449, 237.762741145983) })
            };
            Assert.AreEqual(polygon1 | polygon2, expectedPolygon);
        }

        [TestMethod]
        public void PolygonDifferenceTest1()
        {
            var polygon1 = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (300, 197), (200, 280), (91, 136), (266, 75), (332, 119) })
            };

            var polygon2 = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (321, 218), (282, 233), (267, 141), (395, 94), (456, 139) })
            };

            var expectedPolygon = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (267, 141), (278.975586404978, 214.450263283868), (200, 280), (91, 136), (266, 75), (330.19395465995, 117.7959697733) })
            };
            Assert.AreEqual(polygon1 - polygon2, expectedPolygon);
        }

        [TestMethod]
        public void PolygonDifferenceTest2()
        {
            var rectangular1 = new KtPolygon2D((KtSolidRegion)PolygonMethods.Rectangle((10, 20), 4, 6, true));
            var rectangular2 = new KtPolygon2D((KtSolidRegion)PolygonMethods.Rectangle((9, 18.5), 2, 3, true));
            var rectangular3 = new KtPolygon2D(new KtSolidRegion(new KtPoint2D[] { (8.0, 23.0), (12.0, 23.0), (12.0, 17.0), (10, 17), (10, 20), (8, 20) }));
            var actual = rectangular1 - rectangular2;
            Assert.AreEqual(actual, rectangular3);
            var rotatedRectangle1 = rectangular1.RotateAt(Degrees.Create(30), (10, 20));
            var rotatedRectangle2 = rectangular2.RotateAt(Degrees.Create(30), (10, 20));
            var rotatedRectangle3 = rectangular3.RotateAt(Degrees.Create(30), (10, 20));
            actual = rotatedRectangle1 - rotatedRectangle2;
            Assert.AreEqual(actual, rotatedRectangle3);
        }

        [TestMethod]
        public void PolygonDifferenceTest3()
        {
            var polygon1 = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (237, 276), (36, 80), (97, -6), (294, -2), (326, 70), (343, 148) }),
                new KtHollowRegion(new KtPoint2D[] { (309, 70), (284, 20), (113, 20), (64, 80), (241, 245), (318, 143) }),
                new KtSolidRegion(new KtPoint2D[] { (136, 36), (257, 33), (291, 76), (283, 144), (239, 210), (101, 83) }),
                new KtHollowRegion(new KtPoint2D[] { (271, 83), (249, 57), (138, 55), (124, 82), (229, 183), (261, 136) }),
                new KtSolidRegion(new KtPoint2D[] { (222, 152), (147, 85), (176, 60), (229, 78), (237, 109) })
            };

            var polygon2 = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (227, 132), (233, 19), (432, -8), (493, 90), (433, 167) }),
                new KtHollowRegion(new KtPoint2D[] { (465, 87), (421, 14), (252, 32), (245, 123), (423, 144) }),
                new KtSolidRegion(new KtPoint2D[] { (261, 108), (264, 49), (410, 24), (445, 83), (413, 127) }),
                new KtHollowRegion(new KtPoint2D[] { (424, 84), (399, 40), (282, 60), (283, 100), (405, 113) })
            };

            var expectedPolygon = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (241, 245), (315.015101025014, 146.954022018813), (340.307533745595, 151.25128000532), (237, 276), (36, 80), (97, -6), (294, -2), (299.333333333333, 10), (233, 19), (232.946902654867, 20), (113, 20), (64, 80) }),
                new KtSolidRegion(new KtPoint2D[] { (295.779179810726, 43.5583596214511), (288.078651685393, 28.1573033707865), (306.529817953547, 26.1920903954802), (312.942008486563, 40.6195190947666) }),
                new KtSolidRegion(new KtPoint2D[] { (336.334431630972, 117.416803953871), (339.993864123605, 134.20714127301), (316.575322139789, 131.44427957829), (314.509565217391, 114.688695652174) }),
                new KtSolidRegion(new KtPoint2D[] { (309, 70), (302.267716535433, 56.5354330708661), (318.762577228597, 53.7157987643425), (326, 70), (333.71629908553, 105.404195804196), (313.093981112755, 103.206735692343) }),
                new KtSolidRegion(new KtPoint2D[] { (229, 183), (259.915756341418, 137.592482873542), (283.286674132139, 141.56326987682), (283, 144), (239, 210), (101, 83), (136, 36), (232.224020505309, 33.6142804833394), (230.999522102748, 56.6756670649144), (138, 55), (124, 82) }),
                new KtSolidRegion(new KtPoint2D[] { (264, 49), (262.765988372093, 73.2688953488372), (249.987179487179, 58.1666666666667), (251.913375796178, 33.1261146496815), (257, 33), (268.977272727273, 48.1477272727273) }),
                new KtSolidRegion(new KtPoint2D[] { (286.855072463768, 111.231884057971), (284.916558018253, 127.70925684485), (263.051016175861, 125.129614267939), (266.161290322581, 108.645161290323) }),
                new KtSolidRegion(new KtPoint2D[] { (283, 100), (282.1192103265, 64.7684130599848), (291, 76), (288.112380952381, 100.544761904762) }),
                new KtSolidRegion(new KtPoint2D[] { (227, 132), (228.866140893105, 132.3170627731), (222, 152), (147, 85), (176, 60), (229, 78), (229.719266055046, 80.7871559633028) })
            };
            Assert.AreEqual(polygon1 - polygon2, expectedPolygon);
        }

        [TestMethod]
        public void PolygonIntersectionTest()
        {
            var polygon1 = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (90, 358), (34, 194), (157, 22), (262, 78), (314, 136), (307, 223) }),
                new KtHollowRegion(new KtPoint2D[] { (297, 148), (168, 55), (55, 200), (100, 328), (288, 214) }),
                new KtSolidRegion(new KtPoint2D[] { (166, 81), (283, 153), (275, 208), (105, 311), (74, 204) }),
                new KtHollowRegion(new KtPoint2D[] { (271, 157), (167, 99), (90, 211), (115, 290), (257, 205) }),
                new KtSolidRegion(new KtPoint2D[] { (248, 164), (239, 201), (123, 270), (103, 218), (165, 123) }),
                new KtHollowRegion(new KtPoint2D[] { (238, 167), (169, 139), (116, 224), (131, 249), (227, 197) })
            };

            var polygon2 = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (176, 190), (243, 29), (412, 29), (525, 135), (502, 197), (379, 210) }),
                new KtHollowRegion(new KtPoint2D[] { (499, 177), (503, 139), (400, 49), (262, 43), (209, 172), (374, 191) }),
                new KtSolidRegion(new KtPoint2D[] { (276, 63), (390, 79), (468, 165), (234, 159) }),
                new KtHollowRegion(new KtPoint2D[] { (383, 95), (295, 77), (264, 136), (270, 145), (405, 152) }),
                new KtSolidRegion(new KtPoint2D[] { (279, 137), (295, 87), (375, 105), (391, 144) }),
                new KtHollowRegion(new KtPoint2D[] { (367, 109), (306, 98), (297, 131), (371, 134) })
            };

            var expectedPolygon = new KtPolygon2D
            {
                new KtSolidRegion(new KtPoint2D[] { (236.716272600834, 104.539638386648), (217.368777777778, 90.5914444444444), (229.763470010166, 60.8071840054219), (250.204747774481, 71.7091988130564) }),
                new KtSolidRegion(new KtPoint2D[] { (292.417412530513, 181.605641442908), (310.166137493098, 183.649434014357), (308.604011010617, 203.064434582327), (289.744586831639, 201.206363234644) }),
                new KtSolidRegion(new KtPoint2D[] { (268.493138936535, 127.448542024014), (252.764640883978, 116.109392265193), (266.998384491115, 83.5751211631664), (282.475133579942, 100.837648993013) }),
                new KtSolidRegion(new KtPoint2D[] { (297, 148), (294.608650875386, 146.276004119464), (313.096065796168, 147.234610819061), (311.988529411765, 160.999705882353), (295.285714285714, 160.571428571429) }),
                new KtSolidRegion(new KtPoint2D[] { (297, 131), (309.989361702128, 131.526595744681), (314, 136), (313.744817726948, 139.171551107934), (282.002207505519, 137.187637969095), (279.513983371126, 135.393801965231), (288.442176870748, 107.493197278912), (299.919571045576, 120.29490616622) }),
                new KtSolidRegion(new KtPoint2D[] { (225.571740448757, 131.665009096422), (205.040911294232, 120.215123606398), (210.073411943705, 108.122099657665), (230.164683484055, 120.485959067111) }),
                new KtSolidRegion(new KtPoint2D[] { (264.752504275592, 178.419985340826), (279.062859000759, 180.067844369784), (276.182516556291, 199.870198675497), (258.990223463687, 198.17637669593) }),
                new KtSolidRegion(new KtPoint2D[] { (283, 153), (281.948444031584, 160.229447282861), (270.146341463415, 159.926829268293), (271, 157), (241.96038647343, 140.804830917874), (246.462121212121, 130.515151515152), (270, 145) }),
                new KtSolidRegion(new KtPoint2D[] { (214.90476649013, 157.628021184401), (193.146014632268, 148.798382749326), (197.252017380509, 138.931719428926), (218.312034161491, 149.334860248447) }),
                new KtSolidRegion(new KtPoint2D[] { (235.066098081023, 175.001550688118), (245.044455066922, 176.150573613767), (240.138603562606, 196.319074242621), (227.699049128368, 195.093502377179) }),
                new KtSolidRegion(new KtPoint2D[] { (234.689164086687, 157.424767801858), (238.09036939314, 159.104881266491), (234, 159) })
            };
            Assert.AreEqual(polygon1 & polygon2, expectedPolygon);
        }
    }

    [TestClass]
    public class SegmentsTest
    {
        [TestMethod]
        public void CutByTest()
        {
            _ = new KtSegments2D((8, 17), (12, 17), (12, 23), (8, 23));
        }

        [TestMethod]
        public void RegionInscribesSegments()
        {
            var segments = new KtSegments2D((10, 17), (10, 20), (8, 20));
            var rectangular1 = (KtSolidRegion)PolygonMethods.Rectangle((10, 20), 4, 6, true);
            Assert.IsTrue(rectangular1.Inscribes(segments));
        }
    }
}