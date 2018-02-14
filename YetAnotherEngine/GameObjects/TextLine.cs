using YetAnotherEngine.OpenGL;
using YetAnotherEngine.Utils;

namespace YetAnotherEngine.GameObjects
{
    public class TextLine
    {
        private readonly TextureFont _textFont;

        public TextLine(string textureFileName)
        {
            _textFont = new TextureFont(TextUtil.CreateTextureFromFile($"Textures/Fonts/{textureFileName}"));
        }

        public void WriteFps(string text)
        {
            _textFont.Start();
            _textFont.WriteStringAtRelativePosition(text, 1.8, 20, 98, 0);
            _textFont.Stop();
        }

        public void WriteCoords(string text)
        {
            _textFont.Start();
            _textFont.WriteStringAtRelativePosition(text, 1.8, 20, 98, 0);
            _textFont.Stop();
        }

        public void WriteText(string text, double height, double x, double y)
        {
            _textFont.Start();
            _textFont.WriteStringAtAbsolutePosition(text, height, x, y);
            _textFont.Stop();
        }
    }
}
