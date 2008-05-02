using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DB.Gui;

namespace DB.Diver
{
    public class SpeedyDiver: Diver
    {
        Texture2D walking;

        public SpeedyDiver()
        {
            Dimension = new Rectangle(0, 0, 16, 32);
            walking = DiverGame.DefaultContent.Load<Texture2D>("speedy_walking");
        }

        public override void Draw(Graphics g, GameTime gameTime)
        {
            g.Draw(walking, Position, Color.White);
        }
    }
}
