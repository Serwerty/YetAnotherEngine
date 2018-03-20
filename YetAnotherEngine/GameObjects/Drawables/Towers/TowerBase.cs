using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using YetAnotherEngine.GameObjects.Drawables.Units;

namespace YetAnotherEngine.GameObjects.Drawables.Towers
{
    public abstract class TowerBase : IDrawable
    {
        public Vector2 Location { get; set; }

        public const int TowerWidth = 64;
        public const int TowerHeight = 98;

        public int Range { get; protected set; }
        public int Price { get; protected set; }
        public int Damage { get; protected set; }

        public float CurrentShootigDelay { get; set; }

        protected int TextureId { get; set; }

        public void Draw(Color color)
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

        public abstract void ResetDelay();

        

        protected TowerBase(Vector2 location, int textureId)
        {
            Location = location;
            TextureId = textureId;
        }

        public abstract UnitBase GetTargetUnit(SortedList<int, UnitBase> units);
    }
}