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

            int y = -170;
            float x = 640-fieldWidth;
            // Init fields
            this.shipFieldLeft = new PlayerShipField(this, -640 +x/2, y);

            if (mode == Mode.PlayerVSAI)
            {
                this.shipFieldRight = new AIShipField(this, x/2, y);
            }
            else
            {
                this.shipFieldRight = new PlayerShipField(this, x/2, y);
            }
        }

        public void update(float delta)
        {
            shipFieldLeft.update(delta);
            shipFieldRight.update(delta);
        }

        public void nextTurn()
        {
            if (state == State.Player1Init)
            {
                state = State.Player2Init;
            }
            else if (state == State.Player2Init)
            {
                state = State.Player1Turn;
            }

            // Turn Loop
            else if (state == State.Player1Turn)
            {
                state = State.Player2Turn;
                if (shipFieldRight.hasLost())
                {
                    state = State.Player1Win;
                    gameOver();
                }
            }
            else if (state == State.Player2Turn)
            {
                state = State.Player1Turn;
                if (shipFieldLeft.hasLost())
                {
                    state = State.Player2Win;
                    gameOver();
                }
            }

        }

        public void gameOver() {
            shipFieldLeft.showShips();
            shipFieldRight.showShips();
        }

        public ShipField getFieldLeft()
        {
            return shipFieldLeft;
        }

        public ShipField getFieldRight()
        {
            return shipFieldRight;
        }

        public ShipField getCurrentField()
        {
            if (state == State.Player1Turn || state == State.Player1Init)
            {
                return shipFieldLeft;
            }
            else 
            {
                return shipFieldRight;
            }
        }

        public ShipField getTargetField()
        {
            if (state == State.Player1Turn || state == State.Player1Init)
            {
                return shipFieldRight;
            }
            else
            {
                return shipFieldLeft;
            }
        }

        public State getState()
        {
            return state;
        }

        public Mode getMode()
        {
            return mode;
        }
    }
}
