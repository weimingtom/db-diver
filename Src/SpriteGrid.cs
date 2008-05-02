using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DB.Diver.Src
{
    class SpriteGrid
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

        public void Draw(Gui.Graphics g, Point position, int frame)
        {
            g.Draw(texture, position, GetRectangle(frame), Color.White);
        }

        private Rectangle GetRectangle(int frame)
        {
            return new Rectangle((frame % XCount) * FrameSize.X, (frame / XCount) * FrameSize.Y, FrameSize.X, FrameSize.Y); 
        }
    }
}
