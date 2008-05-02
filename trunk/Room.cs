using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DB.Diver
{
    class Room
    {
        public const int WidthInTiles = 10;
        public const int HeightInTiles = 10;

        public TileMap TileMap;
        //IList<Entity> entities = new List<Entity>();

        public Room(SpriteGrid tileSet)
        {            
            TileMap = new TileMap(tileSet, WidthInTiles, HeightInTiles);
        }

        static Room FromFile(string filename, SpriteGrid tileSet)
        {
            Room room = new Room(tileSet);

            using (TextReader r = new StreamReader(filename))
            {
                room.TileMap.Load(r);

            }

            return room;
        }


    }
}
