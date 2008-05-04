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
        protected int Strength = 10;

        public Point AppliedForce;
        protected SpriteGrid WalkingGrid;
        protected SpriteGrid JumpingGrid;

        SpriteEffects spriteEffects = SpriteEffects.None;

        public ITool Tool1;
        public ITool Tool2;

        int walkingGridFrame = 3;
        public int JumpVelocity;
        bool isOnGround;
        public bool OxygenDecrease = true;
        public bool OxygenIncrease = false;
        public bool Freeze = false;
        public bool JumpEnabled = true;

        public int Oxygen = MaxOxygen;

        public override void Update(State s, Room room)
        {
            if (!Freeze)
            {
                isOnGround = IsTileSolidBelow(room);
                int acceleration = isOnGround ? GroundAcceleration : AirAcceleration;

                if (s.Input.IsHeld(Input.Action.Right) && !s.Input.IsHeld(Input.Action.Left))
                {
                    AppliedForce.X = Strength;
                    Velocity.X = Math.Min(Velocity.X + acceleration, MaxSpeed);
                }
                else if (s.Input.IsHeld(Input.Action.Left) && !s.Input.IsHeld(Input.Action.Right))
                {
                    AppliedForce.X = -Strength;
                    Velocity.X = Math.Max(Velocity.X - acceleration, -MaxSpeed);
                }
                else
                {
                    AppliedForce.X = 0;
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

                if (s.Input.WasPressed(Input.Action.Jump) && isOnGround && JumpEnabled)
                {
                    JumpVelocity = -JumpPower;
                    isOnGround = false;
                }

                if ((s.Input.WasReleased(Input.Action.Jump) && JumpVelocity < 0) || isOnGround)
                {
                    JumpVelocity = 0;
                }

                JumpVelocity += Resolution / 8;

                Velocity.Y = Math.Max(Math.Min(JumpVelocity, MaxFallSpeed), -MaxJumpSpeed);

                MoveWithCollision(room);

                // Bumped head
                if (Velocity.Y == 0)
                {
                    JumpVelocity = 0;
                }
            }

            if (OxygenDecrease)
            {
                Oxygen--;

                if (s.Time.TotalGameTime.Seconds % 4 == 1 && s.Time.TotalGameTime.Milliseconds % 1000 < 500)
                {
                    if (DiverGame.Random.Next(10) == 0)
                    {
                        room.AddEntity(Particle.MakeBigBubble(new Point(X + Width / 2, Y)));
                    }

                    if (DiverGame.Random.Next(5) == 0)
                    {
                        room.AddEntity(Particle.MakeSmallBubble(new Point(X + Width / 2, Y)));
                    }

                    if (DiverGame.Random.Next(4) == 0)
                    {
                        room.AddEntity(Particle.MakeTinyBubble(new Point(X + Width / 2, Y)));
                    }
                }
            }

            if (OxygenIncrease)
                Oxygen += 5;

            if (Oxygen < 0)
            {
                // DIE!!
            }

            if (Oxygen > MaxOxygen)
                Oxygen = MaxOxygen;
        }

        public override void Draw(Graphics g, GameTime gameTime, Room.Layer layer)
        {
            if (layer == Room.Layer.Player)
            {
                Point pos = new Point(Position.X - 2, Position.Y);
                g.Begin();
                if (isOnGround)
                    WalkingGrid.Draw(g, pos, walkingGridFrame / WalkAnimationSpeed, spriteEffects);
                else
                    JumpingGrid.Draw(g, new Point(pos.X, pos.Y - (JumpingGrid.FrameSize.Y - Height)), 1, spriteEffects);
                g.End();
            }
        }

        public void kill()
        {
            Oxygen = 0;
            
        }
    }
}
