using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using DB.Gui;

namespace DB.DoF.Entities
{
    public class TinyDiver: Diver
    {
        public TinyDiver()
        {
            Size = new Point(16, 28);
            WalkingGrid = new SpriteGrid("tiny_walking", 6, 1);
            JumpingGrid = new SpriteGrid("tiny_jumping", 1, 1);
            ClimbingGrid = new SpriteGrid("tiny_climbing", 2, 1);
            Name = "Tiny";
            originalBoatPosition = new Point(250, 224 - Height);
        }

        public override void Update(State s, Room room)
        {
            base.Update(s, room);
        }
    }
}
