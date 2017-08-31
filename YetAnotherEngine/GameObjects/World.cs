using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform.Windows;
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

        private readonly int[] _groundTextures = new int[64]; // for now it wil be one(random) of 5
        private readonly int[,] _groundTexturesMap = new int[WorldHeight, WorldWidth];
        private readonly ConstructionTile[,] _tiles = new ConstructionTile[WorldHeight, WorldWidth];

        private int _basicTowerTextureId;
        private int _selectionTextureId;
        private int _basicUnitTextureId;
        private readonly SortedList<int, TowerBase> _towersList;

        private readonly MouseDevice _mouseDevice;
        private readonly KeyboardDevice _keyboardDevice;

        private readonly TowerBase _towerToBePlaced;
        private readonly bool _shouldTowerBeRendered;

        private List<Vector2> _roadList;
        private List<Vector2> _pathList;

        private List<UnitBase> Units = new List<UnitBase>();
        private Wave _wave;


        public World(MouseDevice mouseDevice, KeyboardDevice keyboardDevice)
        {
            _roadList = new List<Vector2>();
            _mouseDevice = mouseDevice;
            _keyboardDevice = keyboardDevice;
            LoadMapTextures();
            LoadMap();
            _towersList = new SortedList<int, TowerBase>();
            _towerToBePlaced = new BasicTower(new Vector2(0, 0), _basicTowerTextureId);
            _pathList = CalculatePath();
            _wave = new Wave(UnitType.Basic, 5, 1);
            _shouldTowerBeRendered = true;
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

            var basicUnitTexture = new Bitmap(UnitTileFilePath);
            _basicUnitTextureId = TextureLoader.GenerateTexture(basicUnitTexture, 64, 64, 0, 0);

            for (var i = 0; i < WorldHeight; i++)
            {
                for (var j = 0; j < WorldWidth; j++)
                {
                    var random = new Random(unchecked((int)DateTime.Now.Ticks));
                    _groundTexturesMap[i, j] = _groundTextures[random.Next(0, 5)];
                }
            }
        }

        private void LoadMap()
        {
            var tilesTextureMap = new Bitmap(GroundTileFilePath);

            for (var i = 0; i < WorldHeight; i++)
            {
                for (var j = 0; j < WorldWidth; j++)
                {
                    _tiles[i, j] = new ConstructionTile();
                }
            }

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

                        if (type == TileType.Road)
                        {
                            _roadList.Add(new Vector2(i, j));
                        }
                    }
                }
            }
            catch (Exception)
            {
                //TODO: LOG OR SOME SHIT.
            }

            for (var i = 0; i < WorldHeight; i++)
            {
                for (var j = 0; j < WorldWidth; j++)
                {
                    _tiles[i, j].TextureId = TextureLoader.GenerateTexture(tilesTextureMap, WorldConstants.TileWidth,
                                                                           WorldConstants.TileHeight, _tiles[i, j].TextureOffsetX,
                                                                           _tiles[i, j].TextureOffsetY);
                }
            }
        }

        private bool IsTowerPlaceable()
        {
            var x = (int)MouseHelper.Instance.TilePositionObject.TilePosition.X;
            var y = (int)MouseHelper.Instance.TilePositionObject.TilePosition.Y;
            //Game._fpsText.WriteCoords($"Position: [{x}:{y}] " +
            //                          $"Location: [{MouseHelper.Instance.TilePositionObject.TileCoords.X}:{MouseHelper.Instance.TilePositionObject.TileCoords.Y}] " +
            //                          $"Mouse: [{_mouseDevice.X}:{_mouseDevice.Y}]");

            if (_towersList.ContainsKey(x * 100 + y) || x < 0 ||
                x >= WorldHeight || y < 0 || y >= WorldWidth)
            {
                return false;
            }

            return _tiles[x, y].Type == TileType.Tower;
        }

        private List<Vector2> CalculatePath()
        {
            var path = new List<Vector2>();
            var lastRoadPosition = new Vector2(-1, -1);
            Direction direction;
            Direction lastDirection = Direction.Down;
            bool notFirstTile = false;
            foreach (var roadPostion in _roadList)
            {
                if (lastRoadPosition.X != -1)
                {
                    if (notFirstTile)
                    {
                        direction = GetDirection(lastRoadPosition, roadPostion);

                        if (lastDirection != direction)
                        {
                            path.Add(lastRoadPosition);
                        }
                        lastDirection = direction;
                    }
                    else
                    {
                        lastDirection = GetDirection(lastRoadPosition, roadPostion);
                        notFirstTile = true;
                    }
                }
                else path.Add(roadPostion);
                lastRoadPosition = roadPostion;
            }
            path.Add(_roadList.Last());
            return path;
        }

        private void CheckLocation(UnitBase unit)
        {
            if (unit.CurrentTargetLocation == unit.Location)
            {
                if (CoordsCalculator.CalculatePositionFromTileLocation(unit.Location) != _pathList.Last())
                {
                    Vector2 nextTarget = CoordsCalculator.CalculateLocationFromTilePosition(_pathList.ElementAt(
                        _pathList.IndexOf(CoordsCalculator.CalculatePositionFromTileLocation(unit.Location)) + 1));
                    //Random rnd = new Random((int)DateTime.Now.Ticks);
                    //nextTarget.X += rnd.Next(-8, 8);
                    //nextTarget.Y += rnd.Next(-4, 4);
                    unit.CurrentTargetLocation = nextTarget;
                }
                else
                {
                    unit.IsDespawned = true;
                }
            }
        }

        private Direction GetDirection(Vector2 lastPosition, Vector2 currentPosition)
        {
            if (lastPosition.X - currentPosition.X == -1)
            {
                return Direction.Down;
            }

            if (lastPosition.X - currentPosition.X == 1)
            {
                return Direction.Up;
            }

            if (lastPosition.Y - currentPosition.Y == -1)
            {
                return Direction.Right;
            }

            return Direction.Left;
        }

        public void AddTower()
        {
            if (IsTowerPlaceable() && _shouldTowerBeRendered)
            {
                var tileOffset = new Vector2(BasicTower.TowerCenterX - WorldConstants.TileWidth / 2,
                                             BasicTower.TowerCenterY - WorldConstants.TileHeight / 4);

                var location = MouseHelper.Instance.TilePositionObject.TileCoords - tileOffset;
                
                TowerBase tower = new BasicTower(location, _basicTowerTextureId);

                _towersList.Add((int)MouseHelper.Instance.TilePositionObject.TilePosition.X * 100 + (int)MouseHelper.Instance.TilePositionObject.TilePosition.Y, tower);
            }
        }

        public void MoveUnits(double speedMultiplier)
        {
            foreach (var unit in Units)
            {
                CheckLocation(unit);
                unit.Move(speedMultiplier);
            }
        }

        public void SpawnWaves()
        {
            if (_wave.SpawnWave())
            {
                Units.Add(new BasicUnit(CoordsCalculator.CalculateLocationFromTilePosition(_pathList.First()), _basicUnitTextureId));
            }
        }

        public void DeleteDespawnedUnits()
        {
            Units.RemoveAll(u => u.IsDespawned);
        }

        internal void RenderGround()
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

        internal void RenderTowers()
        {
            foreach (var tower in _towersList)
            {
                tower.Value.Draw(Color.White);
            }
        }

        internal void RenderTowerToBePlaced(Vector2 currentOffset)
        {
            if (!_shouldTowerBeRendered || !_keyboardDevice[Key.T])
            {
                return;
            }

            _towerToBePlaced.Location = new Vector2(currentOffset.X + _mouseDevice.X - Game.NominalWidth / 2f - BasicTower.TowerCenterX,
                                                    -currentOffset.Y + _mouseDevice.Y - Game.NominalHeight / 2f - BasicTower.TowerCenterY);

            var towerColor = IsTowerPlaceable() ? WorldConstants.GreenColor : WorldConstants.RedColor;

            _towerToBePlaced.Draw(towerColor);
        }

        internal void RenderUnits()
        {
            foreach (var unit in Units)
            {
                if (!unit.IsDespawned)
                {
                    unit.Draw();
                }
            }
        }

        internal void RenderSelection()
        {
            if (_shouldTowerBeRendered && _keyboardDevice[Key.T])
            {
                var location = MouseHelper.Instance.TilePositionObject.TileCoords;

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
    }
}
