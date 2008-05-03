using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DB.DoF
{
    public class SpriteGrid
    {
        Texture2D texture;
        public int XCount;
        public int YCount;
        public Point FrameSize { get { return new Point(texture.Width / XCount, texture.Height / YCount); } }

        public SpriteGrid(Texture2D texture, int xCount, int yCount)
        {
            this.texture = texture;
            this.XCount = xCount;
            this.YCount = yCount;
        }

        public SpriteGrid(string filename, int xCount, int yCount)
            :this(DiverGame.DefaultContent.Load<Texture2D>(filename), xCount, yCount)
        {
            
        }

        public void Draw(Gui.Graphics g, Point position, int frame, SpriteEffects spriteEffects)
        {
            g.Draw(texture, new Vector2(position.X, position.Y), GetRectangle(frame), Color.White, 0f, Vector2.Zero, 1f, spriteEffects, 0f);
        }

        public void Draw(Gui.Graphics g, Point position, int frame)
        {
            g.Draw(texture, position, GetRectangle(frame), Color.White);
        }

        private Rectangle GetRectangle(int frame)
        {
            return new Rectangle((frame % XCount) * FrameSize.X, ((frame / XCount) % YCount) * FrameSize.Y, FrameSize.X, FrameSize.Y); 
        }
    }
}
