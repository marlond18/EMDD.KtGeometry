using EMDD.KtGeometry.KtPoints;
using EMDD.KtGeometry.KtPolygons;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using EMDD.KtGeometry.KtPolygons.Regions;

namespace TestForm
{
    public class RubberBandRegion
    {
        private Action<string> whatToDoWhenStartedDrawing;
        private readonly PaintEventHandler originalDrawing;
        private readonly float margin;
        private readonly PictureBox pb;
        internal KtPolygon2D polygonInUse;
        private bool _blnDrawing;

        public RubberBandRegion(PictureBox pb, float margin, PaintEventHandler originalDrawing)
        {
            Region = new List<KtPoint2D>();
            this.pb = pb;
            this.margin = margin;
            this.originalDrawing = originalDrawing;
        }

        public void StartDrawing(KtPolygon2D polygon, Action<string> whatToDoWhenStartedDrawing)
        {
            if (_blnDrawing) return;
            polygonInUse = polygon;
            pb.Paint -= originalDrawing;
            pb.Paint += Pb_Paint;
            pb.MouseClick += CreateRegion;
            this.whatToDoWhenStartedDrawing = whatToDoWhenStartedDrawing;
        }

        private void Pb_Paint(object sender, PaintEventArgs e)
        {
            var gr = e.Graphics;
            if (Region?.Count > 2) gr.DrawRegion(Region, Color.Aqua);
            gr.DrawPolygon(polygonInUse, Color.FromArgb(10, 150, 152, 16));
        }

        private void CreateRegion(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!_blnDrawing)
                {
                    pb.MouseMove += RubberBandLastCorner;
                    _blnDrawing = true;
                    AddCorner(e.X, e.Y);
                }
                AddCorner(e.X + 1, e.Y + 1);
            }
            else if (e.Button == MouseButtons.Right)
            {
                _blnDrawing = false;
                if (Region.Count > 0) Region.RemoveAt(Region.Count - 1);
                if (Region.Count > 2) polygonInUse.Add(Region);
                whatToDoWhenStartedDrawing?.Invoke(((KtRegion)Region).AddToPolygonText("actualPolygon"));
                Region = new List<KtPoint2D>();
                pb.MouseMove -= RubberBandLastCorner;
                pb.MouseClick -= CreateRegion;
                pb.Paint += originalDrawing;
                pb.Paint -= Pb_Paint;
            }
            pb.Invalidate();
        }

        private void AddCorner(int x, int y)
        {
            Region.Add(ConvertMouseClickToSetup(x, y));
        }

        public List<KtPoint2D> Region { get; set; }

        private void RubberBandLastCorner(object sender, MouseEventArgs e)
        {
            if (Region == null || Region.Count == 0) return;
            Region[^1] = ConvertMouseClickToSetup(e.X, e.Y);
            pb.Invalidate();
            _ = new[] { 1, 2, 3 };
        }

        private KtPoint2D ConvertMouseClickToSetup(int x, int y) => new((double)x - margin, pb.Height - (double)y - margin);
    }
}
