using System.Drawing;
using Cg1Model;

namespace Cg1WinForm
{
    public interface IVisualizer
    {
        Pen Pen { get; set; }
        void Draw(Graphics graphics, Shape shape);
        T Clone<T>() where T : IVisualizer;
    }
}
