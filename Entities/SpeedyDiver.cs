using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DB.Gui;

namespace DB.DoF.Entities
{
    public class SpeedyDiver: Diver
    {
        public SpeedyDiver()
        {
            Size = new Point(16, 40);
            WalkingGrid = new SpriteGrid("speedy_walking", 6, 1);
            JumpingGrid = new SpriteGrid("speedy_jumping", 1, 1);
            ClimbingGrid = new SpriteGrid("speedy_climbing", 2, 1);
            Name = "Speedy";
            originalBoatPosition = new Point(150, 224 - Height);
        }

        public override void Update(State s, Room room)
        {
            base.Update(s, room);
        }
    }
}
