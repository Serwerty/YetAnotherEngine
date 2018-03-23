using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using YetAnotherEngine.GameObjects.Drawables.Units;
using YetAnotherEngine.Utils.Helpers;

namespace YetAnotherEngine.GameObjects.Drawables.Towers
{
    public class NormalTower : TowerBase
    {
        public const int TextureOffsetX = 0;
        public const int TextureOffsetY = 220;

        public const int TowerCenterX = 15;
        public const int TowerCenterY = 41;

        private const int DefaultRange = 100;
        private const int DefaultPrice = 100;
        private const int DefaultDamage = 10;
        private const int DefaultShootingDelay = 25;

        private UnitBase _currentTargetUnit;

        public NormalTower(Vector2 location, int textureId) : base(location, textureId)
        {
            Range = DefaultRange;
            Price = DefaultPrice;
            Damage = DefaultDamage;
            ShootingDelay = DefaultShootingDelay;
        }

        public override void ResetDelay()
        {
            CurrentShootigDelay = DefaultShootingDelay;
        }

        public override UnitBase GetTargetUnit(SortedList<int, UnitBase> units)
        {
            if (_currentTargetUnit == null)
            {
                _currentTargetUnit = CalculateClosestUnit(units);
            }
            else
            {
                var location = Location - new Vector2(TowerCenterX - 2, -TowerCenterY + 6);
                double distance = Math.Sqrt(
                    (_currentTargetUnit.Location.X - location.X) * (_currentTargetUnit.Location.X - location.X) +
                    (_currentTargetUnit.Location.Y * 2 - location.Y * 2) *
                    (_currentTargetUnit.Location.Y * 2 - location.Y * 2));
                if (distance > Range || _currentTargetUnit.IsDespawned)
                {
                    _currentTargetUnit = CalculateClosestUnit(units);
                }
            }

            return _currentTargetUnit;
        }

        private UnitBase CalculateClosestUnit(SortedList<int, UnitBase> units)
        {
            double minDistance = int.MaxValue;
            int key = 0;

            foreach (var unit in units)
            {
                if (!unit.Value.IsDespawned)
                {
                    var location = Location - new Vector2(TowerCenterX - 2, -TowerCenterY + 6);
                    double distance = Math.Sqrt(
                        (unit.Value.Location.X - location.X) * (unit.Value.Location.X - location.X) +
                        (unit.Value.Location.Y * 2f - location.Y * 2f) *
                        (unit.Value.Location.Y * 2f - location.Y * 2f));

                    if (distance <= Range && distance < minDistance)
                    {
                        minDistance = distance;
                        ShowStatsHelper.StatsMessage = $"distance = {distance:0}";
                        key = unit.Key;
                    }
                }
            }

            return units.FirstOrDefault(x => x.Key == key).Value;
        }
    }
}