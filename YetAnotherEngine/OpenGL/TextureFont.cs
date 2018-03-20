using OpenTK.Graphics.OpenGL;
using YetAnotherEngine.Utils;

namespace YetAnotherEngine.OpenGL
{
    public class TextureFont
    {
        private readonly int _textureId;
        private const double Sixteenth = 1.0 / 16.0;
        public double AdvanceWidth = 0.75;
        public double CharacterBoundingBoxWidth = 0.8;
        public double CharacterBoundingBoxHeight = 0.8;

        public TextureFont(int textureId)
        {
            _textureId = textureId;
        }

        public void Start()
        {
            GL.BindTexture(TextureTarget.Texture2D, _textureId);
            TextUtil.SetParameters();
        }

        public void Stop()
        {
            TextUtil.UnsetParameters();
        }

        public void WriteStringAtAbsolutePosition(string text, double height, double x, double y)
        {
            GL.PushMatrix();
            double width = ComputeWidth(text);
            GL.Translate(x - width, y - 0.5, 0);
            GL.Scale(height, -height, height);
            GL.Begin(PrimitiveType.Quads);
            double xpos = 0;
            foreach (var ch in text)
            {
                WriteCharacter(ch, xpos);
                xpos += AdvanceWidth;
            }
            GL.End();
            GL.PopMatrix();
        }

        public void WriteString(string text)
        {
            GL.PushMatrix();
            //var width = ComputeWidth(text);
            GL.Translate(0, -0.5, 0);
            GL.Begin(PrimitiveType.Quads);
            double xpos = 0;
            foreach (var ch in text)
            {
                WriteCharacter(ch, xpos);
                xpos += AdvanceWidth;
            }
            GL.End();
            GL.PopMatrix();
        }

        public double ComputeWidth(string text)
        {
            return text.Length * AdvanceWidth;
        }

        public void WriteStringAtRelativePosition(string text, double heightPercent, double xPercent, double yPercent, double degreesCounterClockwise)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Ortho(0, 100, 0, 100, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Translate(xPercent, yPercent, 0);
            var aspectRatio = Game.CurrentHeight / Game.CurrentWidth;
            GL.Scale(aspectRatio * heightPercent, heightPercent, heightPercent);
            GL.Rotate(degreesCounterClockwise, 0, 0, 1);
            WriteString(text);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
        }

        private void WriteCharacter(char ch, double xpos)
        {
            byte ascii;
            unchecked { ascii = (byte)ch; }

            var row = ascii >> 4;
            var col = ascii & 0x0F;

            var centerx = (col + 0.5) * Sixteenth;
            var centery = (row + 0.5) * Sixteenth;
            var halfHeight = CharacterBoundingBoxHeight * Sixteenth / 2.0;
            var halfWidth = CharacterBoundingBoxWidth * Sixteenth / 2.0;
            var left = centerx - halfWidth;
            var right = centerx + halfWidth;
            var top = centery - halfHeight;
            var bottom = centery + halfHeight;

            GL.TexCoord2(left, top); GL.Vertex2(xpos, 1);
            GL.TexCoord2(right, top); GL.Vertex2(xpos + 1, 1);
            GL.TexCoord2(right, bottom); GL.Vertex2(xpos + 1, 0);
            GL.TexCoord2(left, bottom); GL.Vertex2(xpos, 0);
        }
    }
}
