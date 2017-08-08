using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK;

namespace YetAnotherEngine
{
    class AnimationOfPlayer
    {
        //count of pictures that will be in animation 
        private int CountOfFrames;
        private Texture[] animation;

        public AnimationOfPlayer(int count, string path)
        {
            if (Directory.Exists(path))
            {
                CountOfFrames = count;
                animation = new Texture[count];
                for (int i = 0; i < count; i++)
                {
                    animation[i] = new Texture(new Bitmap(path + (i + 1).ToString() + ".png"));
                }
            }
        }

        //Bind Texture according to step 
        public void BindAnimation(int step)
        {
            animation[step].Bind();
        }
    }
}
