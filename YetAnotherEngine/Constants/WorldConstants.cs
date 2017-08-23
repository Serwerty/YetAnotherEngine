using System.Drawing;

namespace YetAnotherEngine.Constants
{
    static class WorldConstants
    {
        public const int WorldHeight = 30;
        public const int WorldWidth = 30;
        public const int TileHeight = 64;
        public const int TileWidth = 64;
        public const float CameraScrollSpeed = 6f;

        public const int TargetUpdateRate = 60;
        public const int TargetRanderRate = 60;

        public const int MillisecondsInSecond = 1000;
        
        public static Color RedColor = Color.FromArgb(178,245, 0, 0);
        public static Color GreenColor = Color.FromArgb(178, 0,245,0);

        public const float ZoomSpeed = 0.05f;
        public const float ZoomInLimitation = 0f;
        public const float ZoomOutLimitation = 1f;
    }
}
