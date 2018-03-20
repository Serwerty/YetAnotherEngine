namespace YetAnotherEngine.Utils
{
    class GameStatistic
    {
        private static GameStatistic _instance;

        private GameStatistic()
        {
        }

        public static GameStatistic GetInstance() => _instance ?? (_instance = new GameStatistic());

        public int DamageDealtValue { get; set; }
        public int GoldEarned { get; set; }
        public int UnitsKiled { get; set; }

        public void Init()
        {
            DamageDealtValue = 0;
            GoldEarned = 0;
            UnitsKiled = 0;
        }

    }
}
