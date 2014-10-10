using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Battleship
{
    /**
     * Assets loads all the needed assets and a non cumbersome way to retrieve them
     */
    public class Assets
    {
        public const bool SOUND = false;

        private static Dictionary<String, Rectangle> regions;
        private static ContentManager manager;
        private static Texture2D items;

        public static Texture2D ui;
        public static Texture2D bg1;
        public static Texture2D bg2;

        public static SpriteFont font;

        public static SoundEffect bombSound;
        public static SoundEffect missSound;
        public static Song music;

        public static void load(ContentManager manager) {
            Assets.manager = manager;
            regions = new Dictionary<string, Rectangle>();

            // load our sprite sheet
            items = manager.Load<Texture2D>("Graphics/items");
            ui = manager.Load<Texture2D>("Graphics/ui");
            bg1 = manager.Load<Texture2D>("Graphics/bg1");
            bg2 = manager.Load<Texture2D>("Graphics/bg2");
            
            // Load our assets regions
            loadRegion("tile", 200, 150, 50, 50);
            loadRegion("tileSelect", 200, 200, 50, 50);
            loadRegion("tileWater", 200, 50, 50, 50);
            loadRegion("tileHit", 150, 150, 50, 50);
            loadRegion("tileBomb", 150, 200, 50, 50);
            
            // ship regions
            loadRegion("ship5", 0, 0, 250, 50);
            loadRegion("ship4", 0, 50, 200, 50);
            loadRegion("ship3", 0, 100, 150, 50);
            loadRegion("ship2", 0, 200, 100, 50);

            // others
            loadRegion("pixel", 249, 249, 1, 1);

            // Load UI
            loadRegion("title", 0, 0, 512, 64);
            loadRegion("uiContainer1", 0, 64, 320, 128);
            loadRegion("button", 320, 64, 192, 64);

            // Load font 
            font = manager.Load<SpriteFont>("Font/font");

            // load sound
            if (SOUND)
            {
                bombSound = manager.Load<SoundEffect>("Audio/bomb");
                missSound = manager.Load<SoundEffect>("Audio/miss");

                music = manager.Load<Song>("Audio/music");
                MediaPlayer.Volume = .2f;
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(music);
            }
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
