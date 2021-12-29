using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotatingWireframe
{

    public class Point3D
    {
        public Point3D(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }

        public Point3D RotateX(int angle)
        {
            var rad = angle * Math.PI / 180;
            var cosa = Math.Cos(rad);
            var sina = Math.Sin(rad);
            var yn = (this.Y * cosa) - (this.Z * sina);
            var zn = (this.Y * sina) + (this.Z * cosa);
            return new Point3D(this.X, yn, zn);
        }

        public Point3D RotateY(int angle)
        {
            var rad = angle * Math.PI / 180;
            var cosa = Math.Cos(rad);
            var sina = Math.Sin(rad);
            var Zn = (this.Z * cosa) - (this.X * sina);
            var Xn = (this.Z * sina) + (this.X * cosa);
            return new Point3D(Xn, this.Y, Zn);
        }

        public Point3D RotateZ(int angle)
        {
            var rad = angle * Math.PI / 180;
            var cosa = Math.Cos(rad);
            var sina = Math.Sin(rad);
            var Xn = (this.X * cosa) - (this.Y * sina);
            var Yn = (this.X * sina) + (this.Y * cosa);
            return new Point3D(Xn, Yn, this.Z);
        }

        public Point3D Project(int viewWidth, int viewHeight, int fov, int viewDistance)
        {
            var factor = fov / (viewDistance + this.Z);
            var Xn = this.X * factor + viewWidth / 2;
            var Yn = this.Y * factor + viewHeight / 2;
            return new Point3D(Xn, Yn, this.Z);
        }
    }

}
