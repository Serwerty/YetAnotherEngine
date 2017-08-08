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
    class Gun
    {
        private Vector2 Location;
        private Vector2 Size;
        private Texture Gun1 = new Texture(new Bitmap(@"textures\Guns\weapon1.png"));
        public Gun(Vector2 location, Vector2 size)
        {
            Location = location;
            Size = size;
        }

        public Gun(float locationx, float locationy,float sizex , float sizey)
        {
            Location.X = locationx;
            Location.Y = locationy;
            Size.X = sizex;
            Size.Y = sizey;
        }

        public void Draw(Vector2 location,int direction)
        {
            Location = location;

            Gun1.Bind();
            GL.Begin(BeginMode.Quads);
            GL.Color4(Color.White);

            //draw gun in same direction as player
            if (direction == 1)
            {
                //-> direction
                GL.TexCoord2(0, 0);
                GL.Vertex2(Location);
                GL.TexCoord2(1, 0);
                GL.Vertex2(Location.X + Size.X, Location.Y);
                GL.TexCoord2(1, 1);
                GL.Vertex2(Location.X + Size.X, Location.Y + Size.Y);
                GL.TexCoord2(0, 1);
                GL.Vertex2(Location.X, Location.Y + Size.Y);
            }
            else
            {
                //<- direction
                GL.TexCoord2(1, 0);
                GL.Vertex2(Location);
                GL.TexCoord2(0, 0);
                GL.Vertex2(Location.X + Size.X, Location.Y);
                GL.TexCoord2(0, 1);
                GL.Vertex2(Location.X + Size.X, Location.Y + Size.Y);
                GL.TexCoord2(1, 1);
                GL.Vertex2(Location.X, Location.Y + Size.Y);
            }
            GL.End();
        }
    }
}
