using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DB.DoF
{
    public class Sea
    {
        public int Width;
        public int Height;
        public string Name;
        private IDictionary<string, Room> rooms = new Dictionary<string, Room>();

        public Sea(string name, int width, int height)
        {
            Width = width;
            Height = height;
            Name = name;
        }

        public void preloadAllRooms()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    GetRoom(x, y);
                }
            }
        }

        public Room GetRoom(int x, int y)
        {
            string filename = DiverGame.DefaultContent.RootDirectory + "/" + Name + "_" + x + "_" + y + ".room";            

            if (!rooms.ContainsKey(filename))
            {
                Room room = Room.FromFile(filename, new SpriteGrid(DiverGame.DefaultContent.Load<Texture2D>("tileset"), 4, 4));
                rooms[filename] = room;
                
            }

            return rooms[filename];
        }
    }
}
