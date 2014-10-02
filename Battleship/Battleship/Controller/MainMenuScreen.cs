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
    public class MainMenuScreen : Screen
    {
        public override void init()
        {

        }

        public override void update(float delta)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                setScreen(new GameScreen());
        }

        public override void draw(SpriteBatch batch)
        {
            getGraphics().Clear(Color.SeaGreen);

            batch.Begin();

            HUD.drawCenterString(batch, Start.GAME_NAME, 100, 1.9f);

            HUD.drawCenterString(batch, "Start", 250, 1.4f);

            HUD.drawCenterString(batch, "Exit", 350, 1.4f);

            batch.End();
        }

        public override void dispose()
        {
        }
    }
}
