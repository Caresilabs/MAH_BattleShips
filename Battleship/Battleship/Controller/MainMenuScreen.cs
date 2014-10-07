using Battleship.Entity;
using Battleship.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleship.Tools;
using Battleship.Model;

namespace Battleship.Controller
{
    public class MainMenuScreen : Screen, TouchListener
    {

        private Camera2D camera;
        private List<MenuButton> buttons;

        public override void init()
        {
            this.buttons = new List<MenuButton>();
            this.camera = new Camera2D(1280, 720);

            MenuButton startPVP = new MenuButton(this, "startButtonPVP", "Start P V. P", 0, -50, 1.3f);
            buttons.Add(startPVP);

            MenuButton startPVE = new MenuButton(this, "startButtonPVE", "Start P V. AI", 0, 70, 1.3f);
            buttons.Add(startPVE);
        }

        public override void update(float delta)
        {
            camera.update(delta);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                foreach (var item in buttons)
                {
                    item.touchDown(Mouse.GetState().X, Mouse.GetState().Y);
                }
            }
        }

        public override void draw(SpriteBatch batch)
        {
            getGraphics().Clear(Color.SeaGreen);

            batch.Begin(SpriteSortMode.BackToFront,
                       BlendState.AlphaBlend,
                       SamplerState.LinearClamp,
                       null,
                       null,
                       null,
                       camera.getMatrix());


            foreach (var item in buttons)
            {
                item.draw(batch);
            }

            drawCenterString(batch, Start.GAME_NAME, -200, Color.Red, 1.9f);

            //            HUD.drawCenterString(batch, "Exit", 350, 1.4f);

            batch.End();
        }

        public void drawCenterString(SpriteBatch batch, string text, float y, Color color, float scale = 1)
        {
            batch.DrawString(Assets.font, text,
                    new Vector2(
                         0 - ((Assets.font.MeasureString(text).Length() / 2) * scale), y),
                         color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public void drawCenterString(SpriteBatch batch, string text, float y, float scale = 1)
        {
            drawCenterString(batch, text, y, Color.White, scale);
        }

        public override void dispose()
        {
        }

        void TouchListener.touchDown(string name)
        {
            if (name == "startButtonPVP")
            {
                setScreen(new GameScreen(World.Mode.PlayerVSPlayer));
            }
            else if (name == "startButtonPVE")
            {
                setScreen(new GameScreen(World.Mode.PlayerVSAI));
            }
            else if (name == "exitButton")
            {
                getGame().Exit();
            }
        }
    }
}
