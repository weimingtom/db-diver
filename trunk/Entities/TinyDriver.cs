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
            Size = new Point(16, 16);
            //Speed = 1;
        }

        public override void Draw(Graphics g, GameTime gameTime, Room.Layer layer)
        {
            
        }
    }
}
