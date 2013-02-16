using System;

namespace Cg1Model  
{
    public class Simplex : Shape
    {
        public Simplex(params Vector[] points)
            : base(points)
        {
            if (points.Length != 4)
                throw new ArgumentException();
        }
    }
}
