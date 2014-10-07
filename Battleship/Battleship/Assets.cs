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
            loadRegion("tile", 200, 150, 50, 50);
            loadRegion("tileSelect", 200, 200, 50, 50);
            loadRegion("tileWater", 200, 100, 50, 50);
            loadRegion("tileHit", 150, 100, 50, 50);

            // effects
            loadRegion("tileBomb", 150, 200, 50, 50);
            
            // ship regions
            loadRegion("ship5", 0, 0, 250, 50);
            loadRegion("ship4", 0, 50, 200, 50);
            loadRegion("ship3", 0, 100, 150, 50);
            loadRegion("ship2", 0, 200, 100, 50);

            loadRegion("pixel", 249, 249, 1, 1);

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
