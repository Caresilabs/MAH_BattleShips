using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleship.Entity
{
    public abstract class Entity
    {
        private Rectangle region;
        private Vector2 position;
        private Vector2 drawOffset;
        private Rectangle bounds;
        private Rectangle drawRectangle;

        private bool hidden;
        private float rotation;
        private float width;
        private float height;
        private float zIndex;

        public Entity(Rectangle region, float x, float y, float width, float height)
        {
            this.hidden = false;
            this.rotation = 0;
            this.position = new Vector2(x, y);
            this.bounds = new Rectangle((int)x, (int)y, (int)width, (int)height);
            this.drawOffset = new Vector2();
            this.drawRectangle = new Rectangle();
            this.width = width;
            this.height = height;
            this.region = region;
            this.zIndex = 0;
        }

        public virtual void update(float delta)
        {
            updateBounds();
        }

        public void updateBounds()
        {
            // Position
            this.bounds.X = (int)position.X; 
            this.bounds.Y = (int)position.Y;

            // Width
            this.bounds.Width = (int)width;
            this.bounds.Height = (int)height;
        }

        public virtual void draw(SpriteBatch batch)
        {
            if (hidden) return;

            // position
            this.drawRectangle.X = (int)(position.X + drawOffset.X);
            this.drawRectangle.Y = (int)(position.Y + drawOffset.Y);
            // Width
            this.drawRectangle.Width = (int)width;
            this.drawRectangle.Height = (int)height;

            batch.Draw(Assets.getItems(), drawRectangle, region, Color.White, rotation, Vector2.Zero, SpriteEffects.None, zIndex);
        }

        public void setPosition(float x, float y)
        {
            this.position.X = x;
            this.position.Y = y;
        }

        public void setPosition(Vector2 position)
        {
            this.position.X = position.X;
            this.position.Y = position.Y;
        }

        public void setSize(float width, float height)
        {
            this.width = width;
            this.height = height;
        }

        public void setDrawOffset(float x, float y)
        {
            this.drawOffset.X = x;
            this.drawOffset.Y = y;
        }

        public void addRotation(float radians)
        {
            this.rotation += radians;
        }

        public void setZIndex(float z)
        {
            this.zIndex = z;
        }

        public virtual Rectangle getBounds()
        {
            return bounds;
        }

        public float getX()
        {
            return position.X;
        }

        public float getY()
        {
            return position.Y;
        }
    }
}
