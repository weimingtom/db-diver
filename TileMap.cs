using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace DB.Diver
{
    public class TileMap
    {
        private string fileFormatMapping = ".0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int[] tiles;

        public SpriteGrid TileSet;
        public int Width;
        public int Height;
        public Point SizeInPixels { get { return new Point(Width * TileSet.FrameSize.X, Height * TileSet.FrameSize.Y); } }
        public Point TileSize { get { return TileSet.FrameSize; } }
          
        public int this[int x, int y]
        {
            get { return tiles[x + y * Width]; }
            set { tiles[x + y * Width] = value; }
        }
        
        public TileMap(SpriteGrid tileSet, int width, int height)
        {
            TileSet = tileSet;
            Width = width;
            Height = height;
            tiles = new int[Width * Height];
            Clear(0);
        }

        public void Clear(int value)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    this[x, y] = value;
                }
            }
        }

        public void Load(TextReader r)
        {
            for (int y = 0; y < Height; y++)
            {
                string line = r.ReadLine();

                for (int x = 0; x < Width; x++)
                {
                    this[x, y] = fileFormatMapping.IndexOf(line[x]);
                }
            }
        }

        public void Save(TextWriter w)
        {
            for (int y = 0; y < Height; y++)
            {
                StringBuilder line = new StringBuilder();
                for (int x = 0; x < Width; x++)
                {
                    line.Append(fileFormatMapping[this[x, y]]);
                }

                w.WriteLine(line);
            }
        }

        public void Draw(Gui.Graphics g)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (this[x, y] != 0)
                    {
                        TileSet.Draw(g, new Point(x * TileSize.X, y * TileSize.Y), this[x, y]);
                    }
                }
            }
        }
    }
}
