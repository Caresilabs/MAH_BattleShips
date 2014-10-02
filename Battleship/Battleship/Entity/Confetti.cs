using Asteroid.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.Entity
{
    public class Confetti
    {
        public const float SIZE = 10;

        private Color color;
        private Vector2 position;
        private Vector2 velocity;
        private Rectangle bounds;

        public Confetti(float x, float y)
        {
            this.position = new Vector2(x, y);
            this.bounds = new Rectangle((int)x, (int)y, (int)SIZE, (int)SIZE);
            this.velocity = new Vector2(MathUtils.random(-300f, 300f), MathUtils.random(200f, 600f));
            this.color = new Color(
                (byte)MathUtils.random(0, 255),
                (byte)MathUtils.random(0, 255),
                (byte)MathUtils.random(0, 255)
            );
        }

        public void update(float delta)
        {
            position += (velocity * delta);
            bounds.X = (int)position.X;
            bounds.Y = (int)position.Y;
        }

        public void draw(SpriteBatch batch)
        {
            batch.Draw(Assets.getItems(), bounds, Assets.getRegion("pixel"), color);
        }

        public Vector2 getPosition()
        {
            return position;
        }

    }
}
