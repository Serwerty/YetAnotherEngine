using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
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
            var roughLocation = new Vector2(currentOffset.X + _mouseDevice.X - Game.NominalWidth / 2,
                                 -currentOffset.Y + _mouseDevice.Y - Game.NominalHeight / 2);

            roughLocation.X -= roughLocation.X % WorldConstants.TileWidth;
            roughLocation.Y -= roughLocation.Y % (WorldConstants.TileHeight / 2);

            Vector2 A = roughLocation + new Vector2(0, WorldConstants.TileHeight / 4);
            Vector2 B = roughLocation + new Vector2(WorldConstants.TileWidth / 2, WorldConstants.TileHeight / 2);
            Vector2 C = roughLocation + new Vector2(WorldConstants.TileWidth, WorldConstants.TileHeight / 4);
            Vector2 D = roughLocation + new Vector2(WorldConstants.TileWidth / 2, 0);

            Vector2 Q = roughLocation + new Vector2(WorldConstants.TileWidth / 2, WorldConstants.TileHeight / 4);  // center point
            int a = WorldConstants.TileWidth / 2; // half-width (in the x-direction)
            int b = WorldConstants.TileHeight / 4;     // half-height (y-direction)
            Vector2 U = (C - A) / (2 * a);         // unit vector in x-direction
            Vector2 V = (D - B) / (2 * b);         // unit vector in y-direction

            Vector2 P = new Vector2(currentOffset.X + _mouseDevice.X - Game.NominalWidth / 2,
                -currentOffset.Y + _mouseDevice.Y - Game.NominalHeight / 2);

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
                        tileCoords = roughLocation + new Vector2(WorldConstants.TileWidth / 2, WorldConstants.TileHeight / 4);
                    else
                        tileCoords = roughLocation + new Vector2(WorldConstants.TileWidth / 2, -WorldConstants.TileHeight / 4);
                }
                else
                {
                    if (W.Y > 0)
                        tileCoords = roughLocation + new Vector2(-WorldConstants.TileWidth / 2, WorldConstants.TileHeight / 4);
                    else
                        tileCoords = roughLocation + new Vector2(-WorldConstants.TileWidth / 2, -WorldConstants.TileHeight / 4);
                }
            }
        }

        private void CalculatePosition()
        {
            int i, j;
            j = (int)((2 * tileCoords.Y - tileCoords.X -  WorldConstants.WorldWidth * WorldConstants.TileWidth /2) / WorldConstants.TileWidth);
            i = (int)((tileCoords.Y - WorldConstants.TileHeight / 4 * j) / (WorldConstants.TileHeight / 4));
            j += WorldConstants.WorldWidth;
            i -= WorldConstants.WorldHeight;

            tilePosition = new Vector2(i,j);
            Game._fpsText.WriteFps(i + ":" + j);
        }

    }
}
