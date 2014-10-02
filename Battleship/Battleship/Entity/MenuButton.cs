using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.Entity
{
    public interface TouchListener
    {
        void touchDown(string name);
    }

    public class MenuButton
    {
        private string text;
        private string name;
        private float scale;
        private Rectangle bounds;
        private TouchListener listener;

        public MenuButton(TouchListener listener, string name, string text, float x, float y, float scale = 1)
        {
            this.listener = listener;
            this.text = text;
            this.name = name;
            this.scale = scale;
            this.bounds = new Rectangle((int)x - (int)(Assets.font.MeasureString(text).Length()/2 * scale), (int)y, (int)Assets.font.MeasureString(text).Length(), (int)Assets.font.MeasureString(text).Y);
        }

        public void touchDown(float x, float y)
        {
            if (bounds.Contains((int)x, (int)y))
            {
                listener.touchDown(name);
            }
        }

        public void draw(SpriteBatch batch)
        {
            batch.DrawString(Assets.font, text, new Vector2(bounds.X, bounds.Y), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
    }
}
