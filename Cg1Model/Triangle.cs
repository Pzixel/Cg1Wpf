using System;

namespace Cg1Model
{
    public class Triangle : Shape
    {

        public Triangle(params Vector[] points) : base(points)
        {
            if (points.Length != 3)
                throw new ArgumentException();
        }
    }
}
