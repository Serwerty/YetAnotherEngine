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

        public void WriteText(string text)
        {
            _textFont.Start();
            _textFont.WriteStringAtAbsolutePosition(text, 15, 0, 0);
            _textFont.Stop();
        }
    }
}
