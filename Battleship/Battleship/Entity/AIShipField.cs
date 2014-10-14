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

        private Point? lastHit;
        private List<Point> unfinishedShips = new List<Point>();

        private float time;
        private bool isShooting;

        public AIShipField(World world, float x, float y)
            : base(world, x, y)
        {
            this.time = 0;
            this.isShooting = false;
        }

        public void updateAI(float delta, ShipField target)
        {
            time += delta;

            if (getWorld().getState() == World.State.Player2Init || getWorld().getState() == World.State.Player1Init)
            {
                if (time > 2f)
                {
                    placeShips();
                    nextTurn();
                }
            }
            else if (getWorld().getState() == World.State.Player2Turn || getWorld().getState() == World.State.Player1Turn)
            {
                if (time > 1.0f)
                {
                    if (!isShooting)
                    {
                        shootRandomTile(target);
                        updateUnfinishedShips(target);
                        isShooting = true;
                    }

                    if (updateShoots(delta, target))
                    {
                        nextTurn();
                        isShooting = false;
                    }
                }
            }
        }

        private void shootRandomTile(ShipField target)
        {
            int maxLoop = 500;
            int loops = 0;

            // random attack
            if (MathUtils.random(0, 10) >= 9)
            {
                selectAttack(MathUtils.random(0, getAttacks().Count - 1));
            }

            while (true)
            {
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

                    if (shoot(target, x, y))
                    {
                        foreach (var item in getShootManager().getBombPositions())
                        {
                            // Move hunting target if hit
                            if (target.getTile(item.X, item.Y).getId() > 0)
                            {
                                lastHit = new Point(item.X, item.Y);
                                consumedTiles[target.getX(item.X), target.getY(item.Y)] = id;

                                if (target.getAliveShipParts(target.getTile(item.X, item.Y).getId()) <= 1)
                                {
                                    sankBoat(id);
                                    lastHit = null;
                                }
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
                    int x = MathUtils.random(target.getBounds().X, target.getBounds().Width);
                    int y = MathUtils.random(target.getBounds().Y, target.getBounds().Height);

                    int id = target.getTile(x, y) != null ? target.getTile(x, y).getId() : -10;

                    if (shoot(target, x, y))
                    {
                        foreach (var item in getShootManager().getBombPositions())
                        {
                            if (target.getTile(item.X, item.Y).getId() > 0)
                            {
                                consumedTiles[target.getX(item.X), target.getY(item.Y)] = id;
                                lastHit = new Point(item.X, item.Y);

                                if (target.getAliveShipParts(target.getTile(item.X, item.Y).getId()) <= 1)
                                {
                                    sankBoat(id);
                                    lastHit = null;
                                }
                            }
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
