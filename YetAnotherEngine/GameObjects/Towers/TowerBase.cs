﻿using System.Drawing;
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
