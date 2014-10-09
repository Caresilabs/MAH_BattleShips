using Asteroid.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.Entity
{
    public class Particle
    {
        public const float SIZE_DEFAULT = 10;

        private Color color;
        private Vector2 position;
        private Vector2 velocity;
        private Rectangle bounds;

        private float size;
        private float lifeTime;
        private float life;
        private bool alive;

        public Particle(float x, float y, float vx, float vy, float size = SIZE_DEFAULT, Color? color = null)
        {
            this.position = new Vector2(x, y);
            this.bounds = new Rectangle((int)x, (int)y, (int)size, (int)size);
            this.velocity = new Vector2(vx, vy);
            if (color == null)
            {
                this.color = new Color(
                    (byte)MathUtils.random(0, 255),
                    (byte)MathUtils.random(0, 255),
                    (byte)MathUtils.random(0, 255)
                );
            }
            else
            {
               this.color = (Color)color;
            }

            this.life = 0;
            this.alive = true;
            this.lifeTime = 10;
        }

        public void update(float delta)
        {
            position += (velocity * delta);
            bounds.X = (int)position.X;
            bounds.Y = (int)position.Y;

            life += delta;
            if (life > lifeTime)
            {
                alive = false;
            }
        }

        public void draw(SpriteBatch batch)
        {
            batch.Draw(Assets.getItems(), bounds, Assets.getRegion("pixel"), color);
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public void setLife(float life)
        {
            this.lifeTime = life;
        }

        public bool isAlive()
        {
            return alive;
        }

    }
}
