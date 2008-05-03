using System;
using System.Collections.Generic;
using System.Text;
using DB.Gui;
using Microsoft.Xna.Framework;

namespace DB.DoF.Entities
{
    public abstract class Diver: Entity
    {
        public int MaxSpeed = 2 * Resolution;
        public int GroundAcceleration = 1 * Resolution;
        public int Oxygen = 10000;

        public override void Update(State s, Room room)
        {
            if (s.Input.IsHeld(Input.Action.Right))
            {
                Velocity.X = Math.Min(Velocity.X + 1, 256 * 2);
            }

            if (s.Input.IsHeld(Input.Action.Left))
            {
                Velocity.X = Math.Max(Velocity.X - 1, -256 * 2);
            }

            if (s.Input.IsHeld(Input.Action.Jump))
            {
                Velocity.Y = -20;
            }

            Velocity.Y += 1;

            MoveWithCollision(room);


            Oxygen--;
            if (Oxygen < 0)
            {
                // DIE!!
            }
        }

        public void kill()
        {
            Oxygen = 0;
        }
    }
}
