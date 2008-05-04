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

        int currentRoomX, currentRoomY, firstRoomX, firstRoomY;
        Room room;
        Room boat;
        Room.LeftRoomHandler leftRoomHandler;
        Room.LeftRoomHandler leftBoatHandler;
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

            leftRoomHandler = new Room.LeftRoomHandler(OnLeftRoom);
            leftBoatHandler = new Room.LeftRoomHandler(OnLeftBoat);

            miniMap = new MiniMap(this);

            // To load new game...
            PreloadAllRooms(false);
            // or, to load a saved game (also preloads rooms)
            // LoadState();
           
            panel = DiverGame.DefaultContent.Load<Texture2D>("panel");
            boat = Room.FromFile(DiverGame.DefaultContent.RootDirectory + "/" + name + "_boat.room", tileSet, false);
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
                room = Room.FromFile(filename, tileSet, skipPersistent);
                rooms[filename] = room;
            }
            catch (FileNotFoundException e)
            {
                System.Console.WriteLine("Couldn't find room " + filename);
                room = null;
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
            room.OnLeftRoom -= leftRoomHandler;

            room = GetRoom(x, y);

            if (room == null)
            {
                throw new Exception("Cannot make a null room active! (" + x + "," + y + ")");
            }
            room.OnLeftRoom += leftRoomHandler;
            currentRoomX = x;
            currentRoomY = y;
            room.Diver = diver;

            miniMap.Discover(room, x, y);
            diver.OxygenDecrease = true;
            diver.OxygenIncrease = false;
        }

        void EnterBoat()
        {
            boat.OnLeftRoom += leftBoatHandler;
            room = boat;
            diver.X = 200;
            diver.Y = 129;
            room.Diver = diver;
            currentRoomX = firstRoomX;
            currentRoomY = firstRoomY - 1;
            diver.OxygenDecrease = false;
            diver.OxygenIncrease = true;
        }

        void OnLeftBoat(Entity entity)
        {
            if (entity == diver)
            {
                boat.OnLeftRoom -= leftBoatHandler;
                MakeRoomActive(firstRoomX, firstRoomY);
                entity.Y = -entity.Height + 1;
                entity.X = boat.Size.X / 2 - entity.Width / 2;
            }
        }

        void OnLeftRoom(Entity entity)
        {        
            if (entity == diver)
            {
                if (room.IsEntityLeftOfRoom(entity))
                {
                    MakeRoomActive(--currentRoomX, currentRoomY);
                    entity.X = room.Size.X - 2;
                }

                if (room.IsEntityRightOfRoom(entity))
                {
                    MakeRoomActive(++currentRoomX, currentRoomY);
                    entity.X = -entity.Width + 1;
                }

                if (room.IsEntityBelowRoom(entity))
                {
                    MakeRoomActive(currentRoomX, ++currentRoomY);
                    entity.Y = -entity.Height + 1;
                }
                if (room.IsEntityAboveRoom(entity))
                {
                    MakeRoomActive(currentRoomX, --currentRoomY);
                    entity.Y = room.Size.Y - 2;
                }

                return;
            }
            
            if (entity.IsTransitionable())
            {
                EntityTransition entityTransistion = new EntityTransition(entity, room);
                entityTransitions.Add(entityTransistion);
            }
        }

        public void Draw(Gui.Graphics g, GameTime gt)
        {
            g.PushClipRectangle(new Rectangle(0, 0, room.Size.X, room.Size.Y));
            g.Begin();
            g.Draw(DiverGame.White, new Rectangle(0, 0, room.Size.X, room.Size.Y), new Color(74, 193, 231));
            g.End();
            room.Draw(g, gt);
            g.PopClipRectangle();

            g.PushClipRectangle(new Rectangle(0, room.Size.Y, 400, 300 - room.Size.Y));
            g.Begin();
            g.Draw(panel, new Point(0, 0), Color.White);
            g.Draw(DiverGame.White, new Rectangle(16, 4, (122 * diver.Oxygen) / Diver.MaxOxygen, 4), new Color(199, 77, 77));
            g.End();
            g.PopClipRectangle();

            if (isMiniMapShowing)
            {
                miniMap.Draw(g, currentRoomX, currentRoomY);
            }
        }

        public void Update(State s)
        {
            isMiniMapShowing ^= s.Input.WasPressed(Input.Action.Map);

            foreach(KeyValuePair<string, Room> keyValuePair in rooms)
            {
                if (keyValuePair.Value != null
                    && (keyValuePair.Value.IsUpdateNeeded() || room == keyValuePair.Value))
                    keyValuePair.Value.Update(s);
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
                newRoom = GetRoom(currentRoomX - 1, currentRoomY);

                if (newRoom == null)
                {
                    return;
                }

                entity.X = newRoom.Size.X - 2;
            }

            if (room.IsEntityRightOfRoom(entity))
            {
                newRoom = GetRoom(currentRoomX + 1, currentRoomY);

                if (newRoom == null)
                {
                    return;
                }

                entity.X = -entity.Width + 1;
            }

            if (newRoom != null)
            {
                room.RemoveEntity(entity);
                newRoom.AddEntity(entity);
            }
        }

        public void Broadcast(string channel, string message)
        {
            foreach (KeyValuePair<string, Room> keyValuePair in rooms)
            {
                if (keyValuePair.Value != null)
                    keyValuePair.Value.Broadcast(channel, message);
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
    }
}
