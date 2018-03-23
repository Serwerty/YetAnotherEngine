using System.Collections.Generic;
using System.Drawing;
using YetAnotherEngine.Constants;

namespace YetAnotherEngine.GameObjects.Drawables.Icons
{
    class IconsManager
    {
        private static IconsManager _instance;
        public static IconsManager GetInstance() => _instance ?? (_instance = new IconsManager());
        private readonly List<Icon> _iconsList = new List<Icon>();

        private IconsManager()
        {
        }

        public void Init(int goldIconTextureId, int damageIconTextureId, int rangeIconTextureId, 
            int heartIconTextureId, int timerIconTextureId, int towerSpeedIconTextureId)
        {
            _iconsList.Add(new Icon(goldIconTextureId, IconsConstants.GoldIconLocation, IconsConstants.GoldIconSize));
            _iconsList.Add(new Icon(goldIconTextureId, IconsConstants.PriceIconLocation, IconsConstants.PriceIconSize));
            _iconsList.Add(new Icon(damageIconTextureId, IconsConstants.DamageIconLocation, IconsConstants.DamageIconSize));
            _iconsList.Add(new Icon(rangeIconTextureId, IconsConstants.RangeIconLocation, IconsConstants.RangeIconSize));
            _iconsList.Add(new Icon(heartIconTextureId, IconsConstants.HeartIconLocation, IconsConstants.HeartIconSize));
            _iconsList.Add(new Icon(timerIconTextureId, IconsConstants.TimerIconLocation, IconsConstants.TimerIconSize));
            _iconsList.Add(new Icon(towerSpeedIconTextureId, IconsConstants.TowerSpeedIconLocation, 
                IconsConstants.TowerSpeedIconSize));
        }

        public void RenderIcons()
        {
            foreach (var icon in _iconsList)
            {
                icon.Draw(Color.White);
            }
        }
    }
}
