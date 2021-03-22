using EMDD.KtGeometry.Angles;
using EMDD.KtGeometry.KtVectors;

using KtExtensions;

using System;
using System.Collections.Generic;

namespace EMDD.KtGeometry.Quaternions
{
    public class KtQuaternion : ICloneable, IEquatable<KtQuaternion>
    {
        public KtQuaternion(double w, double x, double y, double z)
        {
            W = w;
            X = x;
            Y = y;
            Z = z;
        }

        public KtQuaternion(Angle angle, KtVector3D axis)
        {
            var c = Angle.Cos(angle / 2);
            var s = Angle.Sin(angle / 2);
            W = c;
            X = s * axis.X;
            Y = s * axis.Y;
            Z = s * axis.Z;
        }

        public double W { get; }
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public object Clone() => new KtQuaternion(W, X, Y, Z);

        public override bool Equals(object obj) =>
            ReferenceEquals(obj, this) ||
            (!(this is null) && !(obj is null) &&
            obj is KtQuaternion q && Equals(q));

        public bool Equals(KtQuaternion other) =>
            ReferenceEquals(other, this) ||
            (!(this is null) &&
            !(other is null) &&
            W.NearEqual(other.W) &&
            X.NearEqual(other.X) &&
            Y.NearEqual(other.Y) &&
            Z.NearEqual(other.Z));

        public double Dot(KtQuaternion q) => q is null ? 0.0 :
            (W * q.W) + (X * q.X) + (Y * q.Y) + (Z * q.Z);

        public override int GetHashCode() => unchecked(HashCode.Combine(W, X, Y, Z));

        public static bool operator ==(KtQuaternion left, KtQuaternion right) =>
            EqualityComparer<KtQuaternion>.Default.Equals(left, right);

        public static bool operator !=(KtQuaternion left, KtQuaternion right) =>
            !(left == right);

        public static KtQuaternion operator *(KtQuaternion a, KtQuaternion b)
        {
            var w = (a.W * b.W) - (a.X * b.X) - (a.Y * b.Y) - (a.Z * b.Z);
            var x = (a.W * b.X) + (a.X * b.W) + (a.Y * b.Z) - (a.Z * b.Y);
            var y = (a.W * b.Y) - (a.X * b.Z) + (a.Y * b.W) + (a.Z * b.X);
            var z = (a.W * b.Z) + (a.X * b.Y) - (a.Y * b.X) + (a.Z * b.W);
            return new KtQuaternion(w, x, y, z);
        }

        public static KtQuaternion operator /(KtQuaternion a, double b)
        {
            if (a is null) return null;
            if (b.NearZero()) throw new DivideByZeroException("Quaternion Divided by zero");
            return new KtQuaternion(a.W / b, a.X / b, a.Y / b, a.Z / b);
        }

        public override string ToString() => $"({W:0.000}, {X:0.000}, {Y:0.000}, {Z:0.000})";

        public double Norm => Math.Sqrt(Dot(this));

        public KtQuaternion Conjugate() => new(W, -X, -Y, -Z);

        public KtQuaternion Inverse() => Conjugate() / Dot(this);
    }
}