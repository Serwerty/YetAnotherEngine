
using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using YetAnotherEngine.Constants;
using YetAnotherEngine.Enums;
using YetAnotherEngine.Utils;
using YetAnotherEngine.Utils.Helpers;

namespace YetAnotherEngine.GameObjects.Drawables.Buttons
{
    class TextButton
    {
        private readonly int _textureId;
        private const string ButtonTileFilePath = "Textures/Buttons/TowerButton.png";

        private readonly float _leftMargin;
        private readonly float _bottomMargin;

        private const float TextSize = 3;

        private readonly string _text;

        private readonly float _buttonWidth;
        private readonly float _buttonHeight;

        //gameState in witch button shoud be clickable;
        private readonly GameState _activeGameState;

        public event EventHandler OnClick;

        public TextButton(string text, float leftMargin, float bottomMargin, float buttonWidth, float buttonHeight, GameState activeGameState)
        {
            _text = text;
            _leftMargin = leftMargin;
            _bottomMargin = bottomMargin;
            _buttonWidth = buttonWidth;
            _buttonHeight = buttonHeight;
            _activeGameState = activeGameState;
            var buttonTexture = new Bitmap(ButtonTileFilePath);
            _textureId = TextureLoader.GenerateTexture(buttonTexture, 124, 124, 0, 0);
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
            GL.Translate(_leftMargin, _bottomMargin, 0);
            var aspectRatio = Game.CurrentHeight / Game.CurrentWidth;
            GL.Scale(aspectRatio * 1, 1, 1);
            GL.Rotate(0, 0, 0, 1);
            GL.PushMatrix();

            GL.BindTexture(TextureTarget.Texture2D, _textureId);

            GL.Begin(PrimitiveType.Quads);

            GL.Color4(Color.White);

            GL.TexCoord2(0, 0);
            GL.Vertex2(0, 0);
            GL.TexCoord2(1, 0);
            GL.Vertex2(_buttonWidth, 0);
            GL.TexCoord2(1, 1);
            GL.Vertex2(_buttonWidth, _buttonHeight);
            GL.TexCoord2(0, 1);
            GL.Vertex2(0, _buttonHeight);

            GL.End();

            double textWidth = _text.Length * aspectRatio * TextConstants.TextSizeCoefficient * TextSize;

            TextLine.Instane().WriteTextAtRelativePosition(_text, TextSize,
                _leftMargin + (_buttonWidth * aspectRatio - textWidth - 0.4 * aspectRatio) / 2, _bottomMargin + (_buttonHeight - TextSize * 0.1) / 2);

        }

        public bool IsMouseInside(Vector2 location)
        {
            if (Game.GameState != _activeGameState) return false;
            var aspectRatio = Game.CurrentHeight / Game.CurrentWidth;
            return (location.X * 100f / Game.CurrentWidth >= _leftMargin && location.X * 100f
                    / Game.CurrentWidth <= _leftMargin + _buttonWidth * aspectRatio && 100 -
                    (location.Y * 100 / Game.CurrentHeight) >= _bottomMargin && 100 - (location.Y * 100f /
                    Game.CurrentHeight) <= _bottomMargin + _buttonHeight);
        }

        internal virtual void Click(object sender)
        {
            OnClick?.Invoke(sender, EventArgs.Empty);
        }
    }
}
