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
        private Rectangle bounds;

        private bool hidden;
        private float rotation;
        private float width;
        private float height;

        public Entity(Rectangle region, float x, float y, float width, float height)
        {
            this.hidden = false;
            this.rotation = 0;
            this.position = new Vector2(x, y);
            this.bounds = new Rectangle((int)x, (int)y, (int)width, (int)height);
            this.width = width;
            this.height = height;
            this.region = region;
        }

        public virtual void update(float delta)
        {
            updateBounds();
        }

        private void updateBounds()
        {
            // Position
            this.bounds.X = (int)position.X; //- (int)width/2;
            this.bounds.Y = (int)position.Y;// - (int)height/2;
            // Width
            this.bounds.Width = (int)width;
            this.bounds.Height = (int)height;
        }

        public void draw(SpriteBatch batch)
        {
            batch.Draw(Assets.getItems(), bounds, region, Color.White, rotation, Vector2.Zero, SpriteEffects.None, 0);
        }

        public void setPosition(float x, float y)
        {
            this.position.X = x;
            this.position.Y = y;
        }

        public void setSize(float width, float height)
        {
            this.width = width;
            this.height = height;
        }

        public Rectangle getBounds()
        {
            return bounds;
        }
    }
}
