using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using DB.Gui;

namespace DB.DoF.Entities
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
            Point newVelocity = velocity;

            if (newVelocity.X > 0)
            {
                int x = (Dimension.X + Dimension.Width + newVelocity.X - 1) / room.TileMap.TileSize.X;
             
                int yStart = Dimension.Y / room.TileMap.TileSize.Y;
                int yEnd = (Dimension.Y + Dimension.Height + newVelocity.Y) / room.TileMap.TileSize.Y;

                for (int y = yStart; y < yEnd; y++)
                {
                    if (room.TileMap.IsSolid(x, y))
                    {
                        Dimension.X = x * room.TileMap.TileSize.X - room.TileMap.TileSize.X;
                        newVelocity.X = 0;
                        break;
                    }
                }
            }
            else if (newVelocity.X < 0)
            {
                int x = (Dimension.X + newVelocity.X + 1) / room.TileMap.TileSize.X;

                int yStart = Dimension.Y / room.TileMap.TileSize.Y;
                int yEnd = (Dimension.Y + Dimension.Height + newVelocity.Y) / room.TileMap.TileSize.Y;

                for (int y = yStart; y < yEnd; y++)
                {
                    if (room.TileMap.IsSolid(x, y))
                    {
                        Dimension.X = Dimension.Width + x * room.TileMap.TileSize.X;
                        newVelocity.X = 0;
                        break;
                    }
                }
            }

            if (newVelocity.Y > 0)
            {
                int y = (Dimension.Y + Dimension.Height + newVelocity.Y) / room.TileMap.TileSize.Y;

                int xStart = (Dimension.X + newVelocity.X) / room.TileMap.TileSize.X;
                int xEnd = (Dimension.X + Dimension.Width + newVelocity.X - 1) / room.TileMap.TileSize.X;

                for (int x = xStart; x <= xEnd; x++)
                {
                    if (room.TileMap.IsSolid(x, y))
                    {   
                        Dimension.Y = y * room.TileMap.TileSize.Y - Dimension.Height;
                        newVelocity.Y = 0;
                        break;       
                    }
                }
            }
            else if (newVelocity.Y < 0)
            {

            }

            return newVelocity;
        }

        public abstract void Draw(Graphics g, GameTime gameTime, Room.Layer layer);

        public virtual void Update(State s, Room room) { }
    }
}
