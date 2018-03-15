using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using YetAnotherEngine.Constants;

namespace YetAnotherEngine.GameObjects.Drawables.Buttons
{
    class TowerButton
    {
        public int TextureId;

        public TowerButton(int textureId)
        {
            TextureId = textureId;
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
            GL.Translate(5,5, 0);
            var aspectRatio = ComputeAspectRatio();
            GL.Scale(aspectRatio * 1, 1, 1);
            GL.Rotate(0, 0, 0, 1);
            GL.PushMatrix();
  
            GL.BindTexture(TextureTarget.Texture2D, TextureId);

            GL.Begin(PrimitiveType.Quads);
            GL.Color4(Color.White);

            GL.TexCoord2(0, 0);
            GL.Vertex2(0, 0);
            GL.TexCoord2(1, 0);
            GL.Vertex2(10, 0);
            GL.TexCoord2(1, 1);
            GL.Vertex2(10,10);
            GL.TexCoord2(0, 1);
            GL.Vertex2(0,10);

            GL.End();

            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
        }

        private static double ComputeAspectRatio()
        {
            var viewport = new int[4];
            GL.GetInteger(GetPName.Viewport, viewport);
            var w = viewport[2];
            var h = viewport[3];
            double aspectRatio = h / (float)w;
            return aspectRatio;
        }
    }
}
