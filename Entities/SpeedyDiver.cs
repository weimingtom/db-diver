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
        SpriteGrid walkingGrid;
        int walkingGridFrame;

        public SpeedyDiver()
        {
            Size = new Point(16, 32);
            //Speed = 3;
            walkingGrid = new SpriteGrid("speedy_walking", 6, 1);
        }

        public override void Draw(Graphics g, GameTime gameTime, Room.Layer layer)
        {
            g.Begin();

            if (layer == Room.Layer.Player)
                walkingGrid.Draw(g, Position, walkingGridFrame % 6);

            if (gameTime.TotalGameTime.Milliseconds % 100 == 0)
                walkingGridFrame++;

            g.End();
        }

        public override void Update(State s, Room room)
        {
            base.Update(s, room);
        }
    }
}
