using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace YetAnotherEngine
{
    public class Brush
    {
        public Vector2 Location { get; private set; }
        public Vector2 Size { get; private set; }
        //private bool solid = true;

        public Brush(Vector2 location, Vector2 size)
        {
            Location = location;
            Size = size;
        }
        
        public void Draw()
        {
            GL.Color4(Color.Red);
           // GL.TexCoord2(0, 0);
            GL.Vertex2(Location);
           // GL.TexCoord2(1, 0);
            GL.Vertex2(Location.X + Size.X, Location.Y);
          //  GL.TexCoord2(1, 1);
            GL.Vertex2(Location.X + Size.X, Location.Y + Size.Y);
          //  GL.TexCoord2(0, 1);
            GL.Vertex2(Location.X, Location.Y + Size.Y);
        }
   }
}
