using EMDD.KtGeometry.Angles;
using EMDD.KtGeometry.KtPoints;
using EMDD.KtGeometry.KtPolygons.Regions;
using EMDD.KtGeometry.KtVectors;

using KtExtensions;

using System.Collections.Generic;

namespace EMDD.KtGeometry.KtPolygons
{
    public static class CreateSpecialRegionMethods
    {
        public static KtRegion Rectangle(KtPoint2D center, double width, double height, bool solid = true)
        {
            if (center == null) center = new KtPoint2D();
            var corners = new List<KtPoint2D>
            {
                center + new KtPoint2D(-width/2.0, -height/2.0),
                center + new KtPoint2D(-width/2.0, height/2.0),
                center + new KtPoint2D(width/2.0, height/2.0),
                center + new KtPoint2D(width/2.0, -height/2.0)
            };
            return solid ? (KtRegion)new KtSolidRegion(corners) : new KtHollowRegion(corners);
        }

        public static KtRegion RegularPolygon(KtPoint2D center, int cornerCount, double circumRadius, bool solid)
        {
            if (circumRadius.NearZero(5) || cornerCount < 2) return null;
            if (center == null) center = new KtPoint2D();
            var corners = new List<KtPoint2D>();
            var intAngle = Cycle.Create(1) / cornerCount;
            for (var i = 0; i < cornerCount; i++)
            {
                var corner = center + (new KtVector2D(intAngle * i) * circumRadius).ToKtPoint2D();
                corners.Add(corner);
            }
            return solid ? (KtRegion)new KtSolidRegion(corners) : new KtHollowRegion(corners);
        }
    }
}