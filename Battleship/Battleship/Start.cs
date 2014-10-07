using System;
using System.Collections.Generic;
using System.Linq;
using Battleship.Controller;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Battleship.Tools;

namespace Battleship
{
    public class Start : Microsoft.Xna.Framework.Game
    {
        public const string GAME_NAME = "Battle of Great Sea Uhyaa";

        private static GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Screen currentScreen;

        private float aspectRatio;

        public Start()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            // Todo resize bug

            aspectRatio = graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;

            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            // graphics.ToggleFullScreen();
            Window.Title = GAME_NAME + " by [Simon Bothen]"; //  set title to our game name
            Window.ClientSizeChanged += new EventHandler<EventArgs>(WindowSizeChanged);

            
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
            
            Assets.load(Content);
            Camera2D.setGraphics(GraphicsDevice);

            // init startup screen
            setScreen(getStartScreen());
        }

        protected override void UnloadContent()
        {
            // Unload any non ContentManager content here
            Assets.unload();
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // get second between last frame and current frame, used for fair physics manipulation and not based on frames
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // then update the screen
            currentScreen.update(delta);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Draw screen
            currentScreen.draw(spriteBatch);

            base.Draw(gameTime);
        }

        internal void setScreen(Screen newScreen)
        {
            if (newScreen == null) return;

            // Dispose old screen
            if (currentScreen != null)
                currentScreen.dispose();

            // init new screen
            currentScreen = newScreen;
            newScreen.setGame(this);
            newScreen.setGraphics(Camera2D.getDefaultGraphics());
            newScreen.setDefaultViewPort(Camera2D.getDefaultGraphics().Viewport);
            currentScreen.init();
        }

        public void WindowSizeChanged(object sender, EventArgs e)
        {
            int new_width = graphics.GraphicsDevice.Viewport.Width;
            int new_height = graphics.GraphicsDevice.Viewport.Height;


            graphics.PreferredBackBufferWidth = new_width;
            graphics.PreferredBackBufferHeight = (int)(new_width / aspectRatio);
            graphics.ApplyChanges();
        }

        public static void changeResolution(int width, int height)
        {
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.ApplyChanges();
        }

        private Screen getStartScreen()
        {
            return new MainMenuScreen();
        }
    }
}
