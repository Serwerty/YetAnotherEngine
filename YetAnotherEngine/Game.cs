using System;
using System.Drawing;
using System.Net;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using OpenTK.Input;
using YetAnotherEngine.GameObjects;
using YetAnotherEngine.Utils;
using YetAnotherEngine.Enums;
using YetAnotherEngine.Constants;
using YetAnotherEngine.GameObjects.World;
using YetAnotherEngine.Utils.Helpers;

namespace YetAnotherEngine
{
    public class Game : GameWindow
    {
        public const int NominalWidth = 1280;
        public const int NominalHeight = 1024;
        private const string WindowHeader = "Tower Defence";

        //for resizing purpose
        private float _projectionWidth;
        private float _projectionHeight;

        private readonly Camera _camera;
        private readonly GameWorld _gameWorld;
        private readonly MainMenu _gameMenu;

        //TODO: refactor
        public static float zScale = 1;
        public static float MultiplierWidth;
        public static float MultiplierHeight;

        public static float CurrentWidth;
        public static float CurrentHeight;

        private float _gameClockMultiplier = 1;

        private GameState _gameState = GameState.InGame;

        public Game() : base(NominalWidth, NominalHeight, new GraphicsMode(32, 16, 8, 16), WindowHeader)
        {
            VSync = VSyncMode.On;

           // WindowBorder = WindowBorder.Fixed;

            MultiplierWidth = NominalWidth * 1f / Width;
            MultiplierHeight = NominalHeight * 1f / Height;

            CurrentWidth = Width;
            CurrentHeight = Height;

            _camera = new Camera(Keyboard, Mouse, NominalWidth, NominalHeight);
            _gameWorld = new GameWorld(Mouse, Keyboard, _camera);
            _gameMenu = new MainMenu();
            MouseHelper.Instance.Init(Mouse, _camera);
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
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.Delta < 0 && zScale > WorldConstants.ZoomInLimitation) // Zoom in
            {
                zScale -= WorldConstants.ZoomSpeed * _gameClockMultiplier;
            }
            else if (e.Delta > 0 && zScale < WorldConstants.ZoomOutLimitation) // Zoom out
            {
                zScale += WorldConstants.ZoomSpeed * _gameClockMultiplier;
            }
        }

        protected override void OnResize(EventArgs E)
        {
            base.OnResize(E);

            MultiplierWidth = NominalWidth * 1f / Width;
            MultiplierHeight = NominalHeight * 1f / Height;
            CurrentWidth = Width;
            CurrentHeight = Height;
            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (Keyboard[Key.T])
            {
                _gameWorld.AddTower();
            }
            _gameWorld.CheckButtons();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            _gameClockMultiplier = GameClock.GetMultiplier((float)e.Time);

            _camera.Move(_gameClockMultiplier);

            _gameWorld.MoveUnits(_gameClockMultiplier);
            _gameWorld.SpawnWaves();
            _gameWorld.MoveProjectiles(_gameClockMultiplier);
            _gameWorld.CheckTowersForShoot();

            MouseHelper.Instance.Calculate();
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
            //GL.Viewport(0, 0, NominalWidth, NominalHeight);
           

            switch (_gameState)
            {
                case GameState.InMainMenu:
                    _gameMenu.SetUpMenuProjection();
                    _gameMenu.RenderMenu();
                    break;
                case GameState.InGame:
                    var projection = Matrix4.CreateOrthographic(-NominalWidth/MultiplierWidth, -NominalHeight/MultiplierHeight, -1, 1);
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadMatrix(ref projection);
                    GL.Translate(_camera.GetPosition().X, _camera.GetPosition().Y, 0);

                    #region Working with zooming
                    GL.Scale(zScale, zScale, zScale);
                    #endregion

                    _gameWorld.RenderGround();
                    _gameWorld.RenderDrawables();
                    //_gameWorld.RenderTowers();
                    _gameWorld.RenderSelection();
                    _gameWorld.RenderTowerToBePlaced(_camera.GetPosition());
                    // _gameWorld.RenderUnits();
                    _gameWorld.RenderButtons();

                    #region ShowInfo

                    FpsHelper.Instance.DrawFpsText(e.Time);
                    MouseHelper.Instance.DrawCoords();
                    MouseHelper.Instance.DrawTilePosition();
                    //ShowStatsHelper.StatsMessage = $"zScale = {zScale}";
                    ShowStatsHelper.Instance.ShowStats();

                    #endregion

                    Gold.Instance().WriteGoldValueLine();

                    break;
                case GameState.InOptions:
                    //_optionsMenu.RenderMenu();
                    break;
            }

            SwapBuffers();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _camera.IsLocked = true;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _camera.IsLocked = false;
        }
    }
}