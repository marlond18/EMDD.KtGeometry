using EMDD.KtGeometry.Angles;
using EMDD.KtGeometry.KtPoints;
using EMDD.KtGeometry.Quaternions;

using KtExtensions;

using System;

namespace EMDD.KtGeometry.KtVectors
{
    public class KtVector3D : ICloneable, IEquatable<KtVector3D>
    {
        public KtVector3D(double x, double y, double z)
        {
            (X, Y, Z) = (x, y, z);
        }

        public double X { get; }
        public double Y { get; }
        public double Z { get; }
        public double Magnitude => Math.Sqrt(SqrtMagnitude);
        public double SqrtMagnitude => Dot(this);

        public static bool operator !=(KtVector3D a, KtVector3D b) => !(a == b);

        public static bool operator ==(KtVector3D a, KtVector3D b) => a.DefaultEquals(b);

        public static KtVector3D operator -(KtVector3D vec) => vec is null ? null : new KtVector3D(-vec.X, -vec.Y, -vec.Z);

        public static KtVector3D operator *(KtVector3D vec, double d) => new(vec.X * d, vec.Y * d, vec.Z * d);

        public static KtVector3D operator *(double d, KtVector3D vec) => new(vec.X * d, vec.Y * d, vec.Z * d);

        public static KtVector3D operator /(KtVector3D vec, double d) => new(vec.X / d, vec.Y / d, vec.Z / d);

        public KtVector3D RotateBy(KtQuaternion q)
        {
            var ww = q.W * q.W;
            var xx = q.X * q.X;
            var yy = q.Y * q.Y;
            var zz = q.Z * q.Z;
            var xy = q.X * q.Y;
            var wz = q.W * q.Z;
            var wy = q.W * q.Y;
            var xz = q.X * q.Z;
            var yz = q.Y * q.Z;
            var wx = q.W * q.X;
            var x =
                (X * (ww + xx - yy - zz)) +
                (2 * Y * (xy - wz)) +
                (2 * Z * (wy + xz));
            var y =
                (2 * X * (wz + xy)) +
                (Y * (ww - xx + yy - zz)) +
                (2 * Z * (yz - wx));
            var z =
                (2 * X * (xz - wy)) +
                (2 * Y * (wx + yz)) +
                (Z * (ww - xx - yy + zz));
            return new KtVector3D(x, y, z);
        }

        public object Clone() => new KtVector3D(X, Y, Z);

        public override bool Equals(object obj) => Equals(obj as KtVector3D);

        public bool Equals(KtVector3D other) => this.TestNullBeforeEquals(other, () =>
        {
            var normal1 = Normalize();
            var normal2 = other.Normalize();
            return normal1.X.NearEqual(normal2.X, 10) && normal1.Y.NearEqual(normal2.Y, 10) && normal1.Z.NearEqual(normal2.Z);
        });

        public double Dot(KtVector3D vec) =>
            vec is null ? 0.0 : (X * vec.X) + (Y * vec.Y) + (Z * vec.Z);

        public KtVector3D Cross(KtVector3D vec) =>
            vec is null ? null : new KtVector3D((Y * vec.Z) - (Z * vec.Y), (Z * vec.X) - (X * vec.Z), (X * vec.Y) - (Y * vec.X));

        public Angle AngleBetween(KtVector3D b) => b switch
        {
            null => throw new ArgumentNullException(nameof(b)),
            _ => AngleBetweenInternal(b, Length, b.Length)
        };

        private Angle AngleBetweenInternal(KtVector3D b, double lengthA, double lengthB)
        {
            if (lengthA.NearZero()) throw new DivideByZeroException($"{this} has zero length");
            if (lengthB.NearZero()) throw new DivideByZeroException($"{b} has zero length");
            return Angle.Acos(Dot(b), lengthA * lengthB);
        }

        public double CosAngle(KtVector3D b) => b switch
        {
            null => throw new ArgumentNullException(nameof(b)),
            _ => CosAngleInternal(b, Length, b.Length)
        };

        public bool IsParallel(KtVector3D vec)
        {
            var a = CosAngle(vec);
            return a.NearEqual(1) || a.NearEqual(-1);
        }

        private double CosAngleInternal(KtVector3D b, double lengthA, double lengthB)
        {
            if (lengthA.NearZero()) throw new DivideByZeroException($"{this} has zero length");
            if (lengthB.NearZero()) throw new DivideByZeroException($"{b} has zero length");
            return Dot(b) / (lengthA * lengthB);
        }

        public KtQuaternion RotationFrom(KtVector3D b) =>
            new(AngleBetween(b), -Cross(b).Normalize());

        public KtPoint3D ToKtPoint3D() => new(X, Y, Z);

        public override int GetHashCode() => unchecked(HashCode.Combine(X, Y, Z));

        public override string ToString() => $"Vector({X:0.000},{Y:0.000},{Z:0.000})";

        public double Length => Math.Sqrt(Dot(this));

        public KtVector3D Normalize() => NormalizeInternal(Length);

        private KtVector3D NormalizeInternal(double len)
        {
            if (len.NearZero()) throw new DivideByZeroException($"{nameof(Normalize)} cannot continue with zero length");
            return this / len;
        }

        public bool IsZero => X.NearZero() && Y.NearZero() && Z.NearZero();

        public double[] ToArray() => new[] { X, Y, Z };
    }
}