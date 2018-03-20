using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using YetAnotherEngine.Enums;
using YetAnotherEngine.GameObjects.Drawables.Buttons;
using YetAnotherEngine.GameObjects.World;
using YetAnotherEngine.Utils;

namespace YetAnotherEngine.GameObjects.Screens
{
    //TODO: implement, refactor, shit
    public class MainMenu
    {
        private const string MenuBackGroundPath = "Textures/Backgrounds/menu-background.jpg";
        private int _backgorundTexture;

        private const string MenuLine1 = "New Game";
        private const string MenuLine2 = "Options";
        private const string MenuLine3 = "Exit";

        private TextButton _startNewGameButton;
        private TextButton _optionsButton;
        private TextButton _exitButton;

        private const int ButtonsWidth = 30;
        private const int ButtonsHeight = 15;

        private readonly Camera _camera;

        public MainMenu(Camera camera)
        {
            _camera = camera;
            InitializeMenu();
        }

        private void InitializeMenu()
        {
            var backGroundTexture = new Bitmap(MenuBackGroundPath);
            _backgorundTexture = TextureLoader.GenerateTexture(backGroundTexture, 4270, 2135, 0, 0);

            double aspectRatio = Game.CurrentHeight / Game.CurrentWidth;

            _startNewGameButton = new TextButton(MenuLine1, (100 - ButtonsWidth * (float)aspectRatio) / 2, 60, ButtonsWidth, ButtonsHeight,
                GameState.InMainMenu);
            _startNewGameButton.OnClick += StartNewGameButtonOnClick;
            ButtonsManager.GetInstance().AddButton(_startNewGameButton);

            _optionsButton = new TextButton(MenuLine2, (100 - ButtonsWidth * (float)aspectRatio) / 2, 44, ButtonsWidth, ButtonsHeight,
                GameState.InMainMenu);
            _optionsButton.OnClick += OptionsButtonOnClick;
            ButtonsManager.GetInstance().AddButton(_optionsButton);

            _exitButton = new TextButton(MenuLine3, (100 - ButtonsWidth * (float)aspectRatio) / 2, 28, ButtonsWidth, ButtonsHeight,
                GameState.InMainMenu);
            _exitButton.OnClick += ExitButtonOnClick;
            ButtonsManager.GetInstance().AddButton(_exitButton);

        }

        private void ExitButtonOnClick(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void OptionsButtonOnClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void StartNewGameButtonOnClick(object sender, EventArgs e)
        {
            GameWorld.GetInstance().StartNewGame(_camera);
        }

        public void RenderMenu()
        {
            GL.BindTexture(TextureTarget.Texture2D, _backgorundTexture);
            GL.Color4(Color.White);
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

            RenderButtons();
        }

        public void RenderButtons()
        {
            _startNewGameButton.Draw();
            _optionsButton.Draw();
            _exitButton.Draw();
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