using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

namespace YetAnotherEngine.Utils
{
    public class TextureLoader
    {
        public static int GenerateTexture(Bitmap textureImage, int width, int height, int offsetx, int offsety)
        {
            var glHandle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, glHandle);

            var bitmapData = textureImage.LockBits(new Rectangle(offsetx, offsety, width, height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width,
                bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);

            textureImage.UnlockBits(bitmapData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            return glHandle;
        }
    }
}
