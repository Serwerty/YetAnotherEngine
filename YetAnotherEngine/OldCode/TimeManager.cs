using System;

namespace YetAnotherEngine
{
    static class TimeManager
    {
        static float FrameInterval = 0.0f;
        static float frameTime = 0.0f;  // Время последнего кадра
        public static void  SetFrameInterval()
        {
            float currentTime = (float)Convert.ToInt64(System.DateTime.Now.Millisecond);

            // Интервал времени, прошедшего с прошлого кадра
            if (currentTime > frameTime)
            FrameInterval = currentTime - frameTime;
            else
                FrameInterval = 1000 - currentTime + frameTime;
            frameTime = currentTime;
        }
        public static float GetFrameInterval()
        {
            return FrameInterval;
        }
        //вернет кол-во "потеряных" fps 
    }
}