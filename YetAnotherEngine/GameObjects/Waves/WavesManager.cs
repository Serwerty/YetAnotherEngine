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
        private readonly Camera _camera;

        public WavesManager(List<Vector2> roadList, int[] unitTextures, Camera camera)
        {
            _unitTextures = unitTextures;
            _roadList = roadList;
            _wave = new Wave(UnitType.Basic, 150, 5);
            _camera = camera;
        }

        public void SpawnWave()
        {
            if (_wave.SpawnWave())
            {
                UnitBase unitToBeRendered =
                    new SimpleUnit(CoordsCalculator.CalculateLocationFromTilePosition(_roadList.First()),
                        _unitTextures[0]);
                TilePositionObject tilePositionObject = new TilePositionObject(
                    Vector2.Divide(_camera.GetPosition(), (float) Game.zScale),
                    new Vector2(unitToBeRendered.Location.X, unitToBeRendered.Location.Y));

                _wave.Units.Add(
                    (int) tilePositionObject.TilePosition.X * 100 + (int) tilePositionObject.TilePosition.Y * 1000 +
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
            //_wave.Units.RemoveAll(x => x.IsDespawned);
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