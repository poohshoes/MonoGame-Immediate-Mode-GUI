using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IansMonogameImgui
{
    public class Scroll
    {
        Imgui imgui;
        int startScroll;
        int scroll;
        int skippedHeight;
        int width;
        int height;
        Vector2 startPosition;
        bool itemFailedToFit;

        public Scroll(Imgui imgui, Vector2 position, int width, int height, int scrollValue)
        {
            this.imgui = imgui;
            startScroll = scrollValue;
            this.scroll = scrollValue;
            skippedHeight = 0;
            this.width = width;
            this.height = height;
            startPosition = position;
            itemFailedToFit = false;
        }

        public int GetRemainingSpace(Vector2 position)
        {
            return height - (int)(position.Y - startPosition.Y);
        }

        public bool ScrollIt(int itemHeight, Vector2 position)
        {
            bool result;
            // Note(ian): This prevents a small item from being drawn after we have skipped a larger item.
            if (itemFailedToFit)
            {
                result = false;
                skippedHeight += itemHeight;
            }
            else if (scroll < 0)
            {
                scroll += itemHeight;
                result = false;
            }
            else
            {
                if (position.Y + itemHeight <= startPosition.Y + height)
                {
                    result = true;
                }
                else
                {
                    itemFailedToFit = true;
                    result = false;
                    skippedHeight += itemHeight;
                }
            }
            return result;
        }

        public void EndScrollableSection(ref int scrollValue)
        {
            if (imgui.Mode == ImguiMode.Update)
            {
                if (imgui.Input.GetMode(startPosition, new Vector2(width, height)) != InputMouseMode.None)
                {
                    int scrollChange = imgui.Input.GetScrollChange() / 2;
                    bool lastItemDrawn = skippedHeight == 0;
                    if (scrollChange > 0 || (scrollChange < 0 && !lastItemDrawn))
                    {
                        scrollValue += scrollChange;
                        if (scrollValue > 0)
                        {
                            scrollValue = 0;
                        }
                    }
                }
            }
            else if (imgui.Mode == ImguiMode.Render)
            {
                // Draw the scroll bar.
                Color barColor = Color.BlueViolet;
                Color darkBarColor = imgui.PushColor(barColor, -50);

                int barWidth = 10;
                int barSpace = 5;

                int scrollFirstY = scroll + (int)startPosition.Y;

                float rate = (height - 2 * barSpace) / (float)(-startScroll + height + skippedHeight);

                if (rate < 1f)
                {
                    float topBarHeight = -startScroll * rate;
                    float highlightHeight = height * rate;
                    float bottomBarHeight = skippedHeight * rate;

                    Vector2 position = startPosition + new Vector2(width - barWidth - barSpace, barSpace);

                    if (topBarHeight > 0)
                    {
                        imgui.DrawFilledBox(position, barWidth, topBarHeight, darkBarColor);
                        position.Y += topBarHeight;
                    }

                    // Note(ian): Our scrolling is janky and the variable scroll can get a little bigger then it should, here we try to compensate for this.
                    float jankyHeight = highlightHeight;
                    float maxJankyY = startPosition.Y + height - barSpace;
                    if (jankyHeight > maxJankyY - position.Y)
                    {
                        jankyHeight = maxJankyY - position.Y;
                    }
                    imgui.DrawFilledBox(position, barWidth, jankyHeight, barColor);
                    position.Y += jankyHeight;

                    if (bottomBarHeight > 0)
                    {
                        imgui.DrawFilledBox(position, barWidth, bottomBarHeight, darkBarColor);
                    }
                }
            }
        }
    }
}
