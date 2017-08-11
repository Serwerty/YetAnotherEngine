using OpenTK;
using OpenTK.Input;
using YetAnotherEngine.Constants;

namespace YetAnotherEngine.GameObjects
{
    class Camera
    {
        private Vector2 _position;
        private KeyboardDevice _keyboardDevice;
        private MouseDevice _mouseDevice;
        private GameWindow _gameWindow;

        private bool _isLocked { get; set; } = false;

        public Camera(KeyboardDevice keyboardDevice, MouseDevice mouseDevice, GameWindow gameWindow)
        {
            _keyboardDevice = keyboardDevice;
            _mouseDevice = mouseDevice;
            _gameWindow = gameWindow;

            _position = new Vector2(0f, 0f);
        }

        public void Move(double multiplier)
        {
            if (!_isLocked)
            {
                bool mouseMoveRight = _mouseDevice.X >= (_gameWindow.Width - _gameWindow.Width* 0.10);
                bool mouseMoveLeft = _mouseDevice.X <= (_gameWindow.Width * 0.10);
                bool mouseMoveUp = _mouseDevice.Y <= (_gameWindow.Width * 0.10);
                bool mouseMoveDown = _mouseDevice.Y >= (_gameWindow.Height - _gameWindow.Width * 0.10);

                if (_keyboardDevice[KeyboardConstants.UpKey] || mouseMoveUp)
                {
                    MoveUp(multiplier);
                }
                if (_keyboardDevice[KeyboardConstants.DownKey] || mouseMoveDown)
                {
                    MoveDown(multiplier);
                }
                if (_keyboardDevice[KeyboardConstants.RightKey] || mouseMoveRight)
                {
                    MoveRight(multiplier);
                }
                if (_keyboardDevice[KeyboardConstants.LeftKey] || mouseMoveLeft)
                {
                    MoveLeft(multiplier);
                }
            }
        }

        private void MoveRight(double multiplier)
        {
            _position.X += (float)multiplier * WorldConstants.CameraScrollSpeed;
        }

        private void MoveLeft(double multiplier)
        {
            _position.X -= (float)multiplier * WorldConstants.CameraScrollSpeed;
        }

        private void MoveUp(double multiplier)
        {
            _position.Y += (float)multiplier * WorldConstants.CameraScrollSpeed;
        }

        private void MoveDown(double multiplier)
        {
            _position.Y -= (float)multiplier * WorldConstants.CameraScrollSpeed;
        }

        public Vector2 GetPosition()
        {
            return _position;
        }
    }
}
