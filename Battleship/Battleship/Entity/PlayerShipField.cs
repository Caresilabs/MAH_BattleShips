using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleship.Model;

namespace Battleship.Entity
{
    public class PlayerShipField : ShipField
    {
        public PlayerShipField(World world, float x, float y)
            : base(world, x, y)
        {
            // Do player specific stuff in future release
        } 
    }
}
