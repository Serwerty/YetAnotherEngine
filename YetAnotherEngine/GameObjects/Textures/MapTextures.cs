using System.Drawing;
using YetAnotherEngine.Constants;
using YetAnotherEngine.GameObjects.Towers;
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

        public int[] GroundTextures = new int[1]; //TODO: expand with map textures
        public int[] TowerTextures = new int[1]; //TODO: expand with tower textures
        public int[] UnitsTextures = new int[1];//TODO: expand with unit textures
        public int SelectionTexture; 


        public MapTextures()
        {
            //TODO: expand with other ground textures
            var backGroundTexture = new Bitmap(GroundTileFilePath);
            GroundTextures[0] = TextureLoader.GenerateTexture(backGroundTexture, WorldConstants.TileWidth,
                WorldConstants.TileWidth, 64, 64);

            var towerTexture = new Bitmap(TowersTileFilePath);
            TowerTextures[0] = TextureLoader.GenerateTexture(towerTexture, SimpleTower.TowerWidth,
                SimpleTower.TowerHeight, SimpleTower.TextureOffsetX, SimpleTower.TextureOffsetY);

            var selectionTexture = new Bitmap(SelectionTileFilePath);
            SelectionTexture = TextureLoader.GenerateTexture(selectionTexture, 64, 64, 0, 0);

            var basicUnitTexture = new Bitmap(UnitTileFilePath);
            UnitsTextures[0] = TextureLoader.GenerateTexture(basicUnitTexture, 64, 64, 0, 0);
        }
    }
}
