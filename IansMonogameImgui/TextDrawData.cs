using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IansMonogameImgui
{
    public class TextDrawData
    {
        public string Text;
        public SpriteFont Font;
        public Color Color;

        public TextDrawData(string text, SpriteFont font, Color color)
        {
            Text = text;
            Font = font;
            Color = color;
        }
    }
}
