using OpenTK;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using YetAnotherEngine.Constants;
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
        private Wave _waveToBeSpawned;
        private readonly List<Vector2> _roadList;
        private readonly int[] _unitTextures;
        private readonly int _hpBarTextureId;
        private readonly Camera _camera;
        private readonly int _wavesCount;
        private int _currentWaveNumber;

        private const double TimerTillNextWave = WorldConstants.TargetUpdateRate * 5;
        private double _currentTimer;
        private const double Exp = 0.00000001;

        public WavesManager(List<Vector2> roadList, int[] unitTextures, Camera camera, int hpBarTextureId)
        {
            _unitTextures = unitTextures;
            _roadList = roadList;
            _waveToBeSpawned = new Wave(UnitType.Basic, 10, 0.5f);
            _camera = camera;
            _hpBarTextureId = hpBarTextureId;
            _wavesCount = 4;
            _currentWaveNumber = 1;
            _currentTimer = 0;
        }

        public void SpawnWave(float gameClockMultiplier)
        {
            if (_waveToBeSpawned.isSpawned && _waveToBeSpawned.Units.Count == 0 && _currentTimer <= Exp)
            {
                _currentWaveNumber++;
                _currentTimer = TimerTillNextWave;
                if (_currentWaveNumber <= _wavesCount)
                {
                    _waveToBeSpawned = new Wave(UnitType.Basic, 10, 0.5f);
                }
                else
                {
                    //TODO: Level Passed
                }
            }

            if (_currentTimer > Exp)
            {
                _currentTimer -= gameClockMultiplier;
            }

            if (_waveToBeSpawned.SpawnWave(gameClockMultiplier) && _currentTimer <= Exp)
            {
                UnitBase unitToBeRendered =
                    new SimpleUnit(CoordsCalculator.CalculateLocationFromTilePosition(_roadList.First()),
                        _unitTextures[0], _hpBarTextureId);
                TilePositionObject tilePositionObject = new TilePositionObject(
                    _camera.GetPosition(),
                    new Vector2(unitToBeRendered.Location.X, unitToBeRendered.Location.Y));

                _waveToBeSpawned.Units.Add(
                    (int) tilePositionObject.TilePosition.X * 1000 + (int) tilePositionObject.TilePosition.Y * 100000 +
                    _waveToBeSpawned.UnitsCount, unitToBeRendered);
            }     
        }

        public void MoveUnits(double speedMultiplier)
        {
            foreach (var unit in _waveToBeSpawned.Units)
            {
                unit.Value.CheckLocation(_roadList);
                if (!unit.Value.IsDespawned)
                {
                    unit.Value.Move(speedMultiplier);
                }
            }

            SortedListHelper.Instance.RemoveAllFromSortedList(_waveToBeSpawned.Units, x => x.Value.IsDespawned);
        }

        public SortedList<int, UnitBase> GetUnits()
        {
            return _waveToBeSpawned.Units;
        }

        public void RenderUnits()
        {
            foreach (var unit in _waveToBeSpawned.Units)
            {
                if (!unit.Value.IsDespawned)
                {
                    unit.Value.Draw(Color.White);
                }
            }
        }

        public void RenderWaveText()
        {
            GL.Color4(Color.White);
            TextLine.Instane().WriteTextAtRelativePosition($"Wave: {_currentWaveNumber}/{_wavesCount}", 
                TextConstants.WaveTextSize, TextConstants.WaveTextLocationX, TextConstants.WaveTextLocationY);
        }
    }
}