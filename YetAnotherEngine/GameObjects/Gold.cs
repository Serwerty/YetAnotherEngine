using System.Drawing;
using OpenTK.Graphics.OpenGL;
using YetAnotherEngine.Constants;

namespace YetAnotherEngine.GameObjects
{
    class Gold
    {
        private static Gold _instance;

        public static Gold Instance() => _instance ?? (_instance = new Gold());
        public int GoldValue { get; set; } = 200;

        private const int WarningDrawDuration = 3;
        private const int WarningDrawCount = 6;
        private int _currentWarningDrawDuration;
        private int _currentWarningDrawCount;

        private const int StartGoldValue = 200;

        private Gold()
        {     
        }

        public void Init()
        {
            GoldValue = StartGoldValue;
        }

        public void WriteGoldValueLine()
        {
            if (_currentWarningDrawCount != 0 && _currentWarningDrawCount % 2 == 0)
            {
                if (_currentWarningDrawDuration != 0)
                {
                    _currentWarningDrawDuration--;
                }
                else
                {
                    _currentWarningDrawDuration = WarningDrawDuration;
                    _currentWarningDrawCount--;

                }
                GL.Color4(Color.Red);
            }
            else if (_currentWarningDrawCount != 0 && _currentWarningDrawCount % 2 == 1)
            {
                if (_currentWarningDrawDuration != 0)
                {
                    _currentWarningDrawDuration--;
                }
                else
                {
                    _currentWarningDrawDuration = WarningDrawDuration;
                    _currentWarningDrawCount--;

                }
                GL.Color4(WorldConstants.OrangeColor);
            }
            else
            {
                GL.Color4(WorldConstants.OrangeColor);
            }
            TextLine.Instane().WriteTextAtRelativePosition($"Gold:{GoldValue}", TextConstants.GoldTextSize,
                TextConstants.GoldTextLocationX, TextConstants.GoldTextLocationY);
        }

        public void StartNotEnoughGoldSequence()
        {
            _currentWarningDrawDuration = WarningDrawDuration;
            _currentWarningDrawCount = WarningDrawCount;
        }
    }
}
