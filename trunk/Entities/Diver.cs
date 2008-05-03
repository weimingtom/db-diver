using System;
using System.Collections.Generic;
using System.Text;
using DB.Gui;
using Microsoft.Xna.Framework;

namespace DB.DoF.Entities
{
    public abstract class Diver: Entity
    {
        protected int MaxSpeed = 1 * Resolution;
        protected int GroundAcceleration = Resolution;
        protected int AirAcceleration = Resolution / 2;
        protected int JumpPower = Resolution * 3;
        protected int MaxJumpSpeed = 2 * Resolution;
        protected int MaxFallSpeed = (3 * Resolution) / 2;

        int jumpVelocity;

        public int Oxygen = 10000;

        public override void Update(State s, Room room)
        {
            bool isOnGround = true;
            int acceleration = GroundAcceleration;

            if (s.Input.IsHeld(Input.Action.Right) && !s.Input.IsHeld(Input.Action.Left))
            {
                Velocity.X = Math.Min(Velocity.X + acceleration, MaxSpeed);
            }
            else if (s.Input.IsHeld(Input.Action.Left) && !s.Input.IsHeld(Input.Action.Right))
            {
                Velocity.X = Math.Max(Velocity.X - acceleration, -MaxSpeed);
            }
            else
            {
                if (Velocity.X > 0)
                {
                    Velocity.X = Math.Max(Velocity.X - acceleration, 0);
                }
                else if (Velocity.X < 0)
                {
                    Velocity.X = Math.Min(Velocity.X + acceleration, 0);
                }
            }

            if (s.Input.WasPressed(Input.Action.Jump) && isOnGround)
            {
                jumpVelocity = -JumpPower;
                isOnGround = false;
            }

            if (s.Input.WasReleased(Input.Action.Jump) && jumpVelocity < 0)
            {
                jumpVelocity = 0;
            }

            jumpVelocity += Resolution / 8;

            Velocity.Y = Math.Max(Math.Min(jumpVelocity, MaxFallSpeed), -MaxJumpSpeed);

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
