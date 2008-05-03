using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using DB.Gui;

namespace DB.DoF.Entities
{
    public abstract class Entity
    {
        public const int Resolution = 256;
        public Point Size;
        Point position;
        public Point Velocity;

        public int X
        {
            get { return position.X / Resolution; }
            set { position.X = value * Resolution; }
        }

        public int Y
        {
            get { return position.Y / Resolution; }
            set { position.Y = value * Resolution; }
        }

        public int Width
        {
            get { return Size.X; }
            set { Size.X = value; }
        }

        public int Height
        {
            get { return Size.Y; }
            set { Size.Y = value; }
        }

        public Point Position
        {
            get { return new Point(X, Y); }
            set { X = value.X; Y = value.Y; }
        }

        public Rectangle Dimension
        {
            get { return new Rectangle(X, Y, Width, Height); }
        }

        public void MoveWithCollision(Room room)
        {
            if (Velocity.X > 0)
            {
                int x = ((position.X + Velocity.X) / Resolution + Width) / room.TileMap.TileSize.X;
             
                int yStart = Y / room.TileMap.TileSize.Y;
                int yEnd = (Y + Height - 2) / room.TileMap.TileSize.Y;

                for (int y = yStart; y <= yEnd; y++)
                {
                    if (room.TileMap.IsSolid(x, y))
                    {
                        X = x * room.TileMap.TileSize.X - room.TileMap.TileSize.X;
                        Velocity.X = 0;
                        break;
                    }
                }
            }
            else if (Velocity.X < 0)
            {
                int x = ((position.X + Velocity.X) / Resolution) / room.TileMap.TileSize.X;

                int yStart = Y / room.TileMap.TileSize.Y;
                int yEnd = (Y + Height - 2) / room.TileMap.TileSize.Y;

                for (int y = yStart; y <= yEnd; y++)
                {
                    if (room.TileMap.IsSolid(x, y))
                    {
                        X = Dimension.Width + x * room.TileMap.TileSize.X;
                        Velocity.X = 0;
                        break;
                    }
                }
            }

            if (Velocity.Y > 0)
            {
                int y = ((position.Y + Velocity.Y) / Resolution + Height) / room.TileMap.TileSize.Y;

                int xStart = ((position.X + Velocity.X) / Resolution) / room.TileMap.TileSize.X;
                int xEnd = ((position.X + Velocity.X) / Resolution + Width - 1) / room.TileMap.TileSize.X;

                for (int x = xStart; x <= xEnd; x++)
                {
                    if (room.TileMap.IsSolid(x, y))
                    {   
                        Y = y * room.TileMap.TileSize.Y - Dimension.Height;
                        Velocity.Y = 0;
                        break;       
                    }
                }
            }
            else if (Velocity.Y < 0)
            {
                int y = ((position.Y + Velocity.Y) / Resolution - 1) / room.TileMap.TileSize.Y;

                int xStart = ((position.X + Velocity.X) / Resolution) / room.TileMap.TileSize.X;
                int xEnd = ((position.X + Velocity.X) / Resolution + Width - 1) / room.TileMap.TileSize.X;

                for (int x = xStart; x <= xEnd; x++)
                {
                    if (room.TileMap.IsSolid(x, y))
                    {
                        Y = y * room.TileMap.TileSize.Y + room.TileMap.TileSize.Y;
                        Velocity.Y = 0;
                        break;
                    }
                }
            }

            MoveWithoutCollision();
        }

        public void MoveWithoutCollision()
        {
            position.X += Velocity.X;
            position.Y += Velocity.Y;
        }

        public bool IsTileSolidBelow(Room room)
        {
            int y = (Dimension.Y + Dimension.Height) / room.TileMap.TileSize.Y;

            int xStart = Dimension.X / room.TileMap.TileSize.X;
            int xEnd = (Dimension.X + Dimension.Width - 1) / room.TileMap.TileSize.X;

            for (int x = xStart; x <= xEnd; x++)
            {
                if (room.TileMap.IsSolid(x, y))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsTileSolidAbove(Room room)
        {
            int y = (Dimension.Y - 1) / room.TileMap.TileSize.Y;

            int xStart = Dimension.X / room.TileMap.TileSize.X;
            int xEnd = (Dimension.X + Dimension.Width - 1) / room.TileMap.TileSize.X;

            for (int x = xStart; x <= xEnd; x++)
            {
                if (room.TileMap.IsSolid(x, y))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsTileSolidLeft(Room room)
        {
            int x = (Dimension.X - 1) / room.TileMap.TileSize.X;

            int yStart = Dimension.Y / room.TileMap.TileSize.Y;
            int yEnd = (Dimension.Y + Dimension.Height - 2) / room.TileMap.TileSize.Y;

            for (int y = yStart; y <= yEnd; y++)
            {
                if (room.TileMap.IsSolid(x, y))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsTileSolidRight(Room room)
        {
            int x = (Dimension.X + Dimension.Width) / room.TileMap.TileSize.X;

            int yStart = (Dimension.Y) / room.TileMap.TileSize.Y;
            int yEnd = (Dimension.Y + Dimension.Height - 2) / room.TileMap.TileSize.Y;

            for (int y = yStart; y <= yEnd; y++)
            {
                if (room.TileMap.IsSolid(x, y))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsTileSolidBelowRight(Room room)
        {
            int x = (Dimension.X + Dimension.Width) / room.TileMap.TileSize.X;
            int y = (Dimension.Y + Dimension.Height) / room.TileMap.TileSize.Y;

            return room.TileMap.IsSolid(x, y);
        }

        public bool IsTileSolidBelowLeft(Room room)
        {
            int x = (Dimension.X - 1) / room.TileMap.TileSize.X;
            int y = (Dimension.Y + Dimension.Height) / room.TileMap.TileSize.Y;

            return room.TileMap.IsSolid(x, y);
        }

        public abstract void Draw(Graphics g, GameTime gameTime, Room.Layer layer);

        public virtual void Update(State s, Room room) { }
    }
}
