using OpenTK.Graphics.OpenGL;
using System.Drawing;
using YetAnotherEngine.GameObjects;

namespace YetAnotherEngine.Utils.Helpers
{
    public class FpsHelper
    {
        private float _avgFps;
        private float _avgCnt;
        private static FpsHelper _instance;

        public static FpsHelper Instance => _instance ?? (_instance = new FpsHelper());

        private FpsHelper()
        {
        }

        public void DrawFpsText(double elapsedTime)
        {
            GL.Color4(Color.White);
            var curFps = (float) (1.0 / elapsedTime);
            if (_avgCnt <= 10.0F)
            {
                _avgFps = curFps;
            }
            else
            {
                _avgFps += (curFps - _avgFps) / _avgCnt;
            }

            _avgCnt++;
             TextLine.Instane().WriteFps($"FPS average: {_avgFps:0} FPS current: {curFps:0}");
        }
    }
}