using EMDD.KtGeometry.KtPolygons;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace TestForm
{
    public partial class PolygonTest : Form
    {
        public PolygonTest()
        {
            InitializeComponent();
            rubberBandRegion = new RubberBandRegion(pbTest, margin, PbTest_Paint);
            pbTest.Paint += ChangeOrientation;
        }

        private void ChangeOrientation(object sender, PaintEventArgs e)
        {
            var gr = e.Graphics;
            gr.TranslateTransform(margin, -margin + pbTest.Height);
            gr.ScaleTransform(1, -1);
        }

        private readonly float margin = 20;

        private void PbTest_Paint(object sender, PaintEventArgs e)
        {
            var gr = e.Graphics;
            var transparency = resultingPolygon != null ? 70.0 : 50.0;
            if (resultingPolygon != null)
            {
                gr.DrawPolygon(resultingPolygon, Color.FromArgb(200, 106, 228, 62));
            }
            else
            {
                gr.DrawPolygon(_polygon1, Color.FromArgb((int)(255 * (1.0 - (transparency / 100))), 226, 152, 16));
                gr.DrawPolygon(_polygon2, Color.FromArgb((int)(255 * (1.0 - (transparency / 100))), 226, 0, 16));
            }
        }

        private readonly KtPolygon2D _polygon1 = new();
        private readonly KtPolygon2D _polygon2 = new();
        private KtPolygon2D resultingPolygon;

        private readonly RubberBandRegion rubberBandRegion;
        public string Report = "";

        private void BtnAddPolygon1_Click(object sender, EventArgs e)
        {
            rubberBandRegion.StartDrawing(_polygon1, s => Report += s);
            resultingPolygon = null;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Report = "var actualPolygon = new KtPolygon2D(); \n" + Report;
            Report += rubberBandRegion.polygonInUse.ConvertedAPolygonToAnInstance("expectedPolygon");
            textBox1.Text = Report;
        }

        private void BtnAddPolygon2_Click(object sender, EventArgs e)
        {
            rubberBandRegion.StartDrawing(_polygon2, s => Report += s);
            resultingPolygon = null;
        }

        private void ExecuteCombination_Click(object sender, EventArgs e)
        {
            switch (cbTypOfCombination.Text)
            {
                case "Union":
                    resultingPolygon = _polygon1 | _polygon2;
                    pbTest.Invalidate();
                    break;
                case "Difference":
                    resultingPolygon = _polygon1 - _polygon2;
                    pbTest.Invalidate();
                    break;

                case "Intersect":
                    resultingPolygon = _polygon1 & _polygon2;
                    pbTest.Invalidate();
                    break;
                case "Xor":
                    MessageBox.Show("Xor");
                    break;
                default:
                    break;
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            var report = _polygon1.ConvertedAPolygonToAnInstance("polygon1") +"\n";
            report += _polygon2.ConvertedAPolygonToAnInstance("polygon2") + "\n";
            report += resultingPolygon.ConvertedAPolygonToAnInstance("expectedPolygon");
            textBox1.Text = report;
        }
    }
}
