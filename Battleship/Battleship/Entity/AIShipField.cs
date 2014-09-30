using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleship.Model;

namespace Battleship.Entity
{
    public class AIShipField : ShipField
    {
        public AIShipField(World world, float x, float y)
            : base(world, x, y)
        {
        } 

        public void updateAI(float delta, ShipField target)
        {
            if (getWorld().getState() == World.State.Player2Init)
            {

            }
            else if (getWorld().getState() == World.State.Player2Turn)
            {

            }
        }
    }
}
