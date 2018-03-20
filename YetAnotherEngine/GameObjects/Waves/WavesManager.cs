using OpenTK;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using YetAnotherEngine.Enums;
using YetAnotherEngine.GameObjects.Drawables.Units;
using YetAnotherEngine.Utils;
using YetAnotherEngine.Utils.Helpers;
using YetAnotherEngine.Utils.Models;

namespace YetAnotherEngine.GameObjects.Waves
{
    //TODO: should be map-related
    public class WavesManager
    {
        //TODO: when having many waves, this should be a list
        private readonly Wave _wave;
        private readonly List<Vector2> _roadList;
        private readonly int[] _unitTextures;
        private readonly int _hpBarTextureId;
        private readonly Camera _camera;

        public WavesManager(List<Vector2> roadList, int[] unitTextures, Camera camera, int hpBarTextureId)
        {
            _unitTextures = unitTextures;
            _roadList = roadList;
            _wave = new Wave(UnitType.Basic, 150, 1);
            _camera = camera;
            _hpBarTextureId = hpBarTextureId;
        }

        public void SpawnWave(float gameClockMultiplier)
        {
            if (_wave.SpawnWave(gameClockMultiplier))
            {
                UnitBase unitToBeRendered =
                    new SimpleUnit(CoordsCalculator.CalculateLocationFromTilePosition(_roadList.First()),
                        _unitTextures[0], _hpBarTextureId);
                TilePositionObject tilePositionObject = new TilePositionObject(
                    _camera.GetPosition(),
                    new Vector2(unitToBeRendered.Location.X, unitToBeRendered.Location.Y));

                _wave.Units.Add(
                    (int) tilePositionObject.TilePosition.X * 1000 + (int) tilePositionObject.TilePosition.Y * 100000 +
                    _wave.UnitsCount, unitToBeRendered);
            }
        }

        public void MoveUnits(double speedMultiplier)
        {
            foreach (var unit in _wave.Units)
            {
                unit.Value.CheckLocation(_roadList);
                if (!unit.Value.IsDespawned)
                {
                    unit.Value.Move(speedMultiplier);
                }
            }

            SortedListHelper.Instance.RemoveAllFromSortedList(_wave.Units, x => x.Value.IsDespawned);
        }

        public SortedList<int, UnitBase> GetUnits()
        {
            return _wave.Units;
        }

        public void RenderUnits()
        {
            foreach (var unit in _wave.Units)
            {
                if (!unit.Value.IsDespawned)
                {
                    unit.Value.Draw(Color.White);
                }
            }
        }
    }
}