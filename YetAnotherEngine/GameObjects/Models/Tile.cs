using YetAnotherEngine.Enums;

namespace YetAnotherEngine.GameObjects
{
    public class ConstructionTile
    {
        public int TextureId { get; set; }
        public int TextureOffsetX { get; set; } = 64;
        public int TextureOffsetY { get; set; } = 64;
        public TileType Type { get; set; } = TileType.Tower;
    }
}
