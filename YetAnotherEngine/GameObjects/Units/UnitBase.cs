using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace YetAnotherEngine.GameObjects.Units
{
    public abstract class UnitBase
    {
        public Vector2 Location { get; set; }

        protected int TextureId { get; set; }

        public abstract void Draw(Color color);

        protected UnitBase(Vector2 location, int textureId)
        {
            Location = location;
            TextureId = textureId;
        }

        protected abstract void Move(Vector2 endPoint);
    }
}
