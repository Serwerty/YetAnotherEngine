﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK;
using YetAnotherEngine.Utils;

namespace YetAnotherEngine.GameObjects.Drawables.Units
{
    public abstract class UnitBase : IDrawable
    {
        public Vector2 Location;

        public int Hp;
        public int CurrentHp { get; set; }

        public Vector2 CurrentTargetLocation;

        protected int TextureId { get; set; }

        public bool IsDespawned { get; set; } = false;

        internal HpBar hpBar;

        public abstract void Draw(Color color);

        protected UnitBase(Vector2 location, int textureId, int hpBarTextureId)
        {
            Location = location;
            TextureId = textureId;
            CurrentTargetLocation = location;
            hpBar = new HpBar(location, hpBarTextureId);
        }

        public abstract void Move(double speedMultiplier);

        public void CheckLocation(List<Vector2> pathList)
        {
            if (CurrentTargetLocation == Location)
            {
                if (CoordsCalculator.CalculatePositionFromTileLocation(Location) != pathList.Last())
                {
                    var nextTarget = CoordsCalculator.CalculateLocationFromTilePosition(pathList.ElementAt(
                        pathList.IndexOf(CoordsCalculator.CalculatePositionFromTileLocation(Location)) + 1));
                    //Random rnd = new Random((int)DateTime.Now.Ticks);
                    //nextTarget.X += rnd.Next(-8, 8);
                    //nextTarget.Y += rnd.Next(-4, 4);
                    CurrentTargetLocation = nextTarget;
                }
                else
                {
                    LivesManager.GetInstance().LoseLive();
                    IsDespawned = true;
                }
            }
        }

        public abstract void Hit(int damage);
    }
}