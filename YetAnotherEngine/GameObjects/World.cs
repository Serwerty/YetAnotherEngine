using System;
using System.Collections.Generic;
using System.Drawing;
using YetAnotherEngine.Utils;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using OpenTK.Input;
using YetAnotherEngine.Constants;
using YetAnotherEngine.GameObjects.Towers;

namespace YetAnotherEngine.GameObjects
{
    public class World
    {
        private const int WorldWidth = Constants.WorldConstants.WorldWidth;
        private const int WorldHeight = Constants.WorldConstants.WorldHeight;

        private const string GroundTileFilePath = "Textures/Tiles/terrain_tile.png";
        private const string TowersTileFilePath = "Textures/Tiles/towers.png";
        private const string SelectionTileFilePath = "Textures/Selection.png";

        private readonly int[] _groundTextures = new int[5]; // for now it wil be one(random) of 5
        private readonly int[,] _groundTexturesMap = new int[WorldHeight, WorldWidth];

        private int _basicTowerTextureID;
        private int _selectionTextureID;
        private List<TowerBase> _towersList;

        private MouseDevice _mouseDevice;
        private KeyboardDevice _keyboardDevice;

        private TowerBase _towerToBePlaced;
        private bool _isTowerShouldBeRendered = true;

        public World(MouseDevice mouseDevice, KeyboardDevice keyboardDevice)
        {
            _mouseDevice = mouseDevice;
            _keyboardDevice = keyboardDevice;
            LoadMapTextures();
            _towersList = new List<TowerBase>();
            TowerBase _tower = new BasicTower(new Vector2(67+64*4, 62+32*4), _basicTowerTextureID);
            _towerToBePlaced = new BasicTower(new Vector2(_mouseDevice.X,_mouseDevice.Y), _basicTowerTextureID);
            _towersList.Add(_tower);
        }

        public void AddTower(MouseDevice mouseDevice)
        {
            TowerBase _tower = new BasicTower(new Vector2(mouseDevice.X, mouseDevice.Y), _basicTowerTextureID);
            _towersList.Add(_tower);
        }

        public void RenderGround()
        {
            for (var i = 0; i < WorldHeight; i++)
            {
                var globalOffsetX = i * 32 + WorldWidth*32;
                var globalOffsetY = i * 16;

                for (var j = 0; j < WorldWidth; j++)
                {
                    var location = new Vector2(globalOffsetX, globalOffsetY);
                    GL.BindTexture(TextureTarget.Texture2D, _groundTexturesMap[i, j]);

                    GL.Begin(PrimitiveType.Quads);
                    GL.Color4(Color.White);

                    GL.TexCoord2(0, 0);
                    GL.Vertex2(location);
                    GL.TexCoord2(1, 0);
                    GL.Vertex2(location.X + 64, location.Y);
                    GL.TexCoord2(1, 1);
                    GL.Vertex2(location.X + 64, location.Y + 64);
                    GL.TexCoord2(0, 1);
                    GL.Vertex2(location.X, location.Y + 64);

                    GL.End();

                    globalOffsetX -= 32;
                    globalOffsetY += 16;
                }
            }
        }

        private void LoadMapTextures()
        {
            var backGroundTexture = new Bitmap(GroundTileFilePath);
            _groundTextures[0] = TextureLoader.GenerateTexture(backGroundTexture, Constants.WorldConstants.TileWidth, 
                                 Constants.WorldConstants.TileWidth, 64, 64);
            _groundTextures[1] = TextureLoader.GenerateTexture(backGroundTexture, Constants.WorldConstants.TileWidth,
                                 Constants.WorldConstants.TileWidth, 64, 64);
            _groundTextures[2] = TextureLoader.GenerateTexture(backGroundTexture, Constants.WorldConstants.TileWidth,
                                 Constants.WorldConstants.TileWidth, 64, 64);
            _groundTextures[3] = TextureLoader.GenerateTexture(backGroundTexture, Constants.WorldConstants.TileWidth,
                                 Constants.WorldConstants.TileWidth, 64, 64);
            _groundTextures[4] = TextureLoader.GenerateTexture(backGroundTexture, Constants.WorldConstants.TileWidth,
                                 Constants.WorldConstants.TileWidth, 64, 64);

            var towerTexture = new Bitmap(TowersTileFilePath);
            _basicTowerTextureID = TextureLoader.GenerateTexture(towerTexture, BasicTower.TowerWidth, BasicTower.TowerHeight, BasicTower.TextureOffsetX, BasicTower.TextureOffsetY);

            var selectionTexture = new Bitmap(SelectionTileFilePath);
            _selectionTextureID = TextureLoader.GenerateTexture(selectionTexture, 64, 64, 0, 0);


            for (var i = 0; i < WorldHeight; i++)
            {
                for (var j = 0; j <  WorldWidth; j++)
                {
                    var random = new Random(unchecked((int)DateTime.Now.Ticks));
                    _groundTexturesMap[i, j] = _groundTextures[random.Next(0, 5)];
                }
            }
        }

        internal void RenderTowers()
        {
            foreach (var tower in _towersList)
            {
                tower.Draw(Color.White);
            }
           
        }

        internal void RenderTowerToBePlaced(Vector2 currentOffset)
        {
            if (_isTowerShouldBeRendered)
            {
                
                if (_keyboardDevice[Key.T])
                {
                    _towerToBePlaced.Location = new Vector2(currentOffset.X + _mouseDevice.X - Game.NominalWidth / 2 - BasicTower.TowerCenterX,
                    -currentOffset.Y + _mouseDevice.Y - Game.NominalHeight / 2 - BasicTower.TowerCenterY);

                    var roughLocation = new Vector2(currentOffset.X + _mouseDevice.X - Game.NominalWidth / 2,
                      -currentOffset.Y + _mouseDevice.Y - Game.NominalHeight / 2);

                    roughLocation.X -= roughLocation.X % 64;
                    roughLocation.Y -= roughLocation.Y % 32;

                    Vector2 location = getSelectionPosition(currentOffset, roughLocation);


                    if (CheckIfTowerCanBePlaced(location))
                        _towerToBePlaced.Draw(WorldConstants.GreenColor);
                    else
                        _towerToBePlaced.Draw(WorldConstants.RedColor);
                }
            }
        }

        internal void RenderSelection(Vector2 currentOffset)
        {
            if (_isTowerShouldBeRendered)
            {
                if (_keyboardDevice[Key.T])
                {
                    var roughLocation = new Vector2(currentOffset.X + _mouseDevice.X - Game.NominalWidth / 2,
                        -currentOffset.Y + _mouseDevice.Y - Game.NominalHeight / 2);

                    roughLocation.X -= roughLocation.X % 64;
                    roughLocation.Y -= roughLocation.Y % 32;

                    Vector2 location = getSelectionPosition(currentOffset, roughLocation);
                
                    GL.BindTexture(TextureTarget.Texture2D, _selectionTextureID);

                    GL.Begin(PrimitiveType.Quads);
                    GL.Color4(Color.White);

                    GL.TexCoord2(0, 0);
                    GL.Vertex2(location);
                    GL.TexCoord2(1, 0);
                    GL.Vertex2(location.X + 64, location.Y);
                    GL.TexCoord2(1, 1);
                    GL.Vertex2(location.X + 64, location.Y + 64);
                    GL.TexCoord2(0, 1);
                    GL.Vertex2(location.X, location.Y + 64);

                    GL.End();
                }
            }
        }

        private bool CheckIfTowerCanBePlaced(Vector2 corner)
        {
            bool canBePlaced = true;
            int i, j;
            j = (int)((2*corner.Y-corner.X - WorldWidth * 32)/64);
            i = (int) ((corner.Y - 16 * j) / 16);
            j += WorldWidth;
            i -= WorldHeight;
           // Game._fpsText.WriteFps(i + ":" + j);
            if (i < 0 || i >= WorldHeight) canBePlaced = false;
            if (j < 0 || j >= WorldWidth) canBePlaced = false;
            return canBePlaced;
        }

        private Vector2 getSelectionPosition(Vector2 currentOffset, Vector2 corner)
        {
            Vector2 A = corner + new Vector2(0, 16);
            Vector2 B = corner + new Vector2(32, 32);
            Vector2 C = corner + new Vector2(64, 16);
            Vector2 D = corner + new Vector2(32, 0);

            Vector2 Q = corner + new Vector2(32,16);           // center point
            int a = 32;     // half-width (in the x-direction)
            int b = 16;     // half-height (y-direction)
            Vector2 U = (C - A) / (2 * a);         // unit vector in x-direction
            Vector2 V = (D - B) / (2 * b);         // unit vector in y-direction

            Vector2 P = new Vector2(currentOffset.X + _mouseDevice.X - Game.NominalWidth / 2, 
                -currentOffset.Y + _mouseDevice.Y - Game.NominalHeight / 2);

            Vector2 W = P - Q;
            float xabs = (W * U).Length;
            float yabs = (W * V).Length;
            if (xabs / a + yabs / b <= 1)
                return corner;
            if (W.X > 0)
            {
                if (W.Y > 0)
                    return corner + new Vector2(32, 16);
                return corner + new Vector2(32, -16);
            }
                if (W.Y > 0)
                    return corner + new Vector2(-32, 16);
                return corner + new Vector2(-32, -16);
        }

    }
}
