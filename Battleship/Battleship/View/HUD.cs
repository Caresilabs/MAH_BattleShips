using Battleship.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.View
{
    public class HUD
    {
        private static string dialogText;
        private static float textTime;

        private World world;
        
        public HUD(World world)
        {
            this.world = world;
        }

        public void update(float delta)
        {
            textTime += delta;

            if (textTime > 4)
            {
                dialogText = "";
            }
        }

        public void draw(SpriteBatch batch)
        {
            batch.DrawString(Assets.font, "Protip:\nUse keys 1-4 to change special attacks", Vector2.Zero, Color.White);

            switch (world.getState())
            {
                case World.State.Player1Init:
                    drawCenterString(batch, "Player 1 turn: Place Ships", 30, 1.3f);
                    drawCenterString(batch, "Press Space when finish", 70);
                    break;
                case World.State.Player2Init:
                    drawCenterString(batch, "Player 2 turn: Place Ships", 30, 1.3f);
                    if (world.getMode() == World.Mode.PlayerVSPlayer)
                    {
                        drawCenterString(batch, "Press Space when finish", 70);
                    }
                    break;
                case World.State.Player1Turn:
                    drawCenterString(batch, "Player 1 turn", 30, 1.3f);
                    drawCenterString(batch, "Press Space to Toggle ships visible", 70);
                    break;
                case World.State.Player2Turn:
                    drawCenterString(batch, "Player 2 turn", 30, 1.3f);
                    if (world.getMode() == World.Mode.PlayerVSPlayer)
                    {
                        drawCenterString(batch, "Press Space to Toggle ships visible", 70);
                    }
                    break;
                case World.State.Player1Win:
                    drawCenterString(batch, "Press R for a rematch", 120, 1.0f);
                    drawCenterString(batch, "Player 1 won!", 70, 1.3f);
                    break;
                case World.State.Player2Win:
                    drawCenterString(batch, "Press R for a rematch", 120, 1.0f);
                    drawCenterString(batch, "Player 2 won!", 70, 1.3f);
                    break;
                default:
                    break;
            }

            if (dialogText != null)
                drawCenterString(batch, dialogText, 120, Color.Red);
        }

        public static void drawCenterString(SpriteBatch batch, string text, float y, Color color, float scale = 1)
        {
            batch.DrawString(Assets.font, text,
                    new Vector2(
                         batch.GraphicsDevice.Viewport.Width / 2 - ((Assets.font.MeasureString(text).Length() / 2) * scale), y),
                         color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public static void drawCenterString(SpriteBatch batch, string text, float y, float scale = 1)
        {
            drawCenterString(batch, text, y, Color.White, scale);
        }

        public static void setDialogText(string text)
        {
            dialogText = text;
            textTime = 0;
        }
    }
}
