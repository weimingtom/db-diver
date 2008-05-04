using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DB.Gui;

namespace DB.DoF.Entities
{
    public class Particle : Entity
    {
        SpriteGrid spriteGrid;
        float framerate;
        Vector2 position;
        Vector2 velocity;
        Vector2 gravity;
        float damping;
        Color color;
        SpriteBlendMode blend;
        Room.Layer layer;
        float frame;

        public Particle(SpriteGrid spriteGrid, float framerate, Vector2 position, Vector2 velocity, Vector2 gravity, float damping, Color color, SpriteBlendMode blend, Room.Layer layer)
        {
            this.spriteGrid = spriteGrid;
            this.framerate = framerate;
            this.position = position;
            this.velocity = velocity;
            this.gravity = gravity;
            this.damping = damping;
            this.color = color;
            this.blend = blend;
            this.layer = layer;            

            Size = spriteGrid.FrameSize;

            X = (int)position.X - Width / 2;
            Y = (int)position.Y - Height / 2;
        }

        public static Particle MakeBigBubble(Point pos)
        {
            SpriteGrid spriteGrid = new SpriteGrid("bubble", 4, 1);
            Random r = DiverGame.Random;
            return new Particle(spriteGrid,
                                0.6f * (float)r.NextDouble() * 0.5f,
                                new Vector2((float)pos.X, (float)pos.Y),
                                new Vector2(((float)r.NextDouble() - 0.5f) * 3.0f, (float)-r.NextDouble() * 1.5f),
                                new Vector2(0, -(float)r.NextDouble() * 0.2f - 0.2f),
                                0.9f,
                                new Color(255, 255, 255, 128),
                                SpriteBlendMode.AlphaBlend,
                                Room.Layer.Background);
        }

        public static Particle MakeSmallBubble(Point pos)
        {
            SpriteGrid spriteGrid = new SpriteGrid("bubblesmall", 1, 1);
            Random r = DiverGame.Random;
            return new Particle(spriteGrid,
                                1.0f,
                                new Vector2((float)pos.X, (float)pos.Y),
                                new Vector2(((float)r.NextDouble() - 0.5f) * 3.0f, (float)-r.NextDouble() * 1.5f),
                                new Vector2(0, -(float)r.NextDouble() * 0.2f - 0.1f),
                                0.9f,
                                new Color(255, 255, 255, 96),
                                SpriteBlendMode.AlphaBlend,
                                Room.Layer.Background);
        }


        public static Particle MakeTinyBubble(Point pos)
        {
            SpriteGrid spriteGrid = new SpriteGrid("bubbletiny", 1, 1);
            Random r = DiverGame.Random;
            return new Particle(spriteGrid,
                                1.0f,
                                new Vector2((float)pos.X, (float)pos.Y),
                                new Vector2(((float)r.NextDouble() - 0.5f) * 3.0f, (float)-r.NextDouble() * 1.5f),
                                new Vector2(0, -(float)r.NextDouble() * 0.1f - 0.07f),
                                0.9f,
                                new Color(255, 255, 255, 64),
                                SpriteBlendMode.AlphaBlend,
                                Room.Layer.Background);
        }

        public override void Draw(Graphics g, GameTime gameTime, Room.Layer layer)
        {
            if (layer == this.layer)
            {
                g.Begin(blend);
                spriteGrid.Draw(g, Position, (int)frame, color);
                g.End();
            }
        }

        public override void Update(State s, Room room)
        {
            position += velocity;
            velocity += gravity;
            velocity *= damping;
            frame += framerate;
            X = (int)position.X;
            Y = (int)position.Y;

            if (!new Rectangle(0, 0, room.Size.X, room.Size.Y).Intersects(Dimension))
            {
                room.RemoveEntity(this);
            }
        }
    }
}
