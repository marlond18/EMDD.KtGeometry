using KtExtensions;
using EMDD.KtGeometry.KtPoints;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EMDD.KtGeometry.KtPolygons.Regions;

namespace TestForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private readonly float margin = 20;

        private void PbTest_Paint(object sender, PaintEventArgs e)
        {
            var gr = e.Graphics;
            gr.TranslateTransform(margin, -margin + pbTest.Height);
            gr.ScaleTransform(1, -1);
            DrawRegion(gr);
        }

        public void DrawRegion(Graphics gr)
        {
            var color = Color.FromArgb(70, 226, 152, 16);
            if (_polygon1.Count > 3) gr.DrawRegion(_polygon1, color);
            if (_polygon2.Count > 3) gr.DrawRegion(_polygon2, color);
            if (_polygonInUse2.Count > 3) gr.DrawRegion(_polygonInUse2, color);
            foreach (var region in regions)
            {
                gr.DrawRegion(region, color);
            }

            if (intersections != null)
            {
                for (int i = 0; i < intersections.Length; i++)
                {
                    var intersection = intersections[i];
                    var intersectionColor = Color.FromArgb(255, 100 + (20 * i).CycleWithin(0, 255), 0, 255);
                    gr.DrawRegion(intersection, intersectionColor);
                }
            }
        }

        private List<KtPoint2D> _polygon1 = new();
        private KtRegion[] intersections;
        private List<KtPoint2D> _polygon2 = new();
        private List<KtPoint2D> _polygonInUse;
        private List<KtPoint2D> _polygonInUse2;
        private readonly List<KtRegion> regions = new();

        private void BtnAddPolygon1_Click(object sender, EventArgs e)
        {
            _polygon1 = new List<KtPoint2D>();
            _polygonInUse = _polygon1;
            pbTest.MouseClick += CreatePolygon;
        }

        public KtPoint2D ConvertMouseClickToSetup(int x, int y) =>
            new((double)x - margin, pbTest.Height - (double)y - margin);

        private void RubberBandRegion(object sender, MouseEventArgs e)
        {
            var tempPoint = ConvertMouseClickToSetup(e.X, e.Y);
            if (_polygonInUse == null || _polygonInUse.Count < 1) return;
            _polygonInUse[^1] = tempPoint;
            pbTest.Invalidate();
        }

        private bool _blnDrawing;

        private void CreatePolygon(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (!_blnDrawing)
                    {
                        _polygonInUse.Clear();
                        pbTest.MouseMove += RubberBandRegion;
                        _blnDrawing = true;
                        _polygonInUse.Add(ConvertMouseClickToSetup(e.X, e.Y));
                    }
                    _polygonInUse.Add(ConvertMouseClickToSetup(e.X + 1, e.Y + 1));
                    break;

                case MouseButtons.Right:
                    _blnDrawing = false;
                    if (_polygonInUse.Count > 0)
                    {
                        _polygonInUse.RemoveAt(_polygonInUse.Count - 1);
                    }
                    pbTest.MouseMove -= RubberBandRegion;
                    pbTest.MouseClick -= CreatePolygon;
                    break;
            }
            pbTest.Invalidate();
        }

        private void BtnAddPolygon2_Click(object sender, EventArgs e)
        {
            _polygon2 = new List<KtPoint2D>();
            _polygonInUse = _polygon2;
            pbTest.MouseClick += CreatePolygon;
        }

        private static void Form1_Load_1(object sender, EventArgs e)
        {
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (_polygon1.Count < 3 || _polygon2.Count < 3) return;
            //intersections = (new KtRegion(_polygon1).Overlay(_polygon2)).ToArray();
            pbTest.Invalidate();
            var str = $"{ConvertRegToInputCode(_polygon1, "Polygon1")} {ConvertRegToInputCode(_polygon2, "Polygon2")} {intersections.Aggregate("", (finalstring, temp) => finalstring + ConvertRegToInputCode(temp, "Intersection"))}";
            textBox1.Text = str;
        }

        public static string ConvertRegToInputCode(KtRegion reg, string name)
        {
            var builder = new System.Text.StringBuilder();
            builder.Append("KtRegion ").Append(name).Append("= new List<KtPoint2D>{");
            var regC = reg.ToList();
            for (int i = 0; i < regC.Count - 1; i++)
            {
                var corner = regC[i];
                var str = $"({corner.X},{corner.Y})";
                builder.Append(str).Append(',');
            }
            var corner2 = regC[^1];
            var str2 = $"({corner2.X},{corner2.Y})";
            builder.Append(str2).Append('}').Append(";\n");
            return builder.ToString();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            _polygon1 = new List<KtPoint2D>();
            _polygon2 = new List<KtPoint2D>();
            _polygonInUse = null;
            intersections = null;
            _blnDrawing = false;
            pbTest.MouseMove -= RubberBandRegion;
            pbTest.MouseClick -= CreatePolygon;
            pbTest.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _polygonInUse2 = new List<KtPoint2D>();
        }

        private void PbTest_Click(object sender, EventArgs e)
        {
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            _polygonInUse2 = new List<KtPoint2D>();
            _polygonInUse = _polygonInUse2;
            pbTest.MouseClick += CreatePolygon;
            pbTest.MouseClick += PbTest_MouseClick;
        }
        private int countRegion = 0;

        private void PbTest_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                countRegion++;
                regions.Add(_polygonInUse2);
                pbTest.MouseClick -= PbTest_MouseClick;
                textBox1.Text += "\n";
                textBox1.Text += $"var region_{countRegion}= new KtRegion(new List<KtPoint2D>{{{_polygonInUse2.Aggregate("",(a,b)=>a+ $"({b.X}, {b.Y}),")}}});";
            }
        }
    }
}