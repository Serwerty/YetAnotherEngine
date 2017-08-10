using System;

namespace YetAnotherEngine.Utils
{
    public static class GameClock
    {
        private static int _lastTick = DateTime.Now.Millisecond;

        public static double GetMultiplier(double sec)
        {
            //var now = DateTime.Now.Millisecond;
            var result = (sec * 1000) / (1000 / 60.0);
            //_lastTick = now;

            return result;
        }

    }
}
