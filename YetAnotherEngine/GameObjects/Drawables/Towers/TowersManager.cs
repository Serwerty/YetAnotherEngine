﻿using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using YetAnotherEngine.Constants;
using YetAnotherEngine.Enums;
using YetAnotherEngine.GameObjects.World;
using YetAnotherEngine.Utils;
using YetAnotherEngine.Utils.Helpers;

namespace YetAnotherEngine.GameObjects.Drawables.Towers
{
    public class TowersManager
    {
        private readonly SortedList<int, TowerBase> _towersList = new SortedList<int, TowerBase>();
        private readonly TowerBase _towerToBePlaced;
        private readonly bool _shouldTowerBeRendered;
        private readonly int[] _towerTexures;

        public TowersManager(int[] towerTextures)
        {
            _towerTexures = towerTextures;
            _towerToBePlaced = new SimpleTower(new Vector2(0, 0), towerTextures[0]);

            _shouldTowerBeRendered = true;
        }

        public void AddTower(int mouseX, int mouseY, MapLoader mapLoader)
        {
            if (IsTowerPlaceable(mouseX, mouseY, mapLoader) && _shouldTowerBeRendered)
            {
                var tileOffset = new Vector2(SimpleTower.TowerCenterX - WorldConstants.TileWidth / 2,
                                             SimpleTower.TowerCenterY - WorldConstants.TileHeight / 4);

                var location = MouseHelper.Instance.TilePositionObject.TileCoords - tileOffset;

                TowerBase tower = new SimpleTower(location, _towerTexures[0]);

                _towersList.Add((int)MouseHelper.Instance.TilePositionObject.TilePosition.X * 100 + (int)MouseHelper.Instance.TilePositionObject.TilePosition.Y * 1000, tower);
            }
        }

        internal SortedList<int, TowerBase> GetTowers()
        {
            return _towersList;
        }

        public void RenderTowers()
        {
            foreach (var tower in _towersList)
            {
                tower.Value.Draw(Color.White);
            }
        }

        public void RenderTowerToBePlaced(Vector2 currentOffset, Vector2 mouseCords, int mouseX, int mouseY, MapLoader mapLoader)
        {
            if (!_shouldTowerBeRendered)
            {
                return;
            }

            _towerToBePlaced.Location = new Vector2(currentOffset.X + mouseCords.X * Game.MultiplierWidth - Game.NominalWidth / 2f - SimpleTower.TowerCenterX,
                                                    -currentOffset.Y + mouseCords.Y * Game.MultiplierHeight - Game.NominalHeight / 2f - SimpleTower.TowerCenterY);

            var towerColor = IsTowerPlaceable(mouseX, mouseY, mapLoader) ? WorldConstants.GreenColor : WorldConstants.RedColor;

            _towerToBePlaced.Draw(towerColor);
        }


        private bool IsTowerPlaceable(int mouseX, int mouseY, MapLoader mapLoader)
        {

            //Game._fpsText.WriteCoords($"Position: [{x}:{y}] " +
              //                        $"Location: [{MouseHelper.Instance.TilePositionObject.TileCoords.X}:{MouseHelper.Instance.TilePositionObject.TileCoords.Y}] " +
                //                     $"Mouse: [{_mouseDevice.X}:{_mouseDevice.Y}]");

            if (_towersList.ContainsKey(mouseX * 100 + mouseY * 1000) || mouseX < 0 ||
                mouseX >= WorldConstants.WorldHeight || mouseY < 0 || mouseY >= WorldConstants.WorldWidth)
            {
                return false;
            }

            return mapLoader.ConstructionTiles[mouseX, mouseY].Type == TileType.Tower;
        }
    }
}