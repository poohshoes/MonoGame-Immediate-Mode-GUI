using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IansMonogameImgui
{
    public enum ImguiMode
    {
        Update,
        Render
    }

    public class Imgui
    {
        public ImguiMode Mode;
        public SpriteBatch SpriteBatch;
        //// TODO(ian): Should we just whatever integer is equivalent to a guid and then people can use a guid if they want and I will just cast it down?
        //public Dictionary<Guid, IPersist> PersistentInfo = new Dictionary<Guid, IPersist>();
        public Input Input = new Input();

        //TODO(ian): Generate the pixel on the fly so we don't need any content for this library to work.
        public Texture2D Pixel;

        //public int LastItemsHeight;
        //public int LastItemsWidth;

        public void Update(GameTime gameTime, MouseState mouseState, MouseState lastMouseState, KeyboardState keyboardState, KeyboardState lastKeyboardState)
        {
            Input.Mouse = mouseState;
            Input.LastMouse = lastMouseState;
            Input.Keyboard = keyboardState;
            Input.LastKeyboard = lastKeyboardState;

            //foreach (IPersist info in PersistentInfo.Values)
            //{
            //    info.Update(gameTime);
            //}
        }

        /// <summary>
        /// Items that are not drawn between calls to BeforeDraw will lose any persistent information that they had.
        /// </summary>
        public void BeforeDraw()
        {
            //var keysToRemove = new List<Guid>();
            //foreach (Guid key in PersistentInfo.Keys)
            //{
            //    if (PersistentInfo[key].UsedThisFrame == false)
            //    {
            //        keysToRemove.Add(key);
            //    }

            //    PersistentInfo[key].UsedThisFrame = false;
            //}
            //foreach (Guid key in keysToRemove)
            //{
            //    PersistentInfo.Remove(key);
            //}
        }

        public void DoText(Vector2 position, TextDrawData data)
        {
            if (Mode == ImguiMode.Render)
            {
                SpriteBatch.DrawString(data.Font, data.Text, position, data.Color);
            }
        }

        public bool DoTextButton(Vector2 position, TextDrawData textData, Color background)
        {
            Vector2 padding = new Vector2(6f, 3f);
            Vector2 size = textData.Font.MeasureString(textData.Text) + 2f * padding;

            switch (Mode)
            {
                case ImguiMode.Update:
                {
                    return Input.GetMode(position, size) == InputMouseMode.Clicked;
                }
                case ImguiMode.Render:
                {
                    Color backgroundColor = background;

                    InputMouseMode mouseMode = Input.GetMode(position, size);
                    // Shift the color for Hover and Down
                    {
                        int change = 60;
                        if (mouseMode == InputMouseMode.Hover)
                        {
                            backgroundColor = PushColor(backgroundColor, change);
                        }
                        else if (mouseMode == InputMouseMode.Down)
                        {
                            backgroundColor = PushColor(backgroundColor, -change);
                        }
                    }

                    // Draw bars around the box.
                    Vector2 barSize = new Vector2(3);
                    {
                        int change = 60;
                        Color barColor = PushColor(backgroundColor, change);
                        // Top
                        DrawFilledBox(position, size.X, barSize.Y, barColor);
                        // Left
                        DrawFilledBox(position + new Vector2(0f, barSize.Y), barSize.X, size.Y - barSize.Y, barColor);
                        barColor = PushColor(backgroundColor, -change);
                        // Bottom
                        DrawFilledBox(position + new Vector2(barSize.X, size.Y - barSize.Y), size.X - barSize.X, barSize.Y, barColor);
                        // Right
                        DrawFilledBox(position + new Vector2(size.X - barSize.X, barSize.Y), barSize.X, size.Y - 2f * barSize.Y, barColor);
                    }

                    DrawFilledBox(position + barSize, size.X - 2 * barSize.X, size.Y - 2 * barSize.Y, backgroundColor);
                    SpriteBatch.DrawString(textData.Font, textData.Text, position + padding, textData.Color);
                    return false;
                }
                default:
                    throw new Exception("Case not handled");
            }
        }

        public void DrawFilledBox(Vector2 topLeft, float width, float height, Color color)
        {
            DrawFilledBox(topLeft, (int)width, (int)height, color);
        }

        public void DrawFilledBox(Vector2 topLeft, int width, int height, Color color)
        {
            if (Mode == ImguiMode.Render)
            {
                SpriteBatch.Draw(Pixel, new Rectangle((int)topLeft.X, (int)topLeft.Y, width, height), new Rectangle(0, 0, (int)Pixel.Width, (int)Pixel.Height), color);
            }
        }

        public void DrawLine(Vector2 point1, Vector2 point2, float width, Color color)
        {
            if (Mode == ImguiMode.Render)
            {
                float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
                float length = Vector2.Distance(point1, point2);
                SpriteBatch.Draw(Pixel, point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0f);
            }
        }

        public Color PushColor(Color color, int push)
        {
            if (push > 0)
            {
                color.R = (byte)Math.Min(255, (short)color.R + push);
                color.G = (byte)Math.Min(255, (short)color.G + push);
                color.B = (byte)Math.Min(255, (short)color.B + push);
            }
            else
            {
                color.R = (byte)Math.Max(0, (short)color.R + push);
                color.G = (byte)Math.Max(0, (short)color.G + push);
                color.B = (byte)Math.Max(0, (short)color.B + push);
            }
            return color;
        }
        
        public int DoDropDown(Vector2 position, int width, int height, ref bool isOpen, TextDrawData textData, List<string> options, int selectedIndex)
        {
            // Note(ian): Do this first so that the click for the text button doesn't get picked up again here.
            int result = -1;
            if (Mode == ImguiMode.Update && isOpen)
            {
                if (Input.GetMode(position, new Vector2(width, height)) == InputMouseMode.Clicked)
                {
                    float yOffset = Input.Mouse.Y - position.Y;
                    int optionStart = textData.Font.LineSpacing;
                    int i;
                    for (i = 0; i < options.Count; i++)
                    {
                        if (optionStart < yOffset && yOffset <= optionStart + textData.Font.LineSpacing)
                        {
                            result = i;
                            isOpen = false;
                        }
                        optionStart += textData.Font.LineSpacing;
                    }
                }
            }

            Panel dropDown = new Panel(this, position, width, height);
            dropDown.DoText(textData);
            position.X += textData.Font.MeasureString(textData.Text).X;
            if (DoTextButton(position, new TextDrawData("v", textData.Font, textData.Color), Color.DarkMagenta))
            {
                isOpen = !isOpen;
            }
            // TODO(ian): Can we get the button width somehow?
            position.X += 25;
            DoText(position, new TextDrawData(options[selectedIndex], textData.Font, textData.Color));
            position.Y += textData.Font.LineSpacing;

            if (isOpen)
            {
                // TODO(ian): Add some spacing and a border for the drop down.
                // Todo(ian): Draw and update here.
                int dropDownScrollValue = 0;// TODO(ian): where do i get this  from?
                dropDown.BeginScrollableSection(dropDownScrollValue);
                foreach (string category in options)
                {
                    dropDown.DoText(new TextDrawData(category, textData.Font, textData.Color));
                }
                dropDown.EndScrollableSection(ref dropDownScrollValue);
            }

            return result;
        }
    }
}
