using Battleship.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.View
{
    public class HUD
    {
        private World world;
        
        public HUD(World world)
        {
            this.world = world;
        }

        public void update(float delta)
        {

        }

        public void draw(SpriteBatch batch)
        {
            batch.DrawString(Assets.font, "" + world.getState(), Vector2.Zero, Color.White);
        }
    }
}
