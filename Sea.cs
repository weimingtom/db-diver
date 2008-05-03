using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DB.DoF.Entities;
using DB.Gui;

namespace DB.DoF
{
    public class Sea
    {
        public int Width;
        public int Height;
        public string Name;
        IDictionary<string, Room> rooms = new Dictionary<string, Room>();
        int currentRoomX, currentRoomY;
        Room room;
        Room.LeftRoomHandler leftRoomHandler;
        SpeedyDiver speedyDiver;
        FattyDiver fattyDiver;
        TinyDiver tinyDiver;
        Diver diver;

        public Sea(string name, 
                   int width, 
                   int height, 
                   int firstRoomX, 
                   int firstRoomY)
        {
            Width = width;
            Height = height;
            Name = name;
            this.currentRoomX = firstRoomX;
            this.currentRoomY = firstRoomY;

            speedyDiver = new SpeedyDiver();
            fattyDiver = new FattyDiver();
            tinyDiver = new TinyDiver();
            diver = speedyDiver;

            leftRoomHandler = new Room.LeftRoomHandler(OnLeftRoom);

            preloadAllRooms();
            MakeRoomActive(firstRoomX, firstRoomY);
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

        Room GetRoom(int x, int y)
        {
            string filename = DiverGame.DefaultContent.RootDirectory + "/" + Name + "_" + x + "_" + y + ".room";

            if (!rooms.ContainsKey(filename))
            {
                Room room;
                try
                {
                    room = Room.FromFile(filename, new SpriteGrid(DiverGame.DefaultContent.Load<Texture2D>("tileset"), 4, 4));
                    rooms[filename] = room;
                }
                catch (FileNotFoundException e)
                {
                    room = null;
                }

                return room;
            }
            else
            {
                return rooms[filename];
            }
        }

        void MakeRoomActive(int x, int y)
        {
            room = GetRoom(x, y);

            if (room == null)
            {
                throw new Exception("Cannot make a null room active! (" + x + "," + y + ")");
            }

            room.OnLeftRoom -= leftRoomHandler;
            room = GetRoom(x, y);
            room.OnLeftRoom += leftRoomHandler;
            currentRoomX = x;
            currentRoomY = y;
            room.Diver = diver;
        }
        
        void OnLeftRoom(Entity entity)
        {        
            if (entity == diver)
            {
                if (room.IsEntityLeftOfRoom(entity))
                {
                    MakeRoomActive(--currentRoomX, currentRoomY);
                }
                if (room.IsEntityRightOfRoom(entity))
                {
                    MakeRoomActive(++currentRoomX, currentRoomY);
                }         
            }

            if (room.IsEntityLeftOfRoom(entity))
            {
               entity.X = room.Size.X - 2;
            }

            if (room.IsEntityRightOfRoom(entity))
            {
                entity.X = -entity.Width + 1;
            }
        }

        public void Draw(Gui.Graphics g, GameTime gt)
        {
            g.PushClipRectangle(new Rectangle(0, 300 - room.Size.Y - 8, room.Size.X, room.Size.Y));
            g.Begin();
            g.Draw(DiverGame.White, new Rectangle(0, 0, room.Size.X, room.Size.Y), Color.CornflowerBlue);
            g.End();
            room.Draw(g, gt);
            g.PopClipRectangle();
        }

        public void Update(State s)
        {
            foreach(KeyValuePair<string, Room> keyValuePair in rooms)
            {
                if (keyValuePair.Value.IsUpdateNeeded() || room == keyValuePair.Value)
                    keyValuePair.Value.Update(s);
            }
        }
    }
}
