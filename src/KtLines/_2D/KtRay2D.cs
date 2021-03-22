using EMDD.KtGeometry.KtPoints;
using EMDD.KtGeometry.KtVectors;

using KtExtensions;

namespace EMDD.KtGeometry.KtLines._2D
{
    public class KtRay2D : KtLine2DBase
    {
        public KtRay2D(KtPoint2D start, KtPoint2D end) : base(start, end)
        {
        }

        public KtRay2D(KtPoint2D start, KtVector2D direction) : this(start, start + direction)
        {
        }

        internal override bool Bounds(KtPoint2D p)
        {
            if (p is null) return false;
            if (p == Start) return true;
            var directionOfPoint = (p - Start).ToKtVector2D().Direction;
            var directionOfLine = Vector.Direction;
            return directionOfPoint == directionOfLine;
        }

        public override KtLine2DBase Clone() => new KtRay2D(Start, End);

        public bool Equals(KtRay2D other) => this.TestNullBeforeEquals(other, () => Start == other.Start && SameSlope(other));

        public override bool Equals(KtLine2DBase other) => Equals(other as KtRay2D);

        public override bool ValidParametricValue(double? parameter) => parameter != null && parameter >= 0;

        public override string ToString() => $"Ray From {Start}, directed towards {Vector}";
    }
}