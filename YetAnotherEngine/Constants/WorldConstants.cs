using System.Drawing;

namespace YetAnotherEngine.Constants
{
    static class WorldConstants
    {
        public const int WorldHeight = 64;
        public const int WorldWidth = 64;
        public const int TileHeight = 64;
        public const int TileWidth = 64;
        public const float CameraScrollSpeed = 4f;

        public const int TargetUpdateRate = 60;
        public const int TargetRanderRate = 60;

        public const int MillisecondsInSecond = 1000;
        
        public static Color RedColor = Color.FromArgb(178,245, 0, 0);
        public static Color GreenColor = Color.FromArgb(178, 0,245,0);
    }
}
