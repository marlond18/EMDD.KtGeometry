using EMDD.KtGeometry.KtPoints;

using KtExtensions;

using System;

namespace EMDD.KtGeometry.KtLines._2D
{
    public class KtLine2D : KtLine2DBase
    {
        public KtLine2D(KtPoint2D start, KtPoint2D end) : base(start, end)
        {
        }

        public KtLine2D(double x1, double y1, double x2, double y2) : base(new KtPoint2D(x1, y1), new KtPoint2D(x2, y2))
        {
        }

        public KtLine2D(double a, double b, double c) : this(CreateStart(a, b, c), CreateStart(a, b, c) + new KtPoint2D(-b, a))
        {
        }

        public double YIntercept => C / B;

        public static KtLine2D CreateHorizontalLine(double y) => new(new KtPoint2D(0, y), new KtPoint2D(1, y));

        public static KtLine2D CreateVerticalLine(double x) => new(new KtPoint2D(x, 0), new KtPoint2D(x, 1));

        public static KtLine2D XAxis() => new(new KtPoint2D(), new KtPoint2D(1, 0));

        public static KtLine2D YAxis() => new(new KtPoint2D(), new KtPoint2D(0, 1));

        internal override bool Bounds(KtPoint2D p) => true;

        public override KtLine2DBase Clone() => new KtLine2D(Start, End);

        public bool Equals(KtLine2D other) => this.TestNullBeforeEquals(other, () => SameSlope(other) && SameYIntercept(other));

        public override bool Equals(KtLine2DBase other) => Equals(other as KtLine2D);

        public override string ToString() => MethodShortcuts.SeedProcess(
            () => $"{A:#.###}{(A.NearZero(5) ? "" : "X")}",
            () => B.NearZero() ? "" : B < 0 ? "-" : "+",
            () => $"{Math.Abs(B):#.###}{(B.NearZero(5) ? "" : "Y")}",
            (x, link, y) => $"({x}{link}{y}={C:0.###})");

        private static KtPoint2D CreateStart(double a, double b, double c) =>
            a.NearZero(5) ? new KtPoint2D(0, c / b) : new KtPoint2D(c / a, 0);

        private bool SameYIntercept(KtLine2D other)
        {
            var yIntercept1 = YIntercept;
            var yIntercept2 = other.YIntercept;
            if (yIntercept1.NearEqual(yIntercept2)) return true;
            if (yIntercept1.InvalidNumber() || yIntercept2.InvalidNumber()) return false;
            return (yIntercept1 - yIntercept2).NearZero(6);
        }

        public override bool ValidParametricValue(double? parameter) => parameter != null;
    }
}