using KtExtensions;

namespace EMDD.KtGeometry.KtPoints.Quadrant_Check
{
    public class AtXAxisp : QuadrantCheck
    {
        public override bool Checked(double x, double y) => x > 0 && y.NearZero(5);

        public override Quadrant2D Quadrant() => Quadrant2D.Posx;
    }
}