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

        public void BindEvents()
        {
            _mouseDevice.Move += _mouseDevice_Move;
            _keyboardDevice.KeyDown += _keyboardDevice_KeyDown;
        }

        private void _mouseDevice_Move(object sender, MouseMoveEventArgs e)
        {
            bool shouldMoveRight = e.X >= (_gameWindow.Width - 10);
            bool shouldMoveLeft = e.X <= (10);
            bool shouldMoveUp = e.Y <= (10);
            bool shouldMoveDown = e.Y >= (_gameWindow.Height - 10);


            if (shouldMoveRight) //Should we move camera right
            {
                MoveRight();
            }

            if (shouldMoveLeft) //Should we move camera left
            {
                MoveLeft();
            }

            if (shouldMoveDown) //Should we move camera down
            {
                MoveDown();
            }

            if (shouldMoveUp) //Should we move camera up
            {
                MoveUp();
            }
        }

        private void _keyboardDevice_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == KeyboardConstants.UpKey)
            {
                MoveUp();
            }

            if (e.Key == KeyboardConstants.DownKey)
            {
                MoveDown();
            }

            if (e.Key == KeyboardConstants.RightKey)
            {
                MoveRight();
            }

            if (e.Key == KeyboardConstants.LeftKey)
            {
                MoveLeft();
            }
        }

        private void MoveRight()
        {
            _position.X += WorldConstants.CameraScrollSpeed;
        }

        private void MoveLeft()
        {
            _position.X -= WorldConstants.CameraScrollSpeed;
        }

        private void MoveUp()
        {
            _position.Y += WorldConstants.CameraScrollSpeed;
        }

        private void MoveDown()
        {
            _position.Y -= WorldConstants.CameraScrollSpeed;
        }

        public Vector2 GetPosition()
        {
            return _position;
        }
    }
}
