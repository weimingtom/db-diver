using System;
using System.Collections.Generic;
using System.Text;
using DB.Gui;
using Microsoft.Xna.Framework;

namespace DB.Diver
{
    public abstract class Diver: Entity
    {
        public int Speed;

        public override void Update(State s)
        {
            if (s.Input.IsHeld(Input.Action.Right))
            {
                Dimension.X+= Speed;
            }

            if (s.Input.IsHeld(Input.Action.Left))
            {
                Dimension.X-= Speed;
            }

            if (s.Input.IsHeld(Input.Action.Jump))
            {
                Dimension.Y -= 3;
            }

            Dimension.Y += 1;
        }
    }
}
