using System.Collections.Generic;
using OpenTK;
using YetAnotherEngine.Constants;
using YetAnotherEngine.GameObjects.Drawables.Towers;
using YetAnotherEngine.GameObjects.Drawables.Units;
using YetAnotherEngine.Utils.Helpers;

namespace YetAnotherEngine.GameObjects.Drawables.Projectiles
{
    public class ProjectilesManager
    {
        private readonly SortedList<int, ProjectileBase> _projectiles = new SortedList<int, ProjectileBase>();
        private int[] _projectilesTextures;
        private int _projectileImpactTextureId;
        public int Counter = 0;

        public ProjectilesManager(int[] projectilesTextures, int projectileImpactTextureId)
        {
            _projectilesTextures = projectilesTextures;
            _projectileImpactTextureId = projectileImpactTextureId;
        }

        internal SortedList<int, ProjectileBase> GetProjectiles()
        {
            return _projectiles;
        }


        public void MoveProjectiles(double speedMultiplier)
        {
            foreach (var projectile in _projectiles)
            {
                projectile.Value.Move(speedMultiplier);
            }

            SortedListHelper.Instance.RemoveAllFromSortedList(_projectiles, x => x.Value.IsDespawned);
        }

        public void AddProjectile(TowerBase tower, UnitBase targetUnit)
        {
            var tileOffset = new Vector2(SimpleTower.TowerCenterX - WorldConstants.TileWidth / 2,
                SimpleTower.TowerCenterY - WorldConstants.TileHeight / 4);

            var towerCenterLocation = tower.Location + tileOffset;


            Arrow arrow = new Arrow(towerCenterLocation, _projectilesTextures[0], _projectileImpactTextureId,
                targetUnit);
            _projectiles.Add((int) arrow.Location.X * 100 + (int) arrow.Location.Y * 10000 + Counter, arrow);
            Counter++;
        }
    }
}