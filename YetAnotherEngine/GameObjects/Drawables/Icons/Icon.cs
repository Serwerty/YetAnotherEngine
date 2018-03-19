using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace YetAnotherEngine.GameObjects.Drawables.Icons
{
    class Icon : IDrawable
    {
        private readonly int _textureId;

        public Vector2 Location { get; set; }
        private readonly float _iconSize;

        public Icon(int textureId, Vector2 location, float iconSize)
        {
            _textureId = textureId;
            Location = location;
            _iconSize = iconSize;
        }

        public void Draw(Color color)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Ortho(0, 100, 0, 100, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Translate(Location.X, Location.Y, 0);
            var aspectRatio = ComputeAspectRatio();
            GL.Scale(aspectRatio * 1, 1, 1);
            GL.Rotate(0, 0, 0, 1);
            GL.PushMatrix();

            GL.BindTexture(TextureTarget.Texture2D, _textureId);

            GL.Begin(PrimitiveType.Quads);

            GL.Color4(Color.White);

            GL.TexCoord2(0, 1);
            GL.Vertex2(0, 0);
            GL.TexCoord2(1, 1);
            GL.Vertex2(_iconSize, 0);
            GL.TexCoord2(1, 0);
            GL.Vertex2(_iconSize, _iconSize);
            GL.TexCoord2(0, 0);
            GL.Vertex2(0, _iconSize);

            GL.End();
        }

        private static double ComputeAspectRatio()
        {
            var viewport = new int[4];
            GL.GetInteger(GetPName.Viewport, viewport);
            var w = viewport[2];
            var h = viewport[3];
            double aspectRatio = h / (float)w;
            return aspectRatio;
        }
    }
}
