using YetAnotherEngine.Enums;

namespace YetAnotherEngine.GameObjects
{
    class Wave
    {
        public int Count { get; set; }
        public int Decrement { get; set; }
        public UnitType Type { get; set; }

        private int _timer;
        private const int DefaultDelay = 45;

        public Wave(UnitType type, int count, int decrement)
        {
            Type = type;
            Count = count;
            Decrement = decrement;
            _timer = 0;
        }

        public bool SpawnWave()
        {
            if (_timer <= 0 && Count > 0)
            {
                _timer = DefaultDelay;
                Count--;
                return true;
            }

            _timer -= Decrement;

            return false;
        }
    }
}
