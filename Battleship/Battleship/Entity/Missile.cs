using Asteroid.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.Entity
{
    public class Missile
    {
        public const float SIZE = 62;
        public const float RADIUS_DEFAULT = 500;

        private Vector2 position;
        private Vector2 target;
        private Rectangle bounds;

        private float radius;
        private float angle;
        private bool isAlive;

        public Missile(float x, float y)
        {
            this.target = new Vector2(x, y);
            this.position = new Vector2(x - RADIUS_DEFAULT, y);
            this.radius = RADIUS_DEFAULT;
            this.isAlive = true;
            this.bounds = new Rectangle((int)position.X, (int)position.Y, (int)SIZE, (int)SIZE);
            this.angle = MathUtils.random(0, (float)Math.PI);
        }

        public void update(float delta)
        {
            //position += (velocity * delta);
            position.X = (float)(target.X + Math.Cos(angle) * radius);
            position.Y = (float)(target.Y + Math.Sin(angle) * radius);

            angle += delta * 14;
            radius -= delta * 300;

            bounds.X = (int)position.X;
            bounds.Y = (int)position.Y;

            if (bounds.Contains((int)target.X, (int)target.Y))
            {
                isAlive = false;
            }
        }

        public void draw(SpriteBatch batch)
        {
            batch.Draw(Assets.getItems(), bounds, Assets.getRegion("tileBomb"), Color.White, (float)(angle + Math.PI*2), new Vector2(SIZE / 2, SIZE / 2), SpriteEffects.None, 0);
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public bool isMissileAlive()
        {
            return isAlive;
        }

    }
}
