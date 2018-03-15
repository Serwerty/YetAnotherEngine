using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using YetAnotherEngine.Constants;

namespace YetAnotherEngine.GameObjects.Drawables.Units
{
    class HpBar : IDrawable
    {
        public Vector2 Location
        {
            get => _location;
            set => _location = new Vector2(value.X + 20, value.Y - 10);
        }

        private const int Width = 24;
        private const int Height = 4;

        private readonly int TextureId;

        public double HpPercent = 1;
        private Vector2 _location;

        public HpBar(Vector2 location, int textureId)
        {
            Location = location;
            TextureId = textureId;
        }

        public void Draw(Color color)
        {
            if (HpPercent != 1)
            {
                GL.BindTexture(TextureTarget.Texture2D, TextureId);

                GL.Begin(PrimitiveType.Quads);
                GL.Color4(WorldConstants.RedColor);

                GL.TexCoord2(0, 0);
                GL.Vertex2(Location.X, Location.Y);
                GL.TexCoord2(1, 0);
                GL.Vertex2(Location.X + Width, Location.Y);
                GL.TexCoord2(1, 1);
                GL.Vertex2(Location.X + Width, Location.Y + Height);
                GL.TexCoord2(0, 1);
                GL.Vertex2(Location.X, Location.Y + Height);

                GL.Color4(WorldConstants.GreenColor);

                GL.TexCoord2(0, 0);
                GL.Vertex2(Location.X, Location.Y);
                GL.TexCoord2(1, 0);
                GL.Vertex2(Location.X + Width * HpPercent, Location.Y);
                GL.TexCoord2(1, 1);
                GL.Vertex2(Location.X + Width * HpPercent, Location.Y + Height);
                GL.TexCoord2(0, 1);
                GL.Vertex2(Location.X, Location.Y + Height);

                GL.End();
            }
        }
    }
}