namespace YetAnotherEngine.Utils
{
    public static class GameClock
    {
        private const double TargetFps = 60.0;
        private const double MultiplierDivider = 1000 / TargetFps;

        public static double GetMultiplier(double seconds)
        {
            const int millisecondsInSecond = 1000;
            return seconds * millisecondsInSecond / MultiplierDivider;
            //todo: discuss the possibility to remove 1000
        }
    }
}
