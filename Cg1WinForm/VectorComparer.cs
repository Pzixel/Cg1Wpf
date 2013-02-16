using System.Collections.Generic;
using Cg1Model;

namespace Cg1WinForm
{
    public class VectorComparer : IEqualityComparer<Vector>
    {
        public bool Equals(Vector x, Vector y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(Vector obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}