using EMDD.KtGeometry.KtLines._2D;

using KtExtensions;

using System;
using System.Linq;

using static EMDD.KtGeometry.KtPolygons.RegionClipMethods;

using EnumerableHollows = System.Collections.Generic.IEnumerable<EMDD.KtGeometry.KtPolygons.Regions.KtHollowRegion>;
using EnumerablePoints = System.Collections.Generic.IEnumerable<EMDD.KtGeometry.KtPoints.KtPoint2D>;
using EnumerableSolids = System.Collections.Generic.IEnumerable<EMDD.KtGeometry.KtPolygons.Regions.KtSolidRegion>;
using ListOfHollows = System.Collections.Generic.List<EMDD.KtGeometry.KtPolygons.Regions.KtHollowRegion>;
using Point = EMDD.KtGeometry.KtPoints.KtPoint2D;

namespace EMDD.KtGeometry.KtPolygons.Regions
{
    public class KtSolidRegion : KtRegion
    {
        internal ListOfHollows holes;

        public KtSolidRegion(EnumerablePoints corners) : base(corners)
        {
            FixCorners();
            holes = new ListOfHollows();
        }

        public EnumerableHollows Holes
        {
            get
            {
                foreach (var reg in holes) yield return reg;
            }
        }

        public EnumerableSolids Add(KtSolidRegion solid)
        {
            var (mainSolids, mainHollows) = Clip(solid, ClipType.ctUnion);
            var (additionalSolids, hollows) = FixHoles(solid);
            foreach (var reg in mainSolids.Concat(additionalSolids))
            {
                reg.holes = hollows.Concat(mainHollows).Where(reg.Inscribes).ToList();
                yield return reg;
            }
        }

        public bool IsTriangle => Corners.Count == 3 && !Holes.Any();

        public override double Area() => InitialArea() + Holes.Aggregate(0.0, (total, hole) => total + hole.InitialArea());

        public override Point AreaCentroid() => InitialAreaCentroid() + Holes.Sum(hole => hole.InitialAreaCentroid());

        public override object Clone() => new KtSolidRegion(Corners.Clone()) { holes = holes.Clone() };

        public override bool Inscribes(Point p) => LoopedCorners().Inscribes(p) && (holes.Count == 0 || holes.All(reg => !reg.Inscribes(p)));

        public override bool Inscribes(KtRegion region) => region?.All(Inscribes) == true && holes.All(reg => !reg.Inscribes(region));

        public override bool Inscribes(KtSegment2D segment) => LoopedCorners().Inscribes(segment) && (holes.Count == 0 || !holes.All(hole => hole.Inscribes(segment)));

        public bool Inscribes(KtSegments2D segments) => LoopedCorners().Inscribes(segments);

        public EnumerableSolids Intersect(KtRegion region)
        {
            if (region is KtHollowRegion hollow) return Intersect(hollow);
            if (region is KtSolidRegion solid) return Intersect(solid);
            return null;
        }

        public EnumerableSolids Xor(KtSolidRegion _)
        {
            //var (solid, hollow) = Clip(region, ClipType.ctXor);
            //var hol = holes.Clip(region, ClipType.ctUnion, true);
#pragma warning disable RCS1079 // Throwing of new NotImplementedException.
            throw new NotImplementedException("XOR method has not been fully implemented yet");
#pragma warning restore RCS1079 // Throwing of new NotImplementedException.
        }

        public EnumerableSolids Intersect(KtHollowRegion hollow) => Intersect(holes, hollow).ToList();

        public KtSolidRegion Translate(Point point) => new(TranslateCorners(point)) { holes = holes.ConvertAll(_h => _h.Translate(point)) };

        public EnumerableSolids Intersect(KtSolidRegion solid) => Intersect(holes.Clip(solid.holes, ClipType.ctUnion, true).hollow, solid).ToList();

        public EnumerableSolids Remove(KtRegion region)
        {
            if (region is KtSolidRegion solid) return Remove(solid);
            if (region is KtHollowRegion hollow) return Remove(hollow);
            return null;
        }

        public EnumerableSolids Remove(KtHollowRegion hole)
        {
            var (testingSolid, testingHole) = holes.Clip(hole, ClipType.ctUnion, true);
            var (solids, hollows) = Clip(testingHole, ClipType.ctDifference);
            var allSolids = solids.Concat(testingSolid);
            foreach (var reg in allSolids) reg.holes = hollows.ToList();
            return allSolids;
        }

        public EnumerableSolids Remove(KtSolidRegion solid)
        {
            var S1MinusS2 = Clip(solid, ClipType.ctDifference);
            var H1MinusS2 = holes.Clip(solid, ClipType.ctDifference, true);
            var S1IntH2 = Clip(solid.holes, ClipType.ctIntersection);
            var H1IntH2 = holes.Clip(solid.holes, ClipType.ctIntersection, true);
            var solidOut = S1MinusS2.solid.Clip(H1MinusS2.hollow, ClipType.ctDifference);
            var solidIn = S1IntH2.solid.Clip(H1IntH2.hollow, ClipType.ctDifference);
            var solids = solidOut.solid.Concat(H1MinusS2.solid, H1IntH2.solid, solidIn.solid);
            var hollows = solidOut.hollow.Concat(S1MinusS2.hollow, S1IntH2.hollow, solidIn.hollow).ToList();
            foreach (var reg in solids) reg.holes = GetValidHoles(reg, hollows).ToList();
            return solids;
        }

        //todo: assign this to all holes
        private static EnumerableHollows GetValidHoles(KtSolidRegion solid, EnumerableHollows hollows)
        {
            var corners = solid.LoopedCorners();
            foreach (var hollow in hollows)
            {
                if (hollow.Edges().All(corners.Inscribes)) yield return hollow;
            }
        }

        public override KtRegion Reverse() => new KtHollowRegion(Corners);

        internal override bool EqualityMethod(KtRegion other)
        {
            if (other is KtHollowRegion) return false;
            if (other is KtSolidRegion solid) return LoopedCorners().Equals(other.LoopedCorners()) && holes.All(solid.holes.Contains) && solid.holes.All(holes.Contains);
            return false;
        }

        internal override int HashCodeMethod()
        {
            unchecked
            {
                return Corners?.GetHashCodeOfEnumerable() * holes.Aggregate(0, (total, val) => total ^ val.GetHashCode()) ?? 0;
            }
        }

        internal override bool IsIntersectedWithAnyEdge(KtRegion region)
        {
            if (region is KtSolidRegion solid && solid.holes.Any(IsIntersectingEdges)) return true;
            return IsIntersectingEdges(region) || holes.Any(region.IsIntersectingEdges);
        }

        private (EnumerableSolids solid, EnumerableHollows hollow) FixHoles(KtSolidRegion solid)
        {
            var (solidA, hollowA) = holes.Clip(solid, ClipType.ctDifference, true);
            var (solidB, hollowB) = solid.holes.Clip(this, ClipType.ctDifference, true);
            var (solidC, hollowC) = holes.Clip(solid.holes, ClipType.ctIntersection, true);
            return (solidA.Concat(solidB, solidC), hollowA.Concat(hollowB, hollowC));
        }

        private EnumerableSolids Intersect(EnumerableHollows cuts, params KtRegion[] region)
        {
            var remaining = Clip(region, ClipType.ctIntersection).solid;
            var (solids, hollows) = remaining.Clip(cuts, ClipType.ctDifference);
            foreach (var reg in solids)
            {
                reg.holes = hollows.ToList();
                yield return reg;
            }
        }
    }
}