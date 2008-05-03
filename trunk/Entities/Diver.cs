using System;
using System.Collections.Generic;
using System.Text;
using DB.Gui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DB.DoF.Entities
{
    public abstract class Diver: Entity
    {
        public const int MaxOxygen = 10000;
        protected int MaxSpeed = 1 * Resolution;
        protected int GroundAcceleration = Resolution;
        protected int AirAcceleration = Resolution / 16;
        protected int JumpPower = (9 * Resolution) / 2;
        protected int MaxJumpSpeed = (5 * Resolution) / 2;
        protected int MaxFallSpeed = (6 * Resolution) / 2;
        protected int WalkAnimationSpeed = Resolution * 3;

        protected SpriteGrid WalkingGrid;

        SpriteEffects spriteEffects = SpriteEffects.None;

        int walkingGridFrame = 3;
        int jumpVelocity;

        public int Oxygen = MaxOxygen;

        public override void Update(State s, Room room)
        {
            bool isOnGround = IsTileSolidBelow(room);
            int acceleration = isOnGround ? GroundAcceleration : AirAcceleration;

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

            if (Velocity.X == 0)
            {
                walkingGridFrame = 3 * WalkAnimationSpeed;
            }
            else
            {
                walkingGridFrame += Math.Abs(Velocity.X);
                spriteEffects = Velocity.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            }

            if (s.Input.WasPressed(Input.Action.Jump) && isOnGround)
            {
                jumpVelocity = -JumpPower;
                isOnGround = false;
            }

            if ((s.Input.WasReleased(Input.Action.Jump) && jumpVelocity < 0) || isOnGround)
            {
                jumpVelocity = 0;
            }

            jumpVelocity += Resolution / 8;

            Velocity.Y = Math.Max(Math.Min(jumpVelocity, MaxFallSpeed), -MaxJumpSpeed);

            MoveWithCollision(room);

            // Bumped head
            if (Velocity.Y == 0)
            {
                jumpVelocity = 0;
            }


            Oxygen--;
            if (Oxygen < 0)
            {
                // DIE!!
            }
        }

        public override void Draw(Graphics g, GameTime gameTime, Room.Layer layer)
        {
            if (layer == Room.Layer.Player)
            {
                Point pos = new Point(Position.X - 2, Position.Y);
                g.Begin();
                WalkingGrid.Draw(g, pos, walkingGridFrame / WalkAnimationSpeed, spriteEffects);
                g.End();
            }
        }

        public void kill()
        {
            Oxygen = 0;
        }
    }
}
