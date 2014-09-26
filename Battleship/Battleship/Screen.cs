using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleship
{
    public abstract class Screen
    {
        private GraphicsDevice graphics;

        private Viewport defaultViewPort;

        private Start game;

        public abstract void init();

        public abstract void update(float delta);

        public abstract void draw(SpriteBatch batch);

        public abstract void dispose();

        public void setScreen(Screen newScreen)
        {
            game.setScreen(newScreen);
        }

        public void setGame(Start game)
        {
            this.game = game;
        }

        public GraphicsDevice getGraphics()
        {
            return graphics;
        }

        public Viewport getDefaultViewPort()
        {
            return defaultViewPort;
        }

        public void setGraphics(GraphicsDevice graphics)
        {
            this.graphics = graphics;
        }

        public void setDefaultViewPort(Viewport viewport)
        {
            this.defaultViewPort = viewport;
        }
    }
}
