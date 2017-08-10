using System.Collections.Generic;

namespace YetAnotherEngine
{
    public abstract class MovingObject
    {
        //private Vector2 Location;
        //private Vector2 Size;
        //private float constSpeed;

        public abstract void Move(List<Brush> world);
    }
}
