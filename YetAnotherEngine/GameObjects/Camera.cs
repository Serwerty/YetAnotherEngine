using OpenTK;
using OpenTK.Input;
using YetAnotherEngine.Constants;

namespace YetAnotherEngine.GameObjects
{
    public class Camera
    {
        private Vector2 _position;
        private readonly KeyboardDevice _keyboardDevice;
        private readonly MouseDevice _mouseDevice;
        private readonly GameWindow _gameWindow;

        public bool IsLocked { get; set; } = false;

        public Camera(KeyboardDevice keyboardDevice, MouseDevice mouseDevice, GameWindow gameWindow)
        {
            _keyboardDevice = keyboardDevice;
            _mouseDevice = mouseDevice;
            _gameWindow = gameWindow;
            ;
            _position = new Vector2(WorldConstants.WorldWidth * WorldConstants.TileWidth / 2f,
                -WorldConstants.WorldHeight * WorldConstants.TileHeight / 4f);
        }

        public void Move(double multiplier)
        {
            if (!IsLocked)
            {
                var mouseMoveRight = _mouseDevice.X >= (_gameWindow.Width - _gameWindow.Width* 0.10);
                var mouseMoveLeft = _mouseDevice.X <= (_gameWindow.Width * 0.10);
                var mouseMoveUp = _mouseDevice.Y <= (_gameWindow.Width * 0.10);
                var mouseMoveDown = _mouseDevice.Y >= (_gameWindow.Height - _gameWindow.Width * 0.10);

                if (_keyboardDevice[KeyboardConstants.UpKey] || mouseMoveUp)
                {
                    MoveUp(multiplier);
                }
                else if (_keyboardDevice[KeyboardConstants.DownKey] || mouseMoveDown)
                {
                    MoveDown(multiplier);
                }
                else if (_keyboardDevice[KeyboardConstants.RightKey] || mouseMoveRight)
                {
                    MoveRight(multiplier);
                }
                else if (_keyboardDevice[KeyboardConstants.LeftKey] || mouseMoveLeft)
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
