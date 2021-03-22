using CircularStack;

using EMDD.KtGeometry.Angles;
using EMDD.KtGeometry.KtLines._2D;

using KtExtensions;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using static EMDD.KtGeometry.KtPolygons.RegionClipMethods;
using static KtExtensions.NumericExtensions;

using EnumerableHollows = System.Collections.Generic.IEnumerable<EMDD.KtGeometry.KtPolygons.Regions.KtHollowRegion>;
using EnumerablePoints = System.Collections.Generic.IEnumerable<EMDD.KtGeometry.KtPoints.KtPoint2D>;
using EnumerableRegions = System.Collections.Generic.IEnumerable<EMDD.KtGeometry.KtPolygons.Regions.KtRegion>;
using EnumerableSolids = System.Collections.Generic.IEnumerable<EMDD.KtGeometry.KtPolygons.Regions.KtSolidRegion>;
using Point = EMDD.KtGeometry.KtPoints.KtPoint2D;
using Points = System.Collections.Generic.List<EMDD.KtGeometry.KtPoints.KtPoint2D>;

namespace EMDD.KtGeometry.KtPolygons.Regions
{
    public abstract class KtRegion : EnumerablePoints, IEquatable<KtRegion>, ICloneable
    {
        public static implicit operator KtRegion(Points points) => points.ToRegion();

        public (double left, double top, double right, double bottom) BoundingRec()
        {
            var (minx, maxx) = Corners.MinMax(corner => corner.X);
            var (miny, maxy) = Corners.MinMax(corner => corner.Y);
            return (minx, maxy, maxx, miny);
        }

        public static implicit operator KtRegion(Point[] points) => points.ToRegion();

        protected KtRegion()
        {
            Corners = new Points();
            IsReadOnly = false;
        }

        protected KtRegion(EnumerablePoints corners, bool isReadOnly = false)
        {
            if (corners == null) throw new ArgumentNullException(nameof(corners), "Cannot create a region with null corners");
            Corners = new Points(corners.PreventEqualFirstAndLastCorner());
            IsReadOnly = isReadOnly;
        }

        public Points Corners { get; }

        public int Count => Corners.Count;

        public bool IsClockwise => Area() < 0.0;

        public bool IsReadOnly { get; }

        public static bool operator !=(KtRegion a, KtRegion b) => !(a == b);

        public static bool operator ==(KtRegion a, KtRegion b) => a.DefaultEquals(b);

        public void Add(Point item)
        {
            if (item == null) throw new NullReferenceException("Cannot add a corner which is null to a region");
            if (Count == 0 || item != Corners[0]) Corners.Add(item);
        }

        public abstract double Area();

        public abstract Point AreaCentroid();

        public Point Centroid() => AreaCentroid() / Area();

        public bool Circumscribes(KtRegion region) => region?.Inscribes(this) == true;

        public void Clear()
        {
            Corners.Clear();
        }

        public abstract object Clone();

        public bool Contains(Point item) => Corners.Find(c => c == item) != null;

        public void CopyTo(Point[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            var ppArray = array;
            if (ppArray == null) throw new ArgumentException("Cannot Copy the array", nameof(array));
            this.ToArray().CopyTo(ppArray, arrayIndex);
        }

        public IEnumerable<KtSegment2D> Edges() => LoopedCorners().ToEdges();

        public bool Equals(KtRegion other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other is null && this is null) return false;
            return EqualityMethod(other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj is null && this is null) return false;
            return obj.GetType() == GetType() && Equals((KtRegion)obj);
        }

        public IEnumerator<Point> GetEnumerator() => Corners.GetEnumerator();

        public override int GetHashCode() => unchecked(HashCodeMethod());

        public abstract bool Inscribes(KtRegion region);

        public abstract bool Inscribes(Point p);

        public abstract bool Inscribes(KtSegment2D segment);

        public bool IsConvex
        {
            get
            {
                var gotNegative = false;
                var gotPositive = false;
                foreach (var corner in LoopedCorners())
                {
                    var cross = (corner.Previous.Value - corner.Value).ToKtVector2D().Cross((corner.Next.Value - corner.Value).ToKtVector2D()).Sign();
                    if (cross == NumberSign.Negative)
                        gotNegative = true;
                    else
                        gotPositive = true;
                    if (gotNegative && gotPositive) return true;
                }
                return gotPositive && gotNegative;
            }
        }

        public bool IsIntersecting(KtRegion region) => IsIntersectedWithAnyEdge(region) || Inscribes(region) || Circumscribes(region);

        public bool IsIntersecting(KtSegment2D segment) => Edges().Any(segment.HasIntersection);

        public bool Remove(Point item) => Corners.Remove(item);

        public abstract KtRegion Reverse();

        public Points RotateEdges(Angle angle) => Corners.ConvertAll(corner => corner.Rotate(angle));

        public Points RotateEdgesAt(Angle angle, Point point) => Corners.ConvertAll(corner => corner.RotateAt(angle, point));

        internal (EnumerableSolids solid, EnumerableHollows hollow) Clip(EnumerableRegions clips, ClipType clipType, bool reversed = false) => new[] { this }.Clip(clips, clipType, reversed);

        internal (EnumerableSolids solid, EnumerableHollows hollow) Clip(KtRegion clip, ClipType clipType, bool reversed = false) => new[] { this }.Clip(new[] { clip }, clipType, reversed);

        internal abstract bool EqualityMethod(KtRegion other);

        internal abstract int HashCodeMethod();

        internal double InitialArea() => Count < 3 ? 0.0 : Corners.CrossProductToNext() / 2;

        internal Point InitialAreaCentroid() => LoopedCorners().Aggregate(new Point(0, 0), (initTotal, corner) => ((corner.Value + corner.Next.Value) * corner.CrossProductToNext()) + initTotal) / 6;

        internal abstract bool IsIntersectedWithAnyEdge(KtRegion region);

        internal bool IsIntersectingEdges(KtRegion region) => region.Edges().Any(IsIntersecting);

        internal CircularStack<Point> LoopedCorners() => new(Corners);

        protected void FixCorners(bool makeSolid = true)
        {
            if (makeSolid != (Corners.CrossProductToNext() > 0)) Corners.Reverse();
        }

        public Points TranslateCorners(Point point) => Corners.ConvertAll(corner => corner.Translate(point));

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}