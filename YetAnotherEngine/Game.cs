using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK;
namespace YetAnotherEngine
{
    class Game : GameWindow
    {
        //Default windows size
        private const int NominalWidth = 700;
        private const int NominalHeight = 525;
        
        //for resize purpose
        private float ProjectionWidth;
        private float ProjectionHeight;

        //Window header
        private static string WindowsHeader = "OpenGL Test";

        //game world instance
        private static World GameWorld;

        private Player player;
        //application entry point
        [STAThread]
        static void Main()
        {
            //Launching new game window
            using (var Game = new Game())
            {
                Game.Run(200,200);
            }
        }

        public Game()
            : base(NominalWidth, NominalHeight, GraphicsMode.Default, WindowsHeader)
        {
            //turning vertical sync on
            VSync = VSyncMode.On;
            GameWorld = World.CreateInstance();
            player = Player.SetInstance(this.Keyboard);

        }

        protected override void OnLoad(EventArgs E)
        {
            base.OnLoad(E);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);

            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GameWorld.LoadMap("map.txt");
        }

        protected override void OnResize(EventArgs E)
        {
            base.OnResize(E);
            GL.Viewport(0, 0, (int)ProjectionWidth, (int)ProjectionHeight);
            ProjectionWidth = NominalWidth;
            ProjectionHeight = (float)ClientRectangle.Height / (float)ClientRectangle.Width * ProjectionWidth;
            if (ProjectionHeight < NominalHeight)
            {
                ProjectionHeight = NominalHeight;
                ProjectionWidth = (float)ClientRectangle.Width / (float)ClientRectangle.Height * ProjectionHeight;
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs E)
        {
            base.OnUpdateFrame(E);
            player.Move(GameWorld.GetWorldObjects());
        }

        protected override void OnRenderFrame(FrameEventArgs E)
        {
            base.OnRenderFrame(E);
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var Projection = Matrix4.CreateOrthographic(-NominalWidth, -NominalHeight, -1, 1);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref Projection);
            GL.Translate(player.GetLocation().X, -player.GetLocation().Y, 0);

            var Modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref Modelview);
            GL.Viewport(0, 0, ClientRectangle.Width, ClientRectangle.Height);
         
            //Draw...
            GameWorld.RenderBackground((float)ClientRectangle.Width, (float)ClientRectangle.Height, 2100, ProjectionHeight,player);
            GameWorld.DrawWorld();
            player.Draw();
            SwapBuffers();
            TimeManager.SetFrameInterval();
        }
    }
}
