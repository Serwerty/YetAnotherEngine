using OpenTK;
using OpenTK.Input;
using YetAnotherEngine.Constants;

namespace YetAnotherEngine.GameObjects
{
    public class Camera
    {
        private const int WorldHeightInPixels = WorldConstants.WorldHeight * WorldConstants.TileHeight / 4 +
                                                WorldConstants.WorldWidth * WorldConstants.TileHeight / 4;

        private const int WorldWidthInPixels = WorldConstants.WorldHeight * WorldConstants.TileWidth / 2 +
                                               WorldConstants.WorldWidth * WorldConstants.TileWidth / 2;

        private const double MovableScreenPercent = 0.10;

        private Vector2 _position;
        private readonly KeyboardDevice _keyboardDevice;
        private readonly MouseDevice _mouseDevice;

        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }

        public bool IsLocked { get; set; } = false;

        public Camera(KeyboardDevice keyboardDevice, MouseDevice mouseDevice, int windowWidth, int windowHeight)
        {
            _keyboardDevice = keyboardDevice;
            _mouseDevice = mouseDevice;
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;


            _position = new Vector2(WorldWidthInPixels / 2 + WorldConstants.TileWidth / 2,
                -WorldHeightInPixels / 2);
        }

        public void Move(double multiplier)
        {
            if (!IsLocked)
            {
                var mouseMoveRight = _mouseDevice.X >= (WindowWidth - WindowWidth * MovableScreenPercent);
                var mouseMoveLeft = _mouseDevice.X <= (WindowWidth * MovableScreenPercent);
                var mouseMoveUp = _mouseDevice.Y <= (WindowWidth * MovableScreenPercent);
                var mouseMoveDown = _mouseDevice.Y >= (WindowHeight - WindowWidth * MovableScreenPercent);

                if (_keyboardDevice[KeyboardConstants.UpKey] || mouseMoveUp)
                {
                    var upperMapBoundary =
                        _position.Y < -WorldHeightInPixels / 2 + (WorldHeightInPixels / 2 - WindowHeight / 2);

                    if (WorldHeightInPixels > WindowHeight && upperMapBoundary)
                    {
                        MoveUp(multiplier);
                    }
                }
                else if (_keyboardDevice[KeyboardConstants.DownKey] || mouseMoveDown)
                {
                    var lowerMapBoundary = _position.Y >
                                           -(WorldHeightInPixels / 2 + (WorldHeightInPixels / 2 - WindowHeight / 2)) -
                                           WorldConstants.TileHeight / 2;

                    if (WorldHeightInPixels > WindowHeight && lowerMapBoundary)
                    {
                        MoveDown(multiplier);
                    }
                }
                else if (_keyboardDevice[KeyboardConstants.RightKey] || mouseMoveRight)
                {
                    var rightMapBoundary = _position.X < WorldWidthInPixels / 2 +
                                           (WorldWidthInPixels / 2 - WindowWidth / 2) + WorldConstants.TileWidth / 2;

                    if (WorldWidthInPixels > WindowWidth && rightMapBoundary)
                    {
                        MoveRight(multiplier);
                    }
                }
                else if (_keyboardDevice[KeyboardConstants.LeftKey] || mouseMoveLeft)
                {
                    var leftMapBoundary = _position.X > WorldWidthInPixels / 2 -
                                          (WorldWidthInPixels / 2 - WindowWidth / 2) + WorldConstants.TileWidth / 2;

                    if (WorldWidthInPixels > WindowWidth && leftMapBoundary)
                    {
                        MoveLeft(multiplier);
                    }
                }
            }
        }

        private void MoveRight(double multiplier)
        {
            _position.X += (float) multiplier * WorldConstants.CameraScrollSpeed;
        }

        private void MoveLeft(double multiplier)
        {
            _position.X -= (float) multiplier * WorldConstants.CameraScrollSpeed;
        }

        private void MoveUp(double multiplier)
        {
            _position.Y += (float) multiplier * WorldConstants.CameraScrollSpeed;
        }

        private void MoveDown(double multiplier)
        {
            _position.Y -= (float) multiplier * WorldConstants.CameraScrollSpeed;
        }

        public Vector2 GetPosition()
        {
            return _position;
        }
    }
}