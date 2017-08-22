using OpenTK;
using OpenTK.Input;
using YetAnotherEngine.Constants;

namespace YetAnotherEngine.Utils
{
    static class CoordsCalculator
    {
        public static Vector2 CalculateCoords(Vector2 currentOffset, Vector2 location)
        {
            //https://math.stackexchange.com/questions/312403/how-do-i-determine-if-a-point-is-within-a-rhombus

            Vector2 tileCoords;

            var roughLocation = new Vector2(currentOffset.X + location.X - Game.NominalWidth / 2f,
                                 -currentOffset.Y + location.Y  - Game.NominalHeight / 2f);

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

            Vector2 P = new Vector2(currentOffset.X + location.X - Game.NominalWidth / 2f,
                -currentOffset.Y + location.Y - Game.NominalHeight / 2f);

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

            return tileCoords;
        }

        public static Vector2 CalculatePositionFromTileLocation(Vector2 location)
        {
            int i, j;
            j = (int)((2 * location.Y - location.X - WorldConstants.WorldWidth * WorldConstants.TileWidth / 2f) / WorldConstants.TileWidth);
            i = (int)((location.Y - WorldConstants.TileHeight / 4f * j) / (WorldConstants.TileHeight / 4f));
            j += WorldConstants.WorldWidth;
            i -= WorldConstants.WorldWidth;

            return new Vector2(i, j);
        }

        public static Vector2 CalculateLocationFromTilePosition(Vector2 position)
        {
            return new Vector2((position.X) * WorldConstants.TileWidth / 2 + WorldConstants.WorldWidth * WorldConstants.TileWidth / 2 
                - (position.Y) * WorldConstants.TileWidth / 2, (position.X) * WorldConstants.TileHeight / 4 + (position.Y) * WorldConstants.TileHeight / 4);
        }

    }
}
