using EMDD.KtGeometry.KtPoints;

using KtExtensions;

using System.Collections.Generic;

namespace EMDD.KtGeometry.KtLines._2D
{
    public class KtSegment2D : KtLine2DBase
    {
        public KtSegment2D(KtPoint2D start, KtPoint2D end) : base(start, end)
        {
        }

        public KtSegment2D(double x1, double y1, double x2, double y2) : this(new KtPoint2D(x1, y1), new KtPoint2D(x2, y2))
        {
        }

        internal override bool Bounds(KtPoint2D p) => p.X.IsWithin(Start.X, End.X) && p.Y.IsWithin(Start.Y, End.Y);

        public override KtLine2DBase Clone() => new KtSegment2D(Start, End);

        public IEnumerable<KtSegment2D> CutBy(KtSegment2D other)
        {
            var intersection = IntersectionWith(other);
            if (intersection is null || intersection == Start || intersection == End) return new[] { this };
            return new[] { new KtSegment2D(Start, intersection), new KtSegment2D(intersection, End) };
        }

        public IEnumerable<KtSegment2D> CutBy(IEnumerable<KtSegment2D> lines) => new[] { this }.CutBy(lines);

        public bool Equals(KtSegment2D other) => this.TestNullBeforeEquals(other, () => Start == other.Start && End == other.End);

        public override bool Equals(KtLine2DBase other) => Equals(other as KtSegment2D);

        public double Length => Vector.Magnitude;

        public override bool ValidParametricValue(double? parameter) => parameter != null && ((double)parameter).IsWithin(-0.00000001, 1.000000001);

        public override string ToString() => $"Line Segment from {Start} to {End}";

        public KtPoint2D MidPoint() => (Start + End) / 2;
    }
}