using System.Collections.Generic;

namespace EMDD.KtGeometry.KtPoints.Quadrant_Check
{
    public abstract class QuadrantCheck
    {
        public abstract bool Checked(double x, double y);

        public abstract Quadrant2D Quadrant();

        public static List<QuadrantCheck> List() =>
            new()
            { new Origin(), new Quad1(), new Quad2(), new Quad3(), new Quad4(), new AtXAxisp(), new AtXAxisn(), new AtYAxisp(), new AtYAxisn() };
    }
}