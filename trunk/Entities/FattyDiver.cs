using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using DB.Gui;

namespace DB.DoF.Entities
{
    public class FattyDiver: Diver
    {
        public FattyDiver()
        {
            Size = new Point(16, 40);
            WalkingGrid = new SpriteGrid("fatty_walking", 6, 1);
            JumpingGrid = new SpriteGrid("fatty_jumping", 1, 1);
            ClimbingGrid = new SpriteGrid("fatty_climbing", 2, 1);
            Name = "Fatty";
            originalBoatPosition = new Point(200, 224 - Height);
            Strength = 20;
        }

        public override void Update(State s, Room room)
        {
            base.Update(s, room);
        }
    }
}
