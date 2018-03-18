using OpenTK;
using OpenTK.Input;
using YetAnotherEngine.Constants;
using YetAnotherEngine.Utils.Helpers;

namespace YetAnotherEngine.GameObjects
{
    public class Camera
    {
        private float _worldHeightInPixels = WorldConstants.WorldHeight * WorldConstants.TileHeight / 4 +
                                             WorldConstants.WorldWidth * WorldConstants.TileHeight / 4;

        private float _worldWidthInPixels = WorldConstants.WorldHeight * WorldConstants.TileWidth / 2 +
                                            WorldConstants.WorldWidth * WorldConstants.TileWidth / 2;

        private const double MovableScreenPercent = 0.07;

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


            _position = new Vector2(_worldWidthInPixels / 2 + WorldConstants.TileWidth / 2,
                -_worldHeightInPixels / 2);
        }

        public void Move(double multiplier)
        {
            //_worldWidthInPixels *= Game.MultiplierWidth;
            //_worldHeightInPixels *= Game.MultiplierHeight;

            if (!IsLocked)
            {
                var mouseMoveRight = _mouseDevice.X / Game.zScale >=
                                     (Game.CurrentWidth - Game.CurrentWidth * MovableScreenPercent);
                var mouseMoveLeft = _mouseDevice.X / Game.zScale <= (Game.CurrentWidth * MovableScreenPercent);
                var mouseMoveUp = _mouseDevice.Y / Game.zScale <= (Game.CurrentWidth * MovableScreenPercent);
                var mouseMoveDown = _mouseDevice.Y / Game.zScale >=
                                    (Game.CurrentHeight - Game.CurrentWidth * MovableScreenPercent);


                if (_keyboardDevice[KeyboardConstants.UpKey] || mouseMoveUp)
                {
                    if (_position.Y < (-Game.CurrentHeight / Game.zScale) / 2)
                    {
                        MoveUp(multiplier);
                    }
                }
                else if (_keyboardDevice[KeyboardConstants.DownKey] || mouseMoveDown)
                {
                    if (_position.Y >
                        (-_worldHeightInPixels - _worldHeightInPixels +
                         Game.CurrentHeight / Game.zScale) / 2)
                    {
                        MoveDown(multiplier);
                    }
                }
                else if (_keyboardDevice[KeyboardConstants.RightKey] || mouseMoveRight)
                {
                    if (_position.X <
                        (_worldWidthInPixels + WorldConstants.TileWidth + _worldWidthInPixels -
                         Game.CurrentWidth / Game.zScale) / 2)
                    {
                        MoveRight(multiplier);
                    }
                }
                else if (_keyboardDevice[KeyboardConstants.LeftKey] || mouseMoveLeft)
                {
                    if (_position.X > (WorldConstants.TileWidth + Game.CurrentWidth / Game.zScale) / 2)
                    {
                        MoveLeft(multiplier);
                    }
                }

                ShowStatsHelper.StatsMessage =
                    $"pos:{_position.Y}, {(-_worldHeightInPixels - _worldHeightInPixels + Game.CurrentHeight / Game.zScale) / 2}";
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
            return new Vector2(_position.X * (float) Game.zScale, _position.Y * (float) Game.zScale);
        }
    }
}