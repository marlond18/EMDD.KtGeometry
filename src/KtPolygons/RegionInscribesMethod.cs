using CircularStack;

using EMDD.KtGeometry.Angles;
using EMDD.KtGeometry.KtLines._2D;
using EMDD.KtGeometry.KtPoints;

using KtExtensions;

using System.Collections.Generic;
using System.Linq;

using EnumerableRegions = System.Collections.Generic.IEnumerable<EMDD.KtGeometry.KtPolygons.Regions.KtRegion>;

namespace EMDD.KtGeometry.KtPolygons
{
    public static class RegionInscribesMethod
    {
        internal static bool Inscribes(this IEnumerable<KtPoint2D> points, KtPoint2D point) =>
            points.ToCircularStack().Inscribes(point);

        internal static bool Inscribes(this IEnumerable<KtPoint2D> points, KtSegment2D segment) =>
            points.ToCircularStack().Inscribes(segment);

        internal static bool Inscribes(this IEnumerable<CircularStack<KtPoint2D>> stacks, KtSegments2D segments) =>
            stacks.Any(stack => stack.Inscribes(segments));

        internal static bool Inscribes(this CircularStack<KtPoint2D> points, KtPoint2D point) =>
            !points.EdgesHasThePoint(point) && !points.InteriorAngle(point).IsZero;

        private static Angle InteriorAngle(this CircularStack<KtPoint2D> points, KtPoint2D point) =>
            points.Aggregate(Degrees.Create(0.0), (total, c) => total + c.InteriorAngle(point));

        private static bool EdgesHasThePoint(this CircularStack<KtPoint2D> points, KtPoint2D point) =>
            points.ToEdges().Any(edge => edge.HasThePoint(point));

        private static Angle InteriorAngle(this CircularStackElement<KtPoint2D> c, KtPoint2D point) =>
            (c?.Next?.Value - point).ToKtVector2D().AngleBetween((c?.Value - point).ToKtVector2D());

        internal static bool Inscribes(this EnumerableRegions clips, KtSegment2D edge) =>
            clips.Select(clip => clip.Corners).Inscribes(edge);

        internal static bool Inscribes(this IEnumerable<KtPoint2D> points, IEnumerable<KtPoint2D> points2) =>
            points2.All(points.Inscribes);

        internal static bool Inscribes(this CircularStack<KtPoint2D> points, CircularStack<KtPoint2D> points2) =>
            points2.All(point => points.Inscribes(point.Value));

        internal static bool Inscribes(this CircularStack<KtPoint2D> points, KtSegment2D segment)
        {
            var point1In = points.Inscribes(segment.Start);
            var point2In = points.Inscribes(segment.End);
            var edges = points.ToEdges();
            var intersectionCounts = edges.Count(segment.HasIntersection);
            if (intersectionCounts > 2) return false;
            if (point1In && point2In) return intersectionCounts < 1;
            if (!point1In && !point2In) return segment.EndLieOnSegments(edges) && points.Inscribes(segment.MidPoint());
            return (!point1In && segment.Start.LiesOnSegments(edges)) || (!point2In && segment.End.LiesOnSegments(edges));
        }

        internal static bool Inscribes(this CircularStack<KtPoint2D> points, KtSegments2D segments) =>
            segments.Any(points.Inscribes);

        internal static bool Inscribes(this IEnumerable<IEnumerable<KtPoint2D>> points, KtSegment2D segment) =>
            points.Any(clipCorner => clipCorner.Inscribes(segment));
    }
}