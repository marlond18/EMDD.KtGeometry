using CircularStack;

using EMDD.KtGeometry.Angles;
using EMDD.KtGeometry.KtLines._2D;
using EMDD.KtGeometry.KtPoints;
using EMDD.KtGeometry.KtPolygons.Regions;

using KtExtensions;

using System;
using System.Collections.Generic;
using System.Linq;

using static KtExtensions.EnumerableExtensions;

using EnumerableRegions = System.Collections.Generic.IEnumerable<EMDD.KtGeometry.KtPolygons.Regions.KtRegion>;

namespace EMDD.KtGeometry.KtPolygons
{
    public static class RegionMethods
    {
        internal static KtSegments2D ToSegments(this CircularStack<KtPoint2D> stackOfPoints) => MethodShortcuts.SeedProcess(
            () => stackOfPoints.Select(s => s.Value).ToList(),
            d => new KtSegments2D((d).Concat(new[] { d.FirstOrDefault() })));

        internal static KtRegion ToRegion(this IEnumerable<KtPoint2D> points) =>
            (points.CrossProductToNext() < 0) ? (KtRegion)new KtHollowRegion(points) : new KtSolidRegion(points);

        internal static double CrossProductToNext(this IEnumerable<KtPoint2D> points) =>
            (new CircularStack<KtPoint2D>(points)).Aggregate(0.0, (total, c) => total + c.CrossProductToNext());

        internal static double CrossProductToNext(this CircularStackElement<KtPoint2D> corner) =>
            corner?.Value?.ToKtVector2D().Cross(corner.Next.Value) ?? 0;

        internal static Ternary GetEdgeAssociation(this KtSegment2D segment, params IEnumerable<KtPoint2D>[] groupOfPoints) =>
            groupOfPoints.SelectMany(ToEdges).Any(segment.HasIntersection) ? Ternary.Neutral : groupOfPoints.Any(points => points.Inscribes(segment)) ? Ternary.True : Ternary.False;

        public static Angle TotalInternalAngle(this CircularStack<KtPoint2D> points, KtPoint2D point) =>
            points.Aggregate(Degrees.Create(0), (total, c) => total + (c?.Next?.Value - point).ToKtVector2D().AngleBetween((c?.Value - point).ToKtVector2D()));

        public static Angle TotalInternalAngle(this IEnumerable<KtPoint2D> points, KtPoint2D point) =>
            points.ToCircularStack().TotalInternalAngle(point);

        internal static bool IsEmpty(this EnumerableRegions regions) => regions?.Any() != true;

        internal static bool HasIntersection(this EnumerableRegions regions1, EnumerableRegions regions2) =>
            regions1.Any(region1 => regions2.Any(region1.IsIntersecting));

        internal static bool LiesOnSegments(this KtPoint2D point, IEnumerable<KtSegment2D> segments) =>
            segments.Any(segment => segment.HasThePoint(point));

        internal static bool EndLieOnSegments(this KtSegment2D segment, IEnumerable<KtSegment2D> segments) =>
            segment.Start.LiesOnSegments(segments) && segment.End.LiesOnSegments(segments);

        internal static IEnumerable<KtPoint2D> PreventEqualFirstAndLastCorner(this IEnumerable<KtPoint2D> corners)
        {
            var tempCorners = corners.ToList();
            var lastCornerIndex = tempCorners.Count - 1;
            while (lastCornerIndex > 0 && tempCorners[0] == tempCorners[lastCornerIndex])
            {
                tempCorners.RemoveAt(lastCornerIndex);
                lastCornerIndex = tempCorners.Count - 1;
            }
            return tempCorners;
        }

        internal static KtPoint2D Sum(this IEnumerable<KtPoint2D> ienumerable, Func<KtPoint2D, KtPoint2D> func) =>
            ienumerable.Aggregate(new KtPoint2D(), (total, point) => total + func?.Invoke(point));

        internal static KtPoint2D Sum(this EnumerableRegions ienumerable, Func<KtRegion, KtPoint2D> func) =>
            ienumerable.Aggregate(new KtPoint2D(), (total, point) => total + func(point));

        internal static CircularStack<KtPoint2D> ToCircularStack(this IEnumerable<KtPoint2D> points) =>
            new(points);

        internal static IEnumerable<KtSegment2D> ToEdges(this CircularStack<KtPoint2D> points) =>
            points.Select(ToEdge);

        internal static KtSegment2D ToEdge(this CircularStackElement<KtPoint2D> point) =>
            new(point.Value, point.Next.Value);

        internal static IEnumerable<KtSegment2D> ToEdges(this IEnumerable<KtPoint2D> points) =>
            points.ToCircularStack().ToEdges();
    }
}