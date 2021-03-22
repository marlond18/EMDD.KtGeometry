using KtExtensions;
using EMDD.KtGeometry.KtPolygons;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using EMDD.KtGeometry.KtPolygons.Regions;

namespace TestForm
{
    public static class GraphicsExtensionForGeometry
    {
        public static void DrawHoledRegion(this Graphics gr, KtSolidRegion reg, Color color)
        {
            if (reg.Count < 3) return;
            using var gp = new GraphicsPath();
            gp.AddPolygon(reg.ToArrayOfPoints());
            foreach (var hole in reg.Holes)
            {
                gp.AddPolygon(hole.ToArrayOfPoints());
            }
            using var pen = new Pen(color);
            gr.FillPath(pen.Brush, gp);
            using var pen2 = new Pen(Color.Red);
            pen2.Width = 2;
            gr.DrawPath(pen2, gp);
        }

        public static Point[] ToArrayOfPoints(this KtRegion region)
        {
            return region.Corners.Select(corner => new Point((int)corner.X, (int)corner.Y)).ToArray();
        }

        public static void DrawRegion(this Graphics gr, KtRegion reg, Color color)
        {
            if (reg.Count < 3) return;
            var regconvert = reg.Corners.Select(corner => new Point((int)corner.X, (int)corner.Y)).ToArray();
            using (var pen = new Pen(!reg.IsClockwise ? color : Color.FromArgb(color.ToArgb() ^ 0xffffff)))
            {
                gr.FillPolygon(pen.Brush, regconvert);
                gr.DrawPolygon(Pens.WhiteSmoke, regconvert);
            }
            if (reg is KtSolidRegion solid)
            {
                foreach (var hole in solid.Holes)
                {
                    gr.DrawRegion(hole, color);
                }
            }
        }

        public static void DrawPolygon(this Graphics gr, KtPolygon2D polygon, Color color)
        {
            foreach (var region1 in polygon)
            {
                gr.DrawHoledRegion(region1, color);
            }
        }

        public static string ConvertedAPolygonToAnInstance(this KtPolygon2D polygon, string polygonName)
        {
            var builder = new StringBuilder();
            builder.Append("var ").Append(polygonName).Append(" = new KtPolygon2D(); \n");
            foreach (var solid in polygon)
            {
                builder.Append(AddToPolygonText(solid, polygonName));
                foreach (var hole in solid.Holes)
                {
                    builder.Append(AddToPolygonText(hole, polygonName));
                }
            }
            return builder.ToString();
        }

        public static string AddToPolygonText(this KtRegion region, string polygonName)
        {
            var builder = new StringBuilder();
            builder.Append(polygonName).Append(".Add(");
            builder.Append(region.CreateARegionInstance());
            builder.Append("); \n");
            return builder.ToString();
        }

        public static string CreateARegionInstance(this KtRegion region)
        {
            var builder = new StringBuilder();
            builder.Append(region is KtSolidRegion ? "new KtSolidRegion" : "new KtHollowRegion");
            if (region == null || region.Count == 0)
            {
                builder.Append("()");
            }
            else
            {
                builder.Append("( new KtPoint2D[]{")
                .Append('(').Append(region.First().X).Append(',').Append(region.First().Y).Append(')');
                foreach (var corner in region.Skip(1))
                {
                    builder.Append(",(").Append(corner.X).Append(',').Append(corner.Y).Append(')');
                }
                builder.Append("})");
            }
            return builder.ToString();
        }
    }
}