using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace YetAnotherEngine.GameObjects
{
    class Gold
    {
        private static Gold _instance;

        public static Gold Instance() => _instance ?? (_instance = new Gold());
        public int GoldValue { get; set; } = 200;

        private const int WarningDrawDuration = 2;
        private const int WarningDrawCount = 5;
        private int _currentWarningDrawDuration = 0;
        private int _currentWarningDrawCount = 0;

        private Gold()
        {
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
                GL.Color4(Color.Goldenrod);
            }
            else
            {
                GL.Color4(Color.Goldenrod);
            }
            TextLine.Instane().WriteGold($"Gold:{GoldValue}");
        }

        public void StartNotEnoughGoldSequence()
        {
            _currentWarningDrawDuration = WarningDrawDuration;
            _currentWarningDrawCount = WarningDrawCount;
        }
    }
}
