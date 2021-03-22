using CircularStack;

using EMDD.KtGeometry.KtLines._2D;
using EMDD.KtGeometry.KtPoints;
using EMDD.KtGeometry.KtPolygons.Regions;

using KtExtensions;

using System;
using System.Collections.Generic;
using System.Linq;

using static EMDD.KtGeometry.KtPolygons.RegionClipMethods;
using static EMDD.KtGeometry.KtPolygons.RegionClipMethods.ClipType;
using static EMDD.KtGeometry.KtPolygons.RegionClipMethods.TypeOfSegment;

using EnumerableHollows = System.Collections.Generic.IEnumerable<EMDD.KtGeometry.KtPolygons.Regions.KtHollowRegion>;
using EnumerableRegions = System.Collections.Generic.IEnumerable<EMDD.KtGeometry.KtPolygons.Regions.KtRegion>;
using EnumerableSolids = System.Collections.Generic.IEnumerable<EMDD.KtGeometry.KtPolygons.Regions.KtSolidRegion>;
using ListOfEnumerablePoints = System.Collections.Generic.List<System.Collections.Generic.IEnumerable<EMDD.KtGeometry.KtPoints.KtPoint2D>>;

namespace EMDD.KtGeometry.KtPolygons
{
    public static class RegionClipMethods
    {
        public enum ClipType
        {
            ctUnion,
            ctDifference,
            ctIntersection,
            ctXor
        }

        internal enum TypeOfSegment
        {
            MainOut,
            MainIn,
            ClipOut,
            ClipIn
        }

        internal static (EnumerableSolids solid, EnumerableHollows hollow) Clip(this EnumerableRegions subjects, KtRegion clip, ClipType clipType, bool reversed = false) => Clip(subjects, new[] { clip }, clipType, reversed);

        internal static (EnumerableSolids solid, EnumerableHollows hollow) Clip(this EnumerableRegions subjects, EnumerableRegions clips, ClipType clipType, bool reversed = false) => CutByToEdges(subjects, clips).MakeRegions(clipType, reversed);

        private static Partition<TypeOfSegment, KtSegments2D> CutByToEdges(EnumerableRegions subjects, EnumerableRegions clips)
        {
            var (maincut, clipcut) = CutAndPatition(subjects.ToListOfCircularStack(),
                clips.ToListOfCircularStack());
            return new Partition<TypeOfSegment, KtSegments2D>
            {
                [MainOut] = maincut[false],
                [MainIn] = maincut[true],
                [ClipOut] = clipcut[false],
                [ClipIn] = clipcut[true]
            };
        }

        private static (Partition<bool, KtSegments2D>, Partition<bool, KtSegments2D>) CutAndPatition(List<CircularStack<KtPoint2D>> edges1, List<CircularStack<KtPoint2D>> edges2) =>
            (edges1.CutAndPartitionIncribes(edges2), edges2.CutAndPartitionIncribes(edges1));

        private static Partition<bool, KtSegments2D> CutAndPartitionIncribes(this List<CircularStack<KtPoint2D>> edges1, List<CircularStack<KtPoint2D>> edges2) => edges1.CutBy(edges2).Partition(edges2.Inscribes);

        private static List<CircularStack<KtPoint2D>> ToListOfCircularStack(this EnumerableRegions regionsCollection) => regionsCollection.Select(region => region.ToCircularStack()).ToList();

        private static bool Inscribes(this IEnumerable<IEnumerable<KtPoint2D>> groupOfEdges, IEnumerable<KtPoint2D> edges) => groupOfEdges.AnyExcept(edges, currentEdges => currentEdges.Inscribes(edges));

        private static (EnumerableSolids solid, EnumerableHollows hollow) MakeRegions(this Partition<TypeOfSegment, KtSegments2D> parts, ClipType clipType, bool reversed)
        {
            return new Dictionary<ClipType, Func<ListOfEnumerablePoints>>
            {
                [ctUnion] = parts.MergeOuterEdges,
                [ctDifference] = parts.MergeOuterMainToInnerClip,
                [ctIntersection] = parts.MergeInnerEdges,
                [ctXor] = () => parts.MergeOuterMainToInnerClip().Concat(parts.MergeOuterClipToInnerMain()).ToList()
            }[clipType].Invoke().FixRegions(reversed);
        }

        private static (EnumerableSolids, EnumerableHollows) FixRegions(this IEnumerable<IEnumerable<KtPoint2D>> groupOfEdges, bool reversed = false) => FixRegionsInner(groupOfEdges.Count() == 1 ? groupOfEdges.FixRegionOfOne() : groupOfEdges.Partition(groupOfEdges.Inscribes), reversed);

        private static (List<KtSolidRegion>, List<KtHollowRegion>) FixRegionsInner(Partition<bool, IEnumerable<KtPoint2D>> group, bool reversed) => (group[reversed].ConvertAll(edges => new KtSolidRegion(edges)), group[!reversed].ConvertAll(edges => new KtHollowRegion(edges)));

        private static Partition<bool, IEnumerable<KtPoint2D>> FixRegionOfOne(this IEnumerable<IEnumerable<KtPoint2D>> groupOfEdges) => new() { { true, new ListOfEnumerablePoints() }, { false, new ListOfEnumerablePoints { groupOfEdges.FirstOrDefault() } } };
    }

    internal static class MergeMethods
    {
        public static ListOfEnumerablePoints MergeOuterMainToInnerClip(this Partition<TypeOfSegment, KtSegments2D> parts) =>
    parts[MainOut].WhereNot(parts[ClipOut].Has).Concat(parts[ClipIn]).ConnectibleSegmentsToCorners();

        public static ListOfEnumerablePoints MergeOuterClipToInnerMain(this Partition<TypeOfSegment, KtSegments2D> parts) =>
            parts[ClipOut].WhereNot(parts[MainOut].Has).Concat(parts[MainIn]).ConnectibleSegmentsToCorners();

        public static ListOfEnumerablePoints MergeOuterEdges(this Partition<TypeOfSegment, KtSegments2D> parts) => parts[MainOut].Concat(parts[ClipOut]).ConnectibleSegmentsToCorners();

        public static ListOfEnumerablePoints MergeInnerEdges(this Partition<TypeOfSegment, KtSegments2D> parts) =>
            parts[MainIn].Concat(parts[ClipIn]).Concat(parts[MainOut].Where(parts[ClipOut].Has)).ConnectibleSegmentsToCorners();

        private static ListOfEnumerablePoints ConnectibleSegmentsToCorners(this IEnumerable<KtSegments2D> listOfSegments)
        {
            var listOfPoints = new ListOfEnumerablePoints();
            var remaining = listOfSegments;
            while (!remaining.IsEmpty())
            {
                var start = remaining.FirstOrDefault();
                if (start.IsRegion())
                {
                    listOfPoints.Add(start.Joints.Skip(1));
                    remaining = remaining.Skip(1);
                }
                else
                {
                    (start, remaining) = start.ConnectToListOfSegments(remaining.Skip(1));
                    if (start.IsRegion()) listOfPoints.Add(start.Joints.Skip(1));
                }
            }
            return listOfPoints;
        }

        private static (KtSegments2D result, IEnumerable<KtSegments2D> remaining) ConnectToListOfSegments(this KtSegments2D segments, IEnumerable<KtSegments2D> listOfSegments) => (listOfSegments?.Any() != true || segments.IsRegion()) ? (segments, listOfSegments) : ConnectToLeftAndRight(segments, listOfSegments);

        private static (KtSegments2D result, IEnumerable<KtSegments2D> remaining) ConnectToLeftAndRight(KtSegments2D segments, IEnumerable<KtSegments2D> remainingSegments) => (segments, remainingSegments).GetAllPossibleConnections(true).GetAllPossibleConnections(false);

        private static (KtSegments2D, IEnumerable<KtSegments2D>) GetAllPossibleConnections(this (KtSegments2D segments, IEnumerable<KtSegments2D> remainingSegments) toBeConnected, bool atStart)
        {
            var (connection, remainingSegments2) = GetConnections(toBeConnected.segments, toBeConnected.remainingSegments, atStart);
            var newSegments = toBeConnected.segments;
            while (connection.Any())
            {
                newSegments = newSegments.Connect(connection.FirstOrDefault(), atStart);
                (connection, remainingSegments2) = GetConnections(newSegments, remainingSegments2, atStart);
            }
            return (newSegments, remainingSegments2);
        }

        private static (IEnumerable<KtSegments2D> matches, IEnumerable<KtSegments2D> nonMatches) GetConnections(KtSegments2D segments, IEnumerable<KtSegments2D> remainingSegments, bool atStart) => remainingSegments.Fork(otherSegments => segments.Connectible(otherSegments, atStart));
    }
}