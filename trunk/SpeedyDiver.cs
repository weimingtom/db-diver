using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DB.Gui;

namespace DB.Diver
{
    public class SpeedyDiver: Diver
    {
        SpriteGrid walkingGrid;
        int walkingGridFrame;

        public SpeedyDiver()
        {
            Dimension = new Rectangle(0, 0, 16, 32);
            walkingGrid = new SpriteGrid("speedy_walking", 6, 1);
        }

        public override void Draw(Graphics g, GameTime gameTime, Room.Layer layer)
        {
            walkingGrid.Draw(g, Position, walkingGridFrame % 6);

            if (gameTime.TotalGameTime.Milliseconds % 100 == 0)
                walkingGridFrame++;
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                Dimension.X++;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                Dimension.X--;
            }

            base.Update(gameTime);
        }
    }
}
