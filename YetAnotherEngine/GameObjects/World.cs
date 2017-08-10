using System;
using System.Drawing;
using YetAnotherEngine.Utils;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace YetAnotherEngine.GameObjects
{
    public class World
    {
        private const int WorldWidth = Constants.WorldConstants.WorldWidth;
        private const int WorldHeight = Constants.WorldConstants.WorldHeight;

        private const string GroundTileFilePath = "Textures/Tiles/terrain_tile.png";

        private readonly int[] _groundTextures = new int[5]; // for now it wil be one(random) of 5
        private readonly int[,] _groundTexturesMap = new int[WorldHeight, WorldWidth];

        public World()
        {
            LoadMapTextures();
        }

        public void RenderGround()
        {
            for (var i = 0; i < WorldHeight; i++)
            {
                var globalOffsetX = i * 32;
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

            for (var i = 0; i < WorldHeight; i++)
            {
                for (var j = 0; j <  WorldWidth; j++)
                {
                    var random = new Random(unchecked((int)DateTime.Now.Ticks));
                    _groundTexturesMap[i, j] = _groundTextures[random.Next(0, 5)];
                }
            }
        }
    }
}
