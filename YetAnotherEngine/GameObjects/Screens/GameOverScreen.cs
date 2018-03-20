using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using YetAnotherEngine.Constants;
using YetAnotherEngine.Enums;
using YetAnotherEngine.GameObjects.Drawables.Buttons;
using YetAnotherEngine.GameObjects.World;
using YetAnotherEngine.Utils;

namespace YetAnotherEngine.GameObjects.Screens
{
    class GameOverScreen
    {
        private const string GameOverBackGroundPath = "Textures/Backgrounds/menu-background.jpg";
        private int _backgorundTexture;
        private TextButton _startNewGameButton;

        private string _gameOverString = "Game Over";
        private const int GameOverTextSize = 10;
        private const int StatsTextSize = 4;
        private readonly Camera _camera;

        public GameOverScreen(Camera camera)
        {
            _camera = camera;
            InitializeScreen();
        }

        private void InitializeScreen()
        {
            var backGroundTexture = new Bitmap(GameOverBackGroundPath);
            _backgorundTexture = TextureLoader.GenerateTexture(backGroundTexture, 4270, 2135, 0, 0);
            double aspectRatio = Game.CurrentHeight / Game.CurrentWidth;
            _startNewGameButton = new TextButton("New Game", (100 - 20 * (float) aspectRatio) / 2, 30, 20, 10,
                GameState.InGameOverScreen);
            _startNewGameButton.OnClick += StartNewGameButtonOnClick;
            ButtonsManager.GetInstance().AddButton(_startNewGameButton);
        }

        private void StartNewGameButtonOnClick(object sender, EventArgs eventArgs)
        {
            GameWorld.GetInstance().StartNewGame(_camera);
        }

        public void RenderScreen()
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

            GL.Color4(Color.White);

            double aspectRatio = Game.CurrentHeight / Game.CurrentWidth;

            double gameOverTextWidth = _gameOverString.Length * aspectRatio * GameOverTextSize *
                                       TextConstants.TextSizeCoefficient;
            TextLine.Instane()
                .WriteTextAtRelativePosition(_gameOverString, GameOverTextSize, (100 - gameOverTextWidth) / 2, 80);

            string goldEarnedString = $"Gold Earned: {GameStatistic.GetInstance().GoldEarned}";
            double goldEarnedTextWidth = goldEarnedString.Length * aspectRatio * StatsTextSize *
                                         TextConstants.TextSizeCoefficient;
            TextLine.Instane()
                .WriteTextAtRelativePosition(goldEarnedString, StatsTextSize, (100 - goldEarnedTextWidth) / 2, 60);

            string unitsKilledString = $"Units Killed: {GameStatistic.GetInstance().UnitsKiled}";
            double unitsKilledTextWidth = unitsKilledString.Length * aspectRatio * StatsTextSize *
                                          TextConstants.TextSizeCoefficient;
            TextLine.Instane()
                .WriteTextAtRelativePosition(unitsKilledString, StatsTextSize, (100 - unitsKilledTextWidth) / 2, 55);

            string damageDealtString = $"Damage Dealt: {GameStatistic.GetInstance().DamageDealtValue}";
            double damageDealtTextWidth = damageDealtString.Length * aspectRatio * StatsTextSize *
                                          TextConstants.TextSizeCoefficient;
            TextLine.Instane()
                .WriteTextAtRelativePosition(damageDealtString, StatsTextSize, (100 - damageDealtTextWidth) / 2, 50);


            RenderButtons();
        }

        public void RenderButtons()
        {
            _startNewGameButton.Draw();
        }

        public void SetUpScreenProjection()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1.0, 1.0, -1.0, 1.0, -1.0, 1.0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }
    }
}