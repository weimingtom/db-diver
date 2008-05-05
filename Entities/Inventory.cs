using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DB.Gui;
using DB.DoF.Tools;

namespace DB.DoF.Entities
{
    public class Inventory : PersistentEntity
    {
        List<ITool> tools = new List<ITool>();

        SpriteFont font;

        Texture2D toolSelectorTexture;
        int inventorySelected = 0;

        bool isGUIActive = false;
        bool diverInInventory = false;

        public Inventory(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.Width = 20;
            this.Height = 20;
            font = DiverGame.DefaultContent.Load<SpriteFont>("Font");
            toolSelectorTexture = DiverGame.DefaultContent.Load<Texture2D>("tool_selector");

            tools.Add(new HarpoonTool());
            tools.Add(new HarpoonTool());
            tools.Add(new HarpoonTool());
            tools.Add(new HarpoonTool());
        }

        public Inventory(int x, int y, ITool[] tools)
        :this(x, y)
        {
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
            if (diverInInventory)
            {
                if (!isGUIActive)
                {
                    g.DrawStringShadowed(font,
                     "Press Space to open inventory",
                     new Rectangle(0, 100, 400, 20),
                     TextAlignment.Center,
                     Color.White);
                }

            }

            if (isGUIActive)
            {
                Color c = new Color(0, 0, 0, 180);

                g.Draw(DiverGame.White, new Rectangle(100, 100, 200, 100), c);
                g.Draw(DiverGame.White, new Rectangle(110, 110, 180, 80), Color.White);
                g.Draw(DiverGame.White, new Rectangle(111, 111, 178, 78), Color.Black);
                g.Draw(DiverGame.White, new Rectangle(160, 100, 80, 20), Color.Black);

                int spaceBetween = 30;
                x = 0;
                foreach (ITool tool in tools)
                {
                    g.Draw(tool.Icon, new Point(124 + x, 124), Color.White);
                    x += spaceBetween;
                }
                
                g.DrawStringShadowed(font,
                     "Inventory",
                     new Rectangle(100, 100, 200, 20),
                     TextAlignment.Center,
                     Color.White);

                g.Draw(toolSelectorTexture, new Point(spaceBetween*inventorySelected + 120, 120), Color.White);

            }

            g.End();

        }

        public override void Update(State s, Room room)
        {

            diverInInventory = false;
            foreach (Entity entity in room.GetCollidingEntities<Diver>(this))
            {
                if (room.Diver == entity)
                {
                    diverInInventory = true;
                    break;
                }
            }

            if (isGUIActive)
            {
                if (s.Input.WasPressed(Input.Action.Select))
                {
                    isGUIActive = false;
                    room.Diver.Freeze = false;
                }

                if (s.Input.WasPressed(Input.Action.Right))
                {
                    inventorySelected = (inventorySelected + 1) % tools.Count;
                }
                if (s.Input.WasPressed(Input.Action.Left))
                {
                    inventorySelected = inventorySelected == 0 ? tools.Count-1:inventorySelected-1;
                }

                ITool selectedTool = tools[inventorySelected];

                if(s.Input.WasPressed(Input.Action.Item1))
                {
                    if (selectedTool == room.Diver.Tool2) room.Diver.Tool2 = room.Diver.Tool1;
                    room.Diver.Tool1 = selectedTool;
                }

                if (s.Input.WasPressed(Input.Action.Item2))
                {
                    if (selectedTool == room.Diver.Tool1) room.Diver.Tool1 = room.Diver.Tool2;
                    room.Diver.Tool2 = selectedTool;
                }

                //System.Console.WriteLine("GUI!! " + s.Time.TotalRealTime.Milliseconds);
            }
            else
            {
                if (s.Input.WasPressed(Input.Action.Select) && diverInInventory)
                {
                    isGUIActive = true;
                    room.Diver.Freeze = true;
                }
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
                toolConstructorStrings[i] = "new " + tool.GetType().Name + "()";
                i++;
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
