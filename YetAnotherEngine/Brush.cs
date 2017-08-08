using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK;

namespace YetAnotherEngine
{
    class Brush
    {
        private Vector2 Location;
        private Vector2 Size;
        private bool solid = true;

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
            GL.Vertex2(Location.X+Size.X,Location.Y);
          //  GL.TexCoord2(1, 1);
            GL.Vertex2(Location.X+Size.X,Location.Y+Size.Y);
          //  GL.TexCoord2(0, 1);
            GL.Vertex2(Location.X,Location.Y+Size.Y);
        }
        public Vector2 GetLocation()
        {
            return Location;
        }
        public Vector2 GetSize()
        {
            return Size;
        }
   }
}
