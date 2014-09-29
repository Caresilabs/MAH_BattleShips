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

        private Tile[,] tiles = new Tile[World.fieldSize, World.fieldSize];
        private Dictionary<int, Ship> ships;
        private Vector2 position;
        private Rectangle bounds;

        private bool isAlive;

        public ShipField(float x, float y)
        {
            this.ships = new Dictionary<int, Ship>();
            this.position = new Vector2(x, y);
            this.bounds = new Rectangle((int)x, (int)y, World.fieldSize * World.tileSize, World.fieldSize * World.tileSize);
            this.isAlive = true;

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
                    tiles[j, i] = new Tile(j, i);
                }
            }
        }

        private void addDefaultShips()
        {
            for (int i = 0; i < defaultShipsSize.Length; i++)
            {
                ships.Add(i + 1, new Ship(position.X, position.Y + i * (World.tileSize + 1), defaultShipsSize[i]));
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
            for (int i = 0; i < World.fieldSize; i++)
            {
                for (int j = 0; j < World.fieldSize; j++)
                {
                    tiles[j, i].draw(batch, position);
                }
            }

            // Draw ships
            foreach (var item in ships)
            {
                item.Value.draw(batch);
            }
        }

        // Return true if hit was done correctly
        public bool hit(float x, float y)
        {
            Tile tile = getTile(x, y);
            if (tile != null)
            {
                int tileId = tile.getId();
                if (tile.hit())
                {
                    // Check if all parts is down
                    if (getAliveShipParts(tileId) == 0)
                    {
                        ships[tileId].kill();
                        checkIfDead();
                    }
                    return true;
                }
            }
            return false;
        }

        private void checkIfDead()
        {
            foreach (var item in ships)
            {
                if (item.Value.isShipAlive()) {
                    return;
                }
            }
            isAlive = false;
        }

        public void placeShip(Ship ship)
        {
            ship.setPosition(
                (getX(ship.getX() + World.tileSize / 2) * World.tileSize) + position.X, // X
                (getY(ship.getY() + World.tileSize / 2) * World.tileSize) + position.Y); //Y

            if (!bounds.Contains(ship.getBounds()))
            {
                // Check X
                if (ship.getX() < position.X)
                {
                    ship.setPosition(position.X, ship.getY());
                }
                else if (ship.getX() + ship.getBounds().Width > bounds.Right)
                {
                    ship.setPosition(bounds.Right - ship.getBounds().Width, ship.getY());
                }

                // Check Y
                if (ship.getY() < position.Y)
                {
                    ship.setPosition(ship.getX(), position.Y );
                }
                else if (ship.getY() + ship.getBounds().Height > bounds.Bottom)
                {
                    ship.setPosition(ship.getX(), bounds.Bottom - ship.getBounds().Height);
                }
            }

            ship.updateBounds();

            // check other ships
            foreach (var item in ships)
            {
                if (item.Value.getBounds().Intersects(ship.getBounds()))
                {
                    if (item.Value == ship) continue;

                    ship.revertGrab();
                }
            }

        }

        public Ship getShipByMouse(float x, float y)
        {
            Point point = new Point((int)x, (int)y);

            foreach (var item in ships)
            {
                if (item.Value.getBounds().Contains(point))
                {
                    item.Value.grab();
                    return item.Value;
                }
            }
            return null;
        }

        // Heavy on CPU, call only once per game
        public void attachShips()
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    foreach (var item in ships)
                    {
                        if (item.Value.getBounds().Contains
                            ((int)(position.X + j * World.tileSize + World.tileSize / 2), 
                            (int)(position.Y + i * World.tileSize + World.tileSize / 2)))
                        {
                            tiles[j, i].setId(item.Key);
                            break;
                        }
                    }
                }
            }
        }

        public void move(Ship ship, float x, float y)
        {
            if (ship == null) return;

            ship.setPosition(x, y);
        }

        public void hover(float x, float y, Tile.TileEffect effect = Tile.TileEffect.Selected)
        {
            if(! bounds.Contains(new Point((int)x, (int)y))) return;

            int u = getX(x);
            int v = getY(y);

            tiles[u, v].setTileEffect(effect);
        }

        public Tile getTile(float x, float y)
        {
            if (!bounds.Contains(new Point((int)x, (int)y))) return null;

            int u = getX(x);
            int v = getY(y);

            return tiles[u, v];
        }

        private void clearHover()
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    tiles[j, i].setTileEffect(Tile.TileEffect.None);
                }
            }
        }

        public int getAliveShipParts(int id)
        {
            if (id <= 0) return -1;

            int parts = 0;
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if (tiles[j, i].getId() == id)
                    {
                        parts++;
                    }
                }
            }
            return parts;
        }

        public void hideShips()
        {
            foreach (var item in ships)
            {
                item.Value.setVisible(false);
            }
        }

        public void showShips()
        {
            foreach (var item in ships)
            {
                item.Value.setVisible(true);
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

        public void rotateShip(Ship ship)
        {
            ship.flip();
        }

        public Rectangle getBounds()
        {
            return bounds;
        }

        public bool hasLost()
        {
            return !isAlive;
        }
    }
}
