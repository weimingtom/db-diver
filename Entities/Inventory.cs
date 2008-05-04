using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DB.DoF.Entities
{
    class Inventory : Entity
    {
        IList<ITool> tools = new List<ITool>();

        public void AddTool(ITool tool)
        {
            tools.Add(tool);
        }

        public void RemoveTool(ITool tool)
        {
            tools.Remove(tool);
        }

        public override void Draw(DB.Gui.Graphics g, Microsoft.Xna.Framework.GameTime gameTime, Room.Layer layer)
        {
            int x = 0, y = 0;
            g.Begin();
            foreach (ITool tool in tools)
            {
                x += 16;
                if (x % 64 == 0)
                {
                    x = 0;
                    y += 16;
                }
                g.Draw(tool.Icon, new Point(X + x, Y + y), Color.White);
            }
            g.End();
        }
    }
}
