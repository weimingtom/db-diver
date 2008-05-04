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

        int firstRoomX, firstRoomY;
        Room currentRoom;
        Room boat;
        SpeedyDiver speedyDiver;
        FattyDiver fattyDiver;
        TinyDiver tinyDiver;
        Diver diver;
        MiniMap miniMap;
        Texture2D panel;
        SpriteGrid tileSet;
        bool isMiniMapShowing;

        private class EntityTransition
        {
            public EntityTransition(Entity entity, Room room)
            {
                Entity = entity;
                Room = room;
            }
            public Entity Entity;
            public Room Room;
        }

        IList<EntityTransition> entityTransitions = new List<EntityTransition>();

        public Sea(string name, 
                   int width, 
                   int height,
                   int firstRoomX,
                   int firstRoomY)
        {
            Width = width;
            Height = height;
            Name = name;
            this.firstRoomX = firstRoomX;
            this.firstRoomY = firstRoomY;

            tileSet = new SpriteGrid(DiverGame.DefaultContent.Load<Texture2D>("tileset"), 4, 4);

            speedyDiver = new SpeedyDiver();
            fattyDiver = new FattyDiver();
            tinyDiver = new TinyDiver();
            diver = speedyDiver;

            miniMap = new MiniMap(this);

            // To load new game...
            PreloadAllRooms(false);
            // or, to load a saved game (also preloads rooms)
            // LoadState();
           
            panel = DiverGame.DefaultContent.Load<Texture2D>("panel");
            boat = Room.FromFile(DiverGame.DefaultContent.RootDirectory + "/" + name + "_boat.room", tileSet, false, this);
            rooms[DiverGame.DefaultContent.RootDirectory + "/" + name + "_boat.room"] = boat;
            EnterBoat();
        }

        public void PreloadAllRooms(bool skipPersistent)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    LoadRoom(x, y, skipPersistent);
                }
            }
        }

        void LoadRoom(int x, int y, bool skipPersistent)
        {
            string filename = DiverGame.DefaultContent.RootDirectory + "/" + Name + "_" + x + "_" + y + ".room";

            Room room;
            try
            {
                room = Room.FromFile(filename, tileSet, skipPersistent, this);
                rooms[filename] = room;
            }
            catch (FileNotFoundException e)
            {
                System.Console.WriteLine("Couldn't find room " + filename + " " + e);
                room = null;
            }

            if (room != null)
            {
                room.SeaX = x;
                room.SeaY = y;
            }

            rooms[filename] = room;
        }

        Room GetRoom(int x, int y)
        {
            string filename = DiverGame.DefaultContent.RootDirectory + "/" + Name + "_" + x + "_" + y + ".room";

            if (!rooms.ContainsKey(filename))
            {
                LoadRoom(x, y, false);
            }
           
            return rooms[filename]; 
        }

        void MakeRoomActive(int x, int y)
        {
            currentRoom = GetRoom(x, y);
            currentRoom.OnDiverLeftRoom();

            if (currentRoom == null)
            {
                throw new Exception("Cannot make a null room active! (" + x + "," + y + ")");
            }

            currentRoom.Diver = diver;

            miniMap.Discover(currentRoom);
            diver.OxygenDecrease = true;
            diver.OxygenIncrease = false;
            diver.JumpEnabled = true;
        }

        public void EnterBoat()
        {
            if (currentRoom != null)
                currentRoom.OnDiverLeftRoom();

            currentRoom = boat;
            diver.X = 200;
            diver.Y = 200;
            currentRoom.Diver = diver;
            currentRoom.SeaX = firstRoomX;
            currentRoom.SeaY = firstRoomY - 1;
            diver.OxygenDecrease = false;
            diver.OxygenIncrease = true;
            diver.JumpEnabled = false;
        }

        void OnLeftBoat(Entity entity)
        {
            if (entity == diver)
            {
                MakeRoomActive(firstRoomX, firstRoomY);
                entity.Y = -entity.Height + 1;
                entity.X = boat.Size.X / 2 - entity.Width / 2;
            }
        }

        public void OnLeftRoom(Entity entity, Room room)
        {
            if (room == boat)
            {
                OnLeftBoat(entity);
                return;
            }

            if (entity == diver)
            {
                if (room.IsEntityLeftOfRoom(entity))
                {
                    MakeRoomActive(1 - room.SeaX, room.SeaY);
                    entity.X = room.Size.X - 2;
                }

                if (room.IsEntityRightOfRoom(entity))
                {
                    MakeRoomActive(1 + room.SeaX, room.SeaY);
                    entity.X = -entity.Width + 1;
                }

                if (room.IsEntityBelowRoom(entity))
                {
                    MakeRoomActive(room.SeaX, 1 + room.SeaY);
                    entity.Y = -entity.Height + 1;
                }
                if (room.IsEntityAboveRoom(entity))
                {
                    MakeRoomActive(room.SeaX, 1 - room.SeaY);
                    entity.Y = room.Size.Y - 2;
                }

                return;
            }
            
            if (entity.IsTransitionable)
            {
                EntityTransition entityTransistion = new EntityTransition(entity, room);
                entityTransitions.Add(entityTransistion);
            }
        }

        public void Draw(Gui.Graphics g, GameTime gt)
        {
            g.PushClipRectangle(new Rectangle(0, 0, currentRoom.Size.X, currentRoom.Size.Y));
            g.Begin();
            g.Draw(DiverGame.White, new Rectangle(0, 0, currentRoom.Size.X, currentRoom.Size.Y), new Color(74, 193, 231));
            g.End();
            currentRoom.Draw(g, gt);
            g.PopClipRectangle();

            g.PushClipRectangle(new Rectangle(0, currentRoom.Size.Y, 400, 300 - currentRoom.Size.Y));
            g.Begin();
            g.Draw(panel, new Point(0, 0), Color.White);
            g.Draw(DiverGame.White, new Rectangle(16, 4, (122 * diver.Oxygen) / Diver.MaxOxygen, 4), new Color(199, 77, 77));

            g.Draw(currentRoom.Diver.Tool1.Icon, new Point(151, 6), Color.White);
            
            g.End();
            g.PopClipRectangle();

            

            if (isMiniMapShowing)
            {
                miniMap.Draw(g, currentRoom.SeaX, currentRoom.SeaY);
            }
        }

        public void Update(State s)
        {
            isMiniMapShowing ^= s.Input.WasPressed(Input.Action.Map);

            foreach(KeyValuePair<string, Room> keyValuePair in rooms)
            {
                if (keyValuePair.Value != null)
                {
                    keyValuePair.Value.Update(s, currentRoom == keyValuePair.Value);
                }
            }

            foreach (EntityTransition entityTransition in entityTransitions)
            {
                PerformTransition(entityTransition);
            }

            entityTransitions.Clear();
        }

        void PerformTransition(EntityTransition entityTransition)
        {
            Room newRoom = null;
            Room room = entityTransition.Room;
            Entity entity = entityTransition.Entity;

            if (room.IsEntityLeftOfRoom(entity))
            {
                newRoom = GetRoom(room.SeaX - 1, room.SeaY);

                if (newRoom == null)
                {
                    return;
                }

                entity.X = newRoom.Size.X - 2;
            }

            if (room.IsEntityRightOfRoom(entity))
            {
                newRoom = GetRoom(room.SeaX + 1, room.SeaY);

                if (newRoom == null)
                {
                    return;
                }

                entity.X = -entity.Width + 1;
            }

            if (room.IsEntityAboveRoom(entity))
            {
                newRoom = GetRoom(room.SeaX, room.SeaY - 1);

                if (newRoom == null)
                {
                    return;
                }

                entity.Y = -newRoom.Size.Y - 2;
            }

            if (room.IsEntityBelowRoom(entity))
            {
                newRoom = GetRoom(room.SeaX, room.SeaY + 1);

                if (newRoom == null)
                {
                    return;
                }

                entity.Y = -entity.Height + 1;
            }

            if (newRoom != null)
            {
                room.RemoveEntity(entity);
                newRoom.AddEntity(entity);
            }
        }

        public void Broadcast(string channel, string message, Object obj)
        {
            foreach (KeyValuePair<string, Room> keyValuePair in rooms)
            {
                if (keyValuePair.Value != null)
                    keyValuePair.Value.Broadcast(channel, message, obj);
            }
        }

        public void SaveState()
        {
            using (TextWriter w = new StreamWriter(Name + ".state"))
            {
                foreach (KeyValuePair<string, Room> keyValuePair in rooms)
                {
                    if (keyValuePair.Value != null)
                    {
                        w.WriteLine(keyValuePair.Key);
                        keyValuePair.Value.SaveState(w);
                    }
                }
            }
        }

        public void LoadState()
        {
            rooms.Clear();
            PreloadAllRooms(true);
            using (StreamReader r = new StreamReader(Name + ".state"))
            {
                while (!r.EndOfStream)
                {
                    string roomname = r.ReadLine();
                    rooms[roomname].LoadState(r);
                }
            }
        }

        public bool IsDiverOnBoat()
        {
            return currentRoom == boat;
        }
    }
}
