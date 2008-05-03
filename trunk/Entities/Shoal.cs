using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DB.DoF.Entities
{
    public class Fishy : Entity
    {
        SmoothFloat xSpeed = new SmoothFloat(0, 0.2f), ySpeed = new SmoothFloat(0, 0.1f);
        
        Vector2 position = new Vector2();
        SpriteGrid animationGrid;
        double animationGridFrame;
        int sprites;

        public Fishy(string spriteGridName, int sprites, int x, int y)
        {
            this.position.X = x;
            this.position.Y = y;
            X = x;
            Y = y;

            this.sprites = sprites;
            animationGrid = new SpriteGrid(spriteGridName, sprites, 1);
            Width = animationGrid.FrameSize.X;
            Height = animationGrid.FrameSize.Y;
        }

        public override void Draw(DB.Gui.Graphics g, GameTime gameTime, Room.Layer layer)
        {
            animationGrid.Draw(g, Position, ((int)animationGridFrame) % sprites, xSpeed.Diff < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
        }

        public override void Update(State s, Room room)
        {
            base.Update(s, room);
            animationGridFrame++;
        }
    }

    public class Shoal : Entity
    {
        Vector2 target = new Vector2();
        IList<Fishy> fishies = new List<Fishy>();

        public Shoal()
        {
            target.X = 100;
            target.Y = 100;

            for(int i = 0; i < 100; i++)
                fishies.Add(new Fishy("yellow_fish", 2, (int)target.X + DiverGame.Random.Next(80) - 40, (int)target.Y + DiverGame.Random.Next(80) - 40));
        }

        public override void Draw(DB.Gui.Graphics g, Microsoft.Xna.Framework.GameTime gameTime, Room.Layer layer)
        {
            g.Begin();

            foreach(Fishy fish in fishies)
            {
                fish.Draw(g, gameTime, layer);
            }
            g.End();
        }

        public override void Update(State s, Room room)
        {
            base.Update(s, room);
            foreach (Fishy fish in fishies)
            {
                fish.Update(s, room);
            }
        }
    }
}
