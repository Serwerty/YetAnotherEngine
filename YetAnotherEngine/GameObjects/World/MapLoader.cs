using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using OpenTK;
using YetAnotherEngine.Constants;
using YetAnotherEngine.Enums;
using YetAnotherEngine.Utils;

namespace YetAnotherEngine.GameObjects.World
{
    //TODO: split path and textures responsibilities into two classes
    public class MapLoader
    {
        private const string DefaultMapPath = "Maps/map.dat";

        private readonly string _mapFilePath;
        private readonly int[] _mapTextures;

        private List<Vector2> _roadList;
        private ConstructionTile[,] _constructionTiles;


        public MapLoader(int[] mapTextures, string mapFilePath = DefaultMapPath)
        {
            _mapTextures = mapTextures;
            _mapFilePath = mapFilePath;
        }

        public ConstructionTile[,] ConstructionTiles
        {
            get
            {
                if (_constructionTiles == null)
                {
                    GetConstructionTiles();
                }
                return _constructionTiles;
            }
        }

        public List<Vector2> RoadList
        {
            get
            {
                if (_roadList == null)
                {
                    GetConstructionTiles();
                }
                return _roadList;
            }
        }

        private void GetConstructionTiles()
        {
            _constructionTiles = new ConstructionTile[WorldConstants.WorldHeight, WorldConstants.WorldWidth];
            _roadList = new List<Vector2>();

            for (var i = 0; i < WorldConstants.WorldHeight; i++)
            {
                for (var j = 0; j < WorldConstants.WorldWidth; j++)
                {
                    _constructionTiles[i, j] = new ConstructionTile();
                }
            }

            try
            {
                using (var map = new StreamReader(_mapFilePath))
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

                        _constructionTiles[i, j].TextureOffsetX = textureOffsetX;
                        _constructionTiles[i, j].TextureOffsetY = textureOffsetY;
                        _constructionTiles[i, j].Type = type;

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

            for (var i = 0; i < WorldConstants.WorldHeight; i++)
            {
                for (var j = 0; j < WorldConstants.WorldWidth; j++)
                {
                    switch (_constructionTiles[i, j].Type)
                    {
                        case TileType.Road:
                            _constructionTiles[i, j].TextureId = _mapTextures[1];
                            break;
                        case TileType.Tower:
                            _constructionTiles[i, j].TextureId = _mapTextures[0];
                            break;
                    }
                }
            }
        }

        private List<Vector2> CalculatePath()
        {
            var path = new List<Vector2>();
            var lastRoadPosition = new Vector2(-1, -1);
            var lastDirection = Direction.Down;
            var notFirstTile = false;
            foreach (var roadPostion in RoadList)
            {
                if (lastRoadPosition.X != -1)
                {
                    if (notFirstTile)
                    {
                        var direction = GetDirection(lastRoadPosition, roadPostion);

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
            path.Add(RoadList.Last());
            return path;
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
    }
}
