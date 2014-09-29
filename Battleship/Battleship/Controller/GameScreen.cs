using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleship.Managers;
using Battleship.Model;
using Battleship.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Battleship.Controller
{
    /**
     * A game screen that manages the world, renderer and input and put them togheter in a convenient way
     */
    public class GameScreen : Screen
    {
        private World world;
        private WorldRenderer renderer;
        private GameState state;
        private InputManager input;
        private HUD hud;
        private float stateTime;

        public enum GameState
        {
            PAUSED, RUNNING, GAMEOVER
        }

        public override void init()
        {
            this.world = new World(World.Mode.PlayerVSPlayer);
            this.renderer = new WorldRenderer(getDefaultViewPort(), getGraphics(), world);
            this.state = GameState.PAUSED;
            this.input = new InputManager(world);
            this.hud = new HUD(world);
        }

        public override void update(float delta)
        {
            stateTime += delta;

            // Update classes
            renderer.update(delta);
            hud.update(delta);
            world.update(delta);

            // Input update
            switch (world.getState())
            {
                case World.State.Player1Init:
                    input.update(delta, world.getCurrentField());
                    break;
                case World.State.Player2Init:
                    if (world.getMode() == World.Mode.PlayerVSPlayer)
                    {
                        input.update(delta, world.getCurrentField());
                    }
                    break;
                case World.State.Player1Turn:
                    input.update(delta, world.getCurrentField(), world.getTargetField());
                    break;
                case World.State.Player2Turn:
                    if (world.getMode() == World.Mode.PlayerVSPlayer)
                    {
                        input.update(delta, world.getCurrentField(), world.getTargetField());
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


        public override void draw(SpriteBatch batch)
        {
            getGraphics().Clear(Color.Magenta);

            // Draw game
            renderer.render(batch);

            // Draw ui
            batch.Begin();
            hud.draw(batch);
            batch.End();
        }

        public override void dispose()
        {

        }

        public GameState getState()
        {
            return state;
        }

    }
}
