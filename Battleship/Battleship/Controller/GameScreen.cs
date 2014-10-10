using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleship.Entity;
using Battleship.Managers;
using Battleship.Model;
using Battleship.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Asteroid.Tools;

namespace Battleship.Controller
{
    /**
     * A game screen that manages the world, renderer and input and put them togheter in a convenient way
     */
    public class GameScreen : Screen
    {
        private List<Particle> confettis;

        private World world;
        private WorldRenderer renderer;
        private InputManager input;
        private HUD hud;
        private World.Mode mode;
        private float stateTime;

        public GameScreen(World.Mode mode)
        {
            this.mode = mode;
        }

        public override void init()
        {
            this.confettis = new List<Particle>();
            this.world = new World(mode);
            this.renderer = new WorldRenderer(world);
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
            updateConfetti(delta);

            // Input update
            switch (world.getState())
            {
                case World.State.Player1Init:
                    if (world.getMode() == World.Mode.AIVSAI)
                    {
                        ((AIShipField)world.getCurrentField()).updateAI(delta, world.getTargetField());
                    }
                    else
                    {
                        input.update(delta, world.getCurrentField()); 
                    }
                    break;
                case World.State.Player2Init:
                    if (world.getMode() == World.Mode.PlayerVSPlayer)
                    {
                        input.update(delta, world.getCurrentField());
                    }
                    else
                    {
                        input.updateHover(world.getCurrentField());
                        ((AIShipField)world.getCurrentField()).updateAI(delta, world.getTargetField());
                    }
                    break;
                case World.State.Player1Turn:
                    if (world.getMode() == World.Mode.AIVSAI)
                    {
                        ((AIShipField)world.getCurrentField()).updateAI(delta, world.getTargetField());
                    }
                    else
                    {
                        input.update(delta, world.getCurrentField(), world.getTargetField());
                    }
                    break;
                case World.State.Player2Turn:
                    if (world.getMode() == World.Mode.PlayerVSPlayer)
                    {
                        input.update(delta, world.getCurrentField(), world.getTargetField());
                    }
                    else
                    {
                        ((AIShipField)world.getCurrentField()).updateAI(delta, world.getTargetField());
                        input.updateHover(world.getCurrentField());
                    }
                    break;
                case World.State.Player1Win:
                    if (MathUtils.random(1, 10) >= 5)
                    {
                        spawnConfetti();
                    }
                    break;
                case World.State.Player2Win:
                    if (MathUtils.random(1, 10) >= 5)
                    {
                        spawnConfetti();
                    }
                    break;
                default:
                    break;
            }

            // Restart
            if (Keyboard.GetState().IsKeyDown(Keys.R))
                setScreen(new GameScreen(mode));

            if (Keyboard.GetState().IsKeyDown(Keys.M))
                setScreen(new MainMenuScreen());
        }

        private void updateConfetti(float delta)
        {
            for (int i = 0; i < confettis.Count; i++)
			{
                Particle confetti = confettis[i];
			    confetti.update(delta);
                if (confetti.getPosition().Y > getGraphics().Viewport.Height)
                {
                    confettis.Remove(confetti);
                }
			}
        }

        public void spawnConfetti()
        {
            Particle confetti = new Particle(MathUtils.random(getGraphics().Viewport.Width), -Particle.SIZE_DEFAULT, MathUtils.random(-300f, 300f), MathUtils.random(200f, 600f));
            confettis.Add(confetti);
        }

        public override void draw(SpriteBatch batch)
        {
            getGraphics().Clear(Color.SeaGreen);
            // Draw game
            renderer.render(batch);

            // Draw ui
            batch.Begin();
            {
                hud.draw(batch);
                drawConfetti(batch);
            }
            batch.End();
        }

        private void drawConfetti(SpriteBatch batch)
        {
            foreach (var confetti in confettis)
            {
                confetti.draw(batch);
            }
        }

        public override void dispose()
        {

        }
    }
}
