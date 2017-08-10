//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Drawing;
//using OpenTK.Graphics;
//using OpenTK.Graphics.OpenGL;
//using OpenTK;

//namespace YetAnotherEngine
//{
//    public class World
//    {
//        private static World _instance;
//        private readonly List<Brush> _worldObjects = new List<Brush>();

//        //background texture
//        private Texture _backGround;

//        private World()
//        {
//        }

//        public static World CreateInstance()
//        {
//            return _instance ?? (_instance = new World());
//        }

//        // TODO: load player position from file
//        // TODO: load background image from file
//        public void LoadMap(string mapName)
//        {
//            string directory = $"{Directory.GetCurrentDirectory()}/map/{mapName}";
//            if (!File.Exists(directory))
//            {
//                throw new FileNotFoundException("Map file not found");
//            }

//            _backGround = new Texture(new Bitmap("textures/background.png"));
//            _worldObjects.Clear();

//            using (var sr = new StreamReader(directory))
//            {
//                while (!sr.EndOfStream)
//                {
//                    var point = sr.ReadLine().Split(' ');

//                    Vector2 location;
//                    Vector2 size;

//                    location.X = (float)Convert.ToDouble(point[0]);
//                    location.Y = (float)Convert.ToDouble(point[1]);
//                    size.X = (float)Convert.ToDouble(point[2]);
//                    size.Y = (float)Convert.ToDouble(point[3]);

//                    _worldObjects.Add(new Brush(location,size));
//                }
//            }
//        }

//        public void DrawWorld()
//        {
//            GL.Begin(PrimitiveType.Quads);

//            for (var i = 0; i < _worldObjects.Count; i++)
//            {
//                _worldObjects[i].Draw();
//            }

//            GL.End();
//        }

//        public void RenderBackground(float width, float height, float projectionWidth, float projectionHeight, Player pl)
//        {
//            _backGround.Bind();
//            GL.Color4(Color4.White);
//            GL.Begin(PrimitiveType.Quads);

//            GL.TexCoord2(0, 0);
//            GL.Vertex2(-width / 2 + pl.GetPlayerLocation().X, -height / 2 + pl.GetPlayerLocation().Y);

//            GL.TexCoord2(width / projectionWidth, 0);
//            GL.Vertex2(width / 2 + pl.GetPlayerLocation().X, -height / 2 + pl.GetPlayerLocation().Y);

//            GL.TexCoord2(width / projectionWidth, height / projectionHeight);
//            GL.Vertex2(width / 2 + pl.GetPlayerLocation().X, height / 2 + pl.GetPlayerLocation().Y);

//            GL.TexCoord2(0, height / projectionHeight);
//            GL.Vertex2(-width / 2 + pl.GetPlayerLocation().X, height / 2 + pl.GetPlayerLocation().Y);

//            GL.End();
//        }
//        public List<Brush> GetWorldObjects()
//        {
//            return _worldObjects;
//        }
//    }
//}
