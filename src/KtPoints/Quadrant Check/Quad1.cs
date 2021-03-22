namespace EMDD.KtGeometry.KtPoints.Quadrant_Check
{
    public class Quad1 : QuadrantCheck
    {
        public override bool Checked(double x, double y) => x > 0 && y > 0;

        public override Quadrant2D Quadrant() => Quadrant2D.I;
    }
}