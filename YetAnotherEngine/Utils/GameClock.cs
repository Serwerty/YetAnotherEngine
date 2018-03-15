namespace YetAnotherEngine.Utils
{
    public static class GameClock
    {
        private const float TargetFps = 60f;
        private const float MultiplierDivider = 1000 / TargetFps;

        public static float GetMultiplier(float seconds)
        {
            const int millisecondsInSecond = 1000;
            return seconds * millisecondsInSecond / MultiplierDivider;
            //todo: discuss the possibility to remove 1000
        }
    }
}
