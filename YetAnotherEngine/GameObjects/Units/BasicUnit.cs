using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace YetAnotherEngine.GameObjects.Units
{
    class BasicUnit : UnitBase
    {
        public const int UnitWidth = 64;
        public const int UnitHeight = 64;

        private const int speed = 2;

        public BasicUnit(Vector2 location, int textureId) : base(location, textureId)
        {
        }

        public override void Draw()
        {
            GL.BindTexture(TextureTarget.Texture2D, TextureId);

            GL.Begin(PrimitiveType.Quads);
            GL.Color4(Color.White);

            GL.TexCoord2(0, 0);
            GL.Vertex2(Location.X+16,Location.Y - 8);
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
            Vector2 path = CurrentTargetLocation - Location;
            if (path.Length < speed * speedMultiplier)
            {
                Location = CurrentTargetLocation;
            }
            else
            {
                Vector2 endPoint = Vector2.Multiply(path.Normalized(), speed * (float)speedMultiplier);
                Location.X += endPoint.X;
                Location.Y += endPoint.Y;
            }
        }
    }
}
