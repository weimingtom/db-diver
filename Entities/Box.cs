using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DB.DoF.Entities
{
    public class Box : PersistentEntity
    {
        Texture2D texture;

        public Box(string texture, int x, int y)
        {
            this.texture = DiverGame.DefaultContent.Load<Texture2D>(texture);
            this.texture.Name = texture;
            this.X = x;
            this.Y = y;
            this.Width = this.texture.Width;
            this.Height = this.texture.Height;
            this.IsTransitionable = true;
        }

        public override bool IsUpdateNeeded(Room room)
        {
            return !IsTileSolidBelow(room);
        }

        protected override string[] GetConstructorArguments()
        {
            return new string[] { Quote(texture.Name), X.ToString(), Y.ToString() };
        }

        public override void Draw(DB.Gui.Graphics g, Microsoft.Xna.Framework.GameTime gameTime, Room.Layer layer)
        {
            if (layer == Room.Layer.Player)
            {
                g.Begin();
                g.Draw(texture, Position, Color.White);
                g.End();
            }
        }

        public override void Update(State s, Room room)
        {
            if (room.Diver != null)
            {
                Rectangle d = room.Diver.Dimension;
                d.Inflate(1, 0);
                if (d.Intersects(Dimension))
                {
                    Velocity.X += room.Diver.Velocity.X;
                }
            }

            Velocity.Y = 200;
            Velocity.X = (int)(Velocity.X * 0.5f);
            MoveWithCollision(room);
        }
    }
}
