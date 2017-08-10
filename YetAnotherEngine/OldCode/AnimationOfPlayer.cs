using System.Drawing;
using System.IO;

namespace YetAnotherEngine
{
    class AnimationOfPlayer
    {
        //count of pictures that will be in animation 
        private readonly Texture[] _animation;

        public AnimationOfPlayer(int count, string path)
        {
            if (!Directory.Exists(path)) return;

            _animation = new Texture[count];
            for (var i = 0; i < count; i++)
            {
                _animation[i] = new Texture(new Bitmap(path + (i + 1) + ".png"));
            }
        }

        //Bind Texture according to step 
        public void BindAnimation(int step)
        {
            _animation[step].Bind();
        }
    }
}
