using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IansMonogameImgui
{
    public enum InputMouseMode
    {
        None,
        Hover,
        Down,
        Clicked
    }

    public class Input
    {
        public MouseState Mouse;
        public MouseState LastMouse;
        public KeyboardState Keyboard;
        public KeyboardState LastKeyboard;
        
        internal InputMouseMode GetMode(Vector2 position, Vector2 size)
        {
            if (Mouse.Position.X > position.X
                && Mouse.Position.Y > position.Y
                && Mouse.Position.X < position.X + size.X
                && Mouse.Position.Y < position.Y + size.Y)
            {
                if (Mouse.LeftButton == ButtonState.Released
                    && LastMouse.LeftButton == ButtonState.Pressed)
                {
                    return InputMouseMode.Clicked;
                }
                else if (Mouse.LeftButton == ButtonState.Pressed)
                {
                    return InputMouseMode.Down;
                }
                else
                {
                    return InputMouseMode.Hover;
                }
            }
            else
            {
                return InputMouseMode.None;
            }
        }

        internal int GetScrollChange()
        {
            return Mouse.ScrollWheelValue - LastMouse.ScrollWheelValue;
        }
    }
}
