using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleship.Model;
using Battleship.Tools;
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

        private Rectangle bounds;

        public ShipField(float x, float y)
        {
            this.ships = new Dictionary<int, Ship>();
            this.hidden = false;
            this.position = new Vector2(x, y);
            this.bounds = new Rectangle((int)x, (int)y, World.fieldSize * World.tileSize, World.fieldSize * World.tileSize);

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
                ships.Add(i, new Ship(position.X, position.Y + i * (World.tileSize + 1), defaultShipsSize[i]));
            }
        }

        public void update(float delta)
        {
            clearHover();

            foreach (var item in ships)
            {
                item.Value.update(delta);
            }
        }

        public void draw(SpriteBatch batch)
        {
            if (hidden) return;

            for (int i = 0; i < World.fieldSize; i++)
            {
                for (int j = 0; j < World.fieldSize; j++)
                {
                    Color color = Color.Green;
                    if (tiles[j, i] == -2)  {
                        color = Color.Red;
                    }
                    batch.Draw(Assets.getItems(), new Rectangle((int)(position.X + j * World.tileSize), (int)(position.Y + i * World.tileSize), World.tileSize, World.tileSize)
                        , Assets.getRegion("tile"), color, 0, Vector2.Zero, SpriteEffects.None, .1f);
                }
            }

            foreach (var item in ships)
            {
                item.Value.draw(batch);
            }
        }

        public void placeShip(Ship ship)
        {
            ship.setPosition(
                (getX(ship.getX() + World.tileSize / 2) * World.tileSize) + position.X, // X
                (getY(ship.getY() + World.tileSize / 2) * World.tileSize) + position.Y); //Y

            if (!bounds.Contains(ship.getBounds()))
            {
                placeInside();
                return;
            }

            // check other ships
            foreach (var item in ships)
            {
                if (item.Value.getBounds().Intersects(ship.getBounds()))
                {
                    if (item.Value == ship) continue;

                    placeInside();
                }
            }

        }

        private void placeInside(Ship ship)
        {

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

        public Ship getShipByMouse(float x, float y)
        {
            Vector2 projectedPosition = Camera2D.unproject(x, y);
            Point point = new Point((int)projectedPosition.X, (int)projectedPosition.Y);

            foreach (var item in ships)
            {
                if (item.Value.getBounds().Contains(point))
                {
                    return item.Value;
                }
            }
            return null;
        }


        public void move(Ship ship, float x, float y)
        {
            if (ship == null) return;

            ship.setPosition(x, y);
        }

        public void hover(float x, float y)
        {
            if(! bounds.Contains(new Point((int)x, (int)y))) return;

            int u = getX(x);
            int v = getY(y);

            tiles[u, v] = -2;
        }

        private void clearHover()
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if (tiles[j, i] == -2)
                    {
                        tiles[j, i] = -1;
                    }
                }
            }
        }

        public void rotateShip(Ship ship)
        {
            ship.flip();
        }

        public Rectangle getBounds()
        {
            return bounds;
        }

    }
}
