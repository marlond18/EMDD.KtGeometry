using EMDD.KtGeometry.Angles;
using EMDD.KtGeometry.KtPoints;
using EMDD.KtGeometry.KtPolygons.Regions;

using KtExtensions;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using EnumerableSolids = System.Collections.Generic.IEnumerable<EMDD.KtGeometry.KtPolygons.Regions.KtSolidRegion>;
using SolidRegions = System.Collections.Generic.List<EMDD.KtGeometry.KtPolygons.Regions.KtSolidRegion>;

namespace EMDD.KtGeometry.KtPolygons
{
    public class KtPolygon2D : EnumerableSolids, ICloneable, IEquatable<KtPolygon2D>
    {
        public static implicit operator KtPolygon2D(KtSolidRegion region) => region ?? new KtPolygon2D(region);

        private SolidRegions _regions;

        public (double left, double top, double right, double bottom) BoundingRectangle()
        {
            var bounds = _regions.Select(reg => reg.BoundingRec());
            var left = bounds.Min(val => val.left);
            var top = bounds.Max(val => val.top);
            var right = bounds.Max(val => val.right);
            var bottom = bounds.Max(val => val.bottom);
            return (left, top, right, bottom);
        }

        public KtPolygon2D(params KtSolidRegion[] regions) : this()
        {
            foreach (var region in regions) Add(region);
        }

        public KtPolygon2D()
        {
            _regions = new SolidRegions();
        }

        public bool IsEmpty => _regions is null || _regions.Count < 1;

        public static bool operator !=(KtPolygon2D a, KtPolygon2D b) => !(a == b);

        public static bool operator ==(KtPolygon2D a, KtPolygon2D b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.Equals(b);
        }

        public void Add(KtRegion region)
        {
            switch (region)
            {
                case KtHollowRegion hollow:
                    Add(hollow);
                    break;

                case KtSolidRegion solid:
                    Add(solid);
                    break;

                default:
                    throw new Exception("");
            }
        }

        public KtPoint2D Centroid()
        {
            var Atotal = 0d;
            var Ad = new KtPoint2D();
            foreach (var r in _regions)
            {
                Atotal += r.Area();
                Ad += r.AreaCentroid();
            }
            return Ad / Atotal;
        }

        internal void Add(KtHollowRegion region)
        {
            if (region == null) throw new ArgumentNullException($"An Empty region= ({region}) cannot be added to the polygon= ({this})");
            var (intersected, notintersected) = _regions.Fork(reg => region.IsIntersecting(reg));
            if (!intersected.IsEmpty())
            {
                _regions = notintersected.ToList();
                _regions.AddRange(intersected.Select(reg => reg.Remove(region)).SelectMany(reg => reg));
            }
        }

        internal void Add(KtSolidRegion region)
        {
            if (region == null) throw new ArgumentNullException($"An Empty region= ({region}) cannot be added to the polygon= ({this})");
            var (intersected, notintersected) = _regions.Fork(reg => region.IsIntersecting(reg));
            if (intersected.IsEmpty())
            {
                _regions.Add(region);
            }
            else
            {
                _regions = notintersected.Concat(new[] { intersected.Aggregate(region, (total, reg) => total.Add(reg).FirstOrDefault()) }).ToList();
            }
        }

        public object Clone() => new KtPolygon2D(_regions.IsEmpty() ? Array.Empty<KtSolidRegion>() : _regions.Select(r => (KtSolidRegion)r.Clone()).ToArray());

        public bool Equals(KtPolygon2D other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (this is null || other is null) return false;
            return this.Count() == other.Count() && _regions.All(other._regions.Contains);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (this is null || obj is null) return false;
            return obj is KtPolygon2D polygon && Equals(polygon);
        }

        public IEnumerator<KtSolidRegion> GetEnumerator() => _regions.GetEnumerator();

        public override int GetHashCode()
        {
            unchecked
            {
                return 2095643811 + _regions.Aggregate(0, (total, val) => total ^ val.GetHashCode());
            }
        }

        public void Remove(KtRegion region)
        {
            var (intersected, notintersected) = _regions.Fork(reg => region.IsIntersecting(reg));
            if (!intersected.IsEmpty())
            {
                _regions = notintersected.ToList();
                foreach (var reg in intersected)
                {
                    foreach (var cut in reg.Remove(region))
                    {
                        _regions.Add(cut);
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static KtPolygon2D operator &(KtPolygon2D a, KtPolygon2D b)
        {
            if (a is null) return b;
            if (b is null) return a;
            var leftOnA = a.MakeEnumerable();
            var leftOnB = b.MakeEnumerable();
            var results = new SolidRegions();
            foreach (var itemA in leftOnA)
            {
                var intersections = leftOnB.Where(itemA.IsIntersecting);
                if (!intersections.IsEmpty())
                {
                    results.AddRange(intersections.Select(itemA.Intersect).SelectMany(ints => ints));
                }
                else
                {
                    results.Add(itemA);
                }
            }
            return new KtPolygon2D { _regions = results };
        }

        public static KtPolygon2D operator |(KtPolygon2D a, KtPolygon2D b)
        {
            if (a is null) return b;
            if (b is null) return a;
            var leftOnA = a.MakeEnumerable();
            var leftOnB = b.MakeEnumerable();
            KtSolidRegion start;
            var regionsResult = new SolidRegions();
            EnumerableSolids intersections;
            while (!leftOnA.IsEmpty() && !leftOnB.IsEmpty())
            {
                (start, leftOnA) = leftOnA.SplitFirst();
                (intersections, leftOnB) = leftOnB.Fork(regionOnB => start.IsIntersecting(regionOnB));
                while (!intersections.IsEmpty())
                {
                    foreach (var item in intersections)
                    {
                        start = start.Add(item).FirstOrDefault();
                    }
                    (intersections, leftOnB) = leftOnB.Fork(regionOnB => start.IsIntersecting(regionOnB));
                    if (intersections.IsEmpty())
                    {
                        (intersections, leftOnA) = leftOnA.Fork(regionOnB => start.IsIntersecting(regionOnB));
                    }
                }
                regionsResult.Add(start);
            }
            return new KtPolygon2D { _regions = regionsResult };
        }

        public static KtPolygon2D operator +(KtPolygon2D a, KtPolygon2D b) => a | b;

        public static KtPolygon2D operator ^(KtPolygon2D a, KtPolygon2D b)
        {
            if (a is null) return b;
            if (b is null) return a;
            var leftOnA = a.MakeEnumerable();
            var leftOnB = b.MakeEnumerable();
            KtSolidRegion start;
            var regionsResult = new SolidRegions();
            EnumerableSolids xor;
            while (!leftOnA.IsEmpty() && !leftOnB.IsEmpty())
            {
                (start, leftOnA) = leftOnA.SplitFirst();
                (xor, leftOnB) = leftOnB.Fork(regionOnB => start.IsIntersecting(regionOnB));
                while (!xor.IsEmpty())
                {
                    foreach (var item in xor)
                    {
                        start = start.Add(item).FirstOrDefault();
                    }
                    (xor, leftOnB) = leftOnB.Fork(regionOnB => start.IsIntersecting(regionOnB));
                    if (xor.IsEmpty())
                    {
                        (xor, leftOnA) = leftOnA.Fork(regionOnB => start.IsIntersecting(regionOnB));
                    }
                }
                regionsResult.Add(start);
            }
            return new KtPolygon2D { _regions = regionsResult };
        }

        public KtPolygon2D Translate(KtPoint2D point) => new(_regions.Select(reg => reg.Translate(point)).ToArray());

        public static KtPolygon2D operator -(KtPolygon2D a, KtPolygon2D b)
        {
            if (a is null) return null;
            if (b is null) return a;
            var leftOnA = a.MakeEnumerable();
            var leftOnB = b.MakeEnumerable();
            var results = new SolidRegions();
            foreach (var itemA in leftOnA)
            {
                var intersections = leftOnB.Where(itemA.IsIntersecting);
                if (!intersections.IsEmpty())
                {
                    results.AddRange(intersections.Aggregate(new[] { itemA }.MakeEnumerable(), (all, inter) => all.Select(pol => pol.Remove(inter)).SelectMany(i => i)));
                }
                else
                {
                    results.Add(itemA);
                }
            }
            return new KtPolygon2D { _regions = results };
        }

        public KtPolygon2D Rotate(Angle angle)
        {
            var newSolids = new SolidRegions();
            foreach (var solid in _regions)
            {
                var newSolid = new KtSolidRegion(solid.RotateEdges(angle)) { holes = solid.Holes.Select(hole => new KtHollowRegion(hole.RotateEdges(angle))).ToList() };
                newSolids.Add(newSolid);
            }
            return new KtPolygon2D { _regions = newSolids };
        }

        public KtPolygon2D RotateAt(Angle angle, KtPoint2D center)
        {
            var newSolids = new SolidRegions();
            foreach (var solid in _regions)
            {
                var newSolid = new KtSolidRegion(solid.RotateEdgesAt(angle, center)) { holes = solid.Holes.Select(hole => new KtHollowRegion(hole.RotateEdgesAt(angle, center))).ToList() };
                newSolids.Add(newSolid);
            }
            return new KtPolygon2D { _regions = newSolids };
        }

        public IEnumerable<(List<KtPoint2D> solid, IEnumerable<List<KtPoint2D>> hollow)> GetPoints()
        {
            foreach (var region in _regions)
            {
                yield return (region.Corners, region.Holes.Select(r => r.Corners));
            }
        }
    }
}