using System.Threading.Tasks;
using _3DWireframeV2.ThreeDMath;


namespace _3DWireframeV2
{
    public partial class WireFrameCubeForm : Form
    {
        private Pen _pen = new Pen(Color.Red);
        private System.Windows.Forms.Timer _timer = new () { Interval = 33 };
        private Vector3[] _vertices;
        private int[,] _faces;
        private int _angle;
        private  Vector3 _camTarget;
        private Vector3 _camPosition;
        private Matrix _projectionMatrix;
        private Matrix _viewMatrix;


        public WireFrameCubeForm()
        {
            InitializeComponent();

            _camTarget = new Vector3(0f, 0f, 0f);
            _camPosition = new Vector3(0f, 0f, -20);

            _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45f), (float) this.Width / this.Height, 1f, 1000f);
            
            _viewMatrix = Matrix.CreateLookAt(_camPosition, _camTarget,
                new Vector3(0f, 1f, 0f));// Y up
            
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

            var projected = new Vector2[8];
            for (var i = 0; i < _vertices.Length; i++)
            {
                var vertex = _vertices[i];
                var matrix = //_worldMatrix *
                             _viewMatrix *
                             _projectionMatrix;

                var transformed = Vector4.Transform(vertex, matrix);

                // normalize if w is different than 1 (convert from homogeneous to Cartesian coordinates)
                if (transformed.W != 1)
                {
                    transformed.X /= transformed.W;
                    transformed.Y /= transformed.W;
                }

                projected[i] = new Vector2(transformed.X, transformed.Y);
                projected[i] *= new Vector2(1000, 1000 * Height / Width);
                projected[i] += new Vector2(Width / 2, Height / 2);
            }

            for (var j = 0; j < 6; j++)
            {
                e.Graphics.DrawLine(_pen,
                    (int)projected[_faces[j, 0]].X,
                    (int)projected[_faces[j, 0]].Y,
                    (int)projected[_faces[j, 1]].X,
                    (int)projected[_faces[j, 1]].Y);

                e.Graphics.DrawLine(_pen,
                    (int)projected[_faces[j, 1]].X,
                    (int)projected[_faces[j, 1]].Y,
                    (int)projected[_faces[j, 2]].X,
                    (int)projected[_faces[j, 2]].Y);

                e.Graphics.DrawLine(_pen,
                    (int)projected[_faces[j, 2]].X,
                    (int)projected[_faces[j, 2]].Y,
                    (int)projected[_faces[j, 3]].X,
                    (int)projected[_faces[j, 3]].Y);

                e.Graphics.DrawLine(_pen,
                    (int)projected[_faces[j, 3]].X,
                    (int)projected[_faces[j, 3]].Y,
                    (int)projected[_faces[j, 0]].X,
                    (int)projected[_faces[j, 0]].Y);
            }
        }




        private void InitCube()
        {
            _vertices = new Vector3[]
            {
                new Vector3(-1, 1, -1),
                new Vector3(1, 1, -1),
                new Vector3(1, -1, -1),
                new Vector3(-1, -1, -1),
                new Vector3(-1, 1, 1),
                new Vector3(1, 1, 1),
                new Vector3(1, -1, 1),
                new Vector3(-1, -1, 1)
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

            Matrix rotationMatrix = Matrix.CreateRotationX(MathHelper.ToRadians(1f));
            _camPosition = Vector3.Transform(_camPosition, rotationMatrix);

            _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45f), (float)this.Width / this.Height, 1f, 1000f);

            _viewMatrix = Matrix.CreateLookAt(_camPosition, _camTarget,
                Vector3.Up);
        }

    }
}