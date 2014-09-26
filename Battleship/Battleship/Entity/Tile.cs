using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.Entity
{
    public class Tile : Entity
    {
        public Tile() : base(Assets.getRegion("tile"), 0, 0, 1, 1)
        {

        }
    }
}
