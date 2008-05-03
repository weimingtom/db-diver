using System;
using System.Collections.Generic;
using System.Text;
using DB.Gui;
using Microsoft.Xna.Framework;

namespace DB.DoF.Entities
{
    public abstract class Diver: Entity
    {
        public int Speed;

        public override void Update(State s, Room room)
        {
            Point velocity = new Point();

            if (s.Input.IsHeld(Input.Action.Right))
            {
                velocity.X += Speed;
            }

            if (s.Input.IsHeld(Input.Action.Left))
            {
                velocity.X -= Speed;
            }

            if (s.Input.IsHeld(Input.Action.Jump))
            {
                velocity.Y -= 3;
            }

            velocity.Y += 1;

            velocity = ResolveCollision(velocity, room);
            Dimension.X += velocity.X;
            Dimension.Y += velocity.Y; 
        }
    }
}
