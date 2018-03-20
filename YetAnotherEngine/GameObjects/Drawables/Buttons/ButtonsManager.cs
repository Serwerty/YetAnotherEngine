using System.Collections.Generic;
using OpenTK;

namespace YetAnotherEngine.GameObjects.Drawables.Buttons
{
    class ButtonsManager
    {
        private static ButtonsManager _instance;

        public static ButtonsManager GetInstance() => _instance ?? (_instance = new ButtonsManager());
        private List<TextButton> _textButtons;

        private ButtonsManager()
        {

        }

        public void Init()
        {
            _textButtons = new List<TextButton>();
        }

        public void AddButton(TextButton buttonToAdd)
        {
            _textButtons.Add(buttonToAdd);
        }

        public void CheckClick(Vector2 mouseLocation)
        {
            foreach (var button in _textButtons)
            {
                if (button.IsMouseInside(mouseLocation)) button.Click(this);
            }
        }
    }
}
