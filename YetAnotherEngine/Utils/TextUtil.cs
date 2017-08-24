using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;

namespace YetAnotherEngine.Utils
{
    public class TextUtil
    {
        public static int CreateRgbTexture(int width, int height, byte[] rgb)
        {
            return CreateTexture(width, height, false, rgb);
        }

        public static int CreateRgbaTexture(int width, int height, byte[] rgba)
        {
            return CreateTexture(width, height, true, rgba);
        }

        public static int CreateTextureFromBitmap(Bitmap bitmap)
        {
            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var texture = GenerateTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);
            return texture;
        }

        public static int CreateTextureFromFile(string path)
        {
            return CreateTextureFromBitmap(new Bitmap(Image.FromFile(path)));
        }

        private static int CreateTexture(int width, int height, bool alpha, byte[] bytes)
        {
            var texture = GenerateTexture();
            Upload(width, height, alpha, bytes);
            return texture;
        }

        private static int GenerateTexture()
        {
            var texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);
            return texture;
        }

        internal static void SetParameters()
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        }

        internal static void UnsetParameters()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private static void Upload(int width, int height, bool alpha, byte[] bytes)
        {
            var internalFormat = alpha ? PixelInternalFormat.Rgba : PixelInternalFormat.Rgb;
            var format = alpha ? OpenTK.Graphics.OpenGL.PixelFormat.Rgba : OpenTK.Graphics.OpenGL.PixelFormat.Rgb;
            GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, width, height, 0, format, PixelType.UnsignedByte, bytes);
        }
    }
}
