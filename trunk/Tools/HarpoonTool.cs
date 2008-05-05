using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using DB.DoF.Entities;
using DB.Gui;

namespace DB.DoF.Tools
{
    public class HarpoonTool : ITool
    {
        enum Action
        {
            None,
            Shooting,
            Pulling,
            Retracting
        };

        Texture2D icon;
        public Texture2D Icon { get { return icon; } }
        Action action;
        int length;
        int direction;
        const int maxLength = 70;
        const int shootSpeed = 8;
        const int pullSpeed = 4;

        public HarpoonTool()
        {
            icon = DiverGame.DefaultContent.Load<Texture2D>("iconharpoon");
        }

        public void Update(Diver diver, Room room, State s)
        {
            switch (action)
            {
                case Action.Shooting:
                    length += shootSpeed;
                    int tipX = diver.Center.X + length * direction;
                    int tipY = diver.Center.Y;
                    if (room.TileMap.IsSolid(tipX / 16, tipY / 16))
                    {
                        action = Action.Pulling;
                    } 
                    else if (length > maxLength)
                    {
                        action = Action.Retracting;
                    }
                    break;

                case Action.Pulling:
                    length -= pullSpeed;
                    diver.X += direction * pullSpeed;
                    if (length <= 0)
                    {
                        action = Action.None;
                        diver.Freeze = false;
                    }
                    break;

                case Action.Retracting:
                    length -= pullSpeed;
                    if (length <= 0)
                    {
                        action = Action.None;
                        diver.Freeze = false;
                    }
                    break;

                case Action.None:
                    break;
            }
        }

        public void OnUse(Diver diver, Room room)
        {
            if (action == Action.None)
            {
                action = Action.Shooting;
                length = 0;
                direction = diver.FacingDirection;
                diver.Freeze = true;
            }
        }

        public void Draw(Graphics graphics, Diver diver)
        {
            graphics.Begin();

            graphics.End();
        }
    }
}
