using System.Drawing;
using System.Windows.Forms;
using Cg1Model;

namespace Cg1WinForm
{
    public class NormalVisualizer : DefaultVisualizer
    {
        public NormalVisualizer(Pen pen) : base(pen)
        {
        }

        protected override void SimpleDraw(Graphics graphics, Shape shape)
        {
            if (shape.IsVisible)
                base.SimpleDraw(graphics, shape);
        }
    }
}