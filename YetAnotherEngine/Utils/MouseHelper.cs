using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using YetAnotherEngine.Constants;

namespace YetAnotherEngine.Utils
{
    class MouseHelper
    {
        private static MouseHelper _instance;
        private MouseDevice _mouseDevice;
        private MouseHelper() { }

        public Vector2 tileCoords { get; private set; }
        public Vector2 tilePosition { get; private set; }

        public static MouseHelper Instance => _instance ?? (_instance = new MouseHelper());

        public void Init(MouseDevice mouseDevice)
        {
            _mouseDevice = mouseDevice;
        }

        public void Calculate(Vector2 currentOffset)
        {
            CalculateCoords(currentOffset);
            CalculatePosition();
        }

        private void CalculateCoords(Vector2 currentOffset)
        {
            //https://math.stackexchange.com/questions/312403/how-do-i-determine-if-a-point-is-within-a-rhombus

            var roughLocation = new Vector2(currentOffset.X + _mouseDevice.X - Game.NominalWidth / 2f,
                                 -currentOffset.Y + _mouseDevice.Y - Game.NominalHeight / 2f);

            roughLocation.X -= roughLocation.X % WorldConstants.TileWidth;
            roughLocation.Y -= roughLocation.Y % (WorldConstants.TileHeight / 2f);

            Vector2 A = roughLocation + new Vector2(0, WorldConstants.TileHeight / 4f);
            Vector2 B = roughLocation + new Vector2(WorldConstants.TileWidth / 2f, WorldConstants.TileHeight / 2f);
            Vector2 C = roughLocation + new Vector2(WorldConstants.TileWidth, WorldConstants.TileHeight / 4f);
            Vector2 D = roughLocation + new Vector2(WorldConstants.TileWidth / 2f, 0);

            Vector2 Q = roughLocation + new Vector2(WorldConstants.TileWidth / 2f, WorldConstants.TileHeight / 4f);  // center point
            int a = WorldConstants.TileWidth / 2; // half-width (in the x-direction)
            int b = WorldConstants.TileHeight / 4;     // half-height (y-direction)
            Vector2 U = (C - A) / (2 * a);         // unit vector in x-direction
            Vector2 V = (D - B) / (2 * b);         // unit vector in y-direction

            Vector2 P = new Vector2(currentOffset.X + _mouseDevice.X - Game.NominalWidth / 2f,
                -currentOffset.Y + _mouseDevice.Y - Game.NominalHeight / 2f);

            Vector2 W = P - Q;
            float xabs = (W * U).Length;
            float yabs = (W * V).Length;
            if (xabs / a + yabs / b <= 1)
                tileCoords = roughLocation;
            else
            {
                if (W.X > 0)
                {
                    if (W.Y > 0)
                        tileCoords = roughLocation + new Vector2(WorldConstants.TileWidth / 2f, WorldConstants.TileHeight / 4f);
                    else
                        tileCoords = roughLocation + new Vector2(WorldConstants.TileWidth / 2f, -WorldConstants.TileHeight / 4f);
                }
                else
                {
                    if (W.Y > 0)
                        tileCoords = roughLocation + new Vector2(-WorldConstants.TileWidth / 2f, WorldConstants.TileHeight / 4f);
                    else
                        tileCoords = roughLocation + new Vector2(-WorldConstants.TileWidth / 2f, -WorldConstants.TileHeight / 4f);
                }
            }
        }

        private void CalculatePosition()
        {
            int i, j;
            j = (int)((2 * tileCoords.Y - tileCoords.X -  WorldConstants.WorldWidth * WorldConstants.TileWidth /2f) / WorldConstants.TileWidth);
            i = (int)((tileCoords.Y - WorldConstants.TileHeight / 4f * j) / (WorldConstants.TileHeight / 4f));
            j += WorldConstants.WorldWidth;
            i -= WorldConstants.WorldWidth;

            tilePosition = new Vector2(i,j);
        }

    }
}
