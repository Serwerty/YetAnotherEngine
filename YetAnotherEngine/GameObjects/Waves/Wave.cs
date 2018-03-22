using System.Collections.Generic;
using YetAnotherEngine.Enums;
using YetAnotherEngine.GameObjects.Drawables.Units;
using YetAnotherEngine.Utils;

namespace YetAnotherEngine.GameObjects.Waves
{
    public class Wave
    {
        public int UnitsCount { get; set; }
        public float UnitIntervalDecrement { get; set; }
        public UnitType Type { get; set; }

        public SortedList<int, UnitBase> Units = new SortedList<int, UnitBase>();

        private float _timer;
        private const float DefaultDelayInFrmaes = 60;
        public bool isSpawned = false;

        public Wave(UnitType type, int unitsCount, float unitIntervalDecrement)
        {
            Type = type;
            UnitsCount = unitsCount;
            UnitIntervalDecrement = unitIntervalDecrement;
            _timer = 0;
        }

        public bool SpawnWave(float gameClockMultiplier)
        {
            if (UnitsCount <= 0)
            {
                isSpawned = true;
                return false;
            }

            if (_timer <= 0)
            {
                _timer = DefaultDelayInFrmaes;
                UnitsCount--;
                return true;
            }

            _timer -= UnitIntervalDecrement * gameClockMultiplier;

            return false;
        }
    }
}