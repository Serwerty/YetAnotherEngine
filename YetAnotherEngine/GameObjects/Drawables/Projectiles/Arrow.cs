using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using YetAnotherEngine.Constants;
using YetAnotherEngine.GameObjects.Drawables.Units;

namespace YetAnotherEngine.GameObjects.Drawables.Projectiles
{
    class Arrow : ProjectileBase
    {
        public const int ProjectileWidth = 16;
        public const int ProjectileHeight = 16;

        private const int Speed = 4;

        public Arrow(Vector2 location, int textureId, UnitBase targetUnit) : base(location, textureId, targetUnit)
        {
        }

        public override void Draw(Color color)
        {
            if (!IsDespawned)
            {
                GL.BindTexture(TextureTarget.Texture2D, TextureId);

                GL.Begin(PrimitiveType.Quads);
                GL.Color4(color);

                GL.TexCoord2(0, 0);
                GL.Vertex2(Location.X + WorldConstants.TileWidth / 2f, Location.Y);
                GL.TexCoord2(1, 0);
                GL.Vertex2(Location.X + ProjectileWidth / 2f + WorldConstants.TileWidth / 2f, Location.Y);
                GL.TexCoord2(1, 1);
                GL.Vertex2(Location.X + ProjectileWidth / 2f + WorldConstants.TileWidth / 2f,
                    Location.Y + ProjectileHeight / 2f);
                GL.TexCoord2(0, 1);
                GL.Vertex2(Location.X + WorldConstants.TileWidth / 2f, Location.Y + ProjectileHeight / 2f);

                GL.End();
            }
        }

        public override void Move(double speedMultiplier)
        {
            if (!TargetUnit.IsDespawned)
            {
                var path = TargetUnit.Location - Location;
                if (path.Length < Speed * speedMultiplier)
                {
                    Location = TargetUnit.Location;
                    IsDespawned = true;
                }
                else
                {
                    var endPoint = Vector2.Multiply(path.Normalized(), Speed * (float) speedMultiplier);
                    Location.X += endPoint.X;
                    Location.Y += endPoint.Y;
                }
            }
            else IsDespawned = true;
        }

       
    }
}
