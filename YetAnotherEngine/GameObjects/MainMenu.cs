using System.Drawing;
using OpenTK.Graphics.OpenGL;
using YetAnotherEngine.Utils;

namespace YetAnotherEngine.GameObjects
{
    public class MainMenu
    {
        private const string MenuBackGroundPath = "Textures/Backgrounds/menu-background.jpg";
        private int _backgorundTexture;

        private readonly TextLine _textLine = new TextLine("big-outline.png");

        private const string MenuLine1 = "Game";
        private const string MenuLine2 = "Options";
        private const string MenuLine3 = "Exit";

        public MainMenu()
        {
            InitializeMenu();
        }

        private void InitializeMenu()
        {
            var backGroundTexture = new Bitmap(MenuBackGroundPath);
            _backgorundTexture = TextureLoader.GenerateTexture(backGroundTexture, 4270, 2135, 0, 0);

        }

        public void RenderMenu()
        {
            GL.BindTexture(TextureTarget.Texture2D, _backgorundTexture);
            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex2(-1.0f, -1.0f);
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex2(1.0f, -1.0f);
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex2(1.0f, 1.0f);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex2(-1.0f, 1.0f);
            GL.End();

            _textLine.WriteText(MenuLine1, 35, 0, 0);
            _textLine.WriteText(MenuLine2, 4, 500, 500);
            _textLine.WriteText(MenuLine3, 4, 700, 500);
        }

        public void SetUpMenuProjection()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1.0, 1.0, -1.0, 1.0, -1.0, 1.0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }
    }
}
