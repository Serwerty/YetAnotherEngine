using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK;

namespace YetAnotherEngine
{
    class Player : MovingObject
    {

        //hadling keyboard events with dictionary
        private OpenTK.Input.KeyboardDevice Keyboard;
        private Gun  Gun1= new Gun(35f,35f,24f,24f);
       
        //time without move 
        private float time = 0;

        //Step of moving 
        private int Step = 0;
        
        //nogravity mod  
        private bool nogravity = false;
        
        //Jumping variables  
        private bool WhenJump=false;
        private float TimeInJump=0;
        private float ConstJumpingSpeed = 0.1f;
        private float CurrentSpeedinjump = 0.1f;


        //player vector location and size
        private Vector2 Location; 
        private Vector2 Size;

        //direction in witch player is moving 
        private int direction = 1;
        //array of textures for animation 
        private AnimationOfPlayer animation = new AnimationOfPlayer(19, @"textures/TextureMan/");
        
        //constspeed
        private const float ConstSpeed = 0.05f;

        //singleton instance
        private static Player instance = null;

        //constspeed of gravity 
        private float gravity = ConstSpeed/10;

        //constructor
        private Player(Vector2 location, Vector2 size, OpenTK.Input.KeyboardDevice keyboard)
        {
            Location = location;
            Size = size;
            Keyboard = keyboard;
        }
       
        //singleton instance creation
        public static Player SetInstance(OpenTK.Input.KeyboardDevice Keyboard)
        {
            if (instance == null)
            {
                instance = new Player(new Vector2(35, 35), new Vector2(24, 24), Keyboard);
            }
            return instance;
        }

        //moving handlers
        #region moving

        // moving <-
        private void MoveLeft()
        {
            Location.X-=TimeManager.GetFrameInterval()*ConstSpeed;
            Step++;
            direction = -1;
        }
        // moving ->
        private void MoveRight()
        {
            Location.X += TimeManager.GetFrameInterval() * ConstSpeed;
            Step++;
            direction = 1;
        }

        // moving ↑
        private void MoveUp()
        {
            Location.Y -= TimeManager.GetFrameInterval() * ConstSpeed;
        }

        //Falling of player
        private void Fall(float gravity)
        {
            Location.Y += TimeManager.GetFrameInterval() * gravity;
        }

        //moving ↓
        private void MoveDown()
        {
            Location.Y += TimeManager.GetFrameInterval() * ConstSpeed;
        }

        private void Jump(float speed)
        {
            Location.Y -= TimeManager.GetFrameInterval() * speed;
        }

        public override void Move(List<Brush> world)
        {
            if (Keyboard[Key.Space] && DownColision(world, ConstSpeed))
            {
               WhenJump=true;
            }
            
            //MoveLeft
            if (Keyboard[Key.A] && !LeftColision(world))
            {
                MoveLeft();
                time = 0;
            }
            else
            {
                time += 0.1f;
            }
                //MoveRight
            if (Keyboard[Key.D] && !RightColision(world))
            {
                MoveRight();
                time = 0;
            }
            else
            {
                time += 0.1f;
            }
                    //MoveUp
           if (Keyboard[Key.W] && !UpColision(world,ConstSpeed) && nogravity)
                MoveUp();

            //MoveDown
            if (Keyboard[Key.S] && !DownColision(world,ConstSpeed) && nogravity)
                MoveDown();

            if (WhenJump && !UpColision(world,CurrentSpeedinjump))
            {
                TimeInJump++;
                Jump(TimeManager.GetFrameInterval() * (CurrentSpeedinjump -= 0.00028f));
                if (Math.Round(CurrentSpeedinjump, 6) == 0 || TimeInJump == 100)
                {
                    WhenJump = false;
                    CurrentSpeedinjump = ConstJumpingSpeed;
                }
            }

            //Falling 
            if (!DownColision(world,gravity) && !nogravity && !WhenJump)
            {
                gravity += ConstSpeed / 50;
                if (gravity > 5)
                    gravity = 5;
                Fall(gravity);
            }
            else
                gravity = ConstSpeed/10;
            if ((int)time == 1)
                Step = 0;
        }

        #endregion

        //Draw Player Quad 
        public void Draw()
        {
            ChooseAnimation();
            GL.Begin(BeginMode.Quads);
            GL.Color4(Color.Red);
            if (direction == 1)
            {
                GL.TexCoord2(0, 0);
                GL.Vertex2(Location);
                GL.TexCoord2(1, 0);
                GL.Vertex2(Location.X + Size.X, Location.Y);
                GL.TexCoord2(1, 1);
                GL.Vertex2(Location.X + Size.X, Location.Y + Size.Y);
                GL.TexCoord2(0, 1);
                GL.Vertex2(Location.X, Location.Y + Size.Y);
            }
            else
            {
                GL.TexCoord2(1, 0);
                GL.Vertex2(Location);
                GL.TexCoord2(0, 0);
                GL.Vertex2(Location.X + Size.X, Location.Y);
                GL.TexCoord2(0, 1);
                GL.Vertex2(Location.X + Size.X, Location.Y + Size.Y);
                GL.TexCoord2(1, 1);
                GL.Vertex2(Location.X, Location.Y + Size.Y);    
            }
            GL.End();
            Gun1.Draw(Location, direction);
        }

        #region Colisions

        //colision "->|"
        private bool RightColision(List<Brush> world)
        {
            for (int i = 0; i<world.Count; i++)
            {
                if (Location.X + Size.X+TimeManager.GetFrameInterval() * ConstSpeed >= world[i].GetLocation().X && Location.X + Size.X < world[i].GetLocation().X + world[i].GetSize().X && Location.Y + Size.Y > world[i].GetLocation().Y && Location.Y < world[i].GetLocation().Y + world[i].GetSize().Y)
                {
                    Location.X = world[i].GetLocation().X - Size.X;
                    Step = 0;
                    return true;
                }
            }
            return false;
        }

        //colision "|<-"
        private bool LeftColision(List<Brush> world)
        {
            for (int i = 0; i < world.Count; i++)
            {
                if (Location.X-TimeManager.GetFrameInterval() * ConstSpeed <= world[i].GetLocation().X + world[i].GetSize().X && Location.X > world[i].GetLocation().X && Location.Y + Size.Y > world[i].GetLocation().Y && Location.Y < world[i].GetLocation().Y + world[i].GetSize().Y)
                {
                    Location.X = world[i].GetLocation().X + world[i].GetSize().X;
                    Step = 0;
                    return true;
                }
            }
            return false;
        }

        //........."_"
        //colision "↑"
        private bool UpColision(List<Brush> world, float ConstSpeed)
        {
            for (int i = 0; i < world.Count; i++)
            {
                if (Location.Y-TimeManager.GetFrameInterval() * ConstSpeed <= world[i].GetLocation().Y + world[i].GetSize().Y && Location.Y > world[i].GetLocation().Y && Location.X + Size.X > world[i].GetLocation().X && Location.X < world[i].GetLocation().X + world[i].GetSize().X)
                {
                    Location.Y = world[i].GetLocation().Y + world[i].GetSize().Y;
                    CurrentSpeedinjump = ConstJumpingSpeed;
                    WhenJump = false;
                    Step = 0;
                    return true;
                }
            }
            return false;
        }

        //colision "↓"
        //........."¯"
        private bool DownColision(List<Brush> world, float ConstSpeed)
        {
            for (int i = 0; i < world.Count; i++)
            {
                if (Location.Y + Size.Y+TimeManager.GetFrameInterval() * ConstSpeed>= world[i].GetLocation().Y && Location.Y + Size.Y < world[i].GetLocation().Y + world[i].GetSize().Y && Location.X + Size.X > world[i].GetLocation().X && Location.X < world[i].GetLocation().X + world[i].GetSize().X)
                {
                    Location.Y = world[i].GetLocation().Y - Size.Y;
                    CurrentSpeedinjump = ConstJumpingSpeed;
                    WhenJump = false;
                    return true;
                }
            }
            return false;
        }
        #endregion 

        //choose animation to bind it (depends on cirrent step)
        private void ChooseAnimation()
        {
            if (Step == 0)
                animation.BindAnimation(0);
            else if (Step < 3 && Step > 0)
                animation.BindAnimation(1);
            else if (Step >= 3 * 18)
            {
                Step = 1;
                animation.BindAnimation(1);
            }
            else
                animation.BindAnimation(Step / 3 + 1);
        }

        public Vector2 GetLocation()
        {
            return Location;
        }
    }
}
