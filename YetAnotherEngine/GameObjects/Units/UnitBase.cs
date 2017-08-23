using System.Drawing;
using OpenTK;

namespace YetAnotherEngine.GameObjects.Units
{
    public abstract class UnitBase
    {
        public Vector2 Location;

        public Vector2 CurrentTargetLocation;

        protected int TextureId { get; set; }

        public bool IsDespawned { get; set; } = false;

        public abstract void Draw();

        protected UnitBase(Vector2 location, int textureId)
        {
            Location = location;
            TextureId = textureId;
            CurrentTargetLocation = location;
        }

        public abstract void Move(double speedMultiplier);
    }
}
