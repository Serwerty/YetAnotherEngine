using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace YetAnotherEngine.GameObjects.Units
{
    class BasicUnit : UnitBase
    {
        public const int UnitWidth = 64;
        public const int UnitHeight = 64;

        public BasicUnit(Vector2 location, int textureId) : base(location, textureId)
        {
        }

        public override void Draw(Color color)
        {
            GL.BindTexture(TextureTarget.Texture2D, TextureId);

            GL.Begin(PrimitiveType.Quads);
            GL.Color4(color);

            GL.TexCoord2(0, 0);
            GL.Vertex2(base.Location);
            GL.TexCoord2(1, 0);
            GL.Vertex2(base.Location.X + UnitWidth / 2f, base.Location.Y);
            GL.TexCoord2(1, 1);
            GL.Vertex2(base.Location.X + UnitWidth / 2f, base.Location.Y + UnitHeight / 2f);
            GL.TexCoord2(0, 1);
            GL.Vertex2(base.Location.X, base.Location.Y + UnitHeight / 2f);

            GL.End();
        }

        protected override void Move(Vector2 endPoint)
        {
            if (Location == endPoint) ();
        }
    }
}
