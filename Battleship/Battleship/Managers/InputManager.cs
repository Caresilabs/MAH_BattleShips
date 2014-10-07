using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleship.Entity;
using Battleship.Model;
using Battleship.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Battleship.View;

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
                updateChangeAttack(field);

                // Draw hover
                field.hover(mouseWorld.X, mouseWorld.Y);

                // Hover effects
                if (targetField != null)
                {
                    if (field.getSelectedAttack() == "Normal Strike")
                    {
                        targetField.hover(mouseWorld.X, mouseWorld.Y, Tile.TileEffect.BombMark);
                    }
                    else if (field.getSelectedAttack() == "Horizontal Strike")
                    {
                        for (int i = 0; i < World.FIELD_SIZE; i++)
                        {
                            targetField.hover(targetField.getBounds().X + i * World.TILE_SIZE, mouseWorld.Y, Tile.TileEffect.BombMark);
                        }
                    }
                    else if (field.getSelectedAttack() == "Vertical Strike")
                    {
                        for (int i = 0; i < World.FIELD_SIZE; i++)
                        {
                            targetField.hover(mouseWorld.X, targetField.getBounds().Y + i * World.TILE_SIZE, Tile.TileEffect.BombMark);
                        }
                    }
                    else if (field.getSelectedAttack() == "Circle Strike")
                    {
                        targetField.hover(mouseWorld.X, mouseWorld.Y + World.TILE_SIZE, Tile.TileEffect.BombMark);
                        targetField.hover(mouseWorld.X, mouseWorld.Y - World.TILE_SIZE, Tile.TileEffect.BombMark);
                        targetField.hover(mouseWorld.X + World.TILE_SIZE, mouseWorld.Y, Tile.TileEffect.BombMark);
                        targetField.hover(mouseWorld.X - World.TILE_SIZE, mouseWorld.Y, Tile.TileEffect.BombMark);
                    }
                    
                }

                // FIRE MAH LAZER!! Actual shooting
                if (wasClicked() && targetField != null)
                {
                    if (field.getSelectedAttack() == "Normal Strike")
                    {
                        if (targetField.hit(mouseWorld.X, mouseWorld.Y))
                        {
                            field.consumeAttack();
                            nextTurn(field);
                        }
                    }
                    else if (field.getSelectedAttack() == "Horizontal Strike")
                    {
                        bool hit = false;
                        for (int i = 0; i < World.FIELD_SIZE; i++)
                        {
                            if (targetField.hit(targetField.getBounds().X + i * World.TILE_SIZE, mouseWorld.Y)) hit = true;
                        }
                        if (hit)
                        {
                            field.consumeAttack();
                            nextTurn(field);
                        }
                    }
                    else if (field.getSelectedAttack() == "Vertical Strike")
                    {
                        bool hit = false;
                        for (int i = 0; i < World.FIELD_SIZE; i++)
                        {
                            if (targetField.hit(mouseWorld.X, targetField.getBounds().Y + i * World.TILE_SIZE)) hit = true;
                        }
                        if (hit)
                        {
                            field.consumeAttack();
                            nextTurn(field);
                        }
                    }
                    else if (field.getSelectedAttack() == "Circle Strike")
                    {
                        bool hit = false;
                        if (targetField.hit(mouseWorld.X, mouseWorld.Y + World.TILE_SIZE)) hit = true;
                        if (targetField.hit(mouseWorld.X, mouseWorld.Y - World.TILE_SIZE)) hit = true;
                        if (targetField.hit(mouseWorld.X + World.TILE_SIZE, mouseWorld.Y)) hit = true;
                        if (targetField.hit(mouseWorld.X - World.TILE_SIZE, mouseWorld.Y)) hit = true;

                        if (hit)
                        {
                            field.consumeAttack();
                            nextTurn(field);
                        }
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
            }

            oldKeyState = Keyboard.GetState();
            oldMouseState = Mouse.GetState();
        }

        private void updateChangeAttack(ShipField field)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                field.selectAttack(0);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                field.selectAttack(1);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                field.selectAttack(2);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D4))
            {
                field.selectAttack(3);
            }
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
                if (wasRightClicked())
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
            field.hideShips();
            world.nextTurn();
            showingShips = false;
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

        public bool wasRightClicked()
        {
            if (Mouse.GetState().RightButton == ButtonState.Released && oldMouseState.RightButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }
    }
}
