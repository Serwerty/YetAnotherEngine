using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using YetAnotherEngine.Constants;
using YetAnotherEngine.GameObjects.Drawables;
using YetAnotherEngine.GameObjects.Drawables.Projectiles;
using YetAnotherEngine.GameObjects.Drawables.Towers;
using YetAnotherEngine.GameObjects.Textures;
using YetAnotherEngine.GameObjects.Waves;
using YetAnotherEngine.Utils;
using YetAnotherEngine.Utils.Helpers;

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
        private ProjectilesManager _projectilesManager;
        private SortedList<int, IDrawable> _drawablesList;

        public GameWorld(MouseDevice mouseDevice, KeyboardDevice keyboardDevice, Camera camera)
        {
            _mapTextures = new MapTextures(); //TODO: should be map-related
            _mapLoader = new MapLoader(_mapTextures.GroundTextures);
            _wavesManager = new WavesManager(_mapLoader.RoadList, _mapTextures.UnitsTextures, camera); //TODO: should be map-related
            _towersManager = new TowersManager(_mapTextures.TowerTextures,_mapTextures.TowerRangeFiledTexture); //TODO: should be map-related
            _projectilesManager = new ProjectilesManager(_mapTextures.ProjectilesTextures);

            _mouseDevice = mouseDevice;
            _keyboardDevice = keyboardDevice;
        }

        public void AddTower()
        {
            var x = (int)MouseHelper.Instance.TilePositionObject.TilePosition.X;
            var y = (int)MouseHelper.Instance.TilePositionObject.TilePosition.Y;

            _towersManager.AddTower(x, y, _mapLoader);
        }

        public void MoveUnits(double speedMultiplier)
        {
            _wavesManager.MoveUnits(speedMultiplier);
        }

        public void MoveProjectiles(double speedMultiplier)
        {
            _projectilesManager.MoveProjectiles(speedMultiplier);
        }

        public void checkTowersForShoot()
        {
            _towersManager.CheckTowersForShoot(_wavesManager.GetUnits(),ref _projectilesManager);
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
            _towersManager.RenderTowers();
        }

        internal void RenderDrawables()
        {
            _drawablesList = new SortedList<int, IDrawable>();
            foreach (var unit in _wavesManager.GetUnits())
            {
                _drawablesList.Add(unit.Key,(IDrawable)unit.Value);
            }
            foreach (var tower in _towersManager.GetTowers())
            {
                _drawablesList.Add(tower.Key, (IDrawable)tower.Value);
            }
            foreach (var projectile in _projectilesManager.GetProjectiles())
            {
                _drawablesList.Add(projectile.Key, (IDrawable)projectile.Value);
            }
            foreach (var drawable in _drawablesList)
            {
                drawable.Value.Draw(Color.White);
            }
        }

        internal void RenderTowerToBePlaced(Vector2 currentOffset)
        {
            if(_keyboardDevice[Key.T])
            {
                var x = (int)MouseHelper.Instance.TilePositionObject.TilePosition.X;
                var y = (int)MouseHelper.Instance.TilePositionObject.TilePosition.Y;

                _towersManager.RenderTowerToBePlaced(currentOffset,
                    new Vector2(_mouseDevice.X, _mouseDevice.Y), x, y, _mapLoader);
            }
        }

        internal void RenderUnits()
        {
            _wavesManager.RenderUnits();
        }

        internal void RenderSelection()
        {
            if (_keyboardDevice[Key.T] /*&& _shouldTowerBeRendered*/)
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