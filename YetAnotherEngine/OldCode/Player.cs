using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK;

namespace YetAnotherEngine
{
    public class Player : MovingObject
    {
        //hadling keyboard events with dictionary
        private readonly KeyboardDevice _keyboard;
        private readonly Gun _playerGun = new Gun(35f, 35f, 24f, 24f);

        //time without move 
        private float time = 0;

        //Step of moving 
        private int Step = 0;

        //nogravity mod  
        private bool nogravity = false;

        //Jumping variables  
        private bool WhenJump = false;
        private float TimeInJump = 0;
        private float ConstJumpingSpeed = 0.1f;
        private float CurrentSpeedinjump = 0.1f;


        //player vector location and size
        private Vector2 _location;
        private Vector2 _size;

        //direction in witch player is moving 
        private int direction = 1;
        //array of textures for animation 
        private AnimationOfPlayer animation = new AnimationOfPlayer(19, @"textures/TextureMan/");

        //constspeed
        private const float ConstSpeed = 0.05f;

        //singleton instance
        private static Player instance = null;

        //constspeed of gravity 
        private float gravity = ConstSpeed / 10;

        //constructor
        private Player(Vector2 location, Vector2 size, KeyboardDevice keyboard)
        {
            _location = location;
            _size = size;
            _keyboard = keyboard;
        }

        //singleton instance creation
        public static Player SetInstance(KeyboardDevice keyboard)
        {
            return instance ?? (instance = new Player(new Vector2(35, 35), new Vector2(24, 24), keyboard));
        }

        //moving handlers
        #region moving

        // moving <-
        private void MoveLeft()
        {
            _location.X -= TimeManager.GetFrameInterval() * ConstSpeed;
            Step++;
            direction = -1;
        }
        // moving ->
        private void MoveRight()
        {
            _location.X += TimeManager.GetFrameInterval() * ConstSpeed;
            Step++;
            direction = 1;
        }

        // moving ↑
        private void MoveUp()
        {
            _location.Y -= TimeManager.GetFrameInterval() * ConstSpeed;
        }

        //Falling of player
        private void Fall(float gravity)
        {
            _location.Y += TimeManager.GetFrameInterval() * gravity;
        }

        //moving ↓
        private void MoveDown()
        {
            _location.Y += TimeManager.GetFrameInterval() * ConstSpeed;
        }

        private void Jump(float speed)
        {
            _location.Y -= TimeManager.GetFrameInterval() * speed;
        }

        public override void Move(List<Brush> world)
        {
            if (_keyboard[Key.Space] && DownColision(world, ConstSpeed))
            {
                WhenJump = true;
            }

            //MoveLeft
            if (_keyboard[Key.A] && !LeftColision(world))
            {
                MoveLeft();
                time = 0;
            }
            else
            {
                time += 0.1f;
            }
            //MoveRight
            if (_keyboard[Key.D] && !RightColision(world))
            {
                MoveRight();
                time = 0;
            }
            else
            {
                time += 0.1f;
            }
            //MoveUp
            if (_keyboard[Key.W] && !UpColision(world, ConstSpeed) && nogravity)
                MoveUp();

            //MoveDown
            if (_keyboard[Key.S] && !DownColision(world, ConstSpeed) && nogravity)
                MoveDown();

            if (WhenJump && !UpColision(world, CurrentSpeedinjump))
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
            if (!DownColision(world, gravity) && !nogravity && !WhenJump)
            {
                gravity += ConstSpeed / 50;
                if (gravity > 5)
                    gravity = 5;
                Fall(gravity);
            }
            else
                gravity = ConstSpeed / 10;
            if ((int)time == 1)
                Step = 0;
        }

        #endregion

        //Draw Player Quad 
        public void Draw()
        {
            ChooseAnimation();
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(Color.Red);
            if (direction == 1)
            {
                GL.TexCoord2(0, 0);
                GL.Vertex2(_location);
                GL.TexCoord2(1, 0);
                GL.Vertex2(_location.X + _size.X, _location.Y);
                GL.TexCoord2(1, 1);
                GL.Vertex2(_location.X + _size.X, _location.Y + _size.Y);
                GL.TexCoord2(0, 1);
                GL.Vertex2(_location.X, _location.Y + _size.Y);
            }
            else
            {
                GL.TexCoord2(1, 0);
                GL.Vertex2(_location);
                GL.TexCoord2(0, 0);
                GL.Vertex2(_location.X + _size.X, _location.Y);
                GL.TexCoord2(0, 1);
                GL.Vertex2(_location.X + _size.X, _location.Y + _size.Y);
                GL.TexCoord2(1, 1);
                GL.Vertex2(_location.X, _location.Y + _size.Y);
            }
            GL.End();
            _playerGun.Draw(_location, direction);
        }

        #region Colisions

        //colision "->|"
        private bool RightColision(List<Brush> world)
        {
            for (int i = 0; i < world.Count; i++)
            {
                if (_location.X + _size.X + TimeManager.GetFrameInterval() * ConstSpeed >= world[i].Location.X 
                    && _location.X + _size.X < world[i].Location.X + world[i].Size.X 
                    && _location.Y + _size.Y > world[i].Location.Y 
                    && _location.Y < world[i].Location.Y + world[i].Size.Y)
                {
                    _location.X = world[i].Location.X - _size.X;
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
                if (_location.X - TimeManager.GetFrameInterval() * ConstSpeed <= world[i].Location.X + world[i].Size.X 
                    && _location.X > world[i].Location.X 
                    && _location.Y + _size.Y > world[i].Location.Y 
                    && _location.Y < world[i].Location.Y + world[i].Size.Y)
                {
                    _location.X = world[i].Location.X + world[i].Size.X;
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
                if (_location.Y - TimeManager.GetFrameInterval() * ConstSpeed <= world[i].Location.Y + world[i].Size.Y 
                    && _location.Y > world[i].Location.Y 
                    && _location.X + _size.X > world[i].Location.X 
                    && _location.X < world[i].Location.X + world[i].Size.X)
                {
                    _location.Y = world[i].Location.Y + world[i].Size.Y;
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
                if (_location.Y + _size.Y + TimeManager.GetFrameInterval() * ConstSpeed >= world[i].Location.Y 
                    && _location.Y + _size.Y < world[i].Location.Y + world[i].Size.Y 
                    && _location.X + _size.X > world[i].Location.X 
                    && _location.X < world[i].Location.X + world[i].Size.X)
                {
                    _location.Y = world[i].Location.Y - _size.Y;
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

        public Vector2 GetPlayerLocation()
        {
            return _location;
        }
    }
}
