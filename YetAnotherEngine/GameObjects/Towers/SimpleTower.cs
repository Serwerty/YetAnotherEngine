using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace YetAnotherEngine.GameObjects.Towers
{
    public class SimpleTower : TowerBase
    {

        public const int TextureOffsetX = 14;
        public const int TextureOffsetY = 0;

        public const int TowerWidth = 128;
        public const int TowerHeight = 196;

        public const int TowerCenterX = 30;
        public const int TowerCenterY = 82;

        public SimpleTower(Vector2 location, int textureId) : base(location, textureId)
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
            GL.Vertex2(base.Location.X + TowerWidth / 2f, base.Location.Y);
            GL.TexCoord2(1, 1);
            GL.Vertex2(base.Location.X + TowerWidth / 2f, base.Location.Y + TowerHeight / 2f);
            GL.TexCoord2(0, 1);
            GL.Vertex2(base.Location.X, base.Location.Y + TowerHeight / 2f);

            GL.End();
        }
    }
}
