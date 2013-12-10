using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Batgai.Entities
{
    public class ScoreCounter
    {
        private int mScore = 0;
        private int mMultiplier = 1;
        private int mMultiplierCounter = 0;

        public void addPoints(int points)
        {
            mScore += points * mMultiplier;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            string score = string.Format("Score: {0}", mScore);
            string multiplier = string.Format("Multiplier: x{0}", mMultiplier);

            spriteBatch.DrawString(spriteFont, score, new Vector2(33, 60), Color.Black);
            spriteBatch.DrawString(spriteFont, score, new Vector2(32, 59), Color.White);

            spriteBatch.DrawString(spriteFont, multiplier, new Vector2(33, 90), Color.Black);
            spriteBatch.DrawString(spriteFont, multiplier, new Vector2(32, 89), Color.White);

            update();
        }

        public void update()
        {
            if (mMultiplierCounter <= 0)
            {
                resetMultiplier();
                mMultiplierCounter = 0;
            }
            else
            {
                mMultiplierCounter--;
            }
        }

        public void iterateMultiplier()
        {
            mMultiplier++;
            mMultiplierCounter = 120;
        }

        public void resetMultiplier()
        {
            mMultiplier = 1;
        }
    }
}