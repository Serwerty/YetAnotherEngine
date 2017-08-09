using System;
using System.Drawing;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;
namespace YetAnotherEngine
{
    class Game : GameWindow
    {
        //Default windows size
        private const int NominalWidth = 700;
        private const int NominalHeight = 525;
        
        //for resize purpose
        private float _projectionWidth;
        private float _projectionHeight;

        //Window header
        private const string WindowsHeader = "OpenGL Test";

        //game world instance
        private static World _gameWorld;

        private readonly Player _player;

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

            _gameWorld = World.CreateInstance();
            _player = Player.SetInstance(Keyboard);
        }

        protected override void OnLoad(EventArgs E)
        {
            base.OnLoad(E);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);

            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            _gameWorld.LoadMap("map.txt");
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
            _player.Move(_gameWorld.GetWorldObjects());
        }

        protected override void OnRenderFrame(FrameEventArgs E)
        {
            base.OnRenderFrame(E);
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var projection = Matrix4.CreateOrthographic(-NominalWidth, -NominalHeight, -1, 1);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
            GL.Translate(_player.GetPlayerLocation().X, -_player.GetPlayerLocation().Y, 0);

            var modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
            GL.Viewport(0, 0, ClientRectangle.Width, ClientRectangle.Height);
         
            //Draw...
            _gameWorld.RenderBackground((float)ClientRectangle.Width, (float)ClientRectangle.Height, 2100, _projectionHeight,_player);
            _gameWorld.DrawWorld();
            _player.Draw();
            SwapBuffers();
            TimeManager.SetFrameInterval();
        }
    }
}
