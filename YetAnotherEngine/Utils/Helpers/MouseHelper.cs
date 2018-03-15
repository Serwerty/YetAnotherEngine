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

        private MouseHelper()
        {
        }


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
            _tilePositionObject = new TilePositionObject(_camera.GetPosition(),
                new Vector2(_mouseDevice.X, _mouseDevice.Y));
        }

        public void DrawCoords()
        {
            GL.Color4(Color.White);
             TextLine.Instane().WriteCoords($"mouse X: {_mouseDevice.X:0} mouse Y: {_mouseDevice.Y:0}");
        }

        public void DrawTilePosition()
        {
            GL.Color4(Color.White);
            if (TilePositionObject == null) return;
            TextLine.Instane().WriteTilePosition(
                $"pos X: {TilePositionObject.TilePosition.X:0} pos Y: {TilePositionObject.TilePosition.Y:0} " +
                $"loc X: {TilePositionObject.TileCoords.X:0} loc Y: {TilePositionObject.TileCoords.Y:0}");
        }
    }
}