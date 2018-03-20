using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using YetAnotherEngine.Enums;

namespace YetAnotherEngine.GameObjects.Drawables.Buttons
{
    class TowerButtons
    {
        private int _textureId;
        private int[] _towerTextures;
        private static TowerButtons _instance;
        public static TowerButtons GetInstance() => _instance ?? (_instance = new TowerButtons());

        private const float LeftMargin = 3;
        private const float BottomMargin = 3;

        private const float TopPadding = 1;
        private const float BottomPadding = 1;
        private const float LeftPadding = 2;
        private const float RightPadding = 2;

        private const float ButtonSize = 10;

        public bool FirstButtonSellected = true;
        public bool SecondButtonSellected;

        private TowerButtons()
        {
        }

        public void Init(int textureId, int[] towerTextures)
        {
            _textureId = textureId;
            _towerTextures = towerTextures;
            FirstButtonSellected = true;
            SecondButtonSellected = false;
        }
        
        public void Draw()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Ortho(0, 100, 0, 100, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Translate(LeftMargin, BottomMargin, 0);
            var aspectRatio = Game.CurrentHeight / Game.CurrentWidth;
            GL.Scale(aspectRatio * 1, 1, 1);
            GL.Rotate(0, 0, 0, 1);
            GL.PushMatrix();            

            GL.BindTexture(TextureTarget.Texture2D, _textureId);

            GL.Begin(PrimitiveType.Quads);

            GL.Color4(FirstButtonSellected ? Color.Green : Color.White);

            GL.TexCoord2(0, 0);
            GL.Vertex2(0, 0);
            GL.TexCoord2(1, 0);
            GL.Vertex2(ButtonSize, 0);
            GL.TexCoord2(1, 1);
            GL.Vertex2(ButtonSize, ButtonSize);
            GL.TexCoord2(0, 1);
            GL.Vertex2(0, ButtonSize);

            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, _towerTextures[0]);

            GL.Begin(PrimitiveType.Quads);

            GL.Color4(Color.White);

            GL.TexCoord2(0, 1);
            GL.Vertex2(LeftPadding, TopPadding);
            GL.TexCoord2(1, 1);
            GL.Vertex2(ButtonSize - RightPadding, 1);
            GL.TexCoord2(1, 0);
            GL.Vertex2(ButtonSize - RightPadding, ButtonSize - BottomPadding);
            GL.TexCoord2(0, 0);
            GL.Vertex2(LeftPadding, ButtonSize - BottomPadding);

            GL.End();

           // GL.Translate(LeftMargin + ButtonSize, 0, 0);

            GL.BindTexture(TextureTarget.Texture2D, _textureId);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(SecondButtonSellected ? Color.Green : Color.White);

            GL.TexCoord2(0, 0);
            GL.Vertex2(LeftMargin + ButtonSize, 0);
            GL.TexCoord2(1, 0);
            GL.Vertex2(ButtonSize*2 + LeftMargin, 0);
            GL.TexCoord2(1, 1);
            GL.Vertex2(ButtonSize*2 + LeftMargin, ButtonSize);
            GL.TexCoord2(0, 1);
            GL.Vertex2(LeftMargin + ButtonSize, ButtonSize);

            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, _towerTextures[1]);

            GL.Begin(PrimitiveType.Quads);

            GL.Color4(Color.White);

            GL.TexCoord2(0, 1);
            GL.Vertex2(LeftPadding + LeftMargin + ButtonSize, TopPadding);
            GL.TexCoord2(1, 1);
            GL.Vertex2(ButtonSize - RightPadding + LeftMargin + ButtonSize, 1);
            GL.TexCoord2(1, 0);
            GL.Vertex2(ButtonSize - RightPadding + LeftMargin + ButtonSize, ButtonSize - BottomPadding);
            GL.TexCoord2(0, 0);
            GL.Vertex2(LeftPadding + LeftMargin + ButtonSize, ButtonSize - BottomPadding);

            GL.End();

            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
        }

        public void IsMouseInside(Vector2 location)
        {
            if (Game.GameState != GameState.InGame) return;
            var aspectRatio = Game.CurrentHeight / Game.CurrentWidth;
            //if (location.X * aspectRatio * 100f / Game.CurrentHeight >= LeftMargin * aspectRatio && location.X *
            //    aspectRatio * 100f / Game.CurrentHeight <= LeftMargin * aspectRatio + ButtonSize * aspectRatio && 100 - 
            //    (location.Y * 100 / Game.CurrentHeight) >= BottomMargin && 100 - (location.Y * 100f /
            //    Game.CurrentHeight) <= BottomMargin + ButtonSize)
            if (location.X  * 100f / Game.CurrentWidth >= LeftMargin && location.X * 100f
                / Game.CurrentWidth <= LeftMargin + ButtonSize * aspectRatio && 100 - 
                (location.Y * 100 / Game.CurrentHeight) >= BottomMargin && 100 - (location.Y * 100f /
                Game.CurrentHeight) <= BottomMargin + ButtonSize)
            {
                FirstButtonSellected = true;
                SecondButtonSellected = false;
            }

            //else if (location.X * aspectRatio * 100f / Game.CurrentHeight >=
            //    LeftMargin * 2 * aspectRatio + ButtonSize * aspectRatio && location.X *
            //    aspectRatio * 100f / Game.CurrentHeight <= LeftMargin * 2 * aspectRatio + ButtonSize * 2 * aspectRatio
            //    && 100 - (location.Y * 100 / Game.CurrentHeight) >= BottomMargin && 100 - (location.Y * 100f /
            //    Game.CurrentHeight) <= BottomMargin + ButtonSize)
            else if (location.X * 100f / Game.CurrentWidth >=
                LeftMargin + LeftMargin * aspectRatio + ButtonSize * aspectRatio  && location.X *
                100f / Game.CurrentWidth <= LeftMargin + LeftMargin * aspectRatio + ButtonSize * 2 * aspectRatio &&
                100 - (location.Y * 100 / Game.CurrentHeight) >= BottomMargin && 100 - (location.Y * 100f /
                Game.CurrentHeight) <= BottomMargin + ButtonSize)
            {
                FirstButtonSellected = false;
                SecondButtonSellected = true;
            }    
        }
    }
}