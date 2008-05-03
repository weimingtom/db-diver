using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DB.Gui;

namespace DB.DoF.Entities
{
    public class Decoration: Entity
    {
        SpriteGrid animationGrid;
        int animationGridFrame;
        int sprites;
        int animationSpeed;

        Room.Layer layer;

        public Decoration(string spriteGridName, int sprites, int animationSpeed, int x, int y, Room.Layer layer)
        {
            X = x * 16;
            Y = y * 16;
            this.layer = layer;
            this.sprites = sprites;
            this.animationSpeed = animationSpeed;
            animationGrid = new SpriteGrid(spriteGridName, sprites, 1);
            Width = animationGrid.FrameSize.X;
            Height = animationGrid.FrameSize.Y;
        }

        public override void Draw(Graphics g, GameTime gameTime, Room.Layer layer)
        {
            g.Begin();

            if (layer == this.layer)
                animationGrid.Draw(g, new Point(X, Y), animationGridFrame % sprites);

            g.End();
        }

        public override void Update(State s, Room room)
        {
            base.Update(s, room);

            if (s.Time.TotalGameTime.Milliseconds % animationSpeed == 0)
            {
                animationGridFrame++;
            }
        }
    }
}
