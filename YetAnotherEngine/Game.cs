using System;
using System.Drawing;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using YetAnotherEngine.GameObjects;

namespace YetAnotherEngine
{
    class Game : GameWindow
    {
        //Default windows size
        private const int NominalWidth = 1024;
        private const int NominalHeight = 780;
        
        //for resize purpose
        private float _projectionWidth;
        private float _projectionHeight;

        //Window header
        private const string WindowsHeader = "Tower Defence";

        private float _avgFps;
        private float _avgCnt;
        private Camera _camera;

        private static World _gameWorld;

        private static TextLine _fpsText;

        //application entry point
        [STAThread]
        static void Main()
        {
            //Launching new game window
            using (var game = new Game())
            {
                game.Run(200,200);
            }
        }

        public Game()
            : base(NominalWidth, NominalHeight, GraphicsMode.Default, WindowsHeader)
        {
            //turning vertical sync on
            VSync = VSyncMode.On;

            _gameWorld = new World();
            _fpsText = new TextLine("big-outline.png");

            //_gameWorld = World.CreateInstance();
            //_player = Player.SetInstance(Keyboard);
        }

        protected override void OnLoad(EventArgs E)
        {
            base.OnLoad(E);

            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            _camera = new Camera(Keyboard);

            //_gameWorld.LoadMap("map.txt");
        }

        protected override void OnResize(EventArgs E)
        {
            base.OnResize(E);
            GL.Viewport(0, 0, (int)_projectionWidth, (int)_projectionHeight);
            _projectionWidth = NominalWidth;
            _projectionHeight = (float)ClientRectangle.Height / (float)ClientRectangle.Width * _projectionWidth;
            if (_projectionHeight < NominalHeight)
            {
                _projectionHeight = NominalHeight;
                _projectionWidth = (float)ClientRectangle.Width / (float)ClientRectangle.Height * _projectionHeight;
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs E)
        {
            base.OnUpdateFrame(E);
            _camera.Move();
            //_player.Move(_gameWorld.GetWorldObjects());
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.ClearColor(Color.White);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var projection = Matrix4.CreateOrthographic(-NominalWidth, -NominalHeight, -1, 1);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
            GL.Translate(_camera.GetPosition().X, _camera.GetPosition().Y, 0);
            GL.Translate(200, 100, 0);

            var modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
            GL.Viewport(0, 0, ClientRectangle.Width, ClientRectangle.Height);

            //Draw...
            //_gameWorld.RenderBackground((float)ClientRectangle.Width, (float)ClientRectangle.Height, 2100, _projectionHeight,_player);
            // _gameWorld.DrawWorld();
            //_player.Draw();

            _gameWorld.RenderGround();

            var curFps = (float)(1.0 / e.Time);
            if (_avgCnt <= 10.0F)
                _avgFps = curFps;
            else
            {
                _avgFps += (curFps - _avgFps) / _avgCnt;
            }
            _avgCnt++;

            _fpsText.WriteText("FPS average: " + _avgFps.ToString("F2") + " - FPS current: " + curFps.ToString("F2"));


            SwapBuffers();
            //TimeManager.SetFrameInterval();
        }
    }
}
