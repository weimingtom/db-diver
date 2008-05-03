using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using DB.Gui;

namespace DB.DoF.Entities
{
    public class Fish : Entity
    {
        SpriteGrid animationGrid;
        int animationGridFrame;
        int sprites;
        Room.Layer layer;

        public Fish(string spriteGridName, int sprites, int x, int y, Room.Layer layer)
        {
            this.X = x*16;
            this.Y = y*16;
            this.layer = layer;
            this.sprites = sprites;
            animationGrid = new SpriteGrid(spriteGridName, sprites, 1);
            Width = animationGrid.FrameSize.X;
            Height = animationGrid.FrameSize.Y;
        }

        public override void Draw(Graphics g, GameTime gameTime, Room.Layer layer)
        {
            g.Begin();

            if (layer == Room.Layer.Player)
                animationGrid.Draw(g, Position, animationGridFrame % sprites);

            if (gameTime.TotalGameTime.Milliseconds % 100 == 0)
                animationGridFrame++;

            g.End();
        }

        public override void Update(State s, Room room)
        {
            base.Update(s, room);
            X += 1;
        }
    }



}
