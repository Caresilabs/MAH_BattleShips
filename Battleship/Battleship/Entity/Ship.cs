using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleship.Model;
using Microsoft.Xna.Framework;

namespace Battleship.Entity
{
    public class Ship : Entity
    {
        private int size;
        private bool isHorizontal;

        public Ship(float x, float y, int size)
            : base(Assets.getRegion("ship" + size), x, y, size * World.tileSize, World.tileSize)
        {
            this.size = size;
            this.isHorizontal = true;
        }

        public void flip()
        {
            if (isHorizontal)
            {
                setSize(1 * World.tileSize, size * World.tileSize);
            }
            else
            {
                setSize(size * World.tileSize, 1 * World.tileSize);
            }

            isHorizontal = !isHorizontal;
        }
    }
}
