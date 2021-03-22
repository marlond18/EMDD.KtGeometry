using KtExtensions;

namespace EMDD.KtGeometry.KtPoints.Quadrant_Check
{
    public class AtYAxisp : QuadrantCheck
    {
        public override bool Checked(double x, double y) => y > 0 && x.NearZero(5);

        public override Quadrant2D Quadrant() => Quadrant2D.Posy;
    }
}