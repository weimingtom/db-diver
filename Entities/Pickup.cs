using System;
using System.Collections.Generic;
using System.Text;

using DB.Gui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DB.DoF.Entities
{
    public class Pickup : Entity
    {
        readonly public ITool Tool;
        string text;

        public Pickup(int x, int y, ITool tool, string text)
        {
            Tool = tool;
            this.text = text;
            X = x;
            Y = y;
            Size = new Point(16, 16);
        }

        public override void Draw(DB.Gui.Graphics g, Microsoft.Xna.Framework.GameTime gameTime, Room.Layer layer)
        {
            if (layer == Room.Layer.Player)
            {
                g.Begin();
                g.Draw(Tool.Icon, new Point(Position.X, Position.Y + (int)(Math.Sin(gameTime.TotalGameTime.Ticks / 10000000.0f) * 8.5f)), Color.White);
                g.End();
            }
        }

        public override void Update(State s, Room room)
        {
            room.AddEntity(Particle.MakeSpark(new Point(X + DiverGame.Random.Next(Width), Y + DiverGame.Random.Next(Height))));

            if (room.Diver.Dimension.Intersects(Dimension))
            {
                room.RemoveEntity(this);
                room.Sea.Broadcast("inventory", "add", Tool);
                // Display text
            }
        }
    }
}
