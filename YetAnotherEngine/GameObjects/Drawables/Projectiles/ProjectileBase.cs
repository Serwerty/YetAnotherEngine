using System.Drawing;
using OpenTK;
using YetAnotherEngine.GameObjects.Drawables.Projectiles.ProjectilesImpact;
using YetAnotherEngine.GameObjects.Drawables.Units;

namespace YetAnotherEngine.GameObjects.Drawables.Projectiles
{
    public abstract class ProjectileBase : IDrawable
    {
        public Vector2 Location;

        public UnitBase TargetUnit;

        public abstract void Draw(Color color);

        public abstract void Move(double speedMultiplier);
        public int Damage { get; protected set; }


        public bool IsDespawned { get; set; } = false;
        public bool IsHitted { get; set; } = false;
        protected int TextureId { get; set; }
        internal ProjectileImpact ProjectilesImpact;

        protected ProjectileBase(Vector2 location, int textureId, int projectileImpactTextureId, UnitBase targetUnit,int damage)
        {
            Location = location;
            TextureId = textureId;
            TargetUnit = targetUnit;
            Damage = damage;
            ProjectilesImpact = new ProjectileImpact(location, projectileImpactTextureId);
        }
    }
}