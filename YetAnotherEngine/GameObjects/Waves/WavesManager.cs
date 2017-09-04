using OpenTK;
using System.Collections.Generic;
using System.Linq;
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
        private readonly List<Vector2> _roadList;
        private readonly int[] _unitTextures;

        public WavesManager(List<Vector2> roadList, int[] unitTextures)
        {
            _unitTextures = unitTextures;
            _roadList = roadList;
            _wave = new Wave(UnitType.Basic, 150, 5);
        }

        public void SpawnWave()
        {
            if (_wave.SpawnWave())
            {
                _wave.Units.Add(new SimpleUnit(CoordsCalculator.CalculateLocationFromTilePosition(_roadList.First()), _unitTextures[0]));
            }
        }

        public void MoveUnits(double speedMultiplier)
        {
            foreach (var unit in _wave.Units)
            {
                unit.CheckLocation(_roadList);
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
