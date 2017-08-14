using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace YetAnotherEngine.GameObjects.Towers
{
    abstract class TowerBase
    {
        protected Vector2 Location;

        protected int TextureId { get; set; }

        public abstract void Draw();

        protected TowerBase(Vector2 location, int textureId)
        {
            Location = location;
            TextureId = textureId;
        }
    }
}
