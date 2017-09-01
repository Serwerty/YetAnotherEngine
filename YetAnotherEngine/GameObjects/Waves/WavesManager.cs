using System.Collections.Generic;
using YetAnotherEngine.Enums;
using YetAnotherEngine.GameObjects.Units;
using YetAnotherEngine.Utils;

namespace YetAnotherEngine.GameObjects.Waves
{
    //TODO: should be map-related
    public class WavesManager
    {
        //TODO: when having many waves, this should be a list
        private readonly Wave _wave;

        public WavesManager()
        {
            _wave = new Wave(UnitType.Basic, 150, 5);
        }

        public void SpawnWave()
        {
            if (_wave.SpawnWave())
            {
                _wave.Units.Add(new SimpleUnit(CoordsCalculator.CalculateLocationFromTilePosition(_pathList.First()), _mapTextures.UnitsTextures[0]));
            }
        }

        public void MoveUnits(double speedMultiplier)
        {
            foreach (var unit in _wave.Units)
            {
                unit.CheckLocation();
                if (!unit.IsDespawned)
                {
                    unit.Move(speedMultiplier);
                }
            }
            _wave.Units.RemoveAll(x => x.IsDespawned);
        }

        public void RenderUnits()
        {
            foreach (var unit in _wave.Units)
            {
                if (!unit.IsDespawned)
                {
                    unit.Draw();
                }
            }
        }
    }
}
