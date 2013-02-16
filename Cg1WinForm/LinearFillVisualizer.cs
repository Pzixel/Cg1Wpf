using System.Drawing;
using Cg1Model;
using Cg1WinForm.Properties;
using Helpers;

namespace Cg1WinForm
{
    internal class LinearFillVisualizer : DefaultVisualizer
    {
        public LinearFillVisualizer(Pen pen) : base(pen)
        {
        }

        private static readonly float[,] Floats;
        static LinearFillVisualizer()
        {
            Floats = Resources.Shademap.GetBrightness();
        }

        protected override void SimpleDraw(Graphics graphics, Shape shape)
        {
            if (!shape.IsVisible)
                return;
            double x = shape.Normal.X*128 + 127;
            double y = shape.Normal.Y*128 + 127;
            var brightness = Floats[(int) x, (int) y]; 
            var color = Color.FromArgb(255, (int) (Pen.Color.R*brightness), (int) (Pen.Color.G*brightness), (int) (Pen.Color.B*brightness));
            graphics.FillPolygon(new SolidBrush(color), shape.Points.CastToPointF());
        }
    }
}