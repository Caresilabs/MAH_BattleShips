using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleship.Entity;
using Battleship.Model;
using Battleship.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Battleship.Managers
{
    public class InputManager
    {
        private World world;
        private Vector2 mouse;
        private KeyboardState oldKeyState;
        private MouseState oldMouseState;

        private Ship currentShip;
        private Vector2 origin;
        private Vector2 mouseWorld;
        private bool showingShips;

        public InputManager(World world)
        {
            this.world = world;
            this.mouse = new Vector2();
            this.origin = new Vector2();
        }

        public void update(float delta, ShipField field, ShipField targetField = null)
        {
            mouse.X = Mouse.GetState().X;
            mouse.Y = Mouse.GetState().Y;

            mouseWorld = Camera2D.unproject(mouse.X, mouse.Y);

            // INIT
            if (world.getState() == World.State.Player1Init || world.getState() == World.State.Player2Init)
            {
                updatePlacingShips(field);
            }
            else if (world.getState() == World.State.Player1Turn || world.getState() == World.State.Player2Turn)// Our turn
            {
                // Draw hover
                if (Mouse.GetState().LeftButton == ButtonState.Released)
                {
                    field.hover(mouseWorld.X, mouseWorld.Y);
                    if (targetField != null) targetField.hover(mouseWorld.X, mouseWorld.Y, Tile.TileEffect.BombMark);
                }

                // FIRE MAH LAZER!!
                if (wasClicked() && targetField != null)
                {
                    if (targetField.hit(mouseWorld.X, mouseWorld.Y))
                    {
                        nextTurn(field);
                    }
                }

                // Show ships
                if (wasClicked(Keys.Space))
                {
                    if (showingShips)
                        field.hideShips();
                    else
                        field.showShips();

                    showingShips = !showingShips;
                }

                // debug
                if (wasClicked(Keys.Q))
                {
                    field.hideShips();
                    world.nextTurn();
                }
            }

            oldKeyState = Keyboard.GetState();
            oldMouseState = Mouse.GetState();
        }

        private void updatePlacingShips(ShipField field)
        {
            // Catch ship
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && currentShip == null)
            {
                currentShip = field.getShipByMouse(mouseWorld.X, mouseWorld.Y);
                if (currentShip != null)
                {
                    origin.X = mouseWorld.X - currentShip.getX();
                    origin.Y = mouseWorld.Y - currentShip.getY();
                }
            }

            // manage ship
            if (currentShip != null)
            {
                // Rotate
                if (wasClicked(Keys.R))
                {
                    field.rotateShip(currentShip);
                    float origX = origin.X;
                    origin.X -= origin.X - origin.Y;
                    origin.Y -= origin.Y - origX;
                }

                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    field.move(currentShip, mouseWorld.X - origin.X, mouseWorld.Y - origin.Y);
                }
                else
                {
                    field.placeShip(currentShip);
                    currentShip = null;
                }
            }

            // Finished placing boats
            if (wasClicked(Keys.Space))
            {
                field.attachShips();
                nextTurn(field);
            }
        }

        private void nextTurn(ShipField field)
        {
            world.nextTurn();
            showingShips = false;
            field.hideShips();
        }

        public bool wasClicked(Keys key)
        {
            if (Keyboard.GetState().IsKeyUp(key) && oldKeyState.IsKeyDown(key))
                return true;
            else
                return false;
        }

        public bool wasClicked()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }
    }
}
