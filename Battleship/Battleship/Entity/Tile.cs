using Battleship.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.Entity
{
    public class Tile
    {
        public const int TILE_EMPTY = 0;
        public const int TILE_HIT = -1;
        public const int TILE_WATER = -2;

        private TileEffect effect;
        private Rectangle region;

        private int id;
        private int x;
        private int y;

        public enum TileEffect
        {
            Selected, BombMark ,None
        }

        public Tile(int x, int y)
        {
            this.id = TILE_EMPTY;
            this.effect = TileEffect.None;
            this.x = x;
            this.y = y;
            this.region = Assets.getRegion("tile");
        }

        public void draw(SpriteBatch batch, Vector2 position)
        {
            Color color = Color.White;

            batch.Draw(Assets.getItems(), new Rectangle((int)(position.X + x * World.tileSize), (int)(position.Y + y * World.tileSize), World.tileSize, World.tileSize)
              , Assets.getRegion("tile"), color, 0, Vector2.Zero, SpriteEffects.None, .5f);

            // Draw action
            if (id == TILE_HIT || id == TILE_WATER)
            {
                batch.Draw(Assets.getItems(), new Rectangle((int)(position.X + x * World.tileSize), (int)(position.Y + y * World.tileSize), World.tileSize, World.tileSize)
              , region, color, 0, Vector2.Zero, SpriteEffects.None, .02f);
            }

            // Effects
            if (id >= 0)
            {
                if (effect == TileEffect.Selected)
                {
                    batch.Draw(Assets.getItems(), new Rectangle((int)(position.X + x * World.tileSize), (int)(position.Y + y * World.tileSize), World.tileSize, World.tileSize)
                    , Assets.getRegion("tileSelect"), Color.White, 0, Vector2.Zero, SpriteEffects.None, .2f);
                }

                if (effect == TileEffect.BombMark)
                {
                    batch.Draw(Assets.getItems(), new Rectangle((int)(position.X + x * World.tileSize), (int)(position.Y + y * World.tileSize), World.tileSize, World.tileSize)
                    , Assets.getRegion("tileBomb"), Color.White, 0, Vector2.Zero, SpriteEffects.None, .2f);
                }
            }
        }

        public bool hit()
        {
            if (id != TILE_EMPTY && id < 0) return false;

            if (id > 0)
            {
                setId(TILE_HIT);
            } else {
                setId(TILE_WATER);
            }
            return true;
        }

        public void setId(int id)
        {
            this.id = id;

            // Set proper region
            if (id == TILE_WATER)
            {
                region = Assets.getRegion("tileWater");
            }
            else if (id == TILE_HIT)
            {
                region = Assets.getRegion("tileHit");
            } 
        }

        public int getId()
        {
            return id;
        }

        public void setTileEffect(TileEffect effect)
        {
            this.effect = effect;
        }

        public TileEffect getTileEffect()
        {
            return effect;
        }

    }
}
