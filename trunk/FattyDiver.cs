using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using DB.Gui;

namespace DB.Diver
{
    public class FattyDiver: Diver
    {
        public FattyDiver()
        {
            this.Dimension = new Rectangle(0, 0, 16, 32);
        }

        public override void Draw(Graphics g, GameTime gameTime, Room.Layer layer)
        {
        }
    }
}
