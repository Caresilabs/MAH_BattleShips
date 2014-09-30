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

            switch (world.getState())
            {
                case World.State.Player1Init:
                    batch.DrawString(Assets.font, "Press Space to finish", new Vector2(0, 50), Color.White);
                    break;
                case World.State.Player2Init:
                    if (world.getMode() == World.Mode.PlayerVSPlayer)
                    {
                        batch.DrawString(Assets.font, "Press Space to finish", new Vector2(0, 50), Color.White);
                    }
                    break;
                case World.State.Player1Turn:
                    batch.DrawString(Assets.font, "Press Space to Toggle ships visible", new Vector2(0, 50), Color.White);
                    break;
                case World.State.Player2Turn:
                    if (world.getMode() == World.Mode.PlayerVSPlayer)
                    {
                        batch.DrawString(Assets.font, "Press Space to Toggle ships visible", new Vector2(0, 50), Color.White);
                    }
                    break;
                case World.State.Player1Win:
                    break;
                case World.State.Player2Win:
                    break;
                default:
                    break;
            }
           
        }
    }
}
