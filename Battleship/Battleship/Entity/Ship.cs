using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleship.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleship.Entity
{
    public class Ship : Entity
    {
        private int size;
        private bool isHorizontal;
        private bool isVisible;
        private bool isAlive;
        private string name;

        private Vector2 grabPosition;

        public Ship(string name, float x, float y, int size)
            : base(Assets.getRegion("ship" + size), x, y, size * World.TILE_SIZE, World.TILE_SIZE)
        {
            this.name = name;
            this.size = size;
            this.isHorizontal = true;
            this.isVisible = true;
            this.isAlive = true;
            this.grabPosition = new Vector2();
            this.setZIndex(.05f);
        }

        public override void draw(SpriteBatch batch)
        {
            if (isVisible)
                base.draw(batch);
        }

        public void flip()
        {
            if (isHorizontal)
            {
                addRotation((float)Math.PI / 2 + (float)Math.PI);
                setDrawOffset(0, World.TILE_SIZE * size);
            }
            else
            {
                setDrawOffset(0,0);
                addRotation(-((float)Math.PI / 2 + (float)Math.PI));
            }

            isHorizontal = !isHorizontal;
            updateBounds();
        }

        public override Rectangle getBounds()
        {
            if (isHorizontal)
            {
                return base.getBounds();
            }
            else
            {
                return new Rectangle(base.getBounds().X, base.getBounds().Y, World.TILE_SIZE, size * World.TILE_SIZE);
            }
        }

        public void setVisible(bool visible)
        {
            if (isAlive)
                isVisible = visible;
        }

        public int getSize()
        {
            return size;
        }

        public void kill()
        {
            isAlive = false;
            isVisible = true;
        }

        public void grab()
        {
            grabPosition.X = getX();
            grabPosition.Y = getY();
        }

        public void revertGrab()
        {
            setPosition(grabPosition);
        }

        public bool isShipAlive()
        {
            return isAlive;
        }

        public string getName()
        {
            return name;
        }
    }
}
