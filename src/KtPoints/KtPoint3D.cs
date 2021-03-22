using EMDD.KtGeometry.KtVectors;

using KtExtensions;

using System;

namespace EMDD.KtGeometry.KtPoints
{
    [Serializable]
    public class KtPoint3D : IEquatable<KtPoint3D>, ICloneable
    {
        public KtPoint3D(double x, double y, double z)
        {
            (X, Y, Z) = (x, y, z);
        }

        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public static KtPoint3D operator -(KtPoint3D a, KtPoint3D b) => a + -b;

        public static KtPoint3D operator -(KtPoint3D a) => a is null ? null : new KtPoint3D(-a.X, -a.Y, -a.Z);

        public static bool operator !=(KtPoint3D a, KtPoint3D b) => !(a == b);

        public static KtPoint3D operator +(KtPoint3D a, KtPoint3D b) =>
            a is null && b is null ? null :
            b is null ? a :
            a is null ? b :
            new KtPoint3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static KtPoint3D operator /(KtPoint3D a, double b) =>
            b.NearZero() ? throw new DivideByZeroException($"{a}/0") :
            a is null ? null :
            new KtPoint3D(a.X / b, a.Y / b, a.Z / b);

        public static KtPoint3D operator *(KtPoint3D a, double b) =>
            b.NearZero() ? new KtPoint3D(0, 0, 0) :
            a is null ? null :
            new KtPoint3D(a.X * b, a.Y * b, a.Z * b);

        public static KtPoint3D operator +(KtPoint3D a, KtVector3D b) =>
            a is null && b is null ? null :
            b is null ? a :
            a is null ? b.ToKtPoint3D() :
            new KtPoint3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static bool operator ==(KtPoint3D a, KtPoint3D b) => a.DefaultEquals(b);

        public object Clone() => new KtPoint3D(X, Y, Z);

        public override bool Equals(object obj) => this.DefaultEquals(obj as KtPoint3D);

        public bool Equals(KtPoint3D other) => this.TestNullBeforeEquals(other, () => (X - other.X).NearZero() && (Y - other.Y).NearZero() && (Z - other.Z).NearZero());

        public override int GetHashCode() => unchecked((X.GetHashCode() * 5) ^ Y.GetHashCode() ^ Z.GetHashCode() * 31);

        public KtVector3D ToKtVector3D() => new(X, Y, Z);

        public override string ToString() => $"Point(X:{X:0.000}, Y:{Y:0.000}, Z:{Z:0.000})";

        public double[] ToArray() => new[] { X, Y, Z };

        public KtPoint3D Offset(double x, double y, double z)
        {
            return new KtPoint3D(X + x, Y + y, Z + z);
        }
    }
}