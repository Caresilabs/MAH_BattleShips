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
        public Ship(float x, float y, int size)
            : base(Assets.getRegion("ship"), x, y, size * World.tileSize, World.tileSize)
        {
            this.size = size;
        }

        public void flip()
        {
            setSize(1, size);
        }
    }
}
