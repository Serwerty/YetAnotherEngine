using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK;

namespace YetAnotherEngine
{
    abstract class MovingObject
    {
        private Vector2 Location;
        private Vector2 Size;
        private float constSpeed;


        public abstract void Move(List<Brush> world);
    }
}
