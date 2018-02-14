using System.Collections.Generic;
using YetAnotherEngine.Enums;
using YetAnotherEngine.GameObjects.Drawables.Units;

namespace YetAnotherEngine.GameObjects.Waves
{
    public class Wave
    {
        public int UnitsCount { get; set; }
        public int UnitIntervalDecrement { get; set; }
        public UnitType Type { get; set; }

        public SortedList<int, UnitBase> Units = new SortedList<int,UnitBase>();

        private int _timer;
        private const int DefaultDelayInMs = 45;

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
                _timer = DefaultDelayInMs;
                UnitsCount--;
                return true;
            }

            _timer -= UnitIntervalDecrement;

            return false;
        }
    }
}
