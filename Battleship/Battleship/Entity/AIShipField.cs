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
        List<Point> unfinishedShips = new List<Point>();

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
                if (time > 1.6f)
                {
                    placeShips();
                    nextTurn();
                }
            }
            else if (getWorld().getState() == World.State.Player2Turn)
            {
                if (time > .1f)
                {

                    shootRandomTile(target);

                    nextTurn();
                }
            }
        }

        private void shootRandomTile(ShipField target)
        {
            int maxLoop = 100;
            int loops = 0;
            while (true)
            {
                // todo...hmm look over
                loops++;

                // hunt down til dead
                if (lastHit != null && loops < maxLoop)
                {
                    int x = lastHit.Value.X + MathUtils.random(-1, 1) * World.TILE_SIZE;
                    int y = lastHit.Value.Y + MathUtils.random(-1, 1) * World.TILE_SIZE;

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

                            if (target.getAliveShipParts(id) == 0)
                            {
                                sankBoat(id);
                                updateUnfinishedShips();
                                lastHit = null;
                                // now check leftovers
                            }
                        }
                        break;
                    }
                }
                else // last option... just random shoot
                {
                    // random shoot
                    int x = MathUtils.random(target.getBounds().X, getBounds().Width);
                    int y = MathUtils.random(target.getBounds().Y, getBounds().Height);

                    if (target.hit(x, y))
                    {
                        if (target.getTile(x, y).getId() == Tile.TILE_HIT)
                        {
                            consumedTiles[target.getX(x), target.getY(y)] = Tile.TILE_HIT;
                            lastHit = new Point(x, y);
                        }
                        else if (target.getTile(x, y).getId() == Tile.TILE_WATER)
                        {
                            consumedTiles[target.getX(x), target.getY(y)] = Tile.TILE_WATER;
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

        private void updateUnfinishedShips()
        {
            unfinishedShips.Clear();

            for (int i = 0; i < consumedTiles.GetLength(0); i++)
            {
                for (int j = 0; j < consumedTiles.GetLength(1); j++)
                {
                    if (consumedTiles[j, i] == Tile.TILE_HIT)
                        unfinishedShips.Add(new Point(j, i));
                }
            }
        }

        private void placeShips()
        {
            attachShips();
        }

        private void nextTurn()
        {
            getWorld().nextTurn();
            time = 0;
        }
    }
}
