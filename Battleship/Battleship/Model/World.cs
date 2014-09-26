using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Asteroid.Tools;
using Battleship.Entity;
using Microsoft.Xna.Framework.Input;
using Battleship.Tools;

namespace Battleship.Model
{
    public class World
    {
        // World size
        public const float fieldWidth = 500;
        public const float fieldHeight = 500;

        public const int fieldSize = 10;

        public static int tileSize;

        private State state;
        private Mode mode;

        private ShipField shipFieldLeft;
        private ShipField shipFieldRight;

        public enum State
        {
            Player1Init, Player2Init, Player1Turn, Player2Turn, Player1Win, Player2Win
        }

        public enum Mode
        {
            PlayerVSAI, PlayerVSPlayer
        }

        public World(Mode mode)
        {
            this.mode = mode;
            this.state = State.Player1Init;

            tileSize = (int)fieldWidth / fieldSize;

            int y = -250;
            // Init fields
            this.shipFieldLeft = new PlayerShipField(-600, y);

            if (mode == Mode.PlayerVSAI)
            {
                this.shipFieldRight = new AIShipField(0, y);
            }
            else
            {
                this.shipFieldRight = new PlayerShipField(0, y);
            }

        }

        public void update(float delta)
        {
            shipFieldLeft.update(delta);
            shipFieldLeft.update(delta);
            //Console.WriteLine(shipFieldLeft.getX(Camera2D.unproject(Mouse.GetState().X, Mouse.GetState().Y).X));
        }

        public ShipField getFieldLeft()
        {
            return shipFieldLeft;
        }

        public ShipField getFieldRight()
        {
            return shipFieldRight;
        }

        public State getState()
        {
            return state;
        }
    }
}
