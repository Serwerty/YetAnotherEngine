using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using YetAnotherEngine.Constants;
using YetAnotherEngine.GameObjects.Textures;
using YetAnotherEngine.GameObjects.Towers;
using YetAnotherEngine.GameObjects.Waves;
using YetAnotherEngine.Utils;

namespace YetAnotherEngine.GameObjects.World
{
    public class GameWorld
    {
        private readonly MouseDevice _mouseDevice;
        private readonly KeyboardDevice _keyboardDevice;

        private readonly MapLoader _mapLoader;
        private readonly MapTextures _mapTextures;
        private readonly WavesManager _wavesManager;
        private readonly TowersManager _towersManager;

        public GameWorld(MouseDevice mouseDevice, KeyboardDevice keyboardDevice)
        {
            _mapLoader = new MapLoader();
            _mapTextures = new MapTextures(); //TODO: should be map-related
            _wavesManager = new WavesManager(); //TODO: should be map-related
            _towersManager = new TowersManager(); //TODO: should be map-related

            _mouseDevice = mouseDevice;
            _keyboardDevice = keyboardDevice;
        }

        public void AddTower()
        {
            _towersManager.AddTower();
        }

        public void MoveUnits(double speedMultiplier)
        {
            _wavesManager.MoveUnits(speedMultiplier);
        }

        public void SpawnWaves()
        {
            _wavesManager.SpawnWave();
        }

        internal void RenderGround()
        {
            for (var i = 0; i < WorldConstants.WorldHeight; i++)
            {
                var globalOffsetX = i * WorldConstants.TileWidth / 2 + WorldConstants.WorldWidth * WorldConstants.TileWidth / 2;
                var globalOffsetY = i * WorldConstants.TileHeight / 4;

                for (var j = 0; j < WorldConstants.WorldWidth; j++)
                {
                    var location = new Vector2(globalOffsetX, globalOffsetY);
                    GL.BindTexture(TextureTarget.Texture2D, _mapLoader.ConstructionTiles[i, j].TextureId);

                    GL.Begin(PrimitiveType.Quads);
                    GL.Color4(Color.White);

                    GL.TexCoord2(0, 0);
                    GL.Vertex2(location);
                    GL.TexCoord2(1, 0);
                    GL.Vertex2(location.X + WorldConstants.TileWidth, location.Y);
                    GL.TexCoord2(1, 1);
                    GL.Vertex2(location.X + WorldConstants.TileWidth, location.Y + WorldConstants.TileHeight);
                    GL.TexCoord2(0, 1);
                    GL.Vertex2(location.X, location.Y + WorldConstants.TileHeight);

                    GL.End();

                    globalOffsetX -= WorldConstants.TileWidth / 2;
                    globalOffsetY += WorldConstants.TileHeight / 4;
                }
            }
        }

        internal void RenderTowers()
        {
            if (!_keyboardDevice[Key.T])
            {
                return;
            }

            _towersManager.RenderTowers();
        }

        internal void RenderTowerToBePlaced(Vector2 currentOffset)
        {
            _towersManager.RenderTowerToBePlaced(currentOffset);
        }

        internal void RenderUnits()
        {
            _wavesManager.RenderUnits();
        }

        internal void RenderSelection()
        {
            if (_shouldTowerBeRendered && _keyboardDevice[Key.T])
            {
                var location = MouseHelper.Instance.TilePositionObject.TileCoords;

                GL.BindTexture(TextureTarget.Texture2D, _mapTextures.SelectionTexture);

                GL.Begin(PrimitiveType.Quads);
                GL.Color4(Color.White);

                GL.TexCoord2(0, 0);
                GL.Vertex2(location);
                GL.TexCoord2(1, 0);
                GL.Vertex2(location.X + WorldConstants.TileWidth, location.Y);
                GL.TexCoord2(1, 1);
                GL.Vertex2(location.X + WorldConstants.TileWidth, location.Y + WorldConstants.TileHeight);
                GL.TexCoord2(0, 1);
                GL.Vertex2(location.X, location.Y + WorldConstants.TileHeight);

                GL.End();
            }
        }
    }
}