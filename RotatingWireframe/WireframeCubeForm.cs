// Simulation of a Wireframe Cube using GDI+
// Originally developed by leonelmachava <leonelmachava@gmail.com>
// http://codentronix.com
//
// Translated into C# by Scott Tunstall (scott.tunstall@ntlworld.com)
//
// Copyright (c) 2011 Leonel Machava


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RotatingWireframe
{
    public partial class WireframeCubeForm : Form
    {
        private Pen _pen = new Pen(Color.Red);
        private Timer _timer = new Timer() { Interval = 33};
        private Vector3[] _vertices;
        private int[,] _faces;
        private int _angle;


        public WireframeCubeForm()
        {
            InitializeComponent();

            _timer.Tick += _timer_Tick;
        }


        private void WireframeCubeForm_Load(object sender, EventArgs e)
        {
            InitCube();
            
            _timer.Start();
        }


        private void WireframeCubeForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.LightBlue);

            var projected = new Point3D[8];
            for (var i = 0; i < _vertices.Length; i++)
            {
                var vertex = _vertices[i];

                // if you comment out 2 Rotates then you can see the cube rotating round the relevant axes 
                var transformed = vertex.RotateX(_angle).RotateY(_angle).RotateZ(_angle);
                projected[i] = transformed.Project(this.ClientSize.Width, this.ClientSize.Height, 256, 6);
            }

            for (var j = 0; j < 6; j++)
            {
                e.Graphics.DrawLine(_pen,
                    (int) projected[_faces[j, 0]].X,
                    (int) projected[_faces[j, 0]].Y,
                    (int) projected[_faces[j, 1]].X,
                    (int) projected[_faces[j, 1]].Y);

                e.Graphics.DrawLine(_pen,
                    (int) projected[_faces[j, 1]].X,
                    (int) projected[_faces[j, 1]].Y,
                    (int) projected[_faces[j, 2]].X,
                    (int) projected[_faces[j, 2]].Y);

                e.Graphics.DrawLine(_pen,
                    (int) projected[_faces[j, 2]].X,
                    (int) projected[_faces[j, 2]].Y,
                    (int) projected[_faces[j, 3]].X,
                    (int) projected[_faces[j, 3]].Y);

                e.Graphics.DrawLine(_pen,
                    (int) projected[_faces[j, 3]].X,
                    (int) projected[_faces[j, 3]].Y,
                    (int) projected[_faces[j, 0]].X,
                    (int) projected[_faces[j, 0]].Y);
            }
        }




        private void InitCube()
        {
            _vertices = new Point3D[]
            {
                new Point3D(-1, 1, -1),
                new Point3D(1, 1, -1),
                new Point3D(1, -1, -1),
                new Point3D(-1, -1, -1),
                new Point3D(-1, 1, 1),
                new Point3D(1, 1, 1),
                new Point3D(1, -1, 1),
                new Point3D(-1, -1, 1)
            };

            // Create an array representing the 6 faces of a cube.Each face is composed by indices to the vertex array

            _faces = new int[,]
            {
                {
                    0, 1, 2, 3
                },
                {
                    1, 5, 6, 2
                },
                {
                    5, 4, 7, 6
                },
                {
                    4, 0, 3, 7
                },
                {
                    0, 4, 5, 1
                },
                {
                    3, 2, 6, 7
                }
            };
        }



        private void _timer_Tick(object sender, EventArgs e)
        {
            this.Invalidate();

            _angle++;
        }


    }
}
