using System;
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

        private const int WorldHeightInPixels = WorldConstants.WorldHeight * WorldConstants.TileHeight / 4 +
                                        WorldConstants.WorldWidth * WorldConstants.TileHeight / 4;

        private const int WorldWidthInPixels = WorldConstants.WorldHeight * WorldConstants.TileWidth / 2 +
                                       WorldConstants.WorldWidth * WorldConstants.TileWidth / 2;

        public Camera(KeyboardDevice keyboardDevice, MouseDevice mouseDevice, GameWindow gameWindow)
        {
            _keyboardDevice = keyboardDevice;
            _mouseDevice = mouseDevice;
            _gameWindow = gameWindow;
          
            _position = new Vector2(WorldWidthInPixels/2 + WorldConstants.TileWidth/2,
                -WorldHeightInPixels/2);
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
                    if (WorldHeightInPixels > _gameWindow.Height &&
                        _position.Y < -WorldHeightInPixels/2 + (WorldHeightInPixels / 2 - _gameWindow.Height/2))
                        MoveUp(multiplier);
                }
                else if (_keyboardDevice[KeyboardConstants.DownKey] || mouseMoveDown)
                {
                    if (WorldHeightInPixels > _gameWindow.Height &&
                        _position.Y > -(WorldHeightInPixels / 2 +
                                        (WorldHeightInPixels / 2 - _gameWindow.Height / 2)) - WorldConstants.TileHeight/2) 
                        MoveDown(multiplier); 
                }
                else if (_keyboardDevice[KeyboardConstants.RightKey] || mouseMoveRight)
                {
                    if (WorldWidthInPixels > _gameWindow.Width &&
                        _position.X < WorldWidthInPixels / 2 + (WorldWidthInPixels / 2 - _gameWindow.Width / 2) + WorldConstants.TileWidth/2)
                        MoveRight(multiplier);
                }
                else if (_keyboardDevice[KeyboardConstants.LeftKey] || mouseMoveLeft)
                {
                    if (WorldWidthInPixels > _gameWindow.Width &&
                         _position.X > WorldWidthInPixels / 2 - (WorldWidthInPixels / 2 - _gameWindow.Width / 2) + WorldConstants.TileWidth / 2)
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
