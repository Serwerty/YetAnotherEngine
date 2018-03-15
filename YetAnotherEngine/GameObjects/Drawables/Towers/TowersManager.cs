using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using YetAnotherEngine.Constants;
using YetAnotherEngine.Enums;
using YetAnotherEngine.GameObjects.Drawables.Projectiles;
using YetAnotherEngine.GameObjects.Drawables.Units;
using YetAnotherEngine.GameObjects.World;
using YetAnotherEngine.Utils.Helpers;

namespace YetAnotherEngine.GameObjects.Drawables.Towers
{
    public class TowersManager
    {
        private readonly SortedList<int, TowerBase> _towersList = new SortedList<int, TowerBase>();
        private readonly TowerBase _towerToBePlaced;
        private readonly TowerRangeField _towerRangeField;
        private readonly bool _shouldTowerBeRendered;
        private readonly int[] _towerTexures;

        public TowersManager(int[] towerTextures, int towerRangeFieldTexture)
        {
            _towerTexures = towerTextures;
            //_towerToBePlaced = new SimpleTower(new Vector2(0, 0), towerTextures[0]);
            _towerToBePlaced = new NormalTower(new Vector2(0, 0), towerTextures[1]);
            _towerRangeField = new TowerRangeField(new Vector2(0, 0), towerRangeFieldTexture, _towerToBePlaced.Range);
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

                    TowerBase tower = new NormalTower(location, _towerTexures[1]);

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

            var location = new Vector2(
                currentOffset.X + mouseCords.X * Game.zScale - Game.NominalWidth / 2f -
                SimpleTower.TowerCenterX,
                -currentOffset.Y + mouseCords.Y * Game.zScale - Game.NominalHeight/ 2f -
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