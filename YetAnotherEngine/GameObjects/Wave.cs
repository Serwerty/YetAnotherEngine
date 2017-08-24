using YetAnotherEngine.Enums;

namespace YetAnotherEngine.GameObjects
{
    class Wave
    {
        public int UnitsCount { get; set; }
        public int UnitIntervalDecrement { get; set; }
        public UnitType Type { get; set; }

        private int _timer;
        private const int DefaultDelayInMS = 45;

        public Wave(UnitType type, int unitsCount, int unitIntervalDecrement)
        {
            Type = type;
            UnitsCount = unitsCount;
            UnitIntervalDecrement = unitIntervalDecrement;
            _timer = 0;
        }

        public bool SpawnWave()
        {
            if (_timer <= 0 && UnitsCount > 0)
            {
                _timer = DefaultDelayInMS;
                UnitsCount--;
                return true;
            }

            _timer -= UnitIntervalDecrement;

            return false;
        }
    }
}
