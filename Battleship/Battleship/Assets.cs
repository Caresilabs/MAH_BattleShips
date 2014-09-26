using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Battleship
{
    /**
     * Assets loads all the needed assets and a non cumbersome way to retrieve them
     */
    public class Assets
    {
        private static Dictionary<String, Rectangle> regions;
        private static ContentManager manager;
        private static Texture2D items;

        public static SpriteFont font;

        public static void load(ContentManager manager) {
            Assets.manager = manager;
            regions = new Dictionary<string, Rectangle>();

            // load our sprite sheet
            items = manager.Load<Texture2D>("Graphics/items");
            
            // Load our assets regions
            loadRegion("tile", 0, 0, 8, 8);
            loadRegion("ship", 8, 0, 8, 8);

            // Load font 
            font = manager.Load<SpriteFont>("Font/font");
        }

        private static void loadRegion(string name, int x, int y, int width, int height) {
            regions.Add(name, new Rectangle(x, y, width, height));
        }

        public static Rectangle getRegion(string name) {
            return regions[name];
        }

        public static Texture2D getItems()
        {
            return items;
        }

        public static void unload()
        {
            manager.Dispose();
        }
    }
}
