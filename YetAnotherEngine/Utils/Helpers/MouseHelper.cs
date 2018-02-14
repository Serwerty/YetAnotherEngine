using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using YetAnotherEngine.GameObjects;
using YetAnotherEngine.Utils.Models;

namespace YetAnotherEngine.Utils.Helpers
{
    public class MouseHelper
    {
        private static MouseHelper _instance;
        private MouseDevice _mouseDevice;
        private Camera _camera;
        private TilePositionObject _tilePositionObject;

        private const string FpsTextFont = "big-outline.png";
        private TextLine _textLineCoords = new TextLine(FpsTextFont);

        private MouseHelper() { }


        public static MouseHelper Instance => _instance ?? (_instance = new MouseHelper());

        public TilePositionObject TilePositionObject => _tilePositionObject;

        //public Vector2 MousePosition => new Vector2(_mouseDevice.X / 3f * 2f, _mouseDevice.Y / 3f * 2f);

        public void Init(MouseDevice mouseDevice, Camera camera)
        {
            _mouseDevice = mouseDevice;
            _camera = camera;
        }

        public void Calculate()
        {
            _tilePositionObject = new TilePositionObject(Vector2.Divide(_camera.GetPosition(),(float)Game.zScale), new Vector2(_mouseDevice.X * Game.MultiplierWidth, _mouseDevice.Y  * Game.MultiplierHeight));
        }

        public void DrawCoords()
        {
            GL.Color4(Color.White);
            _textLineCoords.WriteFps($"mouse X: {_mouseDevice.X:0} mouse Y: {_mouseDevice.Y:0}");
        }
    }
}
