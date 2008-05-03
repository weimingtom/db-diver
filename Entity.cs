using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using DB.Gui;

namespace DB.Diver
{


    public abstract class Entity
    {
        public Rectangle Dimension = Rectangle.Empty;

        public int X
        {
            get { return Dimension.X; }
            set { Dimension.X = value; }
        }

        public int Y
        {
            get { return Dimension.Y; }
            set { Dimension.Y = value; }
        }

        public int Width
        {
            get { return Dimension.Width; }
            set { Dimension.Width = value; }
        }

        public int Height
        {
            get { return Dimension.Height; }
            set { Dimension.Height = value; }
        }

        public Point Position
        {
            get { return new Point(Dimension.X, Dimension.Y); }
            set { Dimension.X = value.X; Dimension.Y = value.Y; }
        }

        public Point Size
        {
            get { return new Point(Dimension.Width, Dimension.Height); }
            set { Dimension.Width = value.X; Dimension.Height = value.Y; }
        }

        public Point ResolveCollision(Point velocity, Room room)
        {
            if (velocity.X > 0)
            {

            }
            else if (velocity.X > 0)
            {

            }

            if (velocity.Y > 0)
            {
                int yStart = (Dimension.Y + Dimension.Height) / room.TileMap.TileSize.Y;
                int yEnd = (Dimension.Y + Dimension.Height + velocity.Y) / room.TileMap.TileSize.Y;

                int xStart = Dimension.X / room.TileMap.TileSize.X;
                int xEnd = (Dimension.X + Dimension.Width) / room.TileMap.TileSize.X;

                for (int y = yStart; y < yEnd; y++)
                {
                    for (int x = xStart; x < xEnd; x++)
                    {
                      
                        if (room.TileMap.IsSolid(x, y))
                        {
                            System.Console.WriteLine("Solid");
                       
                        }

                        //rectangle.Intersects(new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize));
                    }
                }

               
            }
            else if (velocity.Y < 0)
            {

            }

            return velocity;
        }

        public abstract void Draw(Graphics g, GameTime gameTime, Room.Layer layer);

        public virtual void Update(State s, Room room) { }
    }
}
