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
        Color color;

        Room.Layer layer;

        public Decoration(string spriteGridName, int sprites, int animationSpeed, int x, int y, Room.Layer layer)
            :
            this(spriteGridName, sprites, animationSpeed, x, y, layer, Color.White)
        { }

        public Decoration(string spriteGridName, int sprites, int animationSpeed, int x, int y, Room.Layer layer, Color color)
        {
            X = x * 16;
            Y = y * 16;
            this.layer = layer;
            this.sprites = sprites;
            this.animationSpeed = animationSpeed;
            this.color = color;
            animationGrid = new SpriteGrid(spriteGridName, sprites, 1);
            Width = animationGrid.FrameSize.X;
            Height = animationGrid.FrameSize.Y;
        }

        public override void Draw(Graphics g, GameTime gameTime, Room.Layer layer)
        {
            g.Begin();

            if (layer == this.layer)
                animationGrid.Draw(g, new Point(X, Y), animationGridFrame % sprites, color);

            g.End();
        }

        public override void Update(State s, Room room)
        {
            base.Update(s, room);

            if (animationSpeed != 0 && s.Time.TotalGameTime.Milliseconds % animationSpeed == 0)
            {
                animationGridFrame++;
            }
        }
    }
}
