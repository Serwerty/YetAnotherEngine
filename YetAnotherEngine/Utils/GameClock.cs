namespace YetAnotherEngine.Utils
{
    public static class GameClock
    {
        private const double TargetFps = 60.0;
        private const double MultiplierDivider = 1000 / TargetFps;

        public static double GetMultiplier(double seconds)
        {
            return seconds * 1000 / MultiplierDivider;
            //todo: discuss the possibility to remove 1000
        }
    }
}
