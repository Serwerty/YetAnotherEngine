using YetAnotherEngine.Constants;

namespace YetAnotherEngine
{
    public class Application
    {
        static void Main()
        {
            using (var game = new Game())
            {
                game.Run(WorldConstants.TargetUpdateRate, 0);
            }
        }
    }
}