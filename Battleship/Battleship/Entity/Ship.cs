﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Battleship.Entity
{
    public class Ship : Entity
    {
        private int size;
        public Ship(float x, float y, float size) : base(Assets.getRegion("tile"), 0, 0, size, 1)
        {
            this.size = size;
        }

    }
}
