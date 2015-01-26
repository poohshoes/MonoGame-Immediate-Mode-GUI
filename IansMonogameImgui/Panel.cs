using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace IansMonogameImgui
{
    public class Panel
    {
        Imgui imgui;
        public Vector2 StartPosition;
        Vector2 currentPosition;
        public int Width;
        public int Height;

        public Scroll scroll;

        public Panel(Imgui imgui, Vector2 startPosition, int width, int height)
        {
            this.imgui = imgui;
            StartPosition = startPosition;
            currentPosition = startPosition;
            Width = width;
            Height = height;

            if (imgui.Mode == ImguiMode.Render)
            {
                imgui.DrawFilledBox(startPosition, width, height, Color.DarkBlue);
            }
        }

        public void DoLine(Color color)
        {
            imgui.DrawLine(new Vector2(currentPosition.X, currentPosition.Y), new Vector2(currentPosition.X + Width, currentPosition.Y), 1f, color);
        }

        public int DoDropDown(ref bool isOpen, TextDrawData textData, List<string> options, int selectedIndex)
        {
            Debug.Assert(scroll == null);
            int dropDownHeight = textData.Font.LineSpacing;
            if (isOpen)
            {
                dropDownHeight += options.Count * textData.Font.LineSpacing;
            }
            Debug.Assert(dropDownHeight <= GetRemainingHeight());
            int result = imgui.DoDropDown(currentPosition, Width, dropDownHeight, ref isOpen, textData, options, selectedIndex);
            currentPosition.Y += dropDownHeight;
            return result;
        }

        public void DoText(TextDrawData data)
        {
            int itemHeight = data.Font.LineSpacing;
            if (scroll == null)
            {
                imgui.DoText(currentPosition, data);
                currentPosition.Y += itemHeight;
            }
            else
            {
                if(scroll.ScrollIt(itemHeight, currentPosition))
                {
                    imgui.DoText(currentPosition, data);
                    currentPosition.Y += itemHeight;
                }
            }
        }

        public void BeginScrollableSection(int scrollValue)
        {
            Debug.Assert(scroll == null);
            scroll = new Scroll(imgui, currentPosition, Width, GetRemainingHeight(), scrollValue);
        }

        public void EndScrollableSection(ref int scrollValue)
        {
            scroll.EndScrollableSection(ref scrollValue);
            scroll = null;
        }

        private int GetRemainingHeight()
        {
            return Height - (int)(currentPosition.Y - StartPosition.Y);
        }
    }
}
