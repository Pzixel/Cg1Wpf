using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Cg1Model;
using Cg1WinForm.Properties;
using Helpers;

namespace Cg1WinForm
{
    abstract class AbstrClass
    {
        abstract public void SomeMethod();
    }

   internal class GradientFillVisualizer : DefaultVisualizer
    {
        public GradientFillVisualizer(Pen pen) : base(pen)
        {

        }

        private static readonly float[,] Floats;
        static GradientFillVisualizer()
        {
            Floats = BitmapHelper.GetBrightness(Resources.Shademap);
        }

        protected override void SimpleDraw(Graphics graphics, Shape shape)
        {
            throw new NotSupportedException();
        }

        protected override void ComplexDraw(Graphics graphics, ComplexShape shape)
        {
            if (shape != _lastShape)
            {
                _lastShape = shape;
                InitializeDictionary(shape);
            }
            foreach (var s in shape.Shapes)
            {
                if (!s.IsVisible)
                    continue;
                var colors = new Color[s.Points.Length];
                for (int i = 0; i < colors.Length; i++)
                {
                    var normals = _dictionary[s.Points[i]]
                        .Where(sh => sh.IsVisible)
                        .Select(k => k.Normal).ToArray();
                    var aggregatedNormal = normals.Aggregate((sh, sum) => sh + sum)/normals.Length;
                    double x = aggregatedNormal.X*128 + 127;
                    double y = aggregatedNormal.Y*128 + 127;
                    float brightness = Floats[(int) x, (int) y];
                    colors[i] = Color.FromArgb((int) (Pen.Color.R*brightness),
                                               (int) (Pen.Color.G*brightness),
                                               (int) (Pen.Color.B*brightness));
                }

                PointF[] ptsF = s.Points.CastToPointF();
                var pBrush = new PathGradientBrush(ptsF);
                int r = 0, g = 0, b = 0;
                foreach (var color in colors)
                {
                    r += color.R;
                    g += color.G;
                    b += color.B;
                }
                pBrush.SurroundColors = colors;
                pBrush.CenterColor = Color.FromArgb(r/colors.Length, g/colors.Length, b/colors.Length);
                graphics.FillPolygon(pBrush, ptsF);
            }
        }

        private void InitializeDictionary(ComplexShape shape)
        {
            _dictionary = new Dictionary<Vector, List<Shape>>(shape.Points.Length, new VectorComparer());
            foreach (var sh in shape.Shapes)
            {
                foreach (var vector in sh.Points)
                {
                    if (_dictionary.ContainsKey(vector))
                        _dictionary[vector].Add(sh);
                    else
                        _dictionary.Add(vector, new List<Shape> {sh});
                }
            }
        }

        private ComplexShape _lastShape;

        private Dictionary<Vector, List<Shape>> _dictionary;
    }
}