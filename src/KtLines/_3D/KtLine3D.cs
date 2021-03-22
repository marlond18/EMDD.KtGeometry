using EMDD.KtGeometry.KtPoints;
using EMDD.KtGeometry.KtVectors;

using KtExtensions;

using System;

namespace EMDD.KtGeometry.KtLines._3D
{
    public static class KtLine3DHelper
    {
        public static double[] CrossOp(this double[] a, double[] b)
        {
            var len = a.Length;
            if (len != b.Length) throw new Exception($"length of {a} and {b} does not match. CrossOp cannot be performed.");
            var c = new double[len];
            c[0] = (a[1] * b[2]) - (a[^1] * b[^2]);
            for (int i = 2; i + 2 < len; i++)
            {
                c[i] = (a[i + 1] * b[i + 2]) - (a[i - 1] * b[i - 2]);
            }
            if (len > 3)
            {
                c[1] = (a[2] * b[3]) - (a[0] * b[^1]);
                c[^2] = (a[^1] * b[0]) - (a[^3] * b[^4]);
            }
            else if (len == 3)
            {
                c[1] = (a[2] * b[0]) - (a[0] * b[2]);
            }
            c[^1] = (a[0] * b[1]) - (a[^2] * b[^3]);
            return c;
        }

        public static double DotOp(this double[] a, double[] b)
        {
            var len = a.Length;
            if (len != b.Length) throw new Exception($"length of {a} and {b} does not match. CrossOp cannot be performed.");
            var total = 0d;
            for (int i = 0; i < len; i++)
            {
                total += a[i] * b[i];
            }
            return total;
        }
    }

    public abstract class KtLine3DBase
    {
        protected KtLine3DBase(KtPoint3D start, KtVector3D direction)
        {
            Start = start;
            Direction = direction;
        }

        public double Distance(KtPoint3D p2) =>
            Direction.Cross((p2 - Start).ToKtVector3D()).Magnitude / Direction.Magnitude;

        public double Distance(KtLine3DBase line2)
        {
            var V1xV2 = Direction.Cross(line2.Direction);
            var len = V1xV2.Length;
            if (len.NearZero(5)) return double.PositiveInfinity;
            var Vx = (line2.Start - Start).ToKtVector3D();
            return Math.Abs(V1xV2.Dot(Vx) / V1xV2.Length);
        }

        public KtPoint3D PointInsideLine(double parameter) =>
            ParamIsValid(parameter) ? Start + (Direction * parameter).ToKtPoint3D() : null;

        protected abstract bool ParamIsValid(double param);

        public KtPoint3D Start { get; protected set; }
        public KtVector3D Direction { get; protected set; }

        public (KtPoint3D a, KtPoint3D b) Intersect(KtLine3DBase line2)
        {
            if (line2 is null) return (null, null);
            var r = (line2.Start - Start).ToKtVector3D();
            var rv1 = r.Dot(Direction);
            var rv2 = r.Dot(line2.Direction);
            var v1V1 = Direction.Dot(Direction);
            var v2V2 = line2.Direction.Dot(line2.Direction);
            var v1V2 = Direction.Dot(line2.Direction);
            var denominator = (v1V2 * v1V2) - (v1V1 * v2V2);
            if (denominator.NearZero(5)) return (null, null);
            var param1 = ((rv2 * v1V2) - (rv1 * v2V2)) / denominator;
            var param2 = ((rv2 * v1V1) - (rv1 * v1V2)) / denominator;
            return (PointInsideLine(param1), line2.PointInsideLine(param2));
        }

        public (KtPoint3D a, KtPoint3D b) Intersect2(KtLine3DBase line2)
        {
            var V1xV2 = Direction.ToArray().CrossOp(line2.Direction.ToArray());
            var den = V1xV2.DotOp(V1xV2);
            if (den.NearZero(5)) return (null, null);
            var Vx = (line2.Start - Start).ToArray();
            var VxxV1 = Vx.CrossOp(Direction.ToArray());
            var VxxV2 = Vx.CrossOp(line2.Direction.ToArray());
            return (PointInsideLine(VxxV2.DotOp(V1xV2) / den), line2.PointInsideLine(VxxV1.DotOp(V1xV2) / den));
        }

        public (double Distance, KtPoint3D a, KtPoint3D b) DistanceIntersection(KtLine3DBase line2)
        {
            var V1xV2 = Direction.Cross(line2.Direction);
            var den = V1xV2.Dot(V1xV2);
            if (den.NearZero(5)) return (double.PositiveInfinity, null, null);
            var Vx = (line2.Start - Start).ToKtVector3D();
            var VxxV1 = Vx.Cross(Direction);
            var VxxV2 = Vx.Cross(line2.Direction);
            return (Math.Abs(V1xV2.Dot(Vx) / Math.Sqrt(den)), PointInsideLine(VxxV2.Dot(V1xV2) / den), line2.PointInsideLine(VxxV1.Dot(V1xV2) / den));
        }
    }

    public class KtLineSegment3D : KtLine3DBase, IEquatable<KtLineSegment3D>
    {
        public KtLineSegment3D(KtPoint3D start, KtPoint3D end) : base(start, (end - start).ToKtVector3D())
        {
            End = end;
        }

        public KtPoint3D End { get; }

        public override bool Equals(object obj) => this.DefaultEquals(obj as KtLineSegment3D);

        public bool Equals(KtLineSegment3D other) => this.TestNullBeforeEquals(other, () =>
        (Start.DefaultEquals(other.Start) && End.DefaultEquals(other.End)) ||
        (End.DefaultEquals(other.Start) && Start.DefaultEquals(other.End))
        );

        public override int GetHashCode() => unchecked(HashCode.Combine(Start, End) + HashCode.Combine(End, Start));

        protected override bool ParamIsValid(double param) => param >= -0.0000000001 && param <= 1.0000000001;

        public static bool operator ==(KtLineSegment3D left, KtLineSegment3D right) => left.DefaultEquals(right);

        public static bool operator !=(KtLineSegment3D left, KtLineSegment3D right) => !(left == right);
    }

    public class KtRay3D : KtLine3DBase, IEquatable<KtRay3D>
    {
        public KtRay3D(KtPoint3D start, KtVector3D direction) : base(start, direction)
        {
            (Start, Direction) = (start, direction);
        }

        protected override bool ParamIsValid(double param) => param >= -0.0000000001;

        public override bool Equals(object obj) => this.DefaultEquals(obj as KtRay3D);

        public bool Equals(KtRay3D other) => this.TestNullBeforeEquals(other, () =>
        Start.DefaultEquals(other.Start) && Direction.DefaultEquals(other.Direction));

        public override int GetHashCode() => unchecked(HashCode.Combine(Start, Direction));

        public static bool operator ==(KtRay3D left, KtRay3D right) => left.DefaultEquals(right);

        public static bool operator !=(KtRay3D left, KtRay3D right) => !(left == right);
    }

    public class KtLine3D : KtLine3DBase, IEquatable<KtLine3D>
    {
        public KtLine3D(KtPoint3D start, KtVector3D direction) : base(start, direction)
        {
            (Start, Direction) = (start, direction);
        }

        public override bool Equals(object obj) => this.DefaultEquals(obj as KtLine3D);

        public bool Equals(KtLine3D other) => this.TestNullBeforeEquals(other, () =>
        {
            var dir1 = (Start - other.Start).ToKtVector3D();
            return other.Direction.IsParallel(dir1) && Direction.IsParallel(-dir1);
        });

        public override int GetHashCode() => unchecked(HashCode.Combine(Start, Direction, -Direction));

        protected override bool ParamIsValid(double param) => true;

        public static bool operator ==(KtLine3D left, KtLine3D right) => left.DefaultEquals(right);

        public static bool operator !=(KtLine3D left, KtLine3D right) => !(left == right);
    }
}