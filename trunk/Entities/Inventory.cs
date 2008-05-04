using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DB.Gui;

namespace DB.DoF.Entities
{
    public class Inventory : PersistentEntity
    {
        List<ITool> tools = new List<ITool>();

        SpriteFont font;

        bool isGUIActive = false;
        bool diverInInventory = false;

        public Inventory(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.Width = 20;
            this.Height = 20;
            font = DiverGame.DefaultContent.Load<SpriteFont>("Font");
        }

        public Inventory(int x, int y, ITool[] tools)
        :this(x, y)
        {
            ITool[] ted = new ITool[] { null, null, null };
            foreach (ITool tool in tools)
            {
                this.tools.Add(tool);
            }
        }

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

            if (isGUIActive)
            {
                g.DrawStringShadowed(font,
                        "Press Space leave inventory",
                        new Rectangle(0, 100, 400, 20),
                        TextAlignment.Center,
                        Color.White);
            }

            g.End();

        }

        public override void Update(State s, Room room)
        {
            if (isGUIActive)
            {
                if(s.Input.WasPressed(Input.Action.Select))
                {
                    isGUIActive = false;
                    room.Diver.Freeze = false;
                }

                //System.Console.WriteLine("GUI!! " + s.Time.TotalRealTime.Milliseconds);
            }
            else
            {
                bool collidingWithDiver = false;
                foreach (Entity entity in room.GetCollidingEntities<Diver>(this))
                {
                    if (room.Diver == entity)
                    {
                        collidingWithDiver = true;
                    }
                }
                
                if (!diverInInventory && collidingWithDiver)
                {
                    isGUIActive = true;
                    room.Diver.Freeze = true;
                }
                diverInInventory = collidingWithDiver;

            }
        }

        public override void OnMessageReceived(string channel, string message, object obj)
        {
            if (channel == "inventory" && message == "add")
            {
                AddTool((ITool)obj);
            }
        }


        protected override string[] GetConstructorArguments()
        {
            string[] toolConstructorStrings = new string[tools.Count];

            foreach (ITool tool in tools)
            {
                int i = 0;
                i++;

                toolConstructorStrings[i] = tool.GetType().Name + "()";
            }

            string[] arguments = new string[] {
            "" + X,
            "" + Y,
            "new ITool[] {" + CommaSeparate(toolConstructorStrings) + "}"
            }; 
            return arguments;
        }
    }
}
