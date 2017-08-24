using OpenTK;
using OpenTK.Input;

namespace YetAnotherEngine.Utils
{
    class MouseHelper
    {
        private static MouseHelper _instance;
        private MouseDevice _mouseDevice;
        private MouseHelper() { }

        public Vector2 TileCoords { get; private set; }
        public Vector2 TilePosition { get; private set; }

        public static MouseHelper Instance => _instance ?? (_instance = new MouseHelper());

        public void Init(MouseDevice mouseDevice)
        {
            _mouseDevice = mouseDevice;
        }

        public void Calculate(Vector2 currentOffset)
        {
            Vector2 location = new Vector2(_mouseDevice.X,_mouseDevice.Y);
            TileCoords = CoordsCalculator.CalculateCoords(currentOffset, location);
            TilePosition = CoordsCalculator.CalculatePositionFromTileLocation(TileCoords);
        }
    }
}
