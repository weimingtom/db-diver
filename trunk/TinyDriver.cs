using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using DB.Gui;

namespace DB.Diver
{
    public class TinyDiver: Diver
    {
        public TinyDiver()
        {
            this.Dimension = new Rectangle(0, 0, 16, 16);
        }

        public override void Draw(Graphics g, GameTime gameTime, Room.Layer layer)
        {
            
        }
    }
}
