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
    public class CreateGameScreen : Screen, TouchListener
    {
        private Vector2[] sizes = new Vector2[]{ 
            new Vector2(10, 10),
            new Vector2(14, 14),
            new Vector2(16, 16),
            new Vector2(20, 20)
        };

        private int[] ships = new int[]{ 
           3,
           4,
           5,
           7,
           9,
           10
        };

        private Camera2D camera;
        private List<MenuButton> buttons;
        private MouseState oldMouseState;
        private World.Mode mode;
        MenuButton countButton;
        MenuButton sizeButton;

        private int gridSizeId;
        private int shipCountId;

        public CreateGameScreen(World.Mode mode)
        {
            this.mode = mode;
        }

        public override void init()
        {
            this.buttons = new List<MenuButton>();
            this.camera = new Camera2D(1280, 720);
            this.shipCountId = 2;
            this.gridSizeId = 1;
            initButtons();
        }

        private void initButtons()
        {
            MenuButton startButton = new MenuButton(this, "start", "Start!", 0, 190, 1.3f);
            buttons.Add(startButton);

            countButton = new MenuButton(this, "count", "ships: " + ships[shipCountId], 0, 70, 1.3f);
            buttons.Add(countButton);

            sizeButton = new MenuButton(this, "size", sizes[gridSizeId].X + "x" + sizes[gridSizeId].Y, 0, -50, 1.3f);
            buttons.Add(sizeButton);
        }

        public override void update(float delta)
        {
            camera.update(delta);

            if (wasClicked())
            {
                foreach (var item in buttons)
                {
                    item.click(Mouse.GetState().X, Mouse.GetState().Y);
                }
            }
            else if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                foreach (var item in buttons)
                {
                    item.touchDown(Mouse.GetState().X, Mouse.GetState().Y);
                }
            }
            else if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                foreach (var item in buttons)
                {
                    item.touchUp();
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

            batch.Draw(Assets.ui, new Vector2(-256 * 1.5f, -240), Assets.getRegion("title"), Color.White, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);

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
            if (name == "count")
            {
                shipCountId++;
                if (shipCountId >= ships.Count())
                    shipCountId = 0;

                countButton.setText("ships: " + ships[shipCountId]);
            }
            else if (name == "size")
            {
                gridSizeId++;
                if (gridSizeId >= sizes.Count())
                    gridSizeId = 0;

                sizeButton.setText(sizes[gridSizeId].X + "x" + sizes[gridSizeId].Y);
            }
            else if (name == "start")
            {
                World.FIELD_SIZE = (int)sizes[gridSizeId].X;
                ShipField.SHIPS_COUNT = ships[shipCountId];
                setScreen(new GameScreen(mode));
            }
        }
    }
}
