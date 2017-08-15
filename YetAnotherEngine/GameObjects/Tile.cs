using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YetAnotherEngine.Enums;

namespace YetAnotherEngine.GameObjects
{
    class Tile
    {
        public int TextureId { get; set; }
        public int TextureOffsetX { get; set; } = 64;
        public int TextureOffsetY { get; set; } = 64;
        public TileType Type { get; set; } = TileType.Tower;

    }
}
