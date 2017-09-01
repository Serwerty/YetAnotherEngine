using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using YetAnotherEngine.Constants;
using YetAnotherEngine.Enums;
using YetAnotherEngine.Utils;

namespace YetAnotherEngine.GameObjects.Towers
{
    public class TowersManager
    {
        private readonly SortedList<int, TowerBase> _towersList;
        private readonly TowerBase _towerToBePlaced;
        private readonly bool _shouldTowerBeRendered;

        public TowersManager()
        {
            _towersList = new SortedList<int, TowerBase>();
            _towerToBePlaced = new SimpleTower(new Vector2(0, 0), _mapTextures.TowerTextures[0]);

            _shouldTowerBeRendered = true;
        }

        public void AddTower()
        {
            if (IsTowerPlaceable() && _shouldTowerBeRendered)
            {
                var tileOffset = new Vector2(SimpleTower.TowerCenterX - WorldConstants.TileWidth / 2,
                                             SimpleTower.TowerCenterY - WorldConstants.TileHeight / 4);

                var location = MouseHelper.Instance.TilePositionObject.TileCoords - tileOffset;

                TowerBase tower = new SimpleTower(location, _mapTextures.TowerTextures[0]);

                _towersList.Add((int)MouseHelper.Instance.TilePositionObject.TilePosition.X * 100 + (int)MouseHelper.Instance.TilePositionObject.TilePosition.Y, tower);
            }
        }

        public void RenderTowers()
        {
            foreach (var tower in _towersList)
            {
                tower.Value.Draw(Color.White);
            }
        }

        public void RenderTowerToBePlaced(Vector2 currentOffset)
        {
            if (!_shouldTowerBeRendered || !_keyboardDevice[Key.T])
            {
                return;
            }

            _towerToBePlaced.Location = new Vector2(currentOffset.X + _mouseDevice.X - Game.NominalWidth / 2f - SimpleTower.TowerCenterX,
                                                    -currentOffset.Y + _mouseDevice.Y - Game.NominalHeight / 2f - SimpleTower.TowerCenterY);

            var towerColor = IsTowerPlaceable() ? WorldConstants.GreenColor : WorldConstants.RedColor;

            _towerToBePlaced.Draw(towerColor);
        }


        private bool IsTowerPlaceable()
        {
            var x = (int)MouseHelper.Instance.TilePositionObject.TilePosition.X;
            var y = (int)MouseHelper.Instance.TilePositionObject.TilePosition.Y;
            //Game._fpsText.WriteCoords($"Position: [{x}:{y}] " +
            //                          $"Location: [{MouseHelper.Instance.TilePositionObject.TileCoords.X}:{MouseHelper.Instance.TilePositionObject.TileCoords.Y}] " +
            //                          $"Mouse: [{_mouseDevice.X}:{_mouseDevice.Y}]");

            if (_towersList.ContainsKey(x * 100 + y) || x < 0 ||
                x >= WorldConstants.WorldHeight || y < 0 || y >= WorldConstants.WorldWidth)
            {
                return false;
            }

            return _mapLoader.ConstructionTiles[x, y].Type == TileType.Tower;
        }
    }
}
