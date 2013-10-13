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
    class AnimatedSprite : Sprite
    {
        //Animation Data
        public int mCurrentCel = 0;
        public int mNumberOfCels = 0;

        public int msUntilNextCel = 0;
        public int msPerCel = 0;

        public AnimatedSprite()
        {

        }

        public AnimatedSprite(Texture2D aTexture, Rectangle currentCelLocation, Vector2 position, int numCels, int perCel)
            : base(aTexture, currentCelLocation, position)
        {
            mNumberOfCels = numCels;
            msPerCel = perCel;
        }

        public override void update(GameTime gameTime)
        {
            msUntilNextCel -= gameTime.ElapsedGameTime.Milliseconds;

            if (msUntilNextCel <= 0)
            {
                mCurrentCel++;
                msUntilNextCel = msPerCel;
            }

            if (mCurrentCel >= mNumberOfCels)
            {
                mCurrentCel = 0;
            }

            mCurrentCelLocation.X = mCurrentCelLocation.Width * mCurrentCel;
        }
    }
}
