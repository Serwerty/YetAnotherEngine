using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using YetAnotherEngine.Utils.Helpers;

namespace YetAnotherEngine.GameObjects.Drawables.Buttons
{
    class TowerButton
    {
        private int _textureId;
        private int[] _towerTextures;
        private static TowerButton _instance;
        public static TowerButton GetInstance() => _instance ?? (_instance = new TowerButton());

        private TowerButton()
        {
        }

        public void Init(int textureId, int[] towerTextures)
        {
            _textureId = textureId;
            _towerTextures = towerTextures;
        }

        public bool FirstButtonSellected = true;
        public bool SecondButtonSellected = false;

        public void Draw()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Ortho(0, 100, 0, 100, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();
            var aspectRatio = ComputeAspectRatio();
            GL.Scale(aspectRatio * 1, 1, 1);
            GL.Rotate(0, 0, 0, 1);
            GL.PushMatrix();
            GL.Translate(5, 5, 0);

            GL.BindTexture(TextureTarget.Texture2D, _textureId);

            GL.Begin(PrimitiveType.Quads);

            GL.Color4(FirstButtonSellected ? Color.Green : Color.White);

            GL.TexCoord2(0, 0);
            GL.Vertex2(0, 0);
            GL.TexCoord2(1, 0);
            GL.Vertex2(10, 0);
            GL.TexCoord2(1, 1);
            GL.Vertex2(10, 10);
            GL.TexCoord2(0, 1);
            GL.Vertex2(0, 10);

            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, _towerTextures[0]);

            GL.Begin(PrimitiveType.Quads);

            GL.Color4(Color.White);

            GL.TexCoord2(0, 1);
            GL.Vertex2(2, 1);
            GL.TexCoord2(1, 1);
            GL.Vertex2(8, 1);
            GL.TexCoord2(1, 0);
            GL.Vertex2(8, 9);
            GL.TexCoord2(0, 0);
            GL.Vertex2(2, 9);

            GL.End();

            GL.Translate(15, 0, 0);

            GL.BindTexture(TextureTarget.Texture2D, _textureId);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(SecondButtonSellected ? Color.Green : Color.White);

            GL.TexCoord2(0, 0);
            GL.Vertex2(0, 0);
            GL.TexCoord2(1, 0);
            GL.Vertex2(10, 0);
            GL.TexCoord2(1, 1);
            GL.Vertex2(10, 10);
            GL.TexCoord2(0, 1);
            GL.Vertex2(0, 10);

            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, _towerTextures[1]);

            GL.Begin(PrimitiveType.Quads);

            GL.Color4(Color.White);

            GL.TexCoord2(0, 1);
            GL.Vertex2(2, 1);
            GL.TexCoord2(1, 1);
            GL.Vertex2(8, 1);
            GL.TexCoord2(1, 0);
            GL.Vertex2(8, 9);
            GL.TexCoord2(0, 0);
            GL.Vertex2(2, 9);

            GL.End();

            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
        }

        private static double ComputeAspectRatio()
        {
            var viewport = new int[4];
            GL.GetInteger(GetPName.Viewport, viewport);
            var w = viewport[2];
            var h = viewport[3];
            double aspectRatio = h / (float) w;
            return aspectRatio;
        }

        public void IsMouseInside(Vector2 location)
        {
            var aspectRatio = ComputeAspectRatio();
            if (location.X * aspectRatio * 100f / Game.CurrentHeight >= 5 * aspectRatio && location.X * aspectRatio *
                                                                                        100f / Game.CurrentHeight <=
                                                                                        15 * aspectRatio
                                                                                        && 100 - (location.Y * 100 /
                                                                                                  Game.CurrentHeight) >=
                                                                                        5 && 100 -
                                                                                        (location.Y * 100f /
                                                                                         Game.CurrentHeight) <=
                                                                                        15)
            {
                FirstButtonSellected = true;
                SecondButtonSellected = false;
            }

            else if (location.X * aspectRatio * 100f / Game.CurrentHeight >= 20 * aspectRatio && location.X *
                                                                                              aspectRatio * 100f /
                                                                                              Game.CurrentHeight <=
                                                                                              30 * aspectRatio
                                                                                              && 100 - (location.Y *
                                                                                                        100 / Game
                                                                                                            .CurrentHeight
                                                                                              ) >=
                                                                                              5 && 100 -
                                                                                              (location.Y * 100f /
                                                                                               Game.CurrentHeight) <=
                                                                                              15)
            {
                FirstButtonSellected = false;
                SecondButtonSellected = true;
            }
        }
    }
}