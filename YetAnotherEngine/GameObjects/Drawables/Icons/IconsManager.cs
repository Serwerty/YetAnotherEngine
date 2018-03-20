using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
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

        public void Init(int goldIconTextureId, int damageIconTextureId, int rangeIconTextureId, int heartIconTextureId)
        {
            _iconsList.Add(new Icon(goldIconTextureId, IconsConstants.GoldIconLocation, IconsConstants.GoldIconSize));
            _iconsList.Add(new Icon(goldIconTextureId, IconsConstants.PriceIconLocation, IconsConstants.PriceIconSize));
            _iconsList.Add(new Icon(damageIconTextureId, IconsConstants.DamageIconLocation, IconsConstants.DamageIconSize));
            _iconsList.Add(new Icon(rangeIconTextureId, IconsConstants.RangeIconLocation, IconsConstants.RangeIconSize));
            _iconsList.Add(new Icon(heartIconTextureId, IconsConstants.HeartIconLocation, IconsConstants.HeartIconSize));
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
