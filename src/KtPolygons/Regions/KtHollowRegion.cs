using EMDD.KtGeometry.KtLines._2D;

using KtExtensions;

using System.Linq;

using static EMDD.KtGeometry.KtPolygons.RegionClipMethods;

using EnumerableHollows = System.Collections.Generic.IEnumerable<EMDD.KtGeometry.KtPolygons.Regions.KtHollowRegion>;
using EnumerablePoints = System.Collections.Generic.IEnumerable<EMDD.KtGeometry.KtPoints.KtPoint2D>;
using Point = EMDD.KtGeometry.KtPoints.KtPoint2D;

namespace EMDD.KtGeometry.KtPolygons.Regions
{
    public class KtHollowRegion : KtRegion
    {
        public KtHollowRegion(EnumerablePoints corners) : base(corners)
        {
            FixCorners(false);
        }

        public KtHollowRegion Translate(Point point2D) => new(TranslateCorners(point2D));

        public override double Area() => InitialArea();

        public override Point AreaCentroid() => InitialAreaCentroid();

        public override object Clone() => new KtHollowRegion(Corners.Select(c => c.Clone()));

        public EnumerableHollows Cut(KtSolidRegion region) => Clip(region, ClipType.ctDifference, true).hollow;

        public override bool Inscribes(Point p) => LoopedCorners().Inscribes(p);

        public override bool Inscribes(KtRegion region) => region?.All(Inscribes) == true;

        public override bool Inscribes(KtSegment2D segment) => LoopedCorners().Inscribes(segment);

        public EnumerableHollows Intersect2(KtHollowRegion region) => Clip(region, ClipType.ctIntersection, true).hollow;

        public KtHollowRegion Merge(KtHollowRegion region) => Clip(region, ClipType.ctUnion, true).hollow.FirstOrDefault();

        public override KtRegion Reverse() => new KtSolidRegion(Corners);

        internal override bool EqualityMethod(KtRegion other) => LoopedCorners().Equals(other.LoopedCorners());

        internal override int HashCodeMethod() => Corners?.GetHashCodeOfEnumerable() ?? 0;

        internal override bool IsIntersectedWithAnyEdge(KtRegion region) => region is KtSolidRegion solid ? solid.IsIntersectedWithAnyEdge(this) : region.IsIntersectingEdges(this);
    }
}