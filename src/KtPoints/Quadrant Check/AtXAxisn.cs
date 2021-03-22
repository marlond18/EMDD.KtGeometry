using KtExtensions;

namespace EMDD.KtGeometry.KtPoints.Quadrant_Check
{
    public class AtXAxisn : QuadrantCheck
    {
        public override bool Checked(double x, double y) => x < 0 && y.NearZero(5);

        public override Quadrant2D Quadrant() => Quadrant2D.Negx;
    }
}