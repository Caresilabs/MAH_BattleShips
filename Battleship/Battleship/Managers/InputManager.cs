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
        }

        private Ship currentShip;
        public void update(float delta)
        {
            mouse.X = Mouse.GetState().X;
            mouse.Y = Mouse.GetState().Y;

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
               currentShip = world.getFieldLeft().getShipByMouse(mouse.X, mouse.Y);
                if (currentShip != null)
               Console.WriteLine("yaa" );
            }
            else if (Mouse.GetState().LeftButton == ButtonState.Released)
            {

            }

            if (currentShip != null)
            {
                Vector2 pos = Camera2D.unproject(mouse.X, mouse.Y);
                pos.X -= currentShip.getX();
                pos.Y -= currentShip.getY();

                world.getFieldLeft().move(currentShip, mouse.X - pos.X, mouse.Y - pos.Y);
            }
        }
    }
}
