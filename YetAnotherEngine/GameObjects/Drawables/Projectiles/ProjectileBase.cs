using System.Drawing;
using OpenTK;
using YetAnotherEngine.GameObjects.Drawables.Units;

namespace YetAnotherEngine.GameObjects.Drawables.Projectiles
{
    public abstract class ProjectileBase : IDrawable
    {
        public Vector2 Location;

        public UnitBase TargetUnit;

        public abstract void Draw(Color color);

        public abstract void Move(double speedMultiplier);

        public bool IsDespawned { get; set; } = false;

        protected int TextureId { get; set; }

        protected ProjectileBase(Vector2 location, int textureId, UnitBase targetUnit)
        {
            Location = location;
            TextureId = textureId;
            TargetUnit = targetUnit;
        }
    }
}
