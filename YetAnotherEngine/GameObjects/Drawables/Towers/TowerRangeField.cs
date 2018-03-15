using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace YetAnotherEngine.GameObjects.Drawables.Towers
{
    class TowerRangeField : IDrawable
    {
        public Vector2 Location { get; set; }

        public int Range { get; set; }

        protected int TextureId { get; set; }

        public TowerRangeField(Vector2 location, int textureId, int range)
        {
            Location = location;
            Range = range;
            TextureId = textureId;
        }

        public void Draw(Color color)
        {
            GL.BindTexture(TextureTarget.Texture2D, TextureId);

            GL.Begin(PrimitiveType.Quads);
            GL.Color4(color);

            GL.TexCoord2(0, 0);
            GL.Vertex2(Location);
            GL.TexCoord2(1, 0);
            GL.Vertex2(Location.X + Range * 2f, Location.Y);
            GL.TexCoord2(1, 1);
            GL.Vertex2(Location.X + Range * 2f, Location.Y + Range);
            GL.TexCoord2(0, 1);
            GL.Vertex2(Location.X, Location.Y + Range);

            GL.End();
        }
    }
}