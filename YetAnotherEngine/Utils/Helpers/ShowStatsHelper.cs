using System;
using System.Drawing;
using System.Net;
using OpenTK.Graphics.OpenGL;
using YetAnotherEngine.GameObjects;

namespace YetAnotherEngine.Utils.Helpers
{
    class ShowStatsHelper
    {
        private static ShowStatsHelper _instance;

        private ShowStatsHelper()
        {
        }

        public static ShowStatsHelper Instance => _instance ?? (_instance = new ShowStatsHelper());

        public static String StatsMessage { get; set; } = "message";

        public void ShowStats()
        {
            GL.Color4(Color.White);
            TextLine.Instane().WriteLogText(StatsMessage);
        }
    }
}