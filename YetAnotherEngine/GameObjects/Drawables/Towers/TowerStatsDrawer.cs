using YetAnotherEngine.Constants;

namespace YetAnotherEngine.GameObjects.Drawables.Towers
{
    class TowerStatsDrawer
    {
        private static TowerStatsDrawer _instance;

        public static TowerStatsDrawer GetInstance() => _instance ?? (_instance = new TowerStatsDrawer());

        private TowerStatsDrawer()
        {
        }

        private const float DefaultOffset = 1.5f;

        public void RenderStats(int towerDamage, int towerPrice, int towerRange, int towerShootingDelay)
        {
                    TextLine.Instane().WriteTextAtRelativePosition($"{towerDamage}", 2.2, IconsConstants.DamageIconLocation.X +
                                                                                  IconsConstants.DamageIconSize,
                IconsConstants.DamageIconLocation.Y + DefaultOffset);
            TextLine.Instane().WriteTextAtRelativePosition($"{towerPrice}", 2.2, IconsConstants.PriceIconLocation.X +
                                                                                 IconsConstants.PriceIconSize,
                IconsConstants.PriceIconLocation.Y + DefaultOffset);
            TextLine.Instane().WriteTextAtRelativePosition($"{towerRange}", 2.2, IconsConstants.RangeIconLocation.X +
                                                                                 IconsConstants.RangeIconSize,
                IconsConstants.RangeIconLocation.Y + DefaultOffset);
            TextLine.Instane().WriteTextAtRelativePosition($"{WorldConstants.TargetUpdateRate / (towerShootingDelay * 1f)} per second",
                2.2, IconsConstants.TowerSpeedIconLocation.X + IconsConstants.TowerSpeedIconSize,
                IconsConstants.TowerSpeedIconLocation.Y + DefaultOffset);
        }
    }
}