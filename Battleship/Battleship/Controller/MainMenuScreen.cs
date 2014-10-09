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
        private MenuButton resolutionButton;
        private MouseState oldMouseState;
        private int currentRes;

        private Vector2[] resolutions = new Vector2[]{ 
            new Vector2(1280, 720),
           
            new Vector2(1024, 768),
            new Vector2(960, 640),
        };

        public override void init()
        {
            this.buttons = new List<MenuButton>();
            this.camera = new Camera2D(1280, 720);
            this.currentRes = 0;
            initButtons();
        }

        private void initButtons()
        {
            MenuButton startPVP = new MenuButton(this, "startButtonPVP", "Start VS Player", 0, -50, 1.3f);
            buttons.Add(startPVP);

            MenuButton startPVE = new MenuButton(this, "startButtonPVE", "Start VS AI", 0, 70, 1.3f);
            buttons.Add(startPVE);

            resolutionButton = new MenuButton(this, "resolution", resolutions[currentRes].X + "x" + resolutions[currentRes].Y, 0, 190, 1.3f);
            buttons.Add(resolutionButton);
        }

        public override void update(float delta)
        {
            camera.update(delta);

            if (wasClicked())
            {
                foreach (var item in buttons)
                {
                    item.touchDown(Mouse.GetState().X, Mouse.GetState().Y);
                }
            }

            oldMouseState = Mouse.GetState();
        }

        public bool wasClicked()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed)
                return true;
            else
                return false;
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

            batch.Draw(Assets.bg1, new Rectangle(-1280 / 2, -720 / 2, 1280, 720), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);

            foreach (var item in buttons)
            {
                item.draw(batch);
            }

            batch.Draw(Assets.ui, new Vector2(-256*1.5f, -240), Assets.getRegion("title"), Color.White, 0, Vector2.Zero, 1.5f ,SpriteEffects.None, 0);

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
            else if (name == "resolution")
            {
                currentRes++;
                if (currentRes >= resolutions.Count())
                    currentRes = 0;

                resolutionButton.setText(resolutions[currentRes].X + "x" + resolutions[currentRes].Y);

                Start.changeResolution((int)resolutions[currentRes].X, (int)resolutions[currentRes].Y);
            }
        }
    }
}
