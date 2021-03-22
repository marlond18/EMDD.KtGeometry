using EMDD.KtGeometry.Angles;
using EMDD.KtGeometry.KtPoints.Quadrant_Check;
using EMDD.KtGeometry.KtVectors;

using KtExtensions;

using System;

using static System.Math;

namespace EMDD.KtGeometry.KtPoints
{
    [Serializable]
    public class KtPoint2D : ICloneable, IEquatable<KtPoint2D>
    {
        public KtPoint2D() : this(0, 0)
        {
        }

        public KtPoint2D(double x, double y)
        {
            (X, Y) = (x, y);
        }

        public double X { get; }
        public double Y { get; }

        public KtPoint2D Clone() => new(X, Y);

        object ICloneable.Clone() => new KtPoint2D(X, Y);

        public bool Equals(KtPoint2D other) =>
            this.TestNullBeforeEquals(other, () => X.NearEqual(other.X, 6) && Y.NearEqual(other.Y, 6));

        public static KtPoint2D operator -(KtPoint2D a, KtPoint2D b) => a + -b;

        public static KtPoint2D operator -(KtPoint2D a) => a is null ? null : new KtPoint2D(-a.X, -a.Y);

        public static bool operator !=(KtPoint2D a, KtPoint2D b) => !(a == b);

        public static KtPoint2D operator +(KtPoint2D a, KtPoint2D b)
        {
            return (a, b) switch
            {
                (null, null) => default,
                (null, _) => b,
                (_, null) => a,
                _ => new KtPoint2D(a.X + b.X, a.Y + b.Y)
            };
        }

        public static KtPoint2D operator +(KtPoint2D a, KtVector2D b) => a + b.ToKtPoint2D();

        public static KtPoint2D operator *(KtPoint2D a, double b) => a is null ? null : new KtPoint2D(a.X * b, a.Y * b);

        public static KtPoint2D operator *(KtPoint2D a, double? b) => b is null || a is null ? null : a * (double)(a.X * b);

        public static KtPoint2D operator /(KtPoint2D a, double b)
        {
            if (a is null) return default;
            if (b.NearZero(5)) throw new DivideByZeroException($"Cannot divide point {a} with 0");
            return new KtPoint2D(a.X / b, a.Y / b);
        }

        public static bool operator ==(KtPoint2D a, KtPoint2D b) => a.DefaultEquals(b);

        public override bool Equals(object obj) => Equals(obj as KtPoint2D);

        public override int GetHashCode() => unchecked(X.GetHashCode() ^ (Y.GetHashCode() * 31));

        public double Magnitude() => Sqrt(X.RaiseTo(2) + Y.RaiseTo(2));

        public Quadrant2D Quadrant() => QuadrantCheck.List().Find(check => check.Checked(X, Y)).Quadrant();

        public KtPoint2D Rotate(Angle angle) =>
            new((X * Angle.Cos(angle)) - (Y * Angle.Sin(angle)), (X * Angle.Sin(angle)) + (Y * Angle.Cos(angle)));

        public KtPoint2D RotateAt(Angle angle, KtPoint2D point) => Translate(-point).Rotate(angle).Translate(point);

        public KtVector2D ToKtVector2D() => new(X, Y);

        public override string ToString() => $"X:{X:0.000}, Y:{Y:0.000}";

        public KtPoint2D Translate(KtPoint2D distance) => this + distance;

        public KtPoint2D Translate(double x, double y) => this + new KtPoint2D(x, y);

        public static implicit operator KtPoint2D((double X, double Y) tuple) =>
            new(tuple.X, tuple.Y);

        public static implicit operator (double x, double y)(KtPoint2D point) =>
            point == null ? (0.0, 0.0) : (point.X, point.Y);

        public double DistanceFrom(KtPoint2D p) => Sqrt(Pow(p.X - X, 2) + Pow(p.Y - Y, 2));
    }
}