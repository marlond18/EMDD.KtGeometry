using EMDD.KtGeometry.KtPoints;
using EMDD.KtGeometry.KtVectors;

using KtExtensions;

using System;

namespace EMDD.KtGeometry.KtLines._2D
{
    /// <summary>
    /// Ax + By = C
    /// </summary>
    public abstract class KtLine2DBase : IEquatable<KtLine2DBase>
    {
        protected KtLine2DBase(KtPoint2D start, KtPoint2D end)
        {
            if (start == end) throw new ArgumentException("Cannot create zero length line where start and end are equal");
            if (start == null || end == null) throw new ArgumentNullException($"Cannot create a 2D line with points ({start}) and  ({end}) where one is null");
            Start = start;
            End = end;
            Vector = (end - start).ToKtVector2D();
            A = Vector.Y;
            B = -Vector.X;
            C = Start.ToKtVector2D().Cross(Vector);
        }

        public KtVector2D Vector { get; }
        public KtPoint2D End { get; }
        public KtPoint2D Start { get; }
        public double A { get; }
        public double B { get; }
        public double C { get; }

        public bool Inline(KtPoint2D p) => p != null && SubstitutePoint(p).NearZero() && Bounds(p);

        internal abstract bool Bounds(KtPoint2D p);

        public double Distance(KtPoint2D p) => SubstitutePoint(p) / Vector.Magnitude;

        public double SubstitutePoint(KtPoint2D p) => (A * p.X) + (B * p.Y) - C;

        public KtPoint2D PointInsideLine(double parameter) =>
            Start + (Vector * parameter).ToKtPoint2D();

        public static bool operator !=(KtLine2DBase a, KtLine2DBase b) => !(a == b);

        public static bool operator ==(KtLine2DBase a, KtLine2DBase b) => a.DefaultEquals(b);

        public abstract KtLine2DBase Clone();

        public abstract bool Equals(KtLine2DBase other);

        public override bool Equals(object obj) => Equals(obj as KtLine2DBase);

        public override int GetHashCode() => unchecked(-31887.ChainHashCode(Start).ChainHashCode(Vector));

        public enum PointLocationRelativeToLine
        {
            InLine = 0,
            Left = 1,
            Right = -1
        }

        public bool HasIntersection(KtLine2DBase other) => GetParametricIntersection(other).pass;

        public KtPoint2D IntersectionWith(KtLine2DBase other)
        {
            var (pass, t1, _) = GetParametricIntersection(other);
            return !pass ? null : PointInsideLine(t1);
        }

        public bool HasThePoint(KtPoint2D point)
        {
            var parameter = (point - Start).ToKtVector2D() / Vector;
            if (parameter is null) return false;
            return ValidParametricValue(parameter);
        }

        private (bool pass, double t1, double t2) GetParametricIntersection(KtLine2DBase other)
        {
            var (s1Tos2, determinant) = (other.Start - Start, other.Vector.Cross(Vector));
            if (determinant.NearZero(6)) return (false, 0, 0);
            var t1 = other.Vector.Cross(s1Tos2) / determinant;
            var t2 = Vector.Cross(s1Tos2) / determinant;
            return (ValidParametricValue(t1) && other.ValidParametricValue(t2), t1, t2);
        }

        public abstract bool ValidParametricValue(double? parameter);

        [Obsolete("Use IntersectWith Instead")]
        public KtPoint2D IntersectWith3(KtLine2DBase line2)
        {
            var determinant = Vector.Cross(line2.Vector);
            if (determinant.NearZero(5)) return null;
            var (a1, a2, b1, b2, c1, c2) = (A, line2.A, B, line2.B, C, line2.C);
            var p = new KtPoint2D((b2 * c1) - (b1 * c2), (c2 * a1) - (c1 * a2)) / determinant;
            return Bounds(p) && line2.Bounds(p) ? p : null;
        }

        protected bool SameSlope(KtLine2DBase other) => Vector.Direction == other.Vector.Direction;

        public abstract override string ToString();
    }
}