using System.Drawing;
using System.Linq;
using System.Reflection;
using Cg1Model;

namespace Cg1WinForm
{
    public class DefaultVisualizer : IVisualizer
    {
        public Pen Pen { get; set; }
        
        public DefaultVisualizer(Pen pen)
        {
            Pen = pen;
        }

       
        public void Draw(Graphics graphics, Shape shape)
        {
            var complexShape = shape as ComplexShape;
            if (complexShape == null)
            {
                SimpleDraw(graphics, shape);
            }
            else
            {
                ComplexDraw(graphics, complexShape);
            }
        }

        protected virtual void ComplexDraw(Graphics graphics, ComplexShape shape)
        {
            foreach (var s in shape.Shapes)
            {
                Draw(graphics, s);
            }
        }

        protected virtual void SimpleDraw(Graphics graphics, Shape shape)
        {
            var points = shape.Points;
            for (int i = 0; i < points.Length; i++)
            {
                Vector a = points[i], b = points[(i + 1) % points.Length];
                graphics.DrawLine(Pen, (float)a.X, (float)a.Y, (float)b.X, (float)b.Y);
            }
        }

        public T Clone<T>() where T : IVisualizer
        {
            ConstructorInfo constructor = typeof (T).GetConstructors().Single();
           return (T) constructor.Invoke(new object[] {Pen});
        }
    }
}