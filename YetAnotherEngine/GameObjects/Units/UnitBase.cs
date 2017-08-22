using System.Drawing;
using OpenTK;

namespace YetAnotherEngine.GameObjects.Units
{
    public abstract class UnitBase
    {
        public Vector2 Location; 

        protected int TextureId { get; set; }

        public abstract void Draw(Color color);

        protected UnitBase(Vector2 location, int textureId)
        {
            Location = location;
            TextureId = textureId;
        }

        public abstract void Move(Vector2 targetLocation, double speedMultiplier);
    }
}
