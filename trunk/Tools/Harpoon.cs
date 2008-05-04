using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using DB.DoF.Entities;
using DB.Gui;

namespace DB.DoF.Tools
{
    class Harpoon : ITool
    {
        Texture2D icon;
        public Texture2D Icon { get { return icon; } }

        public Harpoon()
        {
            icon = DiverGame.DefaultContent.Load<Texture2D>("iconharpoon");
        }

        public void Update(Diver diver, Room room, State s)
        {

        }

        public void OnUse(Diver diver, Room room)
        {

        }

        public void Draw(Graphics graphics, Diver diver)
        {
            graphics.Begin();

            graphics.End();
        }
    }
}
