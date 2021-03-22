using KtExtensions;

namespace EMDD.KtGeometry.KtPoints.Quadrant_Check
{
    public class Origin : QuadrantCheck
    {
        public override bool Checked(double x, double y) => x.NearZero() && y.NearZero();

        public override Quadrant2D Quadrant() => Quadrant2D.Origin;
    }
}