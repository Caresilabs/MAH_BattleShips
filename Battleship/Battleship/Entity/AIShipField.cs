using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Asteroid.Tools;
using Battleship.Model;
using Microsoft.Xna.Framework;

namespace Battleship.Entity
{
    public class AIShipField : ShipField
    {
        private const int TILE_SUNK = -3;
        private int[,] consumedTiles = new int[World.FIELD_SIZE, World.FIELD_SIZE];

        private float time;
        private Point? lastHit;
        private List<Point> unfinishedShips = new List<Point>();

        public AIShipField(World world, float x, float y)
            : base(world, x, y)
        {
            this.time = 0;
        }

        public void updateAI(float delta, ShipField target)
        {
            time += delta;

            if (getWorld().getState() == World.State.Player2Init)
            {
                if (time > 1.4f)
                {
                    placeShips();
                    nextTurn();
                }
            }
            else if (getWorld().getState() == World.State.Player2Turn)
            {
                if (time > 1.22f)
                {
                    shootRandomTile(target);
                    updateUnfinishedShips(target);
                    nextTurn();
                }
            }
        }

        private void shootRandomTile(ShipField target)
        {
            int maxLoop = 500;
            int loops = 0;
            while (true)
            {
                // todo...hmm look over
                loops++;

                // hunt down til dead
                if (lastHit != null && loops < maxLoop / 1.5f)
                {
                    int x;
                    int y;
                    if (MathUtils.random(100) < 50)
                    {
                        // horizontal
                        x = lastHit.Value.X + (MathUtils.random(-1, 1) * World.TILE_SIZE);
                        y = lastHit.Value.Y;
                    }
                    else
                    {
                        // vertical
                        x = lastHit.Value.X;
                        y = lastHit.Value.Y + (MathUtils.random(-1, 1) * World.TILE_SIZE);
                    }

                    int id = target.getTile(x, y) != null ? target.getTile(x, y).getId() : -10;

                    if (id == -10)
                    {
                        lastHit = null;
                        continue;
                    }

                    if (target.hit(x, y))
                    {
                        // Move hunting target if hit
                        if (target.getTile(x, y).getId() == Tile.TILE_HIT)
                        {
                            lastHit = new Point(x, y);
                            consumedTiles[target.getX(x), target.getY(y)] = id;

                            if (target.getAliveShipParts(id) == 0)
                            {
                                sankBoat(id);
                                lastHit = null;
                                // now check leftovers
                            }
                        }
                        break;
                    }
                }
                else if (unfinishedShips.Count() != 0 && loops < maxLoop / 3 && lastHit == null)
                {
                    lastHit = unfinishedShips[MathUtils.random(unfinishedShips.Count())];
                }
                else // last option... just random shoot
                {
                    // random shoot
                    int x = MathUtils.random(target.getBounds().X, getBounds().Width);
                    int y = MathUtils.random(target.getBounds().Y, getBounds().Height);

                    int id = target.getTile(x, y) != null ? target.getTile(x, y).getId() : -10;

                    if (target.hit(x, y))
                    {
                        if (target.getTile(x, y).getId() == Tile.TILE_HIT)
                        {
                            //consumedTiles[target.getX(x), target.getY(y)] = Tile.TILE_HIT;
                            consumedTiles[target.getX(x), target.getY(y)] = id;
                            lastHit = new Point(x, y);
                        }
                        else if (target.getTile(x, y).getId() == Tile.TILE_WATER)
                        {
                            // consumedTiles[target.getX(x), target.getY(y)] = Tile.TILE_WATER;
                        }
                        break;
                    }
                }

                // safe escape if something terrable happend
                if (loops > maxLoop / 5)
                {
                    lastHit = null;
                }
                if (loops > maxLoop)
                {
                    break;
                }
            }
        }

        private void sankBoat(int id)
        {
            for (int i = 0; i < consumedTiles.GetLength(0); i++)
            {
                for (int j = 0; j < consumedTiles.GetLength(1); j++)
                {
                    if (consumedTiles[j, i] == id)
                        consumedTiles[j, i] = TILE_SUNK;
                }
            }
        }

        private void updateUnfinishedShips(ShipField target)
        {
            unfinishedShips.Clear();

            for (int i = 0; i < consumedTiles.GetLength(0); i++)
            {
                for (int j = 0; j < consumedTiles.GetLength(1); j++)
                {
                    if (consumedTiles[j, i] > 0)
                        unfinishedShips.Add(new Point(target.getBounds().X + j * World.TILE_SIZE, target.getBounds().Y + i * World.TILE_SIZE));
                }
            }
        }

        private void placeShips()
        {
            foreach (var item in getShips())
            {
                Ship ship = item.Value;

                int loops = 0;
                while (loops < 400)
                {
                    loops++;
                    // Arrangement
                    if (MathUtils.randomBool())
                    {
                        rotateShip(ship);
                    }

                    int x = MathUtils.random(getBounds().X + 10, getBounds().X + getBounds().Width - ship.getBounds().Width - 10);
                    int y = MathUtils.random(getBounds().Y + 10, getBounds().Y + getBounds().Height - ship.getBounds().Height - 10);
                    ship.setPosition(x, y);
                    placeShip(ship);

                    // assert that position is ok
                    bool collided = false;
                    foreach (var col in getShips())
                    {
                        if (col.Value.getBounds().Intersects(ship.getBounds()) && col.Value != ship)
                        {
                            collided = true;
                            break;
                        }
                    }

                    // if collided try again
                    if (!collided)
                    {
                        break;
                    }
                }
            }
            attachShips();
        }

        private void nextTurn()
        {
            getWorld().nextTurn();
            time = 0;
        }
    }
}
