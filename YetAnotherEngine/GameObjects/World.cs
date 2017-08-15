using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using YetAnotherEngine.Constants;
using YetAnotherEngine.Enums;
using YetAnotherEngine.GameObjects.Towers;
using YetAnotherEngine.Utils;
using YetAnotherEngine.GameObjects.Units;

namespace YetAnotherEngine.GameObjects
{
    public class World
    {
        private const int WorldWidth = WorldConstants.WorldWidth;
        private const int WorldHeight = WorldConstants.WorldHeight;

        private const string GroundTileFilePath = "Textures/Tiles/terrain_tile.png";
        private const string TowersTileFilePath = "Textures/Tiles/towers.png";
        private const string SelectionTileFilePath = "Textures/Selection.png";

        private readonly int[] _groundTextures = new int[64]; // for now it wil be one(random) of 5
        private readonly int[,] _groundTexturesMap = new int[WorldHeight, WorldWidth];
        private readonly Tile[,] _tiles = new Tile[WorldHeight, WorldWidth];

        private int _basicTowerTextureId;
        private int _selectionTextureId;
        private readonly SortedList<int, TowerBase> _towersList;

        private readonly MouseDevice _mouseDevice;
        private readonly KeyboardDevice _keyboardDevice;

        private readonly TowerBase _towerToBePlaced;
        private readonly bool _isTowerShouldBeRendered;

        private List<UnitBase> Units = new List<UnitBase>();

        public World(MouseDevice mouseDevice, KeyboardDevice keyboardDevice)
        {
            _mouseDevice = mouseDevice;
            _keyboardDevice = keyboardDevice;
            LoadMapTextures();
            LoadMap();
            _towersList = new SortedList<int, TowerBase>();
            _towerToBePlaced = new BasicTower(new Vector2(0, 0), _basicTowerTextureId);
            _isTowerShouldBeRendered = true;
        }

        public void AddTower()
        {
            if (CheckIfTowerCanBePlaced() && _isTowerShouldBeRendered)
            {
                var location = MouseHelper.Instance.tileCoords -
                               new Vector2(BasicTower.TowerCenterX - WorldConstants.TileWidth / 2,
                                   BasicTower.TowerCenterY - WorldConstants.TileHeight / 4);

                TowerBase tower = new BasicTower(location, _basicTowerTextureId);

                _towersList.Add(
                    (int) MouseHelper.Instance.tilePosition.X * 100 + (int) MouseHelper.Instance.tilePosition.Y, tower);
            }
        }

        public void RenderGround()
        {
            for (var i = 0; i < WorldHeight; i++)
            {
                var globalOffsetX = i * WorldConstants.TileWidth / 2 + WorldWidth * WorldConstants.TileWidth / 2;
                var globalOffsetY = i * WorldConstants.TileHeight / 4;

                for (var j = 0; j < WorldWidth; j++)
                {
                    var location = new Vector2(globalOffsetX, globalOffsetY);
                    GL.BindTexture(TextureTarget.Texture2D, _tiles[i, j].TextureId);

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

        public void MoveUnits(double speedMultiplier)
        {
            foreach(var unit in Units)
            {
                var targetLocation = UnitTargetLocation(unit.Location);
                unit.Move(targetLocation, speedMultiplier);
            }
        }

        private Vector2 UnitTargetLocation(Vector2 unitCurrentLocation)
        {
            return new Vector2(0,0);
        }

        private void LoadMapTextures()
        {
            var backGroundTexture = new Bitmap(GroundTileFilePath);
            _groundTextures[0] = TextureLoader.GenerateTexture(backGroundTexture, WorldConstants.TileWidth,
                WorldConstants.TileWidth, 64, 64);
            _groundTextures[1] = TextureLoader.GenerateTexture(backGroundTexture, WorldConstants.TileWidth,
                WorldConstants.TileWidth, 64, 64);
            _groundTextures[2] = TextureLoader.GenerateTexture(backGroundTexture, WorldConstants.TileWidth,
                WorldConstants.TileWidth, 64, 64);
            _groundTextures[3] = TextureLoader.GenerateTexture(backGroundTexture, WorldConstants.TileWidth,
                WorldConstants.TileWidth, 64, 64);
            _groundTextures[4] = TextureLoader.GenerateTexture(backGroundTexture, WorldConstants.TileWidth,
                WorldConstants.TileWidth, 64, 64);

            var towerTexture = new Bitmap(TowersTileFilePath);
            _basicTowerTextureId = TextureLoader.GenerateTexture(towerTexture, BasicTower.TowerWidth,
                BasicTower.TowerHeight, BasicTower.TextureOffsetX, BasicTower.TextureOffsetY);

            var selectionTexture = new Bitmap(SelectionTileFilePath);
            _selectionTextureId = TextureLoader.GenerateTexture(selectionTexture, 64, 64, 0, 0);


            for (var i = 0; i < WorldHeight; i++)
            for (var j = 0; j < WorldWidth; j++)
            {
                var random = new Random(unchecked((int) DateTime.Now.Ticks));
                _groundTexturesMap[i, j] = _groundTextures[random.Next(0, 5)];
            }
        }

        private void LoadMap()
        {
            var tilesTextureMap = new Bitmap(GroundTileFilePath);

            for (var i = 0; i < WorldHeight; i++)
            for (var j = 0; j < WorldWidth; j++)
                _tiles[i, j] = new Tile();

            try
            {
                using (var map = new StreamReader("Maps/map.dat"))
                {
                    string line;
                    while ((line = map.ReadLine()) != null)
                    {
                        var words = line.Split(',');

                        var i = Convert.ToInt32(words[0]);
                        var j = Convert.ToInt32(words[1]);
                        var textureOffsetX = Convert.ToInt32(words[2]);
                        var textureOffsetY = Convert.ToInt32(words[3]);
                        TileType type;
                        Enum.TryParse(words[4], out type);

                        _tiles[i, j].TextureOffsetX = textureOffsetX;
                        _tiles[i, j].TextureOffsetY = textureOffsetY;
                        _tiles[i, j].Type = type;
                    }
                }
            }
            catch (Exception exp)
            {
                //TODO: LOG OR SOME SHIT.
            }

            for (var i = 0; i < WorldHeight; i++)
            for (var j = 0; j < WorldWidth; j++)
                _tiles[i, j].TextureId = TextureLoader.GenerateTexture(tilesTextureMap, WorldConstants.TileWidth,
                    WorldConstants.TileHeight, _tiles[i, j].TextureOffsetX, _tiles[i, j].TextureOffsetY);
        }

        internal void RenderTowers()
        {
            foreach (var tower in _towersList)
                tower.Value.Draw(Color.White);
        }

        internal void RenderTowerToBePlaced(Vector2 currentOffset)
        {
            if (!_isTowerShouldBeRendered) return;

            if (!_keyboardDevice[Key.T]) return;

            _towerToBePlaced.Location =
                new Vector2(currentOffset.X + _mouseDevice.X - Game.NominalWidth / 2f - BasicTower.TowerCenterX,
                    -currentOffset.Y + _mouseDevice.Y - Game.NominalHeight / 2f - BasicTower.TowerCenterY);

            _towerToBePlaced.Draw(CheckIfTowerCanBePlaced()
                ? WorldConstants.GreenColor
                : WorldConstants.RedColor);
        }

        internal void RenderSelection()
        {
            if (_isTowerShouldBeRendered)
                if (_keyboardDevice[Key.T])
                {
                    var location = MouseHelper.Instance.tileCoords;

                    GL.BindTexture(TextureTarget.Texture2D, _selectionTextureId);

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

        private bool CheckIfTowerCanBePlaced()
        {
            var x = (int) MouseHelper.Instance.tilePosition.X;
            var y = (int) MouseHelper.Instance.tilePosition.Y;
            Game._fpsText.WriteCoords("i: " + x + " j:" + y + " x:" + MouseHelper.Instance.tileCoords.X + " y:" +
                                      MouseHelper.Instance.tileCoords.Y);

            if (_towersList.ContainsKey(x * 100 + y))
                return false;

            if (x < 0 || x >= WorldHeight) return false;
            if (y < 0 || y >= WorldWidth) return false;

            return _tiles[x, y].Type == TileType.Tower;
        }
    }
}
