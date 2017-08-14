using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace YetAnotherEngine.GameObjects.Towers
{
    abstract class TowerBase
    {
        public Vector2 Location { get; set; }

        protected int TextureId { get; set; }

        public abstract void Draw(Color color);

        protected TowerBase(Vector2 location, int textureId)
        {
            Location = location;
            TextureId = textureId;
        }
    }
}
