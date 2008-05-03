using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DB.DoF.Entities
{
    public class Fishy : Entity
    {
        public float TargetX, TargetY;
        float x, y;
        float speedX, speedY;

        SpriteGrid animationGrid;
        double animationGridFrame;
        int sprites;

        public Fishy(string spriteGridName, int sprites, int x, int y)
        {
            X = x;
            Y = y;
            TargetX = x;
            TargetY = y;
            this.x = x;
            this.y = y;

            this.sprites = sprites;
            animationGrid = new SpriteGrid(spriteGridName, sprites, 1);
            Width = animationGrid.FrameSize.X;
            Height = animationGrid.FrameSize.Y;
        }

        public override void Draw(DB.Gui.Graphics g, GameTime gameTime, Room.Layer layer)
        {
            animationGrid.Draw(g, Position, ((int)animationGridFrame) % sprites, (float)Math.Atan2(speedX, -speedY)-(float)Math.PI/2f);
        }

        public override void Update(State s, Room room)
        {
            base.Update(s, room);
            Vector2 direction = new Vector2((float)((TargetX - x) * (DiverGame.Random.NextDouble() * 0.2f + 0.8f)), 
                                            (float)((TargetY - y) * (DiverGame.Random.NextDouble() * 0.2f + 0.8f)));
            if(direction.LengthSquared() > 0)
                direction.Normalize();

            speedX += direction.X * 0.07f;
            speedY += direction.Y * 0.07f;
            speedX *= 0.99f;
            speedY *= 0.99f;
            x += speedX;
            y += speedY;
            X = (int)x;
            Y = (int)y;

        }
    }

    public class Shoal : Entity
    {
        SmoothFloat targetX = new SmoothFloat(0f, 0.8f);
        SmoothFloat targetY = new SmoothFloat(0f, 0.8f);

        IList<Fishy> fishies = new List<Fishy>();

        public Shoal()
        {
            targetX.Target = 100;
            targetY.Target = 100;

            for(int i = 0; i < 100; i++)
                fishies.Add(new Fishy("small_fish", 4, (int)targetX.Value + DiverGame.Random.Next(80) - 40, (int)targetY.Value + DiverGame.Random.Next(80) - 40));
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
            if (DiverGame.Random.Next(100) == 0)
            {
                targetX.Target = DiverGame.Random.Next(room.TileMap.SizeInPixels.X);
                targetY.Target = DiverGame.Random.Next(room.TileMap.SizeInPixels.Y);
            }

            targetX.Update();
            targetY.Update();

            foreach (Fishy fish in fishies)
            {
                fish.TargetX = targetX.Value + DiverGame.Random.Next(40) - 80;
                fish.TargetY = targetY.Value + DiverGame.Random.Next(40) - 80;
            }

            foreach (Fishy fish in fishies)
            {
                fish.Update(s, room);
            }
        }
    }
}
