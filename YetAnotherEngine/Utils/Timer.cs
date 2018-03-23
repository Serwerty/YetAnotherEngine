
using YetAnotherEngine.Constants;

namespace YetAnotherEngine.Utils
{
    class Timer
    {
        public double CurrentTime { get; private set; }
        public double Exp = 0.000001;
        public bool IsOver { get; protected set; } = true;

        public Timer()
        {

        }

        public void StartTimer(double time)
        {
            CurrentTime = time * WorldConstants.TargetUpdateRate;
            IsOver = false;
        }

        public void Tick(double gameClockMultiplier)
        {
            if (CurrentTime < Exp) IsOver = true;
            CurrentTime -= gameClockMultiplier;
        }
    }
}
