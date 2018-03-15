using System.Drawing;

namespace YetAnotherEngine.Constants
{
    static class WorldConstants
    {
        public const int WorldHeight = 16;
        public const int WorldWidth = 16;
        public const int TileHeight = 64;
        public const int TileWidth = 64;
        public const float CameraScrollSpeed = 6f;

        public const int TargetUpdateRate = 60;

        public static Color RedColor = Color.FromArgb(178,245, 0, 0);
        public static Color GreenColor = Color.FromArgb(178, 0,245,0);

        public const float ZoomSpeed = 0.01f;
        public const float ZoomInLimitation = 0.5f;
        public const float ZoomOutLimitation = 2f;

        public const string FontTextureName = "big-outline.png";
    }
}
