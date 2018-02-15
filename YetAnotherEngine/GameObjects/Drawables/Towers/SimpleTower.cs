using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using YetAnotherEngine.GameObjects.Drawables.Units;

namespace YetAnotherEngine.GameObjects.Drawables.Towers
{
    public class SimpleTower : TowerBase
    {
        public const int TextureOffsetX = 7;
        public const int TextureOffsetY = 0;

        public const int TowerWidth = 64;
        public const int TowerHeight = 98;

        public const int TowerCenterX = 15;
        public const int TowerCenterY = 41;

        private const int ShootingDelay = 50;

        public SimpleTower(Vector2 location, int textureId) : base(location, textureId)
        {
            Range = 75;
        }

        public override void Draw(Color color)
        {
            GL.BindTexture(TextureTarget.Texture2D, TextureId);

            GL.Begin(PrimitiveType.Quads);
            GL.Color4(color);

            GL.TexCoord2(0, 0);
            GL.Vertex2(Location);
            GL.TexCoord2(1, 0);
            GL.Vertex2(Location.X + TowerWidth / 2f, Location.Y);
            GL.TexCoord2(1, 1);
            GL.Vertex2(Location.X + TowerWidth / 2f, Location.Y + TowerHeight / 2f);
            GL.TexCoord2(0, 1);
            GL.Vertex2(Location.X, Location.Y + TowerHeight / 2f);

            GL.End();
        }

        public override void ResetDelay()
        {
            CurrentShootigDelay = ShootingDelay;
        }

        public override UnitBase CalculateClosestUnit(SortedList<int, UnitBase> units)
        {
            double minDistance = int.MaxValue;
            int key = 0;

            foreach (var unit in units)
            {
                double distance = Math.Sqrt((unit.Value.Location.X - Location.X) * (unit.Value.Location.X - Location.X) +
                                           (unit.Value.Location.Y * 2  - Location.Y * 2) * (unit.Value.Location.Y * 2 - Location.Y * 2));
                if (distance <= Range && distance < minDistance)
                {
                    minDistance = distance;
                    key = unit.Key;
                }
            }

            return units.FirstOrDefault(x => x.Key == key).Value;
        }
    }
}