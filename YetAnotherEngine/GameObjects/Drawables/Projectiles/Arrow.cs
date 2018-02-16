using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using YetAnotherEngine.Constants;
using YetAnotherEngine.GameObjects.Drawables.Projectiles.ProjectilesImpact;
using YetAnotherEngine.GameObjects.Drawables.Units;

namespace YetAnotherEngine.GameObjects.Drawables.Projectiles
{
    class Arrow : ProjectileBase
    {
        public const int ProjectileWidth = 16;
        public const int ProjectileHeight = 16;

        private const int Speed = 4;
        private int _currentProjectileImpactDuration = ProjectileImpact.DrawDuration;

        public Arrow(Vector2 location, int textureId, int projectileImpactTextureId, UnitBase targetUnit) : base(
            location, textureId, projectileImpactTextureId, targetUnit)
        {
        }

        public override void Draw(Color color)
        {
            if (!IsDespawned)
            {
                if (!IsHitted)
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
                else
                {
                    _currentProjectileImpactDuration--;
                    projectilesImpact.Draw(Color.White);
                    if (_currentProjectileImpactDuration < 0) IsDespawned = true;
                }
            }
            else
            {
                projectilesImpact.Draw(Color.White);
            }
        }

        public override void Move(double speedMultiplier)
        {
            if (!TargetUnit.IsDespawned)
            {
                if (!IsHitted)
                {
                    var path = TargetUnit.Location - Location;
                    if (path.Length < Speed * speedMultiplier)
                    {
                        Location = TargetUnit.Location;
                        TargetUnit.Hit(10);
                        projectilesImpact.Location = Location;
                        IsHitted = true;
                    }
                    else
                    {
                        var endPoint = Vector2.Multiply(path.Normalized(), Speed * (float) speedMultiplier);
                        Location.X += endPoint.X;
                        Location.Y += endPoint.Y;
                    }
                }
            }
            else IsDespawned = true;
        }
    }
}