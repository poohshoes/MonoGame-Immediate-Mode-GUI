#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using IansMonogameImgui;
using MonogameImgui.PrefabConstruct;
#endregion

namespace MonogameImgui
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Imgui imgui = new Imgui();

        SpriteFont font, boldFont;
        MouseState lastMouseState;
        KeyboardState lastKeyboardState;

        List<Prefab> Prefabs = new List<Prefab>();

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1300;
            graphics.PreferredBackBufferHeight = 800;
        }

        protected override void Initialize()
        {
            base.Initialize();

            Prefabs.Add(new Prefab("One") { Category = "A" });
            Prefabs.Add(new Prefab("Two") { Category = "A" });
            Prefabs[1].Components.Add(new PositionComponent("Position", 0f, 0f));
            Prefabs.Add(new Prefab("Three") { Category = "A" });
            Prefabs[2].Components.Add(new PositionComponent("My Position", 10f, 15f));
            Prefabs[2].Components.Add(new ButtsComponent("My Butts") { NumberOfButts = 5, TextField = "salty" });

            for (int i = 0; i <= 50; i++)
            {
                Prefabs.Add(new Prefab(i.ToString()) { Category = "B" });
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            imgui.SpriteBatch = spriteBatch;
            imgui.Pixel = Content.Load<Texture2D>("pixel");
            font = Content.Load<SpriteFont>("SegoeUIMono12");
            boldFont = Content.Load<SpriteFont>("SegoeUIMono12Bold");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();

            imgui.Update(gameTime, mouseState, lastMouseState, keyboardState, lastKeyboardState);
            ImguiUpdateRender(ImguiMode.Update);

            base.Update(gameTime);
            lastMouseState = mouseState;
            lastKeyboardState = keyboardState;
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            imgui.BeforeDraw();
            spriteBatch.Begin();
            // Draw your video game here.
            ImguiUpdateRender(ImguiMode.Render);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        string test = "a";
        int prefabScroll = 0;
        int categoryIndex = 0;
        bool isCategoryDropDownOpen = false;
        private void ImguiUpdateRender(ImguiMode mode)
        {
            imgui.Mode = mode;

            if (imgui.DoTextButton(new Vector2(10, 10), new TextDrawData("Expand Text", font, Color.Yellow), Color.Blue))
            {
                if (test[test.Length - 1] == 'a')
                    test += 'b';
                else
                    test += 'a';
            }

            imgui.DoText(new Vector2(10, 100), new TextDrawData(test, font, Color.Black));
            
            // Prefab List
            {
                int listWidth = 200;
                Panel panel = new Panel(imgui, new Vector2(graphics.PreferredBackBufferWidth - listWidth, 50), listWidth, graphics.PreferredBackBufferHeight - 100);

                List<string> categories = Prefabs.Select(x => x.Category).Distinct().ToList();
                int newCategoryIndex = panel.DoDropDown(ref isCategoryDropDownOpen, new TextDrawData("Category", font, Color.Yellow), categories, categoryIndex);
                if (newCategoryIndex != -1 && newCategoryIndex != categoryIndex)
                {
                    prefabScroll = 0;
                    categoryIndex = newCategoryIndex;
                }
                panel.DoLine(Color.Yellow);
                panel.BeginScrollableSection(prefabScroll);
                foreach (Prefab prefab in Prefabs.Where(x => x.Category == categories[categoryIndex]))
                {
                    panel.DoText(new TextDrawData(prefab.Name, font, Color.Yellow));
                }
                panel.EndScrollableSection(ref prefabScroll);
            }
        }
    }
}
