using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace YetAnotherEngine.GameObjects.Drawables.Units
{
    public class SimpleUnit : UnitBase
    {
        public const int UnitWidth = 64;
        public const int UnitHeight = 64;

        private const int Speed = 2;

        public SimpleUnit(Vector2 location, int textureId) : base(location, textureId)
        {
        }

        public override void Draw(Color color)
        {
            GL.BindTexture(TextureTarget.Texture2D, TextureId);

            GL.Begin(PrimitiveType.Quads);
            GL.Color4(color);

            GL.TexCoord2(0, 0);
            GL.Vertex2(Location.X + 16, Location.Y - 8);
            GL.TexCoord2(1, 0);
            GL.Vertex2(Location.X + UnitWidth / 2f + 16, Location.Y - 8);
            GL.TexCoord2(1, 1);
            GL.Vertex2(Location.X + UnitWidth / 2f + 16, Location.Y + UnitHeight / 2f - 8);
            GL.TexCoord2(0, 1);
            GL.Vertex2(Location.X + 16, Location.Y + UnitHeight / 2f - 8);

            GL.End();
        }

        public override void Move(double speedMultiplier)
        {
            var path = CurrentTargetLocation - Location;
            if (path.Length < Speed * speedMultiplier)
            {
                Location = CurrentTargetLocation;
            }
            else
            {
                var endPoint = Vector2.Multiply(path.Normalized(), Speed * (float) speedMultiplier);
                Location.X += endPoint.X;
                Location.Y += endPoint.Y;
            }
        }
    }
}