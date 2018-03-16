using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using YetAnotherEngine.Constants;
using YetAnotherEngine.Enums;
using YetAnotherEngine.GameObjects.Drawables.Buttons;
using YetAnotherEngine.GameObjects.Drawables.Projectiles;
using YetAnotherEngine.GameObjects.Drawables.Units;
using YetAnotherEngine.GameObjects.World;
using YetAnotherEngine.Utils.Helpers;

namespace YetAnotherEngine.GameObjects.Drawables.Towers
{
    public class TowersManager
    {
        private readonly SortedList<int, TowerBase> _towersList = new SortedList<int, TowerBase>();

        private TowerBase _towerToBePlaced;

        private readonly TowerBase _simpleTowerToBePlaced;
        private readonly TowerBase _normalTowerToBePlaced;

        private TowerRangeField _towerRangeField;

        private readonly TowerRangeField _simpleTowerRangeField;
        private readonly TowerRangeField _normalTowerRangeField;

        private readonly bool _shouldTowerBeRendered;
        private readonly int[] _towerTexures;

        public TowersManager(int[] towerTextures, int towerRangeFieldTexture)
        {
            _towerTexures = towerTextures;
            _simpleTowerToBePlaced = new SimpleTower(new Vector2(0, 0), towerTextures[0]);
            _normalTowerToBePlaced = new NormalTower(new Vector2(0, 0), towerTextures[1]);
            _towerToBePlaced = _simpleTowerToBePlaced;
            //_towerToBePlaced = new NormalTower(new Vector2(0, 0), towerTextures[1]);
            _simpleTowerRangeField = new TowerRangeField(new Vector2(0, 0), towerRangeFieldTexture, _simpleTowerToBePlaced.Range);
            _normalTowerRangeField = new TowerRangeField(new Vector2(0, 0), towerRangeFieldTexture, _normalTowerToBePlaced.Range);
            _towerRangeField = _simpleTowerRangeField;
            _shouldTowerBeRendered = true;
        }

        public void AddTower(int mouseX, int mouseY, MapLoader mapLoader)
        {
            if (IsTowerPlaceable(mouseX, mouseY, mapLoader) && _shouldTowerBeRendered)
            {
                if (Gold.Instance().GoldValue >= _towerToBePlaced.Price)
                {
                    var tileOffset = new Vector2(SimpleTower.TowerCenterX - WorldConstants.TileWidth / 2,
                        SimpleTower.TowerCenterY - WorldConstants.TileHeight / 4);

                    var location = MouseHelper.Instance.TilePositionObject.TileCoords - tileOffset;

                    TowerBase tower = new SimpleTower(location, _towerTexures[0]);
                    if (TowerButton.GetInstance().SecondButtonSellected)
                        tower = new NormalTower(location, _towerTexures[1]);


                    _towersList.Add(
                        (int) MouseHelper.Instance.TilePositionObject.TilePosition.X * 1000 +
                        (int) MouseHelper.Instance.TilePositionObject.TilePosition.Y * 100000, tower);
                    Gold.Instance().GoldValue -= _towerToBePlaced.Price;
                }
                else
                {
                    Gold.Instance().StartNotEnoughGoldSequence();
                }
            }
        }

        internal SortedList<int, TowerBase> GetTowers()
        {
            return _towersList;
        }

        public void CheckTowersForShoot(SortedList<int, UnitBase> units, ref ProjectilesManager projectileManager)
        {
            foreach (var tower in _towersList)
            {
                tower.Value.CurrentShootigDelay--;
                if (tower.Value.CurrentShootigDelay <= 0)
                {
                    UnitBase targertUnit = tower.Value.GetTargetUnit(units);
                    if (targertUnit != null)
                    {
                        projectileManager.AddProjectile(tower.Value, targertUnit);
                        tower.Value.ResetDelay();
                    }
                }
            }
        }


        public void RenderTowers()
        {
            foreach (var tower in _towersList)
            {
                tower.Value.Draw(Color.White);
            }
        }

        public void RenderTowerToBePlaced(Vector2 currentOffset, Vector2 mouseCords, int mouseX, int mouseY,
            MapLoader mapLoader)
        {
            if (!_shouldTowerBeRendered)
            {
                return;
            }

            if (TowerButton.GetInstance().FirstButtonSellected)
            {
                _towerToBePlaced = _simpleTowerToBePlaced;
                _towerRangeField = _simpleTowerRangeField;
            }

            if (TowerButton.GetInstance().SecondButtonSellected)
            {
                _towerToBePlaced = _normalTowerToBePlaced;
                _towerRangeField = _normalTowerRangeField;
            }

            var location = new Vector2(
                (currentOffset.X + mouseCords.X - Game.CurrentWidth / 2f ) / Game.zScale -
                SimpleTower.TowerCenterX,
                (-currentOffset.Y + mouseCords.Y - Game.CurrentHeight / 2f ) / Game.zScale -
                SimpleTower.TowerCenterY);

            _towerToBePlaced.Location = location;

            _towerRangeField.Location = location - new Vector2(_towerRangeField.Range - SimpleTower.TowerCenterX, 0);

            var towerColor = IsTowerPlaceable(mouseX, mouseY, mapLoader)
                ? WorldConstants.GreenColor
                : WorldConstants.RedColor;

            _towerToBePlaced.Draw(towerColor);
            _towerRangeField.Draw(towerColor);
        }


        private bool IsTowerPlaceable(int mouseX, int mouseY, MapLoader mapLoader)
        {
            if (_towersList.ContainsKey(mouseX * 1000 + mouseY * 100000) || mouseX < 0 ||
                mouseX >= WorldConstants.WorldHeight || mouseY < 0 || mouseY >= WorldConstants.WorldWidth)
            {
                return false;
            }

            return mapLoader.ConstructionTiles[mouseX, mouseY].Type == TileType.Tower;
        }
    }
}