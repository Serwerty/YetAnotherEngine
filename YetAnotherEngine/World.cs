using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK;

namespace YetAnotherEngine
{
    class World
    {
        private static World Instance = null ;
        private List<Brush> WorldObjects = new List<Brush>();


        //background texture
        private Texture BackGround;

        private World()
        {
        }

        public static World CreateInstance()
        {
            if (Instance == null)
            {
                Instance = new World();
            }
            return Instance;
        }

        // TODO: load player position from file
        // TODO: load background image from file
        public void LoadMap(string mapName)
        {
            string directory = Directory.GetCurrentDirectory() + @"/map/" + mapName;
            if (!File.Exists(directory))
            {
                throw new FileNotFoundException("Карта не найдена");
            }
            BackGround = new Texture(new Bitmap("textures/background.png"));
            WorldObjects.Clear();
            using (StreamReader sr = new StreamReader(directory))
            {

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    string[] point = line.Split(' ');

                    Vector2 Location;
                    Vector2 Size;

                    Location.X = (float)Convert.ToDouble(point[0]);
                    Location.Y = (float)Convert.ToDouble(point[1]);
                    Size.X = (float)Convert.ToDouble(point[2]);
                    Size.Y = (float)Convert.ToDouble(point[3]);

                    WorldObjects.Add(new Brush(Location,Size));
                }
            }

        }

        public void DrawWorld()
        {
            GL.Begin(BeginMode.Quads);

            for (int i = 0; i < WorldObjects.Count; i++)
            {
                WorldObjects[i].Draw();
            }

            GL.End();
        }

        public void RenderBackground(float Width, float Height, float ProjectionWidth, float ProjectionHeight,Player pl)
        {
            BackGround.Bind();
            GL.Color4(Color4.White);
            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(0, 0);
            GL.Vertex2(-Width / 2 + pl.GetLocation().X, -Height / 2 + pl.GetLocation().Y);

            GL.TexCoord2(Width / ProjectionWidth, 0);
            GL.Vertex2(Width / 2 + pl.GetLocation().X, -Height / 2 + pl.GetLocation().Y);

            GL.TexCoord2(Width / ProjectionWidth, Height / ProjectionHeight);
            GL.Vertex2(Width / 2 + pl.GetLocation().X, Height / 2 + pl.GetLocation().Y);

            GL.TexCoord2(0, Height / ProjectionHeight);
            GL.Vertex2(-Width / 2 + pl.GetLocation().X, Height / 2 + pl.GetLocation().Y);

            GL.End();
        }
        public List<Brush> GetWorldObjects()
        {
            return WorldObjects;
        }
    }
}
