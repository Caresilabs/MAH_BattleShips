using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Asteroid.Tools;
using Battleship.Entity;

namespace Battleship.Model
{
    public class World
    {
        // World size
        public const float fieldWidth = 600;
        public const float fieldHeight = 600;

        public const int fieldSize = 10;

        public static int tileSize;

        private int[,] tilesLeft = new int[fieldSize, fieldSize];
        private int[,] tilesRight = new int[fieldSize, fieldSize];

        private Dictionary<int, Ship> ships;

        public World()
        {
            initFields();
            tileSize = (int)fieldWidth / fieldSize;
            this.ships = new Dictionary<int, Ship>();
           
        }

        private void initFields()
        {
            // LEFT Field
            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    tilesLeft[j, i] = -1;
                    tilesRight[j, i] = -1;
                }
            }
        }

        public void update(float delta)
        {

        }

        public int[,] getTilesLeft()
        {
            return tilesLeft;
        }

        public int[,] getTilesRight()
        {
            return tilesRight;
        }

        public Dictionary<int, Ship> getShips()
        {
            return ships;
        }
    }
}
