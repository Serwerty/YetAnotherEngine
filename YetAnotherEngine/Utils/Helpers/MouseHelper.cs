using OpenTK;
using OpenTK.Input;
using YetAnotherEngine.GameObjects;
using YetAnotherEngine.Utils.Models;

namespace YetAnotherEngine.Utils
{
    public class MouseHelper
    {
        private static MouseHelper _instance;
        private MouseDevice _mouseDevice;
        private Camera _camera;
        private TilePositionObject _tilePositionObject;

        private MouseHelper() { }


        public static MouseHelper Instance => _instance ?? (_instance = new MouseHelper());

        public TilePositionObject TilePositionObject => _tilePositionObject;

        public Vector2 MousePosition => new Vector2(_mouseDevice.X, _mouseDevice.Y);

        public void Init(MouseDevice mouseDevice, Camera camera)
        {
            _mouseDevice = mouseDevice;
            _camera = camera;
        }

        public void Calculate()
        {
            _tilePositionObject = new TilePositionObject(Vector2.Divide(_camera.GetPosition(),(float)Game.zScale), new Vector2(_mouseDevice.X, _mouseDevice.Y));
        }
    }
}
