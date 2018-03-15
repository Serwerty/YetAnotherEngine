using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using YetAnotherEngine.Constants;

namespace YetAnotherEngine.GameObjects.Drawables.Projectiles.ProjectilesImpact
{
    class ProjectileImpact
    {
        public const int DrawDuration = 3;

        public Vector2 Location
        {
            get => _location;
            set => _location = value - new Vector2(ProjectileSize.X / 2, ProjectileSize.Y / 2);
        }

        public Vector2 ProjectileSize = new Vector2(12, 12);
        private Vector2 _location;

        public ProjectileImpact(Vector2 location, int textureId)
        {
            Location = location;
            TextureId = textureId;
        }

        protected int TextureId { get; set; }

        public void Draw(Color color)
        {
            GL.BindTexture(TextureTarget.Texture2D, TextureId);

            GL.Begin(PrimitiveType.Quads);
            GL.Color4(color);

            GL.TexCoord2(0, 0);
            GL.Vertex2(Location.X + WorldConstants.TileWidth / 2f, Location.Y);
            GL.TexCoord2(1, 0);
            GL.Vertex2(Location.X + ProjectileSize.X + WorldConstants.TileWidth / 2f, Location.Y);
            GL.TexCoord2(1, 1);
            GL.Vertex2(Location.X + ProjectileSize.X + WorldConstants.TileWidth / 2f,
                Location.Y + ProjectileSize.Y);
            GL.TexCoord2(0, 1);
            GL.Vertex2(Location.X + WorldConstants.TileWidth / 2f, Location.Y + ProjectileSize.Y);

            GL.End();
        }
    }
}