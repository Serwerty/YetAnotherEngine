using System.Drawing;
using OpenTK.Graphics.OpenGL;
using YetAnotherEngine.Constants;
using YetAnotherEngine.Enums;

namespace YetAnotherEngine.GameObjects
{
    class LivesManager
    {
        private static LivesManager _instance;
        public static LivesManager GetInstance() => _instance ?? (_instance = new LivesManager());

        public int LivesCount { get; set; }

        private const int DefaultLifeCount = 20;

        private LivesManager()
        {
        }

        public void Init()
        {
            LivesCount = DefaultLifeCount;
        }

        public void RenderLivesCountHeart()
        {
            GL.Color4(Color.Red);
            TextLine.Instane().WriteTextAtRelativePosition($"{LivesCount}",TextConstants.LivesCountTextSize,
                TextConstants.LivesCountTextLocationX, TextConstants.LivesCountTextLocationY);
        }

        public void LoseLive()
        {
            LivesCount--;
            if (LivesCount == 0)
            {
                Game.GameState = GameState.InGameOverScreen;
            }

        }
    }
}
