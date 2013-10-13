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
    public class FrameRateParticleCounter
    {
        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;
        int mParticleCount = 0;


        public void Update(GameTime gameTime, int particleCount)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }

            mParticleCount = particleCount;
        }


        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            frameCounter++;

            string fps = string.Format("fps: {0}", frameRate);
            string particles = string.Format("particles: {0}", mParticleCount);

            spriteBatch.DrawString(spriteFont, fps, new Vector2(33, 33), Color.Black);
            spriteBatch.DrawString(spriteFont, fps, new Vector2(32, 32), Color.White);

            spriteBatch.DrawString(spriteFont, particles, new Vector2(33, 100), Color.Black);
            spriteBatch.DrawString(spriteFont, particles, new Vector2(32, 99), Color.White);
        }
    }
}
