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
            Size = new Point(16, 32);
            WalkingGrid = new SpriteGrid("speedy_walking", 6, 1);
            JumpingGrid = new SpriteGrid("speedy_jumping", 1, 1);
        }

        public override void Update(State s, Room room)
        {
            base.Update(s, room);
        }
    }
}
