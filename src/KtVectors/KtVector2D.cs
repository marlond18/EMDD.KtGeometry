using EMDD.KtGeometry.Angles;
using EMDD.KtGeometry.KtPoints;

using KtExtensions;

using System;

namespace EMDD.KtGeometry.KtVectors
{
    public class KtVector2D : ICloneable, IEquatable<KtVector2D>
    {
        public KtVector2D((double x, double y) pair)
        {
            (X, Y) = pair;
        }

        public KtVector2D(double x, double y) : this((x, y))
        {
        }

        public KtVector2D(Angle angle) : this(Angle.Cos(angle), Angle.Sin(angle))
        {
        }

        public double X { get; private set; }
        public double Y { get; private set; }

        public void Normalize()
        {
            NormalizeInner(Magnitude);
        }

        private void NormalizeInner(double magnitude)
        {
            if (magnitude.NearZero()) return;
            X /= magnitude;
            Y /= magnitude;
        }

        public void Rotate(Angle angle)
        {
            (X, Y) = RotateInner(X, Y, angle);
        }

        private static (double x, double y) RotateInner(double x, double y, Angle angle) =>
            RotateInner(x, y, Angle.Sin(angle), Angle.Cos(angle));

        private static (double x, double y) RotateInner(double x, double y, double sineAngle, double cosAngle) =>
            ((x * cosAngle) - (y * sineAngle), (x * sineAngle) + (y * cosAngle));

        public Angle AngleBetween(KtVector2D vec) => vec is null ? Degrees.Create(0) : Degrees.Create(NumericExtensions.Atan3(Cross(vec), Dot(vec)));

        public double Magnitude => Math.Sqrt(Dot(this));

        public Angle Direction => Degrees.Create(NumericExtensions.Atan3(Y, X)).MakePositive();

        public double Dot(KtVector2D vec) => (X * vec?.X) + (Y * vec?.Y) ?? 0.0;

        public double Cross(KtVector2D vec) => (X * vec?.Y) - (Y * vec?.X) ?? 0;

        public static bool operator !=(KtVector2D a, KtVector2D b) => !(a == b);

        public static bool operator ==(KtVector2D a, KtVector2D b) => a.DefaultEquals(b);

        public static KtVector2D operator *(KtVector2D a, double b) =>
            a is null ? null : new KtVector2D(a.X * b, a.Y * b);

        public static KtVector2D operator /(KtVector2D a, double b) =>
            a is null || b.NearZero() ? null : new KtVector2D(a.X / b, a.Y / b);

        public static double? operator /(KtVector2D a, KtVector2D b)
        {
            Angle angle1 = a.Direction;
            Angle angle2 = b.Direction;
            if (angle1 == angle2) return a.Magnitude / b.Magnitude;
            if (angle1 == angle2.Complementary().NormalizeAngle()) return -a.Magnitude / b.Magnitude;
            else return null;
        }

        public override bool Equals(object obj) => Equals(obj as KtVector2D);

        public bool Equals(KtVector2D other) =>
            this.TestNullBeforeEquals(other, () => (X - other.X).NearZero() && (Y - other.Y).NearZero());

        public override int GetHashCode() => unchecked(X.GetHashCode().ChainHashCode(Y));

        public object Clone() => new KtVector2D(X, Y);

        public KtPoint2D ToKtPoint2D() => new(X, Y);

        public override string ToString() => $"X:{X:0.000}, Y:{Y:0.000}";

        public double Cross(KtPoint2D ktPoint2D) => (X * ktPoint2D?.Y) - (Y * ktPoint2D?.X) ?? 0;
    }
}