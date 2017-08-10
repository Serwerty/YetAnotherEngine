using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using YetAnotherEngine.Constants;

namespace YetAnotherEngine.GameObjects
{
    class Camera
    {
        private Vector2 _position;
        private KeyboardDevice _keyboardDevice;
        private bool _isLocked { get; set; } = false;

        public Camera(KeyboardDevice keyboardDevice)
        {
            _keyboardDevice = keyboardDevice;
            _position = new Vector2(0f,0f);
        }

        public void Move()
        {
            if (!_isLocked)
            {
                if (_keyboardDevice[Constants.KeyboardConstants.UpKey])
                {
                    _position.Y += WorldConstants.CameraScrollSpeed;
                }
                if (_keyboardDevice[Constants.KeyboardConstants.DownKey])
                {
                    _position.Y -= WorldConstants.CameraScrollSpeed;
                }
                if (_keyboardDevice[Constants.KeyboardConstants.RightKey])
                {
                    _position.X += WorldConstants.CameraScrollSpeed;
                }
                if (_keyboardDevice[Constants.KeyboardConstants.LeftKey])
                {
                    _position.X -= WorldConstants.CameraScrollSpeed;
                }
            }
        }

        public Vector2 GetPosition()
        {
            return _position;
        }
    }
}
