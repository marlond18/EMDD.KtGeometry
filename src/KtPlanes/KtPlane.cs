using EMDD.KtGeometry.KtLines._3D;
using EMDD.KtGeometry.KtPoints;
using EMDD.KtGeometry.KtVectors;

using KtExtensions;

using System;
using System.Collections.Generic;

namespace EMDD.KtGeometry.KtPlanes
{
    public class KtPlane : IEquatable<KtPlane>
    {
        /// <summary>
        ///ax+by+cz=d
        /// </summary>
        /// <param name="a">Coefficient of X</param>
        /// <param name="b">Coefficient of Y</param>
        /// <param name="c">Coefficient of Z</param>
        /// <param name="d">Constant</param>
        public KtPlane(double a, double b, double c, double d)
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }

        public double A { get; }
        public double B { get; }
        public double C { get; }
        public double D { get; }

        public override bool Equals(object obj) => this.DefaultEquals(obj as KtPlane);

        public bool Equals(KtPlane other) => this.TestNullBeforeEquals(other, () =>
            (B / A).NearEqual(other.B / other.A) &&
            (C / A).NearEqual(other.C / other.A) &&
            (D / A).NearEqual(other.D / other.A));

        public override int GetHashCode() => unchecked(HashCode.Combine(B / A, C / A, D / A));

        public static bool operator ==(KtPlane left, KtPlane right) => EqualityComparer<KtPlane>.Default.Equals(left, right);

        public static bool operator !=(KtPlane left, KtPlane right) => !(left == right);

        public double Evaluate(KtPoint3D point) => point is null ? -D :
            (A * point.X) + (B * point.Y) + (C * point.Z) - D;

        public KtVector3D Normal => new(A, B, C);

        public KtPoint3D Intersect(KtLine3DBase line)
        {
            var a = Evaluate(line.Start);
            var b = Normal.Dot(line.Direction);
            if (b.NearZero()) return null;
            return line.PointInsideLine(-a / b);
        }

        public KtLine3D Intersect(KtPlane plane)
        {
            var v = Normal.Cross(plane.Normal);
            if (v.IsZero) return null;
            var d = (A * plane.B) - (plane.A * B);
            var x = ((B * plane.D) - (plane.B * D)) / d;
            var y = ((plane.A * D) - (A * plane.D)) / d;
            return new KtLine3D(new KtPoint3D(x, y, 0), v);
        }
    }
}