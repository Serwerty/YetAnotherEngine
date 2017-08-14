using System;
using System.Drawing;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using YetAnotherEngine.GameObjects;
using YetAnotherEngine.Utils;
using YetAnotherEngine.Enums;

namespace YetAnotherEngine
{
    class Game : GameWindow
    {
        //Default windows size
        public const int NominalWidth = 1024;
        public const int NominalHeight = 780;

        //for resize purpose
        private float _projectionWidth;
        private float _projectionHeight;

        //Window header
        private const string WindowsHeader = "Tower Defence";

        private float _avgFps;
        private float _avgCnt;
        private Camera _camera;

        private static World _gameWorld;
        private static MainMenu _gameMenu;

        private static TextLine _fpsText;

        private GameState _gameState = GameState.InGame;

        public Game() : base(NominalWidth, NominalHeight, new GraphicsMode(32, 16, 8, 16), WindowsHeader)
        {
            //turning vertical sync on
            VSync = VSyncMode.On;

            WindowBorder = WindowBorder.Fixed;

            _gameWorld = new World(Mouse,Keyboard);
            _gameMenu = new MainMenu();
            _fpsText = new TextLine("big-outline.png");
        }

        protected override void OnLoad(EventArgs E)
        {
            base.OnLoad(E);

            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
            GL.DepthMask(false);

            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.PointSmooth);
            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);

            _camera = new Camera(Keyboard, Mouse, this);
        }

        protected override void OnResize(EventArgs E)
        {
            base.OnResize(E);

            GL.Viewport(0, 0, (int)_projectionWidth, (int)_projectionHeight);
            _projectionWidth = NominalWidth;
            _projectionHeight = ClientRectangle.Height / (float)ClientRectangle.Width * _projectionWidth;
            if (_projectionHeight < NominalHeight)
            {
                _projectionHeight = NominalHeight;
                _projectionWidth = ClientRectangle.Width / (float)ClientRectangle.Height * _projectionHeight;
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            var multiplier = GameClock.GetMultiplier(e.Time);
            _camera.Move(multiplier);
            //_player.Move(_gameWorld.GetWorldObjects());
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.ClearColor(Color.White);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);



            var modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
            GL.Viewport(0, 0, ClientRectangle.Width, ClientRectangle.Height);

            switch (_gameState)
            {
                case GameState.InMainMenu:
                    _gameMenu.SetUpMenuProjection();
                    _gameMenu.RenderMenu();
                    break;
                case GameState.InGame:

                    var projection = Matrix4.CreateOrthographic(-NominalWidth, -NominalHeight, -1, 1);
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadMatrix(ref projection);
                    GL.Translate(_camera.GetPosition().X, _camera.GetPosition().Y, 0);
                    GL.Translate(0, 0, 0);
       
                    _gameWorld.RenderGround();
                    _gameWorld.RenderTowers();
                    _gameWorld.RenderSelection(_camera.GetPosition());
                    _gameWorld.RenderTowerToBePlaced(_camera.GetPosition());       
                    break;
                case GameState.InOptions:
                    //_optionsMenu.RenderMenu();
                    break;
            }

            GL.Color4(Color.White);
            var curFps = (float)(1.0 / e.Time);
            if (_avgCnt <= 10.0F)
                _avgFps = curFps;
            else
            {
                _avgFps += (curFps - _avgFps) / _avgCnt;
            }
            _avgCnt++;

            _fpsText.WriteFps("FPS average: " + $"{_avgFps:0}" + " FPS current: " + $"{curFps:0}");


            SwapBuffers();
            //TimeManager.SetFrameInterval();
        }
    }
}
