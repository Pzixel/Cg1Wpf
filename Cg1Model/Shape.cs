using System;
using System.Collections.Generic;
using System.Linq;

namespace Cg1Model
{
    public abstract class Shape
    {
        protected Shape(IEnumerable<Vector> points) : this(SelectPointsToArray(points))
        {

        }

        private Vector _normal;
        public Vector[] Points { get; protected set; }


        protected static Vector[] SelectPointsToArray(IEnumerable<Vector> points)
        {
            return points.Select(p => p.Clone()).ToArray();
        }

        protected Shape(Vector[] points)
        {
            Points = points;
            Normal = CalculateNormal(points);
        }

        public void ProcessAllPoints(Action<Vector> action)
        {
            foreach (var point in Points)
            {
                action(point);
            }
            OnShapeChanged();
        }
        
        protected virtual void OnProcessCenter(Action<Vector> action)
        {

        }

        public void ProcessAllPointsInCenter(Action<Vector> action)
        {

            var center = Center;
            foreach (var point in Points)
            {
                point.Move(-center);
                action(point);
                point.Move(center);
            }
            OnProcessCenter(action);
            OnShapeChanged();
        }

        public void RotateX(double angle)
        {
            ProcessAllPoints(point => point.RotateX(angle));
        }

        public void RotateY(double angle)
        {
            ProcessAllPoints(point => point.RotateY(angle));
        }

        public void RotateZ(double angle)
        {
            ProcessAllPoints(point => point.RotateZ(angle));
        }

        public void Translate(Vector vector)
        {
            ProcessAllPoints(point => point.Move(vector));
        }

        public void ReflectX()
        {
            ProcessAllPoints(point => point.ReflectX());
        }

        public void ReflectY()
        {
            ProcessAllPoints(point => point.ReflectY());
        }

        public void ReflectZ()
        {
            ProcessAllPoints(point => point.ReflectZ());
        }

        public void RotateCenterX(double angle)
        {
            ProcessAllPointsInCenter(vector => vector.RotateX(angle));
        }

        public void RotateCenterY(double angle)
        {
            ProcessAllPointsInCenter(vector => vector.RotateY(angle));
        }

        public void RotateCenterZ(double angle)
        {
            ProcessAllPointsInCenter(vector => vector.RotateZ(angle));
        }


        public void ReflectCenterX()
        {
            ProcessAllPointsInCenter(vector => vector.ReflectX());
        }

        public void ReflectCenterY()
        {
            ProcessAllPointsInCenter(vector => vector.ReflectY());
        }

        public void ReflectCenterZ()
        {
            ProcessAllPointsInCenter(vector => vector.ReflectZ());
        }
        

        public T Clone<T>() where T : Shape
        {
            var clone = (T) MemberwiseClone();
            clone.Points = clone.Points.SelectToArray(p => p.Clone());
            return clone;
        }

        public Vector Center
        {
            get
            {
                return Points.Aggregate((sum, v) => sum + v)/Points.Length;
            }
        }

        public Vector Normal
        {
            get
            {
                return _normal.Normalize();
            }
            set
            {
                _normal = value;
            }
        }

        public virtual bool IsVisible
        {
            get
            {
                return Normal.Z <= 0;
            }
        }

        public void Move(double dx, double dy, double dz)
        {
            var delta = new Vector(dx, dy, dz);
            ProcessAllPoints(point => point.Move(delta));
        }

        public event EventHandler ShapeChanged;

        private void OnShapeChanged()
        {
            var handler = ShapeChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        protected static Vector CalculateNormal(IList<Vector> points)
        {
            var a = CalculateNormal(points[0], points[1], points[2]).Normalize();
            var b = (-points[1]).Normalize();
            var result = (a.X*b.X + a.Y*b.Y + a.Z*b.Z);
            return result > 0 ? a : -a;
        }

        public static Vector CalculateNormal(Vector a, Vector b, Vector c)
        {
            var x = a - b;
            var y = c - b;
            return x*y;
        }

        public void RecalculateNormal()
        {
            Normal = CalculateNormal(Points);
        }
    }
}