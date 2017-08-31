using OpenTK;

namespace YetAnotherEngine.Utils.Models
{
    public class TilePositionObject
    {
        public TilePositionObject(Vector2 currentOffset, Vector2 location)
        {
            TileCoords = CoordsCalculator.CalculateCoords(currentOffset, location);
        }

        public Vector2 TileCoords { get; }
        public Vector2 TilePosition => CoordsCalculator.CalculatePositionFromTileLocation(TileCoords);
    }
}
