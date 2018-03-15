using YetAnotherEngine.Constants;
using YetAnotherEngine.OpenGL;
using YetAnotherEngine.Utils;

namespace YetAnotherEngine.GameObjects
{
    public class TextLine
    {
        private static TextLine _instance;

        public static TextLine Instane() => _instance ?? (_instance = new TextLine());
 
        private readonly TextureFont _textFont;

        private TextLine()
        {
            _textFont = new TextureFont(TextUtil.CreateTextureFromFile($"Textures/Fonts/{WorldConstants.FontTextureName}"));
        }

        public void WriteFps(string text)
        {
            _textFont.Start();
            _textFont.WriteStringAtRelativePosition(text, 1.8, 2, 99, 0);
            _textFont.Stop();
        }

        public void WriteCoords(string text)
        {
            _textFont.Start();
            _textFont.WriteStringAtRelativePosition(text, 1.8, 2, 97, 0);
            _textFont.Stop();
        }

        public void WriteTilePosition(string text)
        {
            _textFont.Start();
            _textFont.WriteStringAtRelativePosition(text, 1.8, 2, 95, 0);
            _textFont.Stop();
        }

        public void WriteLogText(string text)
        {
            _textFont.Start();
            _textFont.WriteStringAtRelativePosition(text, 1.8, 2, 93, 0);
            _textFont.Stop();
        }

        public void WriteText(string text, double height, double x, double y)
        {
            _textFont.Start();
            _textFont.WriteStringAtAbsolutePosition(text, height, x, y);
            _textFont.Stop();
        }


        public void WriteGold(string text)
        {
            _textFont.Start();
            _textFont.WriteStringAtRelativePosition(text, 1.8, 90, 99, 0);
            _textFont.Stop();
        }
    }
}