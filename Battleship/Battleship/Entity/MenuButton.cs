using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleship.Tools;

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
        private Rectangle backgrounds;
        private TouchListener listener;
        private Vector2 startPos;

        public MenuButton(TouchListener listener, string name, string text, float x, float y, float scale = 1)
        {
            this.listener = listener;
            this.text = text;
            this.name = name;
            this.scale = scale;
            this.startPos = new Vector2(x, y);
            this.bounds = new Rectangle((int)x - (int)(Assets.font.MeasureString(text).Length()/2 * scale), (int)y, (int)(Assets.font.MeasureString(text).Length()*scale), (int)Assets.font.MeasureString(text).Y);
            this.backgrounds = new Rectangle(bounds.X - 20, bounds.Y - 5, bounds.Width + 30, bounds.Height+ 30);
        }

        public void touchDown(float x, float y)
        {
            Vector2 un = Camera2D.unproject(x, y);
            if (bounds.Contains((int)un.X, (int)un.Y))
            {
                listener.touchDown(name);
            }
        }

        public void draw(SpriteBatch batch)
        {
            batch.Draw(Assets.ui, backgrounds, Assets.getRegion("button"), Color.White, 0, Vector2.Zero, SpriteEffects.None, .1f);
            batch.DrawString(Assets.font, text, new Vector2(bounds.X, bounds.Y), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public void setText(string text)
        {
            this.text = text;
            this.bounds = new Rectangle((int)startPos.X - (int)(Assets.font.MeasureString(text).Length() / 2 * scale), 
                (int)startPos.Y, (int)(Assets.font.MeasureString(text).Length() * scale), (int)Assets.font.MeasureString(text).Y);
            this.backgrounds = new Rectangle(bounds.X - 20, bounds.Y - 10, bounds.Width + 30, bounds.Height + 30);
        }
    }
}
