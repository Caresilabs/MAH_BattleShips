using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleship.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleship.Entity
{
    public class ShipField
    {
        private int[] defaultShipsSize = new int[] {
            5, 4, 3, 3, 2
        };

        private int[,] tiles = new int[World.fieldSize, World.fieldSize];

        private Dictionary<int, Ship> ships;

        private bool hidden;

        private Vector2 position;

        public ShipField(float x, float y)
        {
            this.ships = new Dictionary<int, Ship>();
            this.hidden = false;
            this.position = new Vector2(x, y);

            initFields();
            addDefaultShips();
        }

        private void initFields()
        {
            // LEFT Field
            for (int i = 0; i < World.fieldSize; i++)
            {
                for (int j = 0; j < World.fieldSize; j++)
                {
                    tiles[j, i] = -1;
                }
            }
        }

        private void addDefaultShips()
        {
            for (int i = 0; i < defaultShipsSize.Length; i++)
            {
                ships.Add(i, new Ship(20, i * 20, defaultShipsSize[i]));
            }
        }

        public void draw(SpriteBatch batch)
        {
            if (hidden) return;

            for (int i = 0; i < World.fieldSize; i++)
            {
                for (int j = 0; j < World.fieldSize; j++)
                {
                    batch.Draw(Assets.getItems(), new Rectangle((int)(position.X + j * World.tileSize), (int)(position.Y + i * World.tileSize), World.tileSize, World.tileSize)
                        , Assets.getRegion("tile"), Color.White);
                    //Console.WriteLine(world.getTilesRight()[j, i]);
                    // world.getTilesLeft[j, i] = -1;
                }
            }
        }

        public int getX(float x)
        {
            return (int)(((x - position.X) / World.fieldWidth) * World.fieldSize); 
        }

        public int getY(float y)
        {
            return (int)(((y - position.Y) / World.fieldHeight) * World.fieldSize);
        }

        public Ship getShip(int id)
        {
            return ships[id];
        }

    }
}
