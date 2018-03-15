﻿using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using YetAnotherEngine.GameObjects.Drawables.Units;

namespace YetAnotherEngine.GameObjects.Drawables.Towers
{
    public abstract class TowerBase : IDrawable
    {
        public Vector2 Location { get; set; }

        public int Range { get; protected set; }

        public int CurrentShootigDelay { get; set; }

        protected int TextureId { get; set; }

        public abstract void Draw(Color color);

        public abstract void ResetDelay();

        public int Price { get; protected set; }

        protected TowerBase(Vector2 location, int textureId)
        {
            Location = location;
            TextureId = textureId;
        }

        public abstract UnitBase GetTargetUnit(SortedList<int, UnitBase> units);
    }
}