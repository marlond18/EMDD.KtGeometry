namespace EMDD.KtGeometry.KtPoints.Quadrant_Check
{
    public class Quad2 : QuadrantCheck
    {
        public override bool Checked(double x, double y) => x < 0 && y > 0;

        public override Quadrant2D Quadrant() => Quadrant2D.Ii;
    }
}