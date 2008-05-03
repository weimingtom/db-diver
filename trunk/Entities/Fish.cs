using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DB.Gui;

namespace DB.DoF.Entities
{
    public class Fish : Entity
    {
        Random r;

        SpriteGrid animationGrid;
        double animationGridFrame;
        int sprites;
        Room.Layer layer;
        Vector2 position = new Vector2();

        SmoothFloat xSpeed = new SmoothFloat(0, 170), ySpeed = new SmoothFloat(0, 170);

        public Fish(string spriteGridName, int sprites, int x, int y, Room.Layer layer)
        {
            this.position.X = x*16;
            this.position.Y = y*16;
            r = DiverGame.Random;

            this.layer = layer;
            this.sprites = sprites;
            animationGrid = new SpriteGrid(spriteGridName, sprites, 1);
            Width = animationGrid.FrameSize.X;
            Height = animationGrid.FrameSize.Y;
        }

        public override void Draw(Graphics g, GameTime gameTime, Room.Layer layer)
        {
            X = (int)position.X;
            Y = (int)position.Y;

            g.Begin();

            if (layer == Room.Layer.Player)
                animationGrid.Draw(g, Position, ((int)animationGridFrame) % sprites, xSpeed.Diff < 0 ? SpriteEffects.FlipHorizontally:SpriteEffects.None);

            g.End();
        }

        public override void Update(State s, Room room)
        {
            base.Update(s, room);

            animationGridFrame += Math.Abs((xSpeed.Diff)) + 0.1;

            xSpeed.Update();
            ySpeed.Update();

            position.X += xSpeed.Value;
            position.Y += ySpeed.Value;

            if (r.Next(400) == 0)
            {
                xSpeed.Target = (float)(r.NextDouble()*1-0.5f);
                ySpeed.Target = (float)(r.NextDouble() * 0.6 - 0.3f);
            }
            if (X < 0 && xSpeed.Target < 0 ||
                X > room.TileMap.SizeInPixels.X && xSpeed.Diff > 0) xSpeed.Target *= -1;
            if (Y < 0 && ySpeed.Target < 0 ||
                Y > room.TileMap.SizeInPixels.Y && ySpeed.Diff > 0) ySpeed.Target *= -1;


        }

        public override bool IsTransitionable()
        {
            return true;
        }
    }



}
