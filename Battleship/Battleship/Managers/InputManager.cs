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

        public InputManager(World world)
        {
            this.world = world;
            this.mouse = new Vector2();
            this.origin = new Vector2();
        }

        private Ship currentShip;
        private Vector2 origin;
        public void update(float delta)
        {
            mouse.X = Mouse.GetState().X;
            mouse.Y = Mouse.GetState().Y;
            Vector2 mouseWorld = Camera2D.unproject(mouse.X, mouse.Y);

            // Catch ship
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && currentShip == null)
            {
               currentShip = world.getFieldLeft().getShipByMouse(mouse.X, mouse.Y);
               if (currentShip != null)
               {
                   
                   origin.X = mouseWorld.X - currentShip.getX();
                   origin.Y = mouseWorld.Y - currentShip.getY();
               }
            }
            else if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                world.getFieldLeft().hover(mouseWorld.X, mouseWorld.Y);
            }

            // manage ship
            if (currentShip != null)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    world.getFieldLeft().move(currentShip, mouseWorld.X - origin.X, mouseWorld.Y - origin.Y);
                }
                else
                {
                   world.getFieldLeft().placeShip(currentShip);
                   currentShip = null;
                }


                /// Check rotation
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    world.getFieldLeft().rotateShip(currentShip);
                }
            }
           
        }
    }
}
