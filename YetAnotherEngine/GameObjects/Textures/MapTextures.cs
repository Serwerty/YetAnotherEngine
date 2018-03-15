using System.Drawing;
using System.Runtime.InteropServices.WindowsRuntime;
using YetAnotherEngine.Constants;
using YetAnotherEngine.GameObjects.Drawables.Towers;
using YetAnotherEngine.Utils;

namespace YetAnotherEngine.GameObjects.Textures
{
    //TODO: should be map-related
    public class MapTextures
    {
        //TODO: place to map file
        private const string GroundTileFilePath = "Textures/Tiles/terrain_tile.png";
        private const string TowersTileFilePath = "Textures/Tiles/towers.png";
        private const string SelectionTileFilePath = "Textures/Selection.png";
        private const string UnitTileFilePath = "Textures/Knowitall_Front_point.png";
        private const string ArrowProjectileTileFilePath = "Textures/projectile_fireball.png";
        private const string TowerRangeFieldTileFilePath = "Textures/range.png";
        private const string HitmarkerTileFilePath = "Textures/hitmarker.png";
        private const string HpBarTileFilePath = "Textures/quad.png";

        public int[] GroundTextures = new int[2]; //TODO: expand with map textures
        public int[] TowerTextures = new int[1]; //TODO: expand with tower textures
        public int[] UnitsTextures = new int[1]; //TODO: expand with unit textures
        public int[] ProjectilesTextures = new int[1]; //TODO: expand with projectile textures
        public int SelectionTexture;
        public int TowerRangeFiledTexture;
        public int HitMarkerTexture;
        public int HpBarTexture;


        public MapTextures()
        {
            //TODO: expand with other ground textures map dependant
            var backGroundTexture = new Bitmap(GroundTileFilePath);
            GroundTextures[0] = TextureLoader.GenerateTexture(backGroundTexture, WorldConstants.TileWidth,
                WorldConstants.TileWidth, 64, 64);
            GroundTextures[1] = TextureLoader.GenerateTexture(backGroundTexture, WorldConstants.TileWidth,
                WorldConstants.TileWidth, 128, 832);

            var towerTexture = new Bitmap(TowersTileFilePath);
            TowerTextures[0] = TextureLoader.GenerateTexture(towerTexture, 128,
                196, SimpleTower.TextureOffsetX, SimpleTower.TextureOffsetY);

            var selectionTexture = new Bitmap(SelectionTileFilePath);
            SelectionTexture = TextureLoader.GenerateTexture(selectionTexture, 64, 64, 0, 0);

            var basicUnitTexture = new Bitmap(UnitTileFilePath);
            UnitsTextures[0] = TextureLoader.GenerateTexture(basicUnitTexture, 64, 64, 0, 0);

            var arrowProjectileTexture = new Bitmap(ArrowProjectileTileFilePath);
            ProjectilesTextures[0] = TextureLoader.GenerateTexture(arrowProjectileTexture, 128, 128, 0, 0);

            var towerRangeFiledTexture = new Bitmap(TowerRangeFieldTileFilePath);
            TowerRangeFiledTexture = TextureLoader.GenerateTexture(towerRangeFiledTexture, 127, 64, 0, 0);

            var hitmarkerTexture = new Bitmap(HitmarkerTileFilePath);
            HitMarkerTexture = TextureLoader.GenerateTexture(hitmarkerTexture, 128, 128, 0, 0);

            var hpBarTexture = new Bitmap(HpBarTileFilePath);
            HpBarTexture = TextureLoader.GenerateTexture(hpBarTexture, 64, 64, 0, 0);
        }
    }
}