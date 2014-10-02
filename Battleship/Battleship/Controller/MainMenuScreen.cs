using Battleship.Entity;
using Battleship.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.Controller
{
    public class MainMenuScreen : Screen, TouchListener
    {

        private List<MenuButton> buttons;

        public override void init()
        {
            this.buttons = new List<MenuButton>();


            MenuButton start = new MenuButton(this, "startButton", "start", getGraphics().Viewport.Width/2, 250, 1.5f);

            buttons.Add(start);
        }

        public override void update(float delta)
        {
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

            batch.Begin();

            foreach (var item in buttons)
            {
                item.draw(batch);
            }

            HUD.drawCenterString(batch, Start.GAME_NAME, 100, 1.9f);

//            HUD.drawCenterString(batch, "Exit", 350, 1.4f);

            batch.End();
        }

        public override void dispose()
        {
        }

        void TouchListener.touchDown(string name) {
            if (name == "startButton")
            {
                setScreen(new GameScreen());
            }
            if (name == "exitButton")
            {
                getGame().Exit();
            }
        }
    }
}
