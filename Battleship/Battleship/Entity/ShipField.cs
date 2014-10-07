using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleship.Model;
using Battleship.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battleship.View;

namespace Battleship.Entity
{
    public class ShipField
    {
        private static readonly string[] Columns = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH" };
        public static string IndexToColumn(int index)
        {
            if (index <= 0)
                throw new IndexOutOfRangeException("Index must be positive");

            return Columns[index - 1];
        }

        private Dictionary<string, int> battleShips = new Dictionary<string, int>() 
        { 
            { "Aircraft carrier", 5 },
            { "Battleship", 4 },
            { "Submarine", 3 },
            { "Destroyer", 3 },
            { "Patrol boat", 2 }
        };

        private Dictionary<string, bool> attacks = new Dictionary<string, bool>() 
        { 
            { "Normal Strike", true },
            { "Horizontal Strike", true },
            { "Vertical Strike", true },
            { "Circle Strike", true }
        };

        private Tile[,] tiles = new Tile[World.FIELD_SIZE, World.FIELD_SIZE];
        private Dictionary<int, Ship> ships;
        private Vector2 position;
        private Rectangle bounds;
        private World world;

        private bool isAlive;
        private int hits;
        private string selectedAttack;

        public ShipField(World world, float x, float y)
        {
            this.world = world;
            this.ships = new Dictionary<int, Ship>();
            this.position = new Vector2(x, y);
            this.bounds = new Rectangle((int)x, (int)y, World.FIELD_SIZE * World.TILE_SIZE, World.FIELD_SIZE * World.TILE_SIZE);
            this.isAlive = true;
            this.selectedAttack = attacks.Keys.ToArray()[0];

            initFields();
            addDefaultShips();
        }

        private void initFields()
        {
            // LEFT Field
            for (int i = 0; i < World.FIELD_SIZE; i++)
            {
                for (int j = 0; j < World.FIELD_SIZE; j++)
                {
                    tiles[j, i] = new Tile(j, i);
                }
            }
        }

        private void addDefaultShips()
        {
            for (int i = 0; i < battleShips.Count; i++)
            {
                Ship ship = new Ship(battleShips.Keys.ToArray()[i], position.X, position.Y + i * (World.TILE_SIZE + 1),
                    battleShips.Values.ToArray()[i]);
                placeShip(ship);
                ships.Add(i + 1, ship);
            }
        }

        public virtual void update(float delta)
        {
            clearHover();

            foreach (var item in ships)
            {
                item.Value.update(delta);
            }
        }

        public void draw(SpriteBatch batch)
        {
            for (int i = 0; i < World.FIELD_SIZE; i++)
            {
                // Draw Columns and Rows
                Vector2 pos = new Vector2(position.X + i * World.TILE_SIZE + World.TILE_SIZE / 3, position.Y - 25);
                batch.DrawString(Assets.font, IndexToColumn(i + 1), pos, Color.White, 0, Vector2.Zero, .5f, SpriteEffects.None, 0);

                pos = new Vector2(position.X - 25, position.Y + i * World.TILE_SIZE + World.TILE_SIZE / 3);
                batch.DrawString(Assets.font, (i + 1).ToString(), pos, Color.White, 0, Vector2.Zero, .5f, SpriteEffects.None, 0);

                // Draw tiles
                for (int j = 0; j < World.FIELD_SIZE; j++)
                {
                    tiles[j, i].draw(batch, position);
                }
            }

            // Draw ships
            foreach (var item in ships)
            {
                item.Value.draw(batch);
            }

            // Draw shoots
            Vector2 textPos = new Vector2(position.X + bounds.Width/2 - 20, position.Y - 50);
            batch.DrawString(Assets.font, "Hits: " + hits, textPos, Color.Red, 0, Vector2.Zero, .5f, SpriteEffects.None, 0);


            // Draw attacks left
            string attackString = "Attacks left: ";
            foreach (var item in attacks)
            {
                if (item.Value == false) continue;
                attackString += " " + item.Key + ",";
            }
            attackString = attackString.Substring(0, attackString.Length - 1);

            textPos = new Vector2(position.X + 10, position.Y + bounds.Height + 2);
            batch.DrawString(Assets.font, attackString, textPos, Color.White, 0, Vector2.Zero, .39f, SpriteEffects.None, 0);
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
                    hits++;

                    // Check if all parts is down
                    if (getAliveShipParts(tileId) == 0)
                    {
                        ships[tileId].kill();
                        if (checkIfAlive())
                        {
                            HUD.setDialogText("You sunk my " + ships[tileId].getName());
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        private bool checkIfAlive()
        {
            foreach (var item in ships)
            {
                if (item.Value.isShipAlive())
                {
                    return true;
                }
            }
            isAlive = false;
            return false;
        }

        public void placeShip(Ship ship)
        {
            ship.setPosition(
                (getX(ship.getX() + World.TILE_SIZE / 2) * World.TILE_SIZE) + position.X, // X
                (getY(ship.getY() + World.TILE_SIZE / 2) * World.TILE_SIZE) + position.Y); //Y

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
                    ship.setPosition(ship.getX(), position.Y);
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
                            ((int)(position.X + j * World.TILE_SIZE + World.TILE_SIZE / 2),
                            (int)(position.Y + i * World.TILE_SIZE + World.TILE_SIZE / 2)))
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
            if (!bounds.Contains(new Point((int)x, (int)y))) return;

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

        public void consumeAttack()
        {
            // Consume attack, if not normal attack
            if (selectedAttack != "Normal Strike")
            {
                attacks[selectedAttack] = false;
                selectedAttack = "Normal Strike";
            }
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
            return (int)(((x - position.X) / World.FIELD_WIDTH) * World.FIELD_SIZE);
        }

        public int getY(float y)
        {
            return (int)(((y - position.Y) / World.FIELD_HEIGHT) * World.FIELD_SIZE);
        }

        public Ship getShip(int id)
        {
            return ships[id];
        }

        public void rotateShip(Ship ship)
        {
            ship.flip();
        }

        public void selectAttack(int id)
        {
            if (attacks.Values.ToArray()[id] == true)
            {
                this.selectedAttack = attacks.Keys.ToArray()[id];
            }
        }

        public string getSelectedAttack()
        {
            return selectedAttack;
        }

        public Rectangle getBounds()
        {
            return bounds;
        }

        public bool hasLost()
        {
            return !isAlive;
        }

        public World getWorld()
        {
            return world;
        }

        public Dictionary<int, Ship> getShips()
        {
            return ships;
        }

        public Dictionary<string, bool> getAttacks()
        {
            return attacks;
        }
    }
}
