using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Asteroid.Tools;
using Microsoft.Xna.Framework;
using Battleship.Model;
using Battleship.Tools;

namespace Battleship.View
{
    /**
     * This class is responsible for drawing the game, thats it. AKA the 'View' in MVC pattern
     */
    public class WorldRenderer
    {
        private World world;
        private Camera2D camera;
        private Texture2D bg;

        public WorldRenderer(World world)
        {
            this.world = world;
            this.camera = new Camera2D(1280, 720);
            this.bg = Assets.bg2;
        }

        public void update(float delta)
        {
            camera.update(delta);
        }

        public void render(SpriteBatch batch)
        {
            // begin batch with cameras matrix
            batch.Begin(SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        SamplerState.LinearClamp,
                        null,
                        null,
                        null,
                        camera.getMatrix());

            drawBackground(batch);

            batch.Draw(Assets.ui, new Rectangle(-400 / 2, -720 / 2 - 10, 400, 135), Assets.getRegion("uiContainer1"), Color.White);
        
            drawTiles(batch);

            batch.End();
        }

        private void drawTiles(SpriteBatch batch)
        {
            world.getFieldLeft().draw(batch);
            world.getFieldRight().draw(batch);
        }

        private void drawBackground(SpriteBatch batch)
        {
            batch.Draw(bg, new Rectangle(-1280/2,-720/2, 1280,720), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
        }

        public Camera2D getCamera()
        {
            return camera;
        }
    }
}
