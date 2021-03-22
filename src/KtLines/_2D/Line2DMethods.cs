using EMDD.KtGeometry.KtPoints;

using KtExtensions;

using System.Collections.Generic;
using System.Linq;

using IEnumerablePoints = System.Collections.Generic.IEnumerable<EMDD.KtGeometry.KtPoints.KtPoint2D>;
using IEnumerableSegments = System.Collections.Generic.IEnumerable<EMDD.KtGeometry.KtLines._2D.KtSegment2D>;

namespace EMDD.KtGeometry.KtLines._2D
{
    public static class Line2DMethods
    {
        internal static IEnumerableSegments CutBy(this IEnumerableSegments segments1, IEnumerableSegments segments2)
        {
            return segments2.Where(IntersectsWithSegment1).Aggregate(segments1, AggregateInternalMethod);
            bool IntersectsWithSegment1(KtSegment2D seg) => segments1.Any(seg.HasIntersection);
        }

        private static IEnumerableSegments AggregateInternalMethod(IEnumerableSegments results, KtSegment2D segment) =>
            results.Select(result => result.CutBy(segment)).SelectMany(cuts => cuts);

        internal static IEnumerable<IEnumerablePoints> ToRegionEdges(this IEnumerableSegments _segments)
        {
            if (!_segments.Any()) yield break;
            var remaining = _segments.Skip(1).GetConnected(_segments.FirstOrDefault()?.PointsToList());
            if (remaining.regionEdges != null) yield return remaining.regionEdges;
            foreach (var regionEdges in remaining.segmentsRemaining.ToRegionEdges()) yield return regionEdges;
        }

        internal static List<KtPoint2D> PointsToList(this KtSegment2D segment) => new() { segment.Start, segment.End };

        internal static (IEnumerablePoints regionEdges, IEnumerableSegments segmentsRemaining) GetConnected(this IEnumerableSegments segments, List<KtPoint2D> joints)
        {
            if (!segments.Any()) return (null, segments);
            var (connectedSegments, remainingSegments) = segments.Fork(segment => segment.Start == joints.Last() || segment.End == joints.Last());
            if (connectedSegments.Any())
            {
                var toAdd = GetJointToAdd(connectedSegments, joints.Last());
                if (joints[0] == toAdd) return (joints, remainingSegments);
                joints.Add(toAdd);
            }
            (connectedSegments, remainingSegments) = remainingSegments.Fork(segment => segment.Start == joints[0] || segment.End == joints[0]);
            if (connectedSegments.Any())
            {
                var toAdd = GetJointToAdd(connectedSegments, joints[0]);
                if (joints.Last() == toAdd) return (joints, remainingSegments);
                joints.Insert(0, toAdd);
                return remainingSegments.GetConnected(joints);
            }
            return (null, remainingSegments);
        }

        private static KtPoint2D GetJointToAdd(IEnumerableSegments connectedSegments, KtPoint2D commonJoint) => MethodShortcuts.SeedProcess(
            connectedSegments.FirstOrDefault,
            connectedSegment => connectedSegment.Start == commonJoint ? connectedSegment.End : connectedSegment.Start);
    }
}