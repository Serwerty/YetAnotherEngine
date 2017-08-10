using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace YetAnotherEngine
{
    public class Gun
    {
        private Vector2 _location;
        private Vector2 _size;
        private readonly Texture _gunTexture = new Texture(new Bitmap(@"textures\Guns\weapon1.png"));
        public Gun(Vector2 location, Vector2 size)
        {
            _location = location;
            _size = size;
        }

        public Gun(float locationx, float locationy, float sizex, float sizey)
        {
            _location.X = locationx;
            _location.Y = locationy;
            _size.X = sizex;
            _size.Y = sizey;
        }

        public void Draw(Vector2 location, int direction)
        {
            _location = location;

            _gunTexture.Bind();
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(Color.White);

            //draw gun in same direction as player
            if (direction == 1)
            {
                //-> direction
                GL.TexCoord2(0, 0);
                GL.Vertex2(_location);
                GL.TexCoord2(1, 0);
                GL.Vertex2(_location.X + _size.X, _location.Y);
                GL.TexCoord2(1, 1);
                GL.Vertex2(_location.X + _size.X, _location.Y + _size.Y);
                GL.TexCoord2(0, 1);
                GL.Vertex2(_location.X, _location.Y + _size.Y);
            }
            else
            {
                //<- direction
                GL.TexCoord2(1, 0);
                GL.Vertex2(_location);
                GL.TexCoord2(0, 0);
                GL.Vertex2(_location.X + _size.X, _location.Y);
                GL.TexCoord2(0, 1);
                GL.Vertex2(_location.X + _size.X, _location.Y + _size.Y);
                GL.TexCoord2(1, 1);
                GL.Vertex2(_location.X, _location.Y + _size.Y);
            }
            GL.End();
        }
    }
}
