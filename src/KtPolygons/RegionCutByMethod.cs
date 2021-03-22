using CircularStack;

using EMDD.KtGeometry.KtLines._2D;
using EMDD.KtGeometry.KtPoints;

using KtExtensions;

using System.Collections.Generic;
using System.Linq;

namespace EMDD.KtGeometry.KtPolygons
{
    public static class RegionCutByMethod
    {
        public static IEnumerable<KtSegments2D> CutBy(this IEnumerable<CircularStack<KtPoint2D>> listOfCorners1, IEnumerable<CircularStack<KtPoint2D>> listOfCorners2)
        {
            if (listOfCorners1.IsEmpty()) return new List<KtSegments2D>();
            var edges = listOfCorners2.SelectMany(arg => arg.ToEdges()).ToList();
            if (edges.IsEmpty()) return listOfCorners1.Select(corners => corners.ToSegments()).ToList();
            using var i = listOfCorners1.GetEnumerator();
            var list = new List<KtSegments2D>();
            while (i.MoveNext())
            {
                var corners = i.Current;
                var segment = corners.IntersectAndBreak(edges);
                if (segment == null)
                {
                    list.Add(corners.ToSegments());
                }
                else
                {
                    list.AddRange(segment.CutBy(edges));
                }
            }
            return list;
        }

        private static IEnumerable<KtSegments2D> CutBy(this KtSegments2D firstEdge, List<KtSegment2D> edges) => edges.Skip(1).Aggregate(firstEdge.CutBy(edges[0]), (results, edge) => results.Select(result => result.CutBy(edge)).SelectMany(result => result)).ToList();

        private static KtSegments2D IntersectAndBreak(this CircularStack<KtPoint2D> corners, List<KtSegment2D> segments)
        {
            var (corner1, segment1) = (corners, segments).FirstOfBoth((corner, segment) => segment.HasIntersection(corner.Previous.ToEdge()));
            if (corner1 == null && segment1 == null) return null;
            var intersection = corner1.Previous.ToEdge().IntersectionWith(segment1);
            var list = corner1.ToList();
            if (intersection != list[0]) list.Insert(0, intersection);
            if (intersection != list.Last()) list.Add(intersection);
            return new KtSegments2D(list);
        }
    }
}