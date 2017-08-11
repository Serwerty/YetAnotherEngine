using System;
using YetAnotherEngine.Constants;

namespace YetAnotherEngine
{
    public class Application
    {
        //application entry point
        [STAThread]
        static void Main()
        {
            //Launching new game window
            using (var game = new Game())
            {
                game.Run(WorldConstants.TargetUpdateRate, 0);
            }
        }

    }
}
